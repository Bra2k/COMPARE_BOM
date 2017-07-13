Imports App_Web.Class_SAP_DATA
Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports App_Web.Class_SQL
Imports App_Web.Class_DIG_FACT
Imports App_Web.Class_DIG_FACT_SQL
Imports App_Web.Class_COMM_APP_WEB
Public Class IPSO_ETQT_NU_SER
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub TextBox_OF_TextChanged(sender As Object, e As EventArgs) Handles TextBox_OF.TextChanged
        Dim sQuery As String = ""
        Try
            'extraction données de l'OF
            Using dtAFKO = SAP_DATA_READ_AFKO($"AUFNR LIKE '%{TextBox_OF.Text}'")
                If dtAFKO Is Nothing Then Throw New Exception($"L'OF n°{TextBox_OF.Text} n'a pas été trouvé dans SAP.")
                sQuery = $"SELECT CONVERT(INTEGER,[AUFNR]) AS AUFNR
                             FROM [SAP].[dbo].[AFKO]
                            WHERE CONVERT(INTEGER,[AUFNR]) = {TextBox_OF.Text}"
                Using dt = SQL_SELE_TO_DT(sQuery, CS_ALMS_PROD_PRD)
                    If dt Is Nothing Then
                        sQuery = $"INSERT INTO [SAP].[dbo].[AFKO]
                                               ([RSNUM]
                                               ,[GAMNG]
                                               ,[PLNBEZ]
                                               ,[AUFNR]
                                               ,[AUFPL]
                                               ,[REVLV]
                                               ,[STLNR]
                                               ,[STLAL]
                                               ,[GSTRS])
                                         VALUES
                                               ('{dtAFKO(0)("RSNUM").ToString}'
                                               ,'{dtAFKO(0)("GAMNG").ToString}'
                                               ,'{dtAFKO(0)("PLNBEZ").ToString}'
                                               ,'{dtAFKO(0)("AUFNR").ToString}'
                                               ,'{dtAFKO(0)("AUFPL").ToString}'
                                               ,'{dtAFKO(0)("REVLV").ToString}'
                                               ,'{dtAFKO(0)("STLNR").ToString}'
                                               ,'{dtAFKO(0)("STLAL").ToString}'
                                               ,'{dtAFKO(0)("GSTRS").ToString}')"
                        SQL_REQ_ACT(sQuery, CS_MES_Digital_Factory)
                    End If
                End Using
            End Using
            Using dt = SAP_DATA_LECT_OF(TextBox_OF.Text)
                Label_CD_ARTI.Text = Trim(dt.Rows(0)("CD_ARTI_ECO").ToString)
                If Trim(dt.Rows(0)("NM_CLIE").ToString) <> "AIR LIQUIDE MEDICAL" Then Throw New Exception($"Le nom du client de l'OF {TextBox_OF.Text} n'est pas AIR LIQUIDE MEDICAL")
                Label_QT_OF.Text = Trim(dt.Rows(0)("QT_OF").ToString)
            End Using
            'todo vérifier si déjà imprimé
            sQuery = $"SELECT [NU_SER_CLIE]
                         FROM [dbo].[V_LIAIS_NU_SER]
                        WHERE NM_CRIT = '{Label_CD_ARTI.Text}' AND [NU_OF] = '{TextBox_OF.Text}'"
            Using dt = SQL_SELE_TO_DT(sQuery, CS_ALMS_PROD_PRD)
                If Not dt Is Nothing Then Throw New Exception($"L'OF {TextBox_OF.Text} a déjà été imprimé.")
            End Using

        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try
    End Sub

    Protected Sub Button_IPSO_Click(sender As Object, e As EventArgs) Handles Button_IPSO.Click
        Dim sFORM_NU_CLIE As String = "", sNS = ""
        Try
            Using dt_CFGR_ARTI_ECO = DIG_FACT_SQL_CFGR_ARTI_ECO(Trim(Label_CD_ARTI.Text))
                Using dt_ETAT_CTRL = COMM_APP_WEB_ETAT_CTRL($"{Trim(Label_CD_ARTI.Text)}|Numéro de série client", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx")
                    If dt_ETAT_CTRL Is Nothing Then Throw New Exception($"La base App_Web_Eco n'as pas été configurée pour l'article {Trim(Label_CD_ARTI.Text)}")
                    If dt_ETAT_CTRL.Columns.Contains("TextBox_FORM_NU_CLIE") = True Then sFORM_NU_CLIE = dt_ETAT_CTRL(0)("TextBox_FORM_NU_CLIE").ToString
                    For i = 1 To Convert.ToDecimal(Label_QT_OF.Text)
                        sNS = DIG_FACT_SQL_GENE_NU_SER_CLIE(dt_CFGR_ARTI_ECO(0)("Encodage Numéro de série client").ToString,
                                                            dt_CFGR_ARTI_ECO(0)("Incrémentation flanc").ToString,
                                                            sFORM_NU_CLIE,
                                                            Label_CD_ARTI.Text,
                                                            "Numéro de série client",
                                                            False,
                                                            Session("matricule"),
                                                            TextBox_OF.Text)
                        Dim sQuery = $"SELECT [ID_NU_SER]
	                                     FROM [dbo].[V_LIAIS_NU_SER]
	                                    WHERE [NU_SER_CLIE] = '{sNS}'
	                                      AND [NU_OF] = '{TextBox_OF.Text}'"
                        Using dt = SQL_SELE_TO_DT(sQuery, CS_MES_Digital_Factory)
                            If dt_ETAT_CTRL.Columns.Contains("TextBox_FICH_MODE") Then DIG_FACT_IMPR_ETIQ_V2(dt_ETAT_CTRL(0)("TextBox_FICH_MODE").ToString, "\\so2k8vm07\ceimp19", TextBox_OF.Text, "", "Numéro de série client", dt(0)("ID_NU_SER").ToString, "", "", "", Nothing)
                        End Using
                    Next
                End Using
            End Using
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try 'impression

    End Sub
End Class