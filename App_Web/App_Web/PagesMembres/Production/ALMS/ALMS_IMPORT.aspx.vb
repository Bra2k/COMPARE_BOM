Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports App_Web.Class_SQL
Imports App_Web.Class_COMM_APP_WEB

Public Class ALMS_IMPORT
    Inherits System.Web.UI.Page
    Dim sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=" & Replace(Replace(My.Computer.Name, "CEDB03", "ALMS_PROD_DEV"), "CEAPP03", "ALMS_PROD_PRD") & ";Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=7200;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"

    Protected Sub Button_ENVOYER_Click(sender As Object, e As EventArgs) Handles Button_ENVOYER.Click

        Dim savePath As String = "c:\sources\temp_App_Web\"
        Dim dt As DataTable

        Dim ref_sequ As String = FIND_REF_SEQU(FileUpload_ALMS.FileName)
        Dim vers_sequ As String = FIND_VERS_SEQU(FileUpload_ALMS.FileName)
        'Initialisation des variables date
        Dim date_val = COMM_APP_WEB_CONV_FORM_DATE(Calendar_VAL.SelectedDate, "yyyy-MM-dd")
        Dim date_app = COMM_APP_WEB_CONV_FORM_DATE(Calendar_APP.SelectedDate, "yyyy-MM-dd")
        Dim date_store_query As String = "INSERT INTO [ALMS_PROD_DEV].[dbo].[DTM_SEQU_RFRC_LIST]
									            ([NM_RFRC_SEQU], [NU_VERS_SEQU] ,[DT_APCT], [DT_VLDT])
								          VALUES ('" & ref_sequ & "', '" & vers_sequ & "', '" & date_val & "', '" & date_app & "')"

        Try
            If Not (FileUpload_ALMS.HasFile) Then Throw New Exception("pas de fichier sélectionné")
            savePath += Server.HtmlEncode(FileUpload_ALMS.FileName)
            FileUpload_ALMS.SaveAs(savePath)
            FileUpload_ALMS.Dispose()

            If Right(savePath, 3) = "csv" Then
                dt = COMM_APP_WEB_IMP_CSV_DT(savePath)
                GridView_CSV_ALMS.DataSource = dt
                GridView_CSV_ALMS.DataBind()
                Try
                    SQL_REQ_ACT(date_store_query, sChaineConnexion)
                Catch ex As Exception
                    LOG_Erreur(GetCurrentMethod, ex.Message)
                End Try
                MultiView_ALMS.SetActiveView(GridView_ALMS)
                PARSE_SEQU_ALMS(dt, FileUpload_ALMS.FileName)
                'LOG_Msg(GetCurrentMethod, dt.Rows(18)(24).ToString()
            Else
                Throw New Exception("Le fichier séléctionné n'a pas l'extension [.csv] attendue")
            End If
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try
    End Sub

    Public Function FIND_REF_SEQU(FileName As String) As String

        Dim myChars() As Char = {"_", "V", "v", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"}
        Dim ref_Sequ As String = ""

        For i As Integer = 0 To FileName.Length - 1
            If FileName(i) = "." Then
                ref_Sequ = ref_Sequ.TrimEnd(myChars)
                Return ref_Sequ
            End If
            ref_Sequ += FileName(i)
        Next

        Return ref_Sequ
    End Function

    Public Function FIND_VERS_SEQU(FileName As String) As String

        Dim r As New Regex("(?<num>[0-9]+)")
        Dim m As Match = r.Match(FileName)
        Dim vers_sequ As String

        If m.Success Then
            vers_sequ = m.Groups("num").ToString
        Else
            vers_sequ = ""
        End If

        Return vers_sequ
    End Function

    Public Function PARSE_SEQU_ALMS(dt As DataTable, savePath As String) As Integer
        Dim vers_sequ = FIND_VERS_SEQU(savePath)
        Dim ref_sequ = FIND_REF_SEQU(savePath)

        Dim NU_ORDR_SEQU = "" 'ok
        Dim LB_MESS_ERRE = "" 'ok
        Dim NM_DRIV = "" 'ok
        Dim LB_PARA_DRIV = "" 'ok
        Dim ID_SAUT_PASS = "" 'ok
        Dim ID_SAUT_FAIL = "" 'ok
        Dim BL_EXCT = "" 'ok
        Dim NU_LIMI_BASS = "" 'ok
        Dim NU_LIMI_HAUT = "" 'ok
        Dim BL_COMP_LIMI = "" 'ok
        Dim BL_ERRE = "" 'ok
        Dim BL_AFFI_FACE_AVAN_LABV = "" 'ok
        Dim BL_RAPP_ACTI = "" 'ok
        Dim ID_TEST = "" 'ok
        Dim BL_SORT_FCGF = "" 'ok
        Dim NU_PHAS_FCGF = "" 'ok
        Dim NM_PHAS_FCGF = "" 'ok
        Dim NU_SOUS_PHAS_FCGF = "" 'ok
        Dim NM_SOUS_PHAS_FCGF = "" 'ok
        Dim NM_UNIT = "" 'ok

        For ligne As Integer = 0 To dt.Rows.Count - 1
            For col As Integer = 0 To dt.Columns.Count - 1
                Select Case col
                    Case 0
                        NU_ORDR_SEQU = dt(ligne)(col).ToString
                    Case 2
                        LB_MESS_ERRE = dt(ligne)(col).ToString
                    Case 3
                        NM_DRIV = dt(ligne)(col).ToString
                    Case 4
                        LB_PARA_DRIV = dt(ligne)(col).ToString
                    Case 5
                        ID_SAUT_PASS = dt(ligne)(col).ToString
                    Case 6
                        ID_SAUT_FAIL = dt(ligne)(col).ToString
                    Case 7
                        BL_EXCT = dt(ligne)(col).ToString
                    Case 12
                        NU_LIMI_BASS = dt(ligne)(col).ToString
                    Case 14
                        NU_LIMI_HAUT = dt(ligne)(col).ToString
                    Case 15
                        BL_COMP_LIMI = dt(ligne)(col).ToString
                    Case 16
                        BL_ERRE = dt(ligne)(col).ToString
                    Case 17
                        BL_AFFI_FACE_AVAN_LABV = dt(ligne)(col).ToString
                    Case 18
                        BL_RAPP_ACTI = dt(ligne)(col).ToString
                    Case 22
                        ID_TEST = dt(ligne)(col).ToString
                    Case 23
                        BL_SORT_FCGF = dt(ligne)(col).ToString
                    Case 24
                        NU_PHAS_FCGF = dt(ligne)(col).ToString
                    Case 25
                        NM_PHAS_FCGF = dt(ligne)(col).ToString
                    Case 26
                        NU_SOUS_PHAS_FCGF = dt(ligne)(col).ToString
                    Case 27
                        NM_SOUS_PHAS_FCGF = dt(ligne)(col).ToString
                    Case 28
                        NM_UNIT = dt(ligne)(col).ToString
                End Select
            Next
            Dim saveCsv As String = "INSERT INTO [ALMS_PROD_DEV].[dbo].[DTM_SEQU_RFRC_DETA]
									            ([NM_RFRC_SEQU], [NU_VERS_SEQU], [NU_ORDR_SEQU] ,[LB_MESS_ERRE], [NM_DRIV], [LB_PARA_DRIV], [ID_SAUT_PASS], [ID_SAUT_FAIL], [BL_EXCT], [NU_LIMI_BASS], [NU_LIMI_HAUT], [BL_COMP_LIMI], [BL_ERRE], [BL_AFFI_FACE_AVAN_LABV], [BL_RAPP_ACTI], [ID_TEST], [BL_SORT_FCGF], [NU_PHAS_FCGF], [NM_PHAS_FCGF], [NU_SOUS_PHAS_FCGF], [NM_SOUS_PHAS_FCGF], [NM_UNIT])
								     VALUES ('" & ref_sequ & "', '" & vers_sequ & "', '" & NU_ORDR_SEQU & "', '" & LB_MESS_ERRE & "', '" & NM_DRIV & "', '" & LB_PARA_DRIV & "', '" & ID_SAUT_PASS & "', '" & ID_SAUT_FAIL & "', '" & BL_EXCT & "', '" & NU_LIMI_BASS & "', '" & NU_LIMI_HAUT & "', '" & BL_COMP_LIMI & "', '" & BL_ERRE & "' , '" & BL_AFFI_FACE_AVAN_LABV & "', '" & BL_RAPP_ACTI & "', '" & ID_TEST & "', '" & BL_SORT_FCGF & "', '" & NU_PHAS_FCGF & "', '" & NM_PHAS_FCGF & "', '" & NU_SOUS_PHAS_FCGF & "', '" & NM_SOUS_PHAS_FCGF & "', '" & NM_UNIT & "')"
            Try
                SQL_REQ_ACT(saveCsv, sChaineConnexion)
            Catch ex As Exception
                LOG_Erreur(GetCurrentMethod, ex.Message)
                Return 1
            End Try
        Next

        Return 0
    End Function
End Class