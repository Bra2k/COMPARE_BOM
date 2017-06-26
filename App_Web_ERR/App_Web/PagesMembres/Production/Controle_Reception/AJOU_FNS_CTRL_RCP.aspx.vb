Public Class AJOU_FNS_CTRL_RCP
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        My.Computer.FileSystem.CopyFile(
    "\\so2k3vm02\Application\Production\Tracabilite\Digital_Factory\Colisage\Etiquettes\MEDRIA_TECHNOLOGIES\MEDEM2IAX_CLIENT.prn",
    "C:\sources\test.prn", overwrite:=True)
    End Sub

End Class