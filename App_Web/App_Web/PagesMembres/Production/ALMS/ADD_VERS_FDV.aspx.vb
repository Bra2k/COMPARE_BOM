Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports App_Web.Class_SQL
Public Class ADD_VERS_FDV
    Inherits System.Web.UI.Page
    Dim sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=" & Replace(Replace(My.Computer.Name, "CEDB03", "ALMS_PROD_DEV"), "CEAPP03", "ALMS_PROD_PRD") & ";Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=7200;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub TextBox_CD_ARTI_ECO_TextChanged(sender As Object, e As EventArgs) Handles TextBox_CD_ARTI_ECO.TextChanged
        Dim sQuery As String = ""
        Dim dt As New DataTable

        Try
            TextBox_CD_ARTI_ECO.Enabled = False
            sQuery = "SELECT [NM_RFRC_FDV] AS [Nom de la FDV]
                            ,[NU_VERS_FDV] AS [Version de la FDV]
                            ,[NU_OF_APCT] AS [OF d'application]
                        FROM [dbo].[DTM_REF_FDV]
                       WHERE [CD_ARTI_PROD] = '" & TextBox_CD_ARTI_ECO.Text & "'
                      ORDER BY [NU_OF_APCT] DESC"
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            If Not dt Is Nothing Then Throw New Exception("¨Pas de FDV renseignée pour le code article " & TextBox_CD_ARTI_ECO.Text)
            GridView_LIST_FDV.DataSource = dt
            GridView_LIST_FDV.DataBind()
            TextBox_NM_RFRC_FDV.Enabled = True
            TextBox_NU_VERS_FDV.Enabled = True
            TextBox_NU_OF_ACPT.Enabled = True
            Button_VALI_ENTER.Enabled = True
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try
    End Sub

    Protected Sub GridView_LIST_FDV_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GridView_LIST_FDV.SelectedIndexChanged
        TextBox_NM_RFRC_FDV.Text = GridView_LIST_FDV.SelectedRow.Cells(1).Text
    End Sub

    Protected Sub Button_VALI_ENTER_Click(sender As Object, e As EventArgs) Handles Button_VALI_ENTER.Click
        Dim sQuery As String = ""
        Try
            If TextBox_NM_RFRC_FDV.Text = "" Then Throw New Exception("Entrer un nom de FDV")
            If TextBox_NU_VERS_FDV.Text = "" Then Throw New Exception("Entrer la version de la FDV")
            If TextBox_NU_OF_ACPT.Text = "" Then Throw New Exception("Entrer un OF d'application")
            sQuery = "INSERT INTO [dbo].[DTM_REF_FDV] ([CD_ARTI_PROD],[NM_RFRC_FDV],[NU_VERS_FDV],[NU_OF_APCT])
                           VALUES ('" & TextBox_CD_ARTI_ECO.Text & "', '" & TextBox_NM_RFRC_FDV.Text & "', '" & TextBox_NU_VERS_FDV.Text & "', '" & TextBox_NU_OF_ACPT.Text & "')"
            SQL_REQ_ACT(sQuery, sChaineConnexion)
            TextBox_NM_RFRC_FDV.Enabled = False
            TextBox_NM_RFRC_FDV.Text = ""
            TextBox_NU_VERS_FDV.Enabled = False
            TextBox_NU_VERS_FDV.Text = ""
            TextBox_NU_OF_ACPT.Enabled = False
            TextBox_NU_OF_ACPT.Text = ""
            Button_VALI_ENTER.Enabled = False
            TextBox_CD_ARTI_ECO.Text = ""
            TextBox_CD_ARTI_ECO.Enabled = True
            LOG_Msg(GetCurrentMethod, "Les données sont enregistrées")
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try
    End Sub
End Class