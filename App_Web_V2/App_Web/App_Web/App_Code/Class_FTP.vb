Imports System.Net
Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports System.IO
Imports System
Public Class Class_FTP

    Public Shared Sub ftp()
        Try
            Dim request As FtpWebRequest = DirectCast(WebRequest.Create("D:\user\Desktop\yolo1.txt"), System.Net.FtpWebRequest)
            Dim files() As Byte = File.ReadAllBytes("ftp://ftp-armorod.alwaysdata.net/test/slt.txt")
            Dim strz As Stream = request.GetRequestStream() 'On créer un stream qui va nous permettre d'envoyer le fichier

            request.Credentials = New NetworkCredential("pseudo", "mdp") '
            request.Method = System.Net.WebRequestMethods.Ftp.DownloadFile
            strz.Write(files, 0, files.Length) 'On envoie le fichier

        Catch ex As Exception 'Une erreur c'est produite, on récupère l'erreur et on l'affiche.
            LOG_Erreur(GetCurrentMethod, ex.Message)
        Finally
            strz.Close() 'On ferme la connection
            strz.Dispose() 'On supprime la connection
        End Try
    End Sub
End Class
