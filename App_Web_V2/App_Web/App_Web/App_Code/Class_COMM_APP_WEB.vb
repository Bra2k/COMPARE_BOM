Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports System.IO
Imports App_Web.Class_SQL
Imports System.Runtime.InteropServices
Imports System
Imports System.Security.Principal
Imports System.DirectoryServices
Imports PdfSharp.Pdf

Public Class Class_COMM_APP_WEB

    Public Shared Function COMM_APP_WEB_CLEA_REPE(sRepère As String) As String

        Dim caractère As String = ""
        Dim nu_char As Integer = 0, nu As String = ""
        Try
            For i As Integer = 1 To Len(sRepère)
                If Not Char.IsLetter(Mid(sRepère, i, 1)) Then Exit For
                caractère = Mid(sRepère, 1, i)
                nu_char += 1
            Next
            sRepère = Right(sRepère, Len(sRepère) - nu_char)
            nu_char = 0
            For i As Integer = 1 To Len(sRepère)
                If Char.IsLetterOrDigit(Mid(sRepère, i, 1)) Then Exit For
                nu_char += 1
            Next
            sRepère = Right(sRepère, Len(sRepère) - nu_char)
            nu_char = 0
            For i As Integer = 1 To Len(sRepère)
                If Not Char.IsNumber(Mid(sRepère, i, 1)) Then Exit For
                nu = Mid(sRepère, 1, i)
                nu_char += 1
            Next
            sRepère = caractère & Convert.ToDecimal(nu).ToString
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try

        LOG_Msg(GetCurrentMethod, "Le repère retourné " & sRepère)
        Return sRepère

    End Function

    Public Shared Function COMM_APP_WEB_IMP_CSV_DT(savePath As String) As DataTable

        Dim dt As New DataTable
        Dim firstLine As Boolean = True
        Try
            If IO.File.Exists(savePath) Then
                Using sr As New StreamReader(savePath, Encoding.GetEncoding(1252))
                    While Not sr.EndOfStream
                        If firstLine Then
                            firstLine = False
                            Dim cols = sr.ReadLine.Split(";")
                            For col As Integer = 1 To cols.Count
                                dt.Columns.Add(New DataColumn(col.ToString, GetType(String)))
                            Next
                        Else
                            Dim data() As String = sr.ReadLine.Split(";")
                            dt.Rows.Add(data.ToArray)
                        End If
                    End While
                End Using
            End If
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try

        LOG_Msg(GetCurrentMethod, "Le fichier " & savePath & " a été importé.")
        Return dt

    End Function

    Public Shared Function COMM_APP_WEB_LVS_DIST(ByVal s As String, ByVal t As String) As Integer
        Dim n As Integer = s.Length
        Dim m As Integer = t.Length
        Dim d(n + 1, m + 1) As Integer
        Dim i As Integer
        Dim j As Integer
        Dim cost As Integer
        Try
            If n = 0 Then
                Return m
            End If

            If m = 0 Then
                Return n
            End If

            For i = 0 To n
                d(i, 0) = i
            Next

            For j = 0 To m
                d(0, j) = j
            Next

            For i = 1 To n
                For j = 1 To m

                    If t(j - 1) = s(i - 1) Then
                        cost = 0
                    Else
                        cost = 1
                    End If

                    d(i, j) = Math.Min(Math.Min(d(i - 1, j) + 1, d(i, j - 1) + 1),
                               d(i - 1, j - 1) + cost)
                Next
            Next
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return 0
        End Try

        LOG_Msg(GetCurrentMethod, "Le code Levenshtein a été exécuté.")
        Return d(n, m)

    End Function

    Public Shared Function COMM_APP_WEB_RELE_OBJ(ByVal obj As Object) As Object
        Try
            Dim intRel As Integer = 0
            Do
                intRel = System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            Loop While intRel > 0
            'intRel = 0
            'Do
            '    intRel = System.Runtime.InteropServices.Marshal.FinalReleaseComObject(obj)
            '    LOG_Msg(GetCurrentMethod, intRel)

            'Loop While intRel > 0
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            obj = Nothing
        Finally
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try
        obj = Nothing

        'LOG_Msg(GetCurrentMethod, "objet killé")
        Return obj

    End Function

    Public Shared Sub COMM_APP_WEB_IMPR_ETIQ_PRN(sFichier As String, sImprimante As String)

        'Dim p As Process
        Try
            Using p = Process.Start("cmd", $"/c COPY ""{sFichier}"" ""{sImprimante}""")
                p.WaitForExit()
                LOG_Msg(GetCurrentMethod, $"Le fichier {sFichier} a été imprimé sur l'imprimante {sImprimante}.")
            End Using
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Exit Sub
        End Try

    End Sub

    Public Shared Function COMM_APP_WEB_CONV_FORM_DATE(sDT As String, sFORM As String) As String

        Dim sWeek As String
        Dim sDT_CONV As String = sDT
        Try
            If sFORM = "" Then Throw New Exception("Pas de format spécifié")
            sDT_CONV = CDate(sDT).ToString(sFORM)
            If sDT_CONV.IndexOf("WW") > 0 Then
                sWeek = DatePart(DateInterval.WeekOfYear, CDate(sDT)) - 1
                sWeek = StrDup(2 - Len(sWeek), "0") & sWeek
                sDT_CONV = Replace(sDT_CONV, "WW", sWeek)
            End If
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return sDT
        End Try

        'LOG_Msg(GetCurrentMethod, "La date " & sDT & " a été convertie en " & sDT_CONV & ".")
        Return sDT_CONV

    End Function

    Public Shared Function COMM_APP_WEB_CONV_BASE_N_2_DEC(sNB_BASE_N As String, iBASE_N As Integer) As Integer
        Const BASENUMBERS As String = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        Dim iNB_DEC, iEXTR_BASE_N As Integer

        Try
            For iChar As Integer = Len(sNB_BASE_N) To 1 Step -1
                iEXTR_BASE_N = BASENUMBERS.IndexOf(Mid(sNB_BASE_N, iChar, 1))
                If iEXTR_BASE_N >= iBASE_N Then Throw New Exception($"Le caractère {Mid(sNB_BASE_N, iChar, 1)} ne peut exister dans un nombre en base {iBASE_N.ToString}")
                iNB_DEC = iNB_DEC + (iEXTR_BASE_N * (iBASE_N ^ (Len(sNB_BASE_N) - iChar)))
            Next
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try

        'LOG_Msg(GetCurrentMethod, "Le nombre " & sNB_BASE_N & " en base " & iBASE_N.ToString & " a été convertie en " & iNB_DEC.ToString & ".")
        Return iNB_DEC

    End Function

    Public Shared Function COMM_APP_WEB_CONV_DEC_2_BASE_N(iNB_DEC As Integer, iBASE_N As Integer, iNB_CARA As Integer) As String
        Const BASENUMBERS As String = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        Dim sNB_BASE_N As String = "", iQuot As Integer = 9999, irest As Integer
        Dim sbNB_BASE_N As New StringBuilder()
        Try
            If iBASE_N = 10 Then
                sNB_BASE_N = iNB_DEC.ToString
            Else
                While iQuot > iBASE_N
                    iQuot = Math.Floor(iNB_DEC / iBASE_N)
                    irest = iNB_DEC - (iQuot * iBASE_N)
                    'sNB_BASE_N = Mid(BASENUMBERS, irest + 1, 1) & sNB_BASE_N
                    sbNB_BASE_N.Insert(0, Mid(BASENUMBERS, irest + 1, 1))
                    iNB_DEC = iQuot
                End While
                'sNB_BASE_N = Mid(BASENUMBERS, iQuot + 1, 1) & sNB_BASE_N
                sbNB_BASE_N.Insert(0, Mid(BASENUMBERS, iQuot + 1, 1))
                sNB_BASE_N = sbNB_BASE_N.ToString
            End If
            sNB_BASE_N = $"{StrDup(iNB_CARA - Len(sNB_BASE_N), "0")}{sNB_BASE_N}"
            'sbNB_BASE_N.Append(StrDup(iNB_CARA - Len(sbNB_BASE_N.ToString), "0"))

            'sNB_BASE_N = sbNB_BASE_N.ToString
            'sNB_BASE_N.Reverse()
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try

        LOG_Msg(GetCurrentMethod, $"Le nombre {iNB_DEC.ToString} a été convertie en {sNB_BASE_N} en base {iBASE_N.ToString}.")
        Return sNB_BASE_N
    End Function

    <DllImport("advapi32.DLL", SetLastError:=True)> Public Shared Function LogonUser(ByVal lpszUsername As String, ByVal lpszDomain As String, ByVal lpszPassword As String, ByVal dwLogonType As Integer, ByVal dwLogonProvider As Integer, ByRef phToken As IntPtr) As Integer
    End Function
    Public Shared Sub COMM_APP_WEB_COPY_FILE(sfichier_or As String, sdestination As String, Optional boverwrite As Boolean = False)
        Dim admin_token As IntPtr
        Dim wid_current As WindowsIdentity = WindowsIdentity.GetCurrent()
        Dim wid_admin As WindowsIdentity = Nothing
        Dim wic As WindowsImpersonationContext = Nothing
        Try
            If LogonUser("ce_adminsv", "eolane", "Eol@ne14", 9, 0, admin_token) = 0 Then Throw New Exception("Log as n'a pas fonctionné")
            wid_admin = New WindowsIdentity(admin_token)
            wic = wid_admin.Impersonate()
            Directory.CreateDirectory(Path.GetDirectoryName(sdestination))
            System.IO.File.Copy(sfichier_or, sdestination, boverwrite)
            If Not System.IO.File.Exists(sdestination) Then Throw New Exception($"Le fichier {sdestination} n'existe pas")
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Exit Sub
        Finally
            If wic IsNot Nothing Then wic.Undo()
        End Try
        LOG_Msg(GetCurrentMethod, $"Le fichier {sfichier_or} a été copié à l'endroit {sdestination}.")
    End Sub
    Public Shared Sub COMM_APP_WEB_MOVE_FILE(sfichier_or As String, sdestination As String)
        Dim admin_token As IntPtr
        'Dim wid_current As WindowsIdentity = WindowsIdentity.GetCurrent()
        'wid_current.di
        'Dim wid_admin As WindowsIdentity = Nothing
        'Dim wic As WindowsImpersonationContext = Nothing
        Try
            Using wid_current As WindowsIdentity = WindowsIdentity.GetCurrent()
                If LogonUser("ce_adminsv", "eolane", "Eol@ne14", 9, 0, admin_token) = 0 Then Throw New Exception("Log as n'a pas fonctionné")
                Using wid_admin = New WindowsIdentity(admin_token)
                    Using wic = wid_admin.Impersonate()
                        Directory.CreateDirectory(Path.GetDirectoryName(sdestination))
                        System.IO.File.Move(sfichier_or, sdestination)
                        If Not System.IO.File.Exists(sdestination) Then Throw New Exception($"Le fichier {sdestination} n'existe pas")
                        wic.Undo()
                        LOG_Msg(GetCurrentMethod, $"Le fichier {sfichier_or} a été déplacé à l'endroit {sdestination}.")
                    End Using
                End Using
            End Using
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Exit Sub
            'Finally
            '    If wic IsNot Nothing Then wic.Undo()
        End Try
    End Sub
    Public Shared Function COMM_APP_WEB_GET_FILE(spath As String, sssearchpattern As String) As String()
        Dim admin_token As IntPtr
        Dim wid_current As WindowsIdentity = WindowsIdentity.GetCurrent()
        Dim wid_admin As WindowsIdentity = Nothing
        Dim wic As WindowsImpersonationContext = Nothing
        Try
            If LogonUser("ce_adminsv", "eolane", "Eol@ne14", 9, 0, admin_token) = 0 Then Throw New Exception("Log as n'a pas fonctionné")
            wid_admin = New WindowsIdentity(admin_token)
            wic = wid_admin.Impersonate()
            Return Directory.GetFiles(spath, sssearchpattern, SearchOption.AllDirectories)
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        Finally
            If wic IsNot Nothing Then wic.Undo()
        End Try
    End Function
    Public Shared Function COMM_APP_WEB_GET_PARA(sNM_PARA As String, sNM_CTRL As String, Optional pageHandler As String = Nothing) As String

        Dim sQuery As String = ""
        Try
            If pageHandler = Nothing Then pageHandler = HttpContext.Current.CurrentHandler.ToString
            'chercher les param dans la base
            sQuery = $"SELECT [VL_CTRL]
                         FROM [dbo].[V_DER_MAJ_DTM_REF_PARA_ETAT_CRTL]
                        WHERE [NM_PAGE] = '{pageHandler}' AND [PARA] = '{Replace(sNM_PARA, "'", "''")}' AND [ID_CTRL] = '{sNM_CTRL}'"
            Using dt = SQL_SELE_TO_DT(sQuery, CS_APP_WEB_ECO)
                If dt Is Nothing Then Throw New Exception($"Pas de données pour le contrôle {sNM_CTRL}")
                LOG_Msg(GetCurrentMethod, $"Le paramètre {sNM_PARA} a chargé {dt(0)("VL_CTRL").ToString} pour le contrôle {sNM_CTRL} de la page {pageHandler}")
                Return dt(0)("VL_CTRL").ToString
            End Using
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try

    End Function
    Public Shared Sub COMM_APP_WEB_PARA_AFFI_LOAD(sNM_PARA As String, sNM_CTRL As String, Optional Ctrl_View As Control = Nothing, Optional sVAL_CTRL As String = "")

        Dim sTP_CTRL As String, CRTL_GUEST As New Control
        Dim sQuery As String = ""
        Try
            Using pageHandler As Page = HttpContext.Current.CurrentHandler
                'chercher les param dans la base
                sQuery = $"SELECT [VL_CTRL]
                             FROM [dbo].[V_DER_MAJ_DTM_REF_PARA_ETAT_CRTL]
                            WHERE [NM_PAGE] = '{pageHandler.ToString}' AND [PARA] = '{Replace(sNM_PARA, "'", "''")}' AND [ID_CTRL] = '{sNM_CTRL}'"
                Using dt = SQL_SELE_TO_DT(sQuery, CS_APP_WEB_ECO)
                    If dt Is Nothing Then Throw New Exception($"Pas de données pour le contrôle {sNM_CTRL}")
                    sTP_CTRL = Left(sNM_CTRL, sNM_CTRL.IndexOf("_"))
                    sVAL_CTRL = dt(0)("VL_CTRL").ToString
                End Using
                If Ctrl_View Is Nothing Then
                    Using MainContent As ContentPlaceHolder = CType(pageHandler.Master.FindControl("MainContent"), ContentPlaceHolder)
                        CRTL_GUEST = MainContent.FindControl(sNM_CTRL)
                    End Using
                Else
                    CRTL_GUEST = Ctrl_View.FindControl(sNM_CTRL)
                End If
                'LOG_Msg(GetCurrentMethod, CRTL_GUEST.ID)
                If IsDBNull(CRTL_GUEST) Then Throw New Exception($"Contrôle {sNM_CTRL} non trouvé")
                Select Case sTP_CTRL
                    Case "CheckBox"
                        Using CheckBox_GUEST = CType(CRTL_GUEST, CheckBox)
                            'LOG_Msg(GetCurrentMethod, CheckBox_GUEST.Text)
                            Select Case sVAL_CTRL
                                Case "True"
                                    CheckBox_GUEST.Checked = True
                                Case "False"
                                    CheckBox_GUEST.Checked = False
                            End Select
                        End Using
                    Case "DropDownList"
                        Using DropDownList_GUEST = CType(CRTL_GUEST, DropDownList)
                            'LOG_Msg(GetCurrentMethod, DropDownList_GUEST.Text)
                            DropDownList_GUEST.SelectedValue = sVAL_CTRL
                        End Using
                    Case "TextBox"
                        Using TextBox_GUEST = CType(CRTL_GUEST, TextBox)
                            'LOG_Msg(GetCurrentMethod, TextBox_GUEST.Text)
                            TextBox_GUEST.Text = sVAL_CTRL
                        End Using
                    Case "RadioButtonList"
                        Using RadioButtonList_GUEST = CType(CRTL_GUEST, RadioButtonList)
                            'LOG_Msg(GetCurrentMethod, RadioButtonList_GUEST.Text)
                            RadioButtonList_GUEST.SelectedValue = sVAL_CTRL
                        End Using
                End Select
            End Using
            CRTL_GUEST.Dispose()
        Catch ex As Exception
            'LOG_Erreur(GetCurrentMethod, ex.Message)
            Exit Sub
        End Try

        'LOG_Msg(GetCurrentMethod, "Le paramètre a été chargé pour le contrôle " & CRTL_GUEST.ID)

    End Sub

    Public Shared Sub COMM_APP_WEB_PARA_AFFI_SAVE(sNM_PARA As String, sNM_CTRL As String, Optional sVAL_CTRL As String = "vide", Optional spage As String = "vide")

        'Dim CRTL_GUEST As New Control, CheckBox_GUEST As New CheckBox, DropDownList_GUEST As New DropDownList

        Dim sQuery As String = ""
        Try
            If spage = "vide" Then spage = HttpContext.Current.CurrentHandler.ToString
            'si sVAL_CTRL est "vide" prendre la valeur qui a été affectée dans l'interface
            'If sVAL_CTRL = "vide" Then

            'End If
            'chercher les param dans la base

            sQuery = $"INSERT INTO [dbo].[DTM_REF_PARA_ETAT_CRTL] ([NM_PAGE], [PARA], [ID_CTRL], [VL_CTRL], [DT_MAJ])
                            VALUES ('{spage}', '{Replace(sNM_PARA, "'", "''")}', '{sNM_CTRL}', '{sVAL_CTRL}', GETDATE())"
            SQL_REQ_ACT(sQuery, CS_APP_WEB_ECO)
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Exit Sub
        End Try

        LOG_Msg(GetCurrentMethod, "le paramètre a été enregistré.")

    End Sub

    Public Shared Function COMM_APP_WEB_GET_NETW_PRIN_INFO(Optional sPrinter As String = "noprinterdeclared") As DataTable
        'Variable declaration
        Dim prtTable As New DataTable("NetworkPrinters")
        Dim dirEntry As DirectoryEntry = Nothing
        Dim dirSearcher As DirectorySearcher = Nothing
        Dim resultCollection As SearchResultCollection = Nothing

        'Prepare the printer datatable
        With prtTable.Columns
            .Add("DirectoryServiceName", GetType(String))   'cn
            .Add("ServerName", GetType(String))             'serverName
            .Add("Name", GetType(String))                   'printerName
            .Add("NetworkName", GetType(String))            'uNCName
            .Add("ShareName", GetType(String))              'printShareName
            .Add("Comments", GetType(String))               'description
            .Add("Model", GetType(String))                  'driverName
            .Add("Location", GetType(String))               'location
            .Add("Port", GetType(String))                   'portName
            .Add("PrinterLanguage", GetType(String))        'printLanguage
            .Add("InstalledMemory", GetType(Integer))       'printMemory
            .Add("PagesPerMinute", GetType(Integer))        'printPagesPerMinute
            .Add("MaxResolution", GetType(Integer))         'printMaxResolutionSupported      
            .Add("SupportsCollation", GetType(Boolean))     'printCollate
            .Add("SupportsColor", GetType(Boolean))         'printColor
            .Add("SupportsDuplex", GetType(Boolean))        'printDuplexSupported
            .Add("SupportsStapling", GetType(Boolean))      'printStaplingSupported
        End With
        Try
            'search for printers
            dirEntry = New DirectoryEntry("")
            dirSearcher = New DirectorySearcher(dirEntry)
            With dirSearcher
                .PageSize = 50
                .Filter = "objectCategory=printQueue" ' search filter
                .PropertyNamesOnly = True
                .PropertiesToLoad.Add("Name")
                .SearchScope = SearchScope.Subtree
            End With
            resultCollection = dirSearcher.FindAll()
            'Get results
            Dim entry As DirectoryEntry = Nothing
            Dim row As DataRow = Nothing
            Dim obj As Object = Nothing
            For Each result As SearchResult In resultCollection
                entry = result.GetDirectoryEntry
                row = prtTable.NewRow()

                obj = entry.Properties("cn").Value
                If obj IsNot Nothing Then
                    row("DirectoryServiceName") = obj.ToString
                Else
                    row("DirectoryServiceName") = String.Empty
                End If

                obj = entry.Properties("serverName").Value
                If obj IsNot Nothing Then
                    row("ServerName") = obj.ToString
                Else
                    row("ServerName") = String.Empty
                End If

                obj = entry.Properties("printerName").Value
                If obj IsNot Nothing Then
                    row("Name") = obj.ToString
                Else
                    row("Name") = String.Empty
                End If

                obj = entry.Properties("uNCName").Value
                If obj IsNot Nothing Then
                    row("NetworkName") = obj.ToString
                Else
                    row("NetworkName") = String.Empty
                End If

                obj = entry.Properties("printShareName").Value
                If obj IsNot Nothing Then
                    row("ShareName") = obj.ToString
                Else
                    row("ShareName") = String.Empty
                End If

                obj = entry.Properties("description").Value
                If obj IsNot Nothing Then
                    row("Comments") = obj.ToString
                Else
                    row("Comments") = String.Empty
                End If

                obj = entry.Properties("driverName").Value
                If obj IsNot Nothing Then
                    row("Model") = obj.ToString
                Else
                    row("Model") = String.Empty
                End If

                obj = entry.Properties("location").Value
                If obj IsNot Nothing Then
                    row("Location") = obj.ToString
                Else
                    row("Location") = String.Empty
                End If

                obj = entry.Properties("portName").Value
                If obj IsNot Nothing Then
                    row("Port") = obj.ToString
                Else
                    row("Port") = String.Empty
                End If

                obj = entry.Properties("printLanguage").Value
                If obj IsNot Nothing Then
                    row("PrinterLanguage") = obj.ToString
                Else
                    row("PrinterLanguage") = String.Empty
                End If

                obj = entry.Properties("printMemory").Value
                If obj IsNot Nothing Then
                    row("InstalledMemory") = CInt(obj)
                Else
                    row("InstalledMemory") = 0
                End If

                obj = entry.Properties("printPagesPerMinute").Value
                If obj IsNot Nothing Then
                    row("PagesPerMinute") = CInt(obj)
                Else
                    row("PagesPerMinute") = 0
                End If

                obj = entry.Properties("printMaxResolutionSupported").Value
                If obj IsNot Nothing Then
                    row("MaxResolution") = CInt(obj)
                Else
                    row("MaxResolution") = 0
                End If

                obj = entry.Properties("printCollate").Value
                If obj IsNot Nothing Then
                    row("SupportsCollation") = CBool(obj)
                Else
                    row("SupportsCollation") = False
                End If

                obj = entry.Properties("printColor").Value
                If obj IsNot Nothing Then
                    row("SupportsColor") = CBool(obj)
                Else
                    row("SupportsColor") = False
                End If

                obj = entry.Properties("printDuplexSupported").Value
                If obj IsNot Nothing Then
                    row("SupportsDuplex") = CBool(obj)
                Else
                    row("SupportsDuplex") = False
                End If

                obj = entry.Properties("printStaplingSupported").Value
                If obj IsNot Nothing Then
                    row("SupportsStapling") = CBool(obj)
                Else
                    row("SupportsStapling") = False
                End If

                prtTable.Rows.Add(row)
                obj = Nothing
                entry.Dispose()
                entry = Nothing
            Next
            dirEntry.Dispose()
            dirSearcher.Dispose()
            resultCollection.Dispose()

        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try
        If sPrinter <> "noprinterdeclared" Then
            Return prtTable.Select("NetworkName = '" & sPrinter.Insert(sPrinter.IndexOf("\", 3), ".eolane.com") & "'").CopyToDataTable
        Else
            Return prtTable
        End If

    End Function

    Public Shared Sub COMM_APP_WEB_COPY_FICH_TO_CAB(sPrinter As String, sFichier As String)
        Dim admin_token As IntPtr
        Dim wid_current As WindowsIdentity = WindowsIdentity.GetCurrent()
        Dim wid_admin As WindowsIdentity = Nothing
        Dim wic As WindowsImpersonationContext = Nothing
        Try
            If LogonUser("ce_adminsv", "eolane", "Eol@ne14", 9, 0, admin_token) = 0 Then Throw New Exception("Log as n'a pas fonctionné")
            wid_admin = New WindowsIdentity(admin_token)
            wic = wid_admin.Impersonate()
            Dim dtPrinters As DataTable = COMM_APP_WEB_GET_NETW_PRIN_INFO(sPrinter)
            Dim sIP As String = Replace(dtPrinters(0)("Port").ToString, "IP_", "")
            Dim sAdresseConnection As String = ""
            COMM_APP_WEB_COPY_FILE(sFichier, My.Settings.RPTR_TPRR & "\" & Path.GetFileName(sFichier), True)
            sFichier = Path.GetFileName(sFichier)
            Select Case dtPrinters(0)("Model").ToString
                Case "CAB EOS1/300"
                    sAdresseConnection = "ftp://ftpcard:card@" & sIP
                    If Path.GetExtension(sFichier) = ".ttf" Or Path.GetExtension(sFichier) = ".TTF" Then
                        My.Computer.Network.UploadFile(My.Settings.RPTR_TPRR & "\" & sFichier, sAdresseConnection & "/fonts/" & sFichier)
                        'LOG_Msg(GetCurrentMethod, System.IO.File.Exists(sAdresseConnection & "/fonts/" & sFichier))
                    Else
                        My.Computer.Network.UploadFile(My.Settings.RPTR_TPRR & "\" & sFichier, sAdresseConnection & "/images/" & sFichier)
                        'LOG_Msg(GetCurrentMethod, System.IO.File.Exists(sAdresseConnection & "/images/" & sFichier))
                    End If
                Case "CAB A4 +/ 600K" Or "CAB MACH4/300"
                    sAdresseConnection = "ftp://root:0000@" & sIP
                    My.Computer.Network.UploadFile(My.Settings.RPTR_TPRR & "\" & sFichier, sAdresseConnection & "/card/" & sFichier)
                    'LOG_Msg(GetCurrentMethod, System.IO.File.Exists(sAdresseConnection & "/card/" & sFichier))
                Case Else
                    Throw New Exception("L'imprimante " & sPrinter & " n'est pas une imprimante CAB")
            End Select
        Catch ex_uriform As UriFormatException
            LOG_Erreur(GetCurrentMethod, ex_uriform.Message)
            Exit Sub
        Catch ex_IO As IOException
            LOG_Erreur(GetCurrentMethod, ex_IO.Message)
            Exit Sub
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Exit Sub
        End Try
    End Sub

    Public Shared Function COMM_APP_WEB_GET_CFGR_AFCG_PAGE_DEFA_TO_DT(sService_Session As String) As DataTable
        Dim sQuery As String = $"SELECT [NM_PAGE]
                                      ,[NM_URL_PAGE]
                                      ,ISNULL([NM_IMAG_URL],'') AS [NM_IMAG_URL]
                                  FROM [APP_WEB_ECO].[dbo].[DTM_REF_AFCG_PAGE_DEFA]
                                 WHERE [ID_SERV_SESS] = '{sService_Session}'"
        Try
            Using dt = SQL_SELE_TO_DT(sQuery, CS_APP_WEB_ECO)
                Return dt
            End Using
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try

        'LOG_Msg(GetCurrentMethod, "Les paramètres d'affichage ont été chagés pour le service ou la session " & sService_Session)

    End Function

    Public Shared Function COMM_APP_WEB_GET_DROI_PAGE(sUrl As String, sService As String, sSession As String) As Boolean
        'Dim sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog= APP_WEB_ECO;Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
        'Dim dt As New DataTable
        Try
            Using dt = SQL_SELE_TO_DT($"SELECT [NM_PAGE]
                                          FROM [dbo].[DTM_REF_AFCG_PAGE_DEFA]
                                         WHERE ([ID_SERV_SESS] = '{sService}' OR [ID_SERV_SESS] = '{sSession}') AND [NM_URL_PAGE] = '{sUrl}'", CS_APP_WEB_ECO)
                If dt Is Nothing Then Throw New Exception($"pas de droit pour {sService} d'accéder à l'adresse {sUrl}")
            End Using
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return False
        End Try

        'LOG_Msg(GetCurrentMethod, "Les paramètres d'affichage ont été chagés pour le service ou la session " & sService_Session)
        Return True
    End Function

    Public Shared Function COMM_APP_WEB_STRI_TRIM_LEFT(str As String, ipos As Integer) As String
        Return Right(str, Len(str) - ipos - 1)
    End Function

    Public Shared Function COMM_APP_WEB_STRI_TRIM_RIGHT(str As String, ipos As Integer) As String
        Return Left(str, Len(str) - ipos)
    End Function

    Public Shared Sub COMM_APP_WEB_ENVO_MAIL(sSMTP As String, sEMET As String, sDEST As String, sSUJE As String, sCONT As String, session As HttpSessionState, Optional sDEST_COPY As String = "vide", Optional sDEST_BLIN_COPY As String = "vide", Optional sCHEM_PJ As String = "vide")
        Dim client As New System.Net.Mail.SmtpClient
        Dim message As New System.Net.Mail.MailMessage
        Try
            client.Host = sSMTP 'définition du serveur smtp
            client.EnableSsl = True
            message.From = New System.Net.Mail.MailAddress(sEMET)
            Dim aDEST As String() = Split(sDEST, ";")
            For Each sELEM As String In aDEST
                message.To.Add(sELEM)
            Next
            If Not (sDEST_COPY <> "vide" Or sDEST_COPY Is Nothing) Then
                Dim asDEST_COPY As String() = Split(sDEST_COPY, ";")
                For Each sELEM As String In asDEST_COPY
                    message.CC.Add(sELEM)
                Next
            End If
            If Not (sDEST_BLIN_COPY <> "vide" Or sDEST_BLIN_COPY Is Nothing) Then
                Dim asDEST_BLIN_COPY As String() = Split(sDEST_BLIN_COPY, ";")
                For Each sELEM As String In asDEST_BLIN_COPY
                    message.Bcc.Add(sELEM)
                Next
            End If
            If sCHEM_PJ <> "vide" Then
                Dim asCHEM_PJ As String() = Split(sCHEM_PJ, ";")
                For Each sELEM As String In asCHEM_PJ
                    COMM_APP_WEB_COPY_FILE(sELEM, $"{My.Settings.RPTR_TPRR}\{Path.GetFileName(sELEM)}", True)
                    Dim item As New System.Net.Mail.Attachment($"{My.Settings.RPTR_TPRR}\{Path.GetFileName(sELEM)}")
                    message.Attachments.Add(item) 'ajout de la pièce jointe au message
                Next
            End If
            message.Subject = sSUJE
            message.IsBodyHtml = True
            sCONT = Replace(sCONT, vbLf, "<br>")
            sCONT = Replace(sCONT, vbCr, "<br>")
            message.Body =
           "<html xmlns:v=""urn:schemas-microsoft-com:vml"" xmlns:o=""urn:schemas-microsoft-com:office:office"" xmlns:w=""urn:schemas-microsoft-com:office:word"" xmlns:m=""http://schemas.microsoft.com/office/2004/12/omml"" xmlns=""http://www.w3.org/TR/REC-html40""><head><meta http-equiv=Content-Type content=""text/html; charset=iso-8859-1""><meta name=Generator content=""Microsoft Word 15 (filtered medium)""><!--[if !mso]><style>v\:* {behavior:url(#default#VML);}
o\:* {behavior:url(#default#VML);}
w\:* {behavior:url(#default#VML);}
.shape {behavior:url(#default#VML);}
</style><![endif]--><style><!--
/* Font Definitions */
@font-face
	{font-family:""Cambria Math"";
	panose-1:2 4 5 3 5 4 6 3 2 4;}
@font-face
	{font-family:Calibri;
	panose-1:2 15 5 2 2 2 4 3 2 4;}
@font-face
	{font-family:Verdana;
	panose-1:2 11 6 4 3 5 4 4 2 4;}
@font-face
	{font-family:""Gill Sans MT"";
	panose-1:2 11 5 2 2 1 4 2 2 3;}
@font-face
	{font-family:Times;
	panose-1:2 2 6 3 5 4 5 2 3 4;}
/* Style Definitions */
p.MsoNormal, li.MsoNormal, div.MsoNormal
	{margin:0cm;
	margin-bottom:.0001pt;
	font-size:11.0pt;
	font-family:""Calibri"",sans-serif;
	mso-fareast-language:EN-US;}
a:link, span.MsoHyperlink
	{mso-style-priority:99;
	color:#0563C1;
	text-decoration:underline;}
a:visited, span.MsoHyperlinkFollowed
	{mso-style-priority:99;
	color:#954F72;
	text-decoration:underline;}
span.EmailStyle17
	{mso-style-type:personal-compose;
	font-family:""Calibri"",sans-serif;
	color:windowtext;}
.MsoChpDefault
	{mso-style-type:export-only;
	font-family:""Calibri"",sans-serif;
	mso-fareast-language:EN-US;}
@page WordSection1
	{size:612.0pt 792.0pt;
	margin:70.85pt 70.85pt 70.85pt 70.85pt;}
div.WordSection1
	{page:WordSection1;}
--></style><!--[if gte mso 9]><xml>
<o:shapedefaults v:ext=""edit"" spidmax=""1026"" />
</xml><![endif]--><!--[if gte mso 9]><xml>
<o:shapelayout v:ext=""edit"">
<o:idmap v:ext=""edit"" data=""1"" />
</o:shapelayout></xml><![endif]--></head>
<body lang=FR link=""#0563C1"" vlink=""#954F72""><div class=WordSection1><div>
<p class=MsoNormal style='background:white;word-break:break-all;border:none;padding:0cm'>
<span style='font-size:11.0pt;font-family:""Calibri"",sans-serif;color:#000000;mso-fareast-language:EN-US'>" & sCONT & "<o:p></o:p></span>
           </p></div><p class=MsoNormal><span lang=EN-US><o:p>&nbsp;</o:p></span></p><p class=MsoNormal><span lang=EN-US><o:p>&nbsp;</o:p></span></p>
<table class=MsoNormalTable border=0 cellpadding=0 width=500 style='width:375.0pt'><tr><td valign=bottom style='padding:0cm 0cm 0cm 0cm'><p class=MsoNormal><b>
<span style='font-size:10.0pt;font-family:""Gill Sans MT"",sans-serif;color:#989000;mso-fareast-language:FR'>" & session("givenname") & " " & session("sn") & "</span></b><span style='font-size:12.0pt;font-family:""Times New Roman"",serif;mso-fareast-language:FR'><br></span><span style='font-size:10.0pt;font-family:""Gill Sans MT"",sans-serif;mso-fareast-language:FR'>" & session("title") & "</span><span style='font-size:12.0pt;font-family:""Times New Roman"",serif;mso-fareast-language:FR'><br></span><b><span style='font-size:10.0pt;font-family:""Verdana"",sans-serif;color:#002F60;mso-fareast-language:FR'>" & session("company") & "</span></b><span style='font-size:12.0pt;font-family:""Times New Roman"",serif;mso-fareast-language:FR'><br></span><span style='font-size:10.0pt;font-family:""Gill Sans MT"",sans-serif;mso-fareast-language:FR'>" & session("streetaddress") & ", </span><span style='font-size:12.0pt;font-family:""Times New Roman"",serif;mso-fareast-language:FR'><br></span><span style='font-size:10.0pt;font-family:""Gill Sans MT"",sans-serif;mso-fareast-language:FR'>" & session("postalcode") & " " & session("l") & ", " & session("st") & "</span><span style='font-size:12.0pt;font-family:""Times New Roman"",serif;mso-fareast-language:FR'><br></span><span style='font-size:10.0pt;font-family:""Gill Sans MT"",sans-serif;mso-fareast-language:FR'>Tél: " & session("telephonenumber") & "</span><span style='font-size:12.0pt;font-family:""Times New Roman"",serif;mso-fareast-language:FR'><br></span><span style='font-size:10.0pt;font-family:""Gill Sans MT"",sans-serif;mso-fareast-language:FR'><a href=""mailto:" & session("mail") & """><span style='color:blue'>mailto:" & session("mail") & "</span></a></span><span style='font-size:12.0pt;font-family:""Times New Roman"",serif;mso-fareast-language:FR'><br></span><span style='font-size:10.0pt;font-family:""Gill Sans MT"",sans-serif;mso-fareast-language:FR'>Web-site :<a href=""http://www.eolane.com""><span style='color:blue'>http://www.eolane.com</span></a></span><span style='font-size:12.0pt;font-family:""Times New Roman"",serif;mso-fareast-language:FR'><o:p></o:p></span></p></td></tr></table><p class=MsoNormal><span style='font-size:12.0pt;font-family:""Times New Roman"",serif;mso-fareast-language:FR'><br></span><span style='font-size:12.0pt;font-family:""Times New Roman"",serif;mso-fareast-language:FR'><img border=0 width=20 height=16 id=""_x0000_i1025"" src=""file:///C:\Users\so_spevi\AppData\Roaming\Microsoft\Signatures\Environnement.jpg""></span><i><span style='font-size:7.5pt;font-family:""Times"",serif;color:#00AE00;mso-fareast-language:FR'>Afin de contribuer au respect de l'environnement, merci de n'imprimer ce courriel que si nécessaire</span></i><o:p></o:p></p></div></body></html>
"


            client.Send(message) 'envoi du mail
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            'TODO traiter les erreurs
        End Try
    End Sub

    Public Shared Function COMM_APP_WEB_ETAT_CTRL(sPARA As String, Optional sNM_PAGE As String = Nothing) As DataTable
        Dim sQuery As String = ""
        'Dim dt, dtret As New DataTable
        Try
            If sNM_PAGE = Nothing Then sNM_PAGE = HttpContext.Current.CurrentHandler.ToString
            'chercher les param dans la base
            sQuery = $"SELECT [ID_CTRL],[VL_CTRL]
                         FROM [dbo].[V_DER_MAJ_DTM_REF_PARA_ETAT_CRTL]
                        WHERE [NM_PAGE] = '{sNM_PAGE}' AND REPLACE(REPLACE([PARA],'  ',''),' |','|') = '{Replace(Replace(Replace(sPARA, "'", "''"), "  ", ""), " |", "|")}'"
            Using dt = SQL_SELE_TO_DT(sQuery, CS_APP_WEB_ECO)
                Using dtret As New DataTable
                    For Each rdt As DataRow In dt.Rows
                        dtret.Columns.Add(rdt("ID_CTRL").ToString, Type.GetType("System.String"))
                    Next
                    dtret.Rows.Add()
                    For Each rdt As DataRow In dt.Rows
                        dtret.Rows(dtret.Rows.Count - 1)(rdt("ID_CTRL").ToString) = rdt("VL_CTRL").ToString
                    Next
                    Return dtret
                End Using
            End Using
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try

    End Function

    Public Shared Function COMM_APP_WEB_ENCO_2OF5_ITLV(chaine$, Optional key As Boolean = False) As String
        Dim i%, checksum&, dummy%
        COMM_APP_WEB_ENCO_2OF5_ITLV$ = ""
        If Len(chaine$) > 0 Then
            'Vérifier si caractères valides
            'Check for valid characters
            For i% = 1 To Len(chaine$)
                If Asc(Mid$(chaine$, i%, 1)) < 48 Or Asc(Mid$(chaine$, i%, 1)) > 57 Then Exit Function
            Next
            'Ajouter si nécessaire la clé
            'Add if necessary the checksum
            If key Then
                For i% = Len(chaine$) To 1 Step -2
                    checksum& = checksum& + Val(Mid$(chaine$, i%, 1))
                Next
                checksum& = checksum& * 3
                For i% = Len(chaine$) - 1 To 1 Step -2
                    checksum& = checksum& + Val(Mid$(chaine$, i%, 1))
                Next
                chaine$ = chaine$ & (10 - checksum& Mod 10) Mod 10
            End If
            'Vérifier si la longueur est paire
            'Check if the length is odd
            If Len(chaine$) \ 2 <> Len(chaine$) / 2 Then Exit Function
            'Calculer la chaine de code
            'Calculation of the code string
            For i% = 1 To Len(chaine$) Step 2
                dummy% = Val(Mid$(chaine$, i%, 2))
                dummy% = IIf(dummy% < 94, dummy% + 33, dummy% + 101)
                COMM_APP_WEB_ENCO_2OF5_ITLV = COMM_APP_WEB_ENCO_2OF5_ITLV & Chr(dummy%)
            Next
            'Ajoute START et STOP / Add START and STOP
            COMM_APP_WEB_ENCO_2OF5_ITLV = Chr(201) & COMM_APP_WEB_ENCO_2OF5_ITLV$ & Chr(202)
        End If
        LOG_Msg(GetCurrentMethod, COMM_APP_WEB_ENCO_2OF5_ITLV)
    End Function

    Public Shared Function COMM_APP_WEB_JS_IPSO_FICH_PDF(sfichier As String) As String
        Try
            Dim sscritp2 = "document.getElementById(""pdf"").onload = function() {window.frames[""pdf""].focus();
                                                                              window.frames[""pdf""].print();};"
            Dim sbjs As New StringBuilder
            sbjs.Append($"document.getElementById(""pdf"").src = ""{Path.GetFileName(sfichier)}"";{vbCrLf}")
            sbjs.Append(sscritp2)
            Return sbjs.ToString
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try

    End Function
End Class