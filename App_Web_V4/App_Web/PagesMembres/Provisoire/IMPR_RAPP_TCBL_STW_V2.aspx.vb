Imports App_Web.Class_SQL
Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports System.IO
Imports App_Web.Class_DIG_FACT


Public Class IMPR_RAPP_TCBL_STW_V2
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Button_GENE_RAPP_Click(sender As Object, e As EventArgs) Handles Button_GENE_RAPP.Click
        Dim sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=MES_Digital_Factory;Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
        Dim sQuerySql As String = "SELECT '' AS A, CASE [NM_NS_SENS] WHEN '' THEN  [ID_CPT] ELSE [NM_NS_SENS] END AS NM_NS_SENS
                                     FROM [MES_Digital_Factory].[dbo].[ID_PF_View]
                                    WHERE [NM_NS_EOL] = '" & TextBox_NS_ENS.Text & "' AND NOT CASE [NM_NS_SENS] WHEN '' THEN [ID_CPT] ELSE [NM_NS_SENS] END IS NULL
                                 ORDER BY [NM_SAP_CPT]"
        Dim sFichier_PDF As String
        Try
            Dim dtNU_SSENS As System.Data.DataTable = SQL_SELE_TO_DT(sQuerySql, sChaineConnexion)
            If dtNU_SSENS Is Nothing Then Throw New Exception("Pas de résultat dans la base de données")

            sFichier_PDF = DIG_FACT_IMPR_PDF("C:\sources\Digital Factory\Etiquettes\STAGO\950271_Rapport.pdf", "1156566", "0", "Carton",
                                         "", TextBox_NS_ENS.Text, "", "",
                                         "", "", dtNU_SSENS,
                                        Nothing, 0, "C:\sources\App_Web\PagesMembres\Provisoire")
            ClientScript.RegisterStartupScript([GetType](), "printPdf", "document.getElementById(""pdf"").src = """ & Path.GetFileName(sFichier_PDF) & """;
                                                                                     document.getElementById(""pdf"").onload = function() {window.frames[""pdf""].focus();
                                                                                                                                           window.frames[""pdf""].print();};", True)
            Label_FICH_SORT.Text = sFichier_PDF
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try
    End Sub


End Class