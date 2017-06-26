Public Class JOUR_APP
    Inherits System.Web.UI.Page

    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    'End Sub

    Protected Sub GridView_JOUR_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView_JOUR.RowDataBound
        'Select Case e.Row.Cells(2).Text
        '    Case "Erreur"
        '        e.Row.ForeColor = System.Drawing.Color.Red

        'End Select
    End Sub

    Protected Sub GridView_JOUR_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GridView_JOUR.SelectedIndexChanged

    End Sub
End Class