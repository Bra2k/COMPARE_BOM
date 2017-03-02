Imports App_Web.Class_SAP_DATA
Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports App_Web.Class_SQL
Imports App_Web.Class_DIG_FACT
Imports App_Web.Class_DIG_FACT_SQL
Imports App_Web.Class_COMM_APP_WEB
Public Class TCBL_OPRT
    Inherits System.Web.UI.Page
    Dim sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=" & Replace(Replace(My.Computer.Name, "CEDB03", "MES_Digital_Factory_DEV"), "CEAPP03", "MES_Digital_Factory") & ";Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=7200;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
    Dim sQuery_Test As String = "INSERT INTO [dbo].[DTM_TEST_FONC_VAL]([ID_EXEC],[ID_TEST],[NB_VAL]) VALUES "
    ' Dim sNU_SER_IMPR As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Protected Sub TextBox_OF_TextChanged(sender As Object, e As EventArgs) Handles TextBox_OF.TextChanged
        Dim dtAFKO, dtAFVC, dt_op As New DataTable

        Try
            'extraction données de l'OF
            dtAFKO = SAP_DATA_READ_AFKO("AUFNR LIKE '%" & TextBox_OF.Text & "'")
            If dtAFKO Is Nothing Then Throw New Exception("L'OF n°" & TextBox_OF.Text & " n'a pas été trouvé dans SAP.")
            'Extraction de la gamme
            With dt_op.Columns
                .Add("OP", Type.GetType("System.String"))
                .Add("Désignation", Type.GetType("System.String"))
            End With
            dtAFVC = SAP_DATA_READ_AFVC("AUFPL EQ '" & dtAFKO(0)("AUFPL").ToString & "'")
            For Each rdtAFVC As DataRow In dtAFVC.Select(Nothing, "VORNR")
                With dt_op
                    .Rows.Add()
                    .Rows(.Rows.Count - 1)("OP") = rdtAFVC("VORNR").ToString
                    .Rows(.Rows.Count - 1)("Désignation") = rdtAFVC("LTXA1").ToString & " (OP:" & (Convert.ToDecimal(rdtAFVC("VORNR").ToString)).ToString & ")"
                End With
            Next
            DropDownList_OP.DataSource = dt_op
            DropDownList_OP.DataTextField = "Désignation" '"VORNR"
            DropDownList_OP.DataValueField = "OP"
            DropDownList_OP.DataBind()

        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try
    End Sub

    Protected Sub Button_VALI_ENTER_Click(sender As Object, e As EventArgs) Handles Button_VALI_ENTER.Click
        Dim sQuery As String = "", sNS As String = ""
        Dim dt, dtAFKO, dtAFVC, dtVRESB, dtSTPO, dtSTPU, dt_POST, dt_LIST_NU_SER_TRAC, dt_SS_ENS, dt_CFGR_ARTI_ECO, dt_ETAT_CTRL As New DataTable
        Dim rdt_CD_ARTI_ENS_SENS_SAP As DataRow
        Dim i_COUN_VRFC_PROD_SS_ENS As Integer
        Try
            'dt = SAP_DATA_Z_ORDOPEINFO_GET(TextBox_OF.Text, DropDownList_OP.SelectedValue)
            dt = SAP_DATA_LECT_OF(TextBox_OF.Text)
            Label_OF.Text = TextBox_OF.Text
            Label_CD_ARTI.Text = Trim(dt.Rows(0)("CD_ARTI_ECO").ToString)
            Label_CLIE.Text = Trim(dt.Rows(0)("NM_CLIE").ToString)
            Label_DES_ARTI.Text = Trim(dt.Rows(0)("NM_DSGT_ARTI").ToString)
            Label_QT_OF.Text = Trim(dt.Rows(0)("QT_OF").ToString)

            'extraction données de l'OF
            dtAFKO = SAP_DATA_READ_AFKO("AUFNR LIKE '%" & TextBox_OF.Text & "'")
            If dtAFKO Is Nothing Then Throw New Exception("L'OF n°" & TextBox_OF.Text & " n'a pas été trouvé dans SAP.")
            'Extraction de la gamme
            dtAFVC = SAP_DATA_READ_AFVC("AUFPL EQ '" & dtAFKO(0)("AUFPL").ToString & "' AND VORNR EQ '" & DropDownList_OP.SelectedValue & "'")
            Label_DES_OP.Text = Trim(dtAFVC(0)("LTXA1").ToString)
            Label_OP.Text = Convert.ToDecimal(DropDownList_OP.SelectedValue).ToString

            'charger les ns entrés
            sQuery = "SELECT [NU_SER_CLIE] AS [Numéro de série client]
                            ,[NU_SER_ECO] AS [Numéro de série Eolane]
                        FROM [dbo].[V_DER_PASS_BON]
                       WHERE [NM_OF] = '" & Label_OF.Text & "'
                         AND [LB_ETP] = '" & Label_DES_OP.Text & " (OP:" & Label_OP.Text & ")'"
            dt_LIST_NU_SER_TRAC = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            If Not dt_LIST_NU_SER_TRAC Is Nothing Then
                'décrément quantité totale 
                Label_QT_REST_OF.Text = (Convert.ToDecimal(Replace(Label_QT_OF.Text, ".", ",")) - dt_LIST_NU_SER_TRAC.Rows.Count).ToString
                If Convert.ToDecimal(Replace(Label_QT_OF.Text, ".", ",")) - dt_LIST_NU_SER_TRAC.Rows.Count = 0 Then 'OF terminé
                    TextBox_OF.Text = ""
                    Label_OF.Text = ""
                    Label_CD_ARTI.Text = ""
                    Label_CLIE.Text = ""
                    Label_DES_ARTI.Text = ""
                    Label_QT_OF.Text = ""
                    Label_DES_OP.Text = ""
                    Label_OP.Text = ""
                    Throw New Exception("L'OF est terminé.")
                    Exit Sub
                End If
                'affichage de la liste des NS tracés
                GridView_SN_TRAC.DataSource = dt_LIST_NU_SER_TRAC
                GridView_SN_TRAC.DataBind()
            End If

            'extraction de la config dédiée du poste
            sQuery = "SELECT [ID_POST]
                        FROM [dbo].[V_POST_HSRQ]
                       WHERE [CD_ARTI_ECO_POST] = '" & Label_CD_ARTI.Text & "' AND 
                             [NU_OP_POST] = '" & Label_OP.Text & "'
                      GROUP BY [ID_POST], [CD_ARTI_ECO_POST], [NU_OP_POST]"
            dt_POST = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            If dt_POST.Rows.Count > 1 Then
                DropDownList_POST.DataSource = dt_POST
                DropDownList_POST.DataTextField = "ID_POST"
                DropDownList_POST.DataValueField = "ID_POST"
                DropDownList_POST.DataBind()
                MultiView_SAIS_OPRT.SetActiveView(View_SAIS_OTLG)
                MultiView_POST.SetActiveView(View_SAIS_POST_MULT)
                Exit Sub
            End If
            Label_POST.Text = dt_POST(0)("ID_POST").ToString

            'affiche config dédiée + extraction nomenclature type pour matériel générique
            If _AFCG_POST_NMCT_TPLG() = "1" Then Exit Sub

            'Extraire la liste des étapes de l'opération
            Session("dt_ETAP") = _ETCT_ETAP_OPRT()

            MultiView_SAIS_OPRT.SetActiveView(View_SAIS_NU_SER_CHEC)

            'Si la TextBox de génération des étiquettes est cochée
            dt_CFGR_ARTI_ECO = DIG_FACT_SQL_CFGR_ARTI_ECO(Trim(Label_CD_ARTI.Text))
            If dt_CFGR_ARTI_ECO(0)("Génération impression numéro de série").ToString = Label_OP.Text Then
                Select Case "1"
                    Case dt_CFGR_ARTI_ECO(0)("Numéro de série Eolane").ToString
                        dt_ETAT_CTRL = COMM_APP_WEB_ETAT_CTRL(Trim(Label_CD_ARTI.Text) & "|Numéro de série Eolane", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx")
                        If dt_ETAT_CTRL Is Nothing Then Throw New Exception("La base App_Web_Eco n'as pas été configurée pour l'article " & Trim(Label_CD_ARTI.Text))
                        sNS = Label_OF.Text & DIG_FACT_SQL_GENE_NU_SER_CLIE("10", "1", "%%%%%%", Label_OF.Text, "Numéro de série Eolane", False,
                                                      Session("matricule"), Label_OF.Text)
                        If dt_ETAT_CTRL.Columns.Contains("TextBox_FICH_MODE") Then
                            DIG_FACT_IMPR_ETIQ(dt_ETAT_CTRL(0)("TextBox_FICH_MODE").ToString,
                                               TextBox_OF.Text, "", "Numéro de série Eolane", "", sNS, "", "", "", Nothing)
                        End If
                    Case dt_CFGR_ARTI_ECO(0)("Numéro de série client").ToString
                        Dim sFORM_NU_CLIE As String = ""
                        dt_ETAT_CTRL = COMM_APP_WEB_ETAT_CTRL(Trim(Label_CD_ARTI.Text) & "|Numéro de série client", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx")
                        If dt_ETAT_CTRL Is Nothing Then Throw New Exception("La base App_Web_Eco n'as pas été configurée pour l'article " & Trim(Label_CD_ARTI.Text))
                        If dt_ETAT_CTRL.Columns.Contains("TextBox_FORM_NU_CLIE") = True Then sFORM_NU_CLIE = dt_ETAT_CTRL(0)("TextBox_FORM_NU_CLIE").ToString
                        sNS = DIG_FACT_SQL_GENE_NU_SER_CLIE(dt_CFGR_ARTI_ECO(0)("Encodage Numéro de série client").ToString,
                                                            dt_CFGR_ARTI_ECO(0)("Incrémentation flanc").ToString,
                                                            sFORM_NU_CLIE,
                                                            Label_CD_ARTI.Text,
                                                            "Numéro de série client",
                                                            False,
                                                            Session("matricule"),
                                                            Label_OF.Text)
                        If dt_ETAT_CTRL.Columns.Contains("TextBox_FICH_MODE") Then
                            DIG_FACT_IMPR_ETIQ(dt_ETAT_CTRL(0)("TextBox_FICH_MODE").ToString,
                                               TextBox_OF.Text, "", "Numéro de série client", sNS, "", "", "", "", Nothing)
                        End If
                End Select
                Session("sNU_SER_IMPR") = sNS
            End If

            'vérification si traçabilité composant à faire
            Label_RES.Text = ""
            dt_SS_ENS = App_Web.TCBL_ESB_SS_ESB_V2._LIST_ENS_SS_ENS(TextBox_OF.Text, DropDownList_OP.SelectedValue)
            If Not dt_SS_ENS Is Nothing Then 'traçabilité composant à faire
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
                        i_COUN_VRFC_PROD_SS_ENS += 1
                    End If
                Next
                GridView_REPE.DataSource = dt_SS_ENS
                GridView_REPE.DataBind()
                If GridView_REPE.Rows.Count <> i_COUN_VRFC_PROD_SS_ENS Then 's'il y a des sous-ensemble de type différent de produit ou semi-fini alors saisir les codes lot
                    MultiView_SAIS_OPRT.SetActiveView(View_SAIS_TCBL_COMP)
                    MultiView_Tracabilité.SetActiveView(View_CONT_LOT_ID_COMP)
                End If
            End If

        Catch ex As Exception When dt_POST Is Nothing
            LOG_Erreur(GetCurrentMethod, "Pas de poste configuré dans la base, prévenir un méthode")
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try
    End Sub

    Protected Sub Button_VALI_ENTER_OTLG_Click(sender As Object, e As EventArgs) Handles Button_VALI_ENTER_OTLG.Click

        Dim sQuery As String = ""
        Try
            'vérification qu'il existe (FER-M-0551)

            'vérification si attribué

            'enregistrement dans table
            For Each rGridView_LIST_MTRE As GridViewRow In GridView_LIST_MTRE.Rows
                If rGridView_LIST_MTRE.Cells(0).Text = Label_TYPE_MTRE.Text Then rGridView_LIST_MTRE.Cells(1).Text = TextBox_MTRE_GNRQ.Text
                If rGridView_LIST_MTRE.Cells(1).Text = "&nbsp;" Then
                    Label_TYPE_MTRE.Text = rGridView_LIST_MTRE.Cells(0).Text
                    TextBox_MTRE_GNRQ.Focus()
                    Exit Sub
                End If
            Next

            'enregistrer
            For Each rGridView_LIST_MTRE As GridViewRow In GridView_LIST_MTRE.Rows
                LOG_Msg(GetCurrentMethod, rGridView_LIST_MTRE.Cells(2).Text)
                If rGridView_LIST_MTRE.Cells(2).Text = "G&#233;n&#233;rique" Then
                    sQuery = "INSERT INTO [dbo].[DTM_REF_PARA]([NM_CRIT],[NM_PARA],[VAL_PARA],[DT_PARA])
                                   VALUES ('" & Label_POST.Text & "','" & rGridView_LIST_MTRE.Cells(1).Text & "','1',GETDATE())"
                    SQL_REQ_ACT(sQuery, sChaineConnexion)
                End If
            Next

            MultiView_SAIS_OPRT.SetActiveView(View_SAIS_NU_SER_CHEC)

        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try

    End Sub

    Protected Sub TextBox_NU_SER_TextChanged(sender As Object, e As EventArgs) Handles TextBox_NU_SER.TextChanged

        Dim dt_ETAP As DataTable = Session("dt_ETAP")
        Session("i_NU_ETAP") = 0
        Session("sQuery_Etap_Test") = ""
        Dim dt, dt_WF As New DataTable, sQuery As String = "", sQuery_WF As String = ""

        Dim sNU_SER_ECO As String = "", sNU_SER_CLIE As String = ""
        Dim dt_PARA, dt_CFGR_ARTI_ECO, dt_ETAT_CTRL As New DataTable
        Try
            Label_RES.Text = ""
            dt_CFGR_ARTI_ECO = DIG_FACT_SQL_CFGR_ARTI_ECO(Trim(Label_CD_ARTI.Text))
            'Si la TextBox de génération des étiquettes est cochée
            If dt_CFGR_ARTI_ECO(0)("Génération impression numéro de série").ToString = Label_OP.Text Then
                If TextBox_NU_SER.Text = "Réimprime-moi le dernier numéro de série" Then 'Réimprimer le dernier numéro de série
                    Select Case "1"
                        Case dt_CFGR_ARTI_ECO(0)("Numéro de série Eolane").ToString
                            dt_ETAT_CTRL = COMM_APP_WEB_ETAT_CTRL(Trim(Label_CD_ARTI.Text) & "|Numéro de série Eolane", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx")
                            If dt_ETAT_CTRL Is Nothing Then Throw New Exception("La base App_Web_Eco n'as pas été configurée pour l'article " & Trim(Label_CD_ARTI.Text))
                            If dt_ETAT_CTRL.Columns.Contains("TextBox_FICH_MODE") Then
                                DIG_FACT_IMPR_ETIQ(dt_ETAT_CTRL(0)("TextBox_FICH_MODE").ToString,
                                                   TextBox_OF.Text, "", "Numéro de série Eolane", "", Session("sNU_SER_IMPR"), "", "", "", Nothing)
                            End If
                        Case dt_CFGR_ARTI_ECO(0)("Numéro de série client").ToString
                            dt_ETAT_CTRL = COMM_APP_WEB_ETAT_CTRL(Trim(Label_CD_ARTI.Text) & "|Numéro de série client", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx")
                            If dt_ETAT_CTRL Is Nothing Then Throw New Exception("La base App_Web_Eco n'as pas été configurée pour l'article " & Trim(Label_CD_ARTI.Text))
                            If dt_ETAT_CTRL.Columns.Contains("TextBox_FICH_MODE") Then
                                DIG_FACT_IMPR_ETIQ(dt_ETAT_CTRL(0)("TextBox_FICH_MODE").ToString,
                                                   TextBox_OF.Text, "", "Numéro de série client", Session("sNU_SER_IMPR"), "", "", "", "", Nothing)
                            End If
                    End Select
                    Exit Sub
                End If
                If Session("sNU_SER_IMPR") <> TextBox_NU_SER.Text Then Throw New Exception("Le numéro de série " & TextBox_NU_SER.Text & " n'est pas identique au dernier numéro de série imprimé " & Session("sNU_SER_IMPR") & ".")
            End If
            Select Case "1"
                Case dt_CFGR_ARTI_ECO(0)("Numéro de série Eolane").ToString
                    sNU_SER_ECO = TextBox_NU_SER.Text
                    'vérifier par rapport à un format
                    If DIG_FACT_VERI_FORM_NU_SER(Trim(Label_CD_ARTI.Text), "Format Numéro de série Eolane", TextBox_NU_SER.Text) = False Then Throw New Exception("Le numéro de série " & TextBox_NU_SER.Text & " ne correspond au format défini dans la base.")
                Case dt_CFGR_ARTI_ECO(0)("Numéro de série client").ToString
                    sNU_SER_CLIE = TextBox_NU_SER.Text
                    'vérifier par rapport à un format
                    If DIG_FACT_VERI_FORM_NU_SER(Trim(Label_CD_ARTI.Text), "Format Numéro de série client", TextBox_NU_SER.Text) = False Then Throw New Exception("Le numéro de série " & TextBox_NU_SER.Text & " ne correspond au format défini dans la base.")
            End Select

            'vérification que le ns n'est pas déjà associé ou passé !!! ATTENTION !!!
            sQuery = "SELECT [NM_NS_EOL]
                        FROM [dbo].[ID_PF_View]
                       WHERE (NM_NS_EOL LIKE '%" & sNU_SER_ECO & "%' AND [NM_NS_CLT] LIKE '%" & sNU_SER_CLIE & "%') AND ([LB_ETP] = '" & Label_DES_OP.Text & " (OP:" & Label_OP.Text & ")')"
            dt_PARA = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            If Not dt_PARA Is Nothing Then Throw New Exception("Le numéro de série " & TextBox_NU_SER.Text & " est déjà attribué dans la base.")

            'Création de l'ID_PSG
            sQuery = "SELECT [NEW_ID_PSG], GETDATE() AS DT_DEB
                        FROM [dbo].[V_NEW_ID_PSG_DTM_PSG]"
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            Session("ID_PSG") = dt(0)("NEW_ID_PSG").ToString
            Session("DT_DEB") = dt(0)("DT_DEB").ToString

            'vérification workflow
            sQuery = "SELECT REPLACE(NM_PARA, 'WORKFLOW étape ', '') AS NU_ETAP
                        FROM dbo.V_DER_DTM_REF_PARA
                       WHERE NM_PARA LIKE N'WORKFLOW étape%'
                         AND CD_ARTI = '" & Label_CD_ARTI.Text & "'
                         AND VAL_PARA = '" & Label_DES_OP.Text & " (OP:" & Label_OP.Text & ")' "
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            If dt Is Nothing Then
                Throw New Exception("Pas de Workflow configuré dans la base. Prévenir un Méthode")
            Else
                If dt(0)("NU_ETAP").ToString <> "1" Then 'Première opération dans le workflow --> pas de vérification
                    sQuery_WF = "SELECT [NM_ETAP_PCDT]      
                                   FROM [dbo].[V_WORK_PASS]
                                  WHERE [NM_ETAP] = '" & Label_DES_OP.Text & " (OP:" & Label_OP.Text & ")' 
                                    AND [NM_OF] = '" & TextBox_OF.Text & "' 
                                    AND [NU_SER_ECO] LIKE '%" & sNU_SER_ECO & "%'
                                    AND [NU_SER_CLIE] LIKE '%" & sNU_SER_CLIE & "%'"
                    dt_WF = SQL_SELE_TO_DT(sQuery_WF, sChaineConnexion)
                    If dt_WF Is Nothing Then
                        _ERGT_OPRT_PASS("F")
                        Throw New Exception("Le numéro de série " & TextBox_NU_SER.Text & " n'est pas passé à l'étape précédente")
                    End If
                End If
            End If


            TextBox_NU_SER.Enabled = False
            'LOG_Msg(GetCurrentMethod, dt_ETAP.Rows.Count)
            If Not dt_ETAP Is Nothing Then 'étape n° 1
                _AFCG_ETAP(0)
                MultiView_ETAP.SetActiveView(View_CHEC_ETAP)
                Exit Sub
            Else ' ou passer à la saisie des numéros de série des sous-ensemble
                For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                    If rGridView_REPE.Cells(3).Text = "PRODUIT" Or rGridView_REPE.Cells(3).Text = "PRODUIT SEMI-FINI" Then
                        Label_CD_SS_ENS.Text = rGridView_REPE.Cells(1).Text
                        MultiView_SAIS_OPRT.SetActiveView(View_SAIS_TCBL_COMP)
                        MultiView_Tracabilité.SetActiveView(View_SAI_SOUS_ENSE)
                        TextBox_SS_ENS.Text = ""
                        TextBox_SS_ENS.Focus()
                        Exit Sub
                    End If
                Next
            End If
            'si pas de sous-ensemble
            _ERGT_OPRT_PASS("P")
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            TextBox_NU_SER.Enabled = True
            TextBox_NU_SER.Text = ""
            TextBox_NU_SER.Focus()
        End Try
    End Sub

    Protected Sub TextBox_VALE_TextChanged(sender As Object, e As EventArgs) Handles TextBox_VALE.TextChanged
        Dim dt_ETAP As DataTable = Session("dt_ETAP")
        Dim i_NU_ETAP As Integer = Session("i_NU_ETAP")
        Dim sQuery As String = ""
        Try
            'Enregistrement
            Session("sQuery_Etap_Test") &= "(" & Session("ID_PSG") & ", " & dt_ETAP(i_NU_ETAP)("ID_TEST").ToString & ", " & Replace(TextBox_VALE.Text, ",", ".") & "), "

            'comparaison
            If Convert.ToDecimal(Replace(TextBox_VALE.Text, ",", ".")) < Convert.ToDecimal(dt_ETAP(i_NU_ETAP)("NU_LIMI_IFRE_ETAP").ToString) Then Throw New Exception("La valeur saisie est en dessous de la limite inférieure")
            If Convert.ToDecimal(Replace(TextBox_VALE.Text, ",", ".")) > Convert.ToDecimal(dt_ETAP(i_NU_ETAP)("NU_LIMI_SPRE_ETAP").ToString) Then Throw New Exception("La valeur saisie est au dessus de la limite inférieure")

            'chargement de l'étape suivante
            If dt_ETAP.Rows.Count - 1 > i_NU_ETAP Then
                Session("i_NU_ETAP") = i_NU_ETAP + 1
                _AFCG_ETAP(Session("i_NU_ETAP"))
            Else 'si fini enregistrement passage ou passer à la saisie des numéros de série des sous-ensemble
                For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                    If rGridView_REPE.Cells(3).Text = "PRODUIT" Or rGridView_REPE.Cells(3).Text = "PRODUIT SEMI-FINI" Then
                        Label_CD_SS_ENS.Text = rGridView_REPE.Cells(1).Text
                        MultiView_SAIS_OPRT.SetActiveView(View_SAIS_TCBL_COMP)
                        MultiView_Tracabilité.SetActiveView(View_SAI_SOUS_ENSE)
                        TextBox_SS_ENS.Text = ""
                        TextBox_SS_ENS.Focus()
                        Exit Sub
                    End If
                Next
                'si pas de sous-ensemble
                _ERGT_OPRT_PASS("P")
            End If
            TextBox_VALE.Text = ""
        Catch ex As Exception 'si mauvais enregistrement passage
            _ERGT_OPRT_PASS("F")
            TextBox_VALE.Text = ""
        End Try

    End Sub

    Protected Sub Button_PASS_Click(sender As Object, e As EventArgs) Handles Button_PASS.Click
        Dim dt_ETAP As DataTable = Session("dt_ETAP")
        Dim i_NU_ETAP As Integer = Session("i_NU_ETAP")
        Dim sQuery As String = ""
        Try
            'Enregistrement
            Session("sQuery_Etap_Test") &= "(" & Session("ID_PSG") & ", " & dt_ETAP(i_NU_ETAP)("ID_TEST").ToString & ", 1), "

            'chargement de l'étape suivante
            If dt_ETAP.Rows.Count - 1 > i_NU_ETAP Then
                Session("i_NU_ETAP") = i_NU_ETAP + 1
                _AFCG_ETAP(Session("i_NU_ETAP"))
            Else 'si fini enregistrement passage ou passer à la saisie des numéros de série des sous-ensemble
                For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                    If rGridView_REPE.Cells(3).Text = "PRODUIT" Or rGridView_REPE.Cells(3).Text = "PRODUIT SEMI-FINI" Then
                        Label_CD_SS_ENS.Text = rGridView_REPE.Cells(1).Text
                        MultiView_SAIS_OPRT.SetActiveView(View_SAIS_TCBL_COMP)
                        MultiView_Tracabilité.SetActiveView(View_SAI_SOUS_ENSE)
                        TextBox_SS_ENS.Text = ""
                        TextBox_SS_ENS.Focus()
                        Exit Sub
                    End If
                Next
                'si pas de sous-ensemble
                _ERGT_OPRT_PASS("P")
            End If
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            TextBox_NU_SER.Enabled = True
            TextBox_NU_SER.Text = ""
            TextBox_NU_SER.Focus()
        End Try

    End Sub

    Protected Sub Button_FAIL_Click(sender As Object, e As EventArgs) Handles Button_FAIL.Click
        Dim dt_ETAP As DataTable = Session("dt_ETAP")
        Dim i_NU_ETAP As Integer = Session("i_NU_ETAP")
        Dim sQuery As String = ""
        Try
            'Enregistrement
            Session("sQuery_Etap_Test") &= "(" & Session("ID_PSG") & ", " & dt_ETAP(i_NU_ETAP)("ID_TEST").ToString & ", 0), "

            'enregistrement 
            _ERGT_OPRT_PASS("F")
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try

    End Sub

    Public Sub _ERGT_OPRT_PASS(sSANC As String)

        Dim sQuery As String = "", sNU_SER_ECO As String = "", sNU_SER_CLIE As String = "", sNS As String = ""
        Dim dt_PARA, dt, dt_CFGR_ARTI_ECO, dt2, dt_LIST_NU_SER_TRAC, dt_ETAT_CTRL, dtMSEG As New DataTable
        Dim iIDTT As Integer
        Try
            dt_CFGR_ARTI_ECO = DIG_FACT_SQL_CFGR_ARTI_ECO(Trim(Label_CD_ARTI.Text))
            Select Case "1"
                Case dt_CFGR_ARTI_ECO(0)("Numéro de série Eolane").ToString
                    sNU_SER_ECO = TextBox_NU_SER.Text
                Case dt_CFGR_ARTI_ECO(0)("Numéro de série client").ToString
                    sNU_SER_CLIE = TextBox_NU_SER.Text
                    'Enregistrement NS client
                    sQuery = "INSERT INTO [dbo].[DTM_TR_CPT] ([NM_NS_CLT],[ID_PSG],[DT_PSG],[ID_CPT])
                                   VALUES ('" & sNU_SER_CLIE & "', " & Session("ID_PSG") & ", GETDATE(),'-')"
                    SQL_REQ_ACT(sQuery, sChaineConnexion)
            End Select

            'Enregistrement
            sQuery = "INSERT INTO [dbo].[DTM_PSG] ([ID_PSG], [LB_ETP], [DT_DEB], [DT_FIN], [LB_MOYN], [LB_PROG], [NM_MATR], [NM_NS_EOL], [LB_SCTN], [NM_OF])
                           VALUES (" & Session("ID_PSG") & ", '" & Label_DES_OP.Text & " (OP:" & Label_OP.Text & ")', '" & Session("DT_DEB") & "', GETDATE(), '" & Label_POST.Text & "', '" & HttpContext.Current.CurrentHandler.ToString & "', '" & Session("matricule") & "', '" & sNU_SER_ECO & "', '" & sSANC & "', '" & Label_OF.Text & "')"
            SQL_REQ_ACT(sQuery, sChaineConnexion)

            'Enregitrement test
            If Session("sQuery_Etap_Test") <> "" Then SQL_REQ_ACT(sQuery_Test & COMM_APP_WEB_STRI_TRIM_RIGHT(Session("sQuery_Etap_Test"), 2), sChaineConnexion)

            'enregistrer dans la base
            For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                iIDTT = 0
                If rGridView_REPE.Cells(3).Text = "PRODUIT" Or rGridView_REPE.Cells(3).Text = "PRODUIT SEMI-FINI" Then
                    sQuery = "INSERT INTO [dbo].[DTM_TR_CPT] ([NM_NS_EOL],[NM_NS_CLT],[ID_CPT],[ID_PSG],[DT_PSG],[NM_SAP_CPT],[NM_NS_SENS])
                                   VALUES ('" & sNU_SER_ECO & "', '" & sNU_SER_CLIE & "', '-', " & Session("ID_PSG") & ", GETDATE(), '" & rGridView_REPE.Cells(1).Text & "','" & rGridView_REPE.Cells(6).Text & "')"
                Else
                    sQuery = "INSERT INTO [dbo].[DTM_TR_CPT] ([NM_NS_EOL],[NM_NS_CLT],[ID_CPT],[ID_PSG],[DT_PSG],[NM_SAP_CPT])
                                   VALUES ('" & sNU_SER_ECO & "', '" & sNU_SER_CLIE & "', '" & rGridView_REPE.Cells(6).Text & "', " & Session("ID_PSG") & ", GETDATE(), '" & rGridView_REPE.Cells(1).Text & "')"
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

            'Affichage

            'mise à jour des quantité restante
            For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                If rGridView_REPE.Cells(5).Text <> "&nbsp;" Then
                    sQuery = "SELECT [NM_QTE_INIT]
                                    ,ISNULL([NB_UTLS],0) AS NB_UTLS
                                FROM [dbo].[V_LIST_CONT_NON_VIDE]
                               WHERE [NM_CNTR] = '" & rGridView_REPE.Cells(5).Text & "' AND [NM_OF] = '" & TextBox_OF.Text & "'"
                    dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
                    If dt Is Nothing Then
                        rGridView_REPE.Cells(8).Text = dt(0)("NM_QTE_INIT").ToString
                    Else
                        rGridView_REPE.Cells(8).Text = Convert.ToDecimal(dt(0)("NM_QTE_INIT").ToString) - (Convert.ToDecimal(dt(0)("NB_UTLS").ToString) * Convert.ToDecimal(rGridView_REPE.Cells(4).Text))
                    End If
                Else
                    dtMSEG = SAP_DATA_READ_MSEG("MBLNR EQ '" & Left(rGridView_REPE.Cells(6).Text, 10) & "' AND ZEILE EQ '" & Mid(rGridView_REPE.Cells(6).Text, 11, 4) & "' AND MJAHR EQ '" & Mid(rGridView_REPE.Cells(6).Text, 15, 4) & "'")
                    If dtMSEG Is Nothing Then Continue For
                    sQuery = "SELECT COUNT([DT_PSG]) AS NB_ID_COMP
                                FROM [dbo].[DTM_TR_CPT]
                               WHERE [ID_CPT] LIKE '" & rGridView_REPE.Cells(6).Text & "%'
                              GROUP BY [ID_CPT]"
                    dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
                    If dt Is Nothing Then
                        rGridView_REPE.Cells(8).Text = Convert.ToDecimal(Replace(dtMSEG(0)("MENGE").ToString, ".", ","))
                    Else
                        rGridView_REPE.Cells(8).Text = Convert.ToDecimal(Replace(dtMSEG(0)("MENGE").ToString, ".", ",")) - Convert.ToDecimal(dt(0)("NB_ID_COMP").ToString) * Convert.ToDecimal(rGridView_REPE.Cells(4).Text)
                    End If
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

            sQuery = "SELECT [NU_SER_CLIE] AS [Numéro de série client]
                            ,[NU_SER_ECO] AS [Numéro de série Eolane]
                        FROM [dbo].[V_DER_PASS_BON]
                       WHERE [NM_OF] = '" & Label_OF.Text & "'
                         AND [LB_ETP] = '" & Label_DES_OP.Text & " (OP:" & Label_OP.Text & ")'"
            dt_LIST_NU_SER_TRAC = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            If Not dt_LIST_NU_SER_TRAC Is Nothing Then
                'décrément quantité totale 
                Label_QT_REST_OF.Text = (Convert.ToDecimal(Replace(Label_QT_OF.Text, ".", ",")) - dt_LIST_NU_SER_TRAC.Rows.Count).ToString
                If Convert.ToDecimal(Replace(Label_QT_OF.Text, ".", ",")) - dt_LIST_NU_SER_TRAC.Rows.Count = 0 Then 'OF terminé
                    For Each rGridView_LIST_MTRE As GridViewRow In GridView_LIST_MTRE.Rows 'Dissociation du matériel générique
                        If rGridView_LIST_MTRE.Cells(2).Text = "Générique" Then
                            sQuery = "INSERT INTO [dbo].[DTM_REF_PARA]([NM_CRIT],[NM_PARA],[VAL_PARA],[DT_PARA])
                                           VALUES ('" & Label_POST.Text & "','" & rGridView_LIST_MTRE.Cells(1).Text & "','0',GETDATE())"
                            SQL_REQ_ACT(sQuery, sChaineConnexion)
                        End If
                    Next
                    TextBox_OF.Text = ""
                    Label_OF.Text = ""
                    Label_CD_ARTI.Text = ""
                    Label_CLIE.Text = ""
                    Label_DES_ARTI.Text = ""
                    Label_QT_OF.Text = ""
                    Label_DES_OP.Text = ""
                    Label_OP.Text = ""
                    Throw New Exception("L'OF est terminé.")
                    Exit Sub
                End If
                'affichage de la liste des NS tracés
                GridView_SN_TRAC.DataSource = dt_LIST_NU_SER_TRAC
                GridView_SN_TRAC.DataBind()
            End If

            MultiView_SAIS_OPRT.SetActiveView(View_SAIS_NU_SER_CHEC)
            MultiView_ETAP.SetActiveView(View_VOID)
            TextBox_NU_SER.Enabled = True
            TextBox_NU_SER.Text = ""
            TextBox_NU_SER.Focus()
            If sSANC = "F" Then
                Label_RES.Text = "Le numéro de série " & TextBox_NU_SER.Text & " est passé mauvais."
                Label_CONS.Text = ""
                Image_PHOT_ILST.Visible = False
                Image_PHOT_ILST.ImageUrl = ""
                Button_PASS.Visible = False
                Button_FAIL.Visible = False
                TextBox_VALE.Visible = False
            Else
                Label_RES.Text = "Le numéro de série " & TextBox_NU_SER.Text & " est passé bon."
                'imprimer une étiquette
                'Si la TextBox de génération des étiquettes est cochée
                dt_CFGR_ARTI_ECO = DIG_FACT_SQL_CFGR_ARTI_ECO(Trim(Label_CD_ARTI.Text))
                If dt_CFGR_ARTI_ECO(0)("Génération impression numéro de série").ToString = Label_OP.Text Then
                    Select Case "1"
                        Case dt_CFGR_ARTI_ECO(0)("Numéro de série Eolane").ToString
                            dt_ETAT_CTRL = COMM_APP_WEB_ETAT_CTRL(Trim(Label_CD_ARTI.Text) & "|Numéro de série Eolane", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx")
                            If dt_ETAT_CTRL Is Nothing Then Throw New Exception("La base App_Web_Eco n'as pas été configurée pour l'article " & Trim(Label_CD_ARTI.Text))
                            sNS = Label_OF.Text & DIG_FACT_SQL_GENE_NU_SER_CLIE("10", "1", "%%%%%%", Label_OF.Text, "Numéro de série Eolane", False,
                                                          Session("matricule"), Label_OF.Text)
                            If dt_ETAT_CTRL.Columns.Contains("TextBox_FICH_MODE") Then
                                DIG_FACT_IMPR_ETIQ(dt_ETAT_CTRL(0)("TextBox_FICH_MODE").ToString,
                                                   TextBox_OF.Text, "", "Numéro de série Eolane", "", sNS, "", "", "", Nothing)
                            End If
                        Case dt_CFGR_ARTI_ECO(0)("Numéro de série client").ToString
                            Dim sFORM_NU_CLIE As String = ""
                            dt_ETAT_CTRL = COMM_APP_WEB_ETAT_CTRL(Trim(Label_CD_ARTI.Text) & "|Numéro de série client", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx")
                            If dt_ETAT_CTRL Is Nothing Then Throw New Exception("La base App_Web_Eco n'as pas été configurée pour l'article " & Trim(Label_CD_ARTI.Text))
                            If dt_ETAT_CTRL.Columns.Contains("TextBox_FORM_NU_CLIE") = True Then sFORM_NU_CLIE = dt_ETAT_CTRL(0)("TextBox_FORM_NU_CLIE").ToString
                            sNS = DIG_FACT_SQL_GENE_NU_SER_CLIE(dt_CFGR_ARTI_ECO(0)("Encodage Numéro de série client").ToString,
                                                            dt_CFGR_ARTI_ECO(0)("Incrémentation flanc").ToString,
                                                            sFORM_NU_CLIE,
                                                            Label_CD_ARTI.Text,
                                                            "Numéro de série client",
                                                            False,
                                                            Session("matricule"),
                                                            Label_OF.Text)
                            If dt_ETAT_CTRL.Columns.Contains("TextBox_FICH_MODE") Then
                                DIG_FACT_IMPR_ETIQ(dt_ETAT_CTRL(0)("TextBox_FICH_MODE").ToString,
                                               TextBox_OF.Text, "", "Numéro de série client", sNS, "", "", "", "", Nothing)
                            End If
                    End Select
                    Session("sNU_SER_IMPR") = sNS
                End If
            End If
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            'Exit Sub
        End Try

    End Sub

    Public Sub _AFCG_ETAP(i_NU_ETAP As Integer)
        Dim dt_ETAP As DataTable = Session("dt_ETAP")
        Dim sfich As String
        Try
            Image_PHOT_ILST.Visible = False
            Image_PHOT_ILST.ImageUrl = ""
            Button_PASS.Visible = False
            Button_FAIL.Visible = False
            TextBox_VALE.Visible = False
            Label_CONS.Text = dt_ETAP(i_NU_ETAP)("LB_CONS_ETAP").ToString
            If dt_ETAP(i_NU_ETAP)("NU_LIMI_IFRE_ETAP").ToString = "1" And dt_ETAP(i_NU_ETAP)("NU_LIMI_SPRE_ETAP").ToString = "1" Then
                Button_PASS.Visible = True
                Button_FAIL.Visible = True
                Button_FAIL.Focus()
            Else
                TextBox_VALE.Visible = True
                TextBox_VALE.Focus()
            End If
            If Not dt_ETAP(i_NU_ETAP)("NM_CHEM_PHOT_ETAP").ToString Is String.Empty Or Not dt_ETAP(i_NU_ETAP)("NM_CHEM_PHOT_ETAP").ToString Is Nothing Then
                Image_PHOT_ILST.Visible = True
                Randomize()
                sfich = "c:\sources\Dev_app_web\PagesMembres\Digital_Factory\photo_" & CInt(Int((1000 * Rnd()) + 1)) & "." & System.IO.Path.GetExtension(dt_ETAP(i_NU_ETAP)("NM_CHEM_PHOT_ETAP").ToString)
                COMM_APP_WEB_COPY_FILE(dt_ETAP(i_NU_ETAP)("NM_CHEM_PHOT_ETAP").ToString, sfich, True)
                Image_PHOT_ILST.ImageUrl = System.IO.Path.GetFileName(sfich)
            End If
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try
    End Sub

    Protected Sub DropDownList_POST_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList_POST.SelectedIndexChanged
        Dim sQuery As String = ""
        Dim dt_POST_NMCT_TPLG As New DataTable
        Try
            Label_POST.Text = DropDownList_POST.SelectedValue
            'affiche config dédiée + extraction nomenclature type pour matériel générique
            If _AFCG_POST_NMCT_TPLG() = "1" Then Exit Sub

            'si pas de saisie de matériel générique Extraire la liste des étapes de l'opération
            Session("dt_ETAP") = _ETCT_ETAP_OPRT()
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try

    End Sub

    Public Function _ETCT_ETAP_OPRT() As DataTable
        Dim sQuery As String = ""
        Dim dt_ETAP As New DataTable
        Try
            sQuery = "SELECT [ID_TEST]
                            ,[ID_INDE]
                            ,[LB_CONS_ETAP]
                            ,[NM_CHEM_PHOT_ETAP]
                            ,[NU_LIMI_IFRE_ETAP]
                            ,[NU_LIMI_SPRE_ETAP]
                            ,[NU_VERS_ETAP]
                        FROM [dbo].[V_LIST_ETAP_OPRT]
                       WHERE [CD_ARTI] = '" & Label_CD_ARTI.Text & "' AND [NM_OPRT] = '" & Label_DES_OP.Text & " (OP:" & Label_OP.Text & ")'
                      ORDER BY [ID_INDE]"
            dt_ETAP = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            Return dt_ETAP

        Catch ex As Exception When dt_ETAP Is Nothing
            LOG_Erreur(GetCurrentMethod, "Pas de config étape")
            Return Nothing
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try
    End Function

    Public Function _AFCG_POST_NMCT_TPLG() As String
        Dim sQuery As String = ""
        Dim dt_POST_NMCT_TPLG As New DataTable
        Try
            sQuery = "SELECT ISNULL(B.VAL_PARA, NM_TYPE_MTRE) AS [Type de matériel]
	                      ,ID_MTRE AS [ID du matériel]
                          ,B_C.VAL_PARA AS [Catégorie]
                      FROM (
		                    SELECT NM_POST, ID_MTRE, VAL_PARA
		                      FROM (
				                    SELECT     NM_PARA AS ID_MTRE, CD_ARTI AS NM_POST
				                      FROM          dbo.V_DER_DTM_REF_PARA
				                     WHERE      (CD_ARTI = '" & Label_POST.Text & "') AND (VAL_PARA = '1')
		                         ) AS DT_MTRE_ERGT INNER JOIN
				                      dbo.V_DER_DTM_REF_PARA AS V_DER_DTM_REF_PARA_1 ON DT_MTRE_ERGT.ID_MTRE = V_DER_DTM_REF_PARA_1.CD_ARTI
		                     WHERE     (V_DER_DTM_REF_PARA_1.NM_PARA = N'Type matériel')) AS B
                    FULL OUTER JOIN (
		                    SELECT CD_ARTI AS ID_POST, 
			                       NM_TYPE_MTRE
		                     FROM (
				                    SELECT     CD_ARTI, NM_PARA, VAL_PARA
				                    FROM         dbo.V_DER_DTM_REF_PARA
				                    WHERE     (CD_ARTI LIKE N'POSTE_%') AND 
						                      (NM_PARA = N'Article poste' OR NM_PARA = N'Opération poste')
			                      ) as a 
		                    pivot (max(VAL_PARA) for NM_PARA in ([Article poste], [Opération poste])) as pt
		                    INNER JOIN dbo.V_POST_NMCT_TPLG ON dbo.V_POST_NMCT_TPLG.CD_ARTI_ECO_POST = pt.[Article poste] AND dbo.V_POST_NMCT_TPLG.NU_OP_POST = pt.[Opération poste]
		                    WHERE CD_ARTI = '" & Label_POST.Text & "'
                    ) AS A ON ID_POST = NM_POST AND VAL_PARA = NM_TYPE_MTRE
                    LEFT OUTER JOIN (SELECT CD_ARTI, VAL_PARA
									  FROM [dbo].[V_DER_DTM_REF_PARA]
									 WHERE NM_PARA = 'Poste dédié') AS B_C ON CD_ARTI = ID_MTRE"
            dt_POST_NMCT_TPLG = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            GridView_LIST_MTRE.DataSource = dt_POST_NMCT_TPLG
            GridView_LIST_MTRE.DataBind()
            For Each rGridView_LIST_MTRE As GridViewRow In GridView_LIST_MTRE.Rows
                If rGridView_LIST_MTRE.Cells(1).Text = "&nbsp;" Then
                    Label_TYPE_MTRE.Text = rGridView_LIST_MTRE.Cells(0).Text
                    TextBox_MTRE_GNRQ.Focus()
                    MultiView_SAIS_OPRT.SetActiveView(View_SAIS_OTLG)
                    MultiView_POST.SetActiveView(View_SAIS_POST_MTRE_GNRQ)
                    Return "1"
                End If
            Next
            Return "0"

        Catch ex As Exception When dt_POST_NMCT_TPLG Is Nothing
            LOG_Erreur(GetCurrentMethod, "Pas de poste type configuré dans la base, prévenir un méthode")
            Return Nothing
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try
    End Function

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

            'Vérifier le format du numéro de série
            dt_CFGR_ARTI_ECO = DIG_FACT_SQL_CFGR_ARTI_ECO(Trim(Label_CD_SS_ENS.Text))
            Select Case "1"
                Case dt_CFGR_ARTI_ECO(0)("Numéro de série Eolane").ToString
                    If DIG_FACT_VERI_FORM_NU_SER(Trim(Label_CD_SS_ENS.Text), "Format Numéro de série Eolane", TextBox_SS_ENS.Text) = False Then Throw New Exception("Le numéro de série " & TextBox_SS_ENS.Text & " ne correspond au format défini dans la base.")
                    If dt_CFGR_ARTI_ECO(0)("Format numéro de série Eolane OF6INC").ToString = "True" Then
                        dtAFKO = SAP_DATA_READ_AFKO("AUFNR LIKE '%" & Left(TextBox_SS_ENS.Text, 7) & "'")
                        Label_CD_SS_ENS.Text = Trim(dtAFKO(0)("PLNBEZ").ToString)
                    End If
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

            'si terminé enregistrer les données
            _ERGT_OPRT_PASS("P")
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            MultiView_Tracabilité.SetActiveView(View_SAI_SOUS_ENSE)
            TextBox_SS_ENS.Text = ""
            TextBox_SS_ENS.Focus()
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
            MultiView_SAIS_OPRT.SetActiveView(View_SAIS_NU_SER_CHEC)
            TextBox_NU_SER.Enabled = True
            TextBox_NU_SER.Text = ""
            TextBox_NU_SER.Focus()
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            TextBox_NU_BAC.Text = ""
            TextBox_NU_BAC.Focus()
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
            MultiView_SAIS_OPRT.SetActiveView(View_SAIS_NU_SER_CHEC)
            TextBox_NU_SER.Enabled = True
            TextBox_NU_SER.Text = ""
            TextBox_NU_SER.Focus()
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            TextBox_ID_COMP.Text = ""
            TextBox_ID_COMP.Focus()
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
            MultiView_SAIS_OPRT.SetActiveView(View_SAIS_NU_SER_CHEC)
            TextBox_NU_SER.Enabled = True
            TextBox_NU_SER.Text = ""
            TextBox_NU_SER.Focus()
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            TextBox_CD_LOT_COMP.Text = ""
            TextBox_CD_LOT_COMP.Focus()
        End Try
    End Sub
End Class