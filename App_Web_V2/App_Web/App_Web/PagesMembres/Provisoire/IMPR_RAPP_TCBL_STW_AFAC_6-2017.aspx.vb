Imports App_Web.Class_WORD
Imports App_Web.Class_SQL
Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports System.IO
Imports Microsoft.Office.Interop.Word
Imports App_Web.Class_COMM_APP_WEB

Public Class IMPR_RAPP_TCBL_STW
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

    Protected Sub Button_GENE_RAPP_Click(sender As Object, e As EventArgs) Handles Button_GENE_RAPP.Click
        Dim sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=MES_Digital_Factory;Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
        Dim sQuerySql As String = "SELECT [NM_SAP_CPT],
                                          CASE [NM_NS_SENS] WHEN '' THEN  [ID_CPT] ELSE [NM_NS_SENS] END AS NM_NS_SENS
                                     FROM [MES_Digital_Factory].[dbo].[ID_PF_View]
                                    WHERE [NM_NS_EOL] = '" & TextBox_NS_ENS.Text & "' AND NOT CASE [NM_NS_SENS] WHEN '' THEN [ID_CPT] ELSE [NM_NS_SENS] END IS NULL"
        Try
            Randomize()
            Dim sfich As String = My.Settings.RPTR_TPRR & "\temp_rapport_traça_stw_" & CInt(Int((1000 * Rnd()) + 1)) & ".docx"
            If File.Exists(sfich) Then My.Computer.FileSystem.DeleteFile(sfich)
            Dim dtNU_SSENS As System.Data.DataTable = SQL_SELE_TO_DT(sQuerySql, sChaineConnexion)
            My.Computer.FileSystem.CopyFile("c:\sources\Digital Factory\Rapport\Traçabilité Ensemble Start Max.docx", sfich, overwrite:=True)
            Dim oWord As Application = WORD_OUVR()
            Dim objDoc As New Document
            objDoc = oWord.Documents.Open(sfich)
            objDoc = oWord.ActiveDocument

            objDoc.Content.Find.Execute(FindText:="@NS_ENS", ReplaceWith:=TextBox_NS_ENS.Text, Replace:=WdReplace.wdReplaceAll)
            Dim rdtNU_SENS As DataRow = dtNU_SSENS.Select("NM_SAP_CPT = 'STAGE950240$'").FirstOrDefault
            If Not rdtNU_SENS Is Nothing Then
                objDoc.Content.Find.Execute(FindText:="@NS_950240", ReplaceWith:=rdtNU_SENS("NM_NS_SENS").ToString, Replace:=WdReplace.wdReplaceAll)
            Else
                objDoc.Content.Find.Execute(FindText:="@NS_950240", ReplaceWith:="", Replace:=WdReplace.wdReplaceAll)
            End If
            rdtNU_SENS = dtNU_SSENS.Select("NM_SAP_CPT = 'STAGE950220$'").FirstOrDefault
            If Not rdtNU_SENS Is Nothing Then
                objDoc.Content.Find.Execute(FindText:="@NS_950220", ReplaceWith:=rdtNU_SENS("NM_NS_SENS").ToString, Replace:=WdReplace.wdReplaceAll)
            Else
                objDoc.Content.Find.Execute(FindText:="@NS_950220", ReplaceWith:="", Replace:=WdReplace.wdReplaceAll)
            End If
            rdtNU_SENS = dtNU_SSENS.Select("NM_SAP_CPT = 'STAGE950250$'").FirstOrDefault
            If Not rdtNU_SENS Is Nothing Then
                objDoc.Content.Find.Execute(FindText:="@NS_950250", ReplaceWith:=rdtNU_SENS("NM_NS_SENS").ToString, Replace:=WdReplace.wdReplaceAll)
            Else
                objDoc.Content.Find.Execute(FindText:="@NS_950250", ReplaceWith:="", Replace:=WdReplace.wdReplaceAll)
            End If
            rdtNU_SENS = dtNU_SSENS.Select("NM_SAP_CPT = 'STAGWP0091628$'").FirstOrDefault
            If Not rdtNU_SENS Is Nothing Then
                objDoc.Content.Find.Execute(FindText:="@NS_91628", ReplaceWith:=rdtNU_SENS("NM_NS_SENS").ToString, Replace:=WdReplace.wdReplaceAll)
            Else
                objDoc.Content.Find.Execute(FindText:="@NS_91628", ReplaceWith:="", Replace:=WdReplace.wdReplaceAll)
            End If
            rdtNU_SENS = dtNU_SSENS.Select("NM_SAP_CPT = 'STAGWP0091627$'").FirstOrDefault
            If Not rdtNU_SENS Is Nothing Then
                objDoc.Content.Find.Execute(FindText:="@NS_91627", ReplaceWith:=rdtNU_SENS("NM_NS_SENS").ToString, Replace:=WdReplace.wdReplaceAll)
            Else
                objDoc.Content.Find.Execute(FindText:="@NS_91627", ReplaceWith:="", Replace:=WdReplace.wdReplaceAll)
            End If
            objDoc.Save()
            oWord.ActivePrinter = TextBox_IMPR.Text
            objDoc.PrintOut()
            objDoc = COMM_APP_WEB_RELE_OBJ(objDoc)
            oWord.Quit(False)
            oWord = COMM_APP_WEB_RELE_OBJ(oWord)
        Catch ex As Exception
            For Each p As Process In Process.GetProcessesByName("WINWORD")
                p.Kill()
            Next
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try
    End Sub


End Class