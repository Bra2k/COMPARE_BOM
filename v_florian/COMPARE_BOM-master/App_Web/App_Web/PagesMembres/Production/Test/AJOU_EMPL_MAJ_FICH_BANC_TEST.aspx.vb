Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports App_Web.Class_SQL

Public Class AJOU_EMPL_MAJ_FICH_BANC_TEST
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Button_EMPL_FICH_TEST_Click(sender As Object, e As EventArgs) Handles Button_EMPL_FICH_TEST.Click


        Dim sQuery As String = "INSERT INTO [MES_Digital_Factory].[dbo].[DTM_REF_PARA] ([NM_CRIT],[NM_PARA],[VAL_PARA],[DT_PARA]) VALUES ('" & TextBox_NU_BANC.Text & "','Emplacement fichier test','" & TextBox_EMPL_FICH_TEST.Text & "', GETDATE())"
        Dim sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=APP_WEB_ECO;Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
        If Left(TextBox_EMPL_FICH_TEST.Text, 2) = "\\" And TextBox_NU_BANC.Text <> "" Then SQL_REQ_ACT(sQuery, sChaineConnexion)

        GridView_EMPL_FICH_TEST.DataBind()

    End Sub
End Class