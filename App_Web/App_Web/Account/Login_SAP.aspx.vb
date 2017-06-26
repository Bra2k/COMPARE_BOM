Imports App_Web.Class_SAP_DATA
Imports App_Web.LOG
Imports System.Reflection.MethodBase
Public Class Login_SAP
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub
    '''
    Protected Sub TextBox_LOG_SAP_TextChanged(sender As Object, e As EventArgs) Handles TextBox_LOG_SAP.TextChanged
        Dim dt_matr As New DataTable
        Try
            dt_matr = SAP_DATA_MATR(TextBox_LOG_SAP.Text)
            If dt_matr Is Nothing Then Throw New Exception("pas de nom/prénom trouvés pour le matricule " & TextBox_LOG_SAP.Text)
            Session("matricule") = TextBox_LOG_SAP.Text
            Session("sn") = LTrim(RTrim(dt_matr(0)("NACHN").ToString))
            Session("givenname") = LTrim(RTrim(dt_matr(0)("VORNA").ToString))
            Session("displayname") = Session("sn") & " " & Session("givenname")
            ''Request.ServerVariables("HTTP_REFERER")
            'LOG_Msg(GetCurrentMethod, Page.PreviousPage.Request.Url.ToString())
            Response.Redirect(Session("UrlReferrer"))
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try
    End Sub
End Class