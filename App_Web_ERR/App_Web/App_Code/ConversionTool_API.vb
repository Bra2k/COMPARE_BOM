Imports System.Web
Imports System.Net
Imports System.IO
Namespace App_Code
    Public Class ConversionTool_API

        Public Shared Function ConversionTool_API_Connexion(sFile As String) As String

            Dim oCT_API As HttpWebRequest
            Dim resp As HttpWebResponse
            Dim srIn As StreamReader
            Dim outbuf As String
            Dim boundary As String

            boundary = MakeBoundary()

            oCT_API = WebRequest.Create("https://conversiontools.io/api/api/files")
            oCT_API.AllowAutoRedirect = False
            oCT_API.Method = "POST"
            oCT_API.ContentType = "multipart/form-data; boundary=" & boundary

            ' NOTE: Use the AppendMultiPartFormField function if you need any other normal "non-file" data fields in the form data

            Encoding.Default.GetBytes("--" & boundary & vbCrLf &
            "Content-Disposition: form-data; name=""file""" & vbCrLf & vbCrLf &
           "C:\sources\SERCEL\Données SQL\Emballage.txt" & vbCrLf & "--" & boundary & "--" & vbCrLf)

            ' NOTE: AppendMultiPartFile is the one which does the real work!
            ' In the sample below "fname" is name of the file data field in the form data. This may or may not be important to your receiving page...

            resp = CType(oCT_API.GetResponse(), HttpWebResponse)

            'NOTE: The next four lines below can be omitted if you don't need the response from the remote server....
            srIn = New StreamReader(resp.GetResponseStream())
            outbuf = srIn.ReadToEnd()
            srIn.Close()

            MsgBox(outbuf)

            Return "toto"

        End Function

        Shared Function MakeBoundary() As String

            Dim tmp As String
            Dim rand As New Random()

            tmp = "---------------------------" & Hex(rand.Next(&H1000, &HFFFF)) & Hex(rand.Next(&H1000, &HFFFF)) & Hex(rand.Next(&H1000, &HFFFF))

            Return tmp

        End Function


        Shared Sub AppendMultiPartFormField(ByRef bw As BinaryWriter, boundary As String, fieldname As String, fielddata As String)
            With bw
                .Write(Encoding.Default.GetBytes("--" & boundary & vbCrLf))
                .Write(Encoding.Default.GetBytes("Content-Disposition: form-data; name=""" & fieldname & """" & vbCrLf))
                .Write(Encoding.Default.GetBytes(vbCrLf))
                .Write(Encoding.Default.GetBytes(fielddata & vbCrLf))
            End With
        End Sub

        Shared Sub AppendMultiPartEndBoundary(ByRef bw As BinaryWriter, boundary As String)

            bw.Write(Encoding.Default.GetBytes("--" & boundary & "--" & vbCrLf))

        End Sub



        Shared Sub AppendMultiPartFile(ByRef bw As BinaryWriter, boundary As String, fieldname As String, filename As String) 'As String

            Dim fsFile As FileStream
            Dim FileData As Byte()

            fsFile = New FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read)
            ReDim FileData(fsFile.Length - 1)
            fsFile.Read(FileData, 0, fsFile.Length)
            fsFile.Close()

            With bw
                .Write(Encoding.Default.GetBytes("--" & boundary & vbCrLf))
                .Write(Encoding.Default.GetBytes("Content-Disposition: form-data; name=""" & fieldname & """; filename=""" & filename & """" & vbCrLf))
                ' NOTE: This function just uses the application/octet-stream mime type for all files. This is normally fine for pretty much any type of file, but if you need a specific type (eg image/jpeg, video/mpeg) then you could enhance the function to use a more specific type according to the file being read...
                .Write(Encoding.Default.GetBytes("Content-Type: application/octet-stream" & vbCrLf))
                .Write(Encoding.Default.GetBytes(vbCrLf))
                .Write(FileData)
                .Write(Encoding.Default.GetBytes(vbCrLf))
            End With
        End Sub
    End Class
End Namespace
