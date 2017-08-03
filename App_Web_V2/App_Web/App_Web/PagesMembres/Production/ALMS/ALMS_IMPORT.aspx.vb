Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports App_Web.Class_SQL
Imports App_Web.Class_COMM_APP_WEB
Imports App_Web.Class_SAP_DATA
Imports System.IO

Public Class ALMS_IMPORT
    Inherits System.Web.UI.Page

    Protected Sub Button_ENVOYER_Click(sender As Object, e As EventArgs) Handles Button_ENVOYER.Click

        Dim savePath As String = $"{My.Settings.RPTR_TPRR}\"
        Dim ref_sequ As String = "", vers_sequ As String = "", date_store_query As String = ""
        Try
            Dim ID_GENE As String = GENERATE_ID()
            Dim date_val = COMM_APP_WEB_CONV_FORM_DATE(Calendar_VAL.SelectedDate, "yyyy-MM-dd")
            Dim date_app = COMM_APP_WEB_CONV_FORM_DATE(Calendar_APP.SelectedDate, "yyyy-MM-dd")
            ref_sequ = FIND_REF_SEQU(FileUpload_ALMS.FileName)
            vers_sequ = FIND_VERS_SEQUENCE(FileUpload_ALMS.FileName)
            If ref_sequ Is Nothing Or vers_sequ Is Nothing Then Throw New Exception("L'extraction du nom du séquenceur n'a pas été exécutée")
            date_store_query = $"INSERT INTO [dbo].[DTM_SEQU_RFRC_LIST] ([NM_RFRC_SEQU], [NU_VERS_SEQU], [ID_RFRC_SEQU], [DT_APCT], [DT_VLDT], [BL_VLDT], [NM_VALI_ECO], [NM_RFRC_GAMM_ECO])
								      VALUES ('{ref_sequ}', '{vers_sequ}', '{ID_GENE}', '{date_val}', '{date_app}', 1, '{Session("displayname")}', '{DropDownList_Gamme.SelectedValue}')"

            If Not (FileUpload_ALMS.HasFile) Then Throw New Exception("pas de fichier sélectionné")
            savePath += Server.HtmlEncode(FileUpload_ALMS.FileName)
            FileUpload_ALMS.SaveAs(savePath)
            FileUpload_ALMS.Dispose()

            If Path.GetExtension(savePath) <> ".csv" Then Throw New Exception($"Le fichier séléctionné ({Path.GetExtension(savePath)}) n'a pas l'extension [.csv] attendue")

            Using dt = COMM_APP_WEB_IMP_CSV_DT(savePath)
                If dt Is Nothing Then Throw New Exception("Erreur de lecture du fichier")
                GridView_CSV_ALMS.DataSource = dt
                GridView_CSV_ALMS.DataBind()
                If CHECK_SEQU(ref_sequ, vers_sequ) = 1 Then Throw New Exception("Le fichier sélectionné a déja été importé")
                'vérif id_test
                For ligne As Integer = 0 To dt.Rows.Count - 1
                    If dt(ligne)(22).ToString = "" Then Continue For
                    Using dtKNMT = SAP_DATA_READ_KNMT($"MATNR LIKE '{TextBox_CODE_ARTI.Text}%' AND KUNNR EQ '0000000451' AND VKORG EQ 'ORC3' AND VTWEG EQ 'CD'")
                        If dtKNMT Is Nothing Then Continue For
                        Dim Query_Verif As String = $"SELECT [NM_DSGT_TEST]
                                                        FROM [dbo].[DTM_REF_ID_TEST] INNER JOIN [dbo].[DTM_REF_GAMM_ARTI] ON 
                                                             [dbo].[DTM_REF_ID_TEST].[ID_GAMM_PROD] = [dbo].[DTM_REF_GAMM_ARTI].[ID_GAMM_PROD] 
                                                       WHERE [ID_TEST] = '{dt(ligne)(22).ToString}'
                                                         AND [CD_ARTI_PROD]= '{dtKNMT(0)("KDMAT").ToString}'"
                        Using dt_VALI_TEST = SQL_SELE_TO_DT(Query_Verif, CS_ALMS_PROD_DEV)
                            If Not dt_VALI_TEST Is Nothing Then
                                If dt_VALI_TEST(0)("NM_DSGT_TEST").ToString <> dt(ligne)(1).ToString Then Throw New Exception($"L'ID Test n°{dt(ligne)(22).ToString} nommé <<{dt(ligne)(1).ToString}>> existe déjà sous la dénomination <<{dt_VALI_TEST(0)("NM_DSGT_TEST").ToString}>> dans la base.")
                            End If
                        End Using
                    End Using
                Next

                MultiView_ALMS.SetActiveView(GridView_ALMS)
                If PARSE_SEQU_ALMS(dt, FileUpload_ALMS.FileName, ID_GENE) = 1 Then Throw New Exception("Erreur d'importation des données dans la table détail")
            End Using
            'Enregistrement dans la table list
            SQL_REQ_ACT(date_store_query, CS_ALMS_PROD_DEV)
            LOG_MESS_UTLS(GetCurrentMethod, $"Le fichier {FileUpload_ALMS.FileName} a été importé")
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try
    End Sub

    Public Function FIND_REF_SEQU(FileName As String) As String

        FileName = FileName.Replace(".csv", "")

        For i As Integer = FileName.Length - 2 To 0 Step -1
            If FileName.Substring(i, 2) = "_V" Then Return Left(FileName, i)
        Next
        Return Nothing

    End Function

    Public Function FIND_VERS_SEQUENCE(FileName As String) As String

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

        Dim sCD_ARTI_CLIE As String = ""
        Try
            Using dtKNMT = SAP_DATA_READ_KNMT($"MATNR LIKE '{TextBox_CODE_ARTI.Text}%' AND KUNNR EQ '0000000451' AND VKORG EQ 'ORC3' AND VTWEG EQ 'CD'")
                If Not dtKNMT Is Nothing Then
                    sCD_ARTI_CLIE = dtKNMT(0)("KDMAT").ToString
                Else
                    If TextBox_CODE_ARTI.Text.Contains("AUT") Then
                        sCD_ARTI_CLIE = TextBox_CODE_ARTI.Text
                    Else
                        Throw New Exception("Le code article client n'a pas été renseigné dans SAP")
                    End If
                End If
            End Using
            Using dt_GAMM = SQL_SELE_TO_DT($"SELECT [ID_GAMM_PROD]
                                               FROM [dbo].[DTM_REF_GAMM_ARTI]
                                              WHERE [CD_ARTI_PROD] = '{sCD_ARTI_CLIE}'", CS_ALMS_PROD_DEV)
                If dt_GAMM Is Nothing Then Throw New Exception("Erreur lors de l'extraction de la gamme")

                For ligne As Integer = 0 To dt.Rows.Count - 1
                    Dim saveIdTest As String = $"INSERT INTO [dbo].[DTM_REF_ID_TEST] ([ID_TEST], [NM_DSGT_TEST], [ID_GAMM_PROD], [BL_ATVT], [DT_CREA])
                                                      VALUES ('{Replace(dt(ligne)(22).ToString, "'", "''")}', '{Replace(dt(ligne)(1).ToString, "'", "''")}', '{dt_GAMM(0)("ID_GAMM_PROD")}', 1 , GETDATE())"

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
                    SQL_REQ_ACT(saveCsv, CS_ALMS_PROD_DEV)
                    SQL_REQ_ACT(saveIdTest, CS_ALMS_PROD_DEV)
                Next
            End Using
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message)
            Return 1
        End Try
        Return 0

    End Function

    Public Function CHECK_SEQU(NM_RFRC_SEQU As String, NU_VERS_SEQU As String) As Integer
        Dim sQuery1 = $"SELECT * 
                         FROM [dbo].[DTM_SEQU_RFRC_LIST] 
                        WHERE [NM_RFRC_SEQU] ='{NM_RFRC_SEQU}' AND [NU_VERS_SEQU]= '{NU_VERS_SEQU}'"
        Try
            Using dtSequ1 = SQL_SELE_TO_DT(sQuery1, CS_ALMS_PROD_DEV)
                If dtSequ1 Is Nothing Then Throw New Exception("Le fichier sélectionné a déja été importé")
                Return 1
            End Using
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message)
            Return 0
        End Try

    End Function

    Protected Sub TextBox_CODE_ARTI_TextChanged(sender As Object, e As EventArgs) Handles TextBox_CODE_ARTI.TextChanged

        Try
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
                        End If
                        Using dt_OP_SORT = .DefaultView.ToTable
                            With DropDownList_Gamme
                                .DataSource = dt_OP
                                .DataTextField = "op" '"VORNR"
                                .DataValueField = "op"
                                .DataBind()
                            End With
                        End Using
                    End Using
                End With
            End Using
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message)
            Exit Sub
        End Try
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("displayname") = "" Then
                Context.GetOwinContext().Authentication.SignOut(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie)
            Else
                If App_Web.Class_COMM_APP_WEB.COMM_APP_WEB_GET_DROI_PAGE(HttpContext.Current.CurrentHandler.ToString, Session("department"), Session("User_Name")) = False Then Response.Redirect("~/PagesMembres/RDRC_PAGE_MEMB.aspx")
            End If
        End If
    End Sub
End Class