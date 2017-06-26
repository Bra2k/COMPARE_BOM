Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports App_Web.Class_SQL
Imports App_Web.Class_COMM_APP_WEB
Imports App_Web.Class_SAP_DATA

Public Class ALMS_IMPORT
    Inherits System.Web.UI.Page
    'Dim sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=" & Replace(Replace(My.Computer.Name, "CEDB03", "ALMS_PROD_DEV"), "CEAPP03", "ALMS_PROD_PRD") & ";Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=7200;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
    Dim sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=ALMS_PROD_PRD;Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=7200;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    '    If Not IsPostBack Then
    '        Calendar_VAL.SelectedDate = Now
    '        Calendar_APP.SelectedDate = Now
    '    End If
    'End Sub
    Protected Sub Button_ENVOYER_Click(sender As Object, e As EventArgs) Handles Button_ENVOYER.Click

        Dim savePath As String = "c:\sources\temp_App_Web\"
        Dim dt, dtKNMT As DataTable
        Dim dt_VALI_TEST As New DataTable
        Dim ID_GENE As String = GENERATE_ID()
        Dim ref_sequ As String = "", vers_sequ As String = "", date_store_query As String = ""
        'Dim ref_sequ As String = FIND_REF_SEQU(FileUpload_ALMS.FileName)
        'Dim vers_sequ As String = FIND_VERS_SEQUENCE(FileUpload_ALMS.FileName)
        'Initialisation des variables date
        Dim date_val = COMM_APP_WEB_CONV_FORM_DATE(Calendar_VAL.SelectedDate, "yyyy-MM-dd")
        Dim date_app = COMM_APP_WEB_CONV_FORM_DATE(Calendar_APP.SelectedDate, "yyyy-MM-dd")
        'Dim date_store_query As String = "INSERT INTO [dbo].[DTM_SEQU_RFRC_LIST] ([NM_RFRC_SEQU], [NU_VERS_SEQU], [ID_RFRC_SEQU], [DT_APCT], [DT_VLDT], [BL_VLDT], [NM_VALI_ECO], [NM_RFRC_GAMM_ECO])
        '          VALUES ('" & ref_sequ & "', '" & vers_sequ & "', '" & ID_GENE & "', '" & date_val & "', '" & date_app & "', 1, '" & Session("displayname") & "', '" & DropDownList_Gamme.SelectedValue & "')"
        Try
            ref_sequ = FIND_REF_SEQU(FileUpload_ALMS.FileName)
            vers_sequ = FIND_VERS_SEQUENCE(FileUpload_ALMS.FileName)
            If ref_sequ Is Nothing Or vers_sequ Is Nothing Then Throw New Exception("L'extraction du nom du séquenceur n'a pas été exécutée")
            'date_store_query = "INSERT INTO [dbo].[DTM_SEQU_RFRC_LIST] ([NM_RFRC_SEQU], [NU_VERS_SEQU], [ID_RFRC_SEQU], [DT_APCT], [DT_VLDT], [BL_VLDT], [NM_VALI_ECO], [NM_RFRC_GAMM_ECO])
            ' VALUES ('" & ref_sequ & "', '" & vers_sequ & "', '" & ID_GENE & "', '" & date_val & "', '" & date_app & "', 1, '" & Session("displayname") & "', '" & DropDownList_Gamme.SelectedValue & "')"
            date_store_query = $"INSERT INTO [dbo].[DTM_SEQU_RFRC_LIST] ([NM_RFRC_SEQU], [NU_VERS_SEQU], [ID_RFRC_SEQU], [DT_APCT], [DT_VLDT], [BL_VLDT], [NM_VALI_ECO], [NM_RFRC_GAMM_ECO])
								     VALUES ('{ref_sequ}', '{vers_sequ}', '{ID_GENE}', '{date_val}', '{date_app}', 1, '{Session("displayname")}', '{DropDownList_Gamme.SelectedValue}')"

            If Not (FileUpload_ALMS.HasFile) Then Throw New Exception("pas de fichier sélectionné")
            savePath += Server.HtmlEncode(FileUpload_ALMS.FileName)
            FileUpload_ALMS.SaveAs(savePath)
            FileUpload_ALMS.Dispose()

            'If Right(savePath, 3) = "csv" Then
            If Right(savePath, 3) <> "csv" Then Throw New Exception("Le fichier séléctionné n'a pas l'extension [.csv] attendue")

            dt = COMM_APP_WEB_IMP_CSV_DT(savePath)
            If dt Is Nothing Then Throw New Exception("Erreur de lecture du fichier")
            GridView_CSV_ALMS.DataSource = dt
            GridView_CSV_ALMS.DataBind()
            'If CHECK_SEQU(ref_sequ, vers_sequ) = 0 Then
            If CHECK_SEQU(ref_sequ, vers_sequ) = 1 Then Throw New Exception("Le fichier sélectionné a déja été importé")
            'vérif id_test
            For ligne As Integer = 0 To dt.Rows.Count - 1
                If dt(ligne)(22).ToString = "" Then Continue For
                dtKNMT = SAP_DATA_READ_KNMT($"MATNR LIKE '{TextBox_CODE_ARTI.Text}%' AND KUNNR EQ '0000000451' AND VKORG EQ 'ORC3' AND VTWEG EQ 'CD'")
                If dtKNMT Is Nothing Then Continue For
                Dim Query_Verif As String = $"SELECT [NM_DSGT_TEST]
                                               FROM [dbo].[DTM_REF_ID_TEST] INNER JOIN [dbo].[DTM_REF_GAMM_ARTI] ON 
                                                    [dbo].[DTM_REF_ID_TEST].[ID_GAMM_PROD] = [dbo].[DTM_REF_GAMM_ARTI].[ID_GAMM_PROD] 
                                              WHERE [ID_TEST] = '{dt(ligne)(22).ToString}'
                                                AND [CD_ARTI_PROD]= '{dtKNMT(0)("KDMAT").ToString}'"

                dt_VALI_TEST = SQL_SELE_TO_DT(Query_Verif, sChaineConnexion)
                If Not dt_VALI_TEST Is Nothing Then
                    If dt_VALI_TEST(0)("NM_DSGT_TEST").ToString <> dt(ligne)(1).ToString Then Throw New Exception($"L'ID Test n°{dt(ligne)(22).ToString} nommé <<{dt(ligne)(1).ToString}>> existe déjà sous la dénomination <<{dt_VALI_TEST(0)("NM_DSGT_TEST").ToString}>> dans la base.")
                End If
            Next

            'SQL_REQ_ACT(date_store_query, sChaineConnexion)
            'Else
            '    MultiView_ALMS.SetActiveView(View_ALMS_DATA_ENTR)
            'Throw New Exception("Le fichier sélectionné a déja été importé")
            'Exit Sub
            'End If
            MultiView_ALMS.SetActiveView(GridView_ALMS)
            If PARSE_SEQU_ALMS(dt, FileUpload_ALMS.FileName, ID_GENE) = 1 Then Throw New Exception("Erreur d'importation des données dans la table détail")

            'Enregistrement dans la table list
            SQL_REQ_ACT(date_store_query, sChaineConnexion)
            LOG_MESS_UTLS(GetCurrentMethod, $"Le fichier {FileUpload_ALMS.FileName} a été importé")

            'Else
            '    Throw New Exception("Le fichier séléctionné n'a pas l'extension [.csv] attendue")
            'End If
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try
    End Sub

    Public Function FIND_REF_SEQU(FileName As String) As String
        'Dim myChars() As Char = {"_", "V", "v", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"}
        'Dim ref_Sequ As String = ""

        'For i As Integer = 0 To FileName.Length - 1
        '    If FileName(i) = "." Then
        '        ref_Sequ = ref_Sequ.TrimEnd(myChars)
        '        Return ref_Sequ
        '    End If
        '    ref_Sequ += FileName(i)
        'Next
        FileName = FileName.Replace(".csv", "")

        For i As Integer = FileName.Length - 2 To 0 Step -1
            If FileName.Substring(i, 2) = "_V" Then Return Left(FileName, i)
        Next
        Return Nothing

    End Function

    Public Function FIND_VERS_SEQUENCE(FileName As String) As String
        'Dim VERS_SEQU As String = ""
        'Dim VERS_SEQU_FINAL As String = ""

        'VERS_SEQU = FileName.Substring(FileName.Length - 7)
        'VERS_SEQU_FINAL = VERS_SEQU.Substring(0, 3)

        'Return VERS_SEQU_FINAL
        FileName = FileName.Replace(".csv", "")

        For i As Integer = FileName.Length - 2 To 0 Step -1
            If FileName.Substring(i, 2) = "_V" Then Return FileName.Substring(i + 2, FileName.Length - i - 2)
        Next
        Return Nothing

    End Function

    Public Function GENERATE_ID() As String
        Dim NU_GENE = DateTime.Now.ToString("yyyyMMddHHmmss")
        LOG_Msg(GetCurrentMethod, NU_GENE)
        Return NU_GENE
    End Function

    Public Function PARSE_SEQU_ALMS(dt As DataTable, savePath As String, ID_GENE As String) As Integer

        'Dim NU_ORDR_SEQU = ""           'ok
        'Dim LB_MESS_ERRE = ""           'ok
        'Dim NM_DRIV = ""                'ok
        'Dim LB_PARA_DRIV = ""           'ok
        'Dim ID_SAUT_PASS = ""           'ok
        'Dim ID_SAUT_FAIL = ""           'ok
        'Dim BL_EXCT = ""                'ok
        'Dim NU_LIMI_BASS = ""           'ok
        'Dim NU_LIMI_HAUT = ""           'ok
        'Dim BL_COMP_LIMI = ""           'ok
        'Dim BL_ERRE = ""                'ok
        'Dim BL_AFFI_FACE_AVAN_LABV = "" 'ok
        'Dim BL_RAPP_ACTI = ""           'ok
        'Dim ID_TEST = ""                'ok
        'Dim BL_SORT_FCGF = ""           'ok
        'Dim NU_PHAS_FCGF = ""           'ok
        'Dim NM_PHAS_FCGF = ""           'ok
        'Dim NU_SOUS_PHAS_FCGF = ""      'ok
        'Dim NM_SOUS_PHAS_FCGF = ""      'ok
        'Dim NM_UNIT = ""                'ok
        'Dim BL_CH_VAL = ""              'ok
        'Dim DESI_TEST = ""                  'ok
        Dim sCD_ARTI_CLIE As String = ""
        Dim dt_GAMM, dtKNMT As New DataTable
        Try
            dtKNMT = SAP_DATA_READ_KNMT($"MATNR LIKE '{TextBox_CODE_ARTI.Text}%' AND KUNNR EQ '0000000451' AND VKORG EQ 'ORC3' AND VTWEG EQ 'CD'")
            If Not dtKNMT Is Nothing Then
                sCD_ARTI_CLIE = dtKNMT(0)("KDMAT").ToString
            Else
                If TextBox_CODE_ARTI.Text.Contains("AUT") Then
                    sCD_ARTI_CLIE = TextBox_CODE_ARTI.Text
                Else
                    Throw New Exception("Le code article client n'a pas été renseigné dans SAP")
                End If
            End If

            dt_GAMM = SQL_SELE_TO_DT($"SELECT [ID_GAMM_PROD]
                                        FROM [dbo].[DTM_REF_GAMM_ARTI]
                                       WHERE [CD_ARTI_PROD] = '{sCD_ARTI_CLIE}'", sChaineConnexion)
            If dt_GAMM Is Nothing Then Throw New Exception("Erreur lors de l'extraction de la gamme")

            For ligne As Integer = 0 To dt.Rows.Count - 1
                'For col As Integer = 0 To dt.Columns.Count - 1
                '    dt(ligne)(col) = Replace(dt(ligne)(col).ToString, "'", "''")
                '    dt(ligne)(col) = Replace(dt(ligne)(col).ToString, ",", ".")
                '    Select Case col
                '        Case 0
                '            NU_ORDR_SEQU = dt(ligne)(col).ToString
                '        Case 1
                '            DESI_TEST = dt(ligne)(col).ToString
                '        Case 2
                '            LB_MESS_ERRE = dt(ligne)(col).ToString
                '        Case 3
                '            NM_DRIV = dt(ligne)(col).ToString
                '        Case 4
                '            LB_PARA_DRIV = dt(ligne)(col).ToString
                '        Case 5
                '            ID_SAUT_PASS = dt(ligne)(col).ToString
                '        Case 6
                '            ID_SAUT_FAIL = dt(ligne)(col).ToString
                '        Case 7
                '            BL_EXCT = dt(ligne)(col).ToString
                '        Case 12
                '            NU_LIMI_BASS = dt(ligne)(col).ToString
                '        Case 14
                '            NU_LIMI_HAUT = dt(ligne)(col).ToString
                '        Case 15
                '            BL_COMP_LIMI = dt(ligne)(col).ToString
                '        Case 16
                '            BL_ERRE = dt(ligne)(col).ToString
                '        Case 17
                '            BL_AFFI_FACE_AVAN_LABV = dt(ligne)(col).ToString
                '        Case 18
                '            BL_RAPP_ACTI = dt(ligne)(col).ToString
                '        Case 22
                '            ID_TEST = dt(ligne)(col).ToString
                '        Case 23
                '            BL_SORT_FCGF = dt(ligne)(col).ToString
                '        Case 24
                '            NU_PHAS_FCGF = dt(ligne)(col).ToString
                '        Case 25
                '            NM_PHAS_FCGF = dt(ligne)(col).ToString
                '        Case 26
                '            NU_SOUS_PHAS_FCGF = dt(ligne)(col).ToString
                '        Case 27
                '            NM_SOUS_PHAS_FCGF = dt(ligne)(col).ToString
                '        Case 28
                '            NM_UNIT = dt(ligne)(col).ToString
                '        Case 29
                '            BL_CH_VAL = dt(ligne)(col).ToString
                '    End Select
                'Next
                'Dim getGamme As String = "SELECT [ID_GAMM_PROD] 
                '                        FROM [dbo].[V_ID_GAMM_CD_ARTI_ECO]
                '                       WHERE [CD_ARTI_PROD_ECO]= '" & TextBox_CODE_ARTI.Text & "' "

                'dt_GAMM = SQL_SELE_TO_DT(getGamme, sChaineConnexion)
                'If dt_GAMM Is Nothing Then Throw New Exception("Erreur lors de l'extraction de la gamme")

                'Try

                '    Catch ex As Exception
                'LOG_Erreur(GetCurrentMethod, 
                '    End Try
                '    If dt_GAMM Is Nothing Then
                '    Return 1
                'Else
                Dim saveIdTest As String = $"INSERT INTO [dbo].[DTM_REF_ID_TEST] ([ID_TEST], [NM_DSGT_TEST], [ID_GAMM_PROD], [BL_ATVT], [DT_CREA])
                                             VALUES ('{Replace(dt(ligne)(22).ToString, "'", "''")}', '{Replace(dt(ligne)(1).ToString, "'", "''")}', '{dt_GAMM(0)("ID_GAMM_PROD")}', 1 , GETDATE())"

                'Dim saveCsv As String = "INSERT INTO [dbo].[DTM_SEQU_RFRC_DETA] ([ID_RFRC_SEQU], [NU_ORDR_SEQU] ,[LB_MESS_ERRE], [NM_DRIV], [LB_PARA_DRIV], [ID_SAUT_PASS], [ID_SAUT_FAIL], [BL_EXCT], [NU_LIMI_BASS], [NU_LIMI_HAUT], [BL_COMP_LIMI], [BL_ERRE], [BL_AFFI_FACE_AVAN_LABV], [BL_RAPP_ACTI], [ID_TEST], [BL_SORT_FCGF], [NU_PHAS_FCGF], [NM_PHAS_FCGF], [NU_SOUS_PHAS_FCGF], [NM_SOUS_PHAS_FCGF], [NM_UNIT], [BL_CH_VAL])
                ' VALUES ('" & ID_GENE & "', '" & NU_ORDR_SEQU & "', '" & LB_MESS_ERRE & "', '" & NM_DRIV & "', '" & LB_PARA_DRIV & "', '" & ID_SAUT_PASS & "', '" & ID_SAUT_FAIL & "', '" & BL_EXCT & "', '" & NU_LIMI_BASS & "', '" & NU_LIMI_HAUT & "', '" & BL_COMP_LIMI & "', '" & BL_ERRE & "' , '" & BL_AFFI_FACE_AVAN_LABV & "', '" & BL_RAPP_ACTI & "', '" & ID_TEST & "', '" & BL_SORT_FCGF & "', '" & NU_PHAS_FCGF & "', '" & NM_PHAS_FCGF & "', '" & NU_SOUS_PHAS_FCGF & "', '" & NM_SOUS_PHAS_FCGF & "', '" & NM_UNIT & "', '" & BL_CH_VAL & "')"

                Dim saveCsv As String = $"INSERT INTO [dbo].[DTM_SEQU_RFRC_DETA] 
                                                    ([ID_RFRC_SEQU]
                                                    ,[NU_ORDR_SEQU] 
                                                    ,[LB_MESS_ERRE]
                                                    ,[NM_DRIV]
                                                    ,[LB_PARA_DRIV]
                                                    ,[ID_SAUT_PASS]
                                                    ,[ID_SAUT_FAIL]
                                                    ,[BL_EXCT]
                                                    ,[NU_LIMI_BASS]
                                                    ,[NU_LIMI_HAUT]
                                                    ,[BL_COMP_LIMI]
                                                    ,[BL_ERRE]
                                                    ,[BL_AFFI_FACE_AVAN_LABV]
                                                    ,[BL_RAPP_ACTI]
                                                    ,[ID_TEST]
                                                    ,[BL_SORT_FCGF]
                                                    ,[NU_PHAS_FCGF]
                                                    ,[NM_PHAS_FCGF]
                                                    ,[NU_SOUS_PHAS_FCGF]
                                                    ,[NM_SOUS_PHAS_FCGF]
                                                    ,[NM_UNIT]
                                                    ,[BL_CH_VAL])
								             VALUES ('{ID_GENE}'
                                                    , '{Replace(dt(ligne)(0).ToString, "'", "''")}'
                                                    , '{Replace(dt(ligne)(2).ToString, "'", "''")}'
                                                    , '{Replace(dt(ligne)(3).ToString, "'", "''")}'
                                                    , '{Replace(dt(ligne)(4).ToString, "'", "''")}'
                                                    , '{Replace(dt(ligne)(5).ToString, "'", "''")}'
                                                    , '{Replace(dt(ligne)(6).ToString, "'", "''")}'
                                                    , '{Replace(dt(ligne)(7).ToString, "'", "''")}'
                                                    , '{Replace(Replace(dt(ligne)(12).ToString, "'", "''"), ",", ".")}'
                                                    , '{Replace(Replace(dt(ligne)(14).ToString, "'", "''"), ",", ".")}'
                                                    , '{Replace(dt(ligne)(15).ToString, "'", "''")}'
                                                    , '{Replace(dt(ligne)(16).ToString, "'", "''")}' 
                                                    , '{Replace(dt(ligne)(17).ToString, "'", "''")}'
                                                    , '{Replace(dt(ligne)(18).ToString, "'", "''")}'
                                                    , '{Replace(dt(ligne)(22).ToString, "'", "''")}'
                                                    , '{Replace(dt(ligne)(23).ToString, "'", "''")}'
                                                    , '{Replace(dt(ligne)(24).ToString, "'", "''")}'
                                                    , '{Replace(dt(ligne)(25).ToString, "'", "''")}'
                                                    , '{Replace(dt(ligne)(26).ToString, "'", "''")}'
                                                    , '{Replace(dt(ligne)(27).ToString, "'", "''")}'
                                                    , '{Replace(dt(ligne)(28).ToString, "'", "''")}'
                                                    , '{Replace(dt(ligne)(29).ToString, "'", "''")}')"
                'Try
                SQL_REQ_ACT(saveCsv, sChaineConnexion)
                SQL_REQ_ACT(saveIdTest, sChaineConnexion)
                'Catch ex As Exception
                '    LOG_Erreur(GetCurrentMethod, ex.Message)
                '    Return 1
                'End Try
                'End If
            Next
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return 1
        End Try
        Return 0

    End Function

    Public Function CHECK_SEQU(NM_RFRC_SEQU As String, NU_VERS_SEQU As String) As Integer
        Dim sQuery1 = $"SELECT * 
                         FROM [dbo].[DTM_SEQU_RFRC_LIST] 
                        WHERE [NM_RFRC_SEQU] ='{NM_RFRC_SEQU}' AND [NU_VERS_SEQU]= '{NU_VERS_SEQU}'"
        Dim dtSequ1 As New DataTable
        Try
            dtSequ1 = SQL_SELE_TO_DT(sQuery1, sChaineConnexion)
            If dtSequ1 Is Nothing Then Throw New Exception("Le fichier sélectionné a déja été importé")
            Return 1
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return 0
        End Try

    End Function

    Protected Sub TextBox_CODE_ARTI_TextChanged(sender As Object, e As EventArgs) Handles TextBox_CODE_ARTI.TextChanged
        'Dim dt_MAPL, dt_PLAS, dt_PLPO, dt_CRHD, dt_OP As New DataTable

        Try
            'As New DataTable
            Using dt_OP As New DataTable
                With dt_OP
                    .Columns.Add("op", Type.GetType("System.String"))
                    .Columns.Add("VORNR", Type.GetType("System.String"))
                    Using dt_MAPL = SAP_DATA_READ_MAPL($"MATNR LIKE '{Trim(TextBox_CODE_ARTI.Text)}%' AND WERKS EQ 'DI31' AND LOEKZ NE 'X'")
                        If dt_MAPL Is Nothing Then
                            If TextBox_CODE_ARTI.Text.Contains("AUT") Then 'exception sur les %AUT% , faire apparaître <<Autotest banc (OP:10)>> dans la DropDownList_Gamme
                                .Rows.Add()
                                .Rows(0)("op") = "AUTOTEST BANC (OP:10)"
                                .Rows(0)("VORNR") = "10"
                            Else
                                Throw New Exception("Pas de gamme pour le code article entré")
                            End If
                        Else
                            Using dt_PLAS = SAP_DATA_READ_PLAS($"PLNNR EQ '{dt_MAPL(0)("PLNNR").ToString}' AND PLNAL EQ '{dt_MAPL(0)("PLNAL").ToString}' AND LOEKZ NE 'X'")
                                If dt_PLAS Is Nothing Then Throw New Exception("Pas de gamme pour le code article entré")
                                For Each rPLAS As DataRow In dt_PLAS.Rows
                                    Using dt_PLPO = SAP_DATA_READ_PLPO($"PLNNR EQ '{rPLAS("PLNNR").ToString}' AND PLNKN EQ '{rPLAS("ZAEHL").ToString}'")
                                        If dt_PLPO Is Nothing Then Continue For
                                        .Rows.Add()
                                        .Rows(.Rows.Count - 1)("op") = $"{Trim(dt_PLPO(0)("LTXA1").ToString)} (OP:{Convert.ToDecimal(dt_PLPO(0)("VORNR")).ToString})"
                                        .Rows(.Rows.Count - 1)("VORNR") = dt_PLPO(0)("VORNR").ToString
                                    End Using
                                Next
                            End Using
                            .DefaultView.Sort = "VORNR ASC"
                            Using dt_OP_SORT = .DefaultView.ToTable
                                With DropDownList_Gamme
                                    .DataSource = dt_OP
                                    .DataTextField = "op" '"VORNR"
                                    .DataValueField = "op"
                                    .DataBind()
                                End With
                            End Using
                        End If
                    End Using
                End With
            End Using
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Exit Sub
        End Try
    End Sub
End Class