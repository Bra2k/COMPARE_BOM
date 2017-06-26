Imports App_Web.Class_SAP_DATA
Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports App_Web.Class_SQL
Imports App_Web.Class_DIG_FACT
Imports App_Web.Class_DIG_FACT_SQL
Imports App_Web.Class_COMM_APP_WEB
Imports App_Web.Class_PDF
Imports itextsharp.text.pdf
Imports itextsharp.text
Imports System.IO
Imports itextsharp
Imports System



Public Class TCBL_OPRT
    Inherits System.Web.UI.Page
    'Dim sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=" & Replace(Replace(My.Computer.Name, "CEDB03", "MES_Digital_Factory_DEV"), "CEAPP03", "MES_Digital_Factory") & ";Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=7200;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
    Dim sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=MES_Digital_Factory;Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=7200;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
    Dim sQuery_Test As String = "INSERT INTO [dbo].[DTM_TEST_FONC_VAL]([ID_EXEC],[ID_TEST],[NB_VAL]) VALUES "
    Dim sChaineConnexion_ALMS As String = "Data Source=cedb03,1433;Initial Catalog=ALMS_PROD_PRD;Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=7200;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            '      HttpContext.Current.Response.Write(" < body <> Script type=""text/javascript"">
            '   var pmt = prompt(""Entrer votre matricule SAP"","""");
            'document.getElementById('<%=Label_OF.ClientID%>').value = pmt;</script></body>")
            '      'Session("matricule") = SAP.InnerHtml
            '      'ClientID.sap

            '      dt_matr = SAP_DATA_MATR(Session("matricule"))
            '      If dt_matr Is Nothing Then Throw New Exception("pas de nom/prénom trouvés pour le matricule " & Session("matricule"))
            '      Session("sn") = LTrim(RTrim(dt_matr(0)("NACHN").ToString))
            '      Session("givenname") = LTrim(RTrim(dt_matr(0)("VORNA").ToString))
            '      Session("displayname") = Session("sn") & " " & Session("givenname")
            '      'Request.ServerVariables("HTTP_REFERER")
            '      'LOG_Msg(GetCurrentMethod, Page.PreviousPage.Request.Url.ToString())
            '      Response.Redirect(Session("UrlReferrer"))
            'GridView_REPE0.DataSource = GridView_REPE.DataSource
            'GridView_REPE0.DataBind()
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try

    End Sub

    Protected Sub TextBox_OF_TextChanged(sender As Object, e As EventArgs) Handles TextBox_OF.TextChanged
        'Dim dt, dtAFKO, dtAFVC, dt_op As New DataTable
        Dim sQuery As String = ""
        Try
            'extraction données de l'OF
            Using dtAFKO = SAP_DATA_READ_AFKO($"AUFNR LIKE '%{TextBox_OF.Text}'")
                If dtAFKO Is Nothing Then Throw New Exception($"L'OF n°{TextBox_OF.Text} n'a pas été trouvé dans SAP.")

                sQuery = $"SELECT CONVERT(INTEGER,[AUFNR]) AS AUFNR
                        FROM [SAP].[dbo].[AFKO]
                       WHERE CONVERT(INTEGER,[AUFNR]) = {TextBox_OF.Text}"
                Using dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion_ALMS)
                    If dt Is Nothing Then
                        sQuery = $"INSERT INTO [SAP].[dbo].[AFKO]
                                   ([RSNUM]
                                   ,[GAMNG]
                                   ,[PLNBEZ]
                                   ,[AUFNR]
                                   ,[AUFPL]
                                   ,[REVLV]
                                   ,[STLNR]
                                   ,[STLAL]
                                   ,[GSTRS])
                             VALUES
                                   ('{dtAFKO(0)("RSNUM").ToString}'
                                   ,'{dtAFKO(0)("GAMNG").ToString}'
                                   ,'{dtAFKO(0)("PLNBEZ").ToString}'
                                   ,'{dtAFKO(0)("AUFNR").ToString}'
                                   ,'{dtAFKO(0)("AUFPL").ToString}'
                                   ,'{dtAFKO(0)("REVLV").ToString}'
                                   ,'{dtAFKO(0)("STLNR").ToString}'
                                   ,'{dtAFKO(0)("STLAL").ToString}'
                                   ,'{dtAFKO(0)("GSTRS").ToString}')"
                        SQL_REQ_ACT(sQuery, sChaineConnexion)
                    End If
                End Using
                Using dt = SAP_DATA_LECT_OF(TextBox_OF.Text)
                    With dt
                        Label_OF.Text = TextBox_OF.Text
                        Label_CD_ARTI.Text = Trim(.Rows(0)("CD_ARTI_ECO").ToString)
                        Label_CLIE.Text = Trim(.Rows(0)("NM_CLIE").ToString)
                        Label_DES_ARTI.Text = Trim(.Rows(0)("NM_DSGT_ARTI").ToString)
                        Label_QT_OF.Text = Trim(.Rows(0)("QT_OF").ToString)
                    End With
                End Using
                'Extraction de la gamme
                Using dt_op As New DataTable
                    With dt_op.Columns
                        .Add("OP", Type.GetType("System.String"))
                        .Add("Désignation", Type.GetType("System.String"))
                    End With
                    Using dtAFVC = SAP_DATA_READ_AFVC($"AUFPL EQ '{dtAFKO(0)("AUFPL").ToString}'")
                        For Each rdtAFVC As DataRow In dtAFVC.Select(Nothing, "VORNR")
                            With dt_op
                                .Rows.Add()
                                .Rows(.Rows.Count - 1)("OP") = rdtAFVC("VORNR").ToString
                                .Rows(.Rows.Count - 1)("Désignation") = $"{rdtAFVC("LTXA1").ToString} (OP:{(Convert.ToDecimal(rdtAFVC("VORNR").ToString)).ToString})"
                            End With
                        Next
                    End Using
                    With DropDownList_OP
                        .DataSource = dt_op
                        .DataTextField = "Désignation" '"VORNR"
                        .DataValueField = "OP"
                        .DataBind()
                    End With
                End Using
            End Using
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try
    End Sub

    Protected Sub Button_VALI_ENTER_Click(sender As Object, e As EventArgs) Handles Button_VALI_ENTER.Click
        Dim sQuery As String = "", sNS As String = ""
        Dim dt, dtCRHD, dtAFKO, dtAFVC, dtVRESB, dtSTPO, dtSTPU, dt_POST, dt_LIST_NU_SER_TRAC, dt_SS_ENS, dt_CFGR_ARTI_ECO, dt_ETAT_CTRL As New DataTable

        Try
            'extraction données de l'OF
            dtAFKO = SAP_DATA_READ_AFKO("AUFNR LIKE '%" & TextBox_OF.Text & "'")
            If dtAFKO Is Nothing Then Throw New Exception("L'OF n°" & TextBox_OF.Text & " n'a pas été trouvé dans SAP.")
            'Extraction de la gamme
            dtAFVC = SAP_DATA_READ_AFVC("AUFPL EQ '" & dtAFKO(0)("AUFPL").ToString & "' AND VORNR EQ '" & DropDownList_OP.SelectedValue & "'")

            'si opération d'emballage, redirection vers la page colisage
            'dtCRHD = SAP_DATA_READ_CRHD("OBJID EQ '" & dtAFVC(0)("ARBID").ToString & "' AND ARBPL LIKE 'EMB%'")
            'If Not dtCRHD Is Nothing Then
            '    Session("OF_TO_CLSG") = Label_OF.Text
            '    Response.Redirect("~/PagesMembres/Digital_Factory/CLSG.aspx")
            '    Exit Sub
            'End If

            'vérification de l'habilitation ALMS sur l'opération Acceptation (DHR)
            dtCRHD = SAP_DATA_READ_CRHD("OBJID EQ '" & dtAFVC(0)("ARBID").ToString & "' AND ARBPL LIKE 'RCT%'")
            If Not dtCRHD Is Nothing Then
                If Session("matricule") = "" Then
                    Session("UrlReferrer") = HttpContext.Current.Request.Url.AbsolutePath.ToString
                    Response.Redirect("~/Account/Login_SAP.aspx")
                End If
                sQuery = "SELECT [DT_ATVT]
                            FROM [dbo].[DTM_REF_OPRT]
                           WHERE [ID_MTCL_ECO] = " & Session("matricule") & "
                             AND [BL_ATVT] = 1
                             AND [ID_HBLT_ALMS] = 1"
                dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion_ALMS)
                If dt Is Nothing Then
                    Response.Redirect("~/PagesMembres/RDRC_PAGE_MEMB.aspx")
                    Throw New Exception(Session("displayname") & " n'a pas le droit d'accéder à cette page")
                End If
            End If

            'Opération
            Label_DES_OP.Text = Trim(dtAFVC(0)("LTXA1").ToString)
            Label_OP.Text = Convert.ToDecimal(DropDownList_OP.SelectedValue).ToString

            'charger les ns entrés0000187124
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
                    DropDownList_OP.ClearSelection()
                    Throw New Exception("L'OF est terminé pour l'opération")
                    'Exit Sub
                End If
                'affichage de la liste des NS tracés
                GridView_SN_TRAC.DataSource = dt_LIST_NU_SER_TRAC
                GridView_SN_TRAC.DataBind()
            End If

            'extraction de la config dédiée du poste
            '     sQuery = "SELECT CD_ARTI AS ID_POST, 
            '                     [Article poste] AS CD_ARTI_ECO_POST, 
            '                     [Opération poste] AS NU_OP_POST
            '                FROM (SELECT     CD_ARTI, NM_PARA, VAL_PARA
            '                        FROM         dbo.V_DER_DTM_REF_PARA
            '                       WHERE     (CD_ARTI LIKE N'POSTE_%') AND (NM_PARA = N'Article poste' OR NM_PARA = N'Opération poste')) as a
            'pivot (max(VAL_PARA) for NM_PARA in ([Article poste], [Opération poste])) as pt
            '               WHERE [Article poste] = '" & Label_CD_ARTI.Text & "' AND [Opération poste] = '" & Label_OP.Text & "'"
            '     dt_POST = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            '     If dt_POST Is Nothing Then Throw New Exception("Pas de poste configuré dans la base, prévenir un méthode")
            '     If dt_POST.Rows.Count > 1 Then
            '         With DropDownList_POST
            '             .DataSource = dt_POST
            '             .DataTextField = "ID_POST"
            '             .DataValueField = "ID_POST"
            '             .DataBind()
            '         End With
            '         MultiView_SAIS_OPRT.SetActiveView(View_SAIS_OTLG)
            '         MultiView_POST.SetActiveView(View_SAIS_POST_MULT)
            '         Exit Sub
            '     End If
            '     Label_POST.Text = dt_POST(0)("ID_POST").ToString

            'affiche config dédiée + extraction nomenclature type pour matériel générique
            If _AFCG_POST_NMCT_TPLG() = "1" Then Exit Sub

            'Extraire la liste des étapes de l'opération
            Session("dt_ETAP") = _ETCT_ETAP_OPRT()

            MultiView_SAIS_OPRT.SetActiveView(View_SAIS_NU_SER_CHEC)
            'Label_RES.Text = ""

            'Si la TextBox de génération des étiquettes est cochée
            _IPRO_ETQT()

            'vérification si traçabilité composant à faire
            _VRFC_TCBL_COMP()

        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try
    End Sub

    Protected Sub Button_VALI_ENTER_OTLG_Click(sender As Object, e As EventArgs) Handles Button_VALI_ENTER_OTLG.Click

        Dim sQuery As String = ""
        Dim dt As New DataTable
        Try
            With TextBox_MTRE_GNRQ
                'vérification qu'il existe (FER-M-0551)
                sQuery = "SELECT [CD_ARTI]
                                ,[NM_PARA]
                                ,[VAL_PARA]
                                ,[DT_PARA]
                            FROM [dbo].[V_DER_DTM_REF_PARA]
                           WHERE CD_ARTI = '" & .Text & "' AND [NM_PARA] = 'Type matériel' AND [VAL_PARA] = '" & HttpUtility.HtmlDecode(Label_TYPE_MTRE.Text) & "'"
                dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
                If dt Is Nothing Then Throw New Exception("Le matériel n°" & .Text & " n'est pas déclaré dans la base de données. Prévenir un méthode.")

                'Dissociation si attribué --> @modifier, si n'a jamais été attribué, première déclaration, fait planter
                'sQuery = "INSERT INTO [dbo].[DTM_REF_PARA]([NM_CRIT],[NM_PARA],[VAL_PARA],[DT_PARA])
                '        SELECT [CD_ARTI],'" & .Text & "','0',GETDATE()
                '          FROM [dbo].[V_DER_DTM_REF_PARA] 
                '            INNER JOIN (SELECT MAX([DT_PARA]) AS MAX_DT_PARA
                '              FROM [dbo].[V_DER_DTM_REF_PARA]
                '             WHERE [NM_PARA] = '" & .Text & "') AS A ON [DT_PARA] = A.MAX_DT_PARA
                '         WHERE [NM_PARA] = '" & .Text & "' AND [VAL_PARA] = '1'"
                'SQL_REQ_ACT(sQuery, sChaineConnexion)
            End With
            'enregistrement dans table
            For Each rGridView_LIST_MTRE As GridViewRow In GridView_LIST_MTRE.Rows
                With rGridView_LIST_MTRE
                    If HttpUtility.HtmlDecode(.Cells(0).Text) = HttpUtility.HtmlDecode(Label_TYPE_MTRE.Text) Then .Cells(1).Text = TextBox_MTRE_GNRQ.Text
                    If .Cells(1).Text = "&nbsp;" Then
                        Label_TYPE_MTRE.Text = .Cells(0).Text
                        TextBox_MTRE_GNRQ.Text = ""
                        TextBox_MTRE_GNRQ.Focus()
                        Exit Sub
                    End If
                End With
            Next

            'enregistrer
            'For Each rGridView_LIST_MTRE As GridViewRow In GridView_LIST_MTRE.Rows
            '    With rGridView_LIST_MTRE
            '        If HttpUtility.HtmlDecode(.Cells(2).Text) = "Générique" Then
            '            sQuery = "INSERT INTO [dbo].[DTM_REF_PARA]([NM_CRIT],[NM_PARA],[VAL_PARA],[DT_PARA])
            '                           VALUES ('" & Label_POST.Text & "','" & HttpUtility.HtmlDecode(.Cells(1).Text) & "','1',GETDATE())"
            '            SQL_REQ_ACT(sQuery, sChaineConnexion)
            '        End If
            '    End With
            'Next

            'Extraire la liste des étapes de l'opération
            Session("dt_ETAP") = _ETCT_ETAP_OPRT()

            MultiView_SAIS_OPRT.SetActiveView(View_SAIS_NU_SER_CHEC)
            'Label_RES.Text = ""

            'Impression étiquette
            _IPRO_ETQT()

            'vérification si traçabilité composant à faire
            _VRFC_TCBL_COMP()

        Catch ex As Exception
            TextBox_MTRE_GNRQ.Text = ""
            TextBox_MTRE_GNRQ.Focus()
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try

    End Sub

    Protected Sub TextBox_NU_SER_TextChanged(sender As Object, e As EventArgs) Handles TextBox_NU_SER.TextChanged

        Dim dt_ETAP As DataTable = Session("dt_ETAP")
        Session("i_NU_ETAP") = 0
        Session("sQuery_Etap_Test") = ""
        Dim dt, dt_WF, dtAFVC, dtAFKO, dtCRHD As New DataTable, sQuery As String = "", sQuery_WF As String = "", sMessage_WF As String = ""

        Dim sNU_SER_ECO As String = "", sNU_SER_CLIE As String = ""
        Dim dt_PARA, dt_CFGR_ARTI_ECO, dt_ETAT_CTRL As New DataTable
        With TextBox_NU_SER
            ' PDF_CREA_FCGF(.Text)
            Try
                dt_CFGR_ARTI_ECO = DIG_FACT_SQL_CFGR_ARTI_ECO(Trim(Label_CD_ARTI.Text))
                'Si la TextBox de génération des étiquettes est cochée
                If dt_CFGR_ARTI_ECO(0)("Génération impression numéro de série").ToString = Label_OP.Text Then
                    Select Case .Text
                        Case "Réimprime-moi le dernier numéro de série" 'Réimprimer le dernier numéro de série
                            _IPRO_ETQT(Session("sNU_SER_IMPR"))
                            .Enabled = True
                            .Text = ""
                            .Focus()
                            Exit Sub
                        Case "Imprime-moi un nouveau numéro de série"
                            'imprimer une étiquette
                            _IPRO_ETQT()
                            .Enabled = True
                            .Text = ""
                            .Focus()
                            Exit Sub
                    End Select
                    If .Text.Contains(Session("sNU_SER_IMPR").ToString) = False Then Throw New Exception("Le numéro de série " & .Text & " n'est pas identique au dernier numéro de série imprimé " & Session("sNU_SER_IMPR") & ".")
                End If

                Select Case "1"
                    Case dt_CFGR_ARTI_ECO(0)("Numéro de série Eolane").ToString
                        sNU_SER_ECO = .Text
                        'vérifier par rapport à un format
                        If DIG_FACT_VERI_FORM_NU_SER(Trim(Label_CD_ARTI.Text), "Format Numéro de série Eolane", .Text) = False Then Throw New Exception("Le numéro de série " & .Text & " ne correspond au format défini dans la base.")
                    Case dt_CFGR_ARTI_ECO(0)("Numéro de série client").ToString
                        sNU_SER_CLIE = .Text
                        'vérifier par rapport à un format
                        If DIG_FACT_VERI_FORM_NU_SER(Trim(Label_CD_ARTI.Text), "Format Numéro de série client", .Text) = False Then Throw New Exception("Le numéro de série " & .Text & " ne correspond au format défini dans la base.")
                End Select

                'vérification de l'appartenance à un OF
                If dt_CFGR_ARTI_ECO(0)("Vérification cohérence OF par numéro de série").ToString = "1" Then
                    sQuery = "SELECT [NU_SER_ECO],[NU_SER_CLIE],[NU_OF]
                                FROM [dbo].[V_LIAIS_NU_SER]
                               WHERE NU_OF = '" & TextBox_OF.Text & "' AND ([NU_SER_ECO] LIKE '%" & sNU_SER_ECO & "%' OR [NU_SER_CLIE] LIKE '%" & sNU_SER_CLIE & "%')"
                    dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
                    If dt Is Nothing Then Throw New Exception("Le numéro de série " & sNU_SER_ECO & sNU_SER_CLIE & " n'appartient pas à l'OF " & TextBox_OF.Text)
                End If

                'vérification que le ns n'est pas déjà associé ou passé !!! ATTENTION !!!
                sQuery = "SELECT [NM_NS_EOL]
                            FROM [dbo].[ID_PF_View]
                           WHERE (NM_NS_EOL LIKE '%" & sNU_SER_ECO & "%' AND [NM_NS_CLT] LIKE '%" & sNU_SER_CLIE & "%') AND ([LB_ETP] = '" & Label_DES_OP.Text & " (OP:" & Label_OP.Text & ")' AND [LB_SCTN] = 'P')"
                dt_PARA = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
                If Not dt_PARA Is Nothing Then Throw New Exception("Le numéro de série " & .Text & " est déjà passé à l'opération " & Label_DES_OP.Text & " (OP:" & Label_OP.Text & ").")

                'Création de l'ID_PSG
                sQuery = "SELECT [NEW_ID_PSG], GETDATE() AS DT_DEB
                            FROM [dbo].[V_NEW_ID_PSG_DTM_PSG]"
                dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
                Session("ID_PSG") = dt(0)("NEW_ID_PSG").ToString
                Session("DT_DEB") = dt(0)("DT_DEB").ToString
                'future nouvelle version à mettre en place quand changement de serveur SQL
                'Session("ID_PSG") = SQL_REQ_ACT_RET_IDTT("INSERT INTO [dbo].[DTM_REF_ID] ([NM_CHAM]) VALUES (1), sChaineConnexion As String)
                'dt = SQL_SELE_TO_DT("SELECT GETDATE() AS DT_DEB", sChaineConnexion)
                'Session("DT_DEB") = dt(0)("DT_DEB").ToString

                'vérification workflow
                If DIG_FACT_SQL_VRFC_WF(sNU_SER_ECO, sNU_SER_CLIE, TextBox_OF.Text, Label_DES_OP.Text & " (OP:" & Label_OP.Text & ")", sChaineConnexion) = False Then
                    _ERGT_OPRT_PASS("F")
                    Throw New Exception("Problème détecté dans le Workflow.")
                End If

                .Enabled = False
                If Not dt_ETAP Is Nothing Then 'étape n° 1
                    _AFCG_ETAP(0)
                    MultiView_ETAP.SetActiveView(View_CHEC_ETAP)
                    Exit Sub
                Else ' ou passer à la saisie des numéros de série des sous-ensemble
                    MultiView_ETAP.SetActiveView(View_VOID)
                    For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                        With rGridView_REPE
                            If HttpUtility.HtmlDecode(.Cells(3).Text) = "PRODUIT" Or HttpUtility.HtmlDecode(.Cells(3).Text) = "PRODUIT SEMI-FINI" Then
                                Label_CD_SS_ENS.Text = .Cells(1).Text
                                'MultiView_SAIS_OPRT.SetActiveView(View_SAIS_TCBL_COMP)
                                'MultiView_Tracabilité.SetActiveView(View_SAI_SOUS_ENSE)
                                MultiView_ETAP.SetActiveView(View_SAI_SOUS_ENSE)
                                TextBox_SS_ENS.Text = ""
                                TextBox_SS_ENS.Focus()
                                Exit Sub
                            End If
                        End With
                    Next
                End If
                'si pas de sous-ensemble

                'Vérification documentaire (FCGF, DHR)
                Select Case _VRFC_DCMT()
                    Case 1
                        Exit Sub
                    Case -1
                        _ERGT_OPRT_PASS("F")
                        Throw New Exception("La vérification documentaire a échouée.")
                End Select

                If DIG_FACT_SQL_GET_PARA(Trim(Label_CD_ARTI.Text), "Scan numéro de série fin opération") = "1" Then
                    'MultiView_VLDT.SetActiveView(View_VLDT_OPRT)
                    'MultiView_SAIS_OPRT.SetActiveView(View_VLDT_OPRT)
                    MultiView_ETAP.SetActiveView(View_VLDT_OPRT)
                    TextBox_NU_SER_VLDT_OPRT.Text = ""
                    TextBox_NU_SER_VLDT_OPRT.Focus()
                Else
                    _ERGT_OPRT_PASS("P")
                End If
            Catch ex As Exception
                LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
                .Enabled = True
                .Text = ""
                .Focus()
                Exit Sub
            End Try
        End With
    End Sub

    Protected Sub TextBox_VALE_TextChanged(sender As Object, e As EventArgs) Handles TextBox_VALE.TextChanged
        Dim dt_ETAP As DataTable = Session("dt_ETAP")
        Dim i_NU_ETAP As Integer = Session("i_NU_ETAP")
        Dim sQuery As String = ""
        Try
            'Enregistrement
            Session("sQuery_Etap_Test") &= "(" & Session("ID_PSG") & ", " & dt_ETAP(i_NU_ETAP)("ID_TEST").ToString & ", " & Replace(TextBox_VALE.Text, ", ", ".") & "), "

            'comparaison
            If Convert.ToDecimal(Replace(TextBox_VALE.Text, ".", ",")) < Convert.ToDecimal(dt_ETAP(i_NU_ETAP)("NU_LIMI_IFRE_ETAP").ToString) Then Throw New Exception("La valeur saisie est en dessous de la limite inférieure")
            If Convert.ToDecimal(Replace(TextBox_VALE.Text, ".", ",")) > Convert.ToDecimal(dt_ETAP(i_NU_ETAP)("NU_LIMI_SPRE_ETAP").ToString) Then Throw New Exception("La valeur saisie est au dessus de la limite inférieure")

            'chargement de l'étape suivante
            If dt_ETAP.Rows.Count - 1 > i_NU_ETAP Then
                Session("i_NU_ETAP") = i_NU_ETAP + 1
                _AFCG_ETAP(Session("i_NU_ETAP"))
            Else 'si fini enregistrement passage ou passer à la saisie des numéros de série des sous-ensemble
                MultiView_ETAP.SetActiveView(View_VOID)
                For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                    With rGridView_REPE
                        If HttpUtility.HtmlDecode(.Cells(3).Text) = "PRODUIT" Or HttpUtility.HtmlDecode(.Cells(3).Text) = "PRODUIT SEMI-FINI" Then
                            Label_CD_SS_ENS.Text = .Cells(1).Text
                            'MultiView_SAIS_OPRT.SetActiveView(View_SAIS_TCBL_COMP)
                            'MultiView_Tracabilité.SetActiveView(View_SAI_SOUS_ENSE)
                            MultiView_ETAP.SetActiveView(View_SAI_SOUS_ENSE)
                            TextBox_SS_ENS.Text = ""
                            TextBox_SS_ENS.Focus()
                            Exit Sub
                        End If
                    End With
                Next
                'si pas de sous-ensemble
                'Vérification documentaire (FCGF, DHR)
                Select Case _VRFC_DCMT()
                    Case 1
                        Exit Sub
                    Case -1
                        _ERGT_OPRT_PASS("F")
                        Throw New Exception("La vérification documentaire a échouée.")
                End Select

                If DIG_FACT_SQL_GET_PARA(Trim(Label_CD_ARTI.Text), "Scan numéro de série fin opération") = "1" Then
                    'MultiView_VLDT.SetActiveView(View_VLDT_OPRT)
                    'MultiView_SAIS_OPRT.SetActiveView(View_VLDT_OPRT)
                    MultiView_ETAP.SetActiveView(View_VLDT_OPRT)
                    TextBox_NU_SER_VLDT_OPRT.Text = ""
                    TextBox_NU_SER_VLDT_OPRT.Focus()
                Else
                    _ERGT_OPRT_PASS("P")
                End If
            End If
            TextBox_VALE.Text = ""
        Catch ex As Exception 'si mauvais enregistrement passage
            _ERGT_OPRT_PASS("F")
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            TextBox_VALE.Text = ""
            Exit Sub
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
                MultiView_ETAP.SetActiveView(View_VOID)
                For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                    With rGridView_REPE
                        If HttpUtility.HtmlDecode(.Cells(3).Text) = "PRODUIT" Or HttpUtility.HtmlDecode(.Cells(3).Text) = "PRODUIT SEMI-FINI" Then
                            Label_CD_SS_ENS.Text = .Cells(1).Text
                            'MultiView_SAIS_OPRT.SetActiveView(View_SAIS_TCBL_COMP)
                            ' MultiView_Tracabilité.SetActiveView(View_SAI_SOUS_ENSE)
                            MultiView_ETAP.SetActiveView(View_SAI_SOUS_ENSE)
                            TextBox_SS_ENS.Text = ""
                            TextBox_SS_ENS.Focus()
                            Exit Sub
                        End If
                    End With
                Next
                'si pas de sous-ensemble

                'Vérification documentaire (FCGF, DHR)
                Select Case _VRFC_DCMT()
                    Case 1
                        Exit Sub
                    Case -1
                        _ERGT_OPRT_PASS("F")
                        Throw New Exception("La vérification documentaire a échouée.")
                End Select

                If DIG_FACT_SQL_GET_PARA(Trim(Label_CD_ARTI.Text), "Scan numéro de série fin opération") = "1" Then
                    'MultiView_VLDT.SetActiveView(View_VLDT_OPRT)
                    'MultiView_SAIS_OPRT.SetActiveView(View_VLDT_OPRT)
                    MultiView_ETAP.SetActiveView(View_VLDT_OPRT)
                    TextBox_NU_SER_VLDT_OPRT.Text = ""
                    TextBox_NU_SER_VLDT_OPRT.Focus()
                Else
                    _ERGT_OPRT_PASS("P")
                End If
            End If
        Catch ex As Exception
            TextBox_NU_SER.Enabled = True
            TextBox_NU_SER.Text = ""
            TextBox_NU_SER.Focus()
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
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
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try

    End Sub

    Public Sub _ERGT_OPRT_PASS(sSANC As String)

        Dim sQuery As String = "", sNU_SER_ECO As String = "", sNU_SER_CLIE As String = "", sNS As String = "", sID_NU_SER As String = ""
        Dim dt_PARA, dt, dt_CFGR_ARTI_ECO, dt2, dt_LIST_NU_SER_TRAC, dt_ETAT_CTRL, dtMSEG, dtAFVC, dtAFKO, dtCRHD As New DataTable
        Dim iIDTT As Integer
        Dim sPOST_TRAV As String = ""
        Dim sIPMT_ETQT As String = ""
        Try
            dt_CFGR_ARTI_ECO = DIG_FACT_SQL_CFGR_ARTI_ECO(Trim(Label_CD_ARTI.Text))
            Select Case "1"
                Case dt_CFGR_ARTI_ECO(0)("Numéro de série Eolane").ToString
                    sNU_SER_ECO = TextBox_NU_SER.Text
                Case dt_CFGR_ARTI_ECO(0)("Numéro de série client").ToString
                    sNU_SER_CLIE = TextBox_NU_SER.Text
            End Select
            sID_NU_SER = DIG_FACT_SQL_ERGT_LIAI_ID_NU_SER(Label_OF.Text, TextBox_NU_SER.Text)
            If sID_NU_SER Is Nothing Then Throw New Exception("Pas d'ID numéro de série trouvé")

            sQuery = "INSERT INTO [dbo].[DTM_TR_CPT] ([NM_NS_EOL],[NM_NS_CLT],[ID_CPT],[ID_PSG],[DT_PSG])
                                   VALUES ('" & sNU_SER_ECO & "', '" & sNU_SER_CLIE & "', '-', " & Session("ID_PSG") & ", GETDATE())"
            iIDTT = SQL_REQ_ACT_RET_IDTT(sQuery, sChaineConnexion)
            If Session("matricule") = "" Then
                LOG_MESS_UTLS(GetCurrentMethod, "Le login a été perdu. Vous allez être redirigé vers la page de login SAP. La saisie en cours sera perdue, il faudra la resaissir.", "Erreur")
                Session("UrlReferrer") = HttpContext.Current.Request.Url.AbsolutePath.ToString()
                Response.Redirect("~/Account/Login_SAP.aspx")
            End If

            'Enregistrement matériel
            For Each rGridView_LIST_MTRE As GridViewRow In GridView_LIST_MTRE.Rows
                sQuery = "INSERT INTO [dbo].[DTM_HIST_MTRE] ([ID_PSG],[ID_MTRE])
                               VALUES (" & Session("ID_PSG") & ",'" & HttpUtility.HtmlDecode(rGridView_LIST_MTRE.Cells(1).Text) & "')"
                SQL_REQ_ACT(sQuery, sChaineConnexion)
                If HttpUtility.HtmlDecode(rGridView_LIST_MTRE.Cells(0).Text) = "Poste de travail" Then sPOST_TRAV = HttpUtility.HtmlDecode(rGridView_LIST_MTRE.Cells(1).Text)
                If HttpUtility.HtmlDecode(rGridView_LIST_MTRE.Cells(0).Text) = "Imprimante étiquette" Then sIPMT_ETQT = HttpUtility.HtmlDecode(rGridView_LIST_MTRE.Cells(1).Text)
            Next

            'Enregistrement passage
            sQuery = "INSERT INTO [dbo].[DTM_PSG] ([ID_PSG], [LB_ETP], [DT_DEB], [DT_FIN], [LB_MOYN], [LB_PROG], [NM_MATR], [NM_NS_EOL], [LB_SCTN], [NM_OF], [ID_NU_SER])
                           VALUES (" & Session("ID_PSG") & ", '" & Label_DES_OP.Text & " (OP:" & Label_OP.Text & ")', '" & Session("DT_DEB") & "', GETDATE(), '" & sPOST_TRAV & "', '" & HttpContext.Current.CurrentHandler.ToString & "', '" & Session("matricule") & "', '" & sNU_SER_ECO & "', '" & sSANC & "', '" & Label_OF.Text & "', " & sID_NU_SER & ")"
            SQL_REQ_ACT(sQuery, sChaineConnexion)

            'Enregitrement test
            If Session("sQuery_Etap_Test") <> "" Then SQL_REQ_ACT(sQuery_Test & COMM_APP_WEB_STRI_TRIM_RIGHT(Session("sQuery_Etap_Test"), 2), sChaineConnexion)

            'enregistrer dans la base les associations composant
            For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                With rGridView_REPE
                    iIDTT = 0
                    If HttpUtility.HtmlDecode(.Cells(3).Text) = "PRODUIT" Or HttpUtility.HtmlDecode(.Cells(3).Text) = "PRODUIT SEMI-FINI" Then
                        sQuery = "INSERT INTO [dbo].[DTM_TR_CPT] ([NM_NS_EOL],[NM_NS_CLT],[ID_CPT],[ID_PSG],[DT_PSG],[NM_SAP_CPT],[NM_NS_SENS])
                                   VALUES ('" & sNU_SER_ECO & "', '" & sNU_SER_CLIE & "', '-', " & Session("ID_PSG") & ", GETDATE(), '" & HttpUtility.HtmlDecode(.Cells(1).Text) & "','" & HttpUtility.HtmlDecode(.Cells(6).Text) & "')"
                    Else
                        sQuery = "INSERT INTO [dbo].[DTM_TR_CPT] ([NM_NS_EOL],[NM_NS_CLT],[ID_CPT],[ID_PSG],[DT_PSG],[NM_SAP_CPT],[NM_NS_SENS])
                                   VALUES ('" & sNU_SER_ECO & "', '" & sNU_SER_CLIE & "', '" & HttpUtility.HtmlDecode(.Cells(6).Text) & "', " & Session("ID_PSG") & ", GETDATE(), '" & HttpUtility.HtmlDecode(.Cells(1).Text) & "', '" & HttpUtility.HtmlDecode(.Cells(7).Text) & "')"
                    End If
                    iIDTT = SQL_REQ_ACT_RET_IDTT(sQuery, sChaineConnexion)
                    If .Cells(5).Text <> "&nbsp;" Then
                        sQuery = "Select [ID_GST_CNTR]
                                    FROM [dbo].[V_LIST_CONT_NON_VIDE]
                                   WHERE [NM_CNTR] = '" & HttpUtility.HtmlDecode(.Cells(5).Text) & "' AND [NM_OF] = '" & TextBox_OF.Text & "'"
                        dt2 = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
                        If dt2 Is Nothing Then Throw New Exception("Le bac n°" & HttpUtility.HtmlDecode(.Cells(5).Text) & " n'existe pas ou est vide")
                        sQuery = "INSERT INTO [dbo].[DTM_TCBL_CONT_COMP] ([ID_GST_CNTR],[ID_TR])
                                   VALUES ('" & dt2(0)("ID_GST_CNTR").ToString & "', '" & iIDTT.ToString & "')"
                        SQL_REQ_ACT(sQuery, sChaineConnexion)
                    End If
                End With
            Next

            'Affichage

            'mise à jour des quantité restante
            For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                With rGridView_REPE
                    If .Cells(5).Text <> "&nbsp;" Then
                        sQuery = "SELECT [NM_QTE_INIT]
                                        ,ISNULL([NB_UTLS],0) AS NB_UTLS
                                    FROM [dbo].[V_LIST_CONT_NON_VIDE]
                                   WHERE [NM_CNTR] = '" & HttpUtility.HtmlDecode(.Cells(5).Text) & "' 
                                     AND [NM_OF] = '" & TextBox_OF.Text & "'"
                        dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
                        If dt Is Nothing Then
                            .Cells(8).Text = dt(0)("NM_QTE_INIT").ToString
                        Else
                            .Cells(8).Text = Convert.ToDecimal(dt(0)("NM_QTE_INIT").ToString) - (Convert.ToDecimal(dt(0)("NB_UTLS").ToString) * Convert.ToDecimal(.Cells(4).Text))
                        End If
                    Else
                        dtMSEG = SAP_DATA_READ_MSEG("MBLNR EQ '" & Left(.Cells(6).Text, 10) & "' AND ZEILE EQ '" & Mid(.Cells(6).Text, 11, 4) & "' AND MJAHR EQ '" & Mid(.Cells(6).Text, 15, 4) & "'")
                        If dtMSEG Is Nothing Then Continue For
                        sQuery = "SELECT COUNT([DT_PSG]) AS NB_ID_COMP
                                    FROM [dbo].[DTM_TR_CPT]
                                   WHERE [ID_CPT] LIKE '" & .Cells(6).Text & "%'
                                  GROUP BY [ID_CPT]"
                        dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
                        If dt Is Nothing Then
                            .Cells(8).Text = Convert.ToDecimal(Replace(dtMSEG(0)("MENGE").ToString, ".", ","))
                        Else
                            .Cells(8).Text = Convert.ToDecimal(Replace(dtMSEG(0)("MENGE").ToString, ".", ",")) - Convert.ToDecimal(dt(0)("NB_ID_COMP").ToString) * Convert.ToDecimal(.Cells(4).Text)
                        End If
                    End If
                End With
            Next

            'vider la gridview
            For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                With rGridView_REPE
                    If HttpUtility.HtmlDecode(.Cells(3).Text) = "PRODUIT" Or HttpUtility.HtmlDecode(.Cells(3).Text) = "PRODUIT SEMI-FINI" Then
                        .Cells(6).Text = ""
                    Else
                        If Convert.ToDecimal(HttpUtility.HtmlDecode(.Cells(8).Text)) = 0 Then
                            MultiView_SAIS_OPRT.SetActiveView(View_SAIS_TCBL_COMP)
                            Label_CD_COMP.Text = HttpUtility.HtmlDecode(.Cells(1).Text)
                        End If
                    End If
                    For Each rGridView_REPE0 As GridViewRow In GridView_REPE0.Rows
                        If HttpUtility.HtmlDecode(rGridView_REPE0.Cells(1).Text) = HttpUtility.HtmlDecode(.Cells(1).Text) Then
                            rGridView_REPE0.Cells(4).Text = HttpUtility.HtmlDecode(.Cells(4).Text)
                            rGridView_REPE0.Cells(5).Text = HttpUtility.HtmlDecode(.Cells(5).Text)
                            rGridView_REPE0.Cells(6).Text = HttpUtility.HtmlDecode(.Cells(6).Text)
                            rGridView_REPE0.Cells(7).Text = HttpUtility.HtmlDecode(.Cells(7).Text)
                            rGridView_REPE0.Cells(8).Text = HttpUtility.HtmlDecode(.Cells(8).Text)
                        End If
                    Next
                End With
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
                        With rGridView_LIST_MTRE
                            If HttpUtility.HtmlDecode(.Cells(2).Text) = "Générique" Then
                                sQuery = "INSERT INTO [dbo].[DTM_REF_PARA]([NM_CRIT],[NM_PARA],[VAL_PARA],[DT_PARA])
                                           VALUES ('" & Label_POST.Text & "','" & HttpUtility.HtmlDecode(.Cells(1).Text) & "','0',GETDATE())"
                                SQL_REQ_ACT(sQuery, sChaineConnexion)
                            End If
                        End With
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
                    'Exit Sub
                End If
                'affichage de la liste des NS tracés
                GridView_SN_TRAC.DataSource = dt_LIST_NU_SER_TRAC
                GridView_SN_TRAC.DataBind()
            End If

            MultiView_SAIS_OPRT.SetActiveView(View_SAIS_NU_SER_CHEC)
            MultiView_ETAP.SetActiveView(View_VOID)

            If sSANC = "F" Then
                Label_CONS.Text = ""
                Image_PHOT_ILST.Visible = False
                Image_PHOT_ILST.ImageUrl = ""
                Button_PASS.Visible = False
                Button_FAIL.Visible = False
                TextBox_VALE.Visible = False
                Throw New Exception("Le numéro de série " & TextBox_NU_SER.Text & " est passé mauvais.")
            Else
                'si opération d'emballage
                dtAFKO = SAP_DATA_READ_AFKO("AUFNR LIKE '%" & TextBox_OF.Text & "'")
                'Extraction de la gamme
                dtAFVC = SAP_DATA_READ_AFVC("AUFPL EQ '" & dtAFKO(0)("AUFPL").ToString & "' AND VORNR EQ '" & DropDownList_OP.SelectedValue & "'")
                dtCRHD = SAP_DATA_READ_CRHD("OBJID EQ '" & dtAFVC(0)("ARBID").ToString & "' AND ARBPL LIKE 'EMB%'")
                If Not dtCRHD Is Nothing Then
                    dt_ETAT_CTRL = COMM_APP_WEB_ETAT_CTRL(Trim(Label_CD_ARTI.Text) & "|Carton", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx")
                    DIG_FACT_IMPR_ETIQ_V2(dt_ETAT_CTRL(0)("TextBox_FICH_MODE").ToString,
                                          sIPMT_ETQT,
                                          TextBox_OF.Text,
                                          "",
                                          "Carton",
                                          sID_NU_SER,
                                          "",
                                          "",
                                          "",
                                          Nothing)
                End If
                LOG_MESS_UTLS(GetCurrentMethod, "Le numéro de série " & TextBox_NU_SER.Text & " est passé bon.")
                With TextBox_NU_SER
                    .Enabled = True
                    .Text = ""
                    .Focus()
                End With
            End If
        Catch ex As Exception
            With TextBox_NU_SER
                .Enabled = True
                .Text = ""
                .Focus()
            End With
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
            'Finally
            '    GridView_REPE0.DataSource = GridView_REPE.DataSource
            '    GridView_REPE0.DataBind()
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
                sfich = "c:\sources\app_web\PagesMembres\Digital_Factory\photo_" & CInt(Int((1000 * Rnd()) + 1)) & System.IO.Path.GetExtension(dt_ETAP(i_NU_ETAP)("NM_CHEM_PHOT_ETAP").ToString)
                COMM_APP_WEB_COPY_FILE(dt_ETAP(i_NU_ETAP)("NM_CHEM_PHOT_ETAP").ToString, sfich, True)
                Image_PHOT_ILST.ImageUrl = System.IO.Path.GetFileName(sfich)
            End If
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
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
            If dt_ETAP Is Nothing Then Throw New Exception("Pas de config étape")
            Return dt_ETAP
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try
    End Function

    Public Function _AFCG_POST_NMCT_TPLG() As String
        Dim sQuery As String = "", sQuery_MTRE As String = ""
        Dim dt_POST_NMCT_TPLG As New DataTable
        Try
            'sQuery_MTRE = "SELECT ISNULL(B.VAL_PARA, NM_TYPE_MTRE) AS [Type de matériel]
            '                   ,ID_MTRE AS [ID du matériel]
            '                      ,ISNULL(B_C.VAL_PARA,'Générique') AS [Catégorie]
            '                  FROM (
            '                  SELECT NM_POST, ID_MTRE, VAL_PARA
            '                    FROM (
            '                    SELECT     NM_PARA AS ID_MTRE, CD_ARTI AS NM_POST
            '                      FROM          dbo.V_DER_DTM_REF_PARA
            '                     WHERE      (CD_ARTI = '" & Label_POST.Text & "') AND (VAL_PARA = '1')
            '                       ) AS DT_MTRE_ERGT INNER JOIN
            '                      dbo.V_DER_DTM_REF_PARA AS V_DER_DTM_REF_PARA_1 ON DT_MTRE_ERGT.ID_MTRE = V_DER_DTM_REF_PARA_1.CD_ARTI
            '                   WHERE     (V_DER_DTM_REF_PARA_1.NM_PARA = N'Type matériel')) AS B
            '                FULL OUTER JOIN (
            '                  SELECT CD_ARTI AS ID_POST, 
            '                      NM_TYPE_MTRE
            '                   FROM (
            '                    SELECT     CD_ARTI, NM_PARA, VAL_PARA
            '                    FROM         dbo.V_DER_DTM_REF_PARA
            '                    WHERE     (CD_ARTI LIKE N'POSTE_%') AND 
            '                        (NM_PARA = N'Article poste' OR NM_PARA = N'Opération poste')
            '                     ) as a 
            '                  pivot (max(VAL_PARA) for NM_PARA in ([Article poste], [Opération poste])) as pt
            '                  INNER JOIN dbo.V_POST_NMCT_TPLG ON dbo.V_POST_NMCT_TPLG.CD_ARTI_ECO_POST = pt.[Article poste] AND dbo.V_POST_NMCT_TPLG.NU_OP_POST = pt.[Opération poste]
            '                  WHERE CD_ARTI = '" & Label_POST.Text & "'
            '                ) AS A ON ID_POST = NM_POST AND VAL_PARA = NM_TYPE_MTRE
            '                LEFT OUTER JOIN (SELECT CD_ARTI, VAL_PARA
            '       FROM [dbo].[V_DER_DTM_REF_PARA]
            '      WHERE NM_PARA = 'Poste dédié') AS B_C ON CD_ARTI = ID_MTRE"
            sQuery_MTRE = "SELECT [NM_TYPE_MTRE] AS [Type de matériel], '' AS [ID du matériel]
                             FROM [dbo].[V_POST_NMCT_TPLG]
                            WHERE [CD_ARTI_ECO_POST] = '" & Label_CD_ARTI.Text & "' AND [NU_OP_POST] = '" & Label_OP.Text & "'"

            dt_POST_NMCT_TPLG = SQL_SELE_TO_DT(sQuery_MTRE, sChaineConnexion)
            If dt_POST_NMCT_TPLG Is Nothing Then Throw New Exception("Pas de poste type configuré dans la base, prévenir un méthode")
            'Dissociation du matériel générique
            For Each rdt_POST_NMCT_TPLG As DataRow In dt_POST_NMCT_TPLG.Rows
                'If rdt_POST_NMCT_TPLG("Catégorie").ToString = "Générique" Then
                '    sQuery = "INSERT INTO [dbo].[DTM_REF_PARA]([NM_CRIT],[NM_PARA],[VAL_PARA],[DT_PARA])
                '                   VALUES ('" & Label_POST.Text & "','" & rdt_POST_NMCT_TPLG("ID du matériel").ToString & "','0',GETDATE())"
                '    SQL_REQ_ACT(sQuery, sChaineConnexion)
                'pc
                If rdt_POST_NMCT_TPLG("Type de matériel").ToString = "PC" Then
                        'sQuery = "INSERT INTO [dbo].[DTM_REF_PARA]([NM_CRIT],[NM_PARA],[VAL_PARA],[DT_PARA])
                        '           VALUES ('" & Label_POST.Text & "','" & System.Net.Dns.GetHostEntry(System.Web.HttpContext.Current.Request.UserHostAddress).HostName() & "','1',GETDATE())"
                        'SQL_REQ_ACT(sQuery, sChaineConnexion)
                        rdt_POST_NMCT_TPLG("ID du matériel") = System.Net.Dns.GetHostEntry(System.Web.HttpContext.Current.Request.UserHostAddress).HostName()
                    End If
                'End If
            Next

            'dt_POST_NMCT_TPLG = SQL_SELE_TO_DT(sQuery_MTRE, sChaineConnexion)
            GridView_LIST_MTRE.DataSource = dt_POST_NMCT_TPLG
            GridView_LIST_MTRE.DataBind()

            For Each rGridView_LIST_MTRE As GridViewRow In GridView_LIST_MTRE.Rows
                With rGridView_LIST_MTRE
                    If .Cells(1).Text = "&nbsp;" Then
                        Label_TYPE_MTRE.Text = HttpUtility.HtmlDecode(.Cells(0).Text)
                        TextBox_MTRE_GNRQ.Focus()
                        MultiView_SAIS_OPRT.SetActiveView(View_SAIS_OTLG)
                        MultiView_POST.SetActiveView(View_SAIS_POST_MTRE_GNRQ)
                        Return "1"
                    End If
                End With
            Next

            Return "0"
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Return "0"
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
        Dim iID_Passage As Integer = 0
        Dim dt_PARA, dt, dt_CFGR_ARTI_ECO As New DataTable
        With TextBox_SS_ENS
            Try
                'vérifier que le ns est disponible
                sQuerySql = "SELECT   NM_NS_SENS
                               FROM   dbo.DTM_TR_CPT
                              WHERE   NM_NS_SENS = '" & .Text & "'"
                dt_TR_CPT = SQL_SELE_TO_DT(sQuerySql, sChaineConnexion)
                If Not dt_TR_CPT Is Nothing Then Throw New Exception("Le numéro de série " & .Text & " est déjà attribué dans la base.")

                'Extraction code article d'un numéro de série Eolane de SAP
                dtAFKO = SAP_DATA_READ_AFKO("AUFNR LIKE '%" & Left(.Text, 7) & "'")
                If Not dtAFKO Is Nothing Then
                    For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                        'attribution du code article si dans la liste
                        If Trim(rGridView_REPE.Cells(1).Text) = Trim(dtAFKO(0)("PLNBEZ").ToString) Then Label_CD_SS_ENS.Text = dtAFKO(0)("PLNBEZ").ToString
                    Next
                End If

                'Vérifier le format du numéro de série
                dt_CFGR_ARTI_ECO = DIG_FACT_SQL_CFGR_ARTI_ECO(Trim(Label_CD_SS_ENS.Text))
                If dt_CFGR_ARTI_ECO(0)("Format Numéro de série Fournisseur").ToString <> "" Then
                    If DIG_FACT_VERI_FORM_NU_SER(Trim(Label_CD_SS_ENS.Text), "Format Numéro de série Fournisseur", .Text) = False Then Throw New Exception("Le numéro de série " & .Text & " ne correspond au format défini dans la base.")
                Else
                    Select Case "1"
                        Case dt_CFGR_ARTI_ECO(0)("Numéro de série Eolane").ToString
                            If DIG_FACT_VERI_FORM_NU_SER(Trim(Label_CD_SS_ENS.Text), "Format Numéro de série Eolane", .Text) = False Then Throw New Exception("Le numéro de série " & .Text & " ne correspond au format défini dans la base.")
                        Case dt_CFGR_ARTI_ECO(0)("Numéro de série client").ToString
                            If DIG_FACT_VERI_FORM_NU_SER(Trim(Label_CD_SS_ENS.Text), "Format Numéro de série client", .Text) = False Then Throw New Exception("Le numéro de série " & .Text & " ne correspond au format défini dans la base.")
                    End Select
                End If
                'Sauvegarder la donnée dans la colonne id composant
                For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                    If Label_CD_SS_ENS.Text = rGridView_REPE.Cells(1).Text Then rGridView_REPE.Cells(6).Text = .Text
                Next

                'saisir le n° de série du prochain sous-ensemble
                For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                    With rGridView_REPE
                        LOG_Msg(GetCurrentMethod, .Cells(6).Text)
                        If (.Cells(6).Text = "&nbsp;" Or .Cells(6).Text = "") And (HttpUtility.HtmlDecode(.Cells(3).Text) = "PRODUIT" Or HttpUtility.HtmlDecode(.Cells(3).Text) = "PRODUIT SEMI-FINI") Then
                            'MultiView_Tracabilité.SetActiveView(View_SAI_SOUS_ENSE)
                            MultiView_ETAP.SetActiveView(View_SAI_SOUS_ENSE)
                            GridView_REPE.SelectRow(.RowIndex)
                            Label_CD_SS_ENS.Text = GridView_REPE.SelectedRow.Cells(1).Text
                            TextBox_SS_ENS.Text = ""
                            TextBox_SS_ENS.Focus()
                            'GridView_REPE0.DataSource = GridView_REPE.DataSource
                            'GridView_REPE0.DataBind()
                            Exit Sub
                        End If
                    End With
                Next

                'Vérification documentaire (FCGF, DHR)
                Select Case _VRFC_DCMT()
                    Case 1
                        Exit Sub
                    Case -1
                        _ERGT_OPRT_PASS("F")
                        Throw New Exception("La vérification documentaire a échouée.")
                End Select

                'si terminé enregistrer les données
                If DIG_FACT_SQL_GET_PARA(Trim(Label_CD_ARTI.Text), "Scan numéro de série fin opération") = "1" Then
                    'MultiView_VLDT.SetActiveView(View_VLDT_OPRT)
                    'MultiView_SAIS_OPRT.SetActiveView(View_VLDT_OPRT)
                    MultiView_ETAP.SetActiveView(View_VLDT_OPRT)
                    TextBox_NU_SER_VLDT_OPRT.Text = ""
                    TextBox_NU_SER_VLDT_OPRT.Focus()
                Else
                    _ERGT_OPRT_PASS("P")
                End If

            Catch ex As Exception
                LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
                'MultiView_Tracabilité.SetActiveView(View_SAI_SOUS_ENSE)
                MultiView_ETAP.SetActiveView(View_SAI_SOUS_ENSE)
                .Text = ""
                .Focus()
                Exit Sub
                'Finally
                '    dt = GridView_REPE.DataSource
                '    GridView_REPE0.DataSource = dt
                '    GridView_REPE0.DataBind()
            End Try
        End With
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
                    With rGridView_REPE
                        If .Cells(8).Text = "&nbsp;" And .Cells(3).Text = "&nbsp;" Then
                            MultiView_SAIS_CD_LOT.SetActiveView(View_SAIS_CD_LOT_SAIS)
                            GridView_REPE.SelectRow(.RowIndex)
                            Label_CD_COMP.Text = GridView_REPE.SelectedRow.Cells(1).Text
                            TextBox_CD_LOT_COMP.Focus()
                            Exit Sub
                        End If
                    End With
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
                With rGridView_REPE
                    If dtMSEG(0)("MATNR").ToString = HttpUtility.HtmlDecode(.Cells(1).Text) Then
                        .Cells(5).Text = TextBox_NU_BAC.Text
                        .Cells(6).Text = dt(0)("ID_CPT").ToString
                        .Cells(7).Text = dtMSEG(0)("CHARG").ToString
                        .Cells(8).Text = Convert.ToDecimal(dt(0)("NM_QTE_INIT").ToString) - (Convert.ToDecimal(dt(0)("NB_UTLS").ToString) * Convert.ToDecimal(.Cells(4).Text))
                    End If
                    For Each rGridView_REPE0 As GridViewRow In GridView_REPE0.Rows
                        If HttpUtility.HtmlDecode(rGridView_REPE0.Cells(1).Text) = HttpUtility.HtmlDecode(.Cells(1).Text) Then
                            rGridView_REPE0.Cells(4).Text = HttpUtility.HtmlDecode(.Cells(4).Text)
                            rGridView_REPE0.Cells(5).Text = HttpUtility.HtmlDecode(.Cells(5).Text)
                            rGridView_REPE0.Cells(6).Text = HttpUtility.HtmlDecode(.Cells(6).Text)
                            rGridView_REPE0.Cells(7).Text = HttpUtility.HtmlDecode(.Cells(7).Text)
                            rGridView_REPE0.Cells(8).Text = HttpUtility.HtmlDecode(.Cells(8).Text)
                        End If
                    Next
                End With
            Next

            For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                With rGridView_REPE
                    If .Cells(8).Text = "&nbsp;" And Not (HttpUtility.HtmlDecode(.Cells(3).Text) = "PRODUIT" Or HttpUtility.HtmlDecode(.Cells(3).Text = "PRODUIT SEMI-FINI")) Then
                        TextBox_NU_BAC.Text = ""
                        TextBox_NU_BAC.Focus()
                        Exit Sub
                    End If
                End With
            Next
            MultiView_SAIS_OPRT.SetActiveView(View_SAIS_NU_SER_CHEC)
            'MultiView_Tracabilité.SetActiveView(View_VOID_2)
            With TextBox_NU_SER
                .Enabled = True
                .Text = ""
                .Focus()
            End With
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            TextBox_NU_BAC.Text = ""
            TextBox_NU_BAC.Focus()
            Exit Sub
            'Finally
            '    dt = GridView_REPE.DataSource
            '    GridView_REPE0.DataSource = dt
            '    GridView_REPE0.DataBind()
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
                With rGridView_REPE
                    If dtMSEG(0)("MATNR").ToString = HttpUtility.HtmlDecode(.Cells(1).Text) Then
                        .Cells(6).Text = TextBox_ID_COMP.Text
                        .Cells(7).Text = dtMSEG(0)("CHARG").ToString
                        sQuery = "SELECT COUNT([DT_PSG]) AS NB_ID_COMP
                                   FROM [dbo].[DTM_TR_CPT]
                                  WHERE [ID_CPT] LIKE '" & TextBox_ID_COMP.Text & "%'
                                 GROUP BY [ID_CPT]"
                        dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
                        If dt Is Nothing Then
                            .Cells(8).Text = Convert.ToDecimal(Replace(dtMSEG(0)("MENGE").ToString, ".", ","))
                        Else
                            fQT_UTLS = Convert.ToDecimal(dt(0)("NB_ID_COMP").ToString) * Convert.ToDecimal(.Cells(4).Text)
                            If fQT_UTLS < Convert.ToDecimal(Replace(dtMSEG(0)("MENGE").ToString, ".", ",")) Then .Cells(8).Text = Convert.ToDecimal(Replace(dtMSEG(0)("MENGE").ToString, ".", ",")) - fQT_UTLS
                        End If
                    End If
                    For Each rGridView_REPE0 As GridViewRow In GridView_REPE0.Rows
                        If HttpUtility.HtmlDecode(rGridView_REPE0.Cells(1).Text) = HttpUtility.HtmlDecode(.Cells(1).Text) Then
                            rGridView_REPE0.Cells(4).Text = HttpUtility.HtmlDecode(.Cells(4).Text)
                            rGridView_REPE0.Cells(5).Text = HttpUtility.HtmlDecode(.Cells(5).Text)
                            rGridView_REPE0.Cells(6).Text = HttpUtility.HtmlDecode(.Cells(6).Text)
                            rGridView_REPE0.Cells(7).Text = HttpUtility.HtmlDecode(.Cells(7).Text)
                            rGridView_REPE0.Cells(8).Text = HttpUtility.HtmlDecode(.Cells(8).Text)
                        End If
                    Next
                End With
            Next

            For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                With rGridView_REPE
                    If .Cells(8).Text = "&nbsp;" And Not (HttpUtility.HtmlDecode(.Cells(3).Text) = "PRODUIT" Or HttpUtility.HtmlDecode(.Cells(3).Text) = "PRODUIT SEMI-FINI") Then
                        TextBox_ID_COMP.Text = ""
                        TextBox_ID_COMP.Focus()
                        Exit Sub
                    End If
                End With
            Next
            MultiView_SAIS_OPRT.SetActiveView(View_SAIS_NU_SER_CHEC)
            'MultiView_Tracabilité.SetActiveView(View_VOID_2)
            With TextBox_NU_SER
                .Enabled = True
                .Text = ""
                .Focus()
            End With
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            TextBox_ID_COMP.Text = ""
            TextBox_ID_COMP.Focus()
            Exit Sub
            'Finally
            '    dt = GridView_REPE.DataSource
            '    GridView_REPE0.DataSource = dt
            '    GridView_REPE0.DataBind()
        End Try
    End Sub

    Protected Sub TextBox_CD_LOT_COMP_TextChanged(sender As Object, e As EventArgs) Handles TextBox_CD_LOT_COMP.TextChanged
        Dim sQuery As String = ""
        Dim dtMSEG, dt As New DataTable
        Dim fQT_UTLS As Decimal
        Try
            For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                With rGridView_REPE
                    If Label_CD_COMP.Text = HttpUtility.HtmlDecode(.Cells(1).Text) Then
                        dtMSEG = SAP_DATA_READ_MSEG("MATNR EQ '" & Label_CD_COMP.Text & "' AND CHARG EQ '" & TextBox_CD_LOT_COMP.Text & "'")
                        If dtMSEG Is Nothing Then Throw New Exception("Pas de données")
                        For Each rdtMSEG As DataRow In dtMSEG.Rows
                            sQuery = "SELECT COUNT([DT_PSG]) AS NB_ID_COMP
                                        FROM [dbo].[DTM_TR_CPT]
                                       WHERE [ID_CPT] LIKE '" & rdtMSEG("MBLNR").ToString & rdtMSEG("ZEILE").ToString & rdtMSEG("MJAHR").ToString & "%'
                                      GROUP BY [ID_CPT]"
                            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
                            If dt Is Nothing Then
                                .Cells(8).Text = Convert.ToDecimal(Replace(rdtMSEG("MENGE").ToString, ".", ","))
                                .Cells(6).Text = rdtMSEG("MBLNR").ToString & rdtMSEG("ZEILE").ToString & rdtMSEG("MJAHR").ToString
                                Exit For
                            Else
                                fQT_UTLS = Convert.ToDecimal(dt(0)("NB_ID_COMP").ToString) * Convert.ToDecimal(.Cells(4).Text)
                                If fQT_UTLS < Convert.ToDecimal(Replace(dtMSEG(0)("MENGE").ToString, ".", ",")) Then
                                    .Cells(8).Text = Convert.ToDecimal(Replace(rdtMSEG("MENGE").ToString, ".", ",")) - fQT_UTLS
                                    .Cells(6).Text = rdtMSEG("MBLNR").ToString & rdtMSEG("ZEILE").ToString & rdtMSEG("MJAHR").ToString
                                    Exit For
                                End If
                            End If
                        Next
                        .Cells(7).Text = TextBox_CD_LOT_COMP.Text
                    End If
                    For Each rGridView_REPE0 As GridViewRow In GridView_REPE0.Rows
                        If HttpUtility.HtmlDecode(rGridView_REPE0.Cells(1).Text) = HttpUtility.HtmlDecode(.Cells(1).Text) Then
                            rGridView_REPE0.Cells(4).Text = HttpUtility.HtmlDecode(.Cells(4).Text)
                            rGridView_REPE0.Cells(5).Text = HttpUtility.HtmlDecode(.Cells(5).Text)
                            rGridView_REPE0.Cells(6).Text = HttpUtility.HtmlDecode(.Cells(6).Text)
                            rGridView_REPE0.Cells(7).Text = HttpUtility.HtmlDecode(.Cells(7).Text)
                            rGridView_REPE0.Cells(8).Text = HttpUtility.HtmlDecode(.Cells(8).Text)
                        End If
                    Next
                End With
            Next

            For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                With rGridView_REPE
                    If .Cells(8).Text = "&nbsp;" And Not (HttpUtility.HtmlDecode(.Cells(3).Text) = "PRODUIT" Or HttpUtility.HtmlDecode(.Cells(3).Text) = "PRODUIT SEMI-FINI") Then
                        MultiView_SAIS_CD_LOT.SetActiveView(View_SAIS_CD_LOT_SAIS)
                        GridView_REPE.SelectRow(.RowIndex)
                        Label_CD_COMP.Text = GridView_REPE.SelectedRow.Cells(1).Text
                        TextBox_CD_LOT_COMP.Text = ""
                        TextBox_CD_LOT_COMP.Focus()
                        Exit Sub
                    End If
                End With
            Next
            MultiView_SAIS_OPRT.SetActiveView(View_SAIS_NU_SER_CHEC)
            'MultiView_Tracabilité.SetActiveView(View_VOID_2)
            With TextBox_NU_SER
                .Enabled = True
                .Text = ""
                .Focus()
            End With
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            TextBox_CD_LOT_COMP.Text = ""
            TextBox_CD_LOT_COMP.Focus()
            Exit Sub
            'Finally
            '    dt = GridView_REPE.DataSource
            '    GridView_REPE0.DataSource = dt
            '    GridView_REPE0.DataBind()
        End Try
    End Sub

    Public Sub _IPRO_ETQT(Optional sNS As String = "")
        Dim dt_CFGR_ARTI_ECO, dt_ETAT_CTRL, dt As New DataTable
        Dim sIPMT_ETQT As String = "", sQuery As String = ""
        Try
            dt_CFGR_ARTI_ECO = DIG_FACT_SQL_CFGR_ARTI_ECO(Trim(Label_CD_ARTI.Text))
            For Each rGridView_LIST_MTRE As GridViewRow In GridView_LIST_MTRE.Rows
                If HttpUtility.HtmlDecode(rGridView_LIST_MTRE.Cells(0).Text) = "Imprimante étiquette" Then sIPMT_ETQT = HttpUtility.HtmlDecode(rGridView_LIST_MTRE.Cells(1).Text)
            Next

            If dt_CFGR_ARTI_ECO(0)("Génération impression numéro de série").ToString = Label_OP.Text Then
                Select Case "1"
                    Case dt_CFGR_ARTI_ECO(0)("Numéro de série Eolane").ToString
                        dt_ETAT_CTRL = COMM_APP_WEB_ETAT_CTRL(Trim(Label_CD_ARTI.Text) & "|Numéro de série Eolane", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx")
                        If dt_ETAT_CTRL Is Nothing Then Throw New Exception("La base App_Web_Eco n'as pas été configurée pour l'article " & Trim(Label_CD_ARTI.Text))
                        If sNS = "" Then sNS = Label_OF.Text & DIG_FACT_SQL_GENE_NU_SER_CLIE("10",
                                                                                             "1",
                                                                                             "%%%%%%",
                                                                                             Label_OF.Text,
                                                                                             "Numéro de série Eolane",
                                                                                             False,
                                                                                             Session("matricule"),
                                                                                             Label_OF.Text)
                        sQuery = "SELECT [ID_NU_SER]
	                            FROM [dbo].[V_LIAIS_NU_SER]
	                           WHERE [NU_SER_ECO] LIKE '%" & sNS & "%'	                    
	                             AND [NU_OF] = '" & Label_OF.Text & "'"
                        dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)

                        If dt_ETAT_CTRL.Columns.Contains("TextBox_FICH_MODE") Then
                            DIG_FACT_IMPR_ETIQ_V2(dt_ETAT_CTRL(0)("TextBox_FICH_MODE").ToString,
                                          sIPMT_ETQT,
                                          TextBox_OF.Text,
                                          "",
                                          "Numéro de série Eolane",
                                          dt(0)("ID_NU_SER").ToString,
                                          "",
                                          "",
                                          "",
                                          Nothing)
                            'DIG_FACT_IMPR_ETIQ(dt_ETAT_CTRL(0)("TextBox_FICH_MODE").ToString,
                            '                   TextBox_OF.Text, "", "Numéro de série Eolane", "", sNS, "", "", "", Nothing)
                        End If
                    Case dt_CFGR_ARTI_ECO(0)("Numéro de série client").ToString
                        Dim sFORM_NU_CLIE As String = ""
                        dt_ETAT_CTRL = COMM_APP_WEB_ETAT_CTRL(Trim(Label_CD_ARTI.Text) & "|Numéro de série client", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx")
                        If dt_ETAT_CTRL Is Nothing Then Throw New Exception("La base App_Web_Eco n'as pas été configurée pour l'article " & Trim(Label_CD_ARTI.Text))
                        If sNS = "" Then
                            If dt_ETAT_CTRL.Columns.Contains("TextBox_FORM_NU_CLIE") = True Then sFORM_NU_CLIE = dt_ETAT_CTRL(0)("TextBox_FORM_NU_CLIE").ToString
                            sNS = DIG_FACT_SQL_GENE_NU_SER_CLIE(dt_CFGR_ARTI_ECO(0)("Encodage Numéro de série client").ToString,
                                                                dt_CFGR_ARTI_ECO(0)("Incrémentation flanc").ToString,
                                                                sFORM_NU_CLIE,
                                                                Label_CD_ARTI.Text,
                                                                "Numéro de série client",
                                                                False,
                                                                Session("matricule"),
                                                                Label_OF.Text)
                        End If
                        sQuery = "SELECT [ID_NU_SER]
	                            FROM [dbo].[V_LIAIS_NU_SER]
	                           WHERE [NU_SER_CLIE] = '" & sNS & "'
	                             AND [NU_OF] = '" & Label_OF.Text & "'"
                        dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)

                        If dt_ETAT_CTRL.Columns.Contains("TextBox_FICH_MODE") Then
                            DIG_FACT_IMPR_ETIQ_V2(dt_ETAT_CTRL(0)("TextBox_FICH_MODE").ToString,
                                          sIPMT_ETQT,
                                          TextBox_OF.Text,
                                          "",
                                          "Numéro de série client",
                                          dt(0)("ID_NU_SER").ToString,
                                          "",
                                          "",
                                          "",
                                          Nothing)
                            'DIG_FACT_IMPR_ETIQ(dt_ETAT_CTRL(0)("TextBox_FICH_MODE").ToString,
                            '                   TextBox_OF.Text, "", "Numéro de série client", sNS, "", "", "", "", Nothing)
                        End If
                End Select
                Session("sNU_SER_IMPR") = sNS
            End If
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try

    End Sub

    Public Sub _VRFC_TCBL_COMP()
        Dim dt_SS_ENS, dtAFKO, dtVRESB, dtSTPO, dtSTPU As New DataTable
        Dim rdt_CD_ARTI_ENS_SENS_SAP As DataRow
        Dim i_COUN_VRFC_PROD_SS_ENS As Integer = 0
        Try
            dtAFKO = SAP_DATA_READ_AFKO("AUFNR LIKE '%" & TextBox_OF.Text & "'")
            If dtAFKO Is Nothing Then Throw New Exception("L'OF n°" & TextBox_OF.Text & " n'a pas été trouvé dans SAP.")

            dt_SS_ENS = App_Web.TCBL_ESB_SS_ESB_V2._LIST_ENS_SS_ENS(TextBox_OF.Text, DropDownList_OP.SelectedValue)
            If Not dt_SS_ENS Is Nothing Then 'traçabilité composant à faire
                With dt_SS_ENS.Columns
                    .Remove("Numéro de série associé")
                    .Add("Repère", Type.GetType("System.String"))
                    .Add("Quantité par produit", Type.GetType("System.String"))
                    .Add("N° de conteneur", Type.GetType("System.String"))
                    .Add("Id composant", Type.GetType("System.String"))
                    .Add("Code lot", Type.GetType("System.String"))
                    .Add("Quantité restante", Type.GetType("System.String"))
                End With
                dtVRESB = SAP_DATA_READ_VRESB("RSNUM EQ '" & dtAFKO(0)("RSNUM").ToString & "' AND SPRAS EQ 'F' AND VORNR EQ '" & DropDownList_OP.SelectedValue & "'")
                For Each rVRESB As DataRow In dtVRESB.Rows
                    rdt_CD_ARTI_ENS_SENS_SAP = dt_SS_ENS.Select("[Code article SAP] = '" & rVRESB("MATNR").ToString & "'").FirstOrDefault
                    If rdt_CD_ARTI_ENS_SENS_SAP Is Nothing Then Continue For

                    'Vérification saisie numéro de série (pour sous-ensemble non-déclaré en produit dans et SAP et nécessité de saisir le n° de série)
                    If DIG_FACT_SQL_GET_PARA(Trim(rVRESB("MATNR").ToString), "Sérialisation article") = "1" Then rdt_CD_ARTI_ENS_SENS_SAP("Repère") = "PRODUIT"

                    dtSTPO = SAP_DATA_READ_STPO("STLNR EQ '" & rVRESB("STLNR").ToString & "' and STLKN EQ '" & rVRESB("STLKN").ToString & "' and STPOZ EQ '" & rVRESB("STPOZ").ToString & "'")
                    If dtSTPO Is Nothing Then
                        rdt_CD_ARTI_ENS_SENS_SAP("Quantité par produit") = Convert.ToDecimal(Replace(Label_QT_OF.Text, ".", ",")) / Convert.ToDecimal(Replace(rVRESB("BDMNG").ToString, ".", ","))
                    Else
                        rdt_CD_ARTI_ENS_SENS_SAP("Quantité par produit") = Convert.ToDecimal(Replace(dtSTPO(0)("MENGE").ToString, ".", ","))
                    End If
                    Select Case rVRESB("MTART").ToString
                        Case "FERT"
                            rdt_CD_ARTI_ENS_SENS_SAP("Repère") = "PRODUIT"
                        Case "HALB"
                            rdt_CD_ARTI_ENS_SENS_SAP("Repère") = "PRODUIT SEMI-FINI"
                        Case Else 'recherche du repère
                            dtSTPU = SAP_DATA_READ_STPU("STLNR EQ '" & rVRESB("STLNR").ToString & "' and STLKN EQ '" & rVRESB("STLKN").ToString & "' and STPOZ EQ '" & rVRESB("STPOZ").ToString & "'")
                            If dtSTPU Is Nothing Then Continue For
                            For Each rdtSTPU As DataRow In dtSTPU.Rows
                                rdt_CD_ARTI_ENS_SENS_SAP("Repère") &= rdtSTPU("EBORT").ToString & "|"
                            Next
                            rdt_CD_ARTI_ENS_SENS_SAP("Repère") = COMM_APP_WEB_STRI_TRIM_RIGHT(rdt_CD_ARTI_ENS_SENS_SAP("Repère"), 1)
                    End Select
                Next
                GridView_REPE.DataSource = dt_SS_ENS
                GridView_REPE.DataBind()
                GridView_REPE0.DataSource = dt_SS_ENS
                GridView_REPE0.DataBind()

                's'il y a des sous-ensemble de type différent de produit ou semi-fini alors saisir les codes lot
                For Each rGridView_REPE As GridViewRow In GridView_REPE.Rows
                    With rGridView_REPE
                        If Not (HttpUtility.HtmlDecode(.Cells(3).Text) = "PRODUIT" Or HttpUtility.HtmlDecode(.Cells(3).Text) = "PRODUIT SEMI-FINI") Then
                            MultiView_SAIS_OPRT.SetActiveView(View_SAIS_TCBL_COMP)
                            ' MultiView_Tracabilité.SetActiveView(View_VOID_2)
                            Exit Sub
                        End If
                    End With
                Next
            End If
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try
    End Sub

    Protected Sub Button_VALI_POST_Click(sender As Object, e As EventArgs) Handles Button_VALI_POST.Click
        Dim sQuery As String = ""
        Dim dt_POST_NMCT_TPLG As New DataTable
        Try
            Label_POST.Text = DropDownList_POST.SelectedValue
            'affiche config dédiée + extraction nomenclature type pour matériel générique
            If _AFCG_POST_NMCT_TPLG() = "1" Then Exit Sub

            'si pas de saisie de matériel générique Extraire la liste des étapes de l'opération
            Session("dt_ETAP") = _ETCT_ETAP_OPRT()
            MultiView_SAIS_OPRT.SetActiveView(View_SAIS_NU_SER_CHEC)

            'Si la TextBox de génération des étiquettes est cochée
            _IPRO_ETQT()

            'vérification si traçabilité composant à faire
            _VRFC_TCBL_COMP()
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try
    End Sub

    Protected Sub TextBox_NU_SER_VLDT_OPRT_TextChanged(sender As Object, e As EventArgs) Handles TextBox_NU_SER_VLDT_OPRT.TextChanged
        If TextBox_NU_SER_VLDT_OPRT.Text = TextBox_NU_SER.Text Then
            _ERGT_OPRT_PASS("P")
        Else
            TextBox_NU_SER_VLDT_OPRT.Focus()
            TextBox_NU_SER_VLDT_OPRT.Text = ""
            LOG_MESS_UTLS(GetCurrentMethod, "Incohérence de numéro de série scanné.", "Erreur")
        End If
    End Sub

    'Protected Sub GridView_REPE0_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GridView_REPE0.SelectedIndexChanged
    '    Label_CD_SS_ENS.Text = GridView_REPE0.SelectedRow.Cells(1).Text
    '    'MultiView_Tracabilité.SetActiveView(View_SAI_SOUS_ENSE)
    '    MultiView_ETAP.SetActiveView(View_SAI_SOUS_ENSE)
    '    TextBox_SS_ENS.Focus()
    'End Sub

    Public Function CREA_FCGF(sNU_SER As String, sOF As String) As String
        Dim dt, dt_PHAS, dt_SEQU, dtKNMT, dt_RESU, dtZMOYENTYPETEST As New DataTable
        Dim sQuery As String = "", sList_inci As String = "Liste des numéros d'incident : "
        'Dim dPDF As Document
        Dim pt As PdfPTable
        Dim c_ENTE As PdfPCell
        Randomize()
        Dim sfich As String = “C:\sources\temp_App_Web\" & CInt(Int((10000000 * Rnd()) + 1)) & ".pdf”
        ' Dim iLOGO As Image
        Dim para As Paragraph
        ' Dim ipagenumber As Integer
        Try
            Using dPDF = New Document(PageSize.A4, 20, 20, 30, 20)

                Dim writer = PdfAWriter.GetInstance(dPDF, New FileStream(sfich, FileMode.Create))
                Dim PageEventHandler As New HeaderFooter_FCGF()
                writer.PageEvent = PageEventHandler
                Session("sNU_SER") = sNU_SER
                sQuery = "SELECT [NM_RFRC_FDV]
      ,[NU_VERS_FDV]
  FROM [dbo].[V_FCGF]
 WHERE [NU_SER_SPFE] = '" & sNU_SER & "'
GROUP BY [NM_RFRC_FDV]
      ,[NU_VERS_FDV]"
                dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion_ALMS)
                Session("sFDV") = dt(0)("NM_RFRC_FDV").ToString
                Session("sVers_FDV") = dt(0)("NU_VERS_FDV").ToString
                dtKNMT = SAP_DATA_READ_KNMT("MATNR LIKE '" & Label_CD_ARTI.Text & "%' AND KUNNR EQ '0000000451' AND VKORG EQ 'ORC3' AND VTWEG EQ 'CD'")
                If dtKNMT Is Nothing Then Throw New Exception("Code article client introuvable sous SAP")
                sQuery = "SELECT [NM_DSGT_ARTI]
  FROM [dbo].[DTM_REF_GAMM_ARTI]
 WHERE [CD_ARTI_PROD] = '" & Trim(dtKNMT(0)("KDMAT").ToString) & "'"
                dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion_ALMS)
                Session("sDSGT_ARTI") = dt(0)("NM_DSGT_ARTI").ToString
                dPDF.Open()

                'numéro de série
                pt = New PdfPTable(2)
                pt.TotalWidth = dPDF.PageSize.Width - 80
                pt.LockedWidth = True
                para = New Paragraph(New Chunk(“SN Produit : " & sNU_SER, FontFactory.GetFont("ARIAL", 16, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.Border = 0
                pt.AddCell(c_ENTE)
                sQuery = "SELECT [NM_STAT_PROD]
  FROM [dbo].[V_FCGF]
 WHERE [NU_SER_SPFE] = '" & sNU_SER & "'
 GROUP BY [NU_SER_SPFE]
      ,[NM_STAT_PROD]
ORDER BY [NM_STAT_PROD] DESC"
                dt_RESU = SQL_SELE_TO_DT(sQuery, sChaineConnexion_ALMS)
                para = New Paragraph(New Chunk("Statut global : " & dt_RESU(0)("NM_STAT_PROD").ToString, FontFactory.GetFont("ARIAL", 14, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.Border = 0
                pt.AddCell(c_ENTE)
                pt.SpacingAfter = 10
                dPDF.Add(pt)

                'code article
                para = New Paragraph(New Chunk(“Code article : " & Trim(dtKNMT(0)("KDMAT").ToString), FontFactory.GetFont("ARIAL", 12, Font.BOLD)))
                para.SpacingAfter = 10
                dPDF.Add(para)

                'of
                para = New Paragraph(New Chunk(“N° OF : " & sOF, FontFactory.GetFont("ARIAL", 12, Font.BOLD)))
                para.SpacingAfter = 10
                dPDF.Add(para)

                'liste des séquences
                sQuery = "SELECT [dbo].[DTM_SEQU_RFRC_LIST].[NM_RFRC_SEQU], [dbo].[DTM_SEQU_RFRC_LIST].[NU_VERS_SEQU], [dbo].[DTM_SEQU_RFRC_LIST].ID_RFRC_SEQU, [dbo].[DTM_SEQU_RFRC_LIST].DT_APCT
  FROM (
SELECT TOP 100 PERCENT [NM_RFRC_SEQU]
                              ,MAX([NU_VERS_SEQU]) AS [NU_VERS_SEQU]
                        FROM [dbo].[V_FCGF] 
                       WHERE [NU_SER_SPFE] = '" & sNU_SER & "'
                      GROUP BY [NM_RFRC_SEQU]
                              ,[NU_OP] 
                      ORDER BY [NU_OP]) AS A INNER JOIN [dbo].[DTM_SEQU_RFRC_LIST] ON 
								A.[NM_RFRC_SEQU] = [dbo].[DTM_SEQU_RFRC_LIST].[NM_RFRC_SEQU] AND A.[NU_VERS_SEQU] = [dbo].[DTM_SEQU_RFRC_LIST].[NU_VERS_SEQU]"
                dt_SEQU = SQL_SELE_TO_DT(sQuery, sChaineConnexion_ALMS)
                pt = New PdfPTable(3)
                pt.TotalWidth = dPDF.PageSize.Width - 80
                pt.LockedWidth = True
                Dim widths_4() As Single = {3, 1, 1}
                pt.SetWidths(widths_4)

                para = New Paragraph(New Chunk(“Liste des séquenceurs et logiciels”, FontFactory.GetFont("ARIAL", 16, Font.BOLDITALIC)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.Border = 0
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk(“Date d'application”, FontFactory.GetFont("ARIAL", 10)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.Border = 0
                c_ENTE.VerticalAlignment = Element.ALIGN_BOTTOM
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk(“ID séquence”, FontFactory.GetFont("ARIAL", 10)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.Border = 0
                c_ENTE.VerticalAlignment = Element.ALIGN_BOTTOM
                pt.AddCell(c_ENTE)
                For Each rdt As DataRow In dt_SEQU.Rows
                    para = New Paragraph(New Chunk(rdt("NM_RFRC_SEQU").ToString & "_V" & rdt("NU_VERS_SEQU").ToString, FontFactory.GetFont("ARIAL", 10)))
                    c_ENTE = New PdfPCell(para)
                    pt.AddCell(c_ENTE)
                    para = New Paragraph(New Chunk(Left(rdt("DT_APCT").ToString, 10), FontFactory.GetFont("ARIAL", 10)))
                    c_ENTE = New PdfPCell(para)
                    pt.AddCell(c_ENTE)
                    para = New Paragraph(New Chunk(rdt("ID_RFRC_SEQU").ToString, FontFactory.GetFont("ARIAL", 10)))
                    c_ENTE = New PdfPCell(para)
                    pt.AddCell(c_ENTE)
                Next
                pt.SpacingAfter = 10
                dPDF.Add(pt)

                'liste métrologie
                pt = New PdfPTable(4)
                pt.TotalWidth = dPDF.PageSize.Width - 80
                pt.LockedWidth = True
                Dim widths_5() As Single = {1, 1, 2, 1}
                pt.SetWidths(widths_5)
                para = New Paragraph(New Chunk(“Liste des appareils”, FontFactory.GetFont("ARIAL", 16, Font.BOLDITALIC)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.Border = 0
                c_ENTE.Colspan = 4
                pt.AddCell(c_ENTE)
                sQuery = "SELECT     DTM_CSTA_POST_1.ID_POST, DTM_CSTA_POST_1.ID_MTRE, ISNULL(MES_Digital_Factory.dbo.V_POST_MTLG_MTRE.DT_VLDT, N'') 
                      AS DT_VLDT, ISNULL([LB_DCPT],MES_Digital_Factory.dbo.V_POST_MTLG_MTRE.NM_TYPE_MTRE) AS NM_TYPE_MTRE
FROM         (SELECT     ID_POST, ID_MTRE, MAX(DT_AFCT) AS MAX_DT_AFCT, ISNULL(BL_SORT_FCGF,0) AS BL_SORT_FCGF
                       FROM          dbo.DTM_CSTA_POST
                       GROUP BY ID_POST, ID_MTRE, BL_SORT_FCGF) AS DT_DERN_AFCT INNER JOIN
                      dbo.DTM_CSTA_POST AS DTM_CSTA_POST_1 ON DT_DERN_AFCT.ID_POST = DTM_CSTA_POST_1.ID_POST AND 
                      DT_DERN_AFCT.ID_MTRE = DTM_CSTA_POST_1.ID_MTRE AND DT_DERN_AFCT.MAX_DT_AFCT = DTM_CSTA_POST_1.DT_AFCT INNER JOIN
                          (SELECT     CASE WHEN ID_POST LIKE 'P-%' THEN ID_POST ELSE 'POSTE_' + ID_POST END AS ID_POST
                            FROM          dbo.V_FCGF
                            WHERE      (NU_SER_SPFE = '" & sNU_SER & "')
                            GROUP BY NU_SER_SPFE, ID_POST) AS DT_LIST_POST ON DT_DERN_AFCT.ID_POST = DT_LIST_POST.ID_POST LEFT OUTER JOIN
                      MES_Digital_Factory.dbo.V_POST_MTLG_MTRE ON DTM_CSTA_POST_1.ID_MTRE = MES_Digital_Factory.dbo.V_POST_MTLG_MTRE.ID_MTRE
					  INNER JOIN [dbo].[DTM_REF_MTRE_LIST] ON [dbo].[DTM_REF_MTRE_LIST].[ID_MTRE] = DTM_CSTA_POST_1.ID_MTRE
WHERE     (DTM_CSTA_POST_1.BL_STAT = 1) AND (DTM_CSTA_POST_1.BL_SORT_FCGF = 1)
GROUP BY      DTM_CSTA_POST_1.ID_POST, DTM_CSTA_POST_1.ID_MTRE, ISNULL(MES_Digital_Factory.dbo.V_POST_MTLG_MTRE.DT_VLDT, N''), ISNULL([LB_DCPT],MES_Digital_Factory.dbo.V_POST_MTLG_MTRE.NM_TYPE_MTRE)"
                dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion_ALMS)
                para = New Paragraph(New Chunk("Poste", FontFactory.GetFont("ARIAL", 10, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk("Id matériel", FontFactory.GetFont("ARIAL", 10, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk("Type de matériel", FontFactory.GetFont("ARIAL", 10, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk("Date de validité", FontFactory.GetFont("ARIAL", 10, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                pt.AddCell(c_ENTE)
                For Each rdt As DataRow In dt.Rows
                    para = New Paragraph(New Chunk(rdt("ID_POST").ToString, FontFactory.GetFont("ARIAL", 10)))
                    c_ENTE = New PdfPCell(para)
                    pt.AddCell(c_ENTE)
                    para = New Paragraph(New Chunk(rdt("ID_MTRE").ToString, FontFactory.GetFont("ARIAL", 10)))
                    c_ENTE = New PdfPCell(para)
                    pt.AddCell(c_ENTE)
                    'type matériel = Interface de test
                    para = New Paragraph(New Chunk(rdt("NM_TYPE_MTRE").ToString, FontFactory.GetFont("ARIAL", 10)))
                    c_ENTE = New PdfPCell(para)
                    pt.AddCell(c_ENTE)
                    If rdt("NM_TYPE_MTRE").ToString = "Interface de test" Then
                        dtZMOYENTYPETEST = SAP_DATA_READ_ZMOYENTYPETEST("NO_MOYEN EQ '" & rdt("ID_MTRE").ToString & "'")
                        If dtZMOYENTYPETEST Is Nothing Then Throw New Exception("Pas de date de validité pour le banc n°" & rdt("NM_TYPE_MTRE").ToString & " sous SAP.")
                        para = New Paragraph(New Chunk(COMM_APP_WEB_CONV_FORM_DATE(Left(dtZMOYENTYPETEST(0)("PROCHVERIF").ToString, 10).Insert(6, "/").Insert(4, "/"), "dd/MM/yyyy"), FontFactory.GetFont("ARIAL", 10)))
                    Else
                        para = New Paragraph(New Chunk(Left(rdt("DT_VLDT").ToString, 10), FontFactory.GetFont("ARIAL", 10)))
                    End If
                    c_ENTE = New PdfPCell(para)
                    pt.AddCell(c_ENTE)
                Next
                pt.SpacingAfter = 10
                dPDF.Add(pt)

                'liste opérateur
                pt = New PdfPTable(2)
                pt.TotalWidth = dPDF.PageSize.Width - 80
                pt.LockedWidth = True
                para = New Paragraph(New Chunk(“Liste des opérateurs de la FCGF”, FontFactory.GetFont("ARIAL", 16, Font.BOLDITALIC)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.Border = 0
                c_ENTE.Colspan = 2
                pt.AddCell(c_ENTE)
                sQuery = "SELECT MAX([DT_TEST]) AS MAX_DT_TEST
      ,[ID_OPRT]
  FROM [dbo].[V_FCGF]
 WHERE [NU_SER_SPFE] = '" & sNU_SER & "'
GROUP BY [NU_SER_SPFE], [ID_OPRT]"
                dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion_ALMS)
                para = New Paragraph(New Chunk("Opérateur", FontFactory.GetFont("ARIAL", 10, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk("Date de phase", FontFactory.GetFont("ARIAL", 10, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                pt.AddCell(c_ENTE)
                For Each rdt As DataRow In dt.Rows
                    para = New Paragraph(New Chunk(rdt("ID_OPRT").ToString, FontFactory.GetFont("ARIAL", 10)))
                    c_ENTE = New PdfPCell(para)
                    pt.AddCell(c_ENTE)
                    para = New Paragraph(New Chunk(rdt("MAX_DT_TEST").ToString, FontFactory.GetFont("ARIAL", 10)))
                    c_ENTE = New PdfPCell(para)
                    pt.AddCell(c_ENTE)
                Next
                pt.SpacingAfter = 10
                dPDF.Add(pt)

                'liste observation
                pt = New PdfPTable(1)
                pt.TotalWidth = dPDF.PageSize.Width - 80
                pt.LockedWidth = True
                para = New Paragraph(New Chunk(“Liste des observations”, FontFactory.GetFont("ARIAL", 16, Font.BOLDITALIC)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.Border = 0
                pt.AddCell(c_ENTE)

                sQuery = "SELECT     dbo.DTM_NC_LIST.NU_INCI
FROM         (SELECT     NU_INCI, MAX(DT_ITVT) AS MAX_DT_ITVT, MAX(ID_STAT) AS DERN_ETAT
                       FROM          dbo.DTM_NC_DETA AS DTM_NC_DETA_1
                       GROUP BY NU_INCI) AS DT_MAX_ITVT INNER JOIN
                      dbo.DTM_NC_LIST ON DT_MAX_ITVT.NU_INCI = dbo.DTM_NC_LIST.NU_INCI
WHERE    [NU_SER_SPFE] = '" & sNU_SER & "'
              AND  (DT_MAX_ITVT.DERN_ETAT >= 4)"
                dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion_ALMS)
                If Not dt Is Nothing Then
                    For Each rdt As DataRow In dt.Rows
                        sList_inci &= rdt("NU_INCI").ToString & ", "
                    Next
                    para = New Paragraph(New Chunk(COMM_APP_WEB_STRI_TRIM_RIGHT(sList_inci, 2), FontFactory.GetFont("ARIAL", 10)))
                    c_ENTE = New PdfPCell(para)
                    pt.AddCell(c_ENTE)
                End If
                dPDF.Add(pt)

                'Séquence
                For Each rdt_SEQU As DataRow In dt_SEQU.Rows
                    dPDF.NewPage()
                    pt = New PdfPTable(2)
                    pt.TotalWidth = dPDF.PageSize.Width - 80
                    pt.LockedWidth = True
                    Dim widths_3() As Single = {3, 2}
                    pt.SetWidths(widths_3)
                    para = New Paragraph(New Chunk(rdt_SEQU("NM_RFRC_SEQU").ToString & "_V" & rdt_SEQU("NU_VERS_SEQU").ToString, FontFactory.GetFont("ARIAL", 16, Font.BOLDITALIC)))
                    c_ENTE = New PdfPCell(para)
                    c_ENTE.Border = 0
                    pt.AddCell(c_ENTE)

                    'phase
                    '       pour chaque phase un tableau
                    sQuery = "SELECT [DT_TEST]
      ,[NM_PHAS]
      ,[LB_DCPT_STAT]
      ,[LB_STAT_PHAS]
      ,[ID_POST]
      ,[ID_OPRT]
      ,[NU_PHAS_FCGF]
  FROM [dbo].[V_FCGF]
 WHERE [NU_SER_SPFE] = '" & sNU_SER & "'
   AND [NM_RFRC_SEQU] = '" & rdt_SEQU("NM_RFRC_SEQU").ToString & "'
   AND [NU_VERS_SEQU] = '" & rdt_SEQU("NU_VERS_SEQU").ToString & "'
  GROUP BY [NU_SER_SPFE]
,[NM_RFRC_SEQU]
,[NU_VERS_SEQU]
      ,[DT_TEST]
      ,[NM_PHAS]
      ,[LB_DCPT_STAT]
      ,[LB_STAT_PHAS]
      ,[ID_POST]
      ,[ID_OPRT]
	  ,[NU_OP]
      ,[NU_PHAS_FCGF]
  ORDER BY       [NU_OP]
      ,[NU_PHAS_FCGF]"
                    dt_PHAS = SQL_SELE_TO_DT(sQuery, sChaineConnexion_ALMS)
                    para = New Paragraph(New Chunk("Date de test : " & dt_PHAS(0)("DT_TEST").ToString, FontFactory.GetFont("ARIAL", 10)))
                    c_ENTE = New PdfPCell(para)
                    c_ENTE.HorizontalAlignment = Element.ALIGN_RIGHT
                    c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                    c_ENTE.Border = 0
                    pt.AddCell(c_ENTE)
                    pt.SpacingAfter = 10
                    dPDF.Add(pt)

                    For Each rdt_PHAS As DataRow In dt_PHAS.Rows
                        pt = New PdfPTable(8)
                        pt.TotalWidth = dPDF.PageSize.Width - 80
                        pt.LockedWidth = True
                        Dim widths_2() As Single = {4, 20, 6, 6, 6, 5, 10, 3}
                        pt.SetWidths(widths_2)

                        para = New Paragraph(New Chunk(rdt_PHAS("NM_PHAS").ToString, FontFactory.GetFont("ARIAL", 12, Font.BOLD)))
                        c_ENTE = New PdfPCell(para)
                        c_ENTE.Border = 0
                        c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                        c_ENTE.Colspan = 8
                        pt.AddCell(c_ENTE)
                        'para = New Paragraph(New Chunk("Date de test : " & rdt_PHAS("DT_TEST").ToString, FontFactory.GetFont("ARIAL", 10)))
                        'c_ENTE = New PdfPCell(para)
                        'c_ENTE.Colspan = 4
                        'c_ENTE.Border = 0
                        'pt.AddCell(c_ENTE)

                        para = New Paragraph(New Chunk("Statut de la phase : " & rdt_PHAS("LB_STAT_PHAS").ToString, FontFactory.GetFont("ARIAL", 10)))
                        c_ENTE = New PdfPCell(para)
                        c_ENTE.Colspan = 3
                        c_ENTE.Border = 0
                        pt.AddCell(c_ENTE)
                        para = New Paragraph(New Chunk("Poste : " & rdt_PHAS("ID_POST").ToString, FontFactory.GetFont("ARIAL", 10)))
                        c_ENTE = New PdfPCell(para)
                        c_ENTE.Colspan = 3
                        c_ENTE.Border = 0
                        pt.AddCell(c_ENTE)
                        para = New Paragraph(New Chunk("Opérateur : " & rdt_PHAS("ID_OPRT").ToString, FontFactory.GetFont("ARIAL", 10)))
                        c_ENTE = New PdfPCell(para)
                        c_ENTE.Colspan = 2
                        c_ENTE.Border = 0
                        pt.AddCell(c_ENTE)

                        '           pour chaque sous-pahse une ligne
                        sQuery = "SELECT [NM_SOUS_PHAS]
      ,[NU_LIMI_BASS]
      ,[NU_VALE]
      ,[LB_VALE]
      ,[NU_LIMI_HAUT]
      ,[NU_UNIT]      
,[NU_ORDR_SEQU]
,CONVERT(INTEGER,[BL_STAT_PHAS]) AS BL_STAT_PHAS
  FROM [dbo].[V_FCGF]
 WHERE [NU_SER_SPFE] = '" & sNU_SER & "'
   AND [NU_PHAS_FCGF] = '" & Replace(rdt_PHAS("NU_PHAS_FCGF").ToString, "'", "''") & "'
   AND [DT_TEST] = '" & rdt_PHAS("DT_TEST").ToString & "'
   AND [NM_RFRC_SEQU] = '" & rdt_SEQU("NM_RFRC_SEQU").ToString & "'
   AND [NU_VERS_SEQU] = '" & rdt_SEQU("NU_VERS_SEQU").ToString & "'
GROUP BY [NU_SER_SPFE]
,[NM_RFRC_SEQU]
,[NU_VERS_SEQU]
,[NM_PHAS]
,[DT_TEST]
,[NM_SOUS_PHAS]
      ,[NU_LIMI_BASS]
      ,[NU_VALE]
      ,[LB_VALE]
      ,[NU_LIMI_HAUT]
      ,[NU_UNIT]
	  ,[NU_OP]
      ,[NU_ORDR_SEQU]
      ,[NU_PHAS_FCGF]
      ,[NU_SOUS_PHAS_FCGF]
,[BL_STAT_PHAS]
ORDER BY [NU_OP]
      ,[NU_PHAS_FCGF]
      ,[NU_SOUS_PHAS_FCGF]"
                        dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion_ALMS)
                        para = New Paragraph(New Chunk("Index", FontFactory.GetFont("ARIAL", 10, Font.BOLD)))
                        c_ENTE = New PdfPCell(para)
                        c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                        pt.AddCell(c_ENTE)
                        para = New Paragraph(New Chunk("Description", FontFactory.GetFont("ARIAL", 10, Font.BOLD)))
                        c_ENTE = New PdfPCell(para)
                        c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                        pt.AddCell(c_ENTE)
                        para = New Paragraph(New Chunk("Min ≤", FontFactory.GetFont("ARIAL", 10, Font.BOLD)))
                        c_ENTE = New PdfPCell(para)
                        c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                        pt.AddCell(c_ENTE)
                        para = New Paragraph(New Chunk("Mesure", FontFactory.GetFont("ARIAL", 10, Font.BOLD)))
                        c_ENTE = New PdfPCell(para)
                        c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                        pt.AddCell(c_ENTE)

                        para = New Paragraph(New Chunk("≤ Max", FontFactory.GetFont("ARIAL", 10, Font.BOLD)))
                        c_ENTE = New PdfPCell(para)
                        c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                        pt.AddCell(c_ENTE)
                        para = New Paragraph(New Chunk("Unité", FontFactory.GetFont("ARIAL", 10, Font.BOLD)))
                        c_ENTE = New PdfPCell(para)
                        c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                        pt.AddCell(c_ENTE)
                        para = New Paragraph(New Chunk("Saisie", FontFactory.GetFont("ARIAL", 10, Font.BOLD)))
                        c_ENTE = New PdfPCell(para)
                        c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                        pt.AddCell(c_ENTE)

                        para = New Paragraph(New Chunk("OK", FontFactory.GetFont("ARIAL", 10, Font.BOLD)))
                        c_ENTE = New PdfPCell(para)
                        c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                        pt.AddCell(c_ENTE)

                        For Each rdt As DataRow In dt.Rows
                            para = New Paragraph(New Chunk(rdt("NU_ORDR_SEQU").ToString, FontFactory.GetFont("ARIAL", 10)))
                            c_ENTE = New PdfPCell(para)
                            pt.AddCell(c_ENTE)
                            para = New Paragraph(New Chunk(rdt("NM_SOUS_PHAS").ToString, FontFactory.GetFont("ARIAL", 10)))
                            c_ENTE = New PdfPCell(para)
                            pt.AddCell(c_ENTE)
                            para = New Paragraph(New Chunk(rdt("NU_LIMI_BASS").ToString, FontFactory.GetFont("ARIAL", 10)))
                            c_ENTE = New PdfPCell(para)
                            c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                            pt.AddCell(c_ENTE)
                            para = New Paragraph(New Chunk(rdt("NU_VALE").ToString, FontFactory.GetFont("ARIAL", 10)))
                            c_ENTE = New PdfPCell(para)
                            c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                            pt.AddCell(c_ENTE)

                            para = New Paragraph(New Chunk(rdt("NU_LIMI_HAUT").ToString, FontFactory.GetFont("ARIAL", 10)))
                            c_ENTE = New PdfPCell(para)
                            c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                            pt.AddCell(c_ENTE)
                            para = New Paragraph(New Chunk(rdt("NU_UNIT").ToString, FontFactory.GetFont("ARIAL", 10)))
                            c_ENTE = New PdfPCell(para)
                            pt.AddCell(c_ENTE)
                            para = New Paragraph(New Chunk(rdt("LB_VALE").ToString, FontFactory.GetFont("ARIAL", 10)))
                            c_ENTE = New PdfPCell(para)
                            pt.AddCell(c_ENTE)
                            para = New Paragraph(New Chunk(rdt("BL_STAT_PHAS").ToString, FontFactory.GetFont("ARIAL", 10)))
                            c_ENTE = New PdfPCell(para)
                            pt.AddCell(c_ENTE)
                        Next
                        'c_ENTE = New PdfPCell(New Phrase(""))
                        'c_ENTE.Colspan = 6
                        'c_ENTE.Border = 0
                        'pt.AddCell(c_ENTE)
                        pt.KeepTogether = True
                        pt.SpacingAfter = 10
                        dPDF.Add(pt)

                    Next
                    '  dPDF.NewPage()
                Next
                para = New Paragraph(New Chunk("Statut global :  " & dt_RESU(0)("NM_STAT_PROD").ToString, FontFactory.GetFont("ARIAL", 14, Font.BOLD)))
                dPDF.Add(para)
                dPDF.Close()
            End Using
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Return Nothing
        Finally
            'dPDF.Close()
        End Try
        Return sfich
    End Function

    Protected Sub Button_DCMT_PASS_Click(sender As Object, e As EventArgs) Handles Button_DCMT_PASS.Click
        'ranger le doc
        Dim sfihcsauv As String = DIG_FACT_SQL_GET_PARA(Trim(Label_CD_ARTI.Text), "Chemin de sauvegarde du fichier PDF")
        COMM_APP_WEB_COPY_FILE(Session("fcgf"), sfihcsauv & "\OF " & Label_OF.Text & "\" & TextBox_NU_SER.Text & "_" & COMM_APP_WEB_CONV_FORM_DATE(Session("DT_DEB"), "ddMMyyyy_HHmmss") & ".pdf", True)
        COMM_APP_WEB_COPY_FILE(Session("listetracabilite"), sfihcsauv & "\OF " & Label_OF.Text & "\" & TextBox_NU_SER.Text & " – NM.pdf", True)
        COMM_APP_WEB_COPY_FILE(Session("dhr"), sfihcsauv & "\OF " & Label_OF.Text & "\DHR-" & TextBox_NU_SER.Text & ".pdf", True)


        If DIG_FACT_SQL_GET_PARA(Trim(Label_CD_ARTI.Text), "Scan numéro de série fin opération") = "1" Then
            'MultiView_VLDT.SetActiveView(View_VLDT_OPRT)
            MultiView_ETAP.SetActiveView(View_VLDT_OPRT)
            'MultiView_SAIS_OPRT.SetActiveView(View_VLDT_OPRT)
            TextBox_NU_SER_VLDT_OPRT.Text = ""
            TextBox_NU_SER_VLDT_OPRT.Focus()

            'Saveasas en javascript
            'COMM_APP_WEB_COPY_FILE("c:\sources\app_web\PagesMembres\Digital_Factory\1.pdf",
            '                       "\\so2k3vm02\Bureautique\Groupes\SO_CLIENTS\AIR LIQUIDE MEDICAL\Projet VENDOME\TAEKC042100$ (Ventilateur Vendome ST40)\F-Donnees de sortie\3-Fiches Suiveuses\" & TextBox_NU_SER.Text & "_" & COMM_APP_WEB_CONV_FORM_DATE(Now, "ddMMyyyy_hhmmss") & ".pdf", True)
        Else
            _ERGT_OPRT_PASS("P")
        End If
    End Sub

    Protected Sub Button_DCMT_FAIL_Click(sender As Object, e As EventArgs) Handles Button_DCMT_FAIL.Click
        '
        _ERGT_OPRT_PASS("F")

    End Sub

    Public Function _VRFC_DCMT() As Integer

        Try
            MultiView_ETAP.SetActiveView(View_VOID)
            'MultiView_Tracabilité.SetActiveView(View_VOID_2)

            'Dim sfich As String = "", sfichdhr As String = "", sfichlisttcbl As String = ""
            If Trim(Label_CLIE.Text) = "AIR LIQUIDE MEDICAL" And Label_DES_OP.Text = "CONTROLE FINAL" And Label_OP.Text = "180" Then
                MultiView_ETAP.SetActiveView(View_DCMT)
                Session("fcgf") = CREA_FCGF(TextBox_NU_SER.Text, Label_OF.Text)
                If Session("fcgf") Is Nothing Then Throw New Exception("La création de la FCGF a échoué")
                COMM_APP_WEB_COPY_FILE(Session("fcgf"), "c:\sources\app_web\PagesMembres\Digital_Factory\1.pdf", True)
                Return 1
            End If
            'DHR
            If Trim(Label_CLIE.Text) = "AIR LIQUIDE MEDICAL" And Label_DES_OP.Text = "ACCEPTATION QUALITE" And Label_OP.Text = "210" Then
                MultiView_ETAP.SetActiveView(View_DCMT)

                If Label_OF.Text = "1173833" Then
                    Session("listetracabilite") = CREA_LIST_TCBL(TextBox_NU_SER.Text, "1164008") 'exception rétrofit
                Else
                    Session("listetracabilite") = CREA_LIST_TCBL(TextBox_NU_SER.Text, Label_OF.Text)
                End If
                If Session("listetracabilite") Is Nothing Then Throw New Exception("La création de la liste de traçabilité a échoué")

                Session("dhr") = CREA_DHR(TextBox_NU_SER.Text, Label_OF.Text)
                If Session("dhr") Is Nothing Then Throw New Exception("La création du DHR a échoué")
                COMM_APP_WEB_COPY_FILE(Session("dhr"), "c:\sources\app_web\PagesMembres\Digital_Factory\1.pdf", True)
                PDF_CCTN_FICH("c:\sources\app_web\PagesMembres\Digital_Factory\1.pdf", Session("listetracabilite"))
                Return 1
            End If
            Return 0
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Return -1
        End Try

    End Function

    Public Function CREA_DHR(sNU_SER As String, sOF As String) As String
        Dim dt, dt_ETAP, dt_SEQU, dt_T_TAB_DOC_SAP, dtPA0002, dtKNMT, dtAFKO, dtZMOYENTYPETEST, dtZGMO_OUTIL_VER As New DataTable
        Dim sQuery As String = ""
        'Dim dPDF As Document
        Dim pt As PdfPTable
        Dim c_ENTE As PdfPCell
        Randomize()
        'Dim iLOGO As Image
        Dim para As Paragraph
        'Dim ipagenumber As Integer.
        Dim phr As Phrase
        Dim sdatefcgf As String = ""
        Dim sfich As String = “C:\sources\temp_App_Web\" & CInt(Int((10000000 * Rnd()) + 1)) & ".pdf”
        Try
            Using dPDF = New Document(PageSize.A4, 20, 20, 30, 20)

                Dim writer = PdfAWriter.GetInstance(dPDF, New FileStream(sfich, FileMode.Create))
                Dim PageEventHandler As New HeaderFooter_DHR()
                writer.PageEvent = PageEventHandler
                dPDF.Open()

                'of
                'dPDF.Add(New Phrase("
                '"))
                pt = New PdfPTable(2)

                With pt
                    pt.LockedWidth = True
                    pt.TotalWidth = dPDF.PageSize.Width - 100
                    Dim widths() As Single = {18, 26}
                    pt.SetWidths(widths)
                    para = New Paragraph(New Chunk(“Référence du produit : ”, FontFactory.GetFont("CALIBRI", 14, Font.BOLD)))
                    c_ENTE = New PdfPCell(para)
                    c_ENTE.Border = 0
                    c_ENTE.HorizontalAlignment = Element.ALIGN_RIGHT
                    pt.AddCell(c_ENTE)
                    para = New Paragraph(New Chunk(Label_DES_ARTI.Text & “ - ” & Label_CD_ARTI.Text, FontFactory.GetFont("CALIBRI", 14)))
                    c_ENTE = New PdfPCell(para)
                    c_ENTE.Border = 0
                    c_ENTE.BorderWidthBottom = 1
                    pt.AddCell(c_ENTE)
                    para = New Paragraph(New Chunk(“Référence ALMS : ”, FontFactory.GetFont("CALIBRI", 14, Font.BOLD)))
                    c_ENTE = New PdfPCell(para)
                    c_ENTE.Border = 0
                    c_ENTE.HorizontalAlignment = Element.ALIGN_RIGHT
                    pt.AddCell(c_ENTE)

                    dtKNMT = SAP_DATA_READ_KNMT("MATNR LIKE '" & Label_CD_ARTI.Text & "%' AND KUNNR EQ '0000000451' AND VKORG EQ 'ORC3' AND VTWEG EQ 'CD'")
                    If dtKNMT Is Nothing Then Throw New Exception("Code article client introuvable sous SAP")
                    para = New Paragraph(New Chunk(Trim(dtKNMT(0)("KDMAT").ToString), FontFactory.GetFont("CALIBRI", 14)))
                    c_ENTE = New PdfPCell(para)
                    c_ENTE.Border = 0
                    c_ENTE.BorderWidthBottom = 1
                    pt.AddCell(c_ENTE)
                    para = New Paragraph(New Chunk(“Numéro de série : ”, FontFactory.GetFont("CALIBRI", 14, Font.BOLD)))
                    c_ENTE = New PdfPCell(para)
                    c_ENTE.Border = 0
                    c_ENTE.HorizontalAlignment = Element.ALIGN_RIGHT
                    pt.AddCell(c_ENTE)
                    para = New Paragraph(New Chunk(sNU_SER, FontFactory.GetFont("CALIBRI", 14)))
                    c_ENTE = New PdfPCell(para)
                    c_ENTE.Border = 0
                    c_ENTE.BorderWidthBottom = 1
                    pt.AddCell(c_ENTE)
                    para = New Paragraph(New Chunk(“Numéro d'OF : ”, FontFactory.GetFont("CALIBRI", 14, Font.BOLD)))
                    c_ENTE = New PdfPCell(para)
                    c_ENTE.Border = 0
                    c_ENTE.HorizontalAlignment = Element.ALIGN_RIGHT
                    pt.AddCell(c_ENTE)
                    para = New Paragraph(New Chunk(sOF, FontFactory.GetFont("CALIBRI", 14)))
                    c_ENTE = New PdfPCell(para)
                    c_ENTE.Border = 0
                    c_ENTE.BorderWidthBottom = 1
                    pt.AddCell(c_ENTE)
                    para = New Paragraph(New Chunk(“Indice dossier de fabrication : ”, FontFactory.GetFont("CALIBRI", 14, Font.BOLD)))
                    c_ENTE = New PdfPCell(para)
                    c_ENTE.Border = 0
                    c_ENTE.HorizontalAlignment = Element.ALIGN_RIGHT
                    pt.AddCell(c_ENTE)
                    dtAFKO = SAP_DATA_READ_AFKO("AUFNR LIKE '%" & sOF & "'")
                    If dtAFKO Is Nothing Then Throw New Exception("l'indice de la gamme non trouvé")
                    para = New Paragraph(New Chunk(dtAFKO(0)("REVLV").ToString, FontFactory.GetFont("CALIBRI", 14)))
                    c_ENTE = New PdfPCell(para)
                    c_ENTE.Border = 0
                    c_ENTE.BorderWidthBottom = 1
                    pt.AddCell(c_ENTE)
                    dPDF.Add(pt)
                End With

                dPDF.Add(New Phrase("
            "))


                pt = New PdfPTable(1)
                pt.LockedWidth = True
                pt.TotalWidth = dPDF.PageSize.Width - 250
                para = New Paragraph(New Chunk(“Acceptation du produit : Revu et approuvé”, FontFactory.GetFont("CALIBRI", 14, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.PaddingTop = 10
                c_ENTE.PaddingBottom = 10
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)

                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk(“Acceptation du produit le : ” & Now.ToString, FontFactory.GetFont("CALIBRI", 12, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.PaddingTop = 15
                c_ENTE.PaddingBottom = 15
                pt.AddCell(c_ENTE)

                Dim sNOM_PREN_VISA As String = “Nom, Prénom : ” & Session("displayname")
                para = New Paragraph(New Chunk(sNOM_PREN_VISA, FontFactory.GetFont("CALIBRI", 12, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.PaddingTop = 15
                c_ENTE.PaddingBottom = 15
                pt.AddCell(c_ENTE)
                dPDF.Add(pt)

                dPDF.Add(New Phrase("
            "))
                'hist de prod
                para = New Paragraph(New Chunk(“1. HISTORIQUE DE PRODUCTION”, FontFactory.GetFont("CALIBRI", 14, Font.BOLD Or Font.UNDERLINE)))
                para.SpacingAfter = 20
                dPDF.Add(para)

                pt = New PdfPTable(5)
                pt.LockedWidth = True
                pt.TotalWidth = dPDF.PageSize.Width - 80
                Dim widths_2() As Single = {45, 15, 25, 25, 15}
                pt.SetWidths(widths_2)
                para = New Paragraph(New Chunk(“Liste des Opérations de Fabrication et de Contrôle”, FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.Rowspan = 2
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk(“Doc de réf”, FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.Rowspan = 2
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk(“Intervention”, FontFactory.GetFont("CALIBRI", 11, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.Rowspan = 2
                c_ENTE.Colspan = 2
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk(“Résultats”, FontFactory.GetFont("CALIBRI", 11, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk(“(OK/NOK)”, FontFactory.GetFont("CALIBRI", 11, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                sQuery = "SELECT    ISNULL(DT_DERN_PASS.DER_DT_DEB, '') AS DT_PASS, 
DT_WF.NM_OPRT, 
DT_WF.CD_ARTI, 
DT_WF.VAL_PARA, 
DT_WF.ID_NU_SER, 
                      CASE ISNULL(dbo.DTM_PSG.LB_SCTN, 'F') WHEN 'P' THEN 'OK' ELSE 'NOK' END AS BL_RESU, 
ISNULL(dbo.DTM_PSG.NM_MATR, '') AS ID_MTCL, dbo.DTM_PSG.LB_ETP,
REPLACE(RIGHT(DT_WF.VAL_PARA, LEN(DT_WF.VAL_PARA) - CHARINDEX(' (OP:',DT_WF.VAL_PARA) - 4),')','') AS NU_OP, 
dbo.DTM_PSG.[LB_MOYN]
FROM         dbo.DTM_PSG INNER JOIN
                          (SELECT     NM_OF, LB_ETP, DER_DT_DEB, NU_SER_CLIE, NU_SER_ECO
                            FROM          dbo.V_DER_PASS_BON
                            WHERE      (NU_SER_CLIE = N'" & sNU_SER & "')) AS DT_DERN_PASS ON dbo.DTM_PSG.LB_ETP = DT_DERN_PASS.LB_ETP AND 
                      dbo.DTM_PSG.NM_OF = DT_DERN_PASS.NM_OF AND dbo.DTM_PSG.DT_DEB = DT_DERN_PASS.DER_DT_DEB RIGHT OUTER JOIN
                          (SELECT     DT_WF_1.NU_ETAP, DT_WF_1.NM_OPRT, DT_WF_1.CD_ARTI, DT_WF_1.VAL_PARA, dbo.V_LIAIS_NU_SER.NU_SER_CLIE, 
                                                   dbo.V_LIAIS_NU_SER.ID_NU_SER
                            FROM          (SELECT     CD_ARTI, LEFT(VAL_PARA, CHARINDEX(' (OP:', VAL_PARA)) AS NM_OPRT, VAL_PARA, REPLACE(NM_PARA, 'WORKFLOW étape ', '') 
                                                                           AS NU_ETAP
                                                    FROM          dbo.V_DER_DTM_REF_PARA
                                                    WHERE      (NM_PARA LIKE N'WORKFLOW étape%')) AS DT_WF_1 INNER JOIN
                                                   SAP.dbo.V_MES_DIG_FACT_IDCT ON DT_WF_1.CD_ARTI = SAP.dbo.V_MES_DIG_FACT_IDCT.PLNBEZ INNER JOIN
                                                   dbo.V_LIAIS_NU_SER ON SAP.dbo.V_MES_DIG_FACT_IDCT.AUFNR = dbo.V_LIAIS_NU_SER.NU_OF
                            WHERE      (dbo.V_LIAIS_NU_SER.NU_SER_CLIE = N'" & sNU_SER & "')) AS DT_WF ON DT_DERN_PASS.LB_ETP = DT_WF.VAL_PARA AND 
                      dbo.DTM_PSG.ID_NU_SER = DT_WF.ID_NU_SER
ORDER BY CONVERT(integer, DT_WF.NU_ETAP)"
                dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
                For Each rdt As DataRow In dt.Rows
                    If Trim(rdt("NM_OPRT").ToString) = "ACCEPTATION QUALITE" Then Continue For
                    If Trim(rdt("NM_OPRT").ToString) = "PALETTISATION" Then Continue For
                    If Trim(rdt("NM_OPRT").ToString) = "CONTROLE FINAL" Then sdatefcgf = COMM_APP_WEB_CONV_FORM_DATE(rdt("DT_PASS").ToString(), "ddMMyyyy_HHmmss")
                    sQuery = "SELECT [NU_OF]
      ,[DT_DEB]
      ,[LB_ETP]
      ,[LB_CONS_ETAP]
	  ,CASE WHEN [NB_VAL] BETWEEN CONVERT(FLOAT, REPLACE([NU_LIMI_IFRE_ETAP], ',', '.')) AND CONVERT(FLOAT, REPLACE([NU_LIMI_SPRE_ETAP], ',', '.')) 
                THEN 'OK' 
				ELSE 'NOK' END AS BL_SANC
  FROM [dbo].[V_RSLT_POIN_ARRE_TCBL_OPRT]
 WHERE NU_SER_CLIE = '" & sNU_SER & "' AND [DT_DEB] = '" & rdt("DT_PASS").ToString & "' AND [LB_ETP] = '" & rdt("LB_ETP").ToString & "'
 ORDER BY [ID_INDE]"
                    dt_ETAP = SQL_SELE_TO_DT(sQuery, sChaineConnexion)

                    para = New Paragraph(New Chunk(rdt("NM_OPRT").ToString, FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                    c_ENTE = New PdfPCell(para)
                    c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                    c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                    pt.AddCell(c_ENTE)

                    dt_T_TAB_DOC_SAP = SAP_DATA_Z_GET_DOC_INFO(sOF, rdt("NU_OP").ToString)
                    If dt_T_TAB_DOC_SAP Is Nothing Then 'Throw New Exception("Pas de FI trouvée dans SAP")
                        'If Trim(rdt("NM_OPRT").ToString) = "CONTROLE FINAL" Then
                        '    para = New Paragraph(New Chunk("Dossier de contrôle", FontFactory.GetFont("CALIBRI", 10))) 'provisoire
                        'Else
                        '    para = New Paragraph(New Chunk("vide", FontFactory.GetFont("CALIBRI", 10)))
                        'End If
                        para = New Paragraph(New Chunk("vide", FontFactory.GetFont("CALIBRI", 10)))
                    Else
                        para = New Paragraph(New Chunk(Trim(dt_T_TAB_DOC_SAP(0)("DKTXT").ToString), FontFactory.GetFont("CALIBRI", 10)))
                    End If
                    c_ENTE = New PdfPCell(para)
                    If Not dt_ETAP Is Nothing Then c_ENTE.Rowspan = dt_ETAP.Rows.Count + 1
                    c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                    c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                    pt.AddCell(c_ENTE)

                    dtPA0002 = SAP_DATA_READ_TBL("PA0002", "|", "", "PERNR NACHN VORNA", "PERNR EQ '*" & rdt("ID_MTCL").ToString & "'")
                    If dtPA0002 Is Nothing Then 'Throw New Exception("pas de nom/prénom trouvés pour le matricule " & rdt("ID_MTCL").ToString)
                        para = New Paragraph(New Chunk("", FontFactory.GetFont("CALIBRI", 10)))
                    Else
                        para = New Paragraph(New Chunk(Trim(dtPA0002(0)("NACHN").ToString) & " " & Trim(dtPA0002(0)("VORNA").ToString), FontFactory.GetFont("CALIBRI", 10)))
                    End If
                    ' Dim sNom_Prenom As String =
                    c_ENTE = New PdfPCell(para)
                    If Not dt_ETAP Is Nothing Then c_ENTE.Rowspan = dt_ETAP.Rows.Count + 1
                    c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                    c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                    pt.AddCell(c_ENTE)

                    para = New Paragraph(New Chunk(rdt("DT_PASS").ToString, FontFactory.GetFont("CALIBRI", 10)))
                    c_ENTE = New PdfPCell(para)
                    If Not dt_ETAP Is Nothing Then c_ENTE.Rowspan = dt_ETAP.Rows.Count + 1
                    c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                    c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                    pt.AddCell(c_ENTE)

                    para = New Paragraph(New Chunk(rdt("BL_RESU").ToString, FontFactory.GetFont("CALIBRI", 10)))
                    c_ENTE = New PdfPCell(para)
                    c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                    c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                    pt.AddCell(c_ENTE)

                    If Not dt_ETAP Is Nothing Then
                        For Each rdt_ETAP As DataRow In dt_ETAP.Rows
                            para = New Paragraph(New Chunk(rdt_ETAP("LB_CONS_ETAP").ToString, FontFactory.GetFont("CALIBRI", 10)))
                            c_ENTE = New PdfPCell(para)
                            c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                            pt.AddCell(c_ENTE)

                            para = New Paragraph(New Chunk(rdt_ETAP("BL_SANC").ToString, FontFactory.GetFont("CALIBRI", 10)))
                            c_ENTE = New PdfPCell(para)
                            c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                            c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                            pt.AddCell(c_ENTE)
                        Next
                    End If
                Next
                'pt.KeepTogether = True
                pt.HeaderRows = 2
                pt.SpacingAfter = 30
                dPDF.Add(pt)

                'traçabilité des composants
                para = New Paragraph(New Chunk(“2. TRAÇABILITE DES COMPOSANTS”, FontFactory.GetFont("CALIBRI", 14, Font.BOLD Or Font.UNDERLINE)))
                para.SpacingAfter = 20
                dPDF.Add(para)
                Dim slist_tarça As String = "Voir annexe 3 : Liste traçabilité " & sNU_SER & " – NM"
                para = New Paragraph(New Chunk(slist_tarça, FontFactory.GetFont("CALIBRI", 10)))
                para.SpacingAfter = 30
                dPDF.Add(para)

                'traçabilité des composants
                para = New Paragraph(New Chunk(“3. MOYENS ET OUTILLAGES”, FontFactory.GetFont("CALIBRI", 14, Font.BOLD Or Font.UNDERLINE)))
                para.SpacingAfter = 20
                dPDF.Add(para)
                pt = New PdfPTable(4)
                pt.LockedWidth = True
                pt.TotalWidth = dPDF.PageSize.Width - 80
                Dim widths_4() As Single = {20, 40, 20, 20}
                pt.SetWidths(widths_4)
                para = New Paragraph(New Chunk("N° EOLANE", FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk("Désignation", FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk("Validité", FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk("N° du poste affecté", FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                pt.SpacingAfter = 30
                sQuery = "SELECT     TOP (100) PERCENT dbo.V_LIAIS_NU_SER.NU_SER_ECO, dbo.V_LIAIS_NU_SER.NU_SER_CLIE, dbo.DTM_HIST_MTRE.ID_MTRE,
                      dbo.V_POST_LIST_MTRE.NM_TYPE_MTRE, dbo.V_POST_MTLG_MTRE.DT_VLDT, dbo.DTM_PSG.LB_MOYN
FROM         dbo.DTM_HIST_MTRE INNER JOIN
                      dbo.DTM_PSG ON dbo.DTM_HIST_MTRE.ID_PSG = dbo.DTM_PSG.ID_PSG INNER JOIN
                      dbo.V_LIAIS_NU_SER ON dbo.DTM_PSG.ID_NU_SER = dbo.V_LIAIS_NU_SER.ID_NU_SER INNER JOIN
                      dbo.V_POST_LIST_MTRE ON dbo.DTM_HIST_MTRE.ID_MTRE = dbo.V_POST_LIST_MTRE.ID_MTRE LEFT OUTER JOIN
                      dbo.V_POST_MTLG_MTRE ON dbo.DTM_HIST_MTRE.ID_MTRE = dbo.V_POST_MTLG_MTRE.ID_MTRE
  WHERE dbo.V_LIAIS_NU_SER.NU_SER_CLIE = '" & sNU_SER & "' AND  dbo.DTM_PSG.LB_SCTN = 'P'
GROUP BY dbo.V_LIAIS_NU_SER.NU_SER_ECO, dbo.V_LIAIS_NU_SER.NU_SER_CLIE, dbo.DTM_HIST_MTRE.ID_MTRE, dbo.V_POST_LIST_MTRE.NM_TYPE_MTRE, 
                      dbo.V_POST_MTLG_MTRE.DT_VLDT, dbo.DTM_PSG.LB_MOYN
ORDER BY dbo.DTM_PSG.LB_MOYN"
                dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
                For Each rdt As DataRow In dt.Rows
                    rdt("ID_MTRE") = Replace(Replace(Replace(LCase(rdt("ID_MTRE").ToString), ".eolane.com", ""), "\\so2k8vm07\", ""), "\\so2k3vm05\", "")
                    If rdt("NM_TYPE_MTRE").ToString = "Poste de travail" Then Continue For
                    para = New Paragraph(New Chunk(rdt("ID_MTRE").ToString, FontFactory.GetFont("CALIBRI", 10)))
                    c_ENTE = New PdfPCell(para)
                    pt.AddCell(c_ENTE)
                    para = New Paragraph(New Chunk(rdt("NM_TYPE_MTRE").ToString, FontFactory.GetFont("CALIBRI", 10)))
                    c_ENTE = New PdfPCell(para)
                    pt.AddCell(c_ENTE)

                    If rdt("NM_TYPE_MTRE").ToString = "Interface de test" Then
                        dtZMOYENTYPETEST = SAP_DATA_READ_ZMOYENTYPETEST("NO_MOYEN EQ '" & rdt("ID_MTRE").ToString & "'")
                        If dtZMOYENTYPETEST Is Nothing Then Throw New Exception("Pas de date de validité pour le banc n°" & rdt("NM_TYPE_MTRE").ToString & " sous SAP.")
                        para = New Paragraph(New Chunk(Replace(COMM_APP_WEB_CONV_FORM_DATE(dtZMOYENTYPETEST(0)("PROCHVERIF").ToString.Insert(6, "/").Insert(4, "/"), "dd/MM/yyyy"), "0001/00/00", ""), FontFactory.GetFont("ARIAL", 10)))
                    Else
                        If rdt("DT_VLDT").ToString = "" Then
                            'rdt("DT_VLDT") = "N/A"
                            dtZGMO_OUTIL_VER = SAP_DATA_READ_ZGMO_OUTIL_VER("ID LIKE '%" & rdt("ID_MTRE").ToString & "'")
                            If Not dtZGMO_OUTIL_VER Is Nothing Then
                                para = New Paragraph(New Chunk(COMM_APP_WEB_CONV_FORM_DATE(dtZGMO_OUTIL_VER(0)("PROCHVERIF1").ToString.Insert(6, "/").Insert(4, "/"), "dd/MM/yyyy"), FontFactory.GetFont("ARIAL", 10)))
                            Else
                                para = New Paragraph(New Chunk("N/A", FontFactory.GetFont("ARIAL", 10)))
                            End If
                        Else
                            para = New Paragraph(New Chunk(Left(rdt("DT_VLDT").ToString, 10), FontFactory.GetFont("ARIAL", 10)))
                        End If
                    End If

                    'para = New Paragraph(New Chunk(Left(rdt("DT_VLDT").ToString, 10), FontFactory.GetFont("CALIBRI", 10)))
                    c_ENTE = New PdfPCell(para)
                    pt.AddCell(c_ENTE)
                    para = New Paragraph(New Chunk(rdt("LB_MOYN").ToString, FontFactory.GetFont("CALIBRI", 10)))
                    c_ENTE = New PdfPCell(para)
                    pt.AddCell(c_ENTE)
                Next
                pt.HeaderRows = 1
                dPDF.Add(pt)

                'par opération
                'extract nomenclature typo
                'trouver pour chaque matériel qui était saisi dans la date
                'extraire le n° de poste physique

                dPDF.NewPage()
                'liste écart prod
                para = New Paragraph(New Chunk(“4. LISTE DES ECARTS DE PRODUCTION”, FontFactory.GetFont("CALIBRI", 14, Font.BOLD Or Font.UNDERLINE)))
                para.SpacingAfter = 20
                dPDF.Add(para)

                'nqi, dérog, hg
                para = New Paragraph(New Chunk(“4.1. NQI, Dérogation, Hors-gamme :”, FontFactory.GetFont("CALIBRI", 12, Font.BOLD Or Font.UNDERLINE)))
                para.SpacingAfter = 20
                dPDF.Add(para)
                pt = New PdfPTable(3)
                pt.LockedWidth = True
                pt.TotalWidth = dPDF.PageSize.Width - 80
                Dim widths_5() As Single = {4, 3, 3}
                pt.SetWidths(widths_5)
                para = New Paragraph(New Chunk("Evénement", FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk("Action réalisée par", FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk("Date", FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                sQuery = "SELECT     dbo.DTM_NC_LIST.NU_DRGT + ' : ' + dbo.DTM_REF_TYPE_NC.NM_DCPO_NC AS NU_DRGT, dbo.DTM_NC_DETA.DT_ITVT, dbo.DTM_NC_DETA.ID_OPRT
FROM         dbo.DTM_NC_DETA INNER JOIN
                      dbo.DTM_NC_LIST ON dbo.DTM_NC_DETA.NU_INCI = dbo.DTM_NC_LIST.NU_INCI INNER JOIN
                      dbo.DTM_REF_TYPE_NC ON dbo.DTM_NC_LIST.ID_TYPE_NC = dbo.DTM_REF_TYPE_NC.ID_TYPE_NC
WHERE     (NOT (dbo.DTM_NC_LIST.NU_DRGT IS NULL)) AND (dbo.DTM_NC_LIST.NU_SER_SPFE = N'" & sNU_SER & "') AND (dbo.DTM_NC_DETA.ID_STAT = 4)"

                dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion_ALMS)
                If Not dt Is Nothing Then
                    For Each rdt As DataRow In dt.Rows
                        para = New Paragraph(New Chunk(rdt("NU_DRGT").ToString, FontFactory.GetFont("CALIBRI", 10)))
                        c_ENTE = New PdfPCell(para)
                        pt.AddCell(c_ENTE)
                        dtPA0002 = SAP_DATA_READ_TBL("PA0002", "|", "", "PERNR NACHN VORNA", "PERNR EQ '*" & rdt("ID_OPRT").ToString & "'")
                        If dtPA0002 Is Nothing Then 'Throw New Exception("pas de nom/prénom trouvés pour le matricule " & rdt("ID_MTCL").ToString)
                            para = New Paragraph(New Chunk("", FontFactory.GetFont("CALIBRI", 10)))
                        Else
                            para = New Paragraph(New Chunk(Trim(dtPA0002(0)("NACHN").ToString) & " " & Trim(dtPA0002(0)("VORNA").ToString), FontFactory.GetFont("CALIBRI", 10)))
                        End If
                        c_ENTE = New PdfPCell(para)
                        pt.AddCell(c_ENTE)
                        para = New Paragraph(New Chunk(COMM_APP_WEB_CONV_FORM_DATE(rdt("DT_ITVT").ToString, "dd/MM/yyyy"), FontFactory.GetFont("CALIBRI", 10)))
                        c_ENTE = New PdfPCell(para)
                        pt.AddCell(c_ENTE)
                    Next
                Else
                    For i = 1 To 9
                        para = New Paragraph(New Chunk("
", FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                        c_ENTE = New PdfPCell(para)
                        pt.AddCell(c_ENTE)
                    Next
                End If
                pt.SpacingAfter = 30
                dPDF.Add(pt)

                'autre
                para = New Paragraph(New Chunk(“4.2. Autres écarts :”, FontFactory.GetFont("CALIBRI", 12, Font.BOLD Or Font.UNDERLINE)))
                para.SpacingAfter = 20
                dPDF.Add(para)

                pt = New PdfPTable(5)
                pt.LockedWidth = True
                pt.TotalWidth = dPDF.PageSize.Width - 80
                Dim widths_6() As Single = {4, 3.5, 3, 3, 2}
                pt.SetWidths(widths_6)
                para = New Paragraph(New Chunk("Description", FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk("Action à réaliser", FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                phr = New Phrase
                phr.Add(New Chunk("Action réalisée par ", FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                phr.Add(New Chunk("(nom & date)", FontFactory.GetFont("CALIBRI", 10)))
                c_ENTE = New PdfPCell(phr)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                phr = New Phrase
                phr.Add(New Chunk("Action contrôlée par ", FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                phr.Add(New Chunk("(nom & date)", FontFactory.GetFont("CALIBRI", 10)))
                c_ENTE = New PdfPCell(phr)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                phr = New Phrase
                phr.Add(New Chunk("Résultat ", FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                phr.Add(New Chunk("(OK/NOK)", FontFactory.GetFont("CALIBRI", 10)))
                c_ENTE = New PdfPCell(phr)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                sQuery = "SELECT [NM_DCPO_ANML]
      ,[NM_ACTI_REAL]
      ,[ID_OPRT_REAL]
      ,[ID_OPRT_CONT]
,[ID_STAT]
      ,[DT_ACTI_CONT]
      ,[DT_ACTI_REAL]
  FROM [dbo].[V_DHR_ECAR_PDTO]
 WHERE [NU_SER_SPFE] = '" & sNU_SER & "'"

                dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion_ALMS)
                If Not dt Is Nothing Then
                    For Each rdt As DataRow In dt.Rows
                        para = New Paragraph(New Chunk(rdt("NM_DCPO_ANML").ToString, FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                        c_ENTE = New PdfPCell(para)
                        pt.AddCell(c_ENTE)
                        para = New Paragraph(New Chunk(rdt("NM_ACTI_REAL").ToString, FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                        c_ENTE = New PdfPCell(para)
                        pt.AddCell(c_ENTE)
                        dtPA0002 = SAP_DATA_READ_TBL("PA0002", "|", "", "PERNR NACHN VORNA", "PERNR EQ '*" & rdt("ID_OPRT_REAL").ToString & "'")
                        If dtPA0002 Is Nothing Then 'Throw New Exception("pas de nom/prénom trouvés pour le matricule " & rdt("ID_MTCL").ToString)
                            para = New Paragraph(New Chunk("", FontFactory.GetFont("CALIBRI", 10)))
                        Else
                            para = New Paragraph(New Chunk(Trim(dtPA0002(0)("NACHN").ToString) & " " & Trim(dtPA0002(0)("VORNA").ToString) & " le " & COMM_APP_WEB_CONV_FORM_DATE(rdt("DT_ACTI_REAL").ToString, "dd/MM/yyyy"), FontFactory.GetFont("CALIBRI", 10)))
                        End If
                        c_ENTE = New PdfPCell(para)
                        pt.AddCell(c_ENTE)
                        dtPA0002 = SAP_DATA_READ_TBL("PA0002", "|", "", "PERNR NACHN VORNA", "PERNR EQ '*" & rdt("ID_OPRT_CONT").ToString & "'")
                        If dtPA0002 Is Nothing Then 'Throw New Exception("pas de nom/prénom trouvés pour le matricule " & rdt("ID_MTCL").ToString)
                            para = New Paragraph(New Chunk("", FontFactory.GetFont("CALIBRI", 10)))
                        Else
                            para = New Paragraph(New Chunk(Trim(dtPA0002(0)("NACHN").ToString) & " " & Trim(dtPA0002(0)("VORNA").ToString) & " le " & COMM_APP_WEB_CONV_FORM_DATE(rdt("DT_ACTI_CONT").ToString, "dd/MM/yyyy"), FontFactory.GetFont("CALIBRI", 10)))
                        End If
                        c_ENTE = New PdfPCell(para)
                        pt.AddCell(c_ENTE)
                        para = New Paragraph(New Chunk(rdt("ID_STAT").ToString, FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                        c_ENTE = New PdfPCell(para)
                        pt.AddCell(c_ENTE)
                    Next
                Else
                    For i = 1 To 15
                        para = New Paragraph(New Chunk("
", FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                        c_ENTE = New PdfPCell(para)
                        pt.AddCell(c_ENTE)
                    Next
                End If
                pt.SpacingAfter = 30
                dPDF.Add(pt)

                'liste de documents 
                para = New Paragraph(New Chunk(“5. LISTE DES DOCUMENTS ASSOCIES”, FontFactory.GetFont("CALIBRI", 14, Font.BOLD Or Font.UNDERLINE)))
                para.SpacingAfter = 20
                dPDF.Add(para)

                pt = New PdfPTable(2)
                pt.LockedWidth = True
                pt.TotalWidth = dPDF.PageSize.Width - 160
                Dim widths_7() As Single = {4, 1}
                pt.SetWidths(widths_7)
                para = New Paragraph(New Chunk("Documents du présent rapport", FontFactory.GetFont("CALIBRI", 11, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk("Nb de page", FontFactory.GetFont("CALIBRI", 11, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                'sdatefcgf = COMM_APP_WEB_CONV_FORM_DATE(sdatefcgf, "ddMMyyyy_HHmmss")


                Dim PdfReader As PdfReader
                Dim sfihcsauv As String = DIG_FACT_SQL_GET_PARA(Trim(Label_CD_ARTI.Text), "Chemin de sauvegarde du fichier PDF")
                Dim sfichfcgf = “C:\sources\temp_App_Web\" & CInt(Int((10000000 * Rnd()) + 1)) & ".pdf”
                COMM_APP_WEB_COPY_FILE(sfihcsauv & "\OF " & sOF & "\" & sNU_SER & "_" & sdatefcgf & ".pdf", sfichfcgf, True)
                PdfReader = New PdfReader(sfichfcgf)
                Dim numberOfPages As Integer = PdfReader.NumberOfPages

                para = New Paragraph(New Chunk("Annexe 1 - Fiche de contrôle globale de fonctionnement " & sNU_SER & "_" & sdatefcgf & ".pdf", FontFactory.GetFont("CALIBRI", 11)))
                c_ENTE = New PdfPCell(para)
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk(numberOfPages.ToString, FontFactory.GetFont("CALIBRI", 11)))
                c_ENTE = New PdfPCell(para)
                pt.AddCell(c_ENTE)

                'COMM_APP_WEB_COPY_FILE(Session("listetracabilite"), sfihcsauv & "\OF " & Label_OF.Text & "\" & sNU_SER & " – NM.pdf", True)

                '???
                sQuery = "SELECT MAX(CONVERT(NVARCHAR,[Date_Saisie])) AS A
                  ,[Rapport_Contrôle] 
              FROM [SAISIES_DEFAUTS].[dbo].[T_Defauts]
             WHERE [N°_série] = '" & sNU_SER & "' AND [OF] = '" & sOF & "'
            GROUP BY [OF]
                  ,[N°_série]   
                  ,[Rapport_Contrôle]"
                dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
                If dt Is Nothing Then Throw New Exception("Pas de rapport de contrôle trouvé")
                para = New Paragraph(New Chunk("Annexe 2 - Rapport de contrôle n°" & dt(0)("Rapport_Contrôle").ToString, FontFactory.GetFont("CALIBRI", 11)))

                'para = New Paragraph(New Chunk("Annexe 2 - Rapport de contrôle n°123456", FontFactory.GetFont("CALIBRI", 11)))
                c_ENTE = New PdfPCell(para)
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk("1", FontFactory.GetFont("CALIBRI", 11)))
                c_ENTE = New PdfPCell(para)
                pt.AddCell(c_ENTE)

                Dim scheminlistetracabilité As String = Session("listetracabilite")
                PdfReader = New PdfReader(scheminlistetracabilité)
                numberOfPages = PdfReader.NumberOfPages
                para = New Paragraph(New Chunk("Annexe 3 - Liste traçabilité " & sNU_SER & " – NM.pdf", FontFactory.GetFont("CALIBRI", 11)))
                c_ENTE = New PdfPCell(para)
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk(numberOfPages.ToString, FontFactory.GetFont("CALIBRI", 11)))
                c_ENTE = New PdfPCell(para)
                pt.AddCell(c_ENTE)
                dPDF.Add(pt)
                dPDF.Close()
            End Using
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Return Nothing
        Finally
            'dPDF.Close()
        End Try
        Return sfich
    End Function

    Public Function CREA_LIST_TCBL(sNU_SER As String, sOF As String) As String
        Dim dt, dtKNMT, dtMSEG, dtMAKT As New DataTable
        Dim sQuery As String = ""
        'Dim dPDF As Document
        Dim pt As PdfPTable
        Dim c_ENTE As PdfPCell
        Randomize()
        'Dim iLOGO As Image
        Dim para As Paragraph
        'Dim ipagenumber As Integer.
        Dim sfich As String = “C:\sources\temp_App_Web\" & CInt(Int((10000000 * Rnd()) + 1)) & ".pdf”, sCD_ARTI_PROD As String = ""
        Try
            Session("sNU_SER") = sNU_SER
            Using dPDF = New Document(PageSize.A4, 20, 20, 30, 20)
                Dim writer = PdfAWriter.GetInstance(dPDF, New FileStream(sfich, FileMode.Create))

                Dim PageEventHandler As New HeaderFooter_LIST_TCBL()
                writer.PageEvent = PageEventHandler
                dPDF.Open()

                pt = New PdfPTable(2)

                With pt
                    .LockedWidth = True
                    .TotalWidth = dPDF.PageSize.Width - 100
                    Dim widths() As Single = {18, 26}
                    .SetWidths(widths)
                    para = New Paragraph(New Chunk(“Référence du produit : ”, FontFactory.GetFont("CALIBRI", 14, Font.BOLD)))
                    c_ENTE = New PdfPCell(para)
                    c_ENTE.Border = 0
                    c_ENTE.HorizontalAlignment = Element.ALIGN_RIGHT
                    .AddCell(c_ENTE)
                    para = New Paragraph(New Chunk(Label_DES_ARTI.Text & “ - ” & Label_CD_ARTI.Text, FontFactory.GetFont("CALIBRI", 14)))
                    c_ENTE = New PdfPCell(para)
                    c_ENTE.Border = 0
                    c_ENTE.BorderWidthBottom = 1
                    .AddCell(c_ENTE)
                    para = New Paragraph(New Chunk(“Référence ALMS : ”, FontFactory.GetFont("CALIBRI", 14, Font.BOLD)))
                    c_ENTE = New PdfPCell(para)
                    c_ENTE.Border = 0
                    c_ENTE.HorizontalAlignment = Element.ALIGN_RIGHT
                    .AddCell(c_ENTE)

                    dtKNMT = SAP_DATA_READ_KNMT("MATNR LIKE '" & Label_CD_ARTI.Text & "%' AND KUNNR EQ '0000000451' AND VKORG EQ 'ORC3' AND VTWEG EQ 'CD'")
                    If dtKNMT Is Nothing Then Throw New Exception("Code article client introuvable sous SAP")
                    sCD_ARTI_PROD = Trim(dtKNMT(0)("KDMAT").ToString)
                    para = New Paragraph(New Chunk(sCD_ARTI_PROD, FontFactory.GetFont("CALIBRI", 14)))
                    c_ENTE = New PdfPCell(para)
                    c_ENTE.Border = 0
                    c_ENTE.BorderWidthBottom = 1
                    .AddCell(c_ENTE)
                    para = New Paragraph(New Chunk(“Numéro de série : ”, FontFactory.GetFont("CALIBRI", 14, Font.BOLD)))
                    c_ENTE = New PdfPCell(para)
                    c_ENTE.Border = 0
                    c_ENTE.HorizontalAlignment = Element.ALIGN_RIGHT
                    .AddCell(c_ENTE)
                    para = New Paragraph(New Chunk(sNU_SER, FontFactory.GetFont("CALIBRI", 14)))
                    c_ENTE = New PdfPCell(para)
                    c_ENTE.Border = 0
                    c_ENTE.BorderWidthBottom = 1
                    .AddCell(c_ENTE)
                    .SpacingAfter = 30
                    dPDF.Add(pt)
                End With

                pt = New PdfPTable(3)
                pt.LockedWidth = True
                pt.TotalWidth = dPDF.PageSize.Width - 80
                Dim widths_2() As Single = {1, 3, 1}
                pt.SetWidths(widths_2)
                para = New Paragraph(New Chunk(“Code article”, FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk(“Désignation”, FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk(“N° de série unitaire ou n° de lot”, FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)

                'ajout traça lot
                Dim dt_MSEG_CLEAN As New DataTable
                With dt_MSEG_CLEAN
                    .Columns.Add("MATNR", Type.GetType("System.String"))
                    .Columns.Add("CHARG", Type.GetType("System.String"))
                    .Columns.Add("MENGE", Type.GetType("System.String"))
                    dtMSEG = SAP_DATA_READ_MSEG("AUFNR LIKE '%" & sOF & "'")
                    'dtMSEG.DefaultView.Sort = "MATNR ASC"
                    'dtMSEG = dtMSEG.DefaultView.ToTable
                    For Each rdtMSEG As DataRow In dtMSEG.Rows
                        Dim rdt_MSEG_CLEAN As DataRow = .Select("MATNR LIKE '%" & Trim(rdtMSEG("MATNR").ToString) & "%' AND CHARG = '" & rdtMSEG("CHARG").ToString & "'").FirstOrDefault()
                        If rdt_MSEG_CLEAN Is Nothing Then
                            .Rows.Add()
                            .Rows(.Rows.Count - 1)("MATNR") = Trim(rdtMSEG("MATNR").ToString)
                            .Rows(.Rows.Count - 1)("CHARG") = rdtMSEG("CHARG").ToString
                            .Rows(.Rows.Count - 1)("MENGE") = rdtMSEG("MENGE").ToString
                        Else
                            If rdtMSEG("BWART").ToString = "262" Then
                                rdt_MSEG_CLEAN("MENGE") = (Convert.ToDecimal(Replace(rdt_MSEG_CLEAN("MENGE").ToString, ".", ",")) - Convert.ToDecimal(Replace(rdtMSEG("MENGE").ToString, ".", ","))).ToString
                            Else
                                rdt_MSEG_CLEAN("MENGE") = (Convert.ToDecimal(Replace(rdt_MSEG_CLEAN("MENGE").ToString, ".", ",")) + Convert.ToDecimal(Replace(rdtMSEG("MENGE").ToString, ".", ","))).ToString
                            End If
                        End If
                    Next
                    '.DefaultView.Sort = "MATNR ASC"
                    'dt_MSEG_CLEAN = .DefaultView.ToTable
                End With
                sQuery = "Select     dbo.DTM_NMCT_PROD.NU_SER_COMP, dbo.DTM_NMCT_PROD.CD_ARTI_COMP, dbo.DTM_NMCT_PROD.NM_DSGT_COMP
FROM         dbo.DTM_NMCT_PROD INNER JOIN
                          (SELECT     NU_SER_SPFE, CD_ARTI_COMP, MAX(DT_AFCT) AS MAX_DT_AFCT
                            FROM          dbo.DTM_NMCT_PROD AS DTM_NMCT_PROD_1
                            GROUP BY NU_SER_SPFE, CD_ARTI_COMP) AS dt_DERN_AFCT ON dt_DERN_AFCT.NU_SER_SPFE = dbo.DTM_NMCT_PROD.NU_SER_SPFE AND 
                      dt_DERN_AFCT.CD_ARTI_COMP = dbo.DTM_NMCT_PROD.CD_ARTI_COMP AND dt_DERN_AFCT.MAX_DT_AFCT = dbo.DTM_NMCT_PROD.DT_AFCT
WHERE     (dbo.DTM_NMCT_PROD.NU_SER_SPFE = '" & sNU_SER & "') AND [BL_TYPE_TCBL] = 1"
                dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion_ALMS)
                For Each rdtMSEG As DataRow In dt_MSEG_CLEAN.Select("MENGE <> '0'")
                    dtMAKT = SAP_DATA_READ_MAKT("MATNR LIKE '" & rdtMSEG("MATNR").ToString & "%'")
                    If dtMAKT Is Nothing Then Throw New Exception("Désignation article introuvable")
                    If Not dt.Select("NM_DSGT_COMP = '" & dtMAKT(0)("MAKTX").ToString & "'").FirstOrDefault Is Nothing Then Continue For
                    If Trim(rdtMSEG("MATNR").ToString) = "TAEKC042100$" Then Continue For
                    dtKNMT = SAP_DATA_READ_KNMT("MATNR LIKE '" & Trim(rdtMSEG("MATNR").ToString) & "%'")
                    If dtKNMT Is Nothing Then
                        sQuery = "INSERT INTO [dbo].[DTM_NMCT_PROD]
           ([NU_SER_SPFE]
           ,[CD_ARTI_PROD]
           ,[NU_SER_COMP]
           ,[CD_ARTI_COMP]
           ,[BL_TYPE_TCBL]
           ,[DT_AFCT]
           ,[NM_DSGT_COMP])
VALUES ('" & sNU_SER & "'
       ,'" & sCD_ARTI_PROD & "'
       ,'" & Trim(rdtMSEG("CHARG").ToString) & "'
       ,'" & Trim(rdtMSEG("MATNR").ToString) & "'
       ," & 0 & "
       ,'" & Session("DT_DEB") & "'
       ,'" & Trim(dtMAKT(0)("MAKTX").ToString) & "')"
                    Else
                        sQuery = "INSERT INTO [dbo].[DTM_NMCT_PROD]
           ([NU_SER_SPFE]
           ,[CD_ARTI_PROD]
           ,[NU_SER_COMP]
           ,[CD_ARTI_COMP]
           ,[BL_TYPE_TCBL]
           ,[DT_AFCT]
           ,[NM_DSGT_COMP])
VALUES ('" & sNU_SER & "'
       ,'" & sCD_ARTI_PROD & "'
       ,'" & Trim(rdtMSEG("CHARG").ToString) & "'
       ,'" & Trim(dtKNMT(0)("KDMAT").ToString) & "'
       ," & 0 & "
       ,'" & Session("DT_DEB") & "'
       ,'" & Trim(dtMAKT(0)("MAKTX").ToString) & "')"
                    End If
                    SQL_REQ_ACT(sQuery, sChaineConnexion_ALMS)
                Next

                'ajout traça unitaire
                sQuery = "Select     dbo.DTM_NMCT_PROD.NU_SER_COMP, dbo.DTM_NMCT_PROD.CD_ARTI_COMP, dbo.DTM_NMCT_PROD.NM_DSGT_COMP
FROM         dbo.DTM_NMCT_PROD INNER JOIN
                          (SELECT     NU_SER_SPFE, CD_ARTI_COMP, MAX(DT_AFCT) AS MAX_DT_AFCT
                            FROM          dbo.DTM_NMCT_PROD AS DTM_NMCT_PROD_1
                            GROUP BY NU_SER_SPFE, CD_ARTI_COMP) AS dt_DERN_AFCT ON dt_DERN_AFCT.NU_SER_SPFE = dbo.DTM_NMCT_PROD.NU_SER_SPFE AND 
                      dt_DERN_AFCT.CD_ARTI_COMP = dbo.DTM_NMCT_PROD.CD_ARTI_COMP AND dt_DERN_AFCT.MAX_DT_AFCT = dbo.DTM_NMCT_PROD.DT_AFCT
WHERE     (dbo.DTM_NMCT_PROD.NU_SER_SPFE = '" & sNU_SER & "' AND dbo.DTM_NMCT_PROD.NU_SER_COMP <> '')
ORDER BY [BL_TYPE_TCBL] DESC, [CD_ARTI_COMP], NU_SER_COMP"
                dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion_ALMS)
                For Each rdt As DataRow In dt.Rows
                    para = New Paragraph(New Chunk(rdt("CD_ARTI_COMP").ToString, FontFactory.GetFont("CALIBRI", 11)))
                    c_ENTE = New PdfPCell(para)
                    pt.AddCell(c_ENTE)
                    para = New Paragraph(New Chunk(rdt("NM_DSGT_COMP").ToString, FontFactory.GetFont("CALIBRI", 11)))
                    c_ENTE = New PdfPCell(para)
                    pt.AddCell(c_ENTE)
                    para = New Paragraph(New Chunk(rdt("NU_SER_COMP").ToString, FontFactory.GetFont("CALIBRI", 11)))
                    c_ENTE = New PdfPCell(para)
                    pt.AddCell(c_ENTE)
                Next

                'Dim sum As Integer
                'Dim dt_MSEG_CLEAN As New DataTable
                'With dt_MSEG_CLEAN
                '    .Columns.Add("MATNR", Type.GetType("System.String"))
                '    .Columns.Add("CHARG", Type.GetType("System.String"))
                '    .Columns.Add("MENGE", Type.GetType("System.String"))
                '    dtMSEG = SAP_DATA_READ_MSEG("AUFNR LIKE '%" & sOF & "'")
                '    dtMSEG.DefaultView.Sort = "MATNR ASC"
                '    dtMSEG = dtMSEG.DefaultView.ToTable
                '    For Each rdtMSEG As DataRow In dtMSEG.Rows
                '        Dim rdt_MSEG_CLEAN As DataRow = .Select("MATNR LIKE '%" & Trim(rdtMSEG("MATNR").ToString) & "%' AND CHARG = '" & rdtMSEG("CHARG").ToString & "'").FirstOrDefault()
                '        If rdt_MSEG_CLEAN Is Nothing Then
                '            .Rows.Add()
                '            .Rows(.Rows.Count - 1)("MATNR") = Trim(rdtMSEG("MATNR").ToString)
                '            .Rows(.Rows.Count - 1)("CHARG") = rdtMSEG("CHARG").ToString
                '            .Rows(.Rows.Count - 1)("MENGE") = rdtMSEG("MENGE").ToString
                '        Else
                '            If rdtMSEG("BWART").ToString = "262" Then
                '                rdt_MSEG_CLEAN("MENGE") = (Convert.ToDecimal(Replace(rdt_MSEG_CLEAN("MENGE").ToString, ".", ",")) - Convert.ToDecimal(Replace(rdtMSEG("MENGE").ToString, ".", ","))).ToString
                '            Else
                '                rdt_MSEG_CLEAN("MENGE") = (Convert.ToDecimal(Replace(rdt_MSEG_CLEAN("MENGE").ToString, ".", ",")) + Convert.ToDecimal(Replace(rdtMSEG("MENGE").ToString, ".", ","))).ToString
                '            End If
                '        End If
                '    Next
                '    .DefaultView.Sort = "MATNR ASC"
                '    dt_MSEG_CLEAN = .DefaultView.ToTable
                'End With


                'For Each rdtMSEG As DataRow In dt_MSEG_CLEAN.Select("MENGE <> '0'")
                '    dtMAKT = SAP_DATA_READ_MAKT("MATNR LIKE '" & rdtMSEG("MATNR").ToString & "%'")
                '    If dtMAKT Is Nothing Then Throw New Exception("Désignation article introuvable")
                '    If Not dt.Select("NM_DSGT_COMP = '" & dtMAKT(0)("MAKTX").ToString & "'").FirstOrDefault Is Nothing Then Continue For

                '    If Trim(rdtMSEG("MATNR").ToString) = "TAEKC042100$" Then Continue For
                '    dtKNMT = SAP_DATA_READ_KNMT("MATNR LIKE '" & Trim(rdtMSEG("MATNR").ToString) & "%'")
                '    If dtKNMT Is Nothing Then
                '        para = New Paragraph(New Chunk(Trim(rdtMSEG("MATNR").ToString), FontFactory.GetFont("CALIBRI", 11)))
                '    Else
                '        para = New Paragraph(New Chunk(Trim(dtKNMT(0)("KDMAT").ToString), FontFactory.GetFont("CALIBRI", 11)))
                '        'Continue For 'Throw New Exception("Code article client introuvable sous SAP")
                '    End If
                '    c_ENTE = New PdfPCell(para)
                '    pt.AddCell(c_ENTE)
                '    'dtMAKT = SAP_DATA_READ_MAKT("MATNR LIKE '" & rdtMSEG("MATNR").ToString & "%'")
                '    'If dtMAKT Is Nothing Then Throw New Exception("Désgnation article introuvable")
                '    para = New Paragraph(New Chunk(Trim(dtMAKT(0)("MAKTX").ToString), FontFactory.GetFont("CALIBRI", 11)))
                '    c_ENTE = New PdfPCell(para)
                '    pt.AddCell(c_ENTE)
                '    para = New Paragraph(New Chunk(rdtMSEG("CHARG").ToString, FontFactory.GetFont("CALIBRI", 11)))
                '    c_ENTE = New PdfPCell(para)
                '    pt.AddCell(c_ENTE)
                'Next

                pt.HeaderRows = 1
                pt.SpacingAfter = 30
                dPDF.Add(pt)
                dPDF.Close()
            End Using
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Return Nothing
        Finally
            'dPDF.Close() 'Ventilateur Vendome ST40 IND01X 08/12/16
        End Try
        Return sfich
    End Function


End Class
Public Class HeaderFooter_FCGF
    Inherits PdfPageEventHelper

    ' C'est un conteneur de texte
    Private cb As PdfContentByte

    ' we will put the final number of pages in a template
    Private template As PdfTemplate

    ' this is the BaseFont we are going to use for the header / footer
    Private bf As BaseFont = Nothing

    ' we override the onOpenDocument method
    Public Overrides Sub OnOpenDocument(ByVal writer As PdfWriter, ByVal document As Document)
        Try
            bf = pdf.BaseFont.CreateFont(pdf.BaseFont.HELVETICA, pdf.BaseFont.CP1252, pdf.BaseFont.NOT_EMBEDDED)
            cb = writer.DirectContent
            ' Création de la plage qui recevra le nbre total de pages du document (vu dans le pied de page).
            template = cb.CreateTemplate(50, 50)
        Catch de As DocumentException
        Catch ioe As System.IO.IOException
        End Try
    End Sub

    Public Overrides Sub OnStartPage(ByVal writer As PdfWriter, ByVal document As Document)
        MyBase.OnStartPage(writer, document)

        Dim pt_ENTE = New PdfPTable(3)
        pt_ENTE.TotalWidth = document.PageSize.Width - 40
        pt_ENTE.LockedWidth = True
        Dim widths() As Single = {10, 13, 10}
        pt_ENTE.SetWidths(widths)

        Dim pt = New PdfPTable(1)
        pt.DefaultCell.Border = Rectangle.NO_BORDER
        Dim iLOGO = Image.GetInstance("C:\sources\Digital Factory\Etiquettes\ALM\air-liquide-healthcare-logo.jpg")
        pt.AddCell(iLOGO)
        Dim para = New Paragraph(New Chunk(“Direction Industrielle”, FontFactory.GetFont("ARIAL", 12, Font.BOLD)))
        Dim c_ENTE = New PdfPCell(para)
        c_ENTE.Border = 0
        c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
        pt.AddCell(c_ENTE)
        pt_ENTE.AddCell(pt)

        para = New Paragraph(New Chunk(“FICHE DE CONTROLE GLOBAL DE FONCTIONNEMENT”, FontFactory.GetFont("ARIAL", 16, Font.BOLD)))
        c_ENTE = New PdfPCell(para)
        c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
        c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
        pt_ENTE.AddCell(c_ENTE)

        pt = New PdfPTable(2)

        Dim sFDV As String = System.Web.HttpContext.Current.Session("sFDV")
        Dim sVers_FDV As String = System.Web.HttpContext.Current.Session("sVers_FDV")
        Dim sDSGT_ARTI As String = System.Web.HttpContext.Current.Session("sDSGT_ARTI")

        para = New Paragraph(New Chunk(“Référence”, FontFactory.GetFont("ARIAL", 10)))
        c_ENTE = New PdfPCell(para)
        c_ENTE.Border = 0
        pt.AddCell(c_ENTE)
        para = New Paragraph(New Chunk(sFDV, FontFactory.GetFont("ARIAL", 10, Font.BOLD))) 'FDV
        c_ENTE = New PdfPCell(para)
        c_ENTE.Border = 0
        pt.AddCell(c_ENTE)
        para = New Paragraph(New Chunk("Révision", FontFactory.GetFont("ARIAL", 10)))
        c_ENTE = New PdfPCell(para)
        c_ENTE.Border = 0
        pt.AddCell(c_ENTE)
        para = New Paragraph(New Chunk(sVers_FDV, FontFactory.GetFont("ARIAL", 10, Font.BOLD))) 'Version FDV
        c_ENTE = New PdfPCell(para)
        c_ENTE.Border = 0
        pt.AddCell(c_ENTE)
        'para = New Paragraph(New Chunk("Date", FontFactory.GetFont("ARIAL", 10)))
        'c_ENTE = New PdfPCell(para)
        'c_ENTE.Border = 0
        'pt.AddCell(c_ENTE)
        'para = New Paragraph(New Chunk(Left(Now.ToString, 10), FontFactory.GetFont("ARIAL", 10, Font.BOLD)))
        'c_ENTE = New PdfPCell(para)
        'c_ENTE.Border = 0
        'pt.AddCell(c_ENTE)
        para = New Paragraph(New Chunk("Propriétaire", FontFactory.GetFont("ARIAL", 10)))
        c_ENTE = New PdfPCell(para)
        c_ENTE.Border = 0
        pt.AddCell(c_ENTE)
        para = New Paragraph(New Chunk("ALMS - M&I", FontFactory.GetFont("ARIAL", 10, Font.BOLD)))
        c_ENTE = New PdfPCell(para)
        c_ENTE.Border = 0
        pt.AddCell(c_ENTE)

        pt_ENTE.AddCell(pt)

        para = New Paragraph(New Chunk(sDSGT_ARTI, FontFactory.GetFont("ARIAL", 18, Font.BOLD)))
        c_ENTE = New PdfPCell(para)
        c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
        c_ENTE.Colspan = 3
        pt_ENTE.AddCell(c_ENTE)
        pt_ENTE.SpacingAfter = 10

        document.Add(pt_ENTE)

    End Sub


    Private moBF As BaseFont = pdf.BaseFont.CreateFont(pdf.BaseFont.HELVETICA, pdf.BaseFont.CP1252, pdf.BaseFont.NOT_EMBEDDED)

    Public Overrides Sub OnEndPage(ByVal writer As PdfWriter, ByVal document As Document)
        MyBase.OnEndPage(writer, document)

        '*** PIED DE PAGE *************
        'Affiche le N° de page 
        Dim oTable As New PdfPTable(1)
        ' Dim coprt As TCBL_OPRT
        With oTable
            Dim iPageNumber As Integer = writer.PageNumber
            Dim sText As String = "Page " & iPageNumber.ToString & " sur "

            oTable.TotalWidth = document.PageSize.Width - 70
            oTable.WriteSelectedRows(0, -1, 36, 15, writer.DirectContent)

            Dim fLen As Single = moBF.GetWidthPoint(sText, 10)
            cb.BeginText()
            cb.SetFontAndSize(moBF, 8)
            cb.SetTextMatrix(40, 16)
            '
            Dim sNU_SER As String = System.Web.HttpContext.Current.Session("sNU_SER")
            cb.ShowText("SN Produit : " & sNU_SER)
            cb.SetTextMatrix(document.PageSize.Width / 2, 16)
            cb.ShowText("FCGF_V1.1")
            cb.SetTextMatrix(document.PageSize.Width - 90, 16)
            cb.ShowText(sText)
            cb.EndText()
            cb.AddTemplate(template, document.PageSize.Width - 100 + fLen, 16)

            'Dim pt As New PdfPTable(4)
            'Dim para = New Paragraph(New Chunk("SN Produit : ", FontFactory.GetFont("ARIAL", 10)))
            'Dim c_ENTE = New PdfPCell(para)
            'pt.AddCell(c_ENTE)

            'para = New Paragraph(New Chunk("VE40-000114", FontFactory.GetFont("ARIAL", 10, Font.BOLD)))
            'c_ENTE = New PdfPCell(para)
            'pt.AddCell(c_ENTE)

            'para = New Paragraph(New Chunk("FCGF_V1", FontFactory.GetFont("ARIAL", 10)))
            'c_ENTE = New PdfPCell(para)
            'pt.AddCell(c_ENTE)

            'para = New Paragraph(New Chunk("cpoucou", FontFactory.GetFont("ARIAL", 10)))
            'c_ENTE = New PdfPCell(para)
            'pt.AddCell(c_ENTE)

            'document.Add(pt)
        End With

    End Sub

    ' Récupère le total des pages du PDF
    Public Overrides Sub OnCloseDocument(ByVal writer As PdfWriter, ByVal document As Document)
        MyBase.OnCloseDocument(writer, document)
        template.BeginText()
        template.SetFontAndSize(bf, 8)
        template.SetTextMatrix(0, 0)
        template.ShowText("" & Convert.ToString((writer.PageNumber)))
        template.EndText()
    End Sub

End Class

Public Class HeaderFooter_DHR
    Inherits PdfPageEventHelper

    ' C'est un conteneur de texte
    Private cb As PdfContentByte

    ' we will put the final number of pages in a template
    Private template As PdfTemplate

    ' this is the BaseFont we are going to use for the header / footer
    Private bf As BaseFont = Nothing

    ' we override the onOpenDocument method
    Public Overrides Sub OnOpenDocument(ByVal writer As PdfWriter, ByVal document As Document)
        Try
            bf = pdf.BaseFont.CreateFont(pdf.BaseFont.HELVETICA, pdf.BaseFont.CP1252, pdf.BaseFont.NOT_EMBEDDED)
            cb = writer.DirectContent
            ' Création de la plage qui recevra le nbre total de pages du document (vu dans le pied de page).
            template = cb.CreateTemplate(50, 50)
        Catch de As DocumentException
        Catch ioe As System.IO.IOException
        End Try
    End Sub

    Public Overrides Sub OnStartPage(ByVal writer As PdfWriter, ByVal document As Document)
        MyBase.OnStartPage(writer, document)
        Dim c_ENTE As PdfPCell
        Dim pt_ENTE = New PdfPTable(3)
        pt_ENTE.TotalWidth = document.PageSize.Width - 40
        pt_ENTE.LockedWidth = True
        Dim widths() As Single = {5, 20, 5}
        pt_ENTE.SetWidths(widths)

        Dim pt = New PdfPTable(1)
        pt.DefaultCell.Border = Rectangle.NO_BORDER

        Dim iLOGO = Image.GetInstance("C:\sources\Digital Factory\Etiquettes\Combrée-1.jpg")

        pt.AddCell(iLOGO)
        Dim para = New Paragraph(New Chunk(“I 369 - B”, FontFactory.GetFont("CALIBRI", 11, Font.BOLD)))
        c_ENTE = New PdfPCell(para)
        c_ENTE.Border = 0
        c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
        pt.AddCell(c_ENTE)
        pt_ENTE.AddCell(pt)

        para = New Paragraph(New Chunk(“Synthèse de Device History Record”, FontFactory.GetFont("CALIBRI", 20, Font.BOLD)))
        c_ENTE = New PdfPCell(para)
        c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
        c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
        pt_ENTE.AddCell(c_ENTE)
        pt_ENTE.WidthPercentage = 100

        para = New Paragraph(New Chunk(“A archiver par le contrôle final par OF”, FontFactory.GetFont("CALIBRI", 11, Font.BOLD)))
        c_ENTE = New PdfPCell(para)
        c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
        c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
        pt_ENTE.AddCell(c_ENTE)
        pt_ENTE.SpacingAfter = 30
        document.Add(pt_ENTE)

    End Sub


    Private moBF As BaseFont = pdf.BaseFont.CreateFont(pdf.BaseFont.HELVETICA, pdf.BaseFont.CP1252, pdf.BaseFont.NOT_EMBEDDED)

    Public Overrides Sub OnEndPage(ByVal writer As PdfWriter, ByVal document As Document)
        MyBase.OnEndPage(writer, document)

        '*** PIED DE PAGE *************
        'Affiche le N° de page 
        Dim oTable As New PdfPTable(1)
        With oTable
            Dim iPageNumber As Integer = writer.PageNumber
            Dim sText As String = "Page " & iPageNumber & " sur "

            oTable.TotalWidth = document.PageSize.Width - 70
            oTable.WriteSelectedRows(0, -1, 36, 15, writer.DirectContent)

            Dim fLen As Single = moBF.GetWidthPoint(sText, 10)
            cb.BeginText()
            cb.SetFontAndSize(moBF, 8)
            cb.SetTextMatrix(document.PageSize.Width - 90, 16)
            cb.ShowText(sText)
            cb.EndText()
            cb.AddTemplate(template, document.PageSize.Width - 100 + fLen, 16)
        End With
    End Sub

    ' Récupère le total des pages du PDF
    Public Overrides Sub OnCloseDocument(ByVal writer As PdfWriter, ByVal document As Document)
        MyBase.OnCloseDocument(writer, document)
        template.BeginText()
        template.SetFontAndSize(bf, 8)
        template.SetTextMatrix(0, 0)
        template.ShowText("" & Convert.ToString((writer.PageNumber)))
        template.EndText()
    End Sub

End Class

Public Class HeaderFooter_LIST_TCBL
    Inherits PdfPageEventHelper

    ' C'est un conteneur de texte
    Private cb As PdfContentByte

    ' we will put the final number of pages in a template
    Private template As PdfTemplate

    ' this is the BaseFont we are going to use for the header / footer
    Private bf As BaseFont = Nothing

    ' we override the onOpenDocument method
    Public Overrides Sub OnOpenDocument(ByVal writer As PdfWriter, ByVal document As Document)
        Try
            bf = pdf.BaseFont.CreateFont(pdf.BaseFont.HELVETICA, pdf.BaseFont.CP1252, pdf.BaseFont.NOT_EMBEDDED)
            cb = writer.DirectContent
            ' Création de la plage qui recevra le nbre total de pages du document (vu dans le pied de page).
            template = cb.CreateTemplate(50, 50)
        Catch de As DocumentException
        Catch ioe As System.IO.IOException
        End Try
    End Sub

    Public Overrides Sub OnStartPage(ByVal writer As PdfWriter, ByVal document As Document)
        MyBase.OnStartPage(writer, document)

        Dim pt_ENTE = New PdfPTable(1)
        pt_ENTE.TotalWidth = document.PageSize.Width - 40
        Dim sNU_SER As String = System.Web.HttpContext.Current.Session("sNU_SER")
        Dim para = New Paragraph(New Chunk("Liste traçabilité " & sNU_SER & " – NM", FontFactory.GetFont("CALIBRI", 16, Font.BOLD)))
        Dim c_ENTE = New PdfPCell(para)
        c_ENTE.Border = 0
        c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
        pt_ENTE.AddCell(c_ENTE)
        pt_ENTE.SpacingAfter = 30
        document.Add(pt_ENTE)

    End Sub


    Private moBF As BaseFont = pdf.BaseFont.CreateFont(pdf.BaseFont.HELVETICA, pdf.BaseFont.CP1252, pdf.BaseFont.NOT_EMBEDDED)

    Public Overrides Sub OnEndPage(ByVal writer As PdfWriter, ByVal document As Document)
        MyBase.OnEndPage(writer, document)

        '*** PIED DE PAGE *************
        'Affiche le N° de page 
        Dim oTable As New PdfPTable(1)
        With oTable
            Dim iPageNumber As Integer = writer.PageNumber
            Dim sText As String = "Page " & iPageNumber & " sur "

            oTable.TotalWidth = document.PageSize.Width - 70
            oTable.WriteSelectedRows(0, -1, 36, 15, writer.DirectContent)

            Dim fLen As Single = moBF.GetWidthPoint(sText, 10)
            cb.BeginText()
            cb.SetFontAndSize(moBF, 8)
            cb.SetTextMatrix(document.PageSize.Width - 90, 16)
            cb.ShowText(sText)
            cb.EndText()
            cb.AddTemplate(template, document.PageSize.Width - 100 + fLen, 16)
        End With
    End Sub

    ' Récupère le total des pages du PDF
    Public Overrides Sub OnCloseDocument(ByVal writer As PdfWriter, ByVal document As Document)
        MyBase.OnCloseDocument(writer, document)
        template.BeginText()
        template.SetFontAndSize(bf, 8)
        template.SetTextMatrix(0, 0)
        template.ShowText("" & Convert.ToString((writer.PageNumber)))
        template.EndText()
    End Sub

End Class