Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports App_Web.Class_COMM_APP_WEB
Imports App_Web.Class_SAP_DATA
Imports App_Web.Class_SQL
Imports App_Web.Class_DIG_FACT_SQL

Imports System.IO
Public Class IMPR_ETIQ_PRN
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Label_DT_EXP.Text = Now
    End Sub

    Protected Sub CheckBox_NU_SER_ECO_GENE_AUTO_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox_NU_SER_ECO_GENE_AUTO.CheckedChanged
        If CheckBox_NU_SER_ECO_GENE_AUTO.Checked = True Then
            TextBox_NU_SER_ECO.Enabled = False
        Else
            TextBox_NU_SER_ECO.Enabled = True
        End If
        If Label_CD_ARTI_ECO.Text <> "" And DropDownList_NM_ETI.SelectedValue <> "" Then COMM_APP_WEB_PARA_AFFI_SAVE(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "CheckBox_NU_SER_ECO_GENE_AUTO", CheckBox_NU_SER_ECO_GENE_AUTO.Checked)
    End Sub

    Protected Sub TextBox_NU_OF_TextChanged(sender As Object, e As EventArgs) Handles TextBox_NU_OF.TextChanged
        Dim dt_CFGR_ARTI_ECO, dt_ETAT_CTRL As New DataTable
        Try
            'chercher les infos
            Dim dtAFKO As DataTable = SAP_DATA_READ_AFKO("AUFNR EQ '00000" & TextBox_NU_OF.Text & "'")
            If dtAFKO Is Nothing Then
                LOG_Erreur(GetCurrentMethod, "L'OF " & TextBox_NU_OF.Text & " n'a pas été trouvé dans SAP.")
                Exit Sub
            End If

            Dim dtMARA As DataTable = SAP_DATA_READ_MARA("MATNR EQ '" & dtAFKO(0)("PLNBEZ").ToString & "'")
            Dim dtT179T As DataTable = SAP_DATA_READ_T179T("PRODH EQ '" & dtMARA(0)("PRDHA").ToString & "'")
            'client
            Session("Client") = dtT179T(0)("VTEXT").ToString
            'quantité de l'of
            Session("QT_OF") = Convert.ToDecimal(Replace(dtAFKO(0)("GAMNG").ToString, ".", ","))
            'code article eolane
            Label_CD_ARTI_ECO.Text = dtAFKO(0)("PLNBEZ").ToString
            dt_CFGR_ARTI_ECO = App_Web.Class_DIG_FACT_SQL.DIG_FACT_SQL_CFGR_ARTI_ECO(Trim(Label_CD_ARTI_ECO.Text))
            If dt_CFGR_ARTI_ECO Is Nothing Then Throw New Exception("La base Digital Factory n'a pas été configurée pour l'article " & Trim(Label_CD_ARTI_ECO.Text))
            dt_ETAT_CTRL = COMM_APP_WEB_ETAT_CTRL(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue)
            If dt_ETAT_CTRL Is Nothing Then Throw New Exception("La base App_Web_Eco n'as pas été configurée pour l'article " & Trim(Label_CD_ARTI_ECO.Text))

            'désignation article
            Dim dtMAKT As DataTable = SAP_DATA_READ_MAKT("MATNR EQ '" & dtAFKO(0)("PLNBEZ").ToString & "'")
            Label_NM_DSGT_ARTI.Text = dtMAKT(0)("MAKTX").ToString
            'code article client
            'TextBox_CD_ARTI_CLIE.Text = DIG_FACT_SQL_GET_PARA(Label_CD_ARTI_ECO.Text, "Code article client")
            TextBox_CD_ARTI_CLIE.Text = dt_CFGR_ARTI_ECO(0)("Code article client").ToString
            'code fournisseur
            TextBox_CD_FNS.Text = DIG_FACT_SQL_GET_PARA(Session("Client"), "Format du code fournisseur")
            'Indice Client
            'TextBox_IND_CLIE.Text = DIG_FACT_SQL_GET_PARA(Label_CD_ARTI_ECO.Text, "Indice client")
            TextBox_IND_CLIE.Text = dt_CFGR_ARTI_ECO(0)("Indice client").ToString
            'DDM
            'Label_DDM.Text = DIG_FACT_SQL_GET_PARA(Label_CD_ARTI_ECO.Text, "Nom du DDM")
            Label_DDM.Text = dt_CFGR_ARTI_ECO(0)("Nom du DDM").ToString

            'vérification du paramétrage dans la base
            'Dim sQuery As String = "SELECT [VL_CTRL] 
            '                          FROM [APP_WEB_ECO].[dbo].[V_DER_MAJ_DTM_REF_PARA_ETAT_CRTL] 
            '                         WHERE [NM_PAGE] = '" & HttpContext.Current.CurrentHandler.ToString & "' AND [PARA] = '" & Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue & "'"
            'Dim sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=APP_WEB_ECO;Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
            'Dim dt As DataTable = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            'If dt Is Nothing Then
            '    LOG_Erreur(GetCurrentMethod, "La base n'est pas configurée pour les paramètres suivants : " & Label_CD_ARTI_ECO.Text & "; " & DropDownList_NM_ETI.SelectedValue)
            '    Exit Sub
            'End If

            'commande            
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "CheckBox_SAI_CMDE")
            If dt_ETAT_CTRL.Columns.Contains("CheckBox_SAI_CMDE") Then CheckBox_SAI_CMDE.Checked = CBool(dt_ETAT_CTRL(0)("CheckBox_SAI_CMDE").ToString)
            If CheckBox_SAI_CMDE.Checked = False Then
                TextBox_NU_CMDE.Enabled = False
            Else
                TextBox_NU_CMDE.Enabled = True
            End If

            'recherche du dernier carton
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "CheckBox_RECH_DERN_NU_CART")
            If dt_ETAT_CTRL.Columns.Contains("CheckBox_RECH_DERN_NU_CART") Then CheckBox_RECH_DERN_NU_CART.Checked = CBool(dt_ETAT_CTRL(0)("CheckBox_RECH_DERN_NU_CART").ToString)
            If CheckBox_RECH_DERN_NU_CART.Checked = True Then
                TextBox_NU_CART.Enabled = False
            Else
                TextBox_NU_CART.Enabled = True
            End If

            'numéro de série client auto généré
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "CheckBox_NU_SER_CLIE_GENE_AUTO")
            If dt_ETAT_CTRL.Columns.Contains("CheckBox_NU_SER_CLIE_GENE_AUTO") Then CheckBox_NU_SER_CLIE_GENE_AUTO.Checked = CBool(dt_ETAT_CTRL(0)("CheckBox_NU_SER_CLIE_GENE_AUTO").ToString)
            If CheckBox_NU_SER_CLIE_GENE_AUTO.Checked = True Then
                MultiView_GENE_NU_SER_CLIE.SetActiveView(View_PARA_GENE_CLIE)
                'TextBox_BASE_NU_CLIE.Text = DIG_FACT_SQL_GET_PARA(Label_CD_ARTI_ECO.Text, "Encodage Numéro de série client")
                TextBox_BASE_NU_CLIE.Text = dt_CFGR_ARTI_ECO(0)("Encodage Numéro de série client").ToString
                'TextBox_ICMT_NU_CLI.Text = DIG_FACT_SQL_GET_PARA(Label_CD_ARTI_ECO.Text, "Incrémentation flanc")
                TextBox_ICMT_NU_CLI.Text = dt_CFGR_ARTI_ECO(0)("Incrémentation flanc").ToString
                'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_FORM_NU_CLIE", View_PARA_GENE_CLIE)
                If dt_ETAT_CTRL.Columns.Contains("TextBox_FORM_NU_CLIE") Then TextBox_FORM_NU_CLIE.Text = dt_ETAT_CTRL(0)("TextBox_FORM_NU_CLIE").ToString
                'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "DropDownList_CRIT_GENE_NU_SER", View_PARA_GENE_CLIE)
                If dt_ETAT_CTRL.Columns.Contains("DropDownList_CRIT_GENE_NU_SER") Then DropDownList_CRIT_GENE_NU_SER.SelectedValue = dt_ETAT_CTRL(0)("DropDownList_CRIT_GENE_NU_SER").ToString
                'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "CheckBox_REPR_NU_SER_REBU", View_PARA_GENE_CLIE)
                If dt_ETAT_CTRL.Columns.Contains("CheckBox_REPR_NU_SER_REBU") Then CheckBox_REPR_NU_SER_REBU.Checked = CBool(dt_ETAT_CTRL(0)("CheckBox_REPR_NU_SER_REBU").ToString)
                TextBox_NU_SER_CLIE.Enabled = False
            End If

            'numéro de série eolane auto-généré         
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "CheckBox_NU_SER_ECO_GENE_AUTO")
            If dt_ETAT_CTRL.Columns.Contains("CheckBox_NU_SER_ECO_GENE_AUTO") Then CheckBox_NU_SER_ECO_GENE_AUTO.Checked = CBool(dt_ETAT_CTRL(0)("CheckBox_NU_SER_ECO_GENE_AUTO").ToString)
            If CheckBox_NU_SER_ECO_GENE_AUTO.Checked = True Then
                TextBox_NU_SER_ECO.Enabled = False
            End If

            'extraction du numéro de série Eolane
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "CheckBox_EXTR_NU_SER_ECO")
            If dt_ETAT_CTRL.Columns.Contains("CheckBox_EXTR_NU_SER_ECO") Then CheckBox_EXTR_NU_SER_ECO.Checked = CBool(dt_ETAT_CTRL(0)("CheckBox_EXTR_NU_SER_ECO").ToString)

            'date de production
            Label_DT_PROD.Text = dtAFKO(0)("GSTRS").ToString.Insert(6, "/").Insert(4, "/")
            'format date de production
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_FT_DT_PROD")
            If dt_ETAT_CTRL.Columns.Contains("TextBox_FT_DT_PROD") Then TextBox_FT_DT_PROD.Text = dt_ETAT_CTRL(0)("TextBox_FT_DT_PROD").ToString
            'format date d'expédition
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_FT_DT_EXP")
            If dt_ETAT_CTRL.Columns.Contains("TextBox_FT_DT_EXP") Then TextBox_FT_DT_EXP.Text = dt_ETAT_CTRL(0)("TextBox_FT_DT_EXP").ToString

            'variables
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_1")
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_2")
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_3")
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_4")
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_5")
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_6")
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_7")
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_8")
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_9")
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_10")
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_11")
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_12")
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_13")
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_14")
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_15")
            If dt_ETAT_CTRL.Columns.Contains("TextBox_VAR_1") Then TextBox_VAR_1.Text = dt_ETAT_CTRL(0)("TextBox_VAR_1").ToString
            If dt_ETAT_CTRL.Columns.Contains("TextBox_VAR_2") Then TextBox_VAR_2.Text = dt_ETAT_CTRL(0)("TextBox_VAR_2").ToString
            If dt_ETAT_CTRL.Columns.Contains("TextBox_VAR_3") Then TextBox_VAR_3.Text = dt_ETAT_CTRL(0)("TextBox_VAR_3").ToString
            If dt_ETAT_CTRL.Columns.Contains("TextBox_VAR_4") Then TextBox_VAR_4.Text = dt_ETAT_CTRL(0)("TextBox_VAR_4").ToString
            If dt_ETAT_CTRL.Columns.Contains("TextBox_VAR_5") Then TextBox_VAR_5.Text = dt_ETAT_CTRL(0)("TextBox_VAR_5").ToString
            If dt_ETAT_CTRL.Columns.Contains("TextBox_VAR_6") Then TextBox_VAR_6.Text = dt_ETAT_CTRL(0)("TextBox_VAR_6").ToString
            If dt_ETAT_CTRL.Columns.Contains("TextBox_VAR_7") Then TextBox_VAR_7.Text = dt_ETAT_CTRL(0)("TextBox_VAR_7").ToString
            If dt_ETAT_CTRL.Columns.Contains("TextBox_VAR_8") Then TextBox_VAR_8.Text = dt_ETAT_CTRL(0)("TextBox_VAR_8").ToString
            If dt_ETAT_CTRL.Columns.Contains("TextBox_VAR_9") Then TextBox_VAR_9.Text = dt_ETAT_CTRL(0)("TextBox_VAR_9").ToString
            If dt_ETAT_CTRL.Columns.Contains("TextBox_VAR_10") Then TextBox_VAR_10.Text = dt_ETAT_CTRL(0)("TextBox_VAR_10").ToString
            If dt_ETAT_CTRL.Columns.Contains("TextBox_VAR_11") Then TextBox_VAR_11.Text = dt_ETAT_CTRL(0)("TextBox_VAR_11").ToString
            If dt_ETAT_CTRL.Columns.Contains("TextBox_VAR_12") Then TextBox_VAR_12.Text = dt_ETAT_CTRL(0)("TextBox_VAR_12").ToString
            If dt_ETAT_CTRL.Columns.Contains("TextBox_VAR_13") Then TextBox_VAR_13.Text = dt_ETAT_CTRL(0)("TextBox_VAR_13").ToString
            If dt_ETAT_CTRL.Columns.Contains("TextBox_VAR_14") Then TextBox_VAR_14.Text = dt_ETAT_CTRL(0)("TextBox_VAR_14").ToString
            If dt_ETAT_CTRL.Columns.Contains("TextBox_VAR_15") Then TextBox_VAR_15.Text = dt_ETAT_CTRL(0)("TextBox_VAR_15").ToString

            'option générer la quantité totale de l'of
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "CheckBox_GENE_QT_TOTA_OF")
            If dt_ETAT_CTRL.Columns.Contains("CheckBox_GENE_QT_TOTA_OF") Then CheckBox_GENE_QT_TOTA_OF.Checked = CBool(dt_ETAT_CTRL(0)("CheckBox_GENE_QT_TOTA_OF").ToString)
            'Fichier modèle
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_FICH_MODE")
            If dt_ETAT_CTRL.Columns.Contains("TextBox_FICH_MODE") Then TextBox_FICH_MODE.Text = dt_ETAT_CTRL(0)("TextBox_FICH_MODE").ToString
            'Imprimante 
            TextBox_IMPR_RESE.Text = DIG_FACT_SQL_GET_PARA(System.Net.Dns.GetHostEntry(System.Web.HttpContext.Current.Request.UserHostAddress).HostName(), "Imprimante étiquette")
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_IMPR_RESE")
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Exit Sub
        End Try
    End Sub

    Protected Sub Button_IMPR_Click(sender As Object, e As EventArgs) Handles Button_IMPR.Click

        'Dim sFichier As String = ""

        'Dim sr As StreamReader
        'Randomize()
        'Dim sfich As String = My.Settings.RPTR_TPRR & "\" & CInt(Int((1000 * Rnd()) + 1)) & "_" & Path.GetFileName(TextBox_FICH_MODE.Text)
        'If File.Exists(sfich) Then My.Computer.FileSystem.DeleteFile(sfich)
        'Try
        '    COMM_APP_WEB_COPY_FILE(TextBox_FICH_MODE.Text, sfich, True)
        '    sr = New StreamReader(sfich, System.Text.Encoding.UTF8)
        '    sFichier = sr.ReadToEnd()
        '    sr.Close()
        'Catch ex As Exception
        '    LOG_Erreur(GetCurrentMethod, ex.Message)
        '    Exit Sub
        'End Try
        Dim dtvar As New DataTable
        dtvar.Columns.Add("0", Type.GetType("System.String"))
        dtvar.Columns.Add("1", Type.GetType("System.String"))
        dtvar.Rows.Add()
        dtvar.Rows(dtvar.Rows.Count - 1)("1") = TextBox_VAR_1.Text
        dtvar.Rows.Add()
        dtvar.Rows(dtvar.Rows.Count - 1)("1") = TextBox_VAR_2.Text
        dtvar.Rows.Add()
        dtvar.Rows(dtvar.Rows.Count - 1)("1") = TextBox_VAR_3.Text
        dtvar.Rows.Add()
        dtvar.Rows(dtvar.Rows.Count - 1)("1") = TextBox_VAR_4.Text
        dtvar.Rows.Add()
        dtvar.Rows(dtvar.Rows.Count - 1)("1") = TextBox_VAR_5.Text
        dtvar.Rows.Add()
        dtvar.Rows(dtvar.Rows.Count - 1)("1") = TextBox_VAR_6.Text
        dtvar.Rows.Add()
        dtvar.Rows(dtvar.Rows.Count - 1)("1") = TextBox_VAR_7.Text
        dtvar.Rows.Add()
        dtvar.Rows(dtvar.Rows.Count - 1)("1") = TextBox_VAR_8.Text
        dtvar.Rows.Add()
        dtvar.Rows(dtvar.Rows.Count - 1)("1") = TextBox_VAR_9.Text
        dtvar.Rows.Add()
        dtvar.Rows(dtvar.Rows.Count - 1)("1") = TextBox_VAR_10.Text
        dtvar.Rows.Add()
        dtvar.Rows(dtvar.Rows.Count - 1)("1") = TextBox_VAR_11.Text
        dtvar.Rows.Add()
        dtvar.Rows(dtvar.Rows.Count - 1)("1") = TextBox_VAR_12.Text
        dtvar.Rows.Add()
        dtvar.Rows(dtvar.Rows.Count - 1)("1") = TextBox_VAR_13.Text
        dtvar.Rows.Add()
        dtvar.Rows(dtvar.Rows.Count - 1)("1") = TextBox_VAR_14.Text
        dtvar.Rows.Add()
        dtvar.Rows(dtvar.Rows.Count - 1)("1") = TextBox_VAR_15.Text

        If CheckBox_GENE_QT_TOTA_OF.Checked = True Then
            For iEtiquette As Integer = 1 To Session("QT_OF")
                App_Web.Class_DIG_FACT.DIG_FACT_IMPR_ETIQ(TextBox_FICH_MODE.Text, TextBox_NU_OF.Text, TextBox_NU_BL.Text, DropDownList_NM_ETI.SelectedValue,
                                         TextBox_NU_SER_CLIE.Text, TextBox_NU_SER_ECO.Text, TextBox_NU_CART.Text, TextBox_NB_QT.Text,
                                         TextBox_NB_CART.Text, dtvar, Session("matricule"))
                'IMPR_ETIQ_PRN_IMPR(sFichier)
            Next
        Else
            App_Web.Class_DIG_FACT.DIG_FACT_IMPR_ETIQ(TextBox_FICH_MODE.Text, TextBox_NU_OF.Text, TextBox_NU_BL.Text, DropDownList_NM_ETI.SelectedValue,
                                         TextBox_NU_SER_CLIE.Text, TextBox_NU_SER_ECO.Text, TextBox_NU_CART.Text, TextBox_NB_QT.Text,
                                         TextBox_NB_CART.Text, dtvar, Session("matricule"))
            'IMPR_ETIQ_PRN_IMPR(sFichier)
        End If

    End Sub

    Protected Sub TextBox_NU_BL_TextChanged(sender As Object, e As EventArgs) Handles TextBox_NU_BL.TextChanged
        'chercher les infos

        Dim dtMARA, dtT179T, dtLIPS, dtLIPSUP, dt, dt_CFGR_ARTI_ECO, dt_ETAT_CTRL As New DataTable
        Try
            dtLIPSUP = SAP_DATA_READ_LIPSUP("VBELN EQ " & TextBox_NU_BL.Text)
            If dtLIPSUP Is Nothing Then Throw New Exception("Pas de données pour le BL n°" & TextBox_NU_BL.Text)
            'code article eolane
            Label_CD_ARTI_ECO.Text = dtLIPSUP(0)("MATNR").ToString
            dt_CFGR_ARTI_ECO = App_Web.Class_DIG_FACT_SQL.DIG_FACT_SQL_CFGR_ARTI_ECO(Trim(Label_CD_ARTI_ECO.Text))
            If dt_CFGR_ARTI_ECO Is Nothing Then Throw New Exception("La base Digital Factory n'a pas été configurée pour l'article " & Trim(Label_CD_ARTI_ECO.Text))
            dt_ETAT_CTRL = COMM_APP_WEB_ETAT_CTRL(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue)
            If dt_ETAT_CTRL Is Nothing Then Throw New Exception("La base App_Web_Eco n'as pas été configurée pour l'article " & Trim(Label_CD_ARTI_ECO.Text))

            'désignation article
            Label_NM_DSGT_ARTI.Text = dtLIPSUP(0)("ARKTX").ToString
            dtMARA = SAP_DATA_READ_MARA("MATNR EQ '" & Label_CD_ARTI_ECO.Text & "'")
            dtT179T = SAP_DATA_READ_T179T("PRODH EQ '" & dtMARA(0)("PRDHA").ToString & "'")
            'client
            Session("Client") = dtT179T(0)("VTEXT").ToString
            'code article client
            'TextBox_CD_ARTI_CLIE.Text = DIG_FACT_SQL_GET_PARA(Label_CD_ARTI_ECO.Text, "Code article client")
            TextBox_CD_ARTI_CLIE.Text = dt_CFGR_ARTI_ECO(0)("Code article client").ToString
            'code fournisseur
            TextBox_CD_FNS.Text = DIG_FACT_SQL_GET_PARA(Session("Client"), "Format du code fournisseur")
            'Indice Client
            'TextBox_IND_CLIE.Text = DIG_FACT_SQL_GET_PARA(Label_CD_ARTI_ECO.Text, "Indice client")
            TextBox_IND_CLIE.Text = dt_CFGR_ARTI_ECO(0)("Indice client").ToString
            'DDM
            'Label_DDM.Text = DIG_FACT_SQL_GET_PARA(Label_CD_ARTI_ECO.Text, "Nom du DDM")
            Label_DDM.Text = dt_CFGR_ARTI_ECO(0)("Nom du DDM").ToString
            'vérification du paramétrage dans la base
            'Dim sQuery As String = "SELECT [VL_CTRL] 
            '                      FROM [APP_WEB_ECO].[dbo].[V_DER_MAJ_DTM_REF_PARA_ETAT_CRTL] 
            '                     WHERE [NM_PAGE] = '" & HttpContext.Current.CurrentHandler.ToString & "' AND [PARA] = '" & Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue & "'"
            'Dim sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=APP_WEB_ECO;Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
            'dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            'If dt Is Nothing Then Throw New Exception("La base n'est pas configurée pour les paramètres suivants : " & Label_CD_ARTI_ECO.Text & "; " & DropDownList_NM_ETI.SelectedValue)

            'commande client
            dtLIPS = SAP_DATA_READ_LIPS("VBELN EQ " & TextBox_NU_BL.Text)
            If Not dtLIPS Is Nothing Then TextBox_NU_CMDE.Text = dtLIPS(0)("VGBEL").ToString
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "CheckBox_SAI_CMDE")
            If dt_ETAT_CTRL.Columns.Contains("CheckBox_SAI_CMDE") Then CheckBox_SAI_CMDE.Checked = CBool(dt_ETAT_CTRL(0)("CheckBox_SAI_CMDE").ToString)
            If CheckBox_SAI_CMDE.Checked = False Then
                TextBox_NU_CMDE.Enabled = False
            Else
                TextBox_NU_CMDE.Enabled = True
            End If

            'format date de production
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_FT_DT_PROD")
            If dt_ETAT_CTRL.Columns.Contains("TextBox_FT_DT_PROD") Then TextBox_FT_DT_PROD.Text = dt_ETAT_CTRL(0)("TextBox_FT_DT_PROD").ToString
            'format date d'expédition
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_FT_DT_EXP")
            If dt_ETAT_CTRL.Columns.Contains("TextBox_FT_DT_EXP") Then TextBox_FT_DT_EXP.Text = dt_ETAT_CTRL(0)("TextBox_FT_DT_EXP").ToString

            'variables
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_1")
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_2")
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_3")
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_4")
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_5")
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_6")
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_7")
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_8")
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_9")
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_10")
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_11")
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_12")
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_13")
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_14")
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_15")
            If dt_ETAT_CTRL.Columns.Contains("TextBox_VAR_1") Then TextBox_VAR_1.Text = dt_ETAT_CTRL(0)("TextBox_VAR_1").ToString
            If dt_ETAT_CTRL.Columns.Contains("TextBox_VAR_2") Then TextBox_VAR_2.Text = dt_ETAT_CTRL(0)("TextBox_VAR_2").ToString
            If dt_ETAT_CTRL.Columns.Contains("TextBox_VAR_3") Then TextBox_VAR_3.Text = dt_ETAT_CTRL(0)("TextBox_VAR_3").ToString
            If dt_ETAT_CTRL.Columns.Contains("TextBox_VAR_4") Then TextBox_VAR_4.Text = dt_ETAT_CTRL(0)("TextBox_VAR_4").ToString
            If dt_ETAT_CTRL.Columns.Contains("TextBox_VAR_5") Then TextBox_VAR_5.Text = dt_ETAT_CTRL(0)("TextBox_VAR_5").ToString
            If dt_ETAT_CTRL.Columns.Contains("TextBox_VAR_6") Then TextBox_VAR_6.Text = dt_ETAT_CTRL(0)("TextBox_VAR_6").ToString
            If dt_ETAT_CTRL.Columns.Contains("TextBox_VAR_7") Then TextBox_VAR_7.Text = dt_ETAT_CTRL(0)("TextBox_VAR_7").ToString
            If dt_ETAT_CTRL.Columns.Contains("TextBox_VAR_8") Then TextBox_VAR_8.Text = dt_ETAT_CTRL(0)("TextBox_VAR_8").ToString
            If dt_ETAT_CTRL.Columns.Contains("TextBox_VAR_9") Then TextBox_VAR_9.Text = dt_ETAT_CTRL(0)("TextBox_VAR_9").ToString
            If dt_ETAT_CTRL.Columns.Contains("TextBox_VAR_10") Then TextBox_VAR_10.Text = dt_ETAT_CTRL(0)("TextBox_VAR_10").ToString
            If dt_ETAT_CTRL.Columns.Contains("TextBox_VAR_11") Then TextBox_VAR_11.Text = dt_ETAT_CTRL(0)("TextBox_VAR_11").ToString
            If dt_ETAT_CTRL.Columns.Contains("TextBox_VAR_12") Then TextBox_VAR_12.Text = dt_ETAT_CTRL(0)("TextBox_VAR_12").ToString
            If dt_ETAT_CTRL.Columns.Contains("TextBox_VAR_13") Then TextBox_VAR_13.Text = dt_ETAT_CTRL(0)("TextBox_VAR_13").ToString
            If dt_ETAT_CTRL.Columns.Contains("TextBox_VAR_14") Then TextBox_VAR_14.Text = dt_ETAT_CTRL(0)("TextBox_VAR_14").ToString
            If dt_ETAT_CTRL.Columns.Contains("TextBox_VAR_15") Then TextBox_VAR_15.Text = dt_ETAT_CTRL(0)("TextBox_VAR_15").ToString
            'Fichier modèle
            'COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_FICH_MODE")
            If dt_ETAT_CTRL.Columns.Contains("TextBox_FICH_MODE") Then TextBox_FICH_MODE.Text = dt_ETAT_CTRL(0)("TextBox_FICH_MODE").ToString

            'Imprimante 
            TextBox_IMPR_RESE.Text = DIG_FACT_SQL_GET_PARA(System.Net.Dns.GetHostEntry(System.Web.HttpContext.Current.Request.UserHostAddress).HostName(), "Imprimante étiquette")
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try
    End Sub

    Protected Sub TextBox_FICH_MODE_TextChanged(sender As Object, e As EventArgs) Handles TextBox_FICH_MODE.TextChanged
        If Label_CD_ARTI_ECO.Text <> "" And DropDownList_NM_ETI.SelectedValue <> "" Then COMM_APP_WEB_PARA_AFFI_SAVE(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_FICH_MODE", TextBox_FICH_MODE.Text)
    End Sub

    Protected Sub TextBox_FT_DT_PROD_TextChanged(sender As Object, e As EventArgs) Handles TextBox_FT_DT_PROD.TextChanged
        If Label_CD_ARTI_ECO.Text <> "" And DropDownList_NM_ETI.SelectedValue <> "" Then COMM_APP_WEB_PARA_AFFI_SAVE(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_FT_DT_PROD", TextBox_FT_DT_PROD.Text)
    End Sub

    'Protected Sub TextBox_FT_DT_EXP_TextChanged(sender As Object, e As EventArgs) Handles TextBox_FT_DT_EXP.TextChanged
    '    If Label_CD_ARTI_ECO.Text <> "" And DropDownList_NM_ETI.SelectedValue <> "" Then COMM_APP_WEB_PARA_AFFI_SAVE(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_FT_DT_EXP", TextBox_FT_DT_EXP.Text)
    'End Sub

    Protected Sub CheckBox_NU_SER_CLIE_GENE_AUTO_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox_NU_SER_CLIE_GENE_AUTO.CheckedChanged
        If CheckBox_NU_SER_CLIE_GENE_AUTO.Checked = True Then
            MultiView_GENE_NU_SER_CLIE.SetActiveView(View_PARA_GENE_CLIE)
            TextBox_NU_SER_CLIE.Enabled = False
        Else
            MultiView_GENE_NU_SER_CLIE.SetActiveView(View_VOID_GENE_CLIE)
            TextBox_NU_SER_CLIE.Enabled = True
        End If
        If Label_CD_ARTI_ECO.Text <> "" And DropDownList_NM_ETI.SelectedValue <> "" Then COMM_APP_WEB_PARA_AFFI_SAVE(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "CheckBox_NU_SER_CLIE_GENE_AUTO", CheckBox_NU_SER_CLIE_GENE_AUTO.Checked)
    End Sub

    Protected Sub TextBox_FORM_NU_CLIE_TextChanged(sender As Object, e As EventArgs) Handles TextBox_FORM_NU_CLIE.TextChanged
        If Label_CD_ARTI_ECO.Text <> "" And DropDownList_NM_ETI.SelectedValue <> "" Then COMM_APP_WEB_PARA_AFFI_SAVE(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_FORM_NU_CLIE", TextBox_FORM_NU_CLIE.Text)
    End Sub

    Protected Sub DropDownList_CRIT_GENE_NU_SER_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList_CRIT_GENE_NU_SER.SelectedIndexChanged
        If Label_CD_ARTI_ECO.Text <> "" And DropDownList_NM_ETI.SelectedValue <> "" Then COMM_APP_WEB_PARA_AFFI_SAVE(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "DropDownList_CRIT_GENE_NU_SER", DropDownList_CRIT_GENE_NU_SER.SelectedValue)
    End Sub

    'Public Function IMPR_ETIQ_PRN_GENE_NU_SER_CLIE(sBase As String, sInc As String, sFormat As String, sCritère As String, sTypeEtiquette As String) As String

    '    Dim sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=MES_Digital_Factory;Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
    '    Dim sQuerySql As String = "", sNU_SER_BASE_N As String = "", sNU_SER_GENE As String = ""
    '    Dim dtDER_NU_SER, dtNU_REGE As New DataTable
    '    Dim iNU_SER_DEC, iNB_CARA As Integer, iPos_Car As Integer = 1
    '    Try
    '        If sBase = "" Or sInc = "" Or sFormat = "" Then Throw New Exception("un champ de configuration du numéro de série client est vide")
    '        'cas particulier pour médria MEDEM2IAX$, incrémentation unique pour les codes articles MEDEM2IAX-EU$ et MEDEM2IAX-US$
    '        sCritère = Replace(sCritère, "M2IAX-EU$", "M2IAX$")
    '        sCritère = Replace(sCritère, "M2IAX-US$", "M2IAX$")

    '        'récupérer le premier numéro de série rebuté à réimprimer
    '        sQuerySql = "SELECT     MIN(NU_SER) AS [NU_REGE]
    '                       FROM         dbo.V_NU_SER_A_RGNR
    '                     GROUP BY NM_CRIT
    '                     HAVING      (NM_CRIT = '" & sCritère & "')"
    '        dtNU_REGE = SQL_SELE_TO_DT(sQuerySql, sChaineConnexion)
    '        If CheckBox_REPR_NU_SER_REBU.Checked = True And Not dtNU_REGE.Rows.Count = 0 Then
    '            sNU_SER_GENE = dtNU_REGE(0)("NU_REGE").ToString
    '        Else
    '            'récupérer le dernier généré
    '            sQuerySql = "SELECT [NU_SER_DERN]
    '                           FROM [dbo].[V_DER_DTM_NU_SER]
    '                          WHERE [NM_CRIT] = '" & sCritère & "' AND [NM_TYPE] = '" & sTypeEtiquette & "'"
    '            dtDER_NU_SER = SQL_SELE_TO_DT(sQuerySql, sChaineConnexion)            '
    '            'convertir en base 10
    '            If dtDER_NU_SER.Rows.Count = 0 Then
    '                iNU_SER_DEC = COMM_APP_WEB_CONV_BASE_N_2_DEC("1", Convert.ToDecimal(sBase))
    '            Else
    '                iNU_SER_DEC = COMM_APP_WEB_CONV_BASE_N_2_DEC(dtDER_NU_SER(0)("NU_SER_DERN").ToString, Convert.ToDecimal(sBase))
    '            End If

    '            'incrémenter
    '            iNU_SER_DEC += Convert.ToDecimal(sInc)
    '            'convertir en base N
    '            For iChar As Integer = 1 To Len(sFormat)
    '                If Mid(sFormat, iChar, 1) = "%" Then iNB_CARA += 1
    '            Next
    '            sNU_SER_BASE_N = COMM_APP_WEB_CONV_DEC_2_BASE_N(iNU_SER_DEC, Convert.ToDecimal(sBase), iNB_CARA)
    '            'appliquer le format
    '            For iChar As Integer = 1 To Len(sFormat)
    '                If Mid(sFormat, iChar, 1) = "%" Then
    '                    sNU_SER_GENE = sNU_SER_GENE & Mid(sNU_SER_BASE_N, iPos_Car, 1)
    '                    iPos_Car += 1
    '                Else
    '                    sNU_SER_GENE = sNU_SER_GENE & Mid(sFormat, iChar, 1)
    '                End If
    '            Next
    '        End If
    '        'Enregistrer dans la base
    '        sQuerySql = "INSERT INTO [dbo].[DTM_NU_SER] ([NM_CRIT], [NU_SER], [NM_TYPE], [DT_CREA])
    '                          VALUES ('" & sCritère & "', '" & sNU_SER_GENE & "', '" & sTypeEtiquette & "', GETDATE())"
    '        SQL_REQ_ACT(sQuerySql, sChaineConnexion)
    '        DIG_FACT_SQL_P_ADD_PSG_DTM_PSG("Impression numéro de série client", Now(), Now(), System.Net.Dns.GetHostEntry(System.Web.HttpContext.Current.Request.UserHostAddress).HostName(), HttpContext.Current.CurrentHandler.ToString, Session("matricule"), "", sNU_SER_GENE, "P", Convert.ToDecimal(TextBox_NU_OF.Text))
    '    Catch ex As Exception
    '        LOG_Erreur(GetCurrentMethod, ex.Message)
    '        Return Nothing
    '    End Try

    '    LOG_Msg(GetCurrentMethod, "Le numéro de série client " & sNU_SER_GENE & " a été généré.")
    '    Return sNU_SER_GENE

    'End Function

    Protected Sub CheckBox_RECH_DERN_NU_CART_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox_RECH_DERN_NU_CART.CheckedChanged
        If Label_CD_ARTI_ECO.Text <> "" And DropDownList_NM_ETI.SelectedValue <> "" Then COMM_APP_WEB_PARA_AFFI_SAVE(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "CheckBox_RECH_DERN_NU_CART", CheckBox_RECH_DERN_NU_CART.Checked)
        If CheckBox_RECH_DERN_NU_CART.Checked = True Then
            TextBox_NU_CART.Enabled = False
        Else
            TextBox_NU_CART.Enabled = True
        End If
    End Sub

    Protected Sub CheckBox_SAI_CMDE_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox_SAI_CMDE.CheckedChanged
        If Label_CD_ARTI_ECO.Text <> "" And DropDownList_NM_ETI.SelectedValue <> "" Then COMM_APP_WEB_PARA_AFFI_SAVE(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "CheckBox_SAI_CMDE", CheckBox_SAI_CMDE.Checked)
        If CheckBox_SAI_CMDE.Checked = False Then
            TextBox_NU_CMDE.Enabled = False
        Else
            TextBox_NU_CMDE.Enabled = True
        End If
    End Sub

    Protected Sub CheckBox_GENE_ETIQ_IDEN_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox_GENE_ETIQ_IDEN.CheckedChanged
        If Label_CD_ARTI_ECO.Text <> "" And DropDownList_NM_ETI.SelectedValue <> "" Then COMM_APP_WEB_PARA_AFFI_SAVE(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "CheckBox_GENE_ETIQ_IDEN", CheckBox_GENE_ETIQ_IDEN.Checked)
    End Sub

    Protected Sub CheckBox_GENE_QT_TOTA_OF_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox_GENE_QT_TOTA_OF.CheckedChanged
        If Label_CD_ARTI_ECO.Text <> "" And DropDownList_NM_ETI.SelectedValue <> "" And (CheckBox_NU_SER_CLIE_GENE_AUTO.Checked = True Or CheckBox_NU_SER_ECO_GENE_AUTO.Checked = True) Then
            COMM_APP_WEB_PARA_AFFI_SAVE(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "CheckBox_GENE_QT_TOTA_OF", CheckBox_GENE_QT_TOTA_OF.Checked)
        Else
            If CheckBox_NU_SER_CLIE_GENE_AUTO.Checked = False And CheckBox_NU_SER_ECO_GENE_AUTO.Checked = False Then
                CheckBox_GENE_QT_TOTA_OF.Checked = False
                LOG_Erreur(GetCurrentMethod, "Impossible de sélectionner la génération autoaùmtique de l'OF si l'une des options suivantes n'a pas été cochée : Générer automatiquement le numéro de série client ou Générer automatiquement le numéro de série Eolane")
            End If
        End If
    End Sub

    Protected Sub CheckBox_REPR_NU_SER_REBU_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox_REPR_NU_SER_REBU.CheckedChanged
        If Label_CD_ARTI_ECO.Text <> "" And DropDownList_NM_ETI.SelectedValue <> "" Then COMM_APP_WEB_PARA_AFFI_SAVE(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "CheckBox_REPR_NU_SER_REBU", CheckBox_REPR_NU_SER_REBU.Checked)
    End Sub

    'Public Sub IMPR_ETIQ_PRN_IMPR(sData As String)
    '    Dim sData_Crit As String = ""
    '    Dim sNU_CLIE As String = "", sNU_ECO As String = ""
    '    Try
    '        sData = Replace(sData, "#NU_OF", TextBox_NU_OF.Text)
    '        sData = Replace(sData, "#CD_ARTI_ECO", Label_CD_ARTI_ECO.Text)
    '        sData = Replace(sData, "#NM_DSGT_ARTI", Label_NM_DSGT_ARTI.Text)
    '        sData = Replace(sData, "#CD_ARTI_CLIE", TextBox_CD_ARTI_CLIE.Text)
    '        sData = Replace(sData, "#NU_CMDE", TextBox_NU_CMDE.Text)
    '        sData = Replace(sData, "#NU_BL", TextBox_NU_BL.Text)
    '        sData = Replace(sData, "#NU_CART", TextBox_NU_CART.Text)
    '        sData = Replace(sData, "#CD_FNS", TextBox_CD_FNS.Text)

    '        Select Case DropDownList_CRIT_GENE_NU_SER.SelectedValue
    '            Case "OF"
    '                sData_Crit = TextBox_NU_OF.Text
    '            Case "Code Article"
    '                sData_Crit = Label_CD_ARTI_ECO.Text
    '            Case "Client"
    '                sData_Crit = Session("Client")
    '        End Select
    '        If CheckBox_NU_SER_CLIE_GENE_AUTO.Checked = True Then
    '            sNU_CLIE = IMPR_ETIQ_PRN_GENE_NU_SER_CLIE(TextBox_BASE_NU_CLIE.Text, TextBox_ICMT_NU_CLI.Text, TextBox_FORM_NU_CLIE.Text, sData_Crit, DropDownList_NM_ETI.SelectedValue)
    '            If sNU_CLIE Is Nothing Then Throw New Exception("Erreur dans la génération du numéro de série client")
    '            TextBox_NU_SER_CLIE.Text = sNU_CLIE
    '        End If
    '        sData = Replace(sData, "#NU_SER_CLIE", TextBox_NU_SER_CLIE.Text)

    '        sData = Replace(sData, "#NU_SER_ECO", TextBox_NU_SER_ECO.Text)
    '        If CheckBox_EXTR_NU_SER_ECO.Checked = True Then
    '            Label_EXTR_NU_SER_ECO.Text = Mid(TextBox_NU_SER_ECO.Text, TextBox_NU_SER_ECO.Text.IndexOf(TextBox_NU_OF.Text) + 1, 13)
    '            sData = Replace(sData, "#EXTR_NU_SER_ECO", Label_EXTR_NU_SER_ECO.Text)
    '        End If

    '        sData = Replace(sData, "#NB_QT", TextBox_NB_QT.Text)
    '        sData = Replace(sData, "#NB_CART", TextBox_NB_CART.Text)

    '        Label_FT_DT_PROD.Text = COMM_APP_WEB_CONV_FORM_DATE(Label_DT_PROD.Text, TextBox_FT_DT_PROD.Text)
    '        sData = Replace(sData, "#DT_PROD", Label_FT_DT_PROD.Text)
    '        Label_FT_DT_EXP.Text = COMM_APP_WEB_CONV_FORM_DATE(Label_DT_EXP.Text, TextBox_FT_DT_EXP.Text)
    '        sData = Replace(sData, "#DT_EXP", Label_FT_DT_EXP.Text)
    '        sData = Replace(sData, "#IND_CLIE", TextBox_IND_CLIE.Text)
    '        sData = Replace(sData, "#DDM", Label_DDM.Text)

    '        sData = Replace(sData, "#var1", TextBox_VAR_1.Text)
    '        sData = Replace(sData, "#var2", TextBox_VAR_2.Text)
    '        sData = Replace(sData, "#var3", TextBox_VAR_3.Text)
    '        sData = Replace(sData, "#var4", TextBox_VAR_4.Text)
    '        sData = Replace(sData, "#var5", TextBox_VAR_5.Text)
    '        sData = Replace(sData, "#var6", TextBox_VAR_6.Text)
    '        sData = Replace(sData, "#var7", TextBox_VAR_7.Text)
    '        sData = Replace(sData, "#var8", TextBox_VAR_8.Text)
    '        sData = Replace(sData, "#var9", TextBox_VAR_9.Text)
    '        sData = Replace(sData, "#varA", TextBox_VAR_10.Text)
    '        sData = Replace(sData, "#varB", TextBox_VAR_11.Text)
    '        sData = Replace(sData, "#varC", TextBox_VAR_12.Text)
    '        sData = Replace(sData, "#varD", TextBox_VAR_13.Text)
    '        sData = Replace(sData, "#varE", TextBox_VAR_14.Text)
    '        sData = Replace(sData, "#varF", TextBox_VAR_15.Text)
    '        Randomize()
    '        Dim sfich As String = My.Settings.RPTR_TPRR & "\" & CInt(Int((1000 * Rnd()) + 1)) & "_" & Path.GetFileName(TextBox_FICH_MODE.Text)
    '        Dim sw As New StreamWriter(sfich, False, System.Text.Encoding.UTF8)
    '        sw.WriteLine(sData)
    '        sw.Close()
    '        COMM_APP_WEB_IMPR_ETIQ_PRN(sfich, TextBox_IMPR_RESE.Text)
    '    Catch ex As Exception
    '        LOG_Erreur(GetCurrentMethod, ex.Message)
    '        Exit Sub
    '    End Try
    'End Sub

    Protected Sub CheckBox_EXTR_NU_SER_ECO_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox_EXTR_NU_SER_ECO.CheckedChanged
        If Label_CD_ARTI_ECO.Text <> "" And DropDownList_NM_ETI.SelectedValue <> "" Then COMM_APP_WEB_PARA_AFFI_SAVE(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "CheckBox_EXTR_NU_SER_ECO", CheckBox_EXTR_NU_SER_ECO.Checked)
    End Sub

    Protected Sub Button_VAR_1_Click(sender As Object, e As EventArgs) Handles Button_VAR_1.Click
        If Label_CD_ARTI_ECO.Text <> "" And DropDownList_NM_ETI.SelectedValue <> "" Then COMM_APP_WEB_PARA_AFFI_SAVE(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_1", TextBox_VAR_1.Text)
    End Sub

    Protected Sub Button_VAR_2_Click(sender As Object, e As EventArgs) Handles Button_VAR_2.Click
        If Label_CD_ARTI_ECO.Text <> "" And DropDownList_NM_ETI.SelectedValue <> "" Then COMM_APP_WEB_PARA_AFFI_SAVE(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_2", TextBox_VAR_2.Text)
    End Sub

    Protected Sub Button_VAR_3_Click(sender As Object, e As EventArgs) Handles Button_VAR_3.Click
        If Label_CD_ARTI_ECO.Text <> "" And DropDownList_NM_ETI.SelectedValue <> "" Then COMM_APP_WEB_PARA_AFFI_SAVE(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_3", TextBox_VAR_3.Text)
    End Sub

    Protected Sub Button_VAR_4_Click(sender As Object, e As EventArgs) Handles Button_VAR_4.Click
        If Label_CD_ARTI_ECO.Text <> "" And DropDownList_NM_ETI.SelectedValue <> "" Then COMM_APP_WEB_PARA_AFFI_SAVE(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_4", TextBox_VAR_4.Text)
    End Sub

    Protected Sub Button_VAR_5_Click(sender As Object, e As EventArgs) Handles Button_VAR_5.Click
        If Label_CD_ARTI_ECO.Text <> "" And DropDownList_NM_ETI.SelectedValue <> "" Then COMM_APP_WEB_PARA_AFFI_SAVE(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_5", TextBox_VAR_5.Text)
    End Sub

    Protected Sub Button_VAR_6_Click(sender As Object, e As EventArgs) Handles Button_VAR_6.Click
        If Label_CD_ARTI_ECO.Text <> "" And DropDownList_NM_ETI.SelectedValue <> "" Then COMM_APP_WEB_PARA_AFFI_SAVE(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_6", TextBox_VAR_6.Text)
    End Sub

    Protected Sub Button_VAR_7_Click(sender As Object, e As EventArgs) Handles Button_VAR_7.Click
        If Label_CD_ARTI_ECO.Text <> "" And DropDownList_NM_ETI.SelectedValue <> "" Then COMM_APP_WEB_PARA_AFFI_SAVE(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_7", TextBox_VAR_7.Text)
    End Sub

    Protected Sub Button_VAR_8_Click(sender As Object, e As EventArgs) Handles Button_VAR_8.Click
        If Label_CD_ARTI_ECO.Text <> "" And DropDownList_NM_ETI.SelectedValue <> "" Then COMM_APP_WEB_PARA_AFFI_SAVE(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_8", TextBox_VAR_8.Text)
    End Sub

    Protected Sub Button_VAR_9_Click(sender As Object, e As EventArgs) Handles Button_VAR_9.Click
        If Label_CD_ARTI_ECO.Text <> "" And DropDownList_NM_ETI.SelectedValue <> "" Then COMM_APP_WEB_PARA_AFFI_SAVE(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_9", TextBox_VAR_9.Text)
    End Sub

    Protected Sub Button_VAR_10_Click(sender As Object, e As EventArgs) Handles Button_VAR_10.Click
        If Label_CD_ARTI_ECO.Text <> "" And DropDownList_NM_ETI.SelectedValue <> "" Then COMM_APP_WEB_PARA_AFFI_SAVE(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_10", TextBox_VAR_10.Text)
    End Sub

    Protected Sub Button_VAR_11_Click(sender As Object, e As EventArgs) Handles Button_VAR_11.Click
        If Label_CD_ARTI_ECO.Text <> "" And DropDownList_NM_ETI.SelectedValue <> "" Then COMM_APP_WEB_PARA_AFFI_SAVE(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_11", TextBox_VAR_11.Text)
    End Sub

    Protected Sub Button_VAR_12_Click(sender As Object, e As EventArgs) Handles Button_VAR_12.Click
        If Label_CD_ARTI_ECO.Text <> "" And DropDownList_NM_ETI.SelectedValue <> "" Then COMM_APP_WEB_PARA_AFFI_SAVE(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_12", TextBox_VAR_12.Text)
    End Sub

    Protected Sub Button_VAR_13_Click(sender As Object, e As EventArgs) Handles Button_VAR_13.Click
        If Label_CD_ARTI_ECO.Text <> "" And DropDownList_NM_ETI.SelectedValue <> "" Then COMM_APP_WEB_PARA_AFFI_SAVE(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_13", TextBox_VAR_13.Text)
    End Sub

    Protected Sub Button_VAR_14_Click(sender As Object, e As EventArgs) Handles Button_VAR_14.Click
        If Label_CD_ARTI_ECO.Text <> "" And DropDownList_NM_ETI.SelectedValue <> "" Then COMM_APP_WEB_PARA_AFFI_SAVE(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_14", TextBox_VAR_14.Text)
    End Sub

    Protected Sub Button_VAR_15_Click(sender As Object, e As EventArgs) Handles Button_VAR_15.Click
        If Label_CD_ARTI_ECO.Text <> "" And DropDownList_NM_ETI.SelectedValue <> "" Then COMM_APP_WEB_PARA_AFFI_SAVE(Label_CD_ARTI_ECO.Text & "|" & DropDownList_NM_ETI.SelectedValue, "TextBox_VAR_15", TextBox_VAR_15.Text)
    End Sub
End Class