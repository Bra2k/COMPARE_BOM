Imports Microsoft.Office.Interop.Word
Imports App_Web.LOG
Imports System.Data
Imports System.Reflection.MethodBase
Imports System.IO
Imports App_Web.Class_COMM_APP_WEB
Public Class Class_WORD

    Public Shared Function WORD_OUVR() As Application

        Dim iCountTimeout As Integer = 1
        Dim oW As Object

        Try
            While 1
                If Process.GetProcessesByName("WINWORD").Count > 10 Then
                    System.Threading.Thread.Sleep(1000)
                Else
                    Exit While
                End If
                If iCountTimeout = 1000 Then Throw New Exception("Les process Word ont été nettoyé sur le serveur après 1000 secondes d'inactivité")
                iCountTimeout += 1
            End While
        Catch ex As Exception
            For Each p As Process In Process.GetProcessesByName("WINWORD")
                p.Kill()
            Next
            LOG_Erreur(GetCurrentMethod, ex.Message)
        Finally
            oW = New Application
            oW.Visible = False
            oW.DisplayAlerts = False
        End Try
        'LOG_Msg(GetCurrentMethod, "L'objet word a été créé en " & iCountTimeout & " secondes.")

        Return oW

    End Function
    Sub WORD_PDF2TXT(sFichierPDF As String, sFichierTXT As String)
        'LOG_Msg(GetCurrentMethod, "")
        Dim objApp As Application = WORD_OUVR()
        Dim objDoc As Document = objApp.Documents.Open(sFichierPDF)
        'LOG_Msg(GetCurrentMethod, "ouvrir document")

        objDoc.Activate()
        objDoc.SaveAs(sFichierTXT, WdSaveFormat.wdFormatEncodedText)
        objDoc.Close()
        objApp.Quit()
        objDoc = COMM_APP_WEB_RELE_OBJ(objDoc)
        objApp = COMM_APP_WEB_RELE_OBJ(objApp)

    End Sub

    Public Shared Sub WORD_COMPR_PICS(sFichierWord As String)

        Dim oWord As Microsoft.Office.Interop.Word.Application = WORD_OUVR()
        Dim objDoc As New Microsoft.Office.Interop.Word.Document
        Try
            objDoc = oWord.Documents.Open(sFichierWord)
            oWord.Visible = True
            With objDoc.Application.CommandBars.FindControl(Id:=6382)
                .SendKeys("%(oe)~{TAB}~")

                .ExecuteMso("PicturesCompress")
                .Execute()
            End With
            'objDoc.SaveAs("c:\sources\coucou.docx", WdSaveFormat.wdFormatDocument)
            objDoc.Save()
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        Finally

            objDoc = COMM_APP_WEB_RELE_OBJ(objDoc)
            oWord.Quit(False)
            oWord = COMM_APP_WEB_RELE_OBJ(oWord)
        End Try
        LOG_Msg(GetCurrentMethod, $"La compression des images du fichier {sFichierWord} a été effectuée.")

    End Sub

    Public Shared Sub WORD_REMP_TEXT(sFichierWord As String, sTexteRecherche As String, sTexteRemplacement As String)
        Dim oWord As Application = WORD_OUVR()
        Dim objDoc As New Document
        Try
            objDoc = oWord.Documents.Open(sFichierWord)
            objDoc = oWord.ActiveDocument

            objDoc.Content.Find.Execute(FindText:=sTexteRecherche, ReplaceWith:=sTexteRemplacement, Replace:=WdReplace.wdReplaceAll)

            objDoc.Save()
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        Finally
            objDoc = COMM_APP_WEB_RELE_OBJ(objDoc)
            oWord.Quit(False)
            oWord = COMM_APP_WEB_RELE_OBJ(oWord)
        End Try
        LOG_Msg(GetCurrentMethod, $"Le texte <<{sTexteRecherche}>> a été remplacé par <<{sTexteRemplacement}>> dans le fichier {sFichierWord}")

    End Sub

    Public Shared Sub WORD_IMPR_DOC(sFichierWord As String, simprimante As String)

        Dim oWord As Application = WORD_OUVR()
        Dim objDoc As New Document
        Try
            objDoc = oWord.Documents.Open(sFichierWord)
            objDoc = oWord.ActiveDocument
            'LOG_Msg(GetCurrentMethod, oWord.ActivePrinter)
            oWord.ActivePrinter = simprimante
            objDoc.PrintOut()
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        Finally
            objDoc = COMM_APP_WEB_RELE_OBJ(objDoc)
            oWord.Quit(False)
            oWord = COMM_APP_WEB_RELE_OBJ(oWord)
        End Try
        LOG_Msg(GetCurrentMethod, $"Le fichier {sFichierWord} a été imprimé sur l'imprimante {simprimante}")

    End Sub
    Public Shared Sub WORD_SAVE_AS(sFichierWord As String, sfichiersaveas As String, Optional oformat As WdSaveFormat = WdSaveFormat.wdFormatDocument)
        Dim oWord As Application = WORD_OUVR()
        Dim objDoc As New Document
        Try
            objDoc = oWord.Documents.Open(sFichierWord)
            objDoc = oWord.ActiveDocument
            objDoc.SaveAs(sfichiersaveas, oformat)
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        Finally
            'objDoc.Close()

            objDoc = COMM_APP_WEB_RELE_OBJ(objDoc)
            oWord.Quit(False)
            oWord = COMM_APP_WEB_RELE_OBJ(oWord)
        End Try
        LOG_Msg(GetCurrentMethod, $"Le fichier {sFichierWord} a été enregistré à l'emplacement {sfichiersaveas}")
    End Sub
    'Private Declare Auto Function GetWindowThreadProcessId Lib "user32.dll" (ByVal hwnd As IntPtr, ByRef lpdwProcessId As Integer) As Integer
End Class
