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

'Imports System.Drawing.Printing
'Imports System.Printing

Public Class Page_ESSAI
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Not IsPostBack Then
        '    If Session("displayname") = "" Then
        '        Context.GetOwinContext().Authentication.SignOut(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie)
        '    Else
        '        If App_Web.Class_COMM_APP_WEB.COMM_APP_WEB_GET_DROI_PAGE(Replace(HttpContext.Current.Request.Url.AbsoluteUri, "http://" & LCase(My.Computer.Name) & "/PagesMembres/", "~/PagesMembres/") & ".aspx", Session("department"), Session("User_Name")) = False Then Response.Redirect("~/PagesMembres/RDRC_PAGE_MEMB.aspx")
        '    End If
        'End If
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

        Using dt4 As New DataTable
            dt4.Columns.Add("#2", Type.GetType("System.String"))
            dt4.Columns.Add("#5", Type.GetType("System.String"))
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A01026F7"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000003"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A010271E"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000004"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102727"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000008"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A010272B"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000010"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A01026E8"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000015"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A010275E"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000017"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102729"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000019"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A010271D"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000024"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A010272F"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000027"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A01026E9"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000029"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102741"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000448"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102740"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000457"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102742"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000471"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102746"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000506"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102745"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000507"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102743"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000510"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102717"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000652"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A01026F4"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000653"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A01026D5"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000654"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A010270B"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000655"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A01026D8"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000658"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A01026FB"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000659"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102701"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000660"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102708"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000661"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A01026F2"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000663"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A01026CF"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000665"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A01026FF"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000669"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A01026ED"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000670"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A01026CA"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000672"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A01026C9"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000673"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A01026CE"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000674"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A01026D2"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000675"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A01026EA"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000676"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102714"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000677"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102705"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000680"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102702"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000684"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102712"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000686"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102700"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000687"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A01026D7"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000689"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A01026F1"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000690"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A010274D"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000692"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A010273B"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000694"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102754"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000695"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A010274A"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000700"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A010274C"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000701"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A010273D"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000703"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102748"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000704"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102715"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000713"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A010273C"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000716"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A01026DA"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000717"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102758"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000719"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A010274E"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000722"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A01026DB"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000723"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102749"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000725"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A010270C"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000727"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A01026F9"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000728"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102752"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000729"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A01026EE"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000730"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102755"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000731"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A010273F"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000732"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102747"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000733"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A01026DF"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000735"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102716"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000736"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A010274B"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000737"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102757"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000739"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102739"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000744"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A01026DC"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000759"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A010275F"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000762"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A01026E0"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000763"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102736"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000764"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A010271C"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000769"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102733"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000771"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102738"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000778"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102759"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000786"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A01026FC"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000788"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102762"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000791"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A010273A"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000793"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102722"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000794"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A010272A"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000796"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102737"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000797"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102734"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000799"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102725"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000800"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102732"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000806"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102764"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000808"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A010275D"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000809"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A01026E2"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000810"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A01026D9"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000811"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A01026FA"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000812"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102711"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000813"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A010270F"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000814"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A01026D1"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000816"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A01026D0"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000817"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A010272C"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000818"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102760"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000819"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A010272E"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000820"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102735"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000939"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102718"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000941"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102703"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000942"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A0102709"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000944"
            dt4.Rows.Add()
            dt4.Rows(dt4.Rows.Count - 1)("#2") = "70B3D580A010271A"
            dt4.Rows(dt4.Rows.Count - 1)("#5") = "1178583000945"
            Dim sData As String = ""
            Using sr_AVA = New StreamReader("\\cedb03\sources\Digital Factory\Etiquettes\Sensing Labs\Etiquette_Modele.prn", Encoding.UTF8)
                sData = sr_AVA.ReadToEnd()
                sr_AVA.Close()
            End Using
            Dim sdata_to_print As String = ""
            For Each rdt In dt4.Rows
                sdata_to_print = Replace(Replace(sData, "#2", rdt("#2").ToString), "#5", rdt("#5").ToString)
                Dim sfich As String = $"{My.Settings.RPTR_TPRR}\{CInt(Int((1000 * Rnd()) + 1))}_Etiquette_Modele.prn"
                Using sw = New StreamWriter(sfich, False, System.Text.Encoding.UTF8)
                    sw.WriteLine(sdata_to_print)
                    sw.Close()
                End Using
                COMM_APP_WEB_IMPR_ETIQ_PRN(sfich, "\\SO2K8VM07\CEIMP16")
            Next


        End Using
        'My.Computer.Network.UploadFile("\\cedb03\sources\Digital Factory\Etiquettes\C43\ARIALN.TTF", "ftp://ftpcard:card@10.100.14.70/fonts/ARIALN.TTF")
        'COMM_APP_WEB_COPY_FICH_TO_CAB("\\SO2K8VM07\CEIMP22", "\\cedb03\sources\Digital Factory\Etiquettes\C43\ARIALN.TTF")
    End Sub

    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ' TextBox1.Text =
        Dim sFichier As String = Class_DIG_FACT.DIG_FACT_IMPR_PDF(TextBox1.Text,
                                                                       "1165128", "", "Carton",
                                                                       "", "(01)03760012570010(11)170109(21)17020002", "", "1",
                                                                       "1", "1", Nothing, Nothing, 0, "c:\Sources\App_Web\PagesMembres\Developpement\")
        ClientScript.RegisterStartupScript([GetType](), "printPdf", "document.getElementById(""pdf"").src = """ & Path.GetFileName(sFichier) & """;
                                                                                     document.getElementById(""pdf"").onload = function() {window.frames[""pdf""].focus();};", True)
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