Imports App_Web.LOG
Imports System.Data
Imports System.Reflection.MethodBase
Imports System.IO
Imports System.Data.SqlClient
Imports App_Web.Class_SAP_DATA
Imports App_Web.Class_EXCEL
Imports App_Web.Class_WORD
Imports App_Web.Class_SQL
Imports App_Web.Class_COMM_APP_WEB
Public Class Comparaison_BOM
    Inherits Page
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            Dim dtListe_Client As DataTable = SAP_DATA_LIST_CLIE("SPRAS EQ 'F'")
            Dim qNMCT_ARTI_GROUP = From rVTEXT In dtListe_Client
                                   Where Len(rVTEXT.Field(Of String)("VTEXT")) > 1
                                   Order By rVTEXT.Field(Of String)("VTEXT")
                                   Group rVTEXT By N_VTEXT = rVTEXT.Field(Of String)("VTEXT") Into N_VTEXT_GROUP = Group Select New With {Key N_VTEXT}
            DropDownList_Client.DataSource = qNMCT_ARTI_GROUP
            DropDownList_Client.DataTextField = "N_VTEXT"
            DropDownList_Client.DataValueField = "N_VTEXT"
            DropDownList_Client.DataBind()

            'Session("ID_COMP")

        End If

        AFF_LOAD("cacher")

    End Sub

    Protected Sub DropDownList_Client_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList_Client.SelectedIndexChanged
        Dim dtListe_Article_Client As DataTable = SAP_DATA_LIST_ARTI_CLIE(DropDownList_Client.SelectedValue, "SEMI-FINI")
        Dim dt_prod As DataTable = SAP_DATA_LIST_ARTI_CLIE(DropDownList_Client.SelectedValue, "PRODUIT")
        If dtListe_Article_Client Is Nothing Then
            dtListe_Article_Client = dt_prod
        ElseIf Not dt_prod Is Nothing Then
            dtListe_Article_Client.Merge(dt_prod)
        End If

        Dim dvListe_Article_Client As New DataView(dtListe_Article_Client)
        dvListe_Article_Client.Sort = "MATNR ASC"
        DropDownList_Article.DataSource = dvListe_Article_Client
        DropDownList_Article.DataTextField = "MATNR"
        DropDownList_Article.DataValueField = "MATNR"
        DropDownList_Article.DataBind()
    End Sub

    Protected Sub Button_Importer_Click(sender As Object, e As EventArgs) Handles Button_Importer.Click

        Dim savePath As String = "c:\sources\temp_App_Web\"

        Try
            If Not (FileUpload_BOM.HasFile) Then Throw New Exception("pas de fichier sélectionné")
            savePath += Server.HtmlEncode(FileUpload_BOM.FileName)
            FileUpload_BOM.SaveAs(savePath)
            FileUpload_BOM.Dispose()

            Select Case Right(savePath, 3)
                Case "PDF", "pdf"
                    'WORD_PDF2TXT(savePath, "c:\sources\temp_App_Web\essai.txt")
                Case "xls", "lsx" 'récupération des onglets du fichier excel
                    Dim dt_ONGL_EXCE As DataTable = EXCE_LIST_ONGL(savePath)
                    If dt_ONGL_EXCE Is Nothing Then Throw New Exception("Aucun onglet trouvé")
                    For Each r_ONGL_EXCE As DataRow In dt_ONGL_EXCE.Rows
                        DropDownList_ONGL_BOM_CLIE.Items.Add(New ListItem(r_ONGL_EXCE("Onglet").ToString, r_ONGL_EXCE("Onglet").ToString))
                    Next
                    MultiView_COMP_BOM.SetActiveView(View_ONGL) 'naviguer vers la vue suivante (View_ONGL)
                Case "csv" 'récupération des données d'un fichier csv
                    Session("dtBOM_CLIE") = COMM_APP_WEB_IMP_CSV_DT(savePath)
                    GridView_FICH_BOM_CLIE.DataSource = Session("dtBOM_CLIE")
                    GridView_FICH_BOM_CLIE.DataBind()
                    MultiView_COMP_BOM.SetActiveView(View_SEL_DONN)
                Case Else
                    Throw New Exception("Le fichier sélectionné n'a pas les extensions suivantes : PDF, pdf, xls, xlsx")
            End Select

            'importer la nomenclature sous SAP
            SQL_REQ_ACT("DELETE FROM [APP_WEB_ECO].[dbo].[DWH_CPRS_BOM_PIVO_SAP]", "Data Source=cedb03, 1433;Initial Catalog=APP_WEB_ECO;Persist Security Info=True;User ID=sa;Password=mdpsa@SQL")
            Dim dtNomenclature As DataTable = SAP_DATA_NMCT_ARTI(DropDownList_Article.SelectedValue)
            Dim dtNomenclature_FILT As DataTable = dtNomenclature.Select("[Fab. Interne/Externe] <> 'F'").CopyToDataTable
            dtNomenclature_FILT.Columns("Repère").ColumnName = "NM_REPE"
            dtNomenclature_FILT.Columns("Composant").ColumnName = "CD_ARTI"
            dtNomenclature_FILT.Columns("Qté").ColumnName = "QT_REPE"
            dtNomenclature_FILT.Columns("Désignation").ColumnName = "NM_DSGT"
            dtNomenclature_FILT.Columns.Remove("N° Nomenclature")
            dtNomenclature_FILT.Columns.Remove("N° Noeud")
            dtNomenclature_FILT.Columns.Remove("Compteur Poste")
            dtNomenclature_FILT.Columns.Remove("Fab. Interne/Externe")
            dtNomenclature_FILT.Columns.Remove("Compteur Sous-Poste")

            SQL_BULK_COPY_DT("Data Source=cedb03,1433;Initial Catalog=APP_WEB_ECO;Persist Security Info=True;User ID=sa;Password=mdpsa@SQL", "[APP_WEB_ECO].[dbo].[DWH_CPRS_BOM_PIVO_SAP]", dtNomenclature_FILT)

            Session("FICH_BOM") = savePath 'enregitrement du chemin du fichier sélectionné dans une variable de session

            PARA_CHAR_SAUV("Chargement") 'chargement des paramètres d'import

        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try

    End Sub

    Protected Sub Button_ONGL_BOM_CLIE_Click(sender As Object, e As EventArgs) Handles Button_ONGL_BOM_CLIE.Click

        Dim dtBOM_CLIE_2 As DataTable = EXCE_DATA_DT(Session("FICH_BOM"), DropDownList_ONGL_BOM_CLIE.SelectedValue)

        Session("dtBOM_CLIE") = dtBOM_CLIE_2
        GridView_FICH_BOM_CLIE.DataSource = Session("dtBOM_CLIE")
        GridView_FICH_BOM_CLIE.DataBind()
        MultiView_COMP_BOM.SetActiveView(View_SEL_DONN)

    End Sub

    Protected Sub Button_APER_PIVO_Click(sender As Object, e As EventArgs) Handles Button_APER_PIVO.Click
        Dim dtBOM_CLIE_PIVO As DataTable = Session("dtBOM_CLIE")

        Dim iLIGN_COUN As Integer = dtBOM_CLIE_PIVO.Rows.Count
        Dim iCOLO_COUNT As Integer = dtBOM_CLIE_PIVO.Columns.Count

        If TextBox_DERN_LIGN_DONN.Text <> "" Then
            For iLigne As Integer = TextBox_DERN_LIGN_DONN.Text - 1 To iLIGN_COUN - 1
                dtBOM_CLIE_PIVO.Rows.Remove(dtBOM_CLIE_PIVO.Rows(TextBox_DERN_LIGN_DONN.Text - 1))
            Next
        End If
        If TextBox_PREM_LIGN_DONN.Text <> "" Then
            For iLigne As Integer = 1 To TextBox_PREM_LIGN_DONN.Text
                dtBOM_CLIE_PIVO.Rows.Remove(dtBOM_CLIE_PIVO.Rows(0))
            Next
        End If

        If TextBox_COLO_CD_ARTI_CLIE.Text <> "" Then dtBOM_CLIE_PIVO.Columns(TextBox_COLO_CD_ARTI_CLIE.Text).ColumnName = "Code article"
        If TextBox_COLO_REPE.Text <> "" Then dtBOM_CLIE_PIVO.Columns(TextBox_COLO_REPE.Text).ColumnName = "Repère"
        If TextBox_COLO_QTE.Text <> "" Then dtBOM_CLIE_PIVO.Columns(TextBox_COLO_QTE.Text).ColumnName = "Quantité"

        For iCOLO As Integer = iCOLO_COUNT - 1 To 0 Step -1
            If Not (dtBOM_CLIE_PIVO.Columns(iCOLO).ColumnName = "Code article" Or dtBOM_CLIE_PIVO.Columns(iCOLO).ColumnName = "Repère" Or dtBOM_CLIE_PIVO.Columns(iCOLO).ColumnName = "Quantité") Then
                dtBOM_CLIE_PIVO.Columns.RemoveAt(iCOLO)
            End If
        Next

        Dim dtBOM_CLIE_PIVO_FILT As DataTable = dtBOM_CLIE_PIVO.Select("NOT [Code article] IS NULL AND NOT [Repère] IS NULL AND NOT [Quantité] IS NULL").CopyToDataTable
        TextBox_DERN_LIGN_DONN.Text = dtBOM_CLIE_PIVO_FILT.Rows.Count
        For iLigne As Integer = dtBOM_CLIE_PIVO_FILT.Rows.Count - 1 To 1 Step -1
            If dtBOM_CLIE_PIVO_FILT.Rows(iLigne)("Quantité") = "0" Then dtBOM_CLIE_PIVO_FILT.Rows.Remove(dtBOM_CLIE_PIVO_FILT.Rows(iLigne))
        Next
        Session.Add("dtBOM_CLIE_PIVO", dtBOM_CLIE_PIVO_FILT)
        GridView_PIVO_BOM_CLIE.DataSource = Session("dtBOM_CLIE_PIVO")
        GridView_PIVO_BOM_CLIE.DataBind()

        MultiView_COMP_BOM.SetActiveView(View_CONF_CAS_PART)
        ANA_CHAM_REPE()

    End Sub


    Protected Sub Button_GENE_PIVO_Click(sender As Object, e As EventArgs) Handles Button_GENE_PIVO.Click

        Dim dtBOM_CLIE_PIVO, dtBOM_CLIE_PIVO_FILT As New DataTable

        Try
            SQL_REQ_ACT("DELETE FROM [APP_WEB_ECO].[dbo].[DWH_CPRS_BOM_PIVO_CLIE]", "Data Source=cedb03, 1433;Initial Catalog=APP_WEB_ECO;Persist Security Info=True;User ID=sa;Password=mdpsa@SQL")

            dtBOM_CLIE_PIVO = Session("dtBOM_CLIE_PIVO")
            dtBOM_CLIE_PIVO_FILT = dtBOM_CLIE_PIVO.Select("NOT [Code article] IS NULL AND NOT [Repère] IS NULL AND NOT [Quantité] IS NULL").CopyToDataTable
            dtBOM_CLIE_PIVO_FILT.Columns("Repère").ColumnName = "NM_REPE"
            dtBOM_CLIE_PIVO_FILT.Columns("Code article").ColumnName = "CD_ARTI"
            dtBOM_CLIE_PIVO_FILT.Columns("Quantité").ColumnName = "QT_REPE"
            SQL_BULK_COPY_DT("Data Source=cedb03,1433;Initial Catalog=APP_WEB_ECO;Persist Security Info=True;User ID=sa;Password=mdpsa@SQL", "[APP_WEB_ECO].[dbo].[DWH_CPRS_BOM_PIVO_CLIE]", dtBOM_CLIE_PIVO_FILT)

            MultiView_COMP_BOM.SetActiveView(View_RESU_COMP)
            PARA_CHAR_SAUV("Sauvegarde") 'Enregistrer les paramètres si changés
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try

    End Sub

    Protected Sub CheckBox_COLO_FORM_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox_COLO_FORM.CheckedChanged
        If CheckBox_COLO_FORM.Checked = False Then
            MultiView_CONF_COLO.SetActiveView(View_COLO_UNIQ)
        Else
            MultiView_CONF_COLO.SetActiveView(View_CONF_MULT)
        End If

    End Sub

    Public Sub PARA_CHAR_SAUV(sACTI As String)

        If sACTI = "Sauvegarde" Then
            If DropDownList_ONGL_BOM_CLIE.Items.Count > 0 Then COMM_APP_WEB_PARA_AFFI_SAVE(DropDownList_Client.SelectedValue, "DropDownList_ONGL_BOM_CLIE", DropDownList_ONGL_BOM_CLIE.SelectedValue)
            If TextBox_CARA_SEPA.Text <> "" Then COMM_APP_WEB_PARA_AFFI_SAVE(DropDownList_Client.SelectedValue, "TextBox_CARA_SEPA", TextBox_CARA_SEPA.Text)
            COMM_APP_WEB_PARA_AFFI_SAVE(DropDownList_Client.SelectedValue, "TextBox_PREM_LIGN_DONN", TextBox_PREM_LIGN_DONN.Text)
            COMM_APP_WEB_PARA_AFFI_SAVE(DropDownList_Client.SelectedValue, "TextBox_COLO_CD_ARTI_CLIE", TextBox_COLO_CD_ARTI_CLIE.Text)
            COMM_APP_WEB_PARA_AFFI_SAVE(DropDownList_Client.SelectedValue, "TextBox_COLO_REPE", TextBox_COLO_REPE.Text)
            COMM_APP_WEB_PARA_AFFI_SAVE(DropDownList_Client.SelectedValue, "TextBox_COLO_QTE", TextBox_COLO_QTE.Text)
            If TextBox_REPE_MULT_COLO_REPE.Text <> "" Then COMM_APP_WEB_PARA_AFFI_SAVE(DropDownList_Client.SelectedValue, "TextBox_REPE_MULT_COLO_REPE", TextBox_REPE_MULT_COLO_REPE.Text)
            If TextBox_REPE_MULT_COLO_REPE.Text <> "" Then COMM_APP_WEB_PARA_AFFI_SAVE(DropDownList_Client.SelectedValue, "RadioButtonList_QTE_PLUS_LIGN", RadioButtonList_QTE_PLUS_LIGN.SelectedValue)
        Else
            COMM_APP_WEB_PARA_AFFI_LOAD(DropDownList_Client.SelectedValue, "DropDownList_ONGL_BOM_CLIE", View_ONGL)
            COMM_APP_WEB_PARA_AFFI_LOAD(DropDownList_Client.SelectedValue, "TextBox_CARA_SEPA", View_COLO_UNIQ)
            COMM_APP_WEB_PARA_AFFI_LOAD(DropDownList_Client.SelectedValue, "TextBox_PREM_LIGN_DONN", View_SEL_DONN)
            COMM_APP_WEB_PARA_AFFI_LOAD(DropDownList_Client.SelectedValue, "TextBox_COLO_CD_ARTI_CLIE", View_CONF_MULT)
            COMM_APP_WEB_PARA_AFFI_LOAD(DropDownList_Client.SelectedValue, "TextBox_COLO_REPE", View_CONF_MULT)
            COMM_APP_WEB_PARA_AFFI_LOAD(DropDownList_Client.SelectedValue, "TextBox_COLO_QTE", View_CONF_MULT)
            COMM_APP_WEB_PARA_AFFI_LOAD(DropDownList_Client.SelectedValue, "TextBox_REPE_MULT_COLO_REPE", View_REPE_MULT_COLO_REPE)
            COMM_APP_WEB_PARA_AFFI_LOAD(DropDownList_Client.SelectedValue, "RadioButtonList_QTE_PLUS_LIGN", View_REPE_MULT_COLO_REPE)
        End If

    End Sub
    Protected Sub Button_REPE_MULT_COLO_REPE_Click(sender As Object, e As EventArgs) Handles Button_REPE_MULT_COLO_REPE.Click
        Dim dtBOM_CLIE_PIVO As DataTable = Session("dtBOM_CLIE_PIVO")
        Dim aLIGN As String()
        Dim iLIGN_COUN As Integer = dtBOM_CLIE_PIVO.Rows.Count
        Dim iLIGN_SPLI As Integer = 0
        Dim iQté As Integer
        'regroupement des repères sur plusieurs lignes pour un même code article
        Dim dtBOM_CLIE_PIVO_GROU As New DataTable
        dtBOM_CLIE_PIVO_GROU.Columns.Add("Code article", Type.GetType("System.String"))
        dtBOM_CLIE_PIVO_GROU.Columns.Add("Repère", Type.GetType("System.String"))
        dtBOM_CLIE_PIVO_GROU.Columns.Add("Quantité", Type.GetType("System.String"))
        Dim rBOM_CLIE_PIVO_GROUP As DataRow
        For Each rBOM_CLIE_PIVO As DataRow In dtBOM_CLIE_PIVO.Rows
            rBOM_CLIE_PIVO_GROUP = dtBOM_CLIE_PIVO_GROU.Select("[Code article] = '" & rBOM_CLIE_PIVO("Code article").ToString & "' AND Quantité = '" & rBOM_CLIE_PIVO("Quantité") & "'").FirstOrDefault()
            If rBOM_CLIE_PIVO_GROUP Is Nothing Then
                dtBOM_CLIE_PIVO_GROU.Rows.Add()
                dtBOM_CLIE_PIVO_GROU.Rows(dtBOM_CLIE_PIVO_GROU.Rows.Count - 1)("Code article") = rBOM_CLIE_PIVO("Code article").ToString
                dtBOM_CLIE_PIVO_GROU.Rows(dtBOM_CLIE_PIVO_GROU.Rows.Count - 1)("Repère") = Replace(rBOM_CLIE_PIVO("Repère").ToString.Trim(), ",", TextBox_REPE_MULT_COLO_REPE.Text)
                dtBOM_CLIE_PIVO_GROU.Rows(dtBOM_CLIE_PIVO_GROU.Rows.Count - 1)("Quantité") = rBOM_CLIE_PIVO("Quantité").ToString
            Else
                rBOM_CLIE_PIVO_GROUP("Repère") &= TextBox_REPE_MULT_COLO_REPE.Text & Replace(rBOM_CLIE_PIVO("Repère").ToString.Trim(), ",", TextBox_REPE_MULT_COLO_REPE.Text)
                'If RadioButtonList_QTE_PLUS_LIGN.SelectedValue = "Une quantité par ligne" Then
                'rBOM_CLIE_PIVO_GROUP("Quantité") += rBOM_CLIE_PIVO("Quantité")
                'End If
            End If
        Next

        For iligne As Integer = 0 To iLIGN_COUN - 1
            If IsDBNull(dtBOM_CLIE_PIVO_GROU.Rows(iLIGN_SPLI)("Repère")) Then
                iLIGN_SPLI += 1
                Continue For
            End If
            aLIGN = Split(dtBOM_CLIE_PIVO_GROU.Rows(iLIGN_SPLI)("Repère"), TextBox_REPE_MULT_COLO_REPE.Text)
            If UBound(aLIGN) = 0 Then
                iQté = dtBOM_CLIE_PIVO_GROU.Rows(iLIGN_SPLI)("Quantité")
            Else
                iQté = dtBOM_CLIE_PIVO_GROU.Rows(iLIGN_SPLI)("Quantité") / (UBound(aLIGN) + 1)
            End If
            LOG_Msg(GetCurrentMethod, "Repère = " & dtBOM_CLIE_PIVO_GROU.Rows(iLIGN_SPLI)("Repère") & "|quantité = " & dtBOM_CLIE_PIVO_GROU.Rows(iLIGN_SPLI)("Quantité") & "|align = " & (UBound(aLIGN) + 1) & "|iqté = " & dtBOM_CLIE_PIVO_GROU.Rows(iLIGN_SPLI)("Quantité") / (UBound(aLIGN) + 1))
            For Each erepe In aLIGN
                If erepe <> "" Then
                    dtBOM_CLIE_PIVO_GROU.Rows.Add()
                    dtBOM_CLIE_PIVO_GROU.Rows(dtBOM_CLIE_PIVO_GROU.Rows.Count - 1)("Code article") = dtBOM_CLIE_PIVO_GROU.Rows(iLIGN_SPLI)("Code article")
                    dtBOM_CLIE_PIVO_GROU.Rows(dtBOM_CLIE_PIVO_GROU.Rows.Count - 1)("Repère") = erepe
                    dtBOM_CLIE_PIVO_GROU.Rows(dtBOM_CLIE_PIVO_GROU.Rows.Count - 1)("Quantité") = Convert.ToString(iQté)
                End If
            Next
            dtBOM_CLIE_PIVO_GROU.Rows.Remove(dtBOM_CLIE_PIVO_GROU.Rows(iLIGN_SPLI))
        Next
        Session("dtBOM_CLIE_PIVO") = dtBOM_CLIE_PIVO_GROU
        GridView_PIVO_BOM_CLIE.DataSource = Session("dtBOM_CLIE_PIVO")
        GridView_PIVO_BOM_CLIE.DataBind()
        ANA_CHAM_REPE()

    End Sub

    Protected Sub Button_CARA_SEPA_Click(sender As Object, e As EventArgs) Handles Button_CARA_SEPA.Click
        MultiView_CONF_COLO.SetActiveView(View_CONF_MULT)
    End Sub

    Protected Sub RadioButtonList_LIST_OPTI_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RadioButtonList_LIST_OPTI.SelectedIndexChanged
        Select Case RadioButtonList_LIST_OPTI.SelectedValue
            Case "REPE_MULT_COLO_REPE"
                MultiView_CONF_CAS_PART.SetActiveView(View_REPE_MULT_COLO_REPE)
            Case "RESU_ANA_REPE_CARA_SPEC"
                MultiView_CONF_CAS_PART.SetActiveView(View_RESU_ANA_REPE_CARA_SPEC)
        End Select
    End Sub

    Protected Sub Button_RAZ_PIVO_Click(sender As Object, e As EventArgs) Handles Button_RAZ_PIVO.Click
        Dim dt As DataTable = Session("dtBOM_CLIE")
        Session("dtBOM_CLIE_PIVO") = dt
        GridView_PIVO_BOM_CLIE.DataSource = Session("dtBOM_CLIE_PIVO")
        GridView_PIVO_BOM_CLIE.DataBind()
        ANA_CHAM_REPE()
    End Sub

    Protected Sub GridView_FICH_BOM_CLIE_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GridView_FICH_BOM_CLIE.SelectedIndexChanged
        TextBox_DERN_LIGN_DONN.Text = GridView_FICH_BOM_CLIE.SelectedIndex
    End Sub

    Protected Sub DropDownList_Article_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList_Article.SelectedIndexChanged
        Dim dtMAKT As DataTable = SAP_DATA_READ_MAKT("MATNR EQ '" & DropDownList_Article.SelectedValue & "'")
        Label_SEL_ARTI.Text = dtMAKT(0)("MAKTX").ToString
    End Sub

    Public Sub AFF_LOAD(sAffiche As String)
        If sAffiche = "affichage" Then
            Image_EOL_LOAD_1.Visible = True
            Image_EOL_LOAD_2.Visible = True
            Image_EOL_LOAD_3.Visible = True
            Label_LOAD.Visible = True
        Else
            Image_EOL_LOAD_1.Visible = False
            Image_EOL_LOAD_2.Visible = False
            Image_EOL_LOAD_3.Visible = False
            Label_LOAD.Visible = False
        End If


    End Sub

    Public Sub ANA_CHAM_REPE()
        Dim dtBOM_CLIE_PIVO As DataTable = Session("dtBOM_CLIE_PIVO")
        Dim nu_char As Integer = 0

        For Each rBOM_CLIE_PIVO As DataRow In dtBOM_CLIE_PIVO.Rows
            For i As Integer = 1 To Len(rBOM_CLIE_PIVO("Repère").ToString)
                If Char.IsLetterOrDigit(Mid(rBOM_CLIE_PIVO("Repère").ToString, i, 1)) = False Then
                    Session("code_article_a_modifier") = rBOM_CLIE_PIVO("Code article").ToString
                    Session("repere_a_modifier") = Trim(rBOM_CLIE_PIVO("Repère").ToString)
                    Session("caractère_a_modifier") = Mid(rBOM_CLIE_PIVO("Repère").ToString, i, 1)
                    Label_RESU_ANA_REPE_CARA_SPEC.Text = "Un caractère spécial a été détecté dans la ligne '" & Session("repere_a_modifier") & "' de la colonne repère : '" & Session("caractère_a_modifier") & "'. Veuillez entrer les repères séparés par des espaces."
                    RadioButtonList_LIST_OPTI.SelectedValue = "RESU_ANA_REPE_CARA_SPEC"
                    MultiView_CONF_CAS_PART.SetActiveView(View_RESU_ANA_REPE_CARA_SPEC)
                    RadioButtonList_GENE_RPR_RPC_CARA.Items.FindByValue("Générer").Text = "Générer les repères " & GENE_RPR_RPC_CARA(Session("repere_a_modifier"), Session("caractère_a_modifier"))
                    RadioButtonList_GENE_RPR_RPC_CARA.Items.FindByValue("Séparer").Text = "Séparer les repères " & SEPA_RPR_RPC_CARA(Session("repere_a_modifier"), Session("caractère_a_modifier"))
                    Exit Sub
                End If
            Next
        Next
    End Sub

    Protected Sub Button_RESU_ANA_REPE_CARA_SPEC_Click(sender As Object, e As EventArgs) Handles Button_RESU_ANA_REPE_CARA_SPEC.Click
        Dim dtBOM_CLIE_PIVO As DataTable = Session("dtBOM_CLIE_PIVO")

        If TextBox_RESU_ANA_REPE_CARA_SPEC.Text = "" Then
            Dim dt As New DataTable
            dt.Columns.Add("Code article", Type.GetType("System.String"))
            dt.Columns.Add("Repère", Type.GetType("System.String"))
            dt.Columns.Add("Quantité", Type.GetType("System.String"))
            Dim aLIGN As String() = {"", ""}
            For Each rBOM_CLIE_PIVO As DataRow In dtBOM_CLIE_PIVO.Rows
                If rBOM_CLIE_PIVO("Repère").ToString.IndexOf(Session("caractère_a_modifier")) <= 0 Then
                    dt.Rows.Add()
                    dt.Rows(dt.Rows.Count - 1)("Code article") = rBOM_CLIE_PIVO("Code article")
                    dt.Rows(dt.Rows.Count - 1)("Repère") = rBOM_CLIE_PIVO("Repère")
                    dt.Rows(dt.Rows.Count - 1)("Quantité") = rBOM_CLIE_PIVO("Quantité")
                Else
                    Select Case RadioButtonList_GENE_RPR_RPC_CARA.SelectedValue
                        Case "Générer"
                            aLIGN = Split(GENE_RPR_RPC_CARA(rBOM_CLIE_PIVO("Repère"), Session("caractère_a_modifier")), " ")
                        Case "Séparer"
                            aLIGN = Split(SEPA_RPR_RPC_CARA(rBOM_CLIE_PIVO("Repère"), Session("caractère_a_modifier")), " ")
                    End Select
                    For Each erepe In aLIGN
                        If erepe <> "" Then
                            dt.Rows.Add()
                            dt.Rows(dt.Rows.Count - 1)("Code article") = rBOM_CLIE_PIVO("Code article")
                            dt.Rows(dt.Rows.Count - 1)("Repère") = erepe
                            dt.Rows(dt.Rows.Count - 1)("Quantité") = 1
                        End If
                    Next
                End If
            Next
            Session("dtBOM_CLIE_PIVO") = dt
        Else
            Dim rBOM_CLIE_PIVO As DataRow = dtBOM_CLIE_PIVO.Select("[Code article] = '" & Session("code_article_a_modifier") & "' AND [Repère] = '" & Session("repere_a_modifier") & "'").FirstOrDefault
            Select Case RadioButtonList_GENE_RPR_RPC_CARA.SelectedValue
                Case "Générer"
                    TextBox_RESU_ANA_REPE_CARA_SPEC.Text = GENE_RPR_RPC_CARA(Session("repere_a_modifier"), Session("caractère_a_modifier"))
                Case "Séparer"
                    TextBox_RESU_ANA_REPE_CARA_SPEC.Text = SEPA_RPR_RPC_CARA(Session("repere_a_modifier"), Session("caractère_a_modifier"))
            End Select
            Dim aLIGN As String() = Split(TextBox_RESU_ANA_REPE_CARA_SPEC.Text, " ")
            For Each erepe In aLIGN
                If erepe <> "" Then
                    dtBOM_CLIE_PIVO.Rows.Add()
                    dtBOM_CLIE_PIVO.Rows(dtBOM_CLIE_PIVO.Rows.Count - 1)("Code article") = rBOM_CLIE_PIVO("Code article")
                    dtBOM_CLIE_PIVO.Rows(dtBOM_CLIE_PIVO.Rows.Count - 1)("Repère") = erepe
                    dtBOM_CLIE_PIVO.Rows(dtBOM_CLIE_PIVO.Rows.Count - 1)("Quantité") = 1
                End If
            Next
            dtBOM_CLIE_PIVO.Rows.Remove(rBOM_CLIE_PIVO)
            Session("dtBOM_CLIE_PIVO") = dtBOM_CLIE_PIVO
        End If


        GridView_PIVO_BOM_CLIE.DataSource = Session("dtBOM_CLIE_PIVO")
        GridView_PIVO_BOM_CLIE.DataBind()
        LOG_Msg(GetCurrentMethod, TextBox_RESU_ANA_REPE_CARA_SPEC.Text)
        TextBox_RESU_ANA_REPE_CARA_SPEC.Text = ""
        ANA_CHAM_REPE()
    End Sub

    Public Function GENE_RPR_RPC_CARA(sRepère As String, sSéparateur As String) As String

        Dim caractère As String = ""
        Dim nu_char As Integer = 0, nu_depart As String = "", nu_arrive As String = ""

        Try
            For i As Integer = 1 To Len(sRepère)
                If Not Char.IsLetter(Mid(sRepère, i, 1)) Then Exit For
                caractère = Mid(sRepère, 1, i)
                nu_char += 1
            Next
            sRepère = Right(sRepère, Len(sRepère) - nu_char)
            nu_char = 0

            For i As Integer = 1 To Len(sRepère)
                If Not Char.IsNumber(Mid(sRepère, i, 1)) Then Exit For
                nu_depart = Mid(sRepère, 1, i)
                nu_char += 1
            Next
            sRepère = Replace(Right(sRepère, Len(sRepère) - nu_char), sSéparateur, "")
            nu_char = 0

            For i As Integer = 1 To Len(sRepère)
                If Not Char.IsNumber(Mid(sRepère, i, 1)) Then Exit For
                nu_arrive = Mid(sRepère, 1, i)
                nu_char += 1
            Next

            sRepère = ""
            For i As Integer = Convert.ToDecimal(nu_depart) To Convert.ToDecimal(nu_arrive)
                sRepère &= " " & caractère & i.ToString
            Next
            sRepère = Right(sRepère, Len(sRepère) - 1)
        Catch When nu_depart = ""
            sRepère = caractère
            LOG_Erreur(GetCurrentMethod, "pas de départ " & sRepère)
        Catch When nu_arrive = ""
            sRepère = caractère
            LOG_Erreur(GetCurrentMethod, "pas d'arrivée " & sRepère)
        Catch ex As Exception
            sRepère = caractère
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try

        LOG_Msg(GetCurrentMethod, sRepère)
        Return sRepère
    End Function

    Public Function SEPA_RPR_RPC_CARA(sRepère As String, sSéparateur As String) As String
        Dim caractère As String = ""

        For i As Integer = 1 To Len(sRepère)
            If Not Char.IsLetter(Mid(sRepère, i, 1)) Then Exit For
            caractère = Mid(sRepère, 1, i)
        Next
        LOG_Msg(GetCurrentMethod, Replace(sRepère, sSéparateur, " " & caractère))
        Return Replace(sRepère, sSéparateur, " " & caractère)

    End Function


End Class