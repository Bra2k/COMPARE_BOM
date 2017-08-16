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
        If Not IsPostBack Then
            If Session("displayname") = "" Then
                Context.GetOwinContext().Authentication.SignOut(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie)
            Else
                If App_Web.Class_COMM_APP_WEB.COMM_APP_WEB_GET_DROI_PAGE(HttpContext.Current.CurrentHandler.ToString, Session("department"), Session("User_Name")) = False Then Response.Redirect("~/PagesMembres/RDRC_PAGE_MEMB.aspx")
            End If
        End If
    End Sub

    Protected Sub TextBox_OF_TextChanged(sender As Object, e As EventArgs) Handles TextBox_OF.TextChanged
        Dim sQuery As String = ""
        Try
            'todo test pour les produits n'ayant pas d'OF créé
            Select Case TextBox_OF.Text
                Case "9000001"
                    Label_CD_ARTI.Text = "TAED966200$"
                    Label_QT_OF.Text = "10"
                Case "9000002"
                    Label_CD_ARTI.Text = "TAED979300$"
                    Label_QT_OF.Text = "10"
                Case "9000003"
                    Label_CD_ARTI.Text = "TAED979200$"
                    Label_QT_OF.Text = "10"
                Case "9000004"
                    Label_CD_ARTI.Text = "TAED966100$"
                    Label_QT_OF.Text = "10"
                Case "9000005"
                    Label_CD_ARTI.Text = "TAED966300$"
                    Label_QT_OF.Text = "10"
                Case "9000006"
                    Label_CD_ARTI.Text = "TAED977800$"
                    Label_QT_OF.Text = "10"
                Case "9000007"
                    Label_CD_ARTI.Text = "TAED977700$"
                    Label_QT_OF.Text = "10"
                Case "9000008"
                    Label_CD_ARTI.Text = "TAED980400$"
                    Label_QT_OF.Text = "10"
                Case "9000009"
                    Label_CD_ARTI.Text = "TAED980500$"
                    Label_QT_OF.Text = "10"
                Case Else
                    'extraction données de l'OF
                    Using dtAFKO = SAP_DATA_READ_AFKO($"AUFNR Like '%{TextBox_OF.Text}'")
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
                    Using dt = SQL_SELE_TO_DT(sQuery, CS_MES_Digital_Factory)
                        If Not dt Is Nothing Then Throw New Exception($"L'OF {TextBox_OF.Text} a déjà été imprimé.")
                    End Using
            End Select
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "alert")
            Label_CD_ARTI.Text = ""
            TextBox_OF.Text = ""
            Label_QT_OF.Text = ""
            Exit Sub
        End Try
    End Sub

    Protected Sub Button_IPSO_Click(sender As Object, e As EventArgs) Handles Button_IPSO.Click
        Dim sFORM_NU_CLIE As String = "", sNS = "", i_nb_etqt = 1
        Try
            'todo vérifier si déjà imprimé
            Dim sQuery = $"SELECT [NU_SER_CLIE]
                             FROM [dbo].[V_LIAIS_NU_SER]
                            WHERE NM_CRIT = '{Label_CD_ARTI.Text}' AND [NU_OF] = '{TextBox_OF.Text}'"
            Using dt = SQL_SELE_TO_DT(sQuery, CS_MES_Digital_Factory)
                If Not dt Is Nothing Then Throw New Exception($"L'OF {TextBox_OF.Text} a déjà été imprimé.")
            End Using
            Using dt_CFGR_ARTI_ECO = DIG_FACT_SQL_CFGR_ARTI_ECO(Trim(Label_CD_ARTI.Text))
                Using dt_ETAT_CTRL = COMM_APP_WEB_ETAT_CTRL($"{Trim(Label_CD_ARTI.Text)}|Numéro de série client", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx")
                    If dt_ETAT_CTRL Is Nothing Then Throw New Exception($"La base App_Web_Eco n'as pas été configurée pour l'article {Trim(Label_CD_ARTI.Text)}")
                    If dt_ETAT_CTRL.Columns.Contains("TextBox_FORM_NU_CLIE") = True Then sFORM_NU_CLIE = dt_ETAT_CTRL(0)("TextBox_FORM_NU_CLIE").ToString
                    Using dtvar As New DataTable
                        dtvar.Columns.Add("0", Type.GetType("System.String"))
                        dtvar.Columns.Add("1", Type.GetType("System.String"))
                        For i = 1 To Convert.ToDecimal(dt_CFGR_ARTI_ECO(0)("Nombre étiquette largeur").ToString)
                            dtvar.Rows.Add()
                        Next
                        For i = 1 To Convert.ToDecimal(Replace(Label_QT_OF.Text, ".", ","))
                            dtvar.Rows(i_nb_etqt - 1)("1") = DIG_FACT_SQL_GENE_NU_SER_CLIE(dt_CFGR_ARTI_ECO(0)("Encodage Numéro de série client").ToString,
                                                                dt_CFGR_ARTI_ECO(0)("Incrémentation flanc").ToString,
                                                                sFORM_NU_CLIE,
                                                                Trim(Label_CD_ARTI.Text),
                                                                "Numéro de série client",
                                                                False,
                                                                Session("matricule"),
                                                                TextBox_OF.Text)
                            If i_nb_etqt = Convert.ToDecimal(dt_CFGR_ARTI_ECO(0)("Nombre étiquette largeur").ToString) Then
                                DIG_FACT_IMPR_ETIQ_V2(dt_ETAT_CTRL(0)("TextBox_FICH_MODE").ToString, TextBox_IPMT.Text, TextBox_OF.Text, "", "Numéro de série client", "", "", "", "", dtvar)
                                For Each rdt In dtvar.Rows
                                    rdt("1") = ""
                                Next
                                i_nb_etqt = 1
                            Else
                                i_nb_etqt += 1
                            End If
                            If i = Convert.ToDecimal(Replace(Label_QT_OF.Text, ".", ",")) And i Mod Convert.ToDecimal(dt_CFGR_ARTI_ECO(0)("Nombre étiquette largeur").ToString) <> 0 Then DIG_FACT_IMPR_ETIQ_V2(dt_ETAT_CTRL(0)("TextBox_FICH_MODE").ToString, TextBox_IPMT.Text, TextBox_OF.Text, "", "Numéro de série client", "", "", "", "", dtvar)
                        Next
                    End Using
                End Using
            End Using
            LOG_MESS_UTLS(GetCurrentMethod, $"L'OF {TextBox_OF.Text} a été imprimé.", "success")
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "alert")
            Exit Sub
        End Try 'impression

    End Sub
End Class