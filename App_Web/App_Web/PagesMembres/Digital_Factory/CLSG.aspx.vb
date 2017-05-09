Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports System.IO
Imports App_Web.Class_SQL
Imports App_Web.Class_SAP_DATA
Imports App_Web.Class_DIG_FACT
Imports App_Web.Class_DIG_FACT_SQL
Imports App_Web.Class_COMM_APP_WEB
Imports App_Web.Class_PDF
'Imports PdfSharp
'Imports PdfSharp.Drawing
'Imports PdfSharp.Drawing.Layout
'Imports PdfSharp.Pdf
Public Class CLSG
    Inherits System.Web.UI.Page
    Dim sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=" & Replace(Replace(My.Computer.Name, "CEDB03", "MES_Digital_Factory_DEV"), "CEAPP03", "MES_Digital_Factory") & ";Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=7200;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            '    If Session("displayname") = "" Then
            '        Context.GetOwinContext().Authentication.SignOut(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie)
            '    Else
            '        If App_Web.Class_COMM_APP_WEB.COMM_APP_WEB_GET_DROI_PAGE(Replace(HttpContext.Current.Request.Url.AbsoluteUri, "http://" & LCase(My.Computer.Name) & "/PagesMembres/", "~/PagesMembres/") & ".aspx", Session("department"), Session("User_Name")) = False Then Response.Redirect("~/PagesMembres/RDRC_PAGE_MEMB.aspx")
            '    End If
            If Session("OF_TO_CLSG") <> "" Then
                TextBox_NU_OF.Text = Session("OF_TO_CLSG")
                Session("OF_TO_CLSG") = ""
                TextBox_OF_TextChanged(sender, e)
            End If

        End If
        'LOG_Msg(GetCurrentMethod, sChaineConnexion)
    End Sub

    Protected Sub TextBox_OF_TextChanged(sender As Object, e As EventArgs) Handles TextBox_NU_OF.TextChanged
        Dim sQuery As String = ""
        Dim dtAFKO, dtMARA, dtT179T, dtMAKT, dt, dt_CFGR_ARTI_ECO, dt_REPE As New DataTable
        MultiView_SAIS.SetActiveView(View_NU_SER)
        Try
            dtAFKO = SAP_DATA_READ_AFKO("AUFNR LIKE '%" & TextBox_NU_OF.Text & "'")
            If dtAFKO Is Nothing Then Throw New Exception("L'OF " & TextBox_NU_OF.Text & " n'a pas été trouvé dans SAP.")
            dtMARA = SAP_DATA_READ_MARA("MATNR EQ '" & dtAFKO(0)("PLNBEZ").ToString & "'")
            If dtMARA Is Nothing Then Throw New Exception("Larticle de l'OF " & TextBox_NU_OF.Text & " n'a pas été trouvé dans SAP.")
            dtT179T = SAP_DATA_READ_T179T("PRODH EQ '" & dtMARA(0)("PRDHA").ToString & "'")
            If dtT179T Is Nothing Then Throw New Exception("Le client de l'OF " & TextBox_NU_OF.Text & " n'a pas été trouvé dans SAP.")
            dtMAKT = SAP_DATA_READ_MAKT("MATNR EQ '" & dtAFKO(0)("PLNBEZ").ToString & "'")
            If dtT179T Is Nothing Then Throw New Exception("La désignation d'aticle de l'OF " & TextBox_NU_OF.Text & " n'a pas été trouvé dans SAP.")

            'OF
            Label_NU_OF.Text = TextBox_NU_OF.Text
            'Nom du client
            Label_NM_CLIE.Text = Trim(dtT179T(0)("VTEXT").ToString)
            'Code article Eolane
            Label_CD_ARTI_ECO.Text = dtAFKO(0)("PLNBEZ").ToString
            dt_CFGR_ARTI_ECO = DIG_FACT_SQL_CFGR_ARTI_ECO(Trim(Label_CD_ARTI_ECO.Text))
            'affichage du bouton BL
            If dt_CFGR_ARTI_ECO(0)("BL").ToString = "1" Then Button_SAI_CART_BL.Enabled = True
            'Désignation article Eolane
            Label_NM_DSGT_ARTI.Text = dtMAKT(0)("MAKTX").ToString
            'Quantité de l'of
            Label_QT_OF.Text = dtAFKO(0)("GAMNG").ToString
            'Quantité restante dans l'of
            sQuery = "SELECT ISNULL(COUNT([NU_NS_EOL]) ,COUNT([NU_NS_CLI])) As NB_NU_NS
                       FROM [dbo].[V_CLSG_PRD_DTM_HIST_LIVR]
                      WHERE [NU_OF] = '" & TextBox_NU_OF.Text & "'"
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            If dt Is Nothing Then
                Label_QT_REST_OF.Text = Label_QT_OF.Text
            Else
                Label_QT_REST_OF.Text = (Replace(Label_QT_OF.Text, ".", ",") - Convert.ToDecimal(dt(0)("NB_NU_NS").ToString)).ToString
                If Convert.ToDecimal(Label_QT_REST_OF.Text) <= 0 Then
                    MultiView_SAIS.SetActiveView(View_OF)
                    TextBox_NU_OF.Text = ""
                    TextBox_NU_OF.Focus()
                    Label_QT_REST_OF.Text = "0"
                    Label_NU_OF.Text = ""
                    Label_NM_CLIE.Text = ""
                    Label_CD_ARTI_ECO.Text = ""
                    Label_NM_DSGT_ARTI.Text = ""
                    Label_QT_OF.Text = ""
                    Throw New Exception("L'OF " & TextBox_NU_OF.Text & " a été entièrement tracé.")
                End If
            End If
            'numéro de carton a modifier en prenant compte de la quantité max dans la carton
            sQuery = "SELECT MAX_NU_CART, NU_OF 
                       FROM (
                             SELECT     MAX(CONVERT(INTEGER, NU_CART)) AS MAX_NU_CART, NU_OF, COUNT(DT_SCAN) AS NB_NU_SER
                               FROM         dbo.V_CLSG_PRD_DTM_HIST_LIVR 
                             GROUP BY NU_OF, NU_PALE, QT_CRTN
					         HAVING      NU_OF = '" & TextBox_NU_OF.Text & "' AND (QT_CRTN IS NULL)
                            ) AS DT_MAX_NU_CART INNER JOIN 
						    (
                             SELECT CONVERT(INTEGER,VAL_PARA) AS VAL_PARA
						       FROM [dbo].[V_DER_DTM_REF_PARA]
						      WHERE CD_ARTI = '" & Label_CD_ARTI_ECO.Text & "' AND NM_PARA = 'Quantité carton'
						    ) AS DT_QT_CART ON VAL_PARA > NB_NU_SER
                      WHERE NOT MAX_NU_CART IS NULL"
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            If dt Is Nothing Then
                sQuery = "SELECT ISNULL(MAX(CONVERT(INTEGER,[NU_CART]))," & TextBox_NU_OF.Text & "000) + 1 As NEW_NU_CART
                            FROM (
                                    SELECT [NU_CART], [NU_OF]
                                      FROM [dbo].[V_CLSG_PRD_DTM_HIST_LIVR]
                                     WHERE NU_OF = '" & TextBox_NU_OF.Text & "'
                                 ) AS A"
                dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
                Label_NU_CART.Text = dt(0)("NEW_NU_CART").ToString
            Else
                Label_NU_CART.Text = dt(0)("MAX_NU_CART").ToString
                'Liste de numéro de série présents dans le carton
                sQuery = "Select "
                If dt_CFGR_ARTI_ECO(0)("Numéro de série client").ToString = "1" Then sQuery &= "[NU_NS_CLI] As [Numéro de série Client],"
                If dt_CFGR_ARTI_ECO(0)("Numéro de série Eolane").ToString = "1" Then sQuery &= "[NU_NS_EOL] As [Numéro de série Eolane],"
                If dt_CFGR_ARTI_ECO(0)("Carton spécifique").ToString = "1" Then sQuery &= "[CD_CRTN] As [Code Carton], [QT_CRTN] As [Quantité Carton],"
                sQuery = Left(sQuery, Len(sQuery) - 1) & " "
                sQuery &= "  FROM (
		                           SELECT MAX([DT_SCAN]) AS [MAX_DT_EXPE],[CD_CRTN],[NU_NS_EOL],[NU_NS_CLI],[QT_CRTN]
		                             FROM [dbo].[V_CLSG_PRD_DTM_HIST_LIVR]
		                            WHERE [NU_CART] = '" & Label_NU_CART.Text & "' AND [NU_OF] = '" & TextBox_NU_OF.Text & "'
		                           GROUP BY [CD_CRTN],[NU_NS_EOL],[NU_NS_CLI],[QT_CRTN],[NU_CART],[NU_OF]
		                           ) AS A
                           ORDER BY MAX_DT_EXPE "
                dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
                GridView_NU_SER_SCAN.DataSource = dt
                GridView_NU_SER_SCAN.DataBind()
                Label_NB_CART.Text = dt.Rows.Count.ToString
            End If

            'Rechercher le n° de palette
            sQuery = "SELECT MAX_NU_PALE, NU_OF
                       FROM (
                             SELECT MAX(CONVERT(INTEGER, NU_PALE)) AS MAX_NU_PALE, NU_OF, COUNT(NU_CART) AS NB_NU_CART
                               FROM (
                                    SELECT NU_PALE, NU_OF, NU_CART
                                      FROM dbo.V_CLSG_PRD_DTM_HIST_LIVR 
                                    GROUP BY NU_OF, NU_PALE, NU_BL, NU_CART
					                HAVING NU_OF = '" & TextBox_NU_OF.Text & "' AND NU_BL IS NULL
                                    ) AS A
                             GROUP BY NU_OF
                            ) AS DT_MAX_NU_CART INNER JOIN 
						    (
                             SELECT CONVERT(INTEGER,VAL_PARA) AS VAL_PARA
						       FROM [dbo].[V_DER_DTM_REF_PARA]
						      WHERE CD_ARTI = '" & Trim(Label_CD_ARTI_ECO.Text) & "' AND NM_PARA = 'Quantité de carton dans la palette'
						    ) AS DT_QT_PALE ON VAL_PARA > NB_NU_CART
                    WHERE NOT MAX_NU_PALE IS NULL"
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            If dt Is Nothing Then
                sQuery = "SELECT ISNULL(MAX([NU_PALE])," & TextBox_NU_OF.Text & "00) + 1 AS NEW_NU_PALE
                            FROM (
                                    SELECT [NU_PALE], [NU_OF]
                                      FROM [dbo].[V_CLSG_PRD_DTM_HIST_LIVR]
                                     WHERE NU_OF = '" & TextBox_NU_OF.Text & "'
                                 ) AS A"
                dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
                Label_NU_PALE_NU_V_NU_SER.Text = dt(0)("NEW_NU_PALE").ToString
            Else
                Label_NU_PALE_NU_V_NU_SER.Text = dt(0)("MAX_NU_PALE").ToString
                'Liste de cartons présents dans la palette
                sQuery = "SELECT [NU_CART] AS [Numéro de carton] , [QT_CRTN] AS [Quantité] 
                            FROM (
		                           SELECT [NU_CART],[QT_CRTN]
		                             FROM [dbo].[V_CLSG_PRD_DTM_HIST_LIVR]
		                            WHERE [NU_PALE] = '" & Label_NU_PALE_NU_V_NU_SER.Text & "'
		                           GROUP BY [NU_PALE],[NU_CART],[QT_CRTN]
		                          ) AS A"
                dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
                GridView_LIST_CART.DataSource = dt
                GridView_LIST_CART.DataBind()
                Label_NB_CART_PALE.Text = dt.Rows.Count.ToString
            End If

            'Sélectionner la textbox
            Select Case "1"
                Case dt_CFGR_ARTI_ECO(0)("Carton spécifique").ToString
                    TextBox_NU_CART_SCFQ.Enabled = True
                    TextBox_NU_CART_SCFQ.Focus()
                Case dt_CFGR_ARTI_ECO(0)("Numéro de série Eolane").ToString
                    TextBox_NU_SER_ECO.Enabled = True
                    TextBox_NU_SER_ECO.Focus()
                Case dt_CFGR_ARTI_ECO(0)("Numéro de série client").ToString
                    TextBox_NU_SER_CLIE.Enabled = True
                    TextBox_NU_SER_CLIE.Focus()
            End Select
            MultiView_SAIS.SetActiveView(View_NU_SER)

            'recherche op et nom d'op
            Dim dtAFVC = SAP_DATA_READ_AFVC("AUFPL EQ '" & dtAFKO(0)("AUFPL").ToString & "'") '177508
            If dtAFVC Is Nothing Then Throw New Exception("Pas de données pour ce numéro de gamme")
            For Each rdtAFVC As DataRow In dtAFVC.Rows
                Dim dtCRHD = SAP_DATA_READ_CRHD("OBJID EQ '" & rdtAFVC("ARBID").ToString & "'")
                If Not dtCRHD Is Nothing Then
                    Dim rdtCRHD = dtCRHD.Select("ARBPL = 'EMB01'").FirstOrDefault
                    If Not rdtCRHD Is Nothing Then
                        Label_NM_DSGT_ARTI_ECO.Text = Trim(rdtAFVC("LTXA1").ToString)
                        Label_NU_OP.Text = Convert.ToDecimal(Trim(rdtAFVC("VORNR").ToString))
                    End If
                End If
            Next

            'si param générer un ns client (medria)
            If dt_CFGR_ARTI_ECO(0)("Génération impression numéro de série").ToString = Label_NU_OP.Text Then
                'If COMM_APP_WEB_GET_PARA(Label_CD_ARTI_ECO.Text & "|Numéro de série client", "CheckBox_NU_SER_CLIE_GENE_AUTO", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx") = "True" Then
                DIG_FACT_IMPR_ETIQ(COMM_APP_WEB_GET_PARA(Label_CD_ARTI_ECO.Text & "|Numéro de série client", "TextBox_FICH_MODE", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx"),
                                   TextBox_NU_OF.Text, "", "Numéro de série client", "", "",
                                   "", "", "", Nothing, Session("matricule"))
            End If

            'Génération impression numéro de série

            dt_REPE = TCBL_ESB_SS_ESB_V2._LIST_ENS_SS_ENS(Label_NU_OF.Text, Label_NU_OP.Text)
            If Not dt_REPE Is Nothing Then
                GridView_REPE.DataSource = dt_REPE
                GridView_REPE.DataBind()
                'pour néomedlight 
                'saisir les sous-ensembles puis allez chercher le n° d'ensemble
                If dt_CFGR_ARTI_ECO(0)("Vue numéro ensemble numéros sous-ensemble").ToString <> "" Then _AFCG_TCBL_SS_ENS()
            End If

        Catch ex As Exception
            MultiView_SAIS.SetActiveView(View_OF)
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try

    End Sub

    Protected Sub TextBox_NU_SER_ECO_TextChanged(sender As Object, e As EventArgs) Handles TextBox_NU_SER_ECO.TextChanged
        Dim sQuery As String = ""
        Dim dt, dt_CFGR_ARTI_ECO As New DataTable
        Try
            dt_CFGR_ARTI_ECO = DIG_FACT_SQL_CFGR_ARTI_ECO(Trim(Label_CD_ARTI_ECO.Text))
            'Vérifier cohérence du format du NS
            If DIG_FACT_VERI_FORM_NU_SER(Label_CD_ARTI_ECO.Text, "Format Numéro de série Eolane", TextBox_NU_SER_ECO.Text) = False Then Throw New Exception("Le numéro de série " & TextBox_NU_SER_ECO.Text & " ne correspond au format défini dans la base.")
            'vérification doublon
            sQuery = "Select [NU_NS_EOL]
                            ,[NU_OF]
                        FROM [dbo].[V_CLSG_PRD_DTM_HIST_LIVR]
                       WHERE [NU_NS_EOL] = '" & TextBox_NU_SER_ECO.Text & "' AND [NU_OF] = '" & TextBox_NU_OF.Text & "'"
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            If Not dt Is Nothing Then Throw New Exception("Le numéro de série " & TextBox_NU_SER_ECO.Text & " a déjà été scanné.")
            'vérification que n° série appartient à l'OF
            If dt_CFGR_ARTI_ECO(0)("Vérification cohérence OF numero serie eolane").ToString = "1" Then 'vérification
                Dim sNU_SER_ECO As String = TextBox_NU_SER_ECO.Text, sOF As String = TextBox_NU_OF.Text
                If sNU_SER_ECO.Contains(sOF) = False Then Throw New Exception("Le numéro de série " & sNU_SER_ECO & " n'appartient pas à l'OF " & sOF)
            End If
            'vérification workflow
            If DIG_FACT_SQL_VRFC_WF(TextBox_NU_SER_ECO.Text, "", TextBox_NU_OF.Text, Label_NM_DSGT_ARTI_ECO.Text & " (OP:" & Label_NU_OP.Text & ")", sChaineConnexion) = False Then Throw New Exception("Problème détecté dans le Workflow.")

            'Saisie suivante
            TextBox_NU_SER_ECO.Enabled = False
            Select Case "1"
                Case dt_CFGR_ARTI_ECO(0)("Numéro de série client").ToString
                    TextBox_NU_SER_CLIE.Enabled = True
                    TextBox_NU_SER_CLIE.Focus()
                Case dt_CFGR_ARTI_ECO(0)("Carton spécifique").ToString
                    'Traçabilité sous-ensemble
                    If GridView_REPE.Rows.Count > 0 Then
                        _AFCG_TCBL_SS_ENS()
                        Exit Select
                    End If
                    'enregistrer les données
                    _ERGT_CLSG(Label_NU_OF.Text, Label_NU_CART.Text, TextBox_NU_SER_ECO.Text, TextBox_NU_SER_CLIE.Text, TextBox_NU_CART_SCFQ.Text, Label_NB_CART.Text)
                    TextBox_NU_CART_SCFQ.Enabled = True
                    TextBox_NU_CART_SCFQ.Focus()
                Case dt_CFGR_ARTI_ECO(0)("Numéro de série Eolane").ToString
                    'Traçabilité sous-ensemble
                    If GridView_REPE.Rows.Count > 0 Then
                        _AFCG_TCBL_SS_ENS()
                        Exit Select
                    End If
                    'enregistrer les données
                    _ERGT_CLSG(Label_NU_OF.Text, Label_NU_CART.Text, TextBox_NU_SER_ECO.Text, TextBox_NU_SER_CLIE.Text, TextBox_NU_CART_SCFQ.Text, Label_NB_CART.Text)
                    TextBox_NU_SER_ECO.Enabled = True
                    TextBox_NU_SER_ECO.Focus()
            End Select

        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            TextBox_NU_SER_ECO.Text = ""
            TextBox_NU_SER_ECO.Focus()
        End Try
    End Sub

    Protected Sub TextBox_NU_SER_CLIE_TextChanged(sender As Object, e As EventArgs) Handles TextBox_NU_SER_CLIE.TextChanged
        Dim sQuery As String = ""
        Dim dt, dt_CFGR_ARTI_ECO As New DataTable
        Try
            dt_CFGR_ARTI_ECO = DIG_FACT_SQL_CFGR_ARTI_ECO(Trim(Label_CD_ARTI_ECO.Text))
            'Vérifier cohérence du format du NS
            If DIG_FACT_VERI_FORM_NU_SER(Label_CD_ARTI_ECO.Text, "Format Numéro de série client", TextBox_NU_SER_CLIE.Text) = False Then Throw New Exception("Le numéro de série " & TextBox_NU_SER_CLIE.Text & " ne correspond au format défini dans la base.")
            'vérification doublon
            sQuery = "SELECT [NU_NS_CLI]
                            ,[NU_OF]
                        FROM [dbo].[V_CLSG_PRD_DTM_HIST_LIVR]
                       WHERE [NU_NS_CLI] = '" & TextBox_NU_SER_CLIE.Text & "' AND [NU_OF] = '" & TextBox_NU_OF.Text & "'"
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            If Not dt Is Nothing Then Throw New Exception("Le numéro de série " & TextBox_NU_SER_CLIE.Text & " a déjà été scanné.")
            'vérification workflow
            If DIG_FACT_SQL_VRFC_WF("", TextBox_NU_SER_CLIE.Text, TextBox_NU_OF.Text, Label_NM_DSGT_ARTI_ECO.Text & " (OP:" & Label_NU_OP.Text & ")", sChaineConnexion) = False Then Throw New Exception("Problème détecté dans le Workflow.")

            'Saisie suivante
            TextBox_NU_SER_ECO.Enabled = False
            'Traçabilité sous-ensemble
            If GridView_REPE.Rows.Count > 0 Then
                _AFCG_TCBL_SS_ENS()
            Else
                'enregistrer les données
                _ERGT_CLSG(Label_NU_OF.Text, Label_NU_CART.Text, TextBox_NU_SER_ECO.Text, TextBox_NU_SER_CLIE.Text, TextBox_NU_CART_SCFQ.Text, Label_NB_CART.Text)
                Select Case "1"
                    Case dt_CFGR_ARTI_ECO(0)("Carton spécifique").ToString
                        TextBox_NU_CART_SCFQ.Enabled = True
                        TextBox_NU_CART_SCFQ.Focus()
                    Case dt_CFGR_ARTI_ECO(0)("Numéro de série Eolane").ToString
                        TextBox_NU_SER_ECO.Enabled = True
                        TextBox_NU_SER_ECO.Focus()
                    Case dt_CFGR_ARTI_ECO(0)("Numéro de série client").ToString
                        TextBox_NU_SER_CLIE.Enabled = True
                        TextBox_NU_SER_CLIE.Focus()
                End Select
            End If
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            TextBox_NU_SER_CLIE.Text = ""
            TextBox_NU_SER_CLIE.Focus()
        End Try

    End Sub

    Protected Sub TextBox_NU_CART_SCFQ_TextChanged(sender As Object, e As EventArgs) Handles TextBox_NU_CART_SCFQ.TextChanged
        Dim dt_CFGR_ARTI_ECO As New DataTable
        Try
            dt_CFGR_ARTI_ECO = DIG_FACT_SQL_CFGR_ARTI_ECO(Trim(Label_CD_ARTI_ECO.Text))
            'Vérifier cohérence du format du Numéro de carton
            If DIG_FACT_VERI_FORM_NU_SER(Label_CD_ARTI_ECO.Text, "Format du code carton", TextBox_NU_CART_SCFQ.Text) = False Then Throw New Exception("Le numéro de carton " & TextBox_NU_CART_SCFQ.Text & " ne correspond au format défini dans la base.")
            'Saisie suivante
            TextBox_NU_CART_SCFQ.Enabled = False

            Select Case "1"
                Case dt_CFGR_ARTI_ECO(0)("Numéro de série Eolane").ToString
                    TextBox_NU_SER_ECO.Enabled = True
                    TextBox_NU_SER_ECO.Focus()
                Case dt_CFGR_ARTI_ECO(0)("Numéro de série client").ToString
                    TextBox_NU_SER_CLIE.Enabled = True
                    TextBox_NU_SER_CLIE.Focus()
                Case dt_CFGR_ARTI_ECO(0)("Carton spécifique").ToString
                    'Traçabilité sous-ensemble
                    If GridView_REPE.Rows.Count > 0 Then
                        _AFCG_TCBL_SS_ENS()
                        Exit Select
                    End If
                    'enregistrer les données
                    _ERGT_CLSG(Label_NU_OF.Text, Label_NU_CART.Text, TextBox_NU_SER_ECO.Text, TextBox_NU_SER_CLIE.Text, TextBox_NU_CART_SCFQ.Text, Label_NB_CART.Text)
                    TextBox_NU_CART_SCFQ.Enabled = True
                    TextBox_NU_CART_SCFQ.Focus()
            End Select
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            TextBox_NU_CART_SCFQ.Text = ""
            TextBox_NU_CART_SCFQ.Focus()
        End Try
    End Sub

    Protected Sub Button_CLOR_CART_Click(sender As Object, e As EventArgs) Handles Button_CLOR_CART.Click
        Dim sQuery As String = "", sFichier_Modele As String = "", sFichier_PDF As String = ""
        Dim dt, dtVar, dt_CFGR_ARTI_ECO, dtLIST_DATA As New DataTable
        Try
            dt_CFGR_ARTI_ECO = DIG_FACT_SQL_CFGR_ARTI_ECO(Trim(Label_CD_ARTI_ECO.Text))

            'Enregistrement des données
            sQuery = "UPDATE [dbo].[DTM_CLSG_CART]
                         SET [QT_CRTN] = '" & Label_NB_CART.Text & "'
                       WHERE [NU_CART] = '" & Label_NU_CART.Text & "'"
            SQL_REQ_ACT(sQuery, sChaineConnexion)
            'impression 
            For Each cell As TableCell In GridView_NU_SER_SCAN.HeaderRow.Cells
                dtVar.Columns.Add(cell.Text)
            Next
            For Each row As GridViewRow In GridView_NU_SER_SCAN.Rows
                dtVar.Rows.Add()
                For i As Integer = 0 To row.Cells.Count - 1
                    dtVar.Rows(row.RowIndex)(i) = row.Cells(i).Text
                Next
            Next
            sFichier_Modele = COMM_APP_WEB_GET_PARA(Label_CD_ARTI_ECO.Text & "|Carton", "TextBox_FICH_MODE", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx")
            Select Case Right(sFichier_Modele, 3)
                Case "PDF", "pdf" 'Impression PDF
                    'vue pdf carton
                    sQuery = "SELECT *
                                FROM [dbo].[" & dt_CFGR_ARTI_ECO(0)("Vue fichier PDF").ToString & "]
                               WHERE [NU_CART] = '" & Label_NU_CART.Text & "'"
                    dtLIST_DATA = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
                    If dtLIST_DATA Is Nothing Then
                        Dim sFichier As String = DIG_FACT_IMPR_PDF(sFichier_Modele,
                                                               TextBox_NU_OF.Text, "", "Carton", TextBox_NU_SER_CLIE.Text, TextBox_NU_SER_ECO.Text,
                                                               Label_NU_CART.Text, Label_NB_CART.Text, "", "", dtVar, dtLIST_DATA)
                        ClientScript.RegisterStartupScript([GetType](), "printPdf", "document.getElementById(""pdf"").src = """ & Path.GetFileName(sFichier) & """;
                                                                                     document.getElementById(""pdf"").onload = function() {window.frames[""pdf""].focus();
                                                                                                                                           window.frames[""pdf""].print();};", True)
                        sFichier_PDF = sFichier
                    Else
                        sFichier_PDF = "c:\sources\App_Web\PagesMembres\Digital_Factory\delivery_form_" & CInt(Int((10000000 * Rnd()) + 1)) & "_merge.pdf"
                        For iPDF = 0 To dtLIST_DATA.Rows.Count - 1 Step Convert.ToDecimal(dt_CFGR_ARTI_ECO(0)("Nombre de ligne dans le fichier PDF").ToString)
                            Dim sFichier As String = DIG_FACT_IMPR_PDF(sFichier_Modele,
                                                                       TextBox_NU_OF.Text, "", "Carton", TextBox_NU_SER_CLIE.Text, TextBox_NU_SER_ECO.Text,
                                                                       Label_NU_CART.Text, Label_NB_CART.Text, "", "", dtVar,
                            dtLIST_DATA, iPDF)
                            sFichier_PDF = PDF_CCTN_FICH(sFichier_PDF, sFichier)
                        Next
                        ClientScript.RegisterStartupScript([GetType](), "printPdf", "document.getElementById(""pdf"").src = """ & Path.GetFileName(sFichier_PDF) & """;
                                                                                     document.getElementById(""pdf"").onload = function() {window.frames[""pdf""].focus();
                                                                                                                                           window.frames[""pdf""].print();};", True)
                    End If

                Case "PRN", "prn" 'impression étiquette PRN
                    'truc pourri pour ALMS, retirer le code article court au début du SN
                    If Label_NM_CLIE.Text = "AIR LIQUIDE MEDICAL" Then

                        TextBox_NU_SER_CLIE.Text = COMM_APP_WEB_STRI_TRIM_LEFT(TextBox_NU_SER_CLIE.Text, TextBox_NU_SER_CLIE.Text.IndexOf("-"))
                        LOG_Msg(GetCurrentMethod, TextBox_NU_SER_CLIE.Text)
                    End If
                    LOG_Msg(GetCurrentMethod, "123")
                    DIG_FACT_IMPR_ETIQ(sFichier_Modele,
                                       TextBox_NU_OF.Text, "", "Carton", TextBox_NU_SER_CLIE.Text, TextBox_NU_SER_ECO.Text,
                                       Label_NU_CART.Text, Label_NB_CART.Text, "", dtVar)
            End Select

            'Rechercher le n° de palette
            'si quantité carton dans palette plein nouveau numéro de palette
            sQuery = "SELECT MAX_NU_PALE, NU_OF
                       FROM (
                             SELECT MAX(CONVERT(INTEGER, NU_PALE)) AS MAX_NU_PALE, NU_OF, COUNT(NU_CART) AS NB_NU_CART
                               FROM (
                                    SELECT NU_PALE, NU_OF, NU_CART
                                      FROM dbo.V_CLSG_PRD_DTM_HIST_LIVR 
                                    GROUP BY NU_OF, NU_PALE, NU_BL, NU_CART
					                HAVING NU_OF = '" & TextBox_NU_OF.Text & "' AND NU_BL IS NULL
                                    ) AS A
                             GROUP BY NU_OF
                            ) AS DT_MAX_NU_CART INNER JOIN 
						    (
                             SELECT CONVERT(INTEGER,VAL_PARA) AS VAL_PARA
						       FROM [dbo].[V_DER_DTM_REF_PARA]
						      WHERE CD_ARTI = '" & Trim(Label_CD_ARTI_ECO.Text) & "' AND NM_PARA = 'Quantité de carton dans la palette'
						    ) AS DT_QT_PALE ON VAL_PARA > NB_NU_CART
                      WHERE NOT MAX_NU_PALE IS NULL"

            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            If dt Is Nothing Then
                sQuery = "SELECT ISNULL(MAX([NU_PALE])," & TextBox_NU_OF.Text & "00) + 1 AS NEW_NU_PALE
                            FROM (
                                    SELECT [NU_PALE], [NU_OF]
                                      FROM [dbo].[V_CLSG_PRD_DTM_HIST_LIVR]
                                     WHERE NU_OF = '" & TextBox_NU_OF.Text & "'
                                 ) AS A"
                dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
                Label_NU_PALE_NU_V_NU_SER.Text = dt(0)("NEW_NU_PALE").ToString
            Else
                Label_NU_PALE_NU_V_NU_SER.Text = dt(0)("MAX_NU_PALE").ToString
                'Liste de cartons présents dans la palette
                sQuery = "SELECT [NU_CART] AS [Numéro de carton] , [QT_CRTN] AS [Quantité] 
                            FROM (
		                           SELECT [NU_CART],[QT_CRTN]
		                             FROM [dbo].[V_CLSG_PRD_DTM_HIST_LIVR]
		                            WHERE [NU_PALE] = '" & Label_NU_PALE_NU_V_NU_SER.Text & "'
		                           GROUP BY [NU_PALE],[NU_CART],[QT_CRTN]
		                          ) AS A"
                dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
                GridView_LIST_CART.DataSource = dt
                GridView_LIST_CART.DataBind()
                Label_NB_CART_PALE.Text = dt.Rows.Count.ToString
            End If

            'si param saisie OF activé retour sur vue OF et tout vider
            If dt_CFGR_ARTI_ECO(0)("Retour saisie Of pour colisage").ToString = "1" Then
                MultiView_SAIS.SetActiveView(View_OF)
                TextBox_NU_OF.Text = ""
                TextBox_NU_OF.Focus()
                Label_NU_CART.Text = ""
                'Label_QT_REST_OF.Text = "0"
                Label_NU_OF.Text = ""
                Label_NM_CLIE.Text = ""
                Label_CD_ARTI_ECO.Text = ""
                Label_NM_DSGT_ARTI.Text = ""
                Label_QT_OF.Text = ""
                GridView_REPE.DataSource = ""
                GridView_REPE.DataBind()
            Else
                sQuery = "SELECT ISNULL(MAX(CONVERT(INTEGER,[NU_CART]))," & TextBox_NU_OF.Text & "000) + 1 As NEW_NU_CART
                            FROM (
                                    SELECT [NU_CART], [NU_OF]
                                      FROM [dbo].[V_CLSG_PRD_DTM_HIST_LIVR]
                                     WHERE NU_OF = '" & TextBox_NU_OF.Text & "'
                                 ) AS A"
                dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
                Label_NU_CART.Text = dt(0)("NEW_NU_CART").ToString
            End If
            'TextBox_NU_CART_SCFQ.Enabled = False
            'TextBox_NU_CART_SCFQ.Text = ""
            'TextBox_NU_SER_ECO.Enabled = False
            'TextBox_NU_SER_ECO.Text = ""
            'TextBox_NU_SER_CLIE.Enabled = False
            'TextBox_NU_SER_CLIE.Text = ""
            Label_NB_CART.Text = ""
            GridView_NU_SER_SCAN.DataSource = ""
            GridView_NU_SER_SCAN.DataBind()
            Button_CLOR_CART.Enabled = False
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try

    End Sub
    Protected Sub _ERGT_CLSG(sNU_OF As String, sNU_CART As String, Optional sNU_SER_ECO As String = "0", Optional sNU_SER_CLIE As String = "vide", Optional sCD_CRTN As String = "vide", Optional sQT_CRTN As String = "vide")
        Dim sQuery As String = ""
        Dim dt, dt_CFGR_ARTI_ECO, dtMSEG, dt_var As New DataTable

        Try
            dt_CFGR_ARTI_ECO = DIG_FACT_SQL_CFGR_ARTI_ECO(Trim(Label_CD_ARTI_ECO.Text))
            sQuery = "SELECT [NEW_ID_PSG]
                        FROM [dbo].[V_NEW_ID_PSG_DTM_PSG]"
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)

            'Enregistrement
            sQuery = "INSERT INTO [dbo].[DTM_PSG] ([ID_PSG], [LB_ETP], [DT_DEB] , [LB_MOYN], [LB_PROG], [NM_MATR], [NM_NS_EOL], [LB_SCTN], [NM_OF])
                           VALUES (" & dt(0)("NEW_ID_PSG").ToString & ", '" & Label_NM_DSGT_ARTI_ECO.Text & " (OP:" & Label_NU_OP.Text & ")', GETDATE(), '" & System.Net.Dns.GetHostEntry(System.Web.HttpContext.Current.Request.UserHostAddress).HostName() & "', '" & HttpContext.Current.CurrentHandler.ToString & "', '" & Session("matricule") & "', '" & sNU_SER_ECO & "', 'P', '" & sNU_OF & "')"
            SQL_REQ_ACT(sQuery, sChaineConnexion)
            If sCD_CRTN = "vide" Or sCD_CRTN = "" Then
                sQuery = "INSERT INTO [dbo].[DTM_CLSG_CART]([NU_CART], [ID_PSG])
                               VALUES ('" & sNU_CART & "', " & dt(0)("NEW_ID_PSG").ToString & ")"
            Else
                sQuery = "INSERT INTO [dbo].[DTM_CLSG_CART]([NU_CART], [ID_PSG], [CD_CRTN], [QT_CRTN])
                               VALUES ('" & sNU_CART & "', " & dt(0)("NEW_ID_PSG").ToString & ", '" & sCD_CRTN & "', '" & sQT_CRTN & "')"
            End If
            SQL_REQ_ACT(sQuery, sChaineConnexion)
            If Not (sNU_SER_CLIE = "vide" Or sNU_SER_CLIE = "") Then
                sQuery = "INSERT INTO [dbo].[DTM_TR_CPT] ([NM_NS_EOL],[NM_NS_CLT],[ID_PSG],[DT_PSG], [ID_CPT])
                               VALUES ('" & sNU_SER_ECO & "', '" & sNU_SER_CLIE & "', " & dt(0)("NEW_ID_PSG").ToString & ", GETDATE(), '-')"
                SQL_REQ_ACT(sQuery, sChaineConnexion)
            End If
            'enregistrer dans la base
            For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                If DIG_FACT_SQL_GET_PARA(rGridView_REPE.Cells(1).Text, "Format Numéro de lot") Is Nothing Then
                    sQuery = "INSERT INTO [dbo].[DTM_TR_CPT] ([NM_NS_EOL],[NM_NS_CLT],[ID_CPT],[ID_PSG],[DT_PSG],[NM_SAP_CPT],[NM_NS_SENS])
                               VALUES ('" & sNU_SER_ECO & "', '" & sNU_SER_CLIE & "', '-', " & dt(0)("NEW_ID_PSG").ToString & ", GETDATE(), '" & rGridView_REPE.Cells(1).Text & "','" & rGridView_REPE.Cells(3).Text & "')"
                Else
                    dtMSEG = SAP_DATA_READ_MSEG("MATNR EQ '" & rGridView_REPE.Cells(1).Text & "' AND CHARG EQ '" & rGridView_REPE.Cells(3).Text & "'")
                    sQuery = "INSERT INTO [dbo].[DTM_TR_CPT] ([NM_NS_EOL],[NM_NS_CLT],[ID_CPT],[ID_PSG],[DT_PSG],[NM_SAP_CPT])
                               VALUES ('" & sNU_SER_ECO & "', '" & sNU_SER_CLIE & "', '" & dtMSEG(0)("MBLNR").ToString & dtMSEG(0)("ZEILE").ToString & dtMSEG(0)("MJAHR").ToString & "', " & dt(0)("NEW_ID_PSG").ToString & ", GETDATE(), '" & rGridView_REPE.Cells(1).Text & "')"
                End If
                SQL_REQ_ACT(sQuery, sChaineConnexion)
            Next
            'enregistrer le n° de palette
            sQuery = "INSERT INTO [dbo].[DTM_CLSG_PALE]([ID_PSG],[NU_PALE])
                           VALUES (" & dt(0)("NEW_ID_PSG").ToString & "," & Label_NU_PALE_NU_V_NU_SER.Text & ")"
            SQL_REQ_ACT(sQuery, sChaineConnexion)
            'impression d'une étiquette packaging
            If dt_CFGR_ARTI_ECO(0)("Impression étiquette packaging numéro de série client").ToString = "1" Then
                dt_var.Columns.Add("0")
                dt_var.Columns.Add("1")
                dt_var.Rows.Add()
                dt_var.Rows(0)("1") = Replace(sNU_SER_CLIE, dt_CFGR_ARTI_ECO(0)("Code article client").ToString & " ", "")
                DIG_FACT_IMPR_ETIQ(COMM_APP_WEB_GET_PARA(Label_CD_ARTI_ECO.Text & "|Numéro de série client", "TextBox_FICH_MODE", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx"),
                                   sNU_OF, "", "Numéro de série client", sNU_SER_CLIE, "", "", "", "", dt_var, Session("matricule"))
            End If
            If dt_CFGR_ARTI_ECO(0)("Impression étiquette packaging numéro de série Eolane").ToString = "1" Then
                DIG_FACT_IMPR_ETIQ(COMM_APP_WEB_GET_PARA(Label_CD_ARTI_ECO.Text & "|Numéro de série Eolane", "TextBox_FICH_MODE", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx"),
                                   sNU_OF, "", "Numéro de série Eolane", "", sNU_SER_ECO, "", "", "", Nothing, Session("matricule"))
            End If

            'si param générer un ns client (medria)
            If dt_CFGR_ARTI_ECO(0)("Génération impression numéro de série").ToString = Label_NU_OP.Text Then
                'If COMM_APP_WEB_GET_PARA(Label_CD_ARTI_ECO.Text & "|Numéro de série client", "CheckBox_NU_SER_CLIE_GENE_AUTO", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx") = "True" Then
                DIG_FACT_IMPR_ETIQ(COMM_APP_WEB_GET_PARA(Label_CD_ARTI_ECO.Text & "|Numéro de série client", "TextBox_FICH_MODE", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx"),
                                   TextBox_NU_OF.Text, "", "Numéro de série client", "", "",
                                   "", "", "", Nothing, Session("matricule"))
            End If
            'MAJ Affichage
            sQuery = "SELECT ISNULL(COUNT([NU_NS_EOL]) ,COUNT([NU_NS_CLI])) As NB_NU_NS
                       FROM [dbo].[V_CLSG_PRD_DTM_HIST_LIVR]
                      WHERE [NU_OF] = '" & sNU_OF & "'"
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            Label_QT_REST_OF.Text = (Convert.ToDecimal(Replace(Label_QT_OF.Text, ".", ",")) - Convert.ToDecimal(dt(0)("NB_NU_NS").ToString)).ToString

            sQuery = "SELECT "
            If dt_CFGR_ARTI_ECO(0)("Numéro de série client").ToString = "1" Then sQuery &= "[NU_NS_CLI] AS [Numéro de série Client],"
            If dt_CFGR_ARTI_ECO(0)("Numéro de série Eolane").ToString = "1" Then sQuery &= "[NU_NS_EOL] AS [Numéro de série Eolane],"
            If dt_CFGR_ARTI_ECO(0)("Carton spécifique").ToString = "1" Then sQuery &= "[CD_CRTN] As [Code Carton], [QT_CRTN] AS [Quantité Carton],"
            sQuery = Left(sQuery, Len(sQuery) - 1) & " "
            sQuery &= "  FROM (
		                           SELECT MAX([DT_SCAN]) AS [MAX_DT_EXPE],[CD_CRTN],[NU_NS_EOL],[NU_NS_CLI],[QT_CRTN]
		                             FROM [dbo].[V_CLSG_PRD_DTM_HIST_LIVR]
		                            WHERE [NU_CART] = '" & Label_NU_CART.Text & "' AND [NU_OF] = '" & TextBox_NU_OF.Text & "'
		                           GROUP BY [CD_CRTN],[NU_NS_EOL],[NU_NS_CLI],[QT_CRTN],[NU_CART],[NU_OF]
		                           ) AS A
						 ORDER BY [MAX_DT_EXPE] DESC"
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            GridView_NU_SER_SCAN.DataSource = dt
            GridView_NU_SER_SCAN.DataBind()
            Label_NB_CART.Text = dt.Rows.Count.ToString

            If Convert.ToDecimal(Label_NB_CART.Text) > 0 Then Button_CLOR_CART.Enabled = True

            'si carton plein
            If Convert.ToDecimal(Label_NB_CART.Text) >= Convert.ToDecimal(dt_CFGR_ARTI_ECO(0)("Quantité carton").ToString) Then
                Dim sender As New Object, e As New EventArgs
                Button_CLOR_CART_Click(sender, e)
            End If

            If Convert.ToDecimal(Label_QT_REST_OF.Text) <= 0 Then
                Dim sender As New Object, e As New EventArgs
                Button_CLOR_CART_Click(sender, e)
                MultiView_SAIS.SetActiveView(View_OF)
                TextBox_NU_OF.Text = ""
                TextBox_NU_OF.Focus()
                Label_NU_CART.Text = ""
                Label_QT_REST_OF.Text = ""
                Label_NU_OF.Text = ""
                Label_NM_CLIE.Text = ""
                Label_CD_ARTI_ECO.Text = ""
                Label_NM_DSGT_ARTI.Text = ""
                Label_QT_OF.Text = ""
                ImageButton_CLOT_PALE_Click(sender, e) 'If dt_CFGR_ARTI_ECO(0)("BL").ToString = "1" Then Button_SAI_PALE_BL_Click(sender, e) 'Button_SAI_CART_BL_Click(sender, e)
            End If

            'RAZ Affichage
            TextBox_NU_CART_SCFQ.Enabled = False
            TextBox_NU_CART_SCFQ.Text = ""
            TextBox_NU_SER_ECO.Enabled = False
            TextBox_NU_SER_ECO.Text = ""
            TextBox_NU_SER_CLIE.Enabled = False
            TextBox_NU_SER_CLIE.Text = ""

        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try
    End Sub

    Protected Sub Button_SUPP_LIGN_Click(sender As Object, e As ImageClickEventArgs) Handles Button_SUPP_LIGN.Click
        Dim sQuery As String = ""
        Dim dt, dt_CFGR_ARTI_ECO As New DataTable
        Try
            'obtenir la configuration
            dt_CFGR_ARTI_ECO = DIG_FACT_SQL_CFGR_ARTI_ECO(Trim(Label_CD_ARTI_ECO.Text))
            'Récupération de l'ID_PSG du numéro de série à supprimer
            sQuery = "SELECT [ID_PSG]
                        FROM [dbo].[V_CLSG_PRD_DTM_HIST_LIVR]"
            Select Case HttpUtility.HtmlDecode(GridView_NU_SER_SCAN.HeaderRow.Cells(1).Text)
                Case "Numéro de série Client"
                    sQuery &= " WHERE [NU_NS_CLI] = '" & GridView_NU_SER_SCAN.SelectedRow.Cells(1).Text & "'"
                Case "Numéro de série Eolane"
                    sQuery &= " WHERE [NU_NS_EOL] = '" & GridView_NU_SER_SCAN.SelectedRow.Cells(1).Text & "'"
                Case "Carton spécifique"
                    sQuery &= " WHERE [CD_CRTN] = '" & GridView_NU_SER_SCAN.SelectedRow.Cells(1).Text & "'"
            End Select
            sQuery &= " AND [NU_OF] = '" & Label_NU_OF.Text & "' AND [NU_CART] = '" & Label_NU_CART.Text & "' ORDER BY [DT_SCAN] DESC "
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            'sQuery = "DELETE FROM [dbo].[DTM_TR_CPT]
            '                WHERE ID_PSG = " & dt(0)("ID_PSG").ToString
            'SQL_REQ_ACT(sQuery, sChaineConnexion)
            'sQuery = "DELETE FROM [dbo].[DTM_CLSG_CART]
            '                WHERE ID_PSG = " & dt(0)("ID_PSG").ToString
            'SQL_REQ_ACT(sQuery, sChaineConnexion)

            'supprimer la ligne
            sQuery = "DELETE FROM [dbo].[DTM_PSG]
                            WHERE ID_PSG = " & dt(0)("ID_PSG").ToString
            SQL_REQ_ACT(sQuery, sChaineConnexion)

            'Rechagement de la quantité restante à effectuer sur l'of
            sQuery = "SELECT ISNULL(COUNT([NU_NS_EOL]) ,COUNT([NU_NS_CLI])) As NB_NU_NS
                       FROM [dbo].[V_CLSG_PRD_DTM_HIST_LIVR]
                      WHERE [NU_OF] = '" & TextBox_NU_OF.Text & "'"
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            Label_QT_REST_OF.Text = (Convert.ToDecimal(Replace(Label_QT_OF.Text, ".", ",")) - Convert.ToDecimal(dt(0)("NB_NU_NS").ToString)).ToString

            sQuery = "SELECT "
            If dt_CFGR_ARTI_ECO(0)("Numéro de série client").ToString = "1" Then sQuery &= "[NU_NS_CLI] AS [Numéro de série Client],"
            If dt_CFGR_ARTI_ECO(0)("Numéro de série Eolane").ToString = "1" Then sQuery &= "[NU_NS_EOL] AS [Numéro de série Eolane],"
            If dt_CFGR_ARTI_ECO(0)("Carton spécifique").ToString = "1" Then sQuery &= "[CD_CRTN] As [Code Carton], [QT_CRTN] AS [Quantité Carton],"
            sQuery = Left(sQuery, Len(sQuery) - 1) & " "
            sQuery &= "  FROM (
		                           SELECT MAX([DT_SCAN]) AS [MAX_DT_EXPE],[CD_CRTN],[NU_NS_EOL],[NU_NS_CLI],[QT_CRTN]
		                             FROM [dbo].[V_CLSG_PRD_DTM_HIST_LIVR]
		                            WHERE [NU_CART] = '" & Label_NU_CART.Text & "' AND [NU_OF] = '" & TextBox_NU_OF.Text & "'
		                           GROUP BY [CD_CRTN],[NU_NS_EOL],[NU_NS_CLI],[QT_CRTN],[NU_CART],[NU_OF]
		                           ) AS A
                       ORDER BY [MAX_DT_EXPE] DESC"
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            If dt Is Nothing Then
                GridView_NU_SER_SCAN.DataSource = dt
                Label_NB_CART.Text = dt.Rows.Count.ToString
            Else
                GridView_NU_SER_SCAN.DataSource = ""
                Label_NB_CART.Text = 0
            End If
            GridView_NU_SER_SCAN.DataBind()
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try

    End Sub

    Protected Sub Button_ANNU_SAIS_CART_Click(sender As Object, e As ImageClickEventArgs) Handles Button_ANNU_SAIS_CART.Click
        Dim sQuery As String = ""
        Dim dt, dt_CFGR_ARTI_ECO As New DataTable
        Try
            dt_CFGR_ARTI_ECO = DIG_FACT_SQL_CFGR_ARTI_ECO(Trim(Label_CD_ARTI_ECO.Text))
            sQuery = "SELECT [ID_PSG]
                        FROM [dbo].[V_CLSG_PRD_DTM_HIST_LIVR]
                       WHERE [NU_OF] = '" & Label_NU_OF.Text & "' AND [NU_CART] = '" & Label_NU_CART.Text & "' 
                      ORDER BY [DT_SCAN] DESC"
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            For Each rdt As DataRow In dt.Rows
                'sQuery = "DELETE FROM [dbo].[DTM_TR_CPT]
                '                WHERE ID_PSG = " & rdt("ID_PSG").ToString
                'SQL_REQ_ACT(sQuery, sChaineConnexion)
                'sQuery = "DELETE FROM [dbo].[DTM_CLSG_CART]
                '                WHERE ID_PSG = " & rdt("ID_PSG").ToString
                'SQL_REQ_ACT(sQuery, sChaineConnexion)
                sQuery = "DELETE FROM [dbo].[DTM_PSG]
                                WHERE ID_PSG = " & rdt("ID_PSG").ToString
                SQL_REQ_ACT(sQuery, sChaineConnexion)
            Next
            sQuery = "SELECT ISNULL(COUNT([NU_NS_EOL]) ,COUNT([NU_NS_CLI])) As NB_NU_NS
                       FROM [dbo].[V_CLSG_PRD_DTM_HIST_LIVR]
                      WHERE [NU_OF] = '" & TextBox_NU_OF.Text & "'"
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            Label_QT_REST_OF.Text = (Convert.ToDecimal(Replace(Label_QT_OF.Text, ".", ",")) - Convert.ToDecimal(dt(0)("NB_NU_NS").ToString)).ToString

            sQuery = "SELECT "
            If dt_CFGR_ARTI_ECO(0)("Numéro de série client").ToString = "1" Then sQuery &= "[NU_NS_CLI] AS [Numéro de série Client],"
            If dt_CFGR_ARTI_ECO(0)("Numéro de série Eolane").ToString = "1" Then sQuery &= "[NU_NS_EOL] AS [Numéro de série Eolane],"
            If dt_CFGR_ARTI_ECO(0)("Carton spécifique").ToString = "1" Then sQuery &= "[CD_CRTN] As [Code Carton], [QT_CRTN] AS [Quantité Carton],"
            sQuery = Left(sQuery, Len(sQuery) - 1) & " "
            sQuery &= "  FROM (
		                           SELECT MAX([DT_SCAN]) AS [MAX_DT_EXPE],[CD_CRTN],[NU_NS_EOL],[NU_NS_CLI],[QT_CRTN]
		                             FROM [dbo].[V_CLSG_PRD_DTM_HIST_LIVR]
		                            WHERE [NU_CART] = '" & Label_NU_CART.Text & "' AND [NU_OF] = '" & TextBox_NU_OF.Text & "'
		                           GROUP BY [CD_CRTN],[NU_NS_EOL],[NU_NS_CLI],[QT_CRTN],[NU_CART],[NU_OF]
		                           ) AS A
								   ORDER BY [MAX_DT_EXPE] DESC"
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            If dt Is Nothing Then
                GridView_NU_SER_SCAN.DataSource = dt
                Label_NB_CART.Text = dt.Rows.Count.ToString
            Else
                GridView_NU_SER_SCAN.DataSource = ""
                Label_NB_CART.Text = 0
            End If
            GridView_NU_SER_SCAN.DataBind()
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try
    End Sub

    Protected Sub TextBox_NU_BL_TextChanged(sender As Object, e As EventArgs) Handles TextBox_NU_BL.TextChanged '@effacer
        Dim dtMARA, dtT179T, dtLIPS, dtLIPSUP, dt As New DataTable
        Dim sQuery As String = ""

        Try
            dtLIPSUP = SAP_DATA_READ_LIPSUP("VBELN EQ " & TextBox_NU_BL.Text)
            If dtLIPSUP Is Nothing Then Throw New Exception("Pas de données pour le BL n°" & TextBox_NU_BL.Text)
            'code article eolane
            Label_CD_ARTI_ECO_V_CART.Text = Trim(dtLIPSUP(0)("MATNR").ToString)
            'désignation article
            Label_NM_DSGT_ARTI_V_CART.Text = dtLIPSUP(0)("ARKTX").ToString
            dtMARA = SAP_DATA_READ_MARA("MATNR EQ '" & Label_CD_ARTI_ECO_V_CART.Text & "'")
            dtT179T = SAP_DATA_READ_T179T("PRODH EQ '" & dtMARA(0)("PRDHA").ToString & "'")
            'client
            Label_NM_CLIE_V_CART.Text = dtT179T(0)("VTEXT").ToString
            'numéro de carton a modifier en prenant compte de la quantité max dans la carton
            sQuery = "SELECT MAX_NU_PALE, NU_BL
                       FROM (
                             SELECT     MAX(CONVERT(INTEGER, NU_PALE)) AS MAX_NU_PALE, NU_BL, COUNT(NU_CART) AS NB_NU_CART
                               FROM         dbo.V_CLSG_PRD_DTM_HIST_LIVR 
                             GROUP BY NU_BL, NU_PALE
					         HAVING      NU_BL = '" & TextBox_NU_BL.Text & "'
                            ) AS DT_MAX_NU_CART INNER JOIN 
						    (
                             SELECT CONVERT(INTEGER,VAL_PARA) AS VAL_PARA
						       FROM [dbo].[V_DER_DTM_REF_PARA]
						      WHERE CD_ARTI = '" & Label_CD_ARTI_ECO_V_CART.Text & "' AND NM_PARA = 'Quantité de carton dans la palette'
						    ) AS DT_QT_PALE ON VAL_PARA > NB_NU_CART"

            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            If dt Is Nothing Then
                sQuery = "SELECT ISNULL(MAX([NU_PALE]),0) + 1 AS NEW_NU_PALE
                            FROM [dbo].[DTM_CLSG_PALE]"
                dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
                Label_NU_PALE.Text = dt(0)("NEW_NU_PALE").ToString
            Else
                Label_NU_PALE.Text = dt(0)("MAX_NU_PALE").ToString
                'Liste de cartons présents dans la palette
                sQuery = "SELECT [NU_CART] AS [Numéro de carton] , [QT_CRTN] AS [Quantité] 
                            FROM (
		                           SELECT [NU_CART],[QT_CRTN]
		                             FROM [dbo].[V_CLSG_PRD_DTM_HIST_LIVR]
		                            WHERE [NU_PALE] = '" & Label_NU_PALE.Text & "' AND [NU_BL] = '" & TextBox_NU_BL.Text & "'
		                           GROUP BY [NU_PALE],[NU_CART],[NU_BL],[QT_CRTN]
		                          ) AS A"
                dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
                GridView_NU_CART.DataSource = dt
                GridView_NU_CART.DataBind()
                Label_NB_CART_SCAN.Text = dt.Rows.Count.ToString
            End If
            TextBox_NU_CART.Enabled = True
            TextBox_NU_CART.Focus()
        Catch ex As Exception
            MultiView_SAIS.SetActiveView(View_OF)
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try
    End Sub

    Protected Sub TextBox_NU_CART_TextChanged(sender As Object, e As EventArgs) Handles TextBox_NU_CART.TextChanged
        Dim sQuery As String = "", dt, dtVar, dtLIST_DATA, dtAFKO, dt_CFGR_ARTI_ECO As New DataTable, sFichier_PDF As String

        Try
            dt_CFGR_ARTI_ECO = DIG_FACT_SQL_CFGR_ARTI_ECO(Trim(Label_CD_ARTI_ECO_V_CART.Text))
            'saisie par ns pour STW
            'vérification existance
            sQuery = "SELECT [NU_CART]                          
                        FROM [dbo].[V_CLSG_PRD_DTM_HIST_LIVR]
                       WHERE [NU_CART] = '" & TextBox_NU_CART.Text & "'"
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            If dt Is Nothing Then Throw New Exception("Le numéro de carton " & TextBox_NU_CART.Text & " n'existe pas.")
            'vérification doublon
            sQuery = "SELECT [NU_CART]
                            ,[NU_PALE]
                            ,[NU_BL]
                        FROM [dbo].[V_CLSG_PRD_DTM_HIST_LIVR]
                       WHERE [NU_CART] = '" & TextBox_NU_CART.Text & "' AND [NU_BL] = '" & TextBox_NU_BL.Text & "'"
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            If Not dt Is Nothing Then Throw New Exception("Le numéro de carton " & TextBox_NU_CART.Text & " a déjà été scanné.")
            'vérification cohérence cd article
            sQuery = "SELECT [NU_OF]                          
                        FROM [dbo].[V_CLSG_PRD_DTM_HIST_LIVR]
                       WHERE [NU_CART] = '" & TextBox_NU_CART.Text & "'
                      GROUP BY [NU_OF]"
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            dtAFKO = SAP_DATA_READ_AFKO("AUFNR LIKE '%" & dt(0)("NU_OF").ToString & "'")
            If Trim(dtAFKO(0)("PLNBEZ").ToString) <> Label_CD_ARTI_ECO_V_CART.Text Then Throw New Exception("Le code article " & Trim(dtAFKO(0)("PLNBEZ").ToString) & " qui a été trouvé dans la base ne correspond pas au code article associé au BL " & TextBox_NU_BL.Text & " (" & Label_CD_ARTI_ECO_V_CART.Text & ")")
            ' If dt Is Nothing Then Throw New Exception("Le numéro de carton " & TextBox_NU_CART.Text & " n'existe pas.")
            'enregistrement
            sQuery = "INSERT INTO [dbo].[DTM_CLSG_PALE]([ID_PSG],[NU_BL],[NU_PALE])
                           SELECT [ID_PSG],'" & TextBox_NU_BL.Text & "','" & Label_NU_PALE.Text & "'
                             FROM [dbo].[DTM_CLSG_CART]
                            WHERE [NU_CART] = '" & TextBox_NU_CART.Text & "'"
            SQL_REQ_ACT(sQuery, sChaineConnexion)

            'vérifier quantité max de la palette
            sQuery = "SELECT MAX_NU_PALE, NU_BL
                       FROM (
                             SELECT MAX_NU_PALE, NU_BL, COUNT(NU_CART) AS NB_NU_CART
					           FROM(
                                     SELECT     MAX(CONVERT(INTEGER, NU_PALE)) AS MAX_NU_PALE, NU_BL, NU_CART
                                       FROM         dbo.V_CLSG_PRD_DTM_HIST_LIVR 
                                     GROUP BY NU_BL, NU_PALE, NU_CART
					                 HAVING      NU_BL = '" & TextBox_NU_BL.Text & "' AND [NU_PALE] = '" & Label_NU_PALE.Text & "'
                                    ) AS DT_MAX_NU_CART 
							 GROUP BY MAX_NU_PALE, NU_BL
                            ) AS DT_COUN_NU_CART INNER JOIN 
						    (
                             SELECT CONVERT(INTEGER,VAL_PARA) AS VAL_PARA
						       FROM [dbo].[V_DER_DTM_REF_PARA]
						      WHERE CD_ARTI = '" & Label_CD_ARTI_ECO_V_CART.Text & "' AND NM_PARA = 'Quantité de carton dans la palette'
						    ) AS DT_QT_PALE ON VAL_PARA > NB_NU_CART"

            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            If dt Is Nothing Then 'nouvelle palette -> impression étiquette palette
                sQuery = "SELECT COUNT([DT_SCAN]) AS NB_NS                         
                            FROM [dbo].[V_CLSG_PRD_DTM_HIST_LIVR]
                           WHERE [NU_PALE] = '" & Label_NU_PALE.Text & "'"
                dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
                If dt Is Nothing Then Throw New Exception("La palette " & Label_NU_PALE.Text & " n'existe pas dans la base.")

                For Each cell As TableCell In GridView_NU_CART.HeaderRow.Cells
                    dtVar.Columns.Add(cell.Text)
                Next
                For Each row As GridViewRow In GridView_NU_CART.Rows
                    dtVar.Rows.Add()
                    For i As Integer = 0 To row.Cells.Count - 1
                        dtVar.Rows(row.RowIndex)(i) = row.Cells(i).Text
                    Next
                Next
                'Dim sARTI_ECO As String = Label_CD_ARTI_ECO_V_CART.Text & StrDup(18 - Len(Label_CD_ARTI_ECO_V_CART.Text), " ")
                Dim sFichier_Modele As String = COMM_APP_WEB_GET_PARA(Label_CD_ARTI_ECO_V_CART.Text & StrDup(18 - Len(Label_CD_ARTI_ECO_V_CART.Text), " ") & "|Palette", "TextBox_FICH_MODE", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx")
                Select Case Right(sFichier_Modele, 3)
                    Case "PDF", "pdf" 'Imprimer PDF
                        sQuery = "SELECT *
                                    FROM [dbo].[" & dt_CFGR_ARTI_ECO(0)("Vue fichier PDF").ToString & "]
                                   WHERE [NU_PALE] = '" & Label_NU_PALE.Text & "'"
                        dtLIST_DATA = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
                        If dtLIST_DATA Is Nothing Then
                            Dim sFichier As String = DIG_FACT_IMPR_PDF(sFichier_Modele,
                                                                       "", TextBox_NU_BL.Text, "Palette",
                                                                       "", "", "", Label_NB_CART_SCAN.Text,
                                                                       Label_NB_CART_SCAN.Text, Label_NU_PALE.Text, dtVar, dtLIST_DATA)
                            ClientScript.RegisterStartupScript([GetType](), "printPdf", "document.getElementById(""pdf"").src = """ & Path.GetFileName(sFichier) & """;
                                                                                     document.getElementById(""pdf"").onload = function() {window.frames[""pdf""].focus();
                                                                                                                                           window.frames[""pdf""].print();};", True)
                            sFichier_PDF = sFichier
                        Else
                            sFichier_PDF = "c:\sources\App_Web\PagesMembres\Digital_Factory\delivery_form_" & CInt(Int((10000000 * Rnd()) + 1)) & "_merge.pdf"
                            For iPDF = 0 To dtLIST_DATA.Rows.Count - 1 Step Convert.ToDecimal(dt_CFGR_ARTI_ECO(0)("Nombre de ligne dans le fichier PDF").ToString)
                                Dim sFichier As String = DIG_FACT_IMPR_PDF(sFichier_Modele,
                                          "", TextBox_NU_BL.Text, "Palette",
                                             "", "", "", Label_NB_CART_SCAN.Text,
                                             Label_NB_CART_SCAN.Text, Label_NU_PALE.Text, dtVar,
                                            dtLIST_DATA, iPDF)
                                sFichier_PDF = PDF_CCTN_FICH(sFichier_PDF, sFichier)
                            Next
                            ClientScript.RegisterStartupScript([GetType](), "printPdf", "document.getElementById(""pdf"").src = """ & Path.GetFileName(sFichier_PDF) & """;
                                                                                     document.getElementById(""pdf"").onload = function() {window.frames[""pdf""].focus();
                                                                                                                                           window.frames[""pdf""].print();};", True)
                            'File.Delete(sFichier_PDF)
                        End If
                        'Envoi de fichier par mail (e-takes care, forsee power, C43)
                        If Not dt_CFGR_ARTI_ECO(0)("Serveur SMTP mail") Is Nothing Then
                            Dim sEMET As String = "expedition.combree@eolane.com"
                            If Session("mail") <> "" Then sEMET = Session("mail")
                            COMM_APP_WEB_ENVO_MAIL(dt_CFGR_ARTI_ECO(0)("Serveur SMTP mail").ToString,
                                               sEMET,
                                               dt_CFGR_ARTI_ECO(0)("Destinataires mail").ToString,
                                               dt_CFGR_ARTI_ECO(0)("Sujet mail").ToString,
                                               dt_CFGR_ARTI_ECO(0)("Contenu mail").ToString,
                                               Session,
                                               dt_CFGR_ARTI_ECO(0)("Destinataires en copie mail").ToString,
                                               dt_CFGR_ARTI_ECO(0)("Destinataires en copie cachée mail").ToString,
                                               sFichier_PDF)
                        End If

                    Case "PRN", "prn" 'imprimer une étiquette
                        DIG_FACT_IMPR_ETIQ(sFichier_Modele,
                                       "", TextBox_NU_BL.Text, "Palette", "", "", "", dt(0)("NB_NS").ToString, Label_NB_CART_SCAN.Text, dtVar)
                End Select

                sQuery = "SELECT ISNULL(MAX([NU_PALE]),0) + 1 AS NEW_NU_PALE
                            FROM [dbo].[DTM_CLSG_PALE]"
                dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
                Label_NU_PALE.Text = dt(0)("NEW_NU_PALE").ToString

            Else
                Label_NU_PALE.Text = dt(0)("MAX_NU_PALE").ToString
                'Liste de cartons présents dans la palette
                sQuery = "SELECT [NU_CART] AS [Numéro de carton] , [QT_CRTN] AS [Quantité] 
                            FROM (
		                           SELECT [NU_CART],[QT_CRTN]
		                             FROM [dbo].[V_CLSG_PRD_DTM_HIST_LIVR]
		                            WHERE [NU_PALE] = '" & Label_NU_PALE.Text & "' AND [NU_BL] = '" & TextBox_NU_BL.Text & "'
		                           GROUP BY [NU_PALE],[NU_CART],[NU_BL],[QT_CRTN]
		                          ) AS A"
                dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
                GridView_NU_CART.DataSource = dt
                GridView_NU_CART.DataBind()
                Label_NB_CART_SCAN.Text = dt.Rows.Count.ToString
            End If

        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try
        TextBox_NU_CART.Text = ""
        TextBox_NU_CART.Focus()
    End Sub

    Protected Sub Button_CLOR_PALE_Click(sender As Object, e As EventArgs) Handles Button_CLOR_PALE.Click 'clore la saisie du BL
        Dim sQuery As String = "", sData As String = "", dt, dtLIPS, dtVar, dtLIST_DATA, dt_CFGR_ARTI_ECO As New DataTable, sFichier_PDF As String
        Randomize()
        Try
            dt_CFGR_ARTI_ECO = DIG_FACT_SQL_CFGR_ARTI_ECO(Trim(Label_CD_ARTI_ECO_V_CART.Text))

            sQuery = "SELECT COUNT([DT_SCAN]) AS NB_NS                         
                        FROM [dbo].[V_CLSG_PRD_DTM_HIST_LIVR]
                       WHERE [NU_PALE] = '" & Label_NU_PALE.Text & "'"
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            If dt Is Nothing Then Throw New Exception("La palette " & Label_NU_PALE.Text & " n'existe pas dans la base.")

            For Each cell As TableCell In GridView_NU_CART.HeaderRow.Cells
                dtVar.Columns.Add(cell.Text)
            Next
            For Each row As GridViewRow In GridView_NU_CART.Rows
                dtVar.Rows.Add()
                For i As Integer = 0 To row.Cells.Count - 1
                    dtVar.Rows(row.RowIndex)(i) = row.Cells(i).Text
                Next
            Next
            'Dim sARTI_ECO As String =
            Dim sFichier_Modele As String = COMM_APP_WEB_GET_PARA(Label_CD_ARTI_ECO_V_CART.Text & StrDup(18 - Len(Label_CD_ARTI_ECO_V_CART.Text), " ") & "|Palette", "TextBox_FICH_MODE", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx")
            Select Case Right(sFichier_Modele, 3)
                Case "PDF", "pdf" 'Imprimer PDF
                    'vue fichier PDF palette
                    sQuery = "SELECT *
                                FROM [dbo].[" & dt_CFGR_ARTI_ECO(0)("Vue fichier PDF").ToString & "]
                               WHERE [NU_PALE] = '" & Label_NU_PALE.Text & "'"
                    dtLIST_DATA = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
                    If dtLIST_DATA Is Nothing Then
                        Dim sFichier As String = DIG_FACT_IMPR_PDF(sFichier_Modele,
                                                                       "", TextBox_NU_BL.Text, "Palette",
                                                                       "", "", "", Label_NB_CART_SCAN.Text,
                                                                       Label_NB_CART_SCAN.Text, Label_NU_PALE.Text, dtVar)
                        ClientScript.RegisterStartupScript([GetType](), "printPdf", "document.getElementById(""pdf"").src = """ & Path.GetFileName(sFichier) & """;
                                                                                     document.getElementById(""pdf"").onload = function() {window.frames[""pdf""].focus();
                                                                                                                                           window.frames[""pdf""].print();};", True)
                        sFichier_PDF = sFichier
                    Else
                        sFichier_PDF = "c:\sources\App_Web\PagesMembres\Digital_Factory\delivery_form_" & CInt(Int((10000000 * Rnd()) + 1)) & "_merge.pdf"
                        For iPDF = 0 To dtLIST_DATA.Rows.Count - 1 Step Convert.ToDecimal(dt_CFGR_ARTI_ECO(0)("Nombre de ligne dans le fichier PDF").ToString)
                            Dim sFichier As String = DIG_FACT_IMPR_PDF(sFichier_Modele,
                                          "", TextBox_NU_BL.Text, "Palette",
                                             "", "", "", Label_NB_CART_SCAN.Text,
                                             Label_NB_CART_SCAN.Text, Label_NU_PALE.Text, dtVar,
                                            dtLIST_DATA, iPDF)
                            sFichier_PDF = PDF_CCTN_FICH(sFichier_PDF, sFichier)
                        Next
                        ClientScript.RegisterStartupScript([GetType](), "printPdf", "document.getElementById(""pdf"").src = """ & Path.GetFileName(sFichier_PDF) & """;
                                                                                     document.getElementById(""pdf"").onload = function() {window.frames[""pdf""].focus();
                                                                                                                                           window.frames[""pdf""].print();};", True)
                        'File.Delete(sFichier_PDF)
                    End If
                    'Envoi de fichier par mail (e-takes care, forsee power, C43)
                    If Not dt_CFGR_ARTI_ECO(0)("Serveur SMTP mail") Is Nothing Then
                        Dim sEMET As String = "expedition.combree@eolane.com"
                        If Session("mail") <> "" Then sEMET = Session("mail")
                        COMM_APP_WEB_ENVO_MAIL(dt_CFGR_ARTI_ECO(0)("Serveur SMTP mail").ToString,
                                               sEMET,
                                               dt_CFGR_ARTI_ECO(0)("Destinataires mail").ToString,
                                               dt_CFGR_ARTI_ECO(0)("Sujet mail").ToString,
                                               dt_CFGR_ARTI_ECO(0)("Contenu mail").ToString,
                                               Session,
                                               dt_CFGR_ARTI_ECO(0)("Destinataires en copie mail").ToString,
                                               dt_CFGR_ARTI_ECO(0)("Destinataires en copie cachée mail").ToString,
                                               sFichier_PDF)
                    End If
                Case "PRN", "prn" 'imprimer une étiquette
                    DIG_FACT_IMPR_ETIQ(sFichier_Modele,
                                       "", TextBox_NU_BL.Text, "Palette", "", "", "", dt(0)("NB_NS").ToString, Label_NB_CART_SCAN.Text, dtVar)
            End Select

            'mettre à jour dt_expe
            sQuery = "with cte as (SELECT        PSG.[ID_PSG] as [1] , PSG.[DT_FIN] as [2] , LIVR.[ID_PSG] as [3]
				                     FROM (SELECT     [ID_PSG], [DT_FIN]
						                    FROM         [dbo].[DTM_PSG]) AS PSG, 
			 		                      (SELECT     [ID_PSG], [NU_BL]
						                    FROM         [dbo].[V_CLSG_PRD_DTM_HIST_LIVR]
						                    WHERE     ([NU_BL] = '" & TextBox_NU_BL.Text & "')) AS LIVR
				                     WHERE (PSG.[ID_PSG] = LIVR.[ID_PSG]))
	                    update cte set [2] = GETDATE()"
            SQL_REQ_ACT(sQuery, sChaineConnexion)
            TextBox_NU_BL.Text = ""
            TextBox_NU_CART.Text = ""
            Label_NU_PALE.Text = ""
            Label_NB_CART_SCAN.Text = ""
            GridView_NU_CART.DataSource = ""
            GridView_NU_CART.DataBind()
            Label_NM_CLIE_V_CART.Text = ""
            Label_CD_ARTI_ECO_V_CART.Text = ""
            Label_NM_DSGT_ARTI_V_CART.Text = ""
            MultiView_SAIS.SetActiveView(View_OF)
            GridView_REPE.DataSource = ""
            GridView_REPE.DataBind()
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try
    End Sub
    Protected Sub Button_SUPP_LIGN_V_CART_Click(sender As Object, e As ImageClickEventArgs) Handles Button_SUPP_LIGN_V_CART.Click

    End Sub
    Protected Sub Button_ANNU_SAIS_PALE_Click(sender As Object, e As ImageClickEventArgs)

    End Sub
    Protected Sub Button_SAI_CART_BL_Click(sender As Object, e As ImageClickEventArgs) Handles Button_SAI_CART_BL.Click
        MultiView_SAIS.SetActiveView(View_CART)
    End Sub
    Protected Sub Button_SAI_CART_BL_OF_Click(sender As Object, e As ImageClickEventArgs) Handles Button_SAI_CART_BL_OF.Click
        MultiView_SAIS.SetActiveView(View_CART)
    End Sub

    Protected Sub _AFCG_TCBL_SS_ENS()
        'MultiView_SAIS.SetActiveView(View_TCBL_SS_ENSE)
        MultiView_BASC_SAI_SEL.SetActiveView(View_SEL)
        Dim b_NU_SER_EOL = COMM_APP_WEB_GET_PARA(Label_CD_ARTI_ECO.Text & "(" & Label_NU_OP.Text & ")", "CheckBox_NU_SER_EOL", "ASP.pagesmembres_digital_factory_tcbl_esb_ss_esb_v2_aspx")
        If b_NU_SER_EOL = "" Or Nothing Then b_NU_SER_EOL = "False"
        Dim b_SAI_AUTO = COMM_APP_WEB_GET_PARA(Label_CD_ARTI_ECO.Text & "(" & Label_NU_OP.Text & ")", "CheckBox_SAI_AUTO", "ASP.pagesmembres_digital_factory_tcbl_esb_ss_esb_v2_aspx")
        If b_SAI_AUTO = "" Or Nothing Then b_SAI_AUTO = "True"
        Select Case "True"
            Case b_NU_SER_EOL
                Label_CD_SS_ENS.Text = ""
                MultiView_BASC_SAI_SEL.SetActiveView(View_SAI)
            Case b_SAI_AUTO
                MultiView_BASC_SAI_SEL.SetActiveView(View_SAI)
                GridView_REPE.SelectRow(0)
                Label_CD_SS_ENS.Text = GridView_REPE.SelectedRow.Cells(1).Text
        End Select
        TextBox_SS_ENS.Focus()
    End Sub

    Protected Sub TextBox_SS_ENS_TextChanged(sender As Object, e As EventArgs) Handles TextBox_SS_ENS.TextChanged

        'vérifier que le ns est disponible
        Dim sQuerySql As String = "SELECT   NM_NS_SENS
                                     FROM   dbo.DTM_TR_CPT
                                    WHERE   NM_NS_SENS = '" & TextBox_SS_ENS.Text & "'"
        Dim dt_TR_CPT, dtAFKO, dt_CFGR_ARTI_ECO, dtS034, dtNU_SSENS, dtLIST_DATA As New DataTable
        Dim sParam_Format_NS As String = "Format Numéro de série Eolane", s_FORM_NU_LOT As String = "", sFichier_PDF As String = "", sNU_ECO As String = "", sNU_CLIE As String = "", sFichier_Modele As String = "", sOF As String = ""
        Dim iID_Passage As Integer = 0
        Dim b_NU_SER_EOL, b_SAI_AUTO, b_SS_ENS_FERT As String, sQuery = ""
        Try
            b_NU_SER_EOL = COMM_APP_WEB_GET_PARA(Label_CD_ARTI_ECO.Text & "(" & Label_NU_OP.Text & ")", "CheckBox_NU_SER_EOL", "ASP.pagesmembres_digital_factory_tcbl_esb_ss_esb_v2_aspx")
            If b_NU_SER_EOL = "" Or Nothing Then b_NU_SER_EOL = "False"
            b_SAI_AUTO = COMM_APP_WEB_GET_PARA(Label_CD_ARTI_ECO.Text & "(" & Label_NU_OP.Text & ")", "CheckBox_SAI_AUTO", "ASP.pagesmembres_digital_factory_tcbl_esb_ss_esb_v2_aspx")
            If b_SAI_AUTO = "" Or Nothing Then b_SAI_AUTO = "True"
            b_SS_ENS_FERT = COMM_APP_WEB_GET_PARA(Label_CD_ARTI_ECO.Text & "(" & Label_NU_OP.Text & ")", "CheckBox_SS_ENS_FERT", "ASP.pagesmembres_digital_factory_tcbl_esb_ss_esb_v2_aspx")
            If b_SS_ENS_FERT = "" Or Nothing Then b_SS_ENS_FERT = "True"
            dt_CFGR_ARTI_ECO = DIG_FACT_SQL_CFGR_ARTI_ECO(Trim(Label_CD_ARTI_ECO.Text))

            dt_TR_CPT = SQL_SELE_TO_DT(sQuerySql, sChaineConnexion)
            If Not dt_TR_CPT Is Nothing Then Throw New Exception("Le numéro de série " & TextBox_SS_ENS.Text & " est déjà attribué dans la base.")

            MultiView_BASC_SAI_SEL.SetActiveView(View_SEL)

            'vérifier la cohérence ns ensemble/ns sous-ensemble (NeoMedLight)
            'pour néomedlight 
            'saisir les sous-ensembles puis allez chercher le n° d'ensemble
            If dt_CFGR_ARTI_ECO(0)("Vue numéro ensemble numéros sous-ensemble").ToString <> "" Then
                sQuery = Replace(dt_CFGR_ARTI_ECO(0)("Vue numéro ensemble numéros sous-ensemble").ToString, "#NU_SER_SOUS_ENSE#", TextBox_SS_ENS.Text)
                dtLIST_DATA = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
                If dtLIST_DATA Is Nothing Then Throw New Exception("Le numéro de série " & TextBox_SS_ENS.Text & " n'est pas associé dans la base")
                If dtLIST_DATA.Columns.Contains("NU_SER_ECO") Then
                    If TextBox_NU_SER_ECO.Text <> "" Then
                        If TextBox_NU_SER_ECO.Text <> dtLIST_DATA(0)("NU_SER_ECO").ToString Then Throw New Exception("Le numéro de série " & TextBox_SS_ENS.Text & " n'est pas associé au numéro " & TextBox_NU_SER_ECO.Text & " mais au numéro " & dtLIST_DATA(0)("NU_SER_ECO").ToString)
                    End If
                    TextBox_NU_SER_ECO.Text = dtLIST_DATA(0)("NU_SER_ECO").ToString
                End If
                If dtLIST_DATA.Columns.Contains("NU_SER_CLIE") Then
                    If TextBox_NU_SER_CLIE.Text <> "" Then
                        If TextBox_NU_SER_CLIE.Text <> dtLIST_DATA(0)("NU_SER_CLIE").ToString Then Throw New Exception("Le numéro de série " & TextBox_SS_ENS.Text & " n'est pas associé au numéro " & TextBox_NU_SER_CLIE.Text & " mais au numéro " & dtLIST_DATA(0)("NU_SER_CLIE").ToString)
                    End If
                    TextBox_NU_SER_CLIE.Text = dtLIST_DATA(0)("NU_SER_CLIE").ToString
                End If
            End If


            'si les sous-ensemble ont des numéros de série eolane
            If b_NU_SER_EOL = "True" Then
                MultiView_BASC_SAI_SEL.SetActiveView(View_SAI)
                TextBox_SS_ENS.Focus()
                'chercher le code article du sous-ensemble à partir de l'of contenu dans le numéro de série
                dtAFKO = SAP_DATA_READ_AFKO("AUFNR EQ '00000" & Left(TextBox_SS_ENS.Text, 7) & "'")
                If dtAFKO.Rows.Count = 0 Then Throw New Exception("le numéro de série saisi ne correspond à aucun code article ci-dessus")

                'verifier que le format du numéro de série est cohérent avec le format déclaré dans la base
                If DIG_FACT_VERI_FORM_NU_SER(Trim(dtAFKO(0)("PLNBEZ").ToString), "Format Numéro de série Eolane", TextBox_SS_ENS.Text) = False Then Throw New Exception("Le numéro de série " & TextBox_SS_ENS.Text & " ne correspond au format défini dans la base.")

                'attribuer le ns à une case
                For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                    If Trim(dtAFKO(0)("PLNBEZ").ToString) = rGridView_REPE.Cells(1).Text Then rGridView_REPE.Cells(3).Text = TextBox_SS_ENS.Text
                Next
            Else

                'vérifier par rapport à un format
                If b_SS_ENS_FERT = "False" Then sParam_Format_NS = "Format Numéro de série Fournisseur"
                s_FORM_NU_LOT = DIG_FACT_SQL_GET_PARA(Trim(Label_CD_SS_ENS.Text), "Format Numéro de lot")
                's'il existe un format de numéro de lot pour le code article donnée utiliser ce paramètre
                If Not s_FORM_NU_LOT Is Nothing Then sParam_Format_NS = "Format Numéro de lot"
                If DIG_FACT_VERI_FORM_NU_SER(Trim(Label_CD_SS_ENS.Text), sParam_Format_NS, TextBox_SS_ENS.Text) = False Then Throw New Exception("Le numéro de lot " & TextBox_SS_ENS.Text & " ne correspond au format défini dans la base.")

                'Vérifier que le numéro de lot existe
                If Not s_FORM_NU_LOT Is Nothing Then
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
                    If b_SAI_AUTO = "True" And b_NU_SER_EOL = "False" Then
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

            MultiView_BASC_SAI_SEL.SetActiveView(View_SAI_ENS)
            sNU_CLIE = TextBox_NU_SER_CLIE.Text
            sNU_ECO = TextBox_NU_SER_ECO.Text
            sOF = TextBox_NU_OF.Text
            _ERGT_CLSG(Label_NU_OF.Text, Label_NU_CART.Text, TextBox_NU_SER_ECO.Text, TextBox_NU_SER_CLIE.Text, TextBox_NU_CART_SCFQ.Text, Label_NB_CART.Text)
            Select Case "1"
                Case dt_CFGR_ARTI_ECO(0)("Carton spécifique").ToString
                    TextBox_NU_CART_SCFQ.Enabled = True
                    TextBox_NU_CART_SCFQ.Focus()
                Case dt_CFGR_ARTI_ECO(0)("Numéro de série Eolane").ToString
                    sQuerySql = "SELECT NM_NS_EOL, 
	                                    ISNULL(CASE WHEN [NM_NS_SENS] = '' OR [NM_NS_SENS] IS NULL THEN [CHARG] ELSE [NM_NS_SENS] END,[ID_CPT]) AS NM_NS_SENS
                                   FROM MES_Digital_Factory.[dbo].[ID_PF_View]
	                                    LEFT OUTER JOIN [SAP].[dbo].[MSEG] ON [ID_CPT] LIKE RTRIM(LTRIM([MBLNR]))+RTRIM(LTRIM([ZEILE]))+'%'
                                  WHERE NOT(ISNULL(CASE WHEN [NM_NS_SENS] = '' OR [NM_NS_SENS] IS NULL	THEN [CHARG] ELSE [NM_NS_SENS] END,[ID_CPT]) IS NULL)
	                                    AND ISNULL(CASE WHEN [NM_NS_SENS] = '' OR [NM_NS_SENS] IS NULL THEN [CHARG] ELSE [NM_NS_SENS] END,[ID_CPT]) <> '-'
	                                    AND ISNULL(CASE WHEN [NM_NS_SENS] = '' OR [NM_NS_SENS] IS NULL THEN [CHARG] ELSE [NM_NS_SENS] END,[ID_CPT]) <> 'Non renseigné'
	                                    AND [NM_NS_EOL] = '" & TextBox_NU_SER_ECO.Text & "'
                                 GROUP BY [NM_SAP_CPT], NM_NS_EOL, [ID_CPT], [NM_NS_SENS], [CHARG]
                                 ORDER BY [NM_SAP_CPT]"
                    TextBox_NU_SER_ECO.Enabled = True
                    TextBox_NU_SER_ECO.Focus()
                Case dt_CFGR_ARTI_ECO(0)("Numéro de série client").ToString
                    sQuerySql = "SELECT NM_NS_CLT, 
	                                    ISNULL(CASE WHEN [NM_NS_SENS] = '' OR [NM_NS_SENS] IS NULL THEN [CHARG] ELSE [NM_NS_SENS] END,[ID_CPT]) AS NM_NS_SENS
                                   FROM MES_Digital_Factory.[dbo].[ID_PF_View]
	                                    LEFT OUTER JOIN [SAP].[dbo].[MSEG] ON [ID_CPT] LIKE RTRIM(LTRIM([MBLNR]))+RTRIM(LTRIM([ZEILE]))+'%'
                                  WHERE NOT(ISNULL(CASE WHEN [NM_NS_SENS] = '' OR [NM_NS_SENS] IS NULL	THEN [CHARG] ELSE [NM_NS_SENS] END,[ID_CPT]) IS NULL)
	                                    AND ISNULL(CASE WHEN [NM_NS_SENS] = '' OR [NM_NS_SENS] IS NULL THEN [CHARG] ELSE [NM_NS_SENS] END,[ID_CPT]) <> '-'
	                                    AND ISNULL(CASE WHEN [NM_NS_SENS] = '' OR [NM_NS_SENS] IS NULL THEN [CHARG] ELSE [NM_NS_SENS] END,[ID_CPT]) <> 'Non renseigné'
	                                    AND [NM_NS_CLT] = '" & TextBox_NU_SER_CLIE.Text & "'
                                 GROUP BY [NM_SAP_CPT], NM_NS_EOL, [ID_CPT], [NM_NS_SENS], [CHARG]
                                 ORDER BY [NM_SAP_CPT]"
                    TextBox_NU_SER_CLIE.Enabled = True
                    TextBox_NU_SER_CLIE.Focus()
            End Select
            'si impression rapport traçabilité
            If dt_CFGR_ARTI_ECO(0)("Chemin fichier rapport de tracabilite").ToString <> "" Then
                dtNU_SSENS = SQL_SELE_TO_DT(sQuerySql, sChaineConnexion)
                If dtNU_SSENS Is Nothing Then Throw New Exception("Pas de résultat dans la base de données")
                sFichier_PDF = DIG_FACT_IMPR_PDF(dt_CFGR_ARTI_ECO(0)("Chemin fichier rapport de tracabilite").ToString, sOF, "0", "Carton", sNU_CLIE, sNU_ECO, "", "", "", "", dtNU_SSENS)
                ClientScript.RegisterStartupScript([GetType](), "printPdf", "document.getElementById(""pdf"").src = """ & Path.GetFileName(sFichier_PDF) & """;
                                                                             document.getElementById(""pdf"").onload = function() {window.frames[""pdf""].focus();
                                                                                                                                   window.frames[""pdf""].print();};", True)
            End If
            'vider la gridview
            For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                rGridView_REPE.Cells(3).Text = "&nbsp;"
            Next

        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            MultiView_BASC_SAI_SEL.SetActiveView(View_SAI)
            'Exit Sub
        Finally
            TextBox_SS_ENS.Text = ""
            TextBox_SS_ENS.Focus()
        End Try

    End Sub

    Protected Sub GridView_REPE_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GridView_REPE.SelectedIndexChanged
        Label_CD_SS_ENS.Text = GridView_REPE.SelectedRow.Cells(1).Text
        MultiView_BASC_SAI_SEL.SetActiveView(View_SAI)
        TextBox_SS_ENS.Focus()
    End Sub

    Protected Sub TextBox_NU_BL_V_PALE_TextChanged(sender As Object, e As EventArgs) Handles TextBox_NU_BL_V_PALE.TextChanged
        Dim dtMARA, dtT179T, dtLIPS, dtLIPSUP, dt, dt2 As New DataTable
        Dim sQuery As String = ""

        Try
            dtLIPSUP = SAP_DATA_READ_LIPSUP("VBELN EQ " & TextBox_NU_BL_V_PALE.Text)
            If dtLIPSUP Is Nothing Then Throw New Exception("Pas de données pour le BL n°" & TextBox_NU_BL_V_PALE.Text)
            'code article eolane
            Label_CD_ARTI_ECO_V_PALE.Text = Trim(dtLIPSUP(0)("MATNR").ToString)
            'désignation article
            Label_NM_DSGT_ARTI_V_PALE.Text = dtLIPSUP(0)("ARKTX").ToString
            dtMARA = SAP_DATA_READ_MARA("MATNR EQ '" & Label_CD_ARTI_ECO_V_PALE.Text & "'")
            dtT179T = SAP_DATA_READ_T179T("PRODH EQ '" & dtMARA(0)("PRDHA").ToString & "'")
            'client
            Label_NM_CLIE_V_PALE.Text = dtT179T(0)("VTEXT").ToString

            sQuery = "SELECT [NU_PALE]
                        FROM [dbo].[V_CLSG_PRD_DTM_HIST_LIVR]
                             INNER JOIN [SAP].[dbo].[AFKO] ON [AUFNR] LIKE '%' + CONVERT(NVARCHAR,[NU_OF])
                       WHERE [NU_BL] IS NULL AND NOT [NU_PALE] IS NULL AND RTRIM(LTRIM(PLNBEZ)) = '" & Label_CD_ARTI_ECO_V_PALE.Text & "'
                      GROUP BY [NU_PALE]"
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            If Not dt Is Nothing Then
                GridView_LIST_PALE_LIBR.DataSource = dt
                GridView_LIST_PALE_LIBR.DataBind()
            End If

            sQuery = "SELECT [NU_PALE]
                        FROM [dbo].[V_CLSG_PRD_DTM_HIST_LIVR]
                       WHERE [NU_BL] = '" & TextBox_NU_BL_V_PALE.Text & "'
                    GROUP BY [NU_PALE]"
            dt2 = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            If Not dt2 Is Nothing Then
                GridView_LIST_PALE_ENTR.DataSource = dt2
                GridView_LIST_PALE_ENTR.DataBind()
            End If

            TextBox_NU_PALE.Enabled = True
            TextBox_NU_PALE.Focus()
        Catch ex As Exception
            TextBox_NU_BL_V_PALE.Focus()

            'MultiView_SAIS.SetActiveView(View_OF)
            LOG_Erreur(GetCurrentMethod, ex.Message)
            TextBox_NU_BL_V_PALE.Text = ""
        End Try
    End Sub

    Protected Sub TextBox_NU_PALE_TextChanged(sender As Object, e As EventArgs) Handles TextBox_NU_PALE.TextChanged
        Dim sQuery As String = "", dt, dt2, dt3, dtAFKO As New DataTable
        Dim sData As String = "", dtLIPS, dtVar, dtLIST_DATA, dt_CFGR_ARTI_ECO As New DataTable, sFichier_PDF As String

        Try
            dt_CFGR_ARTI_ECO = DIG_FACT_SQL_CFGR_ARTI_ECO(Trim(Label_CD_ARTI_ECO_V_PALE.Text))

            'vérification existance
            sQuery = "SELECT [NU_PALE]                          
                        FROM [dbo].[V_CLSG_PRD_DTM_HIST_LIVR]
                       WHERE [NU_PALE] = '" & TextBox_NU_PALE.Text & "'"
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            If dt Is Nothing Then Throw New Exception("Le numéro de palette " & TextBox_NU_PALE.Text & " n'existe pas.")
            'vérification doublon
            sQuery = "SELECT [NU_PALE]
                            ,[NU_BL]
                        FROM [dbo].[V_CLSG_PRD_DTM_HIST_LIVR]
                       WHERE [NU_CART] = '" & TextBox_NU_PALE.Text & "' AND [NU_BL] = '" & TextBox_NU_BL_V_PALE.Text & "'"
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            If Not dt Is Nothing Then Throw New Exception("Le numéro de palette " & TextBox_NU_PALE.Text & " a déjà été scanné.")
            'vérification cohérence cd article
            sQuery = "SELECT [NU_OF]                          
                        FROM [dbo].[V_CLSG_PRD_DTM_HIST_LIVR]
                       WHERE [NU_PALE] = '" & TextBox_NU_PALE.Text & "'
                      GROUP BY [NU_OF]"
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            dtAFKO = SAP_DATA_READ_AFKO("AUFNR LIKE '%" & dt(0)("NU_OF").ToString & "'")
            If Trim(dtAFKO(0)("PLNBEZ").ToString) <> Label_CD_ARTI_ECO_V_PALE.Text Then Throw New Exception("Le code article " & Trim(dtAFKO(0)("PLNBEZ").ToString) & " qui a été trouvé dans la base ne correspond pas au code article associé au BL " & TextBox_NU_BL_V_PALE.Text & " (" & Label_CD_ARTI_ECO_V_PALE.Text & ")")
            'enregistrement
            sQuery = "UPDATE [dbo].[DTM_CLSG_PALE]
                         SET [NU_BL] = '" & TextBox_NU_BL_V_PALE.Text & "'
                       WHERE [NU_PALE] = '" & TextBox_NU_PALE.Text & "'"
            SQL_REQ_ACT(sQuery, sChaineConnexion)

            'For Each cell As TableCell In GridView_NU_CART.HeaderRow.Cells
            '    dtVar.Columns.Add(cell.Text)
            'Next
            'For Each row As GridViewRow In GridView_NU_CART.Rows
            '    dtVar.Rows.Add()
            '    For i As Integer = 0 To row.Cells.Count - 1
            '        dtVar.Rows(row.RowIndex)(i) = row.Cells(i).Text
            '    Next
            'Next
            'Dim sARTI_ECO As String =
            Dim sFichier_Modele As String = COMM_APP_WEB_GET_PARA(Label_CD_ARTI_ECO_V_PALE.Text & StrDup(18 - Len(Label_CD_ARTI_ECO_V_PALE.Text), " ") & "|Palette", "TextBox_FICH_MODE", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx")
            Select Case Right(sFichier_Modele, 3)
                Case "PDF", "pdf" 'Imprimer PDF
                    sQuery = "SELECT *
                                FROM [dbo].[" & dt_CFGR_ARTI_ECO(0)("Vue fichier PDF").ToString & "]
                               WHERE [NU_PALE] = '" & TextBox_NU_PALE.Text & "'"
                    dtLIST_DATA = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
                    If dtLIST_DATA Is Nothing Then
                        Dim sFichier As String = DIG_FACT_IMPR_PDF(sFichier_Modele,
                                                                       "", TextBox_NU_BL_V_PALE.Text, "Palette",
                                                                       "", "", "", Label_NB_CART_SCAN.Text,
                                                                       Label_NB_CART_SCAN.Text, TextBox_NU_PALE.Text, dtVar)
                        ClientScript.RegisterStartupScript([GetType](), "printPdf", "document.getElementById(""pdf"").src = """ & Path.GetFileName(sFichier) & """;
                                                                                     document.getElementById(""pdf"").onload = function() {window.frames[""pdf""].focus();
                                                                                                                                           window.frames[""pdf""].print();};", True)
                        sFichier_PDF = sFichier
                    Else
                        sFichier_PDF = "c:\sources\App_Web\PagesMembres\Digital_Factory\delivery_form_" & CInt(Int((10000000 * Rnd()) + 1)) & "_merge.pdf"
                        For iPDF = 0 To dtLIST_DATA.Rows.Count - 1 Step Convert.ToDecimal(dt_CFGR_ARTI_ECO(0)("Nombre de ligne dans le fichier PDF").ToString)
                            Dim sFichier As String = DIG_FACT_IMPR_PDF(sFichier_Modele,
                                          "", TextBox_NU_BL_V_PALE.Text, "Palette",
                                             "", "", "", Label_NB_CART_SCAN.Text,
                                             Label_NB_CART_SCAN.Text, TextBox_NU_PALE.Text, dtVar,
                                            dtLIST_DATA, iPDF)
                            sFichier_PDF = PDF_CCTN_FICH(sFichier_PDF, sFichier)
                        Next
                        ClientScript.RegisterStartupScript([GetType](), "printPdf", "document.getElementById(""pdf"").src = """ & Path.GetFileName(sFichier_PDF) & """;
                                                                                     document.getElementById(""pdf"").onload = function() {window.frames[""pdf""].focus();
                                                                                                                                           window.frames[""pdf""].print();};", True)
                        'File.Delete(sFichier_PDF)
                    End If
                    'Envoi de fichier par mail (e-takes care, forsee power, C43)
                    If Not dt_CFGR_ARTI_ECO(0)("Serveur SMTP mail") Is Nothing Then
                        Dim sEMET As String = "expedition.combree@eolane.com"
                        If Session("mail") <> "" Then sEMET = Session("mail")
                        COMM_APP_WEB_ENVO_MAIL(dt_CFGR_ARTI_ECO(0)("Serveur SMTP mail").ToString,
                                               sEMET,
                                               dt_CFGR_ARTI_ECO(0)("Destinataires mail").ToString,
                                               dt_CFGR_ARTI_ECO(0)("Sujet mail").ToString,
                                               dt_CFGR_ARTI_ECO(0)("Contenu mail").ToString,
                                               Session,
                                               dt_CFGR_ARTI_ECO(0)("Destinataires en copie mail").ToString,
                                               dt_CFGR_ARTI_ECO(0)("Destinataires en copie cachée mail").ToString,
                                               sFichier_PDF)
                    End If
                Case "PRN", "prn" 'imprimer une étiquette
                    DIG_FACT_IMPR_ETIQ(sFichier_Modele,
                                       "", TextBox_NU_BL_V_PALE.Text, "Palette", "", "", "", dt(0)("NB_NS").ToString, Label_NB_CART_SCAN.Text, dtVar)
            End Select

            'mettre à jour dt_expe
            sQuery = "with cte as (SELECT        PSG.[ID_PSG] as [1] , PSG.[DT_FIN] as [2] , LIVR.[ID_PSG] as [3]
				                     FROM (SELECT     [ID_PSG], [DT_FIN]
						                    FROM         [dbo].[DTM_PSG]) AS PSG, 
			 		                      (SELECT     [ID_PSG], [NU_BL]
						                    FROM         [dbo].[V_CLSG_PRD_DTM_HIST_LIVR]
						                    WHERE     ([NU_BL] = '" & TextBox_NU_BL_V_PALE.Text & "')) AS LIVR
				                     WHERE (PSG.[ID_PSG] = LIVR.[ID_PSG]))
	                    update cte set [2] = GETDATE()"
            SQL_REQ_ACT(sQuery, sChaineConnexion)

            sQuery = "SELECT [NU_PALE] AS [Numéro de palette]
                        FROM [dbo].[V_CLSG_PRD_DTM_HIST_LIVR]
                             INNER JOIN [SAP].[dbo].[AFKO] ON [AUFNR] LIKE '%' + CONVERT(NVARCHAR,[NU_OF])
                       WHERE [NU_BL] IS NULL AND NOT [NU_PALE] IS NULL AND RTRIM(LTRIM(PLNBEZ)) = '" & Label_CD_ARTI_ECO_V_PALE.Text & "'
                      GROUP BY [NU_PALE]"
            dt3 = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            If Not dt3 Is Nothing Then
                GridView_LIST_PALE_LIBR.DataSource = dt3
                GridView_LIST_PALE_LIBR.DataBind()
            End If

            sQuery = "SELECT [NU_PALE] AS [Numéro de palette]
                        FROM [dbo].[V_CLSG_PRD_DTM_HIST_LIVR]
                       WHERE [NU_BL] = '" & TextBox_NU_BL_V_PALE.Text & "'
                    GROUP BY [NU_PALE]"
            dt2 = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            If Not dt2 Is Nothing Then
                GridView_LIST_PALE_ENTR.DataSource = dt2
                GridView_LIST_PALE_ENTR.DataBind()
            End If
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try
        TextBox_NU_PALE.Focus()
        TextBox_NU_PALE.Text = ""
        '-- Dim sender As New Object, e As New EventArgs
        'Button_CLOR_LIVR_Click(sender, e)
    End Sub

    Protected Sub Button_CLOR_LIVR_Click(sender As Object, e As ImageClickEventArgs) Handles Button_CLOR_LIVR.Click
        MultiView_SAIS.SetActiveView(View_OF)
        TextBox_NU_PALE.Text = ""
        TextBox_NU_BL.Text = ""
        Label_CD_ARTI_ECO_V_PALE.Text = ""
        Label_NM_DSGT_ARTI_V_PALE.Text = ""
        Label_NM_CLIE_V_PALE.Text = ""
    End Sub

    Protected Sub Button_SAI_PALE_BL_Click(sender As Object, e As EventArgs) Handles Button_SAI_PALE_BL.Click
        MultiView_SAIS.SetActiveView(View_PALE)
    End Sub

    Protected Sub ImageButton_CLOT_PALE_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton_CLOT_PALE.Click

        Dim sQuery As String = ""
        Dim dt, dt_CFGR_ARTI_ECO As New DataTable
        Try
            'blabla
            dt_CFGR_ARTI_ECO = DIG_FACT_SQL_CFGR_ARTI_ECO(Trim(Label_CD_ARTI_ECO.Text))

            If Not (Label_NB_CART.Text = "" Or Label_NB_CART.Text = "0") Then Button_CLOR_CART_Click(sender, e)
            sQuery = "SELECT ISNULL(MAX([NU_PALE])," & Label_NU_OF.Text & "00) + 1 AS NEW_NU_PALE
                            FROM (
                                    SELECT [NU_PALE], [NU_OF]
                                      FROM [dbo].[V_CLSG_PRD_DTM_HIST_LIVR]
                                     WHERE NU_OF = '" & Label_NU_OF.Text & "'
                                 ) AS A"
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            Label_NU_PALE_NU_V_NU_SER.Text = dt(0)("NEW_NU_PALE").ToString
            'RAZ Affichage
            TextBox_NU_CART_SCFQ.Enabled = False
            TextBox_NU_CART_SCFQ.Text = ""
            TextBox_NU_SER_ECO.Enabled = False
            TextBox_NU_SER_ECO.Text = ""
            TextBox_NU_SER_CLIE.Enabled = False
            TextBox_NU_SER_CLIE.Text = ""
            Select Case "1"
                Case dt_CFGR_ARTI_ECO(0)("Carton spécifique").ToString
                    TextBox_NU_CART_SCFQ.Enabled = True
                    TextBox_NU_CART_SCFQ.Focus()
                Case dt_CFGR_ARTI_ECO(0)("Numéro de série Eolane").ToString
                    TextBox_NU_SER_ECO.Enabled = True
                    TextBox_NU_SER_ECO.Focus()
                Case dt_CFGR_ARTI_ECO(0)("Numéro de série client").ToString
                    TextBox_NU_SER_CLIE.Enabled = True
                    TextBox_NU_SER_CLIE.Focus()
            End Select
            If dt_CFGR_ARTI_ECO(0)("Vue numéro ensemble numéros sous-ensemble").ToString <> "" Then _AFCG_TCBL_SS_ENS()
            GridView_LIST_CART.DataSource = ""
            GridView_LIST_CART.DataBind()
            Label_NB_CART_PALE.Text = "0"
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try

    End Sub

End Class