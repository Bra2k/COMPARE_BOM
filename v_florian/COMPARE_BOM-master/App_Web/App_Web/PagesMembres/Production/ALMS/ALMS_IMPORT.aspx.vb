Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports App_Web.Class_SQL
Imports App_Web.Class_COMM_APP_WEB
Imports App_Web.Class_SAP_DATA

Public Class ALMS_IMPORT
    Inherits System.Web.UI.Page
    Dim sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=" & Replace(Replace(My.Computer.Name, "CEDB03", "ALMS_PROD_DEV"), "CEAPP03", "ALMS_PROD_PRD") & ";Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=7200;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"

    Protected Sub Button_ENVOYER_Click(sender As Object, e As EventArgs) Handles Button_ENVOYER.Click

        Dim savePath As String = "c:\sources\temp_App_Web\"
        Dim dt As DataTable
        Dim ID_GENE As String = GENERATE_ID()

        Dim ref_sequ As String = FIND_REF_SEQU(FileUpload_ALMS.FileName)
        Dim vers_sequ As String = FIND_VERS_SEQUENCE(FileUpload_ALMS.FileName)
        'Initialisation des variables date
        Dim date_val = COMM_APP_WEB_CONV_FORM_DATE(Calendar_VAL.SelectedDate, "yyyy-MM-dd")
        Dim date_app = COMM_APP_WEB_CONV_FORM_DATE(Calendar_APP.SelectedDate, "yyyy-MM-dd")
        Dim date_store_query As String = "INSERT INTO [ALMS_PROD_DEV].[dbo].[DTM_SEQU_RFRC_LIST]
									            ([NM_RFRC_SEQU], [NU_VERS_SEQU], [ID_RFRC_SEQU], [DT_APCT], [DT_VLDT])
								          VALUES ('" & ref_sequ & "', '" & vers_sequ & "', '" & ID_GENE & "', '" & date_val & "', '" & date_app & "')"
        Try
            If Not (FileUpload_ALMS.HasFile) Then Throw New Exception("pas de fichier sélectionné")
            savePath += Server.HtmlEncode(FileUpload_ALMS.FileName)
            FileUpload_ALMS.SaveAs(savePath)
            FileUpload_ALMS.Dispose()

            If Right(savePath, 3) = "csv" Then
                dt = COMM_APP_WEB_IMP_CSV_DT(savePath)
                GridView_CSV_ALMS.DataSource = dt
                GridView_CSV_ALMS.DataBind()
                If CHECK_SEQU(ref_sequ, vers_sequ) = 0 Then
                    Try
                        SQL_REQ_ACT(date_store_query, sChaineConnexion)
                    Catch ex As Exception
                        LOG_Erreur(GetCurrentMethod, ex.Message)
                    End Try
                Else
                    MultiView_ALMS.SetActiveView(View_ALMS_DATA_ENTR)
                    Throw New Exception("Le fichier sélectionné a déja été importé")
                    Exit Sub
                End If
                MultiView_ALMS.SetActiveView(GridView_ALMS)
                PARSE_SEQU_ALMS(dt, FileUpload_ALMS.FileName, ID_GENE)
            Else
                Throw New Exception("Le fichier séléctionné n'a pas l'extension [.csv] attendue")
            End If
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Exit Sub
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

    Public Function FIND_VERS_SEQUENCE(FileName As String) As String
        Dim VERS_SEQU As String = ""
        Dim VERS_SEQU_FINAL As String = ""

        VERS_SEQU = FileName.Substring(FileName.Length - 7)
        VERS_SEQU_FINAL = VERS_SEQU.Substring(0, 3)

        Return VERS_SEQU_FINAL
    End Function

    Public Function GENERATE_ID() As String
        Dim NU_GENE = DateTime.Now.ToString("yyyyMMddHHmmss")
        LOG_Msg(GetCurrentMethod, NU_GENE)
        Return NU_GENE
    End Function

    Public Function PARSE_SEQU_ALMS(dt As DataTable, savePath As String, ID_GENE As String) As Integer

        Dim NU_ORDR_SEQU = ""           'ok
        Dim LB_MESS_ERRE = ""           'ok
        Dim NM_DRIV = ""                'ok
        Dim LB_PARA_DRIV = ""           'ok
        Dim ID_SAUT_PASS = ""           'ok
        Dim ID_SAUT_FAIL = ""           'ok
        Dim BL_EXCT = ""                'ok
        Dim NU_LIMI_BASS = ""           'ok
        Dim NU_LIMI_HAUT = ""           'ok
        Dim BL_COMP_LIMI = ""           'ok
        Dim BL_ERRE = ""                'ok
        Dim BL_AFFI_FACE_AVAN_LABV = "" 'ok
        Dim BL_RAPP_ACTI = ""           'ok
        Dim ID_TEST = ""                'ok
        Dim BL_SORT_FCGF = ""           'ok
        Dim NU_PHAS_FCGF = ""           'ok
        Dim NM_PHAS_FCGF = ""           'ok
        Dim NU_SOUS_PHAS_FCGF = ""      'ok
        Dim NM_SOUS_PHAS_FCGF = ""      'ok
        Dim NM_UNIT = ""                'ok
        Dim BL_CH_VAL = ""              'ok
        Dim DESI_TEST = ""                  'ok
        Dim dt_VALI_TEST As New DataTable

        For ligne As Integer = 0 To dt.Rows.Count
            Dim Query_Verif = "SELECT [ID_TEST] FROM [ALMS_PROD_DEV].[dbo].[DTM_REF_ID_TEST] WHERE [ID_TEST]= '" & dt(ligne)(22).ToString & "'"

            Try
                dt_VALI_TEST = SQL_SELE_TO_DT(Query_Verif, sChaineConnexion)
                If Not dt_VALI_TEST Is Nothing Then
                    'MultiView_ALMS.SetActiveView(View_ALMS_DATA_ENTR)
                    LOG_Erreur(GetCurrentMethod, "ID Test existe déja dans la base")
                    Return 1
                End If
            Catch ex As Exception
                LOG_Erreur(GetCurrentMethod, "L'execution de la requete a échoué")
                'MultiView_ALMS.SetActiveView(View_ALMS_DATA_ENTR)
                Return 1
            End Try
        Next

        For ligne As Integer = 0 To dt.Rows.Count - 1
            For col As Integer = 0 To dt.Columns.Count - 1
                dt(ligne)(col) = Replace(dt(ligne)(col).ToString, "'", "''")
                dt(ligne)(col) = Replace(dt(ligne)(col).ToString, ",", ".")
                Select Case col
                    Case 0
                        NU_ORDR_SEQU = dt(ligne)(col).ToString
                    Case 1
                        DESI_TEST = dt(ligne)(col).ToString
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
                    Case 29
                        BL_CH_VAL = dt(ligne)(col).ToString
                End Select
            Next
            Dim getGamme As String = "SELECT [ID_GAMM_PROD] FROM [ALMS_PROD_DEV].[dbo].[V_ID_GAMM_CD_ARTI_ECO]
                                      WHERE [CD_ARTI_PROD_ECO]= '" & TextBox_CODE_ARTI.Text & "' "

            Dim dt_GAMM As New DataTable

            Try
                dt_GAMM = SQL_SELE_TO_DT(getGamme, sChaineConnexion)
            Catch ex As Exception
                LOG_Erreur(GetCurrentMethod, "Erreur lors de l'extraction de la gamme")
            End Try
            If dt_GAMM Is Nothing Then
                Return 1
            Else
                Dim saveIdTest As String = "INSERT INTO [ALMS_PROD_DEV].[dbo].[DTM_REF_ID_TEST]
                                                ([ID_TEST], [NM_DSGT_TEST], [ID_GAMM_PROD], [BL_ATVT], [DT_CREA])
                                             VALUES ('" & ID_TEST & "', '" & DESI_TEST & "', '" & dt_GAMM(0)("ID_GAMM_PROD") & "', 1 , GETDATE())"

                Dim saveCsv As String = "INSERT INTO [ALMS_PROD_DEV].[dbo].[DTM_SEQU_RFRC_DETA]
									            ([ID_RFRC_SEQU], [NU_ORDR_SEQU] ,[LB_MESS_ERRE], [NM_DRIV], [LB_PARA_DRIV], [ID_SAUT_PASS], [ID_SAUT_FAIL], [BL_EXCT], [NU_LIMI_BASS], [NU_LIMI_HAUT], [BL_COMP_LIMI], [BL_ERRE], [BL_AFFI_FACE_AVAN_LABV], [BL_RAPP_ACTI], [ID_TEST], [BL_SORT_FCGF], [NU_PHAS_FCGF], [NM_PHAS_FCGF], [NU_SOUS_PHAS_FCGF], [NM_SOUS_PHAS_FCGF], [NM_UNIT], [BL_CH_VAL])
								         VALUES ('" & ID_GENE & "', '" & NU_ORDR_SEQU & "', '" & LB_MESS_ERRE & "', '" & NM_DRIV & "', '" & LB_PARA_DRIV & "', '" & ID_SAUT_PASS & "', '" & ID_SAUT_FAIL & "', '" & BL_EXCT & "', '" & NU_LIMI_BASS & "', '" & NU_LIMI_HAUT & "', '" & BL_COMP_LIMI & "', '" & BL_ERRE & "' , '" & BL_AFFI_FACE_AVAN_LABV & "', '" & BL_RAPP_ACTI & "', '" & ID_TEST & "', '" & BL_SORT_FCGF & "', '" & NU_PHAS_FCGF & "', '" & NM_PHAS_FCGF & "', '" & NU_SOUS_PHAS_FCGF & "', '" & NM_SOUS_PHAS_FCGF & "', '" & NM_UNIT & "', '" & BL_CH_VAL & "')"
                Try
                    SQL_REQ_ACT(saveCsv, sChaineConnexion)
                    SQL_REQ_ACT(saveIdTest, sChaineConnexion)
                Catch ex As Exception
                    LOG_Erreur(GetCurrentMethod, ex.Message)
                    Return 1
                End Try
            End If
        Next
        Return 0
    End Function

    Public Function CHECK_SEQU(NM_RFRC_SEQU As String, NU_VERS_SEQU As String) As Integer
        Dim sQuery1 = "SELECT * FROM [ALMS_PROD_DEV].[dbo].[DTM_SEQU_RFRC_LIST] WHERE [NM_RFRC_SEQU] ='" & NM_RFRC_SEQU & "' AND [NU_VERS_SEQU]= '" & NU_VERS_SEQU & "'"
        Dim dtSequ1 As New DataTable

        dtSequ1 = SQL_SELE_TO_DT(sQuery1, sChaineConnexion)
        If dtSequ1 Is Nothing Then
            Return 0
        End If
        Return 1
    End Function

    Public Shared Function SAP_DATA_READ_MAPL(cARTI As String) As DataTable
        Dim dtMAPL As New DataTable

        Try
            dtMAPL = SAP_DATA_READ_TBL("MAPL", "|", "", "PLNNR", "MATNR LIKE '" & cARTI & "%' AND PLNTY EQ 'N' AND WERKS EQ 'DI31'")
            If dtMAPL Is Nothing Then Throw New Exception("L'article '" & cARTI & "' est inconnu ou ne possède pas de gamme")
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try
        LOG_Msg(GetCurrentMethod, "La liste GrpDeGamme a été extraite de SAP")
        Return dtMAPL
    End Function

    Public Shared Function SAP_DATA_READ_PLKO(V_PLNNR As String) As DataTable
        Dim dtPLKO As New DataTable

        Try
            dtPLKO = SAP_DATA_READ_TBL("PLKO", "|", "", "PLNAL", "PLNNR EQ '" & V_PLNNR & "' AND STATU EQ '4'")
            If dtPLKO Is Nothing Then Throw New Exception("Erreur sur le PLNNR")
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try
        LOG_Msg(GetCurrentMethod, "La liste a été extraite de SAP")
        Return dtPLKO
    End Function

    Public Function SAP_DATA_CARO_ROUT_READ(V_MATNR As String, V_PLNNR As String, V_PLNAL As String) As DataTable
        Dim oSAP, RFC, oT_CARO_ROUT_READ As New Object
        Dim dt_T_CARO_ROUT_READ As New DataTable

        'LOG_Msg(GetCurrentMethod, "Result :'" & V_MATNR & "' '" & V_PLNNR & "' '" & V_PLNAL & "'")

        'Try
        'oSAP = SAP_DATA_CONN()
        'RFC = oSAP.Add("CARO_ROUTING_READ")
        'RFC.exports("DATE_FROM") = COMM_APP_WEB_CONV_FORM_DATE(Calendar_APP.SelectedDate, "ddMMyyyy")
        'RFC.exports("DATE_TO") = COMM_APP_WEB_CONV_FORM_DATE(Calendar_VAL.SelectedDate, "ddMMyyyy")
        'RFC.exports("PLNTY") = "N"
        'If V_PLNNR <> "" Then
        'RFC.exports("PLNNR") = V_PLNNR
        'End If
        'If V_PLNAL <> "" Then
        'RFC.exports("PLNAL") = V_PLNAL
        'End If
        'If V_MATNR <> "" Then
        'RFC.exports("MATNR") = V_MATNR
        'End If
        'oT_CARO_ROUT_READ = RFC.Tables("OPR_TAB")
        'If RFC.Call <> -1 Then Throw New Exception(RFC.exception)
        '    For Each o_COL_T_LOG_CONF As Object In oT_CARO_ROUT_READ.Columns
        '        dt_T_CARO_ROUT_READ.Columns.Add(o_COL_T_LOG_CONF.Name, Type.GetType("System.String"))
        '    Next
        '    For iRowIndex = 1 To oT_CARO_ROUT_READ.RowCount
        '        dt_T_CARO_ROUT_READ.Rows.Add()
        '        For iIndex = 1 To oT_CARO_ROUT_READ.ColumnCount - 1
        '            dt_T_CARO_ROUT_READ.Rows(dt_T_CARO_ROUT_READ.Rows.Count - 1)(iIndex - 1) = oT_CARO_ROUT_READ.Value(iRowIndex, iIndex)
        '        Next
        '    Next
        'Catch ex As Exception
        '    LOG_Erreur(GetCurrentMethod, ex.Message)
        'Finally
        '    oSAP = SAP_DATA_DECO(oSAP)
        'End Try
        Return dt_T_CARO_ROUT_READ
    End Function

    Protected Sub TextBox_CODE_ARTI_TextChanged(sender As Object, e As EventArgs) Handles TextBox_CODE_ARTI.TextChanged
        Dim dt_MAPL, dt_PLKO As New DataTable

        Try
            dt_MAPL = SAP_DATA_READ_MAPL(TextBox_CODE_ARTI.Text)
            If dt_MAPL Is Nothing Then
                Throw New Exception("Pas de gamme pour le code article entré")
            End If
            dt_PLKO = SAP_DATA_READ_PLKO(dt_MAPL(0)("PLNNR").ToString)
            If dt_PLKO Is Nothing Then
                Throw New Exception("Pas de Gamme")
            End If
            If dt_PLKO.Rows.Count > 1 Then
                Throw New Exception("Gamme indéterminée")
            End If
            'GridView_OPE_ALMS.DataSource = dt_MAPL
            'GridView_OPE_ALMS.DataBind()
            'GridView_OPE2_ALMS.DataSource = dt_PLKO
            'GridView_OPE2_ALMS.DataBind()
            '1 - LIRE TABLE MAPL (SAP_READ_MAPL) : entree => MATNR(code article) WERKS(di31) PLNTY(N)
            'sortie => PLNNR PLNAL
            Dim dt_CARO_ROUT_READ = SAP_DATA_CARO_ROUT_READ(TextBox_CODE_ARTI.Text, dt_MAPL(0)("PLNNR").ToString, dt_PLKO(0)("PLNAL").ToString)
            'DropDownList_Gamme.DataSource = dt_CARO_ROUT_READ
            'GridView_OPE_ALMS.DataSource = dt_CARO_ROUT_READ
            'GridView_OPE_ALMS.DataBind()
            'dt_CARO_ROUT_READ(row)("VORNR")
            '2 - UTILISER CARO_ROUTING_READ() : entree => PLNNR PLNAL MATNR PLNTY(N)
            'sortie => récupérer dans OPR_TAB => VORNR LTXA1
            'intégration (OP:100)
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Exit Sub
        End Try
    End Sub
End Class