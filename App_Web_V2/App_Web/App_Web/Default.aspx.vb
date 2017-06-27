Imports App_Web.LOG
Imports System.Reflection.MethodBase
Class _Default
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Dim data As Byte() = Session("de_AD").Properties("thumbnailphoto").Value
            System.Convert.ToBase64String(Session("de_AD").Properties("thumbnailphoto").Value).ToString()
            'Session("thumbnailphoto") = data
            LOG_Msg(GetCurrentMethod, $"thumbnailphoto : <img src='data:image/jpeg;base64, {System.Convert.ToBase64String(data)}' alt='photo' />")
            Label_PHOT.Text = $"<img src='data:image/jpeg;base64, {System.Convert.ToBase64String(data)}' alt='photo' />"
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, $"thumbnailphoto : {ex.Message}")
            Exit Sub
        End Try
    End Sub

    'Protected Sub ImageButton1_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton1.Click

    'End Sub

    'Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
    '    Try
    '        GridView1.DataSource = COMM_APP_WEB_GET_NETW_PRIN_INFO()
    '        GridView1.DataBind()

    '        COMM_APP_WEB_COPY_FICH_TO_CAB("\\SO2K8VM07\CEIMP15", "\\so2k3vm02\Application\Production\Tracabilite\Digital_Factory\Colisage\Etiquettes\STAGO\stago.bmp")
    '        COMM_APP_WEB_COPY_FICH_TO_CAB("\\SO2K8VM07\CEIMP15", "\\so2k3vm02\Application\Production\Tracabilite\Digital_Factory\Colisage\Etiquettes\STAGO\ARIALN.TTF")
    '        COMM_APP_WEB_COPY_FICH_TO_CAB("\\SO2K8VM07\CEIMP17", "\\so2k3vm02\Application\Production\Tracabilite\Digital_Factory\Colisage\Etiquettes\MEDRIA_TECHNOLOGIES\DFEC.png")
    '        COMM_APP_WEB_COPY_FICH_TO_CAB("\\SO2K8VM07\CEIMP17", "\\so2k3vm02\Application\Production\Tracabilite\Digital_Factory\Colisage\Etiquettes\MEDRIA_TECHNOLOGIES\CONSOLAS.ttf")

    '        'oShell.Run("runas /user:administrator COPY ""\\so2k3vm02\Application\Production\Tracabilite\Digital_Factory\Colisage\Etiquettes\HILL_ROM\Load_label.prn"" ""c:\sources\Load_label.prn""")
    '        'Shell("""\\so2k3vm02\Application\Production\Gestion_Defauts\Saisie_Defauts\Saisie_des_defauts_V18.10.exe"" -a -q")
    '        'Dim procID As Integer
    '        'Dim newProc As Diagnostics.Process
    '        'newProc = Diagnostics.Process.Start("\\so2k3vm02\Application\Production\Gestion_Defauts\Saisie_Defauts\Saisie_des_defauts_V18.10.exe")
    '        'procID = newProc.Id
    '        'newProc.WaitForExit()
    '        'Dim procEC As Integer = -1
    '        'If newProc.HasExited Then
    '        '    procEC = newProc.ExitCode
    '        'End If
    '        'WORD_COMPR_PICS("\\cedb03\sources\Nouveau Document Microsoft Word - Copie.docx")
    '    Catch ex As Exception
    '        LOG_Erreur(GetCurrentMethod, ex.Message)
    '        'Dim ret As Integer = Marshal.GetLastWin32Error()
    '        Exit Sub
    '    End Try

    'End Sub


    'Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.LoadComplete
    '
    'If Session("User_Name") <> "" Then MultiView_LOG.SetActiveView(View_LOG)

    'Dim dt As DataTable = COMM_APP_WEB_GET_CFGR_AFCG_PAGE_DEFA_TO_DT(Session("department"))
    'Dim hl As HyperLink
    'Dim ijump As Integer = 1
    'If Not dt Is Nothing Then
    'For Each rdt As DataRow In dt.Rows
    '           hl = New HyperLink
    '          hl.Text = rdt("NM_PAGE").ToString
    '         hl.NavigateUrl = rdt("NM_URL_PAGE").ToString
    '        hl.ToolTip = rdt("NM_PAGE").ToString
    '       hl.ImageUrl = rdt("NM_IMAG_URL").ToString
    '      PlaceHolder_LINK.Controls.Add(hl)
    '     ijump += 1
    'If ijump = 4 Then
    '             hl.Text &= "<br />"
    '               ijump = 1
    '          End If
    '     Next
    'End If
    'dt = COMM_APP_WEB_GET_CFGR_AFCG_PAGE_DEFA_TO_DT(Session("User_Name"))
    'If Not dt Is Nothing Then
    '  For Each rdt As DataRow In dt.Rows
    '     hl = New HyperLink
    '        hl.Text = rdt("NM_PAGE").ToString
    '        hl.NavigateUrl = rdt("NM_URL_PAGE").ToString
    '        hl.ToolTip = rdt("NM_PAGE").ToString
    '        hl.ImageUrl = rdt("NM_IMAG_URL").ToString
    '        PlaceHolder_LINK.Controls.Add(hl)
    '        ijump += 1
    '        If ijump = 4 Then
    '            hl.Text &= "<br />"
    '            ijump = 1
    '        End If
    '    Next
    'End If
    'End Sub
End Class