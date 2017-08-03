Imports App_Web.Class_DIG_FACT_SQL
Public Class CFGR_POST
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
    Protected Sub TextBox_POST_TextChanged(sender As Object, e As EventArgs) Handles TextBox_POST.TextChanged
        TextBox_IPMT_DOCU.Text = DIG_FACT_SQL_GET_PARA($"{TextBox_POST.Text}.eolane.com", "Imprimante document")
        TextBox_IPMT_ETQT.Text = DIG_FACT_SQL_GET_PARA($"{TextBox_POST.Text}.eolane.com", "Imprimante étiquette")
        TextBox_LCLS.Text = DIG_FACT_SQL_GET_PARA($"{TextBox_POST.Text}.eolane.com", "Localisation")
        TextBox_MOYE_ASSO.Text = DIG_FACT_SQL_GET_PARA($"{TextBox_POST.Text}.eolane.com","Moyen associé")
    End Sub
    Protected Sub TextBox_IPMT_DOCU_TextChanged(sender As Object, e As EventArgs) Handles TextBox_IPMT_DOCU.TextChanged
        DIG_FACT_SQL_SET_PARA($"{TextBox_POST.Text}.eolane.com", "Imprimante document", TextBox_IPMT_DOCU.Text)

    End Sub

    Protected Sub TextBox_IPMT_ETQT_TextChanged(sender As Object, e As EventArgs) Handles TextBox_IPMT_ETQT.TextChanged
        DIG_FACT_SQL_SET_PARA($"{TextBox_POST.Text}.eolane.com", "Imprimante étiquette", TextBox_IPMT_ETQT.Text)

    End Sub

    Protected Sub TextBox_LCLS_TextChanged(sender As Object, e As EventArgs) Handles TextBox_LCLS.TextChanged
        DIG_FACT_SQL_SET_PARA($"{TextBox_POST.Text}.eolane.com", "Localisation", TextBox_LCLS.Text)

    End Sub

    Protected Sub TextBox_MOYE_ASSO_TextChanged(sender As Object, e As EventArgs) Handles TextBox_MOYE_ASSO.TextChanged
        DIG_FACT_SQL_SET_PARA($"{TextBox_POST.Text}.eolane.com", "Moyen associé", TextBox_MOYE_ASSO.Text)
    End Sub
End Class