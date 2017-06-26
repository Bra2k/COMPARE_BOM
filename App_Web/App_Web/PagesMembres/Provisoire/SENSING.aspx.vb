Imports App_Web.Class_SAP_DATA
Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports App_Web.Class_SQL
Imports System
Imports System.IO

Public Class WebForm1
    Inherits System.Web.UI.Page
    Dim sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=" & Replace(Replace(My.Computer.Name, "CEDB03", "SENSINGLABS_DEV"), "CEAPP03", "SENSINGLABS_PRD") & ";Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=7200;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"

    Protected Sub TextBox_OF_TextChanged(sender As Object, e As EventArgs) Handles TextBox_NU_OF.TextChanged
        Dim dtAFKO, dtOP As New DataTable

        dtAFKO = SAP_DATA_READ_AFKO("AUFNR LIKE '%" & TextBox_NU_OF.Text & "'")
        If dtAFKO Is Nothing Then Throw New Exception("L'OF " & TextBox_NU_OF.Text & " n'a pas été trouvé dans SAP.")
        Label_NU_ARTI.Text = Trim(dtAFKO(0)("PLNBEZ").ToString())

        Dim ope_query = "SELECT DISTINCT [NU_ETP], [LB_ETP]
                         FROM [dbo].[DWH_REF_ETP_PRCS] 
                         WHERE [CD_ARTI] = '" & Label_NU_ARTI.Text & "' 
                         AND [LB_ETP] <> 'Colisage'
                         AND [LB_ETP] <> 'Carte_Nue' 
                         AND [LB_ETP] <> 'Integration'
                         ORDER BY [NU_ETP] ASC"
        Try
            dtOP = SQL_SELE_TO_DT(ope_query, sChaineConnexion)

            DropDownList_OP.DataSource = dtOP
            DropDownList_OP.DataTextField = "LB_ETP"
            DropDownList_OP.DataValueField = "NU_ETP"
            DropDownList_OP.DataBind()
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try
    End Sub

    Protected Sub TextBox_NU_SER_TextChanged(sender As Object, e As EventArgs) Handles TextBox_NU_SER.TextChanged

        'If CHECK_PREV_OP() = 0 Then

        If FIND_CD_ARTI_IN_DTM(TextBox_NU_SER.Text) = Trim(Label_NU_ARTI.Text) Then

            MultiView_Sensing.SetActiveView(View_SAVE_TO_DB)

            Label_OF.Text = "" & TextBox_NU_OF.Text & ""
            Label_OP.Text = "" & DropDownList_OP.SelectedValue & ""
            Label_ART.Text = "" & FIND_CD_ARTI_IN_DTM(TextBox_NU_SER.Text) & ""
            Label_NU_SER.Text = "" & FIND_NU_SER_IN_DTM(TextBox_NU_SER.Text) & ""
            Label_DEVEUI.Text = "" & FIND_DEVEUI_IN_DTM(TextBox_NU_SER.Text) & ""
        Else
            Label_CHECK_WF.Text = "L'article scanné diffère du code article récupéré."
        End If

        'ElseIf CHECK_PREV_OP() = 1 Then
        'Label_CHECK_WF.Text = "L'opération précédente est un échec, enregistrement impossible"
        'End If
    End Sub

    Public Function CHECK_PREV_OP() As Integer
        Dim dtEtat, dtOP As New DataTable
        Dim op_cpt As Integer = 0
        Dim Tab_op(2) As Double
        Dim sQuery As String = ""

        'Request to get operations
        Dim opQuery = "SELECT DISTINCT [NU_ETP]
                        FROM [dbo].[DWH_REF_ETP_PRCS]
                        WHERE [CD_ARTI] = '" & Label_NU_ARTI.Text & "'
                        And [LB_ETP] <> 'Colisage'
                        And [LB_ETP] <> 'Carte_Nue' 
                        ORDER BY [NU_ETP] ASC"

        dtOP = SQL_SELE_TO_DT(opQuery, sChaineConnexion)

        op_cpt = dtOP.Rows.Count

        For i = 0 To op_cpt - 1
            Tab_op(i) = Convert.ToDecimal(dtOP(i)(0))
        Next

        'Return 0 si OK || 1 si NOK
        If Convert.ToDecimal(DropDownList_OP.SelectedValue) = Tab_op(0) Then
            Return 0
        ElseIf Convert.ToDecimal(DropDownList_OP.SelectedValue) = Tab_op(1) Then
            'vérifier que Tab_op(0) est succes
            sQuery = "SELECT TOP 1 [BL_STA]
                        FROM [dbo].[DWH_DIM_WRKF]
                        WHERE [NU_OF] = '" & TextBox_NU_OF.Text & "'
                        AND NU_ETP = '" & Tab_op(0) & "'
                        ORDER BY [ID_PSG] DESC"
            dtEtat = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            If dtEtat(0)(0).ToString = "True" Then
                Return 0
            Else
                Return 1
            End If

        ElseIf Convert.ToDecimal(DropDownList_OP.SelectedValue) = Tab_op(2) Then
            'vérifier que Tab_op(1) est succes
            sQuery = "SELECT TOP 1 [BL_STA]
                        FROM [dbo].[DWH_DIM_WRKF]
                        WHERE [NU_OF] = '" & TextBox_NU_OF.Text & "'
                        AND NU_ETP = '" & Tab_op(1) & "'
                        ORDER BY [ID_PSG] DESC"
            dtEtat = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            If dtEtat(0)(0).ToString = "True" Then
                Return 0
            Else
                Return 1
            End If
        End If
        Return 1
    End Function

    Public Function FIND_DEVEUI_IN_DTM(Text As String) As String
        Dim DEVEUI As String = Text.Substring(21, 16)
        Return Trim(DEVEUI)
    End Function

    Public Function FIND_CD_ARTI_IN_DTM(Text As String) As String
        Dim CD_ARTI As String = Text.Substring(38, 7)
        Dim MODIFIED As String = CD_ARTI.Insert(0, "SENE")
        Dim FINAL As String = MODIFIED.Insert(11, "$")

        Return Trim(FINAL)
    End Function

    Public Function FIND_NU_SER_IN_DTM(Text As String) As String
        Dim NU_SER As String = Text.Substring(49)
        Return Trim(NU_SER)
    End Function

    Public Function FIND_NU_OF_IN_DTM(Text As String) As String
        Dim NU_INC As String = Text.Substring(49)
        Dim NU_OF As String = NU_INC.Substring(0, 7)

        Return Trim(NU_OF)
    End Function

    Protected Sub Button_OK_Click(sender As Object, e As EventArgs) Handles Button_OK.Click
        Dim sQuery As String = "INSERT INTO [dbo].[DWH_DIM_WRKF] 
                                    ([DT_PSG], [NU_OF], [CD_ARTI], [NU_NS], [BL_STA], [NU_ETP])
                                VALUES(GETDATE(), '" & TextBox_NU_OF.Text & "', '" & Label_NU_ARTI.Text & "', '" & FIND_NU_SER_IN_DTM(TextBox_NU_SER.Text) & "', 1, " & DropDownList_OP.SelectedValue & ")"
        Try
            SQL_REQ_ACT(sQuery, sChaineConnexion)
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try
        TextBox_NU_SER.Text = ""
        Label_CHECK_WF.Text = ""
        MultiView_Sensing.SetActiveView(View_DATA_ENTR)
        TextBox_NU_SER.Focus()
    End Sub

    Protected Sub Button_NON_Click(sender As Object, e As EventArgs) Handles Button_NON.Click
        Dim sQuery As String = "INSERT INTO [dbo].[DWH_DIM_WRKF] 
                                    ([DT_PSG], [NU_OF], [CD_ARTI], [NU_NS], [BL_STA], [NU_ETP])
                                VALUES(GETDATE(), '" & TextBox_NU_OF.Text & "', '" & Label_NU_ARTI.Text & "', '" & FIND_NU_SER_IN_DTM(TextBox_NU_SER.Text) & "', 0, " & DropDownList_OP.SelectedValue & ")"
        Try
            SQL_REQ_ACT(sQuery, sChaineConnexion)
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try
        TextBox_NU_SER.Text = ""
        Label_CHECK_WF.Text = ""
        MultiView_Sensing.SetActiveView(View_DATA_ENTR)
        TextBox_NU_SER.Focus()
    End Sub
End Class