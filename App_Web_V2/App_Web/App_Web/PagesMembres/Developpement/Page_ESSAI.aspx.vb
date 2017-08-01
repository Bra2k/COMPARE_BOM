Imports App_Web.Class_COMM_APP_WEB
Imports PdfSharp
Imports PdfSharp.Drawing
Imports PdfSharp.Drawing.Layout
Imports PdfSharp.Pdf
Imports System.Diagnostics
Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports System.Drawing.Printing
'Imports System.Windows.Controls
'Imports AcroPDFLib
Imports System.Security.Principal
Imports System.Runtime.InteropServices
Imports App_Web.Class_PDF
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Windows.Forms
Imports App_Web.Class_DIG_FACT_SQL
Imports App_Web.Class_SQL
Imports App_Web.Class_SAP_DATA
Imports App_Web.Class_DIG_FACT
Imports App_Web.Class_DOC_ITEXT_SHARP
Imports iTextSharp.text.pdf
Imports iTextSharp.text
Imports iTextSharp
Imports System

'Imports System.Drawing.Printing
'Imports System.Printing

Public Class Page_ESSAI
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim sQuery = "SELECT top 100 [CD_DEVDEUI]    as DEV_EUI
        'FROM [dbo].[DWH_DIM_DEVDEUI]" 'todo order by date
        'Dim sFichier = ""
        'Using dt = SQL_SELE_TO_DT(sQuery, CS_SENSINGLABS_PRD)
        '    sFichier = DOC_ITEXT_SHARP_LIST_CLSG_SENS_LABS(dt, "SENED959700$")
        'End Using
        'sQuery = "SELECT top 50 [CD_DEVDEUI]    as DEV_EUI
        'From [dbo].[DWH_DIM_DEVDEUI]" 'todo order by date
        'Using dt = SQL_SELE_TO_DT(sQuery, CS_SENSINGLABS_PRD)
        '    sFichier = DOC_ITEXT_SHARP_LIST_CLSG_SENS_LABS(dt, "SENED959700$")
        'End Using
        'ClientScript.RegisterStartupScript([GetType](), "printPdf", "document.getElementById(""pdf"").src = """ & Path.GetFileName(sFichier) & """;
        '                                                                             document.getElementById(""pdf"").onload = function() {window.frames[""pdf""].focus();};", True)
    End Sub
    <DllImport("advapi32.DLL", SetLastError:=True)> Public Shared Function LogonUser(ByVal lpszUsername As String, ByVal lpszDomain As String, ByVal lpszPassword As String, ByVal dwLogonType As Integer, ByVal dwLogonProvider As Integer, ByRef phToken As IntPtr) As Integer
    End Function

    <DllImport("shell32.dll")> Public Shared Function ShellExecuteA(hwnd As IntPtr, operation As String, file As String, paramters As String, directory As String, showcmd As Integer) As Integer
    End Function
    Public _filename As String = vbNull


    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            'Image1.ImageUrl = Session("thumbnailphoto")
            'Image1. = Session("thumbnailphoto")
            ''LOG_Msg(GetCurrentMethod, )
            'Dim bPhoto As Byte = System.Convert.ToBase64String(Session("thumbnailphoto"))
            'Dim img = Me.Items.Item("coucou")
            'img.ToString()
            '"<img src='data:image/jpeg;base64, " + System.Convert.ToBase64String(data) + "' alt='photo' />"

            'Dim strTempFile = "C:\sources\App_Web\App_Themes\PIC_CHAR\image.jpg"
            'Dim Stream = New System.IO.FileStream(strTempFile, System.IO.FileMode.Create)
            'Stream.Write(bPhoto, 0, Len(Session("thumbnailphoto")))
            'Stream.Close()

            ''Dim msPhoto As MemoryStream = New MemoryStream(bPhoto)
            ''Dim iPhoto = Bitmap.FromStream(msPhoto)

            '' Dim saveImagePath = Server.MapPath("~\App_Themes\PIC_CHAR\image.jpg")
            ''iPhoto.Save(saveImagePath, ImageFormat.Jpeg)
            'Image1.ImageUrl = "~\App_Themes\PIC_CHAR\image.jpg"
            'Image1.source
            Dim sEMET As String = "mail généric"
            If Session("mail") <> "" Then sEMET = Session("mail")
            COMM_APP_WEB_ENVO_MAIL(
           "smtp.eolane.com",
            sEMET,
            "vincent.sperperini@eolane.com;vincent.sperperini@eolane.com",
           "CREDENTIAL",
           "Bonjour " & vbCrLf & "Vous trouverez ci-joint",
           Session,
           "vide",
           "vide",
           "\\so2k3vm02\Bureautique\Groupes\SO_CLIENTS\C43 (EDF)\Télécommande\C43E978550$ Ens. Télécommande\F-Donnees de sortie\3-Fiches Suiveuses\Colisage\delivery_form_20161118081913.pdf")

            COMM_APP_WEB_ENVO_MAIL(
           "smtp.eolane.com",
           sEMET,
            "vincent.sperperini@eolane.com;vincent.sperperini@eolane.com",
           "CREDENTIAL",
           "Bonjour " & vbCrLf & "Vous trouverez ci-joint",
           Session,
           "vide",
           "vide",
           "\\cedb03\sources\temp_App_Web\VALEURS_825.csv")

            COMM_APP_WEB_ENVO_MAIL(
           "smtp.eolane.com",
            sEMET,
            "vincent.sperperini@eolane.com;vincent.sperperini@eolane.com",
           "CREDENTIAL",
           "Bonjour " & vbCrLf & "Vous trouverez ci-joint",
           Session,
           "vide",
           "vide",
           "\\so2k3vm02\Bureautique\Groupes\SO_CLIENTS\C43 (EDF)\Télécommande\C43E978550$ Ens. Télécommande\F-Donnees de sortie\3-Fiches Suiveuses\Colisage\delivery_form_20161118081913.pdf;\\so2k3vm02\Bureautique\Groupes\SO_CLIENTS\C43 (EDF)\Télécommande\C43E978550$ Ens. Télécommande\F-Donnees de sortie\3-Fiches Suiveuses\Colisage\delivery_form_20161118081913.pdf")

            COMM_APP_WEB_ENVO_MAIL(DIG_FACT_SQL_GET_PARA("C43E978550$", "Serveur SMTP mail"),
                                               sEMET,
                                               DIG_FACT_SQL_GET_PARA("C43E978550$", "Destinataires mail"),
                                               DIG_FACT_SQL_GET_PARA("C43E978550$", "Sujet mail"),
                                               DIG_FACT_SQL_GET_PARA("C43E978550$", "Contenu mail"),
                                               Session,
                                               DIG_FACT_SQL_GET_PARA("C43E978550$", "Destinataires en copie mail"),
                                               DIG_FACT_SQL_GET_PARA("C43E978550$", "Destinataires en copie cachée mail"),
                                                "\\so2k3vm02\Bureautique\Groupes\SO_CLIENTS\C43 (EDF)\Télécommande\C43E978550$ Ens. Télécommande\F-Donnees de sortie\3-Fiches Suiveuses\Colisage\delivery_form_20161118081913.pdf;\\so2k3vm02\Bureautique\Groupes\SO_CLIENTS\C43 (EDF)\Télécommande\C43E978550$ Ens. Télécommande\F-Donnees de sortie\3-Fiches Suiveuses\Colisage\delivery_form_20161118081913.pdf")



            'Dim admin_token As IntPtr
            'Dim wid_current As WindowsIdentity = WindowsIdentity.GetCurrent()
            'Dim wid_admin As WindowsIdentity = Nothing
            'Dim wic As WindowsImpersonationContext = Nothing

            'If LogonUser("ce_adminsv", "eolane", "Eol@ne14", 9, 0, admin_token) = 0 Then Throw New Exception("Log as n'a pas fonctionné")
            'wid_admin = New WindowsIdentity(admin_token)
            'wic = wid_admin.Impersonate()
            'Dim pathToExecutable As String = "C:\Program Files (x86)\Adobe\Reader 11.0\Reader\AcroRd32.exe"
            'Dim sReport = "c:\sources\temp_rapport_traça_stw.pdf" 'Complete name/path of PDF file	 
            'Dim SPrinter = "\\SO2K8VM07\SO_IMP08" 'Name Of printer	 
            'Dim starter As New ProcessStartInfo("""" & pathToExecutable & """", " /t """ + sReport + """ " + SPrinter)
            'Dim Process As New Process()
            'Process.StartInfo = starter
            'Process.Start()
            ''Dim sOutput As String

            ''Try And close the process With 20 seconds delay
            'System.Threading.Thread.Sleep(1000)
            '    'Process.CloseMainWindow()
            '    Dim iLoop As Int16 = 0
            '    'check the process has exited or not
            '    If Process.HasExited = False Then
            '        'if not then loop for 100 time to try and close the process'with 10 seconds delay
            '        While Not Process.HasExited
            '            System.Threading.Thread.Sleep(1000)
            '            'Process.CloseMainWindow()
            '            iLoop = CShort(iLoop + 1)
            '            If iLoop >= 10 Then
            '                'Process.Kill()

            '                Exit While
            '            End If
            '        End While
            '    End If
            '    'Using oStreamReader As System.IO.StreamReader = Process.StandardOutput
            '    '    sOutput = oStreamReader.ReadToEnd()
            '    'End Using
            '    Process.Close()
            '    Process.Dispose()
            '    Process = Nothing
            '    starter = Nothing
            '    ''LOG_Msg(GetCurrentMethod, sOutput)
            '    'LOG_Msg(GetCurrentMethod, """" & pathToExecutable & """ /t """ + sReport + """ " + SPrinter)
            '    'LOG_Msg(GetCurrentMethod, "/c """ & pathToExecutable & """ /t """ + sReport + """ " + SPrinter)
            '    Dim OpenCMD
            '    OpenCMD = CreateObject("wscript.shell")
            'OpenCMD.run("""" & pathToExecutable & """ /t """ + sReport + """ " + SPrinter)

            ''ShellExecuteA(IntPtr.Zero, "print", "c:\sources\temp_rapport_traça_stw.pdf", vbNull, vbNull, 0)
            'Dim starter As New ProcessStartInfo("C:\PsTools\PsExec.exe", " -i \\SOSV0002 -u Eolane\ce_adminsv -p Eol@ne14 calc")
            'Dim Process As New Process()

            'Process.StartInfo = starter

            'Using oStreamReader As System.IO.StreamReader = Process.StandardOutput
            '    Process.Start()
            '    LOG_Msg(GetCurrentMethod, oStreamReader.ReadToEnd())
            'End Using
            'Process.Close()
            'Process.Dispose()
            'Process = Nothing
            ''starter = Nothing
            'Dim procStartInfo As New ProcessStartInfo
            'Dim procExecuting As New Process
            ''Dim userPassword As String = "Eol@ne14"
            ''Dim securePassword As New System.Security.SecureString()
            ''For Each c As Char In userPassword
            ''    securePassword.AppendChar(c)
            ''Next c
            'With procStartInfo
            '    .UseShellExecute = False
            '    .FileName = "cmd"
            '    '.WindowStyle = ProcessWindowStyle.Maximized
            '    '.Domain = "Eolane"
            '    '.UserName = "ce_adminsv"
            '    '.Password = securePassword
            '    .Arguments = "/c C:\PsTools\PsExec.exe -i \\SOSV0002 -u Eolane\ce_adminsv -p Eol@ne14 calc"
            'End With

            'procExecuting = Process.Start("cmd", "/c C:\PsTools\PsExec.exe -i \\SOSV0002 -u Eolane\ce_adminsv -p Eol@ne14 calc")
            ''C:\PsTools\PsExec.exe -i \\SOSV0002 -u "Eolane\ce_adminsv" -p "Eol@ne14" "calc"

            ' Dim OpenCMD
            ' OpenCMD = CreateObject("wscript.shell")
            '  OpenCMD.run("C:\PsTools\PsExec.exe -i \\SOSV0002 -u ""Eolane\ce_adminsv"" -p ""Eol@ne14"" calc")


            'ShellExecuteA(IntPtr.Zero, "runas", "/user:\\Eolane\ce_adminsv C:\PsTools\PsExec.exe", "-i \\SOSV0002 -u Eolane\ce_adminsv -p Eol@ne14 calc", vbNull, 0)

        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try



        '        Comme noté dans les exemples du lien que je t'ai donné, il faut que tu te serves de ce paramètre de ligne de commande : 
        '        pdfcreator.exe / If"C:\description.ps" /OF"C:\description.pdf" /OutputSubFormat"PDF/A-1b"

        'If est Then l'input file (fichier à transformer) 
        'OF est l'output file (fichier pdf résultant) 
        'OutputSubFormat je ne sais pas (à toi de regarder la doc de PDFCreator) 

        'Tu remarqueras que les chemins des fichiers sont à inclure dans le paramètre, ainsi, il sera créé au bon endroit. 

        'Dans ton application, utilise l'objet Process et son paramètre ProcessStartInfo pour démarrer PDFCreator avec les paramètres appropriés. 

        'Note:       A cause des espaces souvent contenus dans les chemins de fichiers, tu devras encadrer outputfile et inputfile par des guillemets en te servant par exemple du très pratique String.Format
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Dim sQuery = "SELECT top 100 [CD_DEVDEUI]    as DEV_EUI
        FROM [dbo].[DWH_DIM_DEVDEUI]" 'todo order by date
        Dim sFichier = ""
        Using dt = SQL_SELE_TO_DT(sQuery, CS_SENSINGLABS_PRD)
            sFichier = DOC_ITEXT_SHARP_LIST_CLSG_SENS_LABS(dt, "SENED959700$")
        End Using
        COMM_APP_WEB_COPY_FILE(sFichier, Server.MapPath($"~/PagesMembres/Developpement/{Path.GetFileName(sFichier)}"), True) 'todo possible ne sert à rien
        ClientScript.RegisterStartupScript([GetType](), "printPdf", COMM_APP_WEB_JS_IPSO_FICH_PDF(sFichier), True)
    End Sub

    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim sQuery = "SELECT top 50 [CD_DEVDEUI]    as DEV_EUI
        From [dbo].[DWH_DIM_DEVDEUI]" 'todo order by date
        Dim sFichier = ""
        Using dt = SQL_SELE_TO_DT(sQuery, CS_SENSINGLABS_PRD)
            sFichier = DOC_ITEXT_SHARP_LIST_CLSG_SENS_LABS(dt, "SENED959700$")
        End Using
        COMM_APP_WEB_COPY_FILE(sFichier, Server.MapPath($"~/PagesMembres/Developpement/{Path.GetFileName(sFichier)}"), True) 'todo possible ne sert à rien
        ClientScript.RegisterStartupScript([GetType](), "printPdf", COMM_APP_WEB_JS_IPSO_FICH_PDF(sFichier), True)
    End Sub

    'Private WithEvents p_Document As PrintDocument = Nothing

    'Private Sub SelectPrinterThenPrint()
    '    Dim PrintersDialog As New PrintDialog()

    '    If PrintersDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
    '        Try
    '            p_Document = New PrintDocument()
    '            PrintersDialog.Document = p_Document
    '            AddHandler p_Document.PrintPage, AddressOf HandleOnPrintPage

    '        Catch CurrentException As Exception

    '        End Try
    '    End If
    'End Sub

    'Private Sub HandleOnPrintPage(ByVal sender As Object, ByVal e As PrintPageEventArgs) Handles p_Document.PrintPage
    '    Dim MorePagesPending As Boolean = False

    '    'e.Graphics.Draw...(....)
    '    'e.Graphics.DrawString(....)
    '    ' Draw everything...

    '    If MorePagesPending Then
    '        e.HasMorePages = True
    '    Else
    '        e.HasMorePages = False
    '    End If
    'End Sub
    '    Public Shared Sub FilePrinter(filename As String)

    '        _filename = filename
    '    End Sub

    '    Public Shared Sub Print()

    '        If Not _filename = vbNull Then ShellExecuteA(IntPtr.Zero, "print", _filename, vbNull, vbNull, 0)

    '    End Sub
    '    Public Shared Function FileName() As String

    '		Get			Return _filename

    '		Set 				_filename = value


    'End Function
End Class