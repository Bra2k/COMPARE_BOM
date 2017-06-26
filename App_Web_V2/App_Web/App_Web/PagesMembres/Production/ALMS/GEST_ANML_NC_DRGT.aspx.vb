Imports App_Web.Class_SQL
Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports App_Web.Class_SAP_DATA
Imports App_Web.Class_DIG_FACT
Imports App_Web.Class_DIG_FACT_SQL
'Imports System.Data.SqlClient

Public Class GEST_ANML_NC_DRGT
    Inherits System.Web.UI.Page

    'Dim CS_MES_Digital_Factory_ALMS As String = $"Data Source=cedb03,1433;Initial Catalog={Replace(Replace(My.Computer.Name, "CEDB03", "ALMS_PROD_DEV"), "CEAPP03", "ALMS_PROD_PRD")};Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=7200;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
    'Dim CS_MES_Digital_Factory As String = $"Data Source=cedb03,1433;Initial Catalog={Replace(Replace(My.Computer.Name, "CEDB03", "MES_Digital_Factory_DEV"), "CEAPP03", "MES_Digital_Factory")};Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=7200;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
    'Dim CS_MES_Digital_Factory_ALMS As String = "Data Source=cedb03,1433;Initial Catalog=ALMS_PROD_PRD;Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=7200;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
    'Dim CS_MES_Digital_Factory As String = "Data Source=cedb03,1433;Initial Catalog=MES_Digital_Factory;Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=7200;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
    'Dim sQuery As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'vérification de l'habilitation
        'Dim dt As New DataTable
        Try
            Dim sQuery = $"SELECT [DT_ATVT]
                             FROM [dbo].[DTM_REF_OPRT]
                            WHERE [ID_MTCL_ECO] = {Session("matricule")} AND [BL_ATVT] = 1 AND [ID_HBLT_ALMS] = 1"
            Using dt = SQL_SELE_TO_DT(sQuery, CS_ALMS_PROD_PRD)
                If dt Is Nothing Then Throw New Exception($"{Session("displayname")} n'a pas le droit d'accéder à cette page")
            End Using
            LOG_MESS_UTLS(GetCurrentMethod, sQuery)

        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Response.Redirect("~/PagesMembres/RDRC_PAGE_MEMB.aspx")
            Exit Sub
        End Try
    End Sub

    Protected Sub Button_AJOU_TYPE_NC_Click(sender As Object, e As EventArgs) Handles Button_AJOU_TYPE_NC.Click
        MultiView_ANML.SetActiveView(View_NOUV_TYPE_NC)
    End Sub

    Protected Sub Button_ADD_TYPE_CAUS_Click(sender As Object, e As EventArgs) Handles Button_ADD_TYPE_CAUS.Click
        MultiView_ANML.SetActiveView(View_NOUV_TYPE_CAUS)
    End Sub

    Protected Sub TextBox_NU_SER_SPFE_TextChanged(sender As Object, e As EventArgs) Handles TextBox_NU_SER_SPFE.TextChanged
        'Dim dt, dt_inci As New DataTable
        Try
            'recherche OF
            Dim sQuery = $"SELECT [NU_OF]
                             FROM [dbo].[V_LIAIS_NU_SER]
                            WHERE [NU_SER_CLIE] = '{TextBox_NU_SER_SPFE.Text}'"
            Using dt = SQL_SELE_TO_DT(sQuery, CS_MES_Digital_Factory)
                If dt Is Nothing Then Throw New Exception($"L'OF du numéro de série {TextBox_NU_SER_SPFE.Text} n'a pas été retrouvé dans la base.")
                Label_NU_OF.Text = dt(0)("NU_OF").ToString
            End Using

            'recherche code article
            Using dt = SAP_DATA_LECT_OF(Label_NU_OF.Text)
                If dt Is Nothing Then Throw New Exception($"Pas d'info dans SAP retrouvées sur l'OF {Label_NU_OF.Text} n'a pas été retrouvé dans la base.")
                Label_CD_ARTI_ECO.Text = Trim(dt(0)("CD_ARTI_ECO").ToString)

                'désignation 
                Label_NM_DSGT_ECO.Text = Trim(dt(0)("NM_DSGT_ARTI").ToString)
            End Using

            'code article ALMS
            Using dt = SAP_DATA_READ_KNMT($"MATNR LIKE '{Label_CD_ARTI_ECO.Text}%' AND KUNNR EQ '0000000451' AND VKORG EQ 'ORC3' AND VTWEG EQ 'CD'")
                If dt Is Nothing Then Throw New Exception($"L'équivalent de {Label_CD_ARTI_ECO.Text} en code article ALMS n'a pas été retrouvé dans SAP.")
                Label_CD_ARTI_ALMS.Text = Trim(dt(0)("KDMAT").ToString)
            End Using

            'Numéro d'incident
            sQuery = $"SELECT [NU_INCI]
                        FROM [dbo].[V_NC_LIST_NU_INCI_NU_SER]
                       WHERE [NU_SER_SPFE] = '{TextBox_NU_SER_SPFE.Text}'"
            Using dt_nu_inci = SQL_SELE_TO_DT(sQuery, CS_ALMS_PROD_PRD)
                If Not dt_nu_inci Is Nothing Then 'si existant
                    Label_NU_INCI.Text = dt_nu_inci(0)("NU_INCI").ToString
                    sQuery = $"SELECT [ID_TYPE_NC]
                                ,REPLACE([NM_TYPE_NC], 'cd_arti', '{Label_CD_ARTI_ALMS.Text}') AS [NM_TYPE_NC]
                                ,[NM_DCPO_NC]
                                ,[ID_RFRC_SEQU]
                                ,[NM_RFRC_SEQU]
                                ,[NU_PHAS_FCGF]
                                ,[NM_PHAS]
                                ,[NU_SOUS_PHAS_FCGF]
                                ,[NM_SOUS_PHAS]
                                ,[LB_DCPT_STAT]
                            FROM [dbo].[V_NC_LIST_INFO_NU_INCI]
                           WHERE [NU_INCI] = {Label_NU_INCI.Text}
                             And [NU_SER_SPFE] = '{TextBox_NU_SER_SPFE.Text}'"
                    Using dt_inci = SQL_SELE_TO_DT(sQuery, CS_ALMS_PROD_PRD)
                        If dt_inci Is Nothing Then Throw New Exception($"Pas de séquence passée mauvaise n'a été retrouvé dans la base pour le numéro de série {TextBox_NU_SER_SPFE.Text}.")
                        Label_ID_STAT.Text = dt_inci(0)("LB_DCPT_STAT").ToString

                        '   recherche séquence
                        sQuery = $"SELECT [NM_RFRC_SEQU]
                                ,[ID_RFRC_SEQU]
                            FROM [dbo].[V_NC_LIST_SEQU_FAIL_NU_SER]
                           WHERE [NU_SER_SPFE] = '{TextBox_NU_SER_SPFE.Text}'"
                        Using dt2 = SQL_SELE_TO_DT(sQuery, CS_ALMS_PROD_PRD)
                            If dt2 Is Nothing Then Throw New Exception($"Pas de séquence passée mauvaise n'a été retrouvé dans la base pour le numéro de série {TextBox_NU_SER_SPFE.Text}.")
                            With DropDownList_SEQU_FAIL
                                .DataSource = dt2
                                .DataTextField = "NM_RFRC_SEQU"
                                .DataValueField = "ID_RFRC_SEQU"
                                .DataBind()
                                .Enabled = True
                                .SelectedValue = dt_inci(0)("ID_RFRC_SEQU").ToString
                            End With
                        End Using
                        '   recherche phase
                        sQuery = $"SELECT [NU_PHAS_FCGF]
                            ,[NM_PHAS]
                        FROM [dbo].[V_NC_LIST_PHAS_FAIL_NU_SER_ID_SEQ]
                       WHERE [NU_SER_SPFE] = '{TextBox_NU_SER_SPFE.Text}'
                         AND [ID_RFRC_SEQU] = {DropDownList_SEQU_FAIL.SelectedValue}
                      Group BY [NU_PHAS_FCGF], [NM_PHAS]"
                        Using dt2 = SQL_SELE_TO_DT(sQuery, CS_ALMS_PROD_PRD)
                            If dt2 Is Nothing Then Throw New Exception($"Pas de phase mauvaise trouvée pour le numéro de série {TextBox_NU_SER_SPFE.Text}.")
                            With DropDownList_PHAS_FAIL
                                .DataSource = dt2
                                .DataTextField = "NM_PHAS"
                                .DataValueField = "NU_PHAS_FCGF"
                                .DataBind()
                                .Enabled = True
                                .SelectedValue = dt_inci(0)("NU_PHAS_FCGF").ToString
                            End With
                        End Using
                        '   recherche sous-phase
                        sQuery = $"SELECT [NU_SOUS_PHAS_FCGF]
                            ,[NM_SOUS_PHAS]
                        FROM [dbo].[V_NC_LIST_PHAS_FAIL_NU_SER_ID_SEQ]
                       WHERE [NU_SER_SPFE] = '{TextBox_NU_SER_SPFE.Text}'
                         AND [ID_RFRC_SEQU] = {DropDownList_SEQU_FAIL.SelectedValue}
                         And [NU_PHAS_FCGF] = {DropDownList_PHAS_FAIL.SelectedValue}
                      GROUP BY [NU_SOUS_PHAS_FCGF],[NM_SOUS_PHAS],[NU_PHAS_FCGF],[NM_PHAS]"
                        Using dt2 = SQL_SELE_TO_DT(sQuery, CS_ALMS_PROD_PRD)
                            If dt2 Is Nothing Then Throw New Exception($"Pas de sous-phase mauvaise trouvée pour le numéro de série {TextBox_NU_SER_SPFE.Text}.")
                            With DropDownList_SOUS_PHAS_FAIL
                                .DataSource = dt2
                                .DataTextField = "NM_SOUS_PHAS"
                                .DataValueField = "NU_SOUS_PHAS_FCGF"
                                .DataBind()
                                .Enabled = True
                                .SelectedValue = dt_inci(0)("NU_SOUS_PHAS_FCGF").ToString
                            End With
                        End Using
                        '   recherche type NC
                        sQuery = $"SELECT [ID_TYPE_NC]
                            ,REPLACE([NM_TYPE_NC], 'cd_arti', '{Label_CD_ARTI_ALMS.Text}') AS [NM_TYPE_NC]
                        FROM [dbo].[V_NC_LIST_TYPE_NC_ID_SEQ]
                       WHERE [NU_PHAS_FCGF] = {DropDownList_PHAS_FAIL.SelectedValue}
                         And [NU_SOUS_PHAS_FCGF] = {DropDownList_SOUS_PHAS_FAIL.SelectedValue}
                         AND [ID_RFRC_SEQU] = {DropDownList_SEQU_FAIL.SelectedValue}"
                        Using dt2 = SQL_SELE_TO_DT(sQuery, CS_ALMS_PROD_PRD)
                            If dt2 Is Nothing Then Throw New Exception("Pas de type de NC trouvé dans la base")
                            With DropDownList_TYPE_NC
                                .DataSource = dt2
                                .DataTextField = "NM_TYPE_NC"
                                .DataValueField = "ID_TYPE_NC"
                                .DataBind()
                                .Enabled = True
                                .SelectedValue = dt_inci(0)("ID_TYPE_NC").ToString
                            End With
                        End Using
                        GridView_TYPE_NC.DataBind()

                        '   recherche type Cause
                        sQuery = $"SELECT 'TDC_' + CONVERT(NVARCHAR,[ID_TYPE_CAUS]) AS NM_TYPE_CAUS
                                         ,[ID_TYPE_CAUS]
                                     FROM [dbo].[DTM_REF_TYPE_CAUS]
 FULL OUTER JOIN (SELECT '' AS NM_TYPE_CAUS) AS A ON A.NM_TYPE_CAUS = [dbo].[DTM_REF_TYPE_CAUS].[ID_TYPE_CAUS]
                    ORDER BY [ID_TYPE_DEFA]
[ID_TYPE_NC]
                            ,REPLACE([NM_TYPE_NC], 'cd_arti', '{Label_CD_ARTI_ALMS.Text}') AS [NM_TYPE_NC]
                        From [dbo].[V_NC_LIST_TYPE_NC_ID_SEQ]
                       Where [NU_PHAS_FCGF] = {DropDownList_PHAS_FAIL.SelectedValue}
                         AND [NU_SOUS_PHAS_FCGF] = {DropDownList_SOUS_PHAS_FAIL.SelectedValue}
                         And [ID_RFRC_SEQU] = {DropDownList_SEQU_FAIL.SelectedValue}"
                        Using dt2 = SQL_SELE_TO_DT(sQuery, CS_ALMS_PROD_PRD)
                            If dt2 Is Nothing Then Throw New Exception("Pas de type de cause trouvé dans la base")
                            With DropDownList_TYPE_NC
                                .DataSource = dt2
                                .DataTextField = "NM_TYPE_NC"
                                .DataValueField = "ID_TYPE_NC"
                                .DataBind()
                                .Enabled = True
                                .SelectedValue = dt_inci(0)("ID_TYPE_NC").ToString
                            End With
                        End Using
                    End Using
                    GridView_TYPE_NC.DataBind()
                    DropDownList_TYPE_CAUSE.Enabled = True
                    Button_ADD_TYPE_CAUS.Enabled = True
                Else 'sinon
                    '   affiche liste séquence fail pour le ns
                    sQuery = $"SELECT [NM_RFRC_SEQU]
                                ,[ID_RFRC_SEQU]
                            FROM [dbo].[V_NC_LIST_SEQU_FAIL_NU_SER]
                           WHERE [NU_SER_SPFE] = '{TextBox_NU_SER_SPFE.Text}'"
                    Using dt2 = SQL_SELE_TO_DT(sQuery, CS_ALMS_PROD_PRD)
                        If dt2 Is Nothing Then Throw New Exception($"Pas de séquence passée mauvaise n'a été retrouvé dans la base pour le numéro de série {TextBox_NU_SER_SPFE.Text}.")
                        With DropDownList_SEQU_FAIL
                            .DataSource = dt2
                            .DataTextField = "NM_RFRC_SEQU"
                            .DataValueField = "ID_RFRC_SEQU"
                            .DataBind()
                            .Items.Insert(0, New ListItem("", ""))
                            .Enabled = True
                        End With
                    End Using

                End If
            End Using
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try
    End Sub

    Protected Sub DropDownList_SEQU_FAIL_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList_SEQU_FAIL.SelectedIndexChanged
        'affiche liste phase fail pour le ns et la séquence
        'Dim dt As New DataTable
        Try
            Dim sQuery = $"SELECT [NU_PHAS_FCGF]
                            ,[NM_PHAS]
                        FROM [dbo].[V_NC_LIST_PHAS_FAIL_NU_SER_ID_SEQ]
                       WHERE [NU_SER_SPFE] = '{TextBox_NU_SER_SPFE.Text}'
                         AND [ID_RFRC_SEQU] = {DropDownList_SEQU_FAIL.SelectedValue}
                      Group BY [NU_PHAS_FCGF], [NM_PHAS]"
            Using dt = SQL_SELE_TO_DT(sQuery, CS_ALMS_PROD_PRD)
                If dt Is Nothing Then Throw New Exception($"Pas de phase mauvaise trouvée pour le numéro de série {TextBox_NU_SER_SPFE.Text}.")
                With DropDownList_PHAS_FAIL
                    .DataSource = dt
                    .DataTextField = "NM_PHAS"
                    .DataValueField = "NU_PHAS_FCGF"
                    .DataBind()
                    .Items.Insert(0, New ListItem("", ""))
                    .Enabled = True
                End With
            End Using
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try
    End Sub

    Protected Sub DropDownList_PHAS_FAIL_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList_PHAS_FAIL.SelectedIndexChanged
        'affiche liste sous-phase fail pour le ns, la séquence et la phase
        'Dim dt As New DataTable
        Try
            Dim sQuery = $"SELECT [NU_SOUS_PHAS_FCGF]
                            ,[NM_SOUS_PHAS]
                        FROM [dbo].[V_NC_LIST_PHAS_FAIL_NU_SER_ID_SEQ]
                       WHERE [NU_SER_SPFE] = '{TextBox_NU_SER_SPFE.Text}'
                         AND [ID_RFRC_SEQU] = {DropDownList_SEQU_FAIL.SelectedValue}
                         And [NU_PHAS_FCGF] = {DropDownList_PHAS_FAIL.SelectedValue}
                      GROUP BY [NU_SOUS_PHAS_FCGF],[NM_SOUS_PHAS],[NU_PHAS_FCGF],[NM_PHAS]"
            Using dt = SQL_SELE_TO_DT(sQuery, CS_ALMS_PROD_PRD)
                If dt Is Nothing Then Throw New Exception($"Pas de sous-phase mauvaise trouvée pour le numéro de série {TextBox_NU_SER_SPFE.Text}.")
                With DropDownList_SOUS_PHAS_FAIL
                    .DataSource = dt
                    .DataTextField = "NM_SOUS_PHAS"
                    .DataValueField = "NU_SOUS_PHAS_FCGF"
                    .DataBind()
                    .Items.Insert(0, New ListItem("", ""))
                    .Enabled = True
                End With
            End Using
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try
    End Sub

    Protected Sub DropDownList_SOUS_PHAS_FAIL_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList_SOUS_PHAS_FAIL.SelectedIndexChanged
        'affiche liste des type de nc
        'Dim dt As New DataTable
        Try
            Button_AJOU_TYPE_NC.Enabled = True
            Dim sQuery = $"SELECT [ID_TYPE_NC]
                            ,REPLACE([NM_TYPE_NC], 'cd_arti', '{Label_CD_ARTI_ALMS.Text}') AS [NM_TYPE_NC]
                        FROM [dbo].[V_NC_LIST_TYPE_NC_ID_SEQ]
                       WHERE [NU_PHAS_FCGF] = {DropDownList_PHAS_FAIL.SelectedValue}
                         And [NU_SOUS_PHAS_FCGF] = {DropDownList_SOUS_PHAS_FAIL.SelectedValue}
                         AND [ID_RFRC_SEQU] = {DropDownList_SEQU_FAIL.SelectedValue}"
            Using dt = SQL_SELE_TO_DT(sQuery, CS_ALMS_PROD_PRD)
                If dt Is Nothing Then
                    MultiView_ANML.SetActiveView(View_NOUV_TYPE_NC)
                    Throw New Exception("Pas de type de NC trouvé dans la base")
                End If
                With DropDownList_TYPE_NC
                    .DataSource = dt
                    .DataTextField = "NM_TYPE_NC"
                    .DataValueField = "ID_TYPE_NC"
                    .DataBind()
                    .Items.Insert(0, New ListItem("", ""))
                    .Enabled = True
                End With
            End Using
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try
    End Sub

    Protected Sub DropDownList_TYPE_CAUSE_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList_TYPE_CAUSE.SelectedIndexChanged
        'Dim dt As New DataTable
        Try
            'code article soumis à traçabilité ?
            Dim sQuery = $"SELECT [CD_ARTI_COMP]
                        From [dbo].[V_NC_LIST_CD_ARTI_TCBL_UNIT]
                       Where [CD_ARTI_PROD] = '{Label_CD_ARTI_ALMS.Text}'
                         And [ID_TYPE_CAUS] = {DropDownList_TYPE_CAUSE.SelectedValue}"
            Using dt = SQL_SELE_TO_DT(sQuery, CS_ALMS_PROD_PRD)
                ' Session("Ret_vue") = View_SAIS
                If Not dt Is Nothing Then
                    Label_CD_ARTI_CHAN.Text = dt(0)("CD_ARTI_COMP").ToString
                    MultiView_ANML.SetActiveView(View_CHAN_TCBL_UNIT)
                End If
            End Using
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try
    End Sub

    'Protected Sub TextBox_CD_ARTI_DFTE_TextChanged(sender As Object, e As EventArgs) Handles TextBox_CD_ARTI_DFTE.TextChanged

    '    Dim dt As New DataTable
    '    Try
    '        'code article soumis à traçabilité ?
    '        sQuery = "SELECT [ID_TYPE_CAUS]
    '                    FROM [dbo].[V_NC_LIST_CD_ARTI_TCBL_UNIT]
    '                   WHERE [CD_ARTI_PROD] = '{Label_CD_ARTI_ALMS.Text}'
    '                     AND [CD_ARTI_COMP] = {TextBox_CD_ARTI_DFTE.Text
    '        dt = SQL_SELE_TO_DT(sQuery, CS_MES_Digital_Factory_ALMS)
    '        'Session("Ret_vue") = View_NOUV_TYPE_CAUS
    '        'If Not dt Is Nothing Then MultiView_ANML.SetActiveView(View_CHAN_TCBL_UNIT)

    '    Catch ex As Exception
    '        LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
    '        Exit Sub
    '    End Try
    'End Sub

    Protected Sub Button_VALI_ENTER0_Click(sender As Object, e As EventArgs) Handles Button_VALI_ENTER0.Click
        'enregistrement + création n° NC et affichage liste des type de nc
        'Dim SQL_Connexion = New SqlConnection()
        'Dim cmd As New SqlCommand
        ' Dim dt As New DataTable
        Try
            Using SQL_Connexion = SQL_CONN(CS_ALMS_PROD_PRD)
                Using cmd = SQL_CALL_STOR_PROC(SQL_Connexion, "P_SAVE_NEW_TYPE_NC")
                    SQL_ADD_PARA_STOR_PROC(cmd, "ID_TYPE_NC", SqlDbType.TinyInt, 4000, "0", "Output")
                    SQL_ADD_PARA_STOR_PROC(cmd, "NM_DCPO_NC", SqlDbType.NVarChar, 4000, TextBox_NOUV_NC_DCPO.Text)
                    SQL_ADD_PARA_STOR_PROC(cmd, "NU_PHAS_FCGF", SqlDbType.SmallInt, 4000, DropDownList_PHAS_FAIL.SelectedValue)
                    SQL_ADD_PARA_STOR_PROC(cmd, "NU_SOUS_PHAS_FCGF", SqlDbType.SmallInt, 4000, DropDownList_SOUS_PHAS_FAIL.SelectedValue)
                    SQL_ADD_PARA_STOR_PROC(cmd, "ID_RFRC_SEQ", SqlDbType.BigInt, 4000, DropDownList_SEQU_FAIL.SelectedValue)
                    SQL_ADD_PARA_STOR_PROC(cmd, "RES", SqlDbType.NVarChar, 4000, "", "Output")
                    'dt = SQL_GET_DT_FROM_STOR_PROC(cmd)
                    SQL_EXEC_STOR_PROC(cmd)
                    If SQL_GET_PARA_VAL(cmd, "RES") <> "PASS" Then Throw New Exception(SQL_GET_PARA_VAL(cmd, "RES"))
                    MultiView_ANML.SetActiveView(View_SAIS)
                    Dim sQuery = $"SELECT [ID_TYPE_NC]
                            ,REPLACE([NM_TYPE_NC], 'cd_arti', '{Label_CD_ARTI_ALMS.Text}') AS [NM_TYPE_NC]
                        FROM [dbo].[V_NC_LIST_TYPE_NC_ID_SEQ]
                       WHERE [NU_PHAS_FCGF] = {DropDownList_PHAS_FAIL.SelectedValue}
                         And [NU_SOUS_PHAS_FCGF] = {DropDownList_SOUS_PHAS_FAIL.SelectedValue}
                         AND [ID_RFRC_SEQU] = {DropDownList_SEQU_FAIL.SelectedValue}"
                    Using dt = SQL_SELE_TO_DT(sQuery, CS_ALMS_PROD_PRD)
                        If dt Is Nothing Then Throw New Exception("Pas de type de NC trouvé dans la base")
                        With DropDownList_TYPE_NC
                            .DataSource = dt
                            .DataTextField = "NM_TYPE_NC"
                            .DataValueField = "ID_TYPE_NC"
                            .DataBind()
                            .SelectedValue = SQL_GET_PARA_VAL(cmd, "ID_TYPE_NC")
                            .Enabled = True
                        End With
                    End Using
                    DropDownList_TYPE_CAUSE.Enabled = True
                    Button_ADD_TYPE_CAUS.Enabled = True
                    cmd.Parameters.Clear()
                End Using
                SQL_Connexion.Close()
            End Using
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
            'Finally
            '    SQL_Connexion = SQL_CLOS(SQL_Connexion)
        End Try
    End Sub

    Protected Sub Button_VALI_ENTER_Click(sender As Object, e As EventArgs) Handles Button_VALI_ENTER.Click
        'Dim SQL_Connexion = New SqlConnection()
        'Dim cmd As New SqlCommand
        ' Dim dt As New DataTable
        Try
            Using SQL_Connexion = SQL_CONN(CS_ALMS_PROD_PRD)
                Using cmd = SQL_CALL_STOR_PROC(SQL_Connexion, "P_SAVE_NEW_TYPE_CAUS")
                    SQL_ADD_PARA_STOR_PROC(cmd, "ID_TYPE_CAUS", SqlDbType.TinyInt, 4000, "0", "Output")
                    SQL_ADD_PARA_STOR_PROC(cmd, "ID_TYPE_DEFA", SqlDbType.TinyInt, 4000, DropDownList_TYPE_DEFA.SelectedValue)
                    SQL_ADD_PARA_STOR_PROC(cmd, "CD_ARTI_DFTE", SqlDbType.NVarChar, 4000, TextBox_CD_ARTI_DFTE.Text)
                    SQL_ADD_PARA_STOR_PROC(cmd, "NM_RSLT", SqlDbType.NVarChar, 4000, TextBox_RSLT.Text)
                    SQL_ADD_PARA_STOR_PROC(cmd, "ID_RFRC_SEQU_RETO", SqlDbType.BigInt, 4000, DropDownList_SEQU_RETO_PDTO.SelectedValue)
                    SQL_ADD_PARA_STOR_PROC(cmd, "NU_PHAS_FCGF_RETO", SqlDbType.SmallInt, 4000, DropDownList_PHAS_RETO_PDTO.SelectedValue)
                    SQL_ADD_PARA_STOR_PROC(cmd, "RES", SqlDbType.VarChar, 4000, "", "Output")
                    'dt = SQL_GET_DT_FROM_STOR_PROC(cmd)
                    SQL_EXEC_STOR_PROC(cmd)
                    If SQL_GET_PARA_VAL(cmd, "RES") <> "PASS" Then Throw New Exception(SQL_GET_PARA_VAL(cmd, "RES"))
                    With DropDownList_TYPE_CAUSE
                        '.DataSource = dt
                        .DataBind()
                        .SelectedValue = SQL_GET_PARA_VAL(cmd, "ID_TYPE_CAUS")
                        .Enabled = True
                    End With
                    cmd.Parameters.Clear()
                End Using
                MultiView_ANML.SetActiveView(View_SAIS)
                SQL_Connexion.Close()
            End Using
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
            'Finally
            'SQL_Connexion = SQL_CLOS(SQL_Connexion)
        End Try

    End Sub

    Protected Sub Button_VALI_ENTER1_Click(sender As Object, e As EventArgs) Handles Button_VALI_ENTER1.Click
        'Dim dt, dtKNMT, dt_CFGR_ARTI_ECO As New DataTable
        Dim sID_NU_SER As String = "", iIDTT As String = "", sQuery As String = ""
        Try
            Using dtKNMT = SAP_DATA_READ_KNMT($"KDMAT Like '{Trim(Label_CD_ARTI_CHAN.Text)}%'")
                If dtKNMT Is Nothing Then Throw New Exception("pas de code article Eolane correspondant")
                Using dt_CFGR_ARTI_ECO = DIG_FACT_SQL_CFGR_ARTI_ECO(Trim(dtKNMT(0)("MATNR").ToString))

                    sQuery = $"SELECT   NM_NS_SENS
                               FROM   dbo.DTM_TR_CPT
                              WHERE   NM_NS_SENS = '{TextBoxNU_SER_CHAN.Text}'"
                    Using dt = SQL_SELE_TO_DT(sQuery, CS_MES_Digital_Factory)
                        If Not dt Is Nothing Then Throw New Exception($"Le numéro de série {TextBoxNU_SER_CHAN.Text} est déjà attribué dans la base.")
                    End Using
                    If dt_CFGR_ARTI_ECO(0)("Format Numéro de série Fournisseur").ToString <> "" Then
                        If DIG_FACT_VERI_FORM_NU_SER(Trim(dtKNMT(0)("MATNR").ToString), "Format Numéro de série Fournisseur", TextBoxNU_SER_CHAN.Text) = False Then Throw New Exception("Le numéro de série {TextBoxNU_SER_CHAN.Text} ne correspond au format défini dans la base.")
                    Else
                        Select Case "1"
                            Case dt_CFGR_ARTI_ECO(0)("Numéro de série Eolane").ToString
                                If DIG_FACT_VERI_FORM_NU_SER(Trim(dtKNMT(0)("MATNR").ToString), "Format Numéro de série Eolane", TextBoxNU_SER_CHAN.Text) = False Then Throw New Exception("Le numéro de série {TextBoxNU_SER_CHAN.Text} ne correspond au format défini dans la base.")
                            Case dt_CFGR_ARTI_ECO(0)("Numéro de série client").ToString
                                If DIG_FACT_VERI_FORM_NU_SER(Trim(dtKNMT(0)("MATNR").ToString), "Format Numéro de série client", TextBoxNU_SER_CHAN.Text) = False Then Throw New Exception("Le numéro de série {TextBoxNU_SER_CHAN.Text} ne correspond au format défini dans la base.")
                        End Select
                    End If
                End Using
                sID_NU_SER = DIG_FACT_SQL_ERGT_LIAI_ID_NU_SER(Label_NU_OF.Text, TextBox_NU_SER_SPFE.Text)
                If sID_NU_SER Is Nothing Then Throw New Exception("Pas d'ID numéro de série trouvé")
                sQuery = "SELECT [NEW_ID_PSG], GETDATE() AS DT_DEB
                            FROM [dbo].[V_NEW_ID_PSG_DTM_PSG]"
                Using dt = SQL_SELE_TO_DT(sQuery, CS_MES_Digital_Factory)
                    'sQuery = "INSERT INTO [dbo].[DTM_TR_CPT] ([NM_NS_EOL],[NM_NS_CLT],[ID_CPT],[ID_PSG],[DT_PSG])
                    '                       VALUES ('', '{TextBox_NU_SER_SPFE.Text}', '-', {dt(0)("NEW_ID_PSG").ToString}, GETDATE())"
                    'iIDTT = SQL_REQ_ACT_RET_IDTT(sQuery, sChaineConnexion)

                    If Session("matricule") = "" Then
                        LOG_MESS_UTLS(GetCurrentMethod, "Le login a été perdu. Vous allez être redirigé vers la page de login SAP. La saisie en cours sera perdue, il faudra la resaissir.", "Erreur")
                        Session("UrlReferrer") = HttpContext.Current.Request.Url.AbsolutePath.ToString()
                        Response.Redirect("~/Account/Login_SAP.aspx")
                    End If

                    'Enregistrement passage
                    sQuery = $"INSERT INTO [dbo].[DTM_PSG] ([ID_PSG], [LB_ETP], [DT_DEB], [DT_FIN], [LB_PROG], [NM_MATR], [NM_NS_EOL], [LB_SCTN], [NM_OF], [ID_NU_SER])
                           VALUES ({dt(0)("NEW_ID_PSG").ToString}, 'Remplacement sous-ensemble (OP:9999)', GETDATE(), GETDATE(), '{HttpContext.Current.CurrentHandler.ToString}', '{Session("matricule")}', '', 'P', '{Label_NU_OF.Text}', {sID_NU_SER})"
                    SQL_REQ_ACT(sQuery, CS_MES_Digital_Factory)

                    'enregistrer dans la base les associations composant
                    sQuery = $"INSERT INTO [dbo].[DTM_TR_CPT] ([NM_NS_EOL],[NM_NS_CLT],[ID_CPT],[ID_PSG],[DT_PSG],[NM_SAP_CPT],[NM_NS_SENS])
                                   VALUES ('', '{TextBox_NU_SER_SPFE.Text}', '-', {dt(0)("NEW_ID_PSG").ToString}, GETDATE(), '{Trim(dtKNMT(0)("MATNR").ToString)}','{TextBoxNU_SER_CHAN.Text}')"
                    iIDTT = SQL_REQ_ACT_RET_IDTT(sQuery, CS_MES_Digital_Factory)
                End Using
            End Using
            MultiView_ANML.SetActiveView(View_SAIS)
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try
    End Sub

    Protected Sub DropDownList_TYPE_NC_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList_TYPE_NC.SelectedIndexChanged
        DropDownList_TYPE_CAUSE.Enabled = True
        Button_ADD_TYPE_CAUS.Enabled = True
    End Sub

    Protected Sub Button_ADD_TYPE_DEFA_Click(sender As Object, e As EventArgs) Handles Button_ADD_TYPE_DEFA.Click

    End Sub

    Protected Sub Button_VALI_ENTER2_Click(sender As Object, e As EventArgs) Handles Button_VALI_ENTER2.Click
        'Dim SQL_Connexion = New SqlConnection()
        'Dim cmd As New SqlCommand
        Dim dt As New DataTable
        Dim sQuery As String = ""
        Try
            If Label_NU_INCI.Text = "" Then
                Using SQL_Connexion = SQL_CONN(CS_ALMS_PROD_PRD)
                    Using cmd = SQL_CALL_STOR_PROC(SQL_Connexion, "P_GET_NEW_NU_INCI")
                        SQL_ADD_PARA_STOR_PROC(cmd, "NU_INCI", SqlDbType.TinyInt, 4000, "0", "Output")
                        'dt = SQL_GET_DT_FROM_STOR_PROC(cmd)
                        SQL_EXEC_STOR_PROC(cmd)
                        Label_NU_INCI.Text = SQL_GET_PARA_VAL(cmd, "NU_INCI")
                        cmd.Parameters.Clear()
                    End Using
                    SQL_Connexion.Close()
                End Using
                sQuery = $"INSERT INTO [dbo].[DTM_NC_LIST]
           ([ID_TYPE_NC]
           ,[NU_INCI]
           ,[NU_SER_SPFE]
           ,[CD_ARTI_PROD])
     VALUES
           ({DropDownList_TYPE_NC.SelectedValue}
           ,{Label_NU_INCI.Text}
           ,'{TextBox_NU_SER_SPFE.Text}'
           ,'{Label_CD_ARTI_ALMS.Text}')"
                SQL_REQ_ACT(sQuery, CS_ALMS_PROD_PRD)

                sQuery = $"INSERT INTO [dbo].[DTM_NC_DETA]
           ([NU_INCI]
           ,[DT_ITVT]
           ,[ID_OPRT]
           ,[BL_ETAT]
           ,[ID_STAT]
           ,[ID_TYPE_CAUS])
     VALUES
           ({Label_NU_INCI.Text}
           ,GETDATE()
           ,{Session("matricule")}
           ,0
           ,1
           ,{DropDownList_TYPE_CAUSE.SelectedValue})"
                SQL_REQ_ACT(sQuery, CS_ALMS_PROD_PRD)


            End If
            sQuery = $"INSERT INTO [dbo].[DTM_NC_DETA]
           ([NU_INCI]
           ,[DT_ITVT]
           ,[ID_OPRT]
           ,[BL_ETAT]
           ,[ID_STAT]
           ,[ID_TYPE_CAUS])
     VALUES
           ({Label_NU_INCI.Text}
           ,GETDATE()
           ,{Session("matricule")}
           ,0
           ,2
           ,{DropDownList_TYPE_CAUSE.SelectedValue})"
            SQL_REQ_ACT(sQuery, CS_ALMS_PROD_PRD)
            Label_ID_STAT.Text = "En cours"
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
            'Finally
            '    SQL_Connexion = SQL_CLOS(SQL_Connexion)
        End Try
    End Sub

    Protected Sub RadioButtonList_TYPE_INCI_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RadioButtonList_TYPE_INCI.SelectedIndexChanged
        Select Case RadioButtonList_TYPE_INCI.SelectedValue
            Case "Dérogations"
                Session("View_state") = MultiView_ANML.ActiveViewIndex
                MultiView_ANML.SetActiveView(View_DRGT)
            Case "Anomalies"
                MultiView_ANML.SetActiveView(Session("View_state"))
        End Select
    End Sub

    Protected Sub TextBox_NU_DRGT_TextChanged(sender As Object, e As EventArgs) Handles TextBox_NU_DRGT.TextChanged
        'Dim dt1 As New DataTable
        Try
            Dim sQuery = $"SELECT [NU_SER_SPFE] AS [Numéro de série]
  From [dbo].[DTM_NC_LIST]
 Where [NU_DRGT] = '{TextBox_NU_DRGT.Text}'"
            Using dt1 = SQL_SELE_TO_DT(sQuery, CS_ALMS_PROD_PRD)
                If dt1 Is Nothing Then
                    '  dt1 = New DataTable
                    dt1.Columns.Add("Numéro de série", Type.GetType("System.String"))
                End If
                Session("NU_SER_DRGT") = dt1
                GridView_LIST_NU_SER_DRGT.DataSource = dt1
                GridView_LIST_NU_SER_DRGT.DataBind()
            End Using
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try
    End Sub

    Protected Sub TextBox_NU_SER_DRGT_TextChanged(sender As Object, e As EventArgs) Handles TextBox_NU_SER_DRGT.TextChanged
        'Dim dt1, dt2, dt3, dt4 As New DataTable
        Try
            'recherche OF
            Dim sQuery = $"Select [NU_OF]
  From [dbo].[V_LIAIS_NU_SER]
                       Where [NU_SER_CLIE] = '{TextBox_NU_SER_DRGT.Text}'"
            Using dt1 = SQL_SELE_TO_DT(sQuery, CS_MES_Digital_Factory)
                If dt1 Is Nothing Then Throw New Exception($"L'OF du numéro de série {TextBox_NU_SER_DRGT.Text} n'a pas été retrouvé dans la base.")
                'recherche code article
                Using dt2 = SAP_DATA_LECT_OF(dt1(0)("NU_OF").ToString)
                    If dt2 Is Nothing Then Throw New Exception($"Pas d'info dans SAP retrouvées sur l'OF {dt1(0)("NU_OF").ToString} n'a pas été retrouvé dans la base.")
                    'code article ALMS
                    Using dt3 = SAP_DATA_READ_KNMT($"MATNR LIKE '{Trim(dt2(0)("CD_ARTI_ECO").ToString)}%' AND KUNNR EQ '0000000451' AND VKORG EQ 'ORC3' AND VTWEG EQ 'CD'")
                        If dt3 Is Nothing Then Throw New Exception($"L'équivalent de {Trim(dt2(0)("CD_ARTI_ECO").ToString)} en code article ALMS n'a pas été retrouvé dans SAP.")
                        If Label_NU_OF.Text = "" And Label_CD_ARTI_ECO.Text = "" And Label_NM_DSGT_ECO.Text = "" And Label_CD_ARTI_ALMS.Text = "" Then
                            Label_NU_OF.Text = dt1(0)("NU_OF").ToString
                            Label_CD_ARTI_ECO.Text = Trim(dt2(0)("CD_ARTI_ECO").ToString)
                            Label_NM_DSGT_ECO.Text = Trim(dt2(0)("NM_DSGT_ARTI").ToString)
                            Label_CD_ARTI_ALMS.Text = Trim(dt3(0)("KDMAT").ToString)
                        Else
                            If Label_NU_OF.Text <> dt1(0)("NU_OF").ToString Then Throw New Exception($"L'of {Label_NU_OF.Text} du numéro de série {TextBox_NU_SER_SPFE.Text} est différent")
                        End If
                    End Using
                End Using
            End Using
            Using dt4 = Session("NU_SER_DRGT")
                dt4.Rows.Add()
                dt4.Rows(dt4.Rows.Count - 1)("Numéro de série") = TextBox_NU_SER_DRGT.Text
                Session("NU_SER_DRGT") = dt4
                GridView_LIST_NU_SER_DRGT.DataSource = dt4
                GridView_LIST_NU_SER_DRGT.DataBind()
            End Using
            TextBox_NU_SER_DRGT.Text = ""
            TextBox_NU_SER_DRGT.Focus()
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try
    End Sub

    Protected Sub Button_VALI_ENTER3_Click(sender As Object, e As EventArgs) Handles Button_VALI_ENTER3.Click
        'enregistrement + création n° NC et affichage liste des type de nc
        'Dim SQL_Connexion = New SqlConnection()
        'Dim cmd As New SqlCommand
        'Dim dt As New DataTable
        Dim iTYPE_NC As Integer = 0, iINCI As Integer = 0
        Try
            Using SQL_Connexion = SQL_CONN(CS_ALMS_PROD_PRD)
                Using cmd = SQL_CALL_STOR_PROC(SQL_Connexion, "P_SAVE_NEW_TYPE_NC")
                    SQL_ADD_PARA_STOR_PROC(cmd, "ID_TYPE_NC", SqlDbType.TinyInt, 4000, "0", "Output")
                    SQL_ADD_PARA_STOR_PROC(cmd, "NM_DCPO_NC", SqlDbType.NVarChar, 4000, TextBox_NM_DCPO_DRGT.Text)
                    SQL_ADD_PARA_STOR_PROC(cmd, "NU_PHAS_FCGF", SqlDbType.SmallInt, 4000, "0")
                    SQL_ADD_PARA_STOR_PROC(cmd, "NU_SOUS_PHAS_FCGF", SqlDbType.SmallInt, 4000, "0")
                    SQL_ADD_PARA_STOR_PROC(cmd, "ID_RFRC_SEQ", SqlDbType.BigInt, 4000, "0")
                    SQL_ADD_PARA_STOR_PROC(cmd, "RES", SqlDbType.NVarChar, 4000, "", "Output")
                    SQL_EXEC_STOR_PROC(cmd)
                    'If SQL_GET_PARA_VAL(cmd, "RES") <> "PASS" Then Throw New Exception(SQL_GET_PARA_VAL(cmd, "RES"))
                    iTYPE_NC = SQL_GET_PARA_VAL(cmd, "ID_TYPE_NC")
                    cmd.Parameters.Clear()
                End Using
                Using dt = Session("NU_SER_DRGT")
                    For Each rdt As DataRow In dt.Rows
                        Using cmd2 = SQL_CALL_STOR_PROC(SQL_Connexion, "P_GET_NEW_NU_INCI")
                            SQL_ADD_PARA_STOR_PROC(cmd2, "NU_INCI", SqlDbType.TinyInt, 4000, "0", "Output")
                            'dt = SQL_GET_DT_FROM_STOR_PROC(cmd)
                            SQL_EXEC_STOR_PROC(cmd2)
                            Label_NU_INCI.Text = SQL_GET_PARA_VAL(cmd2, "NU_INCI")
                            cmd2.Parameters.Clear()
                        End Using
                        Dim sQuery = $"INSERT INTO [dbo].[DTM_NC_LIST]
           ([ID_TYPE_NC]
           ,[NU_INCI]
           ,[NU_SER_SPFE]
           ,[CD_ARTI_PROD]
           ,[NU_DRGT])
     VALUES
           ({iTYPE_NC.ToString}
           ,{Label_NU_INCI.Text}
           ,'{rdt("Numéro de série").ToString}'
           ,'{Label_CD_ARTI_ALMS.Text}'
           ,'{TextBox_NU_DRGT.Text}')"
                        SQL_REQ_ACT(sQuery, CS_ALMS_PROD_PRD)

                        sQuery = $"INSERT INTO [dbo].[DTM_NC_DETA]
           ([NU_INCI]
           ,[DT_ITVT]
           ,[ID_OPRT]
           ,[BL_ETAT]
           ,[ID_STAT])
     VALUES
           ({Label_NU_INCI.Text}
           ,GETDATE()
           ,{Session("matricule")}
           ,1
,1)"
                        SQL_REQ_ACT(sQuery, CS_ALMS_PROD_PRD)
                        sQuery = $"INSERT INTO [dbo].[DTM_NC_DETA]
           ([NU_INCI]
           ,[DT_ITVT]
           ,[ID_OPRT]
           ,[BL_ETAT]
           ,[ID_STAT])
     VALUES
           ({Label_NU_INCI.Text}
           ,GETDATE()
           ,{Session("matricule")}
           ,1
,4)"
                        SQL_REQ_ACT(sQuery, CS_ALMS_PROD_PRD)
                    Next
                End Using
                SQL_Connexion.Close()
            End Using
            RadioButtonList_TYPE_INCI.SelectedValue = "Anomalies"
            MultiView_ANML.SetActiveView(View_SAIS)

        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
            'Finally
            '    SQL_Connexion = SQL_CLOS(SQL_Connexion)
        End Try
    End Sub
End Class