Imports App_Web.Class_SAP_DATA
Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports App_Web.Class_SQL
Imports System
Imports System.IO
Imports App_Web.Class_COMM_APP_WEB

Public Class WebForm1
    Inherits System.Web.UI.Page

    Protected Sub TextBox_OF_TextChanged(sender As Object, e As EventArgs) Handles TextBox_NU_OF.TextChanged
        Try
            Using dtAFKO = SAP_DATA_READ_AFKO($"AUFNR LIKE '%{TextBox_NU_OF.Text}'")
                If dtAFKO Is Nothing Then Throw New Exception($"L'OF {TextBox_NU_OF.Text} n'a pas été trouvé dans SAP.")
                'todo vérifier que le nom du client est bien SENSING LABS
                Label_NU_ARTI.Text = Trim(dtAFKO(0)("PLNBEZ").ToString)
            End Using
            'Using db As New SENSINGLABS_PRDEntities
            '    Dim ETP As DWH_REF_ETP_PRCS = db.DWH_REF_ETP_PRCS.Find(ID)
            'End Using
            Dim ope_query = $"SELECT DISTINCT [NU_ETP], [LB_ETP]
                                FROM [dbo].[DWH_REF_ETP_PRCS] 
                               WHERE [CD_ARTI] = '{Label_NU_ARTI.Text}' 
                                 AND [LB_ETP] <> 'Colisage'
                                 AND [LB_ETP] <> 'Carte_Nue' 
                                 AND [LB_ETP] <> 'Integration'
                              ORDER BY [NU_ETP] ASC"
            Using dtOP = SQL_SELE_TO_DT(ope_query, CS_SENSINGLABS_PRD)
                DropDownList_OP.DataSource = dtOP
                DropDownList_OP.DataTextField = "LB_ETP"
                DropDownList_OP.DataValueField = "NU_ETP"
                DropDownList_OP.DataBind()
            End Using
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
                Exit Sub
            End Try
    End Sub

    Protected Sub TextBox_NU_SER_TextChanged(sender As Object, e As EventArgs) Handles TextBox_NU_SER.TextChanged

        Try
            Dim split_ns = Split(TextBox_NU_SER.Text, " ")
            Label_OF.Text = TextBox_NU_OF.Text
            Label_OP.Text = DropDownList_OP.SelectedValue
            Label_ART.Text = $"SENE{COMM_APP_WEB_STRI_TRIM_RIGHT(Trim(split_ns(1)), 3)}$"
            If Label_ART.Text <> Label_NU_ARTI.Text Then Throw New Exception("L'article scanné diffère du code article récupéré.")
            Label_NU_SER.Text = Trim(split_ns(2))
            Label_DEVEUI.Text = Trim(split_ns(0))
            If CHECK_PREV_OP() = 0 Then
                Button_NON_Click(sender, e)
                Throw New Exception("Une des opérations précédentes est en échec, enregistrement impossible")
            End If
            MultiView_Sensing.SetActiveView(View_SAVE_TO_DB)
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try

    End Sub

    Public Function CHECK_PREV_OP() As Integer
        Try
            Dim sQuery = $"SELECT     DT_ETAP_VRFE.NU_ETP
                             FROM         (SELECT     NU_ETP
                                             FROM          dbo.DWH_REF_ETP_PRCS
                                            WHERE      (CD_ARTI = '{Label_ART.Text}') AND (NU_ETP < {Label_OP.Text })) AS DT_ETAP_VRFE LEFT OUTER JOIN
                                                  (SELECT     DWH_DIM_WRKF_1.DT_PSG, DWH_DIM_WRKF_1.NU_NS, DWH_DIM_WRKF_1.BL_STA, DWH_DIM_WRKF_1.NU_ETP
                                                     FROM          (SELECT     NU_NS, MAX(DT_PSG) AS DERN_DT_PSG, NU_ETP
                                                                      FROM          dbo.DWH_DIM_WRKF
                                                                    GROUP BY NU_NS, NU_ETP
                                                                     HAVING      (NU_NS = '{Label_NU_SER.Text}')) AS DT_DERN_PSG_1 INNER JOIN
                                                                   dbo.DWH_DIM_WRKF AS DWH_DIM_WRKF_1 ON DT_DERN_PSG_1.NU_NS = DWH_DIM_WRKF_1.NU_NS AND 
                                                                   DT_DERN_PSG_1.DERN_DT_PSG = DWH_DIM_WRKF_1.DT_PSG AND DT_DERN_PSG_1.NU_ETP = DWH_DIM_WRKF_1.NU_ETP) 
                                                      AS DT_DERN_PSG ON DT_ETAP_VRFE.NU_ETP = DT_DERN_PSG.NU_ETP
                              WHERE     (ISNULL(DT_DERN_PSG.BL_STA, 0) = 0)"
            Dim dtEtat = SQL_SELE_TO_DT(sQuery, CS_SENSINGLABS_PRD)
            If Not dtEtat Is Nothing Then
                Dim sb_etap As New StringBuilder
                For Each rdtetat In dtEtat.Rows
                    sb_etap.Append(rdtetat("NU_ETP").ToString)
                    sb_etap.Append(", ")
                Next
                Throw New Exception($"Le numéro de série {Label_NU_SER.Text} n'est pas passé ou est passé mauvais à l'étape {COMM_APP_WEB_STRI_TRIM_RIGHT(sb_etap.ToString, 2)}")
            End If
            Return 1

        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Return 0
        End Try

    End Function

    Protected Sub Button_OK_Click(sender As Object, e As EventArgs) Handles Button_OK.Click

        Try
            Using db As New SENSINGLABS_PRDEntities
                Dim wrkf As New DWH_DIM_WRKF
                wrkf.DT_PSG = Now
                wrkf.NU_OF = Label_OF.Text
                wrkf.CD_ARTI = Label_ART.Text
                wrkf.NU_NS = Label_NU_SER.Text
                wrkf.BL_STA = "1"
                wrkf.NU_ETP = Label_OP.Text
                db.DWH_DIM_WRKF.Add(wrkf)
                db.SaveChanges()
            End Using
            LOG_MESS_UTLS(GetCurrentMethod, $"Le numéro de série {Label_NU_SER.Text} est passé bon à l'étape {Label_OP.Text}")
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Exit Sub
        Finally
            TextBox_NU_SER.Text = ""
            Label_CHECK_WF.Text = ""
            MultiView_Sensing.SetActiveView(View_DATA_ENTR)
            TextBox_NU_SER.Focus()
        End Try

    End Sub

    Protected Sub Button_NON_Click(sender As Object, e As EventArgs) Handles Button_NON.Click

        Try
            Using db As New SENSINGLABS_PRDEntities
                Dim wrkf As New DWH_DIM_WRKF
                wrkf.DT_PSG = Now
                wrkf.NU_OF = Label_OF.Text
                wrkf.CD_ARTI = Label_ART.Text
                wrkf.NU_NS = Label_NU_SER.Text
                wrkf.BL_STA = "0"
                wrkf.NU_ETP = Label_OP.Text
                db.DWH_DIM_WRKF.Add(wrkf)
                db.SaveChanges()
            End Using
            LOG_MESS_UTLS(GetCurrentMethod, $"Le numéro de série {Label_NU_SER.Text} est passé mauvais à l'étape {Label_OP.Text}")
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Exit Sub
        Finally
            TextBox_NU_SER.Text = ""
            Label_CHECK_WF.Text = ""
            MultiView_Sensing.SetActiveView(View_DATA_ENTR)
            TextBox_NU_SER.Focus()
        End Try

    End Sub

    Protected Sub Button_RAZ_Click(sender As Object, e As EventArgs) Handles Button_RAZ.Click

    End Sub
End Class