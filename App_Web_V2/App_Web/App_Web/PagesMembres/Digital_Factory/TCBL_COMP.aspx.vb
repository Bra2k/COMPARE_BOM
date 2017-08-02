Imports App_Web.Class_SAP_DATA
Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports App_Web.Class_DIG_FACT
Imports App_Web.Class_DIG_FACT_SQL
Imports System.Data.SqlClient
Imports App_Web.Class_SQL
Imports App_Web.Class_COMM_APP_WEB
Imports System.IO

Public Class TCBL_COMP
    Inherits System.Web.UI.Page
    Dim sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=" & Replace(Replace(My.Computer.Name, "CEDB03", "MES_Digital_Factory_DEV"), "CEAPP03", "MES_Digital_Factory") & ";Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=7200;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
    Public Sub Button_reload_impress_Click(sender As Object, e As EventArgs) Handles Button_reload_impress.Click
        Dim last_ser_num As String = "SELECT [NU_SER_DERN] FROM [dbo].[V_DER_DTM_NU_SER] WHERE [NM_CRIT] = '" & TextBox_OF.Text & "'"
        Dim dt As DataTable = SQL_SELE_TO_DT(last_ser_num, sChaineConnexion)
        If dt Is Nothing Then
            last_ser_num = "0"
        Else
            last_ser_num = dt(0)("NU_SER_DERN").ToString
        End If
        Try
            DIG_FACT_IMPR_ETIQ("\\ceapp03\Sources\Digital Factory\Etiquettes\AVALUN\AVALUN.prn", TextBox_OF.Text, "", "Numéro de série Eolane", "", TextBox_OF.Text & (Convert.ToDecimal(last_ser_num) + 1).ToString, last_ser_num, "", "", Nothing)
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
        End Try
    End Sub

    Public Sub Button_VALI_ENTER_Click(sender As Object, e As EventArgs) Handles Button_VALI_ENTER.Click

        Dim dtAFKO, dtVRESB, dtSTPU, dtSTPO, dt_SS_ENS, dt_CD_ARTI_ENS_SENS_SAP, dt_NS_TRAC As New DataTable
        Dim sFiltre As String = "MTART = 'FERT'", sQuery = "" 'HALB
        Dim rdt_CD_ARTI_ENS_SENS_SAP As DataRow
        Dim compteur As Integer

        Try
            dtAFKO = SAP_DATA_READ_AFKO("AUFNR LIKE '%" & TextBox_OF.Text & "'")
            If dtAFKO Is Nothing Then Throw New Exception("L'OF n°" & TextBox_OF.Text & " n'a pas été trouvé dans SAP.")
            'récupération des composants à tracer sur l'opération (config SAP plus base DF)
            dt_SS_ENS = App_Web.TCBL_ESB_SS_ESB_V2._LIST_ENS_SS_ENS(TextBox_OF.Text, DropDownList_OP.SelectedValue)
            'affichage
            dt_SS_ENS.Columns.Remove("Numéro de série associé")
            dt_SS_ENS.Columns.Add("Repère", Type.GetType("System.String"))
            dt_SS_ENS.Columns.Add("Quantité par produit", Type.GetType("System.String"))
            dt_SS_ENS.Columns.Add("N° de conteneur", Type.GetType("System.String"))
            dt_SS_ENS.Columns.Add("Id composant", Type.GetType("System.String"))
            dt_SS_ENS.Columns.Add("Code lot", Type.GetType("System.String"))
            dt_SS_ENS.Columns.Add("Quantité restante", Type.GetType("System.String"))
            dtVRESB = SAP_DATA_READ_VRESB("RSNUM EQ '" & dtAFKO(0)("RSNUM").ToString & "' AND SPRAS EQ 'F' AND VORNR EQ '" & DropDownList_OP.SelectedValue & "'")
            For Each rVRESB As DataRow In dtVRESB.Rows
                rdt_CD_ARTI_ENS_SENS_SAP = dt_SS_ENS.Select("[Code article SAP] = '" & rVRESB("MATNR").ToString & "'").FirstOrDefault
                If rdt_CD_ARTI_ENS_SENS_SAP Is Nothing Then Continue For

                'Vérification saisie numéro de série (pour sous-ensemble non-déclaré en produit dans et SAP et nécessité de saisir le n° de série)
                If DIG_FACT_SQL_GET_PARA(Trim(rVRESB("MATNR").ToString), "Sérialisation article") = "1" Then rdt_CD_ARTI_ENS_SENS_SAP("Repère") = "PRODUIT"

                dtSTPO = SAP_DATA_READ_STPO("STLNR EQ '" & rVRESB("STLNR").ToString & "' and STLKN EQ '" & rVRESB("STLKN").ToString & "' and STPOZ EQ '" & rVRESB("STPOZ").ToString & "'")
                rdt_CD_ARTI_ENS_SENS_SAP("Quantité par produit") = Convert.ToDecimal(Replace(dtSTPO(0)("MENGE").ToString, ".", ","))
                Select Case rVRESB("MTART").ToString
                    Case "FERT"
                        rdt_CD_ARTI_ENS_SENS_SAP("Repère") = "PRODUIT"
                    Case "HALB"
                        rdt_CD_ARTI_ENS_SENS_SAP("Repère") = "PRODUIT SEMI-FINI"
                    Case Else
                        dtSTPU = SAP_DATA_READ_STPU("STLNR EQ '" & rVRESB("STLNR").ToString & "' and STLKN EQ '" & rVRESB("STLKN").ToString & "' and STPOZ EQ '" & rVRESB("STPOZ").ToString & "'")
                        If dtSTPU Is Nothing Then Continue For
                        For Each rdtSTPU As DataRow In dtSTPU.Rows
                            rdt_CD_ARTI_ENS_SENS_SAP("Repère") &= rdtSTPU("EBORT").ToString & "|"
                        Next
                        rdt_CD_ARTI_ENS_SENS_SAP("Repère") = COMM_APP_WEB_STRI_TRIM_RIGHT(rdt_CD_ARTI_ENS_SENS_SAP("Repère"), 1)
                End Select
                If rdt_CD_ARTI_ENS_SENS_SAP("Repère") = "PRODUIT" Then
                    compteur = compteur + 1
                End If
            Next
            GridView_REPE.DataSource = dt_SS_ENS
            GridView_REPE.DataBind()
            Dim count_rows = GridView_REPE.Rows.Count
            Label_OF.Text = TextBox_OF.Text

            Dim dtMARA As DataTable = SAP_DATA_READ_MARA("MATNR EQ '" & dtAFKO(0)("PLNBEZ").ToString & "'")
            Dim dtT179T As DataTable = SAP_DATA_READ_T179T("PRODH EQ '" & dtMARA(0)("PRDHA").ToString & "'")

            Label_CLIE.Text = dtT179T(0)("VTEXT").ToString
            Label_CD_ARTI.Text = dtAFKO(0)("PLNBEZ").ToString
            Dim dtMAKT As DataTable = SAP_DATA_READ_MAKT("MATNR EQ '" & dtAFKO(0)("PLNBEZ").ToString & "'")
            Label_DES_ARTI.Text = dtMAKT(0)("MAKTX").ToString
            Label_OP.Text = Convert.ToDecimal(DropDownList_OP.SelectedValue).ToString
            Dim dtAFVC As DataTable = SAP_DATA_READ_AFVC("AUFPL EQ '" & dtAFKO(0)("AUFPL").ToString & "' AND VORNR EQ '" & DropDownList_OP.SelectedValue & "'")
            Label_DES_OP.Text = Trim(dtAFVC(0)("LTXA1").ToString)
            Label_QT_OF.Text = dtAFKO(0)("GAMNG").ToString

            sQuery = "SELECT ISNULL(CASE [NM_NS_CLT] WHEN '' THEN [NM_NS_EOL] ELSE [NM_NS_CLT] END,[NM_NS_EOL]) AS [Numéros de série tracés]
                        FROM [dbo].[ID_PF_View]
                       WHERE [LB_ETP] = '" & Label_DES_OP.Text & " (OP:" & Label_OP.Text & ")' AND [NM_OF] = '" & Label_OF.Text & "'
                      GROUP BY ISNULL(CASE [NM_NS_CLT] WHEN '' THEN [NM_NS_EOL] ELSE [NM_NS_CLT] END,[NM_NS_EOL]), [LB_ETP], [NM_OF]
                      ORDER BY ISNULL(CASE [NM_NS_CLT] WHEN '' THEN [NM_NS_EOL] ELSE [NM_NS_CLT] END,[NM_NS_EOL])"
            dt_NS_TRAC = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            If Not dt_NS_TRAC Is Nothing Then
                Session("DT_NS_TRAC") = dt_NS_TRAC
                GridView_SN_TRAC.DataSource = Session("DT_NS_TRAC")
                GridView_SN_TRAC.DataBind()
                If dt_NS_TRAC.Rows.Count = Convert.ToDecimal(Replace(Label_QT_OF.Text, ".", ",")) Then Throw New Exception("L'OF est entièrement tracé pour cette opération")
            End If
            COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI.Text & "(" & Label_OP.Text & ")", "CheckBox_GENE_ETI_ENS", View_DATA_ENTR)

            'Si la TextBox de génération des étiquettes est cochée
            If CheckBox_GENE_ETI_ENS.Checked = True Then
                Dim last_ser_num As String = "SELECT [NU_SER_DERN] FROM [dbo].[V_DER_DTM_NU_SER] WHERE [NM_CRIT] = '" & TextBox_OF.Text & "'"
                Dim dt As DataTable = SQL_SELE_TO_DT(last_ser_num, sChaineConnexion)
                If dt Is Nothing Then
                    last_ser_num = "0"
                Else
                    last_ser_num = dt(0)("NU_SER_DERN").ToString
                End If
                DIG_FACT_IMPR_ETIQ("\\ceapp03\Sources\Digital Factory\Etiquettes\AVALUN\AVALUN.prn", TextBox_OF.Text, "", "Numéro de série Eolane", "", TextBox_OF.Text & (Convert.ToDecimal(last_ser_num) + 1).ToString, last_ser_num, "", "", Nothing)
            End If

            COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI.Text & "(" & Label_OP.Text & ")", "CheckBox_SAI_AUTO", View_SAIS_ENS)
            COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI.Text & "(" & Label_OP.Text & ")", "CheckBox_NU_SER_EOL", View_SAIS_ENS)
            TextBox_ENS.Focus()
            Label_RES.Text = ""
            If count_rows = compteur Then
                MultiView_Tracabilité.SetActiveView(View_SAIS_ENS)
            Else
                MultiView_Tracabilité.SetActiveView(View_CONT_LOT_ID_COMP)
            End If
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
        End Try
    End Sub

    Protected Sub TextBox_ENS_TextChanged(sender As Object, e As EventArgs) Handles TextBox_ENS.TextChanged

        Dim sQuery As String = "", sNU_SER_ECO As String = "", sNU_SER_CLIE As String = ""
        Dim dt_PARA, dt, dt_CFGR_ARTI_ECO As New DataTable
        Try
            dt_CFGR_ARTI_ECO = DIG_FACT_SQL_CFGR_ARTI_ECO(Trim(Label_CD_ARTI.Text))
            Select Case "1"
                Case dt_CFGR_ARTI_ECO(0)("Numéro de série Eolane").ToString
                    sNU_SER_ECO = TextBox_ENS.Text
                    'vérifier par rapport à un format
                    If DIG_FACT_VERI_FORM_NU_SER(Trim(Label_CD_ARTI.Text), "Format Numéro de série Eolane", TextBox_ENS.Text) = False Then Throw New Exception("Le numéro de série " & TextBox_ENS.Text & " ne correspond au format défini dans la base.")
                Case dt_CFGR_ARTI_ECO(0)("Numéro de série client").ToString
                    sNU_SER_CLIE = TextBox_ENS.Text
                    'vérifier par rapport à un format
                    If DIG_FACT_VERI_FORM_NU_SER(Trim(Label_CD_ARTI.Text), "Format Numéro de série client", TextBox_ENS.Text) = False Then Throw New Exception("Le numéro de série " & TextBox_ENS.Text & " ne correspond au format défini dans la base.")
            End Select

            'vérification que le ns n'est pas déjà associé 
            sQuery = "SELECT [NM_NS_EOL]
                        FROM [dbo].[ID_PF_View]
                       WHERE (NM_NS_EOL LIKE '%" & sNU_SER_ECO & "%' AND [NM_NS_CLT] LIKE '%" & sNU_SER_CLIE & "%') AND ([LB_ETP] = '" & Label_DES_OP.Text & " (OP:" & Label_OP.Text & ")')"
            dt_PARA = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            If Not dt_PARA Is Nothing Then Throw New Exception("Le numéro de série " & TextBox_ENS.Text & " est déjà attribué dans la base.")

            'Passer à la saisie des numéros de série des sous-ensemble
            For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                If rGridView_REPE.Cells(3).Text = "PRODUIT" Or rGridView_REPE.Cells(3).Text = "PRODUIT SEMI-FINI" Then
                    Label_CD_SS_ENS.Text = rGridView_REPE.Cells(1).Text
                    MultiView_BASC_SAI_SEL.SetActiveView(View_SAI)
                    TextBox_SS_ENS.Text = ""
                    TextBox_SS_ENS.Focus()
                    Exit Sub
                End If
            Next

            'Enregistrement
            _ERGT_TCBL(sNU_SER_ECO, sNU_SER_CLIE)
            _RAZ_AFCG()
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            TextBox_ENS.Text = ""
            TextBox_ENS.Focus()
        End Try
    End Sub

    Protected Sub GridView_REPE_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GridView_REPE.SelectedIndexChanged
        Label_CD_COMP.Text = GridView_REPE.SelectedRow.Cells(1).Text
        MultiView_SAIS_CD_LOT.SetActiveView(View_SAIS_CD_LOT_SAIS)
        TextBox_CD_LOT_COMP.Focus()
    End Sub

    Protected Sub TextBox_SS_ENS_TextChanged(sender As Object, e As EventArgs) Handles TextBox_SS_ENS.TextChanged

        Dim sQuerySql As String = "", sNU_SER_ECO As String = "", sNU_SER_CLIE As String = ""
        Dim dt_TR_CPT, dtAFKO, dt_NS_TRAC, dtS034 As New DataTable
        Dim sParam_Format_NS As String = "Format Numéro de série Eolane"
        Dim iID_Passage As Integer = 0
        Dim dt_PARA, dt, dt_CFGR_ARTI_ECO As New DataTable
        Try
            'vérifier que le ns est disponible
            sQuerySql = "SELECT   NM_NS_SENS
                           FROM   dbo.DTM_TR_CPT
                          WHERE   NM_NS_SENS = '" & TextBox_SS_ENS.Text & "'"
            dt_TR_CPT = SQL_SELE_TO_DT(sQuerySql, sChaineConnexion)
            If Not dt_TR_CPT Is Nothing Then Throw New Exception("Le numéro de série " & TextBox_SS_ENS.Text & " est déjà attribué dans la base.")

            'Extraction code article d'un numéro de série Eolane de SAP
            dtAFKO = SAP_DATA_READ_AFKO("AUFNR LIKE '%" & Left(TextBox_SS_ENS.Text, 7) & "'")
            If Not dtAFKO Is Nothing Then
                For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                    'attribution du code article si dans la liste
                    If Trim(rGridView_REPE.Cells(1).Text) = Trim(dtAFKO(0)("PLNBEZ").ToString) Then Label_CD_SS_ENS.Text = dtAFKO(0)("PLNBEZ").ToString
                Next
            End If

            'Vérifier le format du numéro de série
            dt_CFGR_ARTI_ECO = DIG_FACT_SQL_CFGR_ARTI_ECO(Trim(Label_CD_SS_ENS.Text))
            Select Case "1"
                Case dt_CFGR_ARTI_ECO(0)("Numéro de série Eolane").ToString
                    If DIG_FACT_VERI_FORM_NU_SER(Trim(Label_CD_SS_ENS.Text), "Format Numéro de série Eolane", TextBox_SS_ENS.Text) = False Then Throw New Exception("Le numéro de série " & TextBox_SS_ENS.Text & " ne correspond au format défini dans la base.")
                Case dt_CFGR_ARTI_ECO(0)("Numéro de série client").ToString
                    If DIG_FACT_VERI_FORM_NU_SER(Trim(Label_CD_SS_ENS.Text), "Format Numéro de série client", TextBox_SS_ENS.Text) = False Then Throw New Exception("Le numéro de série " & TextBox_SS_ENS.Text & " ne correspond au format défini dans la base.")
            End Select

            'Sauvegarder la donnée dans la colonne id composant
            For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                If Label_CD_SS_ENS.Text = rGridView_REPE.Cells(1).Text Then rGridView_REPE.Cells(6).Text = TextBox_SS_ENS.Text
            Next

            'saisir le n° de série du prochain sous-ensemble
            For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                If rGridView_REPE.Cells(6).Text = "&nbsp;" And (rGridView_REPE.Cells(3).Text = "PRODUIT" Or rGridView_REPE.Cells(3).Text = "PRODUIT SEMI-FINI") Then
                    MultiView_SAIS_CD_LOT.SetActiveView(View_SAIS_CD_LOT_SAIS)
                    GridView_REPE.SelectRow(rGridView_REPE.RowIndex)
                    Label_CD_SS_ENS.Text = GridView_REPE.SelectedRow.Cells(1).Text
                    TextBox_SS_ENS.Text = ""
                    TextBox_SS_ENS.Focus()
                    Exit Sub
                End If
            Next

            dt_CFGR_ARTI_ECO = DIG_FACT_SQL_CFGR_ARTI_ECO(Trim(Label_CD_ARTI.Text))
            Select Case "1"
                Case dt_CFGR_ARTI_ECO(0)("Numéro de série Eolane").ToString
                    sNU_SER_ECO = TextBox_ENS.Text
                Case dt_CFGR_ARTI_ECO(0)("Numéro de série client").ToString
                    sNU_SER_CLIE = TextBox_ENS.Text
            End Select
            'si terminé enregistrer les données
            _ERGT_TCBL(sNU_SER_ECO, sNU_SER_CLIE)
            _RAZ_AFCG()
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            MultiView_BASC_SAI_SEL.SetActiveView(View_SAI)
            TextBox_SS_ENS.Text = ""
            TextBox_SS_ENS.Focus()
            'Exit Sub
        End Try
    End Sub

    Protected Sub CheckBox_GENE_ETI_ENS_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox_GENE_ETI_ENS.CheckedChanged
        If Label_CD_ARTI.Text <> "" And Label_OP.Text <> "" Then COMM_APP_WEB_PARA_AFFI_SAVE(Label_CD_ARTI.Text & "(" & Label_OP.Text & ")", "CheckBox_GENE_ETI_ENS", CheckBox_GENE_ETI_ENS.Checked)
    End Sub

    Protected Sub CheckBox_SS_ENS_FERT_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox_SS_ENS_FERT.CheckedChanged
        If Label_CD_ARTI.Text <> "" And Label_OP.Text <> "" Then COMM_APP_WEB_PARA_AFFI_SAVE(Label_CD_ARTI.Text & "(" & Label_OP.Text & ")", "CheckBox_SS_ENS_FERT", CheckBox_SS_ENS_FERT.Checked)
    End Sub

    Protected Sub GridView_SN_TRAC_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GridView_SN_TRAC.PageIndexChanging

        GridView_SN_TRAC.PageIndex = e.NewPageIndex
        GridView_SN_TRAC.DataSource = Session("DT_NS_TRAC")
        GridView_SN_TRAC.DataBind()
    End Sub

    Protected Sub TextBox_OF_TextChanged(sender As Object, e As EventArgs) Handles TextBox_OF.TextChanged
        Dim dtAFKO, dtVRESB, dt_OP As New DataTable
        Dim rdt_OP As DataRow
        Try
            dtAFKO = SAP_DATA_READ_AFKO("AUFNR LIKE '%" & TextBox_OF.Text & "'")
            If dtAFKO Is Nothing Then Throw New Exception("L'OF n°" & TextBox_OF.Text & " n'a pas été trouvé dans SAP.")

            dtVRESB = SAP_DATA_READ_VRESB("RSNUM EQ '" & dtAFKO(0)("RSNUM").ToString & "'")
            dt_OP.Columns.Add("Opération", Type.GetType("System.String"))

            For Each rVRESB As DataRow In dtVRESB.Rows 'Select(sFiltre)
                rdt_OP = dt_OP.Select("[Opération] = '" & rVRESB("VORNR").ToString & "'").FirstOrDefault
                If Not rdt_OP Is Nothing Then Continue For
                dt_OP.Rows.Add()
                dt_OP.Rows(dt_OP.Rows.Count - 1)("Opération") = rVRESB("VORNR").ToString
            Next
            DropDownList_OP.DataSource = dt_OP
            DropDownList_OP.DataTextField = "Opération"
            DropDownList_OP.DataValueField = "Opération"
            DropDownList_OP.DataBind()
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
        End Try
    End Sub

    Protected Sub TextBox_NU_BAC_TextChanged(sender As Object, e As EventArgs) Handles TextBox_NU_BAC.TextChanged

        'vérifier n° bac associé à l'OF et si le composant correspond
        Dim sQuery As String = ""
        Dim dtMSEG, dt As New DataTable
        Try
            sQuery = "SELECT [ID_GST_CNTR]
                            ,[ID_CPT]
                            ,[NM_QTE_INIT]
                            ,ISNULL([NB_UTLS],0) AS NB_UTLS
                        FROM [dbo].[V_LIST_CONT_NON_VIDE]
                       WHERE [NM_CNTR] = '" & TextBox_NU_BAC.Text & "' AND [NM_OF] = '" & TextBox_OF.Text & "'"
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            If dt Is Nothing Then Throw New Exception("Le bac n°" & TextBox_NU_BAC.Text & " n'existe pas ou est vide")
            dtMSEG = SAP_DATA_READ_MSEG("MBLNR EQ '" & Left(dt(0)("ID_CPT").ToString, 10) & "' AND ZEILE EQ '" & Mid(dt(0)("ID_CPT").ToString, 11, 4) & "' AND MJAHR EQ '" & Mid(dt(0)("ID_CPT").ToString, 15, 4) & "'")
            If dtMSEG Is Nothing Then Throw New Exception("Pas de données trouvés pour l'ID composant")
            For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                If dtMSEG(0)("MATNR").ToString = rGridView_REPE.Cells(1).Text Then
                    rGridView_REPE.Cells(5).Text = TextBox_NU_BAC.Text
                    rGridView_REPE.Cells(6).Text = dt(0)("ID_CPT").ToString
                    rGridView_REPE.Cells(7).Text = dtMSEG(0)("CHARG").ToString
                    rGridView_REPE.Cells(8).Text = Convert.ToDecimal(dt(0)("NM_QTE_INIT").ToString) - (Convert.ToDecimal(dt(0)("NB_UTLS").ToString) * Convert.ToDecimal(rGridView_REPE.Cells(4).Text))
                End If
            Next
            For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                If rGridView_REPE.Cells(8).Text = "&nbsp;" And Not (rGridView_REPE.Cells(3).Text = "PRODUIT" Or rGridView_REPE.Cells(3).Text = "PRODUIT SEMI-FINI") Then
                    TextBox_NU_BAC.Text = ""
                    TextBox_NU_BAC.Focus()
                    Exit Sub
                End If
            Next
            MultiView_Tracabilité.SetActiveView(View_SAIS_ENS)
            TextBox_ENS.Text = ""
            TextBox_ENS.Focus()
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            TextBox_NU_BAC.Text = ""
            TextBox_NU_BAC.Focus()
        End Try
    End Sub

    Protected Sub TextBox_CD_LOT_COMP_TextChanged(sender As Object, e As EventArgs) Handles TextBox_CD_LOT_COMP.TextChanged
        Dim sQuery As String = ""
        Dim dtMSEG, dt As New DataTable
        Dim fQT_UTLS As Decimal
        Try
            For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                If Label_CD_COMP.Text = rGridView_REPE.Cells(1).Text Then
                    dtMSEG = SAP_DATA_READ_MSEG("MATNR EQ '" & Label_CD_COMP.Text & "' AND CHARG EQ '" & TextBox_CD_LOT_COMP.Text & "'")
                    If dtMSEG Is Nothing Then Throw New Exception("Pas de données")
                    For Each rdtMSEG As DataRow In dtMSEG.Rows
                        sQuery = "SELECT COUNT([DT_PSG]) AS NB_ID_COMP
                                   FROM [dbo].[DTM_TR_CPT]
                                  WHERE [ID_CPT] LIKE '" & rdtMSEG("MBLNR").ToString & rdtMSEG("ZEILE").ToString & rdtMSEG("MJAHR").ToString & "%'
                                 GROUP BY [ID_CPT]"
                        dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
                        If dt Is Nothing Then
                            rGridView_REPE.Cells(8).Text = Convert.ToDecimal(Replace(rdtMSEG("MENGE").ToString, ".", ","))
                            rGridView_REPE.Cells(6).Text = rdtMSEG("MBLNR").ToString & rdtMSEG("ZEILE").ToString & rdtMSEG("MJAHR").ToString
                            Exit For
                        Else
                            fQT_UTLS = Convert.ToDecimal(dt(0)("NB_ID_COMP").ToString) * Convert.ToDecimal(rGridView_REPE.Cells(4).Text)
                            If fQT_UTLS < Convert.ToDecimal(Replace(dtMSEG(0)("MENGE").ToString, ".", ",")) Then
                                rGridView_REPE.Cells(8).Text = Convert.ToDecimal(Replace(rdtMSEG("MENGE").ToString, ".", ",")) - fQT_UTLS
                                rGridView_REPE.Cells(6).Text = rdtMSEG("MBLNR").ToString & rdtMSEG("ZEILE").ToString & rdtMSEG("MJAHR").ToString
                                Exit For
                            End If
                        End If
                    Next
                    rGridView_REPE.Cells(7).Text = TextBox_CD_LOT_COMP.Text
                End If
            Next
            For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                If rGridView_REPE.Cells(8).Text = "&nbsp;" And Not (rGridView_REPE.Cells(3).Text = "PRODUIT" Or rGridView_REPE.Cells(3).Text = "PRODUIT SEMI-FINI") Then
                    MultiView_SAIS_CD_LOT.SetActiveView(View_SAIS_CD_LOT_SAIS)
                    GridView_REPE.SelectRow(rGridView_REPE.RowIndex)
                    Label_CD_COMP.Text = GridView_REPE.SelectedRow.Cells(1).Text
                    TextBox_CD_LOT_COMP.Text = ""
                    TextBox_CD_LOT_COMP.Focus()
                    Exit Sub
                End If
            Next
            MultiView_Tracabilité.SetActiveView(View_SAIS_ENS)
            TextBox_ENS.Text = ""
            TextBox_ENS.Focus()
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            TextBox_CD_LOT_COMP.Text = ""
            TextBox_CD_LOT_COMP.Focus()
        End Try
    End Sub

    Protected Sub TextBox_ID_COMP_TextChanged(sender As Object, e As EventArgs) Handles TextBox_ID_COMP.TextChanged
        'chercher 
        Dim sQuery As String = ""
        Dim dtMSEG, dt As New DataTable
        Dim fQT_UTLS As Decimal
        Try
            dtMSEG = SAP_DATA_READ_MSEG("MBLNR EQ '" & Left(TextBox_ID_COMP.Text, 10) & "' AND ZEILE EQ '" & Mid(TextBox_ID_COMP.Text, 11, 4) & "' AND MJAHR EQ '" & Mid(TextBox_ID_COMP.Text, 15, 4) & "'")
            If dtMSEG Is Nothing Then Throw New Exception("Pas de données trouvés pour l'ID composant")
            For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                If dtMSEG(0)("MATNR").ToString = rGridView_REPE.Cells(1).Text Then
                    rGridView_REPE.Cells(6).Text = TextBox_ID_COMP.Text
                    rGridView_REPE.Cells(7).Text = dtMSEG(0)("CHARG").ToString
                    sQuery = "SELECT COUNT([DT_PSG]) AS NB_ID_COMP
                                   FROM [dbo].[DTM_TR_CPT]
                                  WHERE [ID_CPT] LIKE '" & TextBox_ID_COMP.Text & "%'
                                 GROUP BY [ID_CPT]"
                    dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
                    If dt Is Nothing Then
                        rGridView_REPE.Cells(8).Text = Convert.ToDecimal(Replace(dtMSEG(0)("MENGE").ToString, ".", ","))
                    Else
                        fQT_UTLS = Convert.ToDecimal(dt(0)("NB_ID_COMP").ToString) * Convert.ToDecimal(rGridView_REPE.Cells(4).Text)
                        If fQT_UTLS < Convert.ToDecimal(Replace(dtMSEG(0)("MENGE").ToString, ".", ",")) Then rGridView_REPE.Cells(8).Text = Convert.ToDecimal(Replace(dtMSEG(0)("MENGE").ToString, ".", ",")) - fQT_UTLS
                    End If
                End If
            Next
            For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                If rGridView_REPE.Cells(8).Text = "&nbsp;" And Not (rGridView_REPE.Cells(3).Text = "PRODUIT" Or rGridView_REPE.Cells(3).Text = "PRODUIT SEMI-FINI") Then
                    TextBox_ID_COMP.Text = ""
                    TextBox_ID_COMP.Focus()
                    Exit Sub
                End If
            Next
            MultiView_Tracabilité.SetActiveView(View_SAIS_ENS)
            TextBox_ENS.Text = ""
            TextBox_ENS.Focus()
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            TextBox_ID_COMP.Text = ""
            TextBox_ID_COMP.Focus()
        End Try
    End Sub

    Protected Sub RadioButtonList_SELE_COMP_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RadioButtonList_SELE_COMP.SelectedIndexChanged
        Select Case RadioButtonList_SELE_COMP.SelectedValue
            Case "Bac"
                MultiView_SELE_COMP.SetActiveView(View_BAC)
            Case "ID Composant"
                MultiView_SELE_COMP.SetActiveView(View_ID_COMP)
            Case "Code lot"
                MultiView_SELE_COMP.SetActiveView(View_CD_LOT)
                For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                    If rGridView_REPE.Cells(8).Text = "&nbsp;" And rGridView_REPE.Cells(3).Text = "&nbsp;" Then
                        MultiView_SAIS_CD_LOT.SetActiveView(View_SAIS_CD_LOT_SAIS)
                        GridView_REPE.SelectRow(rGridView_REPE.RowIndex)
                        Label_CD_COMP.Text = GridView_REPE.SelectedRow.Cells(1).Text
                        TextBox_CD_LOT_COMP.Focus()
                        Exit Sub
                    End If
                Next
        End Select
    End Sub

    Protected Sub _ERGT_TCBL(Optional sNU_SER_ECO As String = "0", Optional sNU_SER_CLIE As String = "vide")

        Dim sQuery As String = ""
        Dim dt_PARA, dt, dt_CFGR_ARTI_ECO, dt2 As New DataTable
        Dim iIDTT As Integer
        Try
            dt_CFGR_ARTI_ECO = DIG_FACT_SQL_CFGR_ARTI_ECO(Trim(Label_CD_ARTI.Text))

            sQuery = "SELECT [NEW_ID_PSG]
                        FROM [dbo].[V_NEW_ID_PSG_DTM_PSG]"
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)

            'Enregistrement
            sQuery = "INSERT INTO [dbo].[DTM_PSG] ([ID_PSG], [LB_ETP], [DT_DEB] , [LB_MOYN], [LB_PROG], [NM_MATR], [NM_NS_EOL], [LB_SCTN], [NM_OF])
                           VALUES (" & dt(0)("NEW_ID_PSG").ToString & ", '" & Label_DES_OP.Text & " (OP:" & Label_OP.Text & ")', GETDATE(), '" & System.Net.Dns.GetHostEntry(System.Web.HttpContext.Current.Request.UserHostAddress).HostName() & "', '" & HttpContext.Current.CurrentHandler.ToString & "', '" & Session("matricule") & "', '" & sNU_SER_ECO & "', 'P', '" & Label_OF.Text & "')"
            SQL_REQ_ACT(sQuery, sChaineConnexion)

            'enregistrer dans la base
            For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                iIDTT = 0
                If rGridView_REPE.Cells(3).Text = "PRODUIT" Or rGridView_REPE.Cells(3).Text = "PRODUIT SEMI-FINI" Then
                    sQuery = "INSERT INTO [dbo].[DTM_TR_CPT] ([NM_NS_EOL],[NM_NS_CLT],[ID_CPT],[ID_PSG],[DT_PSG],[NM_SAP_CPT],[NM_NS_SENS])
                                   VALUES ('" & sNU_SER_ECO & "', '" & sNU_SER_CLIE & "', '-', " & dt(0)("NEW_ID_PSG").ToString & ", GETDATE(), '" & rGridView_REPE.Cells(1).Text & "','" & rGridView_REPE.Cells(6).Text & "')"
                Else
                    sQuery = "INSERT INTO [dbo].[DTM_TR_CPT] ([NM_NS_EOL],[NM_NS_CLT],[ID_CPT],[ID_PSG],[DT_PSG],[NM_SAP_CPT])
                                   VALUES ('" & sNU_SER_ECO & "', '" & sNU_SER_CLIE & "', '" & rGridView_REPE.Cells(6).Text & "', " & dt(0)("NEW_ID_PSG").ToString & ", GETDATE(), '" & rGridView_REPE.Cells(1).Text & "')"
                End If
                iIDTT = SQL_REQ_ACT_RET_IDTT(sQuery, sChaineConnexion)
                If rGridView_REPE.Cells(5).Text <> "&nbsp;" Then
                    sQuery = "SELECT [ID_GST_CNTR]
                               FROM [dbo].[V_LIST_CONT_NON_VIDE]
                               WHERE [NM_CNTR] = '" & rGridView_REPE.Cells(5).Text & "' AND [NM_OF] = '" & TextBox_OF.Text & "'"
                    dt2 = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
                    If dt2 Is Nothing Then Throw New Exception("Le bac n°" & rGridView_REPE.Cells(5).Text & " n'existe pas ou est vide")
                    sQuery = "INSERT INTO [dbo].[DTM_TCBL_CONT_COMP] ([ID_GST_CNTR],[ID_TR])
                                   VALUES ('" & dt2(0)("ID_GST_CNTR").ToString & "', '" & iIDTT.ToString & "')"
                    SQL_REQ_ACT(sQuery, sChaineConnexion)
                End If
            Next
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            'Exit Sub
        End Try
    End Sub

    Protected Sub _RAZ_AFCG()
        Dim sQuery As String = ""
        'Dim dt_NS_TRAC, dtMSEG, dt As New DataTable
        'Dim fQT_UTLS As Decimal
        Try
            MultiView_BASC_SAI_SEL.SetActiveView(View_VOID)
            Label_CD_SS_ENS.Text = ""
            sQuery = $"SELECT ISNULL(CASE [NM_NS_CLT] WHEN '' THEN [NM_NS_EOL] ELSE [NM_NS_CLT] END,[NM_NS_EOL]) AS [Numéros de série tracés]
                         FROM [dbo].[ID_PF_View]
                        WHERE [LB_ETP] = '{Label_DES_OP.Text} (OP:{Label_OP.Text})' AND [NM_OF] = '{Label_OF.Text}'
                       GROUP BY ISNULL(CASE [NM_NS_CLT] WHEN '' THEN [NM_NS_EOL] ELSE [NM_NS_CLT] END,[NM_NS_EOL]), [LB_ETP], [NM_OF]
                       ORDER BY ISNULL(CASE [NM_NS_CLT] WHEN '' THEN [NM_NS_EOL] ELSE [NM_NS_CLT] END,[NM_NS_EOL]) DESC"
            TextBox_SS_ENS.Text = ""
            Using dt_NS_TRAC = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
                Session("DT_NS_TRAC") = dt_NS_TRAC
                GridView_SN_TRAC.DataSource = Session("DT_NS_TRAC")
                GridView_SN_TRAC.DataBind()
                MultiView_Tracabilité.SetActiveView(View_SAIS_ENS)

                'mise à jour des quantité restante
                For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                    If rGridView_REPE.Cells(5).Text <> "&nbsp;" Then
                        sQuery = $"SELECT [NM_QTE_INIT], ISNULL([NB_UTLS],0) AS NB_UTLS
                                     FROM [dbo].[V_LIST_CONT_NON_VIDE]
                                    WHERE [NM_CNTR] = '{rGridView_REPE.Cells(5).Text}' AND [NM_OF] = '{TextBox_OF.Text}'"
                        Using dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
                            If dt Is Nothing Then
                                rGridView_REPE.Cells(8).Text = dt(0)("NM_QTE_INIT").ToString
                            Else
                                rGridView_REPE.Cells(8).Text = Convert.ToDecimal(dt(0)("NM_QTE_INIT").ToString) - (Convert.ToDecimal(dt(0)("NB_UTLS").ToString) * Convert.ToDecimal(rGridView_REPE.Cells(4).Text))
                            End If
                        End Using
                    Else
                        Using dtMSEG = SAP_DATA_READ_MSEG($"MBLNR EQ '{Left(rGridView_REPE.Cells(6).Text, 10)}' AND ZEILE EQ '{Mid(rGridView_REPE.Cells(6).Text, 11, 4)}' AND MJAHR EQ '{Mid(rGridView_REPE.Cells(6).Text, 15, 4)}'")
                            If dtMSEG Is Nothing Then Continue For
                            sQuery = $"SELECT COUNT([DT_PSG]) AS NB_ID_COMP
                                         FROM [dbo].[DTM_TR_CPT]
                                        WHERE [ID_CPT] LIKE '{rGridView_REPE.Cells(6).Text}%'
                                       GROUP BY [ID_CPT]"
                            Using dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
                                If dt Is Nothing Then
                                    rGridView_REPE.Cells(8).Text = Convert.ToDecimal(Replace(dtMSEG(0)("MENGE").ToString, ".", ","))
                                Else
                                    rGridView_REPE.Cells(8).Text = Convert.ToDecimal(Replace(dtMSEG(0)("MENGE").ToString, ".", ",")) - Convert.ToDecimal(dt(0)("NB_ID_COMP").ToString) * Convert.ToDecimal(rGridView_REPE.Cells(4).Text)
                                End If
                            End Using
                        End Using
                    End If

                Next

                'vider la gridview
                For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                    If rGridView_REPE.Cells(3).Text = "PRODUIT" Or rGridView_REPE.Cells(3).Text = "PRODUIT SEMI-FINI" Then
                        rGridView_REPE.Cells(6).Text = "&nbsp;"
                    Else
                        If Convert.ToDecimal(rGridView_REPE.Cells(8).Text) = 0 Then
                            MultiView_Tracabilité.SetActiveView(View_CONT_LOT_ID_COMP)
                            Label_CD_COMP.Text = rGridView_REPE.Cells(1).Text
                        End If
                    End If
                Next

                LOG_MESS_UTLS(GetCurrentMethod, $"Le numéro de série {TextBox_ENS.Text} est tracé.")
                'Label_RES.Text = "Le numéro de série " & TextBox_ENS.Text & " est tracé."
                TextBox_ENS.Text = ""
                TextBox_ENS.Focus()

                'vérifier la quantité de l'of  
                If dt_NS_TRAC.Rows.Count = Convert.ToDecimal(Replace(Label_QT_OF.Text, ".", ",")) Then
                    TextBox_OF.Text = ""
                    DropDownList_OP.DataSource = ""
                    DropDownList_OP.DataBind()
                    'DropDownList_OP.SelectedValue = ""
                    MultiView_Tracabilité.SetActiveView(View_DATA_ENTR)
                    LOG_MESS_UTLS(GetCurrentMethod, $"L'OF {Label_OF.Text} est entièrement tracé.")
                    'Label_RES.Text = "L'OF " & Label_OF.Text & " est entièrement tracé."
                    Exit Sub
                End If
            End Using
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try
    End Sub
End Class