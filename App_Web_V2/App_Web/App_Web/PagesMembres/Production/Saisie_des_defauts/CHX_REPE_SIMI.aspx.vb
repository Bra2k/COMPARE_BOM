Imports App_Web.Class_SAP_DATA
Imports App_Web.Class_COMM_APP_WEB
Public Class CHX_REPE_SIMI
    Inherits System.Web.UI.Page

    Protected Sub Button_Valider_Click(sender As Object, e As EventArgs) Handles Button_Valider.Click

        Label_résultat.Text = ""
        Dim dtAFKO As DataTable = SAP_DATA_READ_AFKO("AUFNR EQ '00000" & TextBox_OF.Text & "'")
        Dim dtVRESB As DataTable = SAP_DATA_READ_VRESB("RSNUM EQ '" & dtAFKO(0)("RSNUM").ToString & "' AND SPRAS EQ 'F'")
        Dim dtSTPU As DataTable = SAP_DATA_READ_STPU("STLNR EQ '" & dtVRESB(0)("STLNR") & "'")

        Dim sRepère As String = COMM_APP_WEB_CLEA_REPE(TextBox_Repère.Text.ToUpper)

        For Each rSTPU As DataRow In dtSTPU.Rows
            If COMM_APP_WEB_CLEA_REPE(rSTPU("EBORT").ToString) = sRepère Then
                Label_résultat.Text = rSTPU("EBORT").ToString
                Exit For
            Else
                Label_résultat.Text = "Non-trouvé"
            End If
        Next
        GridView1.DataSource = dtSTPU
        GridView1.DataBind()

    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("displayname") = "" Then
                Context.GetOwinContext().Authentication.SignOut(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie)
            Else
                If App_Web.Class_COMM_APP_WEB.COMM_APP_WEB_GET_DROI_PAGE(HttpContext.Current.CurrentHandler.ToString, Session("department"), Session("User_Name")) = False Then Response.Redirect("~/PagesMembres/RDRC_PAGE_MEMB.aspx")
            End If
        End If
    End Sub
End Class