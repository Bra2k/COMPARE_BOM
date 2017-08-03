Imports App_Web.Class_SAP_DATA
Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports App_Web.Class_DIG_FACT
Imports App_Web.Class_DIG_FACT_SQL
Imports System.Data.SqlClient
Imports App_Web.Class_SQL
Imports App_Web.Class_COMM_APP_WEB

Public Class TCBL_ESB_SS_ESB_V2
    Inherits System.Web.UI.Page

    Public Sub Button_VALI_ENTER_Click(sender As Object, e As EventArgs) Handles Button_VALI_ENTER.Click

        Dim dtAFKO, dtVRESB, dt_SS_ENS, dt_CD_ARTI_ENS_SENS_SAP As New DataTable
        Dim sFiltre As String = "MTART = 'FERT'"
        Dim rdt_CD_ARTI_ENS_SENS_SAP, rdt_CD_ARTI_ENS_SENS_SAP_2 As DataRow

        Try
            dtAFKO = SAP_DATA_READ_AFKO("AUFNR EQ '00000" & TextBox_OF.Text & "'")
            If dtAFKO Is Nothing Then Throw New Exception("L'OF n°" & TextBox_OF.Text & " n'a pas été trouvé dans SAP.")

            TextBox_OP.Text = StrDup(4 - Len(TextBox_OP.Text), "0") & TextBox_OP.Text
            dtVRESB = SAP_DATA_READ_VRESB("RSNUM EQ '" & dtAFKO(0)("RSNUM").ToString & "' AND SPRAS EQ 'F' AND VORNR EQ '" & TextBox_OP.Text & "'")
            If dtAFKO Is Nothing Then Throw New Exception("L'extraction des composant associés à l'OP " & TextBox_OP.Text & " a échoué.")
            dt_SS_ENS.Columns.Add("Code article SAP", Type.GetType("System.String"))
            dt_SS_ENS.Columns.Add("Désignation de l'article", Type.GetType("System.String"))
            dt_SS_ENS.Columns.Add("Numéro de série associé", Type.GetType("System.String"))

            COMM_APP_WEB_PARA_AFFI_LOAD(dtAFKO(0)("PLNBEZ").ToString & "(" & TextBox_OP.Text & ")", "CheckBox_SS_ENS_FERT", View_DATA_ENTR)
            If CheckBox_SS_ENS_FERT.Checked = False Then sFiltre = "MTART = 'ROH'"
            'chercher les liaisons ensemble / sous-ensemble sur sql
            dt_CD_ARTI_ENS_SENS_SAP = DIG_FACT_SQL_GET_ASCT_ENS_SS_ENS_BY_CD_ART_SAP(dtAFKO(0)("PLNBEZ").ToString)
            For Each rVRESB As DataRow In dtVRESB.Select(sFiltre)
                rdt_CD_ARTI_ENS_SENS_SAP = dt_CD_ARTI_ENS_SENS_SAP.Select("[CD_SAP_SENS] = '" & rVRESB("MATNR").ToString & "'").FirstOrDefault
                If rdt_CD_ARTI_ENS_SENS_SAP Is Nothing Then Continue For
                rdt_CD_ARTI_ENS_SENS_SAP_2 = dt_SS_ENS.Select("[Code article SAP] = '" & rVRESB("MATNR").ToString & "'").FirstOrDefault
                If Not rdt_CD_ARTI_ENS_SENS_SAP_2 Is Nothing Then Continue For
                dt_SS_ENS.Rows.Add()
                dt_SS_ENS.Rows(dt_SS_ENS.Rows.Count - 1)("Code article SAP") = rVRESB("MATNR").ToString
                dt_SS_ENS.Rows(dt_SS_ENS.Rows.Count - 1)("Désignation de l'article") = rVRESB("MAKTX").ToString
            Next
            If dt_SS_ENS.Rows.Count = 0 Then Throw New Exception("Pas de sous ensemble (PRODUIT) pour l'opération " & TextBox_OP.Text)
            GridView_REPE.DataSource = dt_SS_ENS
            GridView_REPE.DataBind()

            Label_OF.Text = TextBox_OF.Text

            Dim dtMARA As DataTable = SAP_DATA_READ_MARA("MATNR EQ '" & dtAFKO(0)("PLNBEZ").ToString & "'")
            Dim dtT179T As DataTable = SAP_DATA_READ_T179T("PRODH EQ '" & dtMARA(0)("PRDHA").ToString & "'")

            Label_CLIE.Text = dtT179T(0)("VTEXT").ToString
            Label_CD_ARTI.Text = dtAFKO(0)("PLNBEZ").ToString
            Dim dtMAKT As DataTable = SAP_DATA_READ_MAKT("MATNR EQ '" & dtAFKO(0)("PLNBEZ").ToString & "'")
            Label_DES_ARTI.Text = dtMAKT(0)("MAKTX").ToString
            Label_OP.Text = TextBox_OP.Text
            Dim dtAFVC As DataTable = SAP_DATA_READ_AFVC("AUFPL EQ '" & dtAFKO(0)("AUFPL").ToString & "' AND VORNR EQ '" & Label_OP.Text & "'")
            Label_DES_OP.Text = dtAFVC(0)("LTXA1").ToString
            Label_QT_OF.Text = dtAFKO(0)("GAMNG").ToString
            Dim dt_NS_TRAC As DataTable = DIG_FACT_SQL_P_GET_TRAC_NU_SER_ENS_NU_SER_SS_ENS(Label_OF.Text, Label_DES_OP.Text & " (OP:" & Label_OP.Text & ")")
            'Session("DT_NS_TRAC") = dt_NS_TRAC
            If dt_NS_TRAC Is Nothing Then
                Dim dt_NS_TRAC_temp As New DataTable
                dt_NS_TRAC_temp.Columns.Add(Label_CD_ARTI.Text, Type.GetType("System.String"))
                For Each r_SS_ENS As DataRow In dt_SS_ENS.Rows
                    dt_NS_TRAC_temp.Columns.Add(r_SS_ENS("Code article SAP").ToString, Type.GetType("System.String"))
                Next
                dt_NS_TRAC = dt_NS_TRAC_temp
            Else
                Session("DT_NS_TRAC") = dt_NS_TRAC
            End If
            GridView_SN_TRAC.DataSource = Session("DT_NS_TRAC")
            GridView_SN_TRAC.DataBind()
            COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI.Text & "(" & Label_OP.Text & ")", "CheckBox_GENE_ETI_ENS", View_DATA_ENTR)
            If CheckBox_GENE_ETI_ENS.Checked = True Then
                'générer un numéro d'ensemble par la base via sp sql
                'TextBox_ENS.Text = 123
            End If

            MultiView_Tracabilité.SetActiveView(View_SAIS_ENS)
            COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI.Text & "(" & Label_OP.Text & ")", "CheckBox_SAI_AUTO", View_SAIS_ENS)
            COMM_APP_WEB_PARA_AFFI_LOAD(Label_CD_ARTI.Text & "(" & Label_OP.Text & ")", "CheckBox_NU_SER_EOL", View_SAIS_ENS)
            TextBox_ENS.Focus()
            Label_RES.Text = ""
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try

    End Sub

    Protected Sub TextBox_ENS_TextChanged(sender As Object, e As EventArgs) Handles TextBox_ENS.TextChanged
        'vérification que le ns n'est pas déjà associé 
        Dim sQuerySql As String = "SELECT [NM_NS_EOL], [NM_NS_SENS]
                                     FROM [MES_Digital_Factory].[dbo].[ID_PF_View]
                                    WHERE (Not ([NM_NS_SENS] Is NULL)) And (NM_NS_EOL = '" & TextBox_ENS.Text & "') AND ([LB_ETP] = '" & Label_DES_OP.Text & " (OP:" & Label_OP.Text & ")')"
        '                           "SELECT     NM_NS_EOL, NM_NS_SENS
        '                             FROM     [MES_Digital_Factory].dbo.DTM_TR_CPT
        '                            WHERE     (NOT (NM_NS_SENS IS NULL)) AND (NM_NS_EOL = '" & TextBox_ENS.Text & "')"
        Dim sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=MES_Digital_Factory;Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
        Dim dt_PARA As DataTable = SQL_SELE_TO_DT(sQuerySql, sChaineConnexion)
        If Not dt_PARA Is Nothing Then
            LOG_Erreur(GetCurrentMethod, "Le numéro de série " & TextBox_ENS.Text & " est déjà attribué dans la base.")
            TextBox_ENS.Text = ""
            TextBox_ENS.Focus()
            Exit Sub
        End If

        'vérifier par rapport à un format
        If DIG_FACT_VERI_FORM_NU_SER(Label_CD_ARTI.Text, "Format Numéro de série Eolane", TextBox_ENS.Text) = False Then
            LOG_Erreur(GetCurrentMethod, "Le numéro de série " & TextBox_ENS.Text & " ne correspond au format défini dans la base.")
            TextBox_ENS.Text = ""
            TextBox_ENS.Focus()
            Exit Sub
        End If

        Label_NS_ENS.Text = TextBox_ENS.Text
        MultiView_Tracabilité.SetActiveView(View_SAIS_SS_ENS)
        MultiView_BASC_SAI_SEL.SetActiveView(View_SEL)
        Select Case True
            Case CheckBox_NU_SER_EOL.Checked
                Label_CD_SS_ENS.Text = ""
                MultiView_BASC_SAI_SEL.SetActiveView(View_SAI)
            Case CheckBox_SAI_AUTO.Checked
                MultiView_BASC_SAI_SEL.SetActiveView(View_SAI)
                GridView_REPE.SelectRow(0)
                Label_CD_SS_ENS.Text = GridView_REPE.SelectedRow.Cells(1).Text
        End Select
        Label_RES.Text = ""
        TextBox_SS_ENS.Focus()

    End Sub

    Protected Sub GridView_REPE_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GridView_REPE.SelectedIndexChanged
        Label_CD_SS_ENS.Text = GridView_REPE.SelectedRow.Cells(1).Text
        MultiView_BASC_SAI_SEL.SetActiveView(View_SAI)
        TextBox_SS_ENS.Focus()
    End Sub

    Protected Sub TextBox_SS_ENS_TextChanged(sender As Object, e As EventArgs) Handles TextBox_SS_ENS.TextChanged

        'vérifier que le ns est disponible
        Dim sQuerySql As String = "Select   NM_NS_SENS
                                     FROM   [MES_Digital_Factory].dbo.DTM_TR_CPT
                                     WHERE   NM_NS_SENS = '" & TextBox_SS_ENS.Text & "'"
        Dim sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=MES_Digital_Factory;Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
        Dim dt_TR_CPT, dtAFKO, dt_NS_TRAC, dtS034 As New DataTable
        Dim sParam_Format_NS As String = "Format Numéro de série Eolane"
        Dim iID_Passage As Long = 0
        Try
            dt_TR_CPT = SQL_SELE_TO_DT(sQuerySql, sChaineConnexion)
            If Not dt_TR_CPT Is Nothing Then Throw New Exception("Le numéro de série " & TextBox_SS_ENS.Text & " est déjà attribué dans la base.")

            MultiView_BASC_SAI_SEL.SetActiveView(View_SEL)

            'si les sous-ensemble ont des numéros de série eolane
            If CheckBox_NU_SER_EOL.Checked = True Then
                MultiView_BASC_SAI_SEL.SetActiveView(View_SAI)
                TextBox_SS_ENS.Focus()
                'chercher le code article du sous-ensemble à partir de l'of contenu dans le numéro de série
                dtAFKO = SAP_DATA_READ_AFKO("AUFNR EQ '00000" & Left(TextBox_SS_ENS.Text, 7) & "'")
                If dtAFKO.Rows.Count = 0 Then Throw New Exception("le numéro de série saisi ne correspond à aucun code article ci-dessus")

                'verifier que le format du numéro de série est cohérent avec le format déclaré dans la base
                If DIG_FACT_VERI_FORM_NU_SER(dtAFKO(0)("PLNBEZ").ToString, "Format Numéro de série Eolane", TextBox_SS_ENS.Text) = False Then Throw New Exception("Le numéro de série " & TextBox_SS_ENS.Text & " ne correspond au format défini dans la base.")

                'attribuer le ns à une case
                For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                    If Trim(dtAFKO(0)("PLNBEZ").ToString) = Trim(rGridView_REPE.Cells(1).Text) Then rGridView_REPE.Cells(3).Text = TextBox_SS_ENS.Text
                Next
            Else
                'vérifier par rapport à un format
                If CheckBox_SS_ENS_FERT.Checked = False Then sParam_Format_NS = "Format Numéro de série Fournisseur"
                's'il existe un format de numéro de lot pour le code article donnée utiliser ce paramètre
                If Not DIG_FACT_SQL_GET_PARA(Label_CD_SS_ENS.Text, "Format Numéro de lot") Is Nothing Then sParam_Format_NS = "Format Numéro de lot"
                If DIG_FACT_VERI_FORM_NU_SER(Label_CD_SS_ENS.Text, sParam_Format_NS, TextBox_SS_ENS.Text) = False Then Throw New Exception("Le numéro de lot " & TextBox_SS_ENS.Text & " ne correspond au format défini dans la base.")

                'Vérifier que le numéro de lot existe
                If Not DIG_FACT_SQL_GET_PARA(Label_CD_SS_ENS.Text, "Format Numéro de lot") Is Nothing Then
                    dtS034 = SAP_DATA_READ_S034("MATNR EQ '" & Trim(Label_CD_SS_ENS.Text) & "' AND CHARG EQ '" & TextBox_SS_ENS.Text & "'")
                    If dtS034 Is Nothing Then Throw New Exception("Le numéro de lot " & TextBox_SS_ENS.Text & " n'existe pas sous SAP.")
                End If
                'attribuer le ns à une case
                For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                    If Label_CD_SS_ENS.Text = rGridView_REPE.Cells(1).Text Then rGridView_REPE.Cells(3).Text = TextBox_SS_ENS.Text
                Next
            End If

            Label_CD_SS_ENS.Text = ""
            TextBox_SS_ENS.Text = ""

            For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                'si une case de la gridview est vide
                If rGridView_REPE.Cells(3).Text = "&nbsp;" Then
                    ' la saisie auto et que ce ne sont pas des numéros de série eolane
                    If CheckBox_SAI_AUTO.Checked = True And CheckBox_NU_SER_EOL.Checked = False Then
                        'saisir le sous-ensemble suivant
                        MultiView_BASC_SAI_SEL.SetActiveView(View_SAI)
                        GridView_REPE.SelectRow(rGridView_REPE.RowIndex)
                        Label_CD_SS_ENS.Text = GridView_REPE.SelectedRow.Cells(1).Text
                        TextBox_SS_ENS.Focus()
                    End If
                    'sortir de la fonction si il reste des éléments à saisir (case vide)
                    Exit Sub
                End If
            Next

            'imprimer ns

            'enregistrer dans la base
            For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                If Not DIG_FACT_SQL_GET_PARA(rGridView_REPE.Cells(1).Text, "Format Numéro de lot") Is Nothing Then
                    iID_Passage = DIG_FACT_SQL_P_ADD_TCBL_MAT_NS_ENS_V2(iID_Passage, Label_NS_ENS.Text, "", Label_DES_OP.Text & " (OP:" & Label_OP.Text & ")",
                                                                    rGridView_REPE.Cells(1).Text, "", Label_OF.Text,
                                                                    System.Net.Dns.GetHostEntry(System.Web.HttpContext.Current.Request.UserHostAddress).HostName(),
                                                                    HttpContext.Current.CurrentHandler.ToString, Replace(Session("matricule"), vbNull, "0"), rGridView_REPE.Cells(3).Text)
                Else
                    iID_Passage = DIG_FACT_SQL_P_ADD_TCBL_MAT_NS_ENS_V2(iID_Passage, Label_NS_ENS.Text, "", Label_DES_OP.Text & " (OP:" & Label_OP.Text & ")",
                                                                    rGridView_REPE.Cells(1).Text, rGridView_REPE.Cells(3).Text, Label_OF.Text,
                                                                    System.Net.Dns.GetHostEntry(System.Web.HttpContext.Current.Request.UserHostAddress).HostName(),
                                                                    HttpContext.Current.CurrentHandler.ToString, Replace(Session("matricule"), vbNull, "0"), "-")
                End If
            Next

            'vérifier la quantité de l'of                                                                                                                                     
            dt_NS_TRAC = DIG_FACT_SQL_P_GET_TRAC_NU_SER_ENS_NU_SER_SS_ENS(Label_OF.Text, Label_DES_OP.Text & " (OP:" & Label_OP.Text & ")")
            If dt_NS_TRAC.Rows.Count = Convert.ToDecimal(Replace(Label_QT_OF.Text, ".", ",")) Then
                TextBox_OF.Text = ""
                TextBox_OP.Text = ""
                MultiView_Tracabilité.SetActiveView(View_DATA_ENTR)
                LOG_Msg(GetCurrentMethod, "L'OF " & Label_OF.Text & " est entièrement tracé.")
                Label_RES.Text = "L'OF " & Label_OF.Text & " est entièrement tracé."
                Exit Sub
            End If
            Session("DT_NS_TRAC") = dt_NS_TRAC
            GridView_SN_TRAC.DataSource = Session("DT_NS_TRAC")
            GridView_SN_TRAC.DataBind()

            MultiView_Tracabilité.SetActiveView(View_SAIS_ENS)
            'vider la gridview
            For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                rGridView_REPE.Cells(3).Text = "&nbsp;"
            Next
            LOG_Msg(GetCurrentMethod, "Le numéro de série " & TextBox_ENS.Text & " est tracé.")
            Label_RES.Text = "Le numéro de série " & TextBox_ENS.Text & " est tracé."
            TextBox_ENS.Text = ""
            TextBox_ENS.Focus()

        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            MultiView_BASC_SAI_SEL.SetActiveView(View_SAI)
            'Exit Sub
        Finally
            TextBox_SS_ENS.Text = ""
            TextBox_SS_ENS.Focus()
        End Try

    End Sub

    Protected Sub CheckBox_GENE_ETI_ENS_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox_GENE_ETI_ENS.CheckedChanged
        If Label_CD_ARTI.Text <> "" And Label_OP.Text <> "" Then COMM_APP_WEB_PARA_AFFI_SAVE(Label_CD_ARTI.Text & "(" & Label_OP.Text & ")", "CheckBox_GENE_ETI_ENS", CheckBox_GENE_ETI_ENS.Checked)
    End Sub

    Protected Sub CheckBox_SAI_AUTO_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox_SAI_AUTO.CheckedChanged
        If Label_CD_ARTI.Text <> "" And Label_OP.Text <> "" Then COMM_APP_WEB_PARA_AFFI_SAVE(Label_CD_ARTI.Text & "(" & Label_OP.Text & ")", "CheckBox_SAI_AUTO", CheckBox_SAI_AUTO.Checked)
    End Sub

    Protected Sub CheckBox_NU_SER_EOL_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox_NU_SER_EOL.CheckedChanged
        If Label_CD_ARTI.Text <> "" And Label_OP.Text <> "" Then COMM_APP_WEB_PARA_AFFI_SAVE(Label_CD_ARTI.Text & "(" & Label_OP.Text & ")", "CheckBox_NU_SER_EOL", CheckBox_NU_SER_EOL.Checked)
    End Sub

    Protected Sub CheckBox_SS_ENS_FERT_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox_SS_ENS_FERT.CheckedChanged
        If Label_CD_ARTI.Text <> "" And Label_OP.Text <> "" Then COMM_APP_WEB_PARA_AFFI_SAVE(Label_CD_ARTI.Text & "(" & Label_OP.Text & ")", "CheckBox_SS_ENS_FERT", CheckBox_SS_ENS_FERT.Checked)
    End Sub

    Protected Sub GridView_SN_TRAC_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GridView_SN_TRAC.PageIndexChanging

        GridView_SN_TRAC.PageIndex = e.NewPageIndex
        GridView_SN_TRAC.DataSource = Session("DT_NS_TRAC")
        GridView_SN_TRAC.DataBind()
    End Sub
    Public Shared Function _LIST_ENS_SS_ENS(sOF As String, sOP As String) As DataTable
        Dim dtAFKO, dtVRESB, dt_SS_ENS, dt_CD_ARTI_ENS_SENS_SAP As New DataTable
        'Dim sFiltre As String = "MTART = 'FERT'"
        Dim rdt_CD_ARTI_ENS_SENS_SAP, rdt_CD_ARTI_ENS_SENS_SAP_2 As DataRow
        Try
            dtAFKO = SAP_DATA_READ_AFKO("AUFNR LIKE '%" & sOF & "'")
            If dtAFKO Is Nothing Then Throw New Exception("L'OF n°" & sOF & " n'a pas été trouvé dans SAP.")

            sOP = StrDup(4 - Len(sOP), "0") & sOP
            dtVRESB = SAP_DATA_READ_VRESB("RSNUM EQ '" & dtAFKO(0)("RSNUM").ToString & "' AND SPRAS EQ 'F' AND VORNR EQ '" & sOP & "'")
            If dtAFKO Is Nothing Then Throw New Exception("L'extraction des composant associés à l'OP " & sOP & " a échoué.")
            dt_SS_ENS.Columns.Add("Code article SAP", Type.GetType("System.String"))
            dt_SS_ENS.Columns.Add("Désignation de l'article", Type.GetType("System.String"))
            dt_SS_ENS.Columns.Add("Numéro de série associé", Type.GetType("System.String"))

            'COMM_APP_WEB_PARA_AFFI_LOAD(dtAFKO(0)("PLNBEZ").ToString & "(" & TextBox_OP.Text & ")", "CheckBox_SS_ENS_FERT", View_DATA_ENTR)
            'If CheckBox_SS_ENS_FERT.Checked = False Then sFiltre = "MTART = 'ROH'"
            'chercher les liaisons ensemble / sous-ensemble sur sql
            dt_CD_ARTI_ENS_SENS_SAP = DIG_FACT_SQL_GET_ASCT_ENS_SS_ENS_BY_CD_ART_SAP(dtAFKO(0)("PLNBEZ").ToString)
            For Each rVRESB As DataRow In dtVRESB.Rows 'Select(sFiltre)
                rdt_CD_ARTI_ENS_SENS_SAP = dt_CD_ARTI_ENS_SENS_SAP.Select("[CD_SAP_SENS] = '" & rVRESB("MATNR").ToString & "'").FirstOrDefault
                If rdt_CD_ARTI_ENS_SENS_SAP Is Nothing Then Continue For
                rdt_CD_ARTI_ENS_SENS_SAP_2 = dt_SS_ENS.Select("[Code article SAP] = '" & rVRESB("MATNR").ToString & "'").FirstOrDefault
                If Not rdt_CD_ARTI_ENS_SENS_SAP_2 Is Nothing Then Continue For
                dt_SS_ENS.Rows.Add()
                dt_SS_ENS.Rows(dt_SS_ENS.Rows.Count - 1)("Code article SAP") = rVRESB("MATNR").ToString
                dt_SS_ENS.Rows(dt_SS_ENS.Rows.Count - 1)("Désignation de l'article") = rVRESB("MAKTX").ToString
            Next
            If dt_SS_ENS.Rows.Count = 0 Then Throw New Exception("Pas de sous ensemble (PRODUIT) pour l'opération " & sOP)
            Return dt_SS_ENS
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try
    End Function

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