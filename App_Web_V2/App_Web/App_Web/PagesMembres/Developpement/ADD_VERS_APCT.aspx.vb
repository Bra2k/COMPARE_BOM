Imports App_Web.Class_SQL
Public Class ADD_VERS_APCT
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

    Protected Sub Button_VALI_Click(sender As Object, e As EventArgs) Handles Button_VALI.Click

        Dim sQuery As String = "SELECT     LEFT(DER_VERS, CHARINDEX('.', DER_VERS) - 1) AS EVOL_MAJE, RIGHT(DER_VERS, LEN(DER_VERS) - CHARINDEX('.', DER_VERS)) AS EVOL_MINE
                                  FROM         (SELECT     MAX(NU_VERS) AS DER_VERS
                                                  FROM          dbo.DTM_GEST_VERS_PROG
                                                GROUP BY NM_APP
                                                HAVING      (NM_APP = N'" & DropDownList_PROG.SelectedValue & "')) AS DT_VERS"
        Dim sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=APP_WEB_ECO;Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
        Dim dt_VERS As DataTable = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
        Dim sVersion As String = ""
        Select Case DropDownList_NIVE.SelectedValue
            Case "VAL_MINE"
                sVersion = dt_VERS(0)("EVOL_MAJE").ToString & "." & Convert.ToString(Convert.ToDecimal(dt_VERS(0)("EVOL_MINE").ToString) + 1)
            Case "VAL_MAJE"
                sVersion = Convert.ToString(Convert.ToDecimal(dt_VERS(0)("EVOL_MAJE").ToString) + 1) & "." & dt_VERS(0)("EVOL_MINE").ToString
        End Select

        sQuery = "INSERT INTO APP_WEB_ECO.[dbo].[DTM_GEST_VERS_PROG]
                               ([NM_APP]
                               ,[NM_PAGE]
                               ,[NU_VERS]
                               ,[DT_VERS]
                               ,[TX_DESC_VERS])
                         VALUES
                               ('" & DropDownList_PROG.SelectedValue & "'
                               ,'" & TextBox_PAGE.Text & "'
                               ,'" & sVersion & "'
                               ,GETDATE()
                               ,'" & Replace(TextBox_CONT.Text, "'", "''") & "')"
        SQL_REQ_ACT(sQuery, sChaineConnexion)
    End Sub
End Class