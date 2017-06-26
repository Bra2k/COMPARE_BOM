Imports App_Web.Class_SQL
Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports App_Web.Class_COMM_APP_WEB
Public Class IDCT_GLOB_LIGN
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("displayname") = "" Then
                Context.GetOwinContext().Authentication.SignOut(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie)
            Else
                If App_Web.Class_COMM_APP_WEB.COMM_APP_WEB_GET_DROI_PAGE(Replace(HttpContext.Current.Request.Url.AbsoluteUri, "http://" & LCase(My.Computer.Name) & "/PagesMembres/", "~/PagesMembres/") & ".aspx", Session("department"), Session("User_Name")) = False Then Response.Redirect("~/PagesMembres/RDRC_PAGE_MEMB.aspx")
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
        Try
            If TextBox_OF.Text = "" And TextBox_CD_ARTI_ECO.Text = "" Then Throw New Exception("Vous devez entrer soit un code article ou soit un OF")

        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
        End Try
    End Sub
End Class