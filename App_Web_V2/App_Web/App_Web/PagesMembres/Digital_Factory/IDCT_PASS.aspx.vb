Imports App_Web.Class_SQL
Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports App_Web.Class_COMM_APP_WEB
Public Class IDCT_PASS
    Inherits System.Web.UI.Page
    Dim sChaineConnexion As String = $"Data Source=cedb03,1433;Initial Catalog={Replace(Replace(My.Computer.Name, "CEDB03", "MES_Digital_Factory_DEV"), "CEAPP03", "MES_Digital_Factory")};Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=7200;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("displayname") = "" Then
                Context.GetOwinContext().Authentication.SignOut(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie)
            Else
                If App_Web.Class_COMM_APP_WEB.COMM_APP_WEB_GET_DROI_PAGE(HttpContext.Current.CurrentHandler.ToString, Session("department"), Session("User_Name")) = False Then Response.Redirect("~/PagesMembres/RDRC_PAGE_MEMB.aspx")
            End If
            Calendar_DEB.SelectedDate = Date.Today
            Label_DEB.Text = COMM_APP_WEB_CONV_FORM_DATE(Calendar_DEB.SelectedDate, "yyyy-MM-dd")
            Calendar_FIN.SelectedDate = Date.Today.AddDays(1)
            Label_FIN.Text = COMM_APP_WEB_CONV_FORM_DATE(Calendar_FIN.SelectedDate, "yyyy-MM-dd")
        End If

    End Sub

    Protected Sub Calendar_DEB_SelectionChanged(sender As Object, e As EventArgs) Handles Calendar_DEB.SelectionChanged
        Label_DEB.Text = COMM_APP_WEB_CONV_FORM_DATE(Calendar_DEB.SelectedDate, "yyyy-MM-dd")
    End Sub

    Protected Sub Calendar_FIN_SelectionChanged(sender As Object, e As EventArgs) Handles Calendar_FIN.SelectionChanged
        Label_FIN.Text = COMM_APP_WEB_CONV_FORM_DATE(Calendar_FIN.SelectedDate, "yyyy-MM-dd")
    End Sub

    Protected Sub Button_FILT_Click(sender As Object, e As EventArgs) Handles Button_FILT.Click
        Dim sQuery As String = "", dt As New DataTable
        Try
            Label_DEB.Text = COMM_APP_WEB_CONV_FORM_DATE(Calendar_DEB.SelectedDate, "yyyy-MM-dd")
            Label_FIN.Text = COMM_APP_WEB_CONV_FORM_DATE(Calendar_FIN.SelectedDate, "yyyy-MM-dd")
            sQuery = $"SELECT * FROM [dbo].[F_IDCT_PASS]('{Label_DEB.Text}', '{Label_FIN.Text}', '{TextBox_OF.Text}', '{TextBox_CD_ARTI_ECO.Text}') ORDER BY [OF] DESC"
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            If dt Is Nothing Then Throw New Exception($"pas de données entre le {Label_DEB.Text} et le {Label_FIN.Text}.")
            GridView_RES.DataSource = dt
            GridView_RES.DataBind()
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try

    End Sub
End Class