Imports Microsoft.Office.Interop.Excel
Imports App_Web.LOG
Imports System.Data
Imports System.Reflection.MethodBase
Imports System.IO
Imports App_Web.Class_COMM_APP_WEB
Imports System.Security.Permissions
Imports System.Threading
Public Class Class_EXCEL

    Public Shared Function EXCE_OUVR() As Application

        Dim iCountTimeout As Integer = 1
        Dim oXL As Object

        Try
            While 1
                If Process.GetProcessesByName("EXCEL").Count > 0 Then
                    System.Threading.Thread.Sleep(1000)
                Else
                    Exit While
                End If
                If iCountTimeout = 1000 Then Throw New Exception("Les process Excel ont été nettoyé sur le serveur après 1000 secondes d'inactivité")
                iCountTimeout += 1
            End While
        Catch ex As Exception
            For Each p As Process In Process.GetProcessesByName("EXCEL")
                p.Kill()
            Next
            LOG_Erreur(GetCurrentMethod, ex.Message)
        Finally
            oXL = New Application
            oXL.Visible = False
            oXL.DisplayAlerts = False
        End Try

        'LOG_Msg(GetCurrentMethod, "L'objet excel a été créé en " & iCountTimeout & " secondes.")
        Return oXL

    End Function
    Public Shared Sub EXCE_XLS2CSV(sFichierXLS As String, sFichierCSV As String)

        Dim oXL As Application = EXCE_OUVR()
        Dim oWB As Workbook = oXL.Workbooks.Open(sFichierXLS, 2, False)

        Try
            If File.Exists(sFichierCSV) Then
                LOG_Msg(GetCurrentMethod, "Fichier " & sFichierCSV & " déjà existant : supprimé")
                File.Delete(sFichierCSV)
            End If

            For Each oWS As Worksheet In oWB.Sheets
                oWS.Select()
                oWB.SaveAs(sFichierCSV.Insert(Len(sFichierCSV) - 4, "_" & oWS.Name), XlFileFormat.xlCSVMSDOS)
            Next
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Exit Sub
        Finally
            For Each oWS As Worksheet In oWB.Sheets
                oWS = COMM_APP_WEB_RELE_OBJ(oWS)
            Next
            oWB = COMM_APP_WEB_RELE_OBJ(oWB)

            oXL.Quit()
            oXL = COMM_APP_WEB_RELE_OBJ(oXL)
        End Try

        LOG_Msg(GetCurrentMethod, "Le fichier " & sFichierXLS & " a été converti")

    End Sub

    Public Shared Function EXCE_LIST_ONGL(sFichier As String) As Data.DataTable

        Dim oXL As Application = EXCE_OUVR()
        Dim oWB As Workbook = oXL.Workbooks.Open(sFichier, 2, False)
        Dim dt As New Data.DataTable

        Try
            dt.Columns.Add("Onglet", Type.GetType("System.String"))
            For Each oWS As Worksheet In oWB.Sheets
                dt.Rows.Add()
                dt.Rows(dt.Rows.Count - 1)("Onglet") = oWS.Name
            Next
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        Finally
            For Each oWS As Worksheet In oWB.Sheets
                oWS = COMM_APP_WEB_RELE_OBJ(oWS)
            Next
            oWB = COMM_APP_WEB_RELE_OBJ(oWB)

            oXL.Quit()
            oXL = COMM_APP_WEB_RELE_OBJ(oXL)
        End Try

        LOG_Msg(GetCurrentMethod, "Les onglets du fichier " & sFichier & " ont été extraits correctement.")
        Return dt

    End Function

    Public Shared Function EXCE_DATA_DT(sFICH As String, sONGL As String) As Data.DataTable

        Dim oXL As Application = EXCE_OUVR()
        Dim oWB As Workbook = oXL.Workbooks.Open(sFICH, 2, False)
        Dim oWS As Worksheet = oWB.Worksheets(sONGL)
        Dim dt As New Data.DataTable
        Dim COLO_COUNT, LIGN_COUNT, iLign_EX As Integer

        Try
            COLO_COUNT = oWS.UsedRange.Columns.Count
            LIGN_COUNT = oWS.UsedRange.Rows.Count
            For iCOLO As Integer = 1 To COLO_COUNT
                dt.Columns.Add(iCOLO, Type.GetType("System.String"))
            Next
            For iLIGN As Integer = 1 To LIGN_COUNT
                dt.Rows.Add()
                'LOG_Msg(GetCurrentMethod, iLIGN)
                For iCOLO As Integer = 1 To COLO_COUNT
                    dt.Rows(dt.Rows.Count - 1)(iCOLO - 1) = oWS.Cells(iLIGN, iCOLO).Value
                Next
                iLign_EX = iLIGN
            Next
        Catch ex_TAE As ThreadInterruptedException
            For iLIGN As Integer = iLign_EX To LIGN_COUNT
                dt.Rows.Add()
                'LOG_Msg(GetCurrentMethod, ex_TAE.Message & " " & iLIGN)
                For iCOLO As Integer = 1 To COLO_COUNT
                    dt.Rows(dt.Rows.Count - 1)(iCOLO - 1) = oWS.Cells(iLIGN, iCOLO).Value
                Next
            Next
            Return dt
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing

        Finally
            oWS = COMM_APP_WEB_RELE_OBJ(oWS)
            oWB = COMM_APP_WEB_RELE_OBJ(oWB)

            oXL.Quit()
            oXL = COMM_APP_WEB_RELE_OBJ(oXL)
        End Try

        LOG_Msg(GetCurrentMethod, "Les données de l'onglet " & sONGL & " du fichier " & sFICH & " ont été extraites correctement.")
        Return dt

    End Function

End Class
