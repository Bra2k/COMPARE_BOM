Imports App_Web.Class_SQL
Imports App_Web.LOG
Imports System.Reflection.MethodBase
Public Class CSTT_RESU_TEST_FONC
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Not IsPostBack Then
        '    If Session("displayname") = "" Then
        '        Context.GetOwinContext().Authentication.SignOut(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie)
        '    Else
        '        If App_Web.Class_COMM_APP_WEB.COMM_APP_WEB_GET_DROI_PAGE(Replace(HttpContext.Current.Request.Url.AbsoluteUri, "http://cedb03/PagesMembres/", "~/PagesMembres/") & ".aspx", Session("department"), Session("User_Name")) = False Then Response.Redirect("~/PagesMembres/RDRC_PAGE_MEMB.aspx")
        '    End If
        'End If
    End Sub

    Protected Sub TextBox_NU_SER_TextChanged(sender As Object, e As EventArgs) Handles TextBox_NU_SER.TextChanged
        Dim sQuery As String = "SELECT *
                                  FROM [MES_Digital_Factory].[dbo].[V_DTM_TEST_FONC_VAL]
                                 WHERE [NU_SER] = '" & TextBox_NU_SER.Text & "' AND [BL_SANC] = 'F'
                                ORDER BY DT_TEST DESC", sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=APP_WEB_ECO;Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
        Dim dt As New DataTable
        Try
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            GridView_RESU_FONC.DataSource = dt
            GridView_RESU_FONC.DataBind()
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try
    End Sub
End Class