Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports App_Web.Class_SQL
Imports App_Web.Class_SAP_DATA
Imports System.Windows.Forms
Public Event ItemCheck As ItemCheckEventHandler
Public Class CST_HEUR_SAI_BW
    Inherits System.Web.UI.Page

    Protected Sub TextBox_Matricule_TextChanged(sender As Object, e As EventArgs) Handles TextBox_Matricule.TextChanged

        Dim dtPA0002 As DataTable = SAP_DATA_READ_PA0002()
        CheckBoxList_LIST_OP.DataSource = dtPA0002
        CheckBoxList_LIST_OP.DataTextField = "NACHN" & " " & "VORNA"
        CheckBoxList_LIST_OP.DataValueField = "PERNR"
        CheckBoxList_LIST_OP.DataBind()

        Dim sQuery = "SELECT [CD_OPER]
                        FROM [APP_WEB_ECO].[dbo].[V_REF_DER_LIAI_CHEF_EQUI_OPER]
                       WHERE [CD_CHEF_EQUI] = '" & TextBox_Matricule.Text & "'"
        Dim sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=APP_WEB_ECO;Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"

        Dim dt As DataTable = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
        If dt.Rows.Count = 0 Then
            LOG_Erreur(GetCurrentMethod, "Vous n'êtes pas autorisé à utiliser ces fonctions")
            Exit Sub
        End If

        For Each rdt As DataRow In dt.Rows
            For Each li As ListItem In CheckBoxList_LIST_OP.Items
                If li.Value = rdt("CD_OPER".ToString) Then li.Selected = True
            Next
        Next

        Dim dtHEUR, dtZ_LOG_CONF_GET, dtZ_LOG_ACT_GET As New DataTable
        For Each li As ListItem In CheckBoxList_LIST_OP.Items
            If li.Selected = True Then
                'dtZ_LOG_CONF_GET = SAP_DATA_Z_LOG_CONF_GET(V_AUFNR As String, V_VORNR As String, li.Value, DATE_DEBUT As String, DATE_FIN As String)
                'dtHEUR.Merge(dtZ_LOG_CONF_GET)
                'dtZ_LOG_ACT_GET = SAP_DATA_Z_LOG_ACT_GET(li.Value, DATE_DEBUT As String, DATE_FIN As String)
                'dtHEUR.Merge(dtZ_LOG_ACT_GET)
            End If
        Next

    End Sub

    Private Sub CheckBoxList_LIST_OP_ItemCheck(sender As Object, e As ItemCheckEventArgs)

        Dim sQuery = "SELECT [CD_OPER]
                        FROM [APP_WEB_ECO].[dbo].[V_REF_DER_LIAI_CHEF_EQUI_OPER]
                       WHERE [CD_CHEF_EQUI] = '" & TextBox_Matricule.Text & "'"
        Dim sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=APP_WEB_ECO;Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
        SQL_REQ_ACT(sQuery, sChaineConnexion)
        Dim messageBoxVB As New System.Text.StringBuilder()
        messageBoxVB.AppendFormat("{0} = {1}", "Index", e.Index)
        messageBoxVB.AppendLine()
        messageBoxVB.AppendFormat("{0} = {1}", "NewValue", e.NewValue)
        messageBoxVB.AppendLine()
        messageBoxVB.AppendFormat("{0} = {1}", "CurrentValue", e.CurrentValue)
        messageBoxVB.AppendLine()
        MessageBox.Show(messageBoxVB.ToString(), "ItemCheck Event")

    End Sub
End Class