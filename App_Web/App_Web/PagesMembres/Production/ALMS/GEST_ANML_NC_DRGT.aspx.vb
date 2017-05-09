Imports App_Web.Class_SQL
Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports App_Web.Class_SAP_DATA
Imports System.Data.SqlClient

Public Class GEST_ANML_NC_DRGT
    Inherits System.Web.UI.Page

    Dim sChaineConnexion_ALMS As String = "Data Source=cedb03,1433;Initial Catalog=" & Replace(Replace(My.Computer.Name, "CEDB03", "ALMS_PROD_DEV"), "CEAPP03", "ALMS_PROD_PRD") & ";Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=7200;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
    Dim sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=" & Replace(Replace(My.Computer.Name, "CEDB03", "MES_Digital_Factory_DEV"), "CEAPP03", "MES_Digital_Factory") & ";Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=7200;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
    Dim sQuery As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'vérification de l'habilitation
        Dim dt As New DataTable
        Try
            sQuery = "SELECT [DT_ATVT]
                        FROM [dbo].[DTM_REF_OPRT]
                       WHERE [ID_MTCL_ECO] = " & Session("matricule") & "
                         AND [BL_ATVT] = 1
                         AND [ID_HBLT_ALMS] = 1"
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion_ALMS)
            If dt Is Nothing Then Throw New Exception(Session("displayname") & " n'a pas le droit d'accéder à cette page")
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
        Dim dt As New DataTable
        Try
            'recherche OF
            sQuery = "SELECT [NU_OF]
                        FROM [dbo].[V_LIAIS_NU_SER]
                       WHERE [NU_SER_CLIE] = '" & TextBox_NU_SER_SPFE.Text & "'"
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            If dt Is Nothing Then Throw New Exception("L'OF du numéro de série " & TextBox_NU_SER_SPFE.Text & " n'a pas été retrouvé dans la base.")
            Label_NU_OF.Text = dt(0)("NU_OF").ToString

            'recherche code article
            dt = SAP_DATA_LECT_OF(Label_NU_OF.Text)
            If dt Is Nothing Then Throw New Exception("Pas d'info dans SAP retrouvées sur l'OF " & Label_NU_OF.Text & " n'a pas été retrouvé dans la base.")
            Label_CD_ARTI_ECO.Text = Trim(dt(0)("CD_ARTI_ECO").ToString)

            'désignation 
            Label_NM_DSGT_ECO.Text = Trim(dt(0)("NM_DSGT_ARTI").ToString)

            'code article ALMS
            dt = SAP_DATA_READ_KNMT("MATNR LIKE '" & Label_CD_ARTI_ECO.Text & "%' AND KUNNR EQ '0000000451' AND VKORG EQ 'ORC3' AND VTWEG EQ 'CD'")
            If dt Is Nothing Then Throw New Exception("L'équivalent de " & Label_CD_ARTI_ECO.Text & " en code article ALMS n'a pas été retrouvé dans SAP.")
            Label_CD_ARTI_ALMS.Text = Trim(dt(0)("KDMAT").ToString)

            'Numéro d'incident
            sQuery = "SELECT [NU_INCI]
                        FROM [dbo].[V_NC_LIST_NU_INCI_NU_SER]
                       WHERE [NU_SER_SPFE] = '" & TextBox_NU_SER_SPFE.Text & "'"
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion_ALMS)
            If Not dt Is Nothing Then 'si existant
                Label_NU_INCI.Text = dt(0)("NU_INCI").ToString
                sQuery = "SELECT [ID_TYPE_NC]
                                ,[NM_TYPE_NC]
                                ,[NM_DCPO_NC]
                                ,[ID_RFRC_SEQU]
                                ,[NM_RFRC_SEQU]
                                ,[NU_PHAS_FCGF]
                                ,[NM_PHAS]
                                ,[NU_SOUS_PHAS_FCGF]
                                ,[NM_SOUS_PHAS]
                            FROM [dbo].[V_NC_LIST_INFO_NU_INCI]
                           WHERE [NU_INCI] = " & Label_NU_INCI.Text & "
                             AND [NU_SER_SPFE] = '" & TextBox_NU_SER_SPFE.Text & "'"
                dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion_ALMS)
                If dt Is Nothing Then Throw New Exception("Pas de séquence passée mauvaise n'a été retrouvé dans la base pour le numéro de série " & TextBox_NU_SER_SPFE.Text & ".")

                '   recherche séquence
                With DropDownList_SEQU_FAIL
                    .DataSource = dt
                    .DataTextField = "NM_RFRC_SEQU"
                    .DataValueField = "ID_RFRC_SEQU"
                    .DataBind()
                    .Enabled = True
                End With
                '   recherche phase
                With DropDownList_PHAS_FAIL
                    .DataSource = dt
                    .DataTextField = "NM_PHAS"
                    .DataValueField = "NU_PHAS_FCGF"
                    .DataBind()
                    .Enabled = True
                End With
                '   recherche sous-phase
                With DropDownList_SOUS_PHAS_FAIL
                    .DataSource = dt
                    .DataTextField = "NM_SOUS_PHAS"
                    .DataValueField = "NU_SOUS_PHAS_FCGF"
                    .DataBind()
                    .Enabled = True
                End With
                '   recherche type NC
                With DropDownList_TYPE_NC
                    .DataSource = dt
                    .DataTextField = "NM_TYPE_NC"
                    .DataValueField = "ID_TYPE_NC"
                    .DataBind()
                    .Enabled = True
                End With
                GridView_TYPE_NC.DataBind()
            Else 'sinon
                '   affiche liste séquence fail pour le ns
                sQuery = "SELECT [NM_RFRC_SEQU]
                                ,[ID_RFRC_SEQU]
                            FROM [dbo].[V_NC_LIST_SEQU_FAIL_NU_SER]
                           WHERE [NU_SER_SPFE] = '" & TextBox_NU_SER_SPFE.Text & "'"
                dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion_ALMS)
                If dt Is Nothing Then Throw New Exception("Pas de séquence passée mauvaise n'a été retrouvé dans la base pour le numéro de série " & TextBox_NU_SER_SPFE.Text & ".")
                With DropDownList_SEQU_FAIL
                    .DataSource = dt
                    .DataTextField = "NM_RFRC_SEQU"
                    .DataValueField = "ID_RFRC_SEQU"
                    .DataBind()
                    .Enabled = True
                End With
            End If
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try
    End Sub

    Protected Sub DropDownList_SEQU_FAIL_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList_SEQU_FAIL.SelectedIndexChanged
        'affiche liste phase fail pour le ns et la séquence
        Dim dt As New DataTable
        Try
            sQuery = "SELECT [NU_PHAS_FCGF]
                            ,[NM_PHAS]
                        FROM [dbo].[V_NC_LIST_PHAS_FAIL_NU_SER_ID_SEQ]
                       WHERE [NU_SER_SPFE] = '" & TextBox_NU_SER_SPFE.Text & "'
                         AND [ID_RFRC_SEQU] = " & DropDownList_SEQU_FAIL.SelectedValue & "
                      GROUP BY [NU_PHAS_FCGF],[NM_PHAS]"
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion_ALMS)
            If dt Is Nothing Then Throw New Exception("Pas de phase mauvaise trouvée pour le numéro de série " & TextBox_NU_SER_SPFE.Text & ".")
            With DropDownList_PHAS_FAIL
                .DataSource = dt
                .DataTextField = "NM_PHAS"
                .DataValueField = "NU_PHAS_FCGF"
                .DataBind()
                .Enabled = True
            End With
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try
    End Sub

    Protected Sub DropDownList_PHAS_FAIL_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList_PHAS_FAIL.SelectedIndexChanged
        'affiche liste sous-phase fail pour le ns, la séquence et la phase
        Dim dt As New DataTable
        Try
            sQuery = "SELECT [NU_SOUS_PHAS_FCGF]
                            ,[NM_SOUS_PHAS]
                        FROM [dbo].[V_NC_LIST_PHAS_FAIL_NU_SER_ID_SEQ]
                       WHERE [NU_SER_SPFE] = '" & TextBox_NU_SER_SPFE.Text & "'
                         AND [ID_RFRC_SEQU] = " & DropDownList_SEQU_FAIL.SelectedValue & "
                         AND [NU_PHAS_FCGF] = " & DropDownList_PHAS_FAIL.SelectedValue & "
                      GROUP BY [NU_PHAS_FCGF],[NM_PHAS]"
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion_ALMS)
            If dt Is Nothing Then Throw New Exception("Pas de sous-phase mauvaise trouvée pour le numéro de série " & TextBox_NU_SER_SPFE.Text & ".")
            With DropDownList_SOUS_PHAS_FAIL
                .DataSource = dt
                .DataTextField = "NM_SOUS_PHAS"
                .DataValueField = "NU_SOUS_PHAS_FCGF"
                .DataBind()
                .Enabled = True
            End With
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try
    End Sub

    Protected Sub DropDownList_SOUS_PHAS_FAIL_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList_SOUS_PHAS_FAIL.SelectedIndexChanged
        'affiche liste des type de nc
        Dim dt As New DataTable
        Try
            sQuery = "SELECT [ID_TYPE_NC]
                            ,[NM_TYPE_NC]
                        FROM [dbo].[V_NC_LIST_TYPE_NC_ID_SEQ]
                       WHERE [NU_PHAS_FCGF] = " & DropDownList_PHAS_FAIL.SelectedValue & "
                         AND [NU_SOUS_PHAS_FCGF] = " & DropDownList_SOUS_PHAS_FAIL.SelectedValue & "
                         AND [ID_RFRC_SEQU] = " & DropDownList_SEQU_FAIL.SelectedValue
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion_ALMS)
            If dt Is Nothing Then Throw New Exception("Pas de type de NC trouvé dans la base")
            With DropDownList_TYPE_NC
                .DataSource = dt
                .DataTextField = "NM_TYPE_NC"
                .DataValueField = "ID_TYPE_NC"
                .DataBind()
                .Enabled = True
            End With
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try
    End Sub

    Protected Sub DropDownList_TYPE_CAUSE_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList_TYPE_CAUSE.SelectedIndexChanged
        Dim dt As New DataTable
        Try
            'code article soumis à traçabilité ?
            sQuery = "SELECT [CD_ARTI_COMP]
                        FROM [dbo].[V_NC_LIST_CD_ARTI_TCBL_UNIT]
                       WHERE [CD_ARTI_PROD] = '" & Label_CD_ARTI_ALMS.Text & "'
                         AND [ID_TYPE_CAUS] = " & DropDownList_TYPE_CAUSE.SelectedValue
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion_ALMS)
            Session("Ret_vue") = View_SAIS
            If Not dt Is Nothing Then MultiView_ANML.SetActiveView(View_CHAN_TCBL_UNIT)
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try
    End Sub

    Protected Sub TextBox_CD_ARTI_DFTE_TextChanged(sender As Object, e As EventArgs) Handles TextBox_CD_ARTI_DFTE.TextChanged

        Dim dt As New DataTable
        Try
            'code article soumis à traçabilité ?
            sQuery = "SELECT [ID_TYPE_CAUS]
                        FROM [dbo].[V_NC_LIST_CD_ARTI_TCBL_UNIT]
                       WHERE [CD_ARTI_PROD] = '" & Label_CD_ARTI_ALMS.Text & "'
                         AND [CD_ARTI_COMP] = " & TextBox_CD_ARTI_DFTE.Text
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion_ALMS)
            Session("Ret_vue") = View_NOUV_TYPE_CAUS
            If Not dt Is Nothing Then MultiView_ANML.SetActiveView(View_CHAN_TCBL_UNIT)

        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try
    End Sub

    Protected Sub Button_VALI_ENTER0_Click(sender As Object, e As EventArgs) Handles Button_VALI_ENTER0.Click
        'enregistrement + création n° NC et affichage liste des type de nc
        Dim SQL_Connexion = New SqlConnection()
        Dim cmd As New SqlCommand
        Dim dt As New DataTable
        Try
            SQL_Connexion = SQL_CONN(sChaineConnexion_ALMS)
            cmd = SQL_CALL_STOR_PROC(SQL_Connexion, "P_SAVE_NEW_TYPE_NC")
            SQL_ADD_PARA_STOR_PROC(cmd, "ID_TYPE_NC", SqlDbType.TinyInt, 4000, "", "Output")
            SQL_ADD_PARA_STOR_PROC(cmd, "NM_DCPO_NC", SqlDbType.VarChar, 4000, TextBox_NOUV_NC_DCPO.Text)
            SQL_ADD_PARA_STOR_PROC(cmd, "NU_PHAS_FCGF", SqlDbType.SmallInt, 4000, DropDownList_PHAS_FAIL.SelectedValue)
            SQL_ADD_PARA_STOR_PROC(cmd, "NU_SOUS_PHAS_FCGF", SqlDbType.SmallInt, 4000, DropDownList_SOUS_PHAS_FAIL.SelectedValue)
            SQL_ADD_PARA_STOR_PROC(cmd, "ID_RFRC_SEQ", SqlDbType.BigInt, 4000, DropDownList_SEQU_FAIL.SelectedValue)
            SQL_ADD_PARA_STOR_PROC(cmd, "RES", SqlDbType.VarChar, 4000, "", "Output")
            dt = SQL_GET_DT_FROM_STOR_PROC(cmd)
            SQL_EXEC_STOR_PROC(cmd)
            If SQL_GET_PARA_VAL(cmd, "RES") <> "PASS" Then Throw New Exception(SQL_GET_PARA_VAL(cmd, "RES"))
            sQuery = "SELECT [ID_TYPE_NC]
                            ,[NM_TYPE_NC]
                        FROM [dbo].[V_NC_LIST_TYPE_NC_ID_SEQ]
                       WHERE [NU_PHAS_FCGF] = " & DropDownList_PHAS_FAIL.SelectedValue & "
                         AND [NU_SOUS_PHAS_FCGF] = " & DropDownList_SOUS_PHAS_FAIL.SelectedValue & "
                         AND [ID_RFRC_SEQU] = " & DropDownList_SEQU_FAIL.SelectedValue
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion_ALMS)
            If dt Is Nothing Then Throw New Exception("Pas de type de NC trouvé dans la base")
            With DropDownList_TYPE_NC
                .DataSource = dt
                .DataTextField = "NM_TYPE_NC"
                .DataValueField = "ID_TYPE_NC"
                .DataBind()
                .SelectedValue = SQL_GET_PARA_VAL(cmd, "ID_TYPE_NC")
                .Enabled = True
            End With
            cmd.Parameters.Clear()
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        Finally
            SQL_Connexion = SQL_CLOS(SQL_Connexion)
        End Try
    End Sub

    Protected Sub Button_VALI_ENTER_Click(sender As Object, e As EventArgs) Handles Button_VALI_ENTER.Click
        Dim SQL_Connexion = New SqlConnection()
        Dim cmd As New SqlCommand
        Dim dt As New DataTable
        Try
            SQL_Connexion = SQL_CONN(sChaineConnexion_ALMS)
            cmd = SQL_CALL_STOR_PROC(SQL_Connexion, "P_SAVE_NEW_TYPE_CAUS")
            SQL_ADD_PARA_STOR_PROC(cmd, "ID_TYPE_CAUS", SqlDbType.TinyInt, 4000, "", "Output")
            SQL_ADD_PARA_STOR_PROC(cmd, "ID_TYPE_DEFA", SqlDbType.TinyInt, 4000, DropDownList_TYPE_DEFA.SelectedValue)
            SQL_ADD_PARA_STOR_PROC(cmd, "CD_ARTI_DFTE", SqlDbType.NVarChar, 4000, TextBox_CD_ARTI_DFTE.Text)
            SQL_ADD_PARA_STOR_PROC(cmd, "NM_RSLT", SqlDbType.NVarChar, 4000, TextBox_RSLT.Text)
            SQL_ADD_PARA_STOR_PROC(cmd, "ID_RFRC_SEQU_RETO", SqlDbType.BigInt, 4000, DropDownList_SEQU_RETO_PDTO.SelectedValue)
            SQL_ADD_PARA_STOR_PROC(cmd, "NU_PHAS_FCGF_RETO", SqlDbType.SmallInt, 4000, DropDownList_PHAS_RETO_PDTO.SelectedValue)
            SQL_ADD_PARA_STOR_PROC(cmd, "RES", SqlDbType.VarChar, 4000, "", "Output")
            dt = SQL_GET_DT_FROM_STOR_PROC(cmd)
            SQL_EXEC_STOR_PROC(cmd)
            If SQL_GET_PARA_VAL(cmd, "RES") <> "PASS" Then Throw New Exception(SQL_GET_PARA_VAL(cmd, "RES"))
            With DropDownList_TYPE_CAUSE
                .DataBind()
                .SelectedValue = SQL_GET_PARA_VAL(cmd, "ID_TYPE_CAUS")
                .Enabled = True
            End With
            cmd.Parameters.Clear()
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        Finally
            SQL_Connexion = SQL_CLOS(SQL_Connexion)
        End Try


    End Sub

    Protected Sub Button_VALI_ENTER1_Click(sender As Object, e As EventArgs) Handles Button_VALI_ENTER1.Click
        Try
            'Select Case
            'Session("Ret_vue") = 
            'MultiView_ANML.SetActiveView(View_SAIS)
            MultiView_ANML.SetActiveView(Session("Ret_vue"))

        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try
    End Sub

End Class