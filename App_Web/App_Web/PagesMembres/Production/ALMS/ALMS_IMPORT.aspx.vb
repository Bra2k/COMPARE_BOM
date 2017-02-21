Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports App_Web.Class_SQL
Imports App_Web.Class_COMM_APP_WEB

Public Class ALMS_IMPORT
    Inherits System.Web.UI.Page
    Dim sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=" & Replace(Replace(My.Computer.Name, "CEDB03", "MES_Digital_Factory_DEV"), "CEAPP03", "MES_Digital_Factory") & ";Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=7200;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"

    Protected Sub Button_ENVOYER_Click(sender As Object, e As EventArgs) Handles Button_ENVOYER.Click

        Dim savePath As String = "c:\sources\temp_App_Web\"
        Dim dt As DataTable

        Try
            If Not (FileUpload_ALMS.HasFile) Then Throw New Exception("pas de fichier sélectionné")
            savePath += Server.HtmlEncode(FileUpload_ALMS.FileName)
            FileUpload_ALMS.SaveAs(savePath)
            FileUpload_ALMS.Dispose()

            If Right(savePath, 3) = "csv" Then
                dt = COMM_APP_WEB_IMP_CSV_DT(savePath)
                GridView_CSV_ALMS.DataSource = dt
                GridView_CSV_ALMS.DataBind()
                MultiView_ALMS.SetActiveView(GridView_ALMS)
                'LOG_Msg(GetCurrentMethod, dt.Rows(0)(0).ToString())
                Dim store_date_query As String = ""
            Else
                Throw New Exception("Le fichier séléctionné n'a pas l'extension [.csv] attendue")
            End If
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try
    End Sub
End Class
