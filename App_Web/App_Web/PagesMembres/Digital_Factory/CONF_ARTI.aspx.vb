Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports App_Web.Class_SAP_DATA
Imports App_Web.Class_DIG_FACT_SQL
Imports App_Web.Class_COMM_APP_WEB


Public Class CONF_ARTI
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("displayname") = "" Then
                Context.GetOwinContext().Authentication.SignOut(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie)
            Else
                If App_Web.Class_COMM_APP_WEB.COMM_APP_WEB_GET_DROI_PAGE(Replace(HttpContext.Current.Request.Url.AbsoluteUri, "http://" & LCase(My.Computer.Name) & "/PagesMembres/", "~/PagesMembres/") & ".aspx", Session("department"), Session("User_Name")) = False Then Response.Redirect("~/PagesMembres/RDRC_PAGE_MEMB.aspx")
            End If
        End If
        'LOG_Msg(GetCurrentMethod, sChaineConnexion)
    End Sub

    Protected Sub TextBox_CD_ARTI_ECO_TextChanged(sender As Object, e As EventArgs) Handles TextBox_CD_ARTI_ECO.TextChanged

        'Dim sClient As String = SAP_DATA_GET_CLIE_FROM_CD_ARTI(TextBox_CD_ARTI_ECO.Text)
        Dim sClient As String = ""
        Dim dt As New DataTable
        Try
            sClient = SAP_DATA_GET_CLIE_FROM_CD_ARTI(TextBox_CD_ARTI_ECO.Text)
            dt = DIG_FACT_SQL_CFGR_ARTI_ECO(TextBox_CD_ARTI_ECO.Text)

            TextBox_CD_ARTI_CLIE.Text = dt(0)("Code article client").ToString
            TextBox_NM_ARTI_CLIE.Text = dt(0)("Désignation article client").ToString
            Dim sOption_OF As String = DIG_FACT_SQL_GET_PARA(sClient, "OF")
            If sOption_OF Is Nothing Then sOption_OF = dt(0)("OF").ToString
            If sOption_OF = "1" Then
                CheckBox_SAI_OF.Checked = True
            Else
                CheckBox_SAI_OF.Checked = False
            End If
            Dim sOption_BL As String = DIG_FACT_SQL_GET_PARA(sClient, "BL")
            If sOption_BL Is Nothing Then sOption_BL = dt(0)("BL").ToString
            If sOption_BL = "1" Then
                CheckBox_SAI_BL.Checked = True
            Else
                CheckBox_SAI_BL.Checked = False
            End If
            Dim sOption_numerocarton As String = DIG_FACT_SQL_GET_PARA(sClient, "Numéro de carton")
            If sOption_numerocarton Is Nothing Then sOption_numerocarton = dt(0)("Numéro de carton").ToString
            If sOption_numerocarton = "1" Then
                CheckBox_SAI_NU_CART.Checked = True
            Else
                CheckBox_SAI_NU_CART.Checked = False
            End If
            Dim sOption_numeroeolane As String = DIG_FACT_SQL_GET_PARA(sClient, "Numéro de série Eolane")
            If sOption_numeroeolane Is Nothing Then sOption_numeroeolane = dt(0)("Numéro de série Eolane").ToString
            If sOption_numeroeolane = "1" Then
                CheckBox_SAI_NU_SER_ECO.Checked = True
            Else
                CheckBox_SAI_NU_SER_ECO.Checked = False
            End If
            Dim sOption_numeroclient As String = DIG_FACT_SQL_GET_PARA(sClient, "Numéro de série client")
            If sOption_numeroclient Is Nothing Then sOption_numeroclient = dt(0)("Numéro de série client").ToString
            If sOption_numeroclient = "1" Then
                CheckBox_SAI_NU_SER_CLIE.Checked = True
            Else
                CheckBox_SAI_NU_SER_CLIE.Checked = False
            End If
            Dim sOption_cartonspecifique As String = DIG_FACT_SQL_GET_PARA(sClient, "Carton spécifique")
            If sOption_cartonspecifique Is Nothing Then sOption_cartonspecifique = dt(0)("Carton spécifique").ToString
            If sOption_cartonspecifique = "1" Then
                CheckBox_SAI_NU_CART_SPEC.Checked = True
            Else
                CheckBox_SAI_NU_CART_SPEC.Checked = False
            End If
            If DIG_FACT_SQL_GET_PARA(sClient, "Code fournisseur") = "1" Then
                CheckBox_SAI_CD_FNS.Checked = True
            Else
                CheckBox_SAI_CD_FNS.Checked = False
            End If
            If dt(0)("Suivi numéro de série par ordre").ToString = "1" Then
                CheckBox_SUIV_NU_SER_ORDR_CROIS.Checked = True
            Else
                CheckBox_SUIV_NU_SER_ORDR_CROIS.Checked = False
            End If

            TextBox_FORM_NU_CMD.Text = dt(0)("Format du numéro de commande").ToString
            TextBox_FORM_NU_BL.Text = dt(0)("Format du numéro de BL").ToString
            TextBox_FORM_CD_CART.Text = dt(0)("Format du code carton").ToString
            TextBox_FORM_CD_FNS.Text = DIG_FACT_SQL_GET_PARA(sClient, "Format du code fournisseur")
            TextBox_FORM_NU_SER_CLIE.Text = dt(0)("Format Numéro de série client").ToString
            TextBox_FORM_NU_SER_ECO.Text = dt(0)("Format Numéro de série Eolane").ToString
            TextBox_QT_CART.Text = dt(0)("Quantité carton").ToString
            TextBox_QT_FLAN.Text = dt(0)("Quantité flanc").ToString
            TextBox_INC_NU_SER.Text = dt(0)("Incrémentation flanc").ToString

            If dt(0)("Retour saisie OF pour colisage").ToString = "1" Then
                CheckBox_SAI_OF_CART_TERM.Checked = True
            Else
                CheckBox_SAI_OF_CART_TERM.Checked = False
            End If
            TextBox_FORM_ETIQ_CART.Text = dt(0)("Format de étiquette Carton").ToString
            TextBox_ENCO_NU_SER_CLIE.Text = dt(0)("Encodage Numéro de série client").ToString
            TextBox_FORM_NU_SER_FNS.Text = dt(0)("Format Numéro de série Fournisseur").ToString
            TextBox_INDI_CLIE.Text = dt(0)("Indice client").ToString
            TextBox_NM_DDM.Text = dt(0)("Nom du DDM").ToString
            TextBox_SS_ENS_ASS_1.Text = dt(0)("Sous-ensemble associé n°1").ToString
            TextBox_SS_ENS_ASS_2.Text = dt(0)("Sous-ensemble associé n°2").ToString
            TextBox_SS_ENS_ASS_3.Text = dt(0)("Sous-ensemble associé n°3").ToString
            TextBox_SS_ENS_ASS_4.Text = dt(0)("Sous-ensemble associé n°4").ToString
            TextBox_SS_ENS_ASS_5.Text = dt(0)("Sous-ensemble associé n°5").ToString
            TextBox_SS_ENS_ASS_6.Text = dt(0)("Sous-ensemble associé n°6").ToString
            TextBox_FORM_NU_LOT.Text = dt(0)("Format Numéro de lot").ToString
            TextBox_QT_CART_PALE.Text = dt(0)("Quantité de carton dans la palette").ToString
            TextBox_NB_LIGN_FICH_PDF.Text = dt(0)("Nombre de ligne dans le fichier PDF").ToString
            TextBox_NM_CHEM_FICH_PDF.Text = dt(0)("Chemin de sauvegarde du fichier PDF").ToString
            Dim sARTI_ECO As String = TextBox_CD_ARTI_ECO.Text & StrDup(18 - Len(TextBox_CD_ARTI_ECO.Text), " ")
            TextBox_CHEM_FICH_IMPR_NU_SER_ECO.Text = COMM_APP_WEB_GET_PARA(sARTI_ECO & "|Numéro de série Eolane", "TextBox_FICH_MODE", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx")
            TextBox_CHEM_FICH_IMPR_NU_SER_CLIE.Text = COMM_APP_WEB_GET_PARA(sARTI_ECO & "|Numéro de série client", "TextBox_FICH_MODE", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx")
            TextBox_CHEM_FICH_IMPR_CART.Text = COMM_APP_WEB_GET_PARA(sARTI_ECO & "|Carton", "TextBox_FICH_MODE", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx")
            TextBox_CHEM_FICH_IMPR_PALE.Text = COMM_APP_WEB_GET_PARA(sARTI_ECO & "|Palette", "TextBox_FICH_MODE", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx")

            If dt(0)("Impression étiquette packaging numéro de série Eolane").ToString = "1" Then
                CheckBox_ETIQ_PACK_NU_SER_ECO.Checked = True
            Else
                CheckBox_ETIQ_PACK_NU_SER_ECO.Checked = False
            End If
            If dt(0)("Impression étiquette packaging numéro de série client").ToString = "1" Then
                CheckBox_ETIQ_PACK_NU_SER_CLIE.Checked = True
            Else
                CheckBox_ETIQ_PACK_NU_SER_CLIE.Checked = False
            End If
            TextBox_PCTG_RECE_CLIE.Text = dt(0)("Pourcentage pour la recette client").ToString
            If dt(0)("Sérialisation article").ToString = "1" Then
                CheckBox_SRLS_ARTI.Checked = True
            Else
                CheckBox_SRLS_ARTI.Checked = False
            End If
            TextBox_MAIL_SMTP.Text = dt(0)("Serveur SMTP mail").ToString
            TextBox_DEST_MAIL.Text = dt(0)("Destinataires mail").ToString
            TextBox_DEST_COPY_MAIL.Text = dt(0)("Destinataires en copie mail").ToString
            TextBox_DEST_BLIND_COPY_MAIL.Text = dt(0)("Destinataires en copie cachée mail").ToString
            TextBox_SUJE_MAIL.Text = dt(0)("Sujet mail").ToString
            TextBox_CONT_MAIL.Text = dt(0)("Contenu mail").ToString
            TextBox_CHEM_FICH_RAPP_TCBL.Text = dt(0)("Chemin fichier rapport de tracabilite").ToString
            TextBox_VUE_FICH_PDF.Text = dt(0)("Vue fichier PDF").ToString
            TextBox_VUE_NU_SER_ENSE_SOUS_ENSE.Text = dt(0)("Vue numéro ensemble numéros sous-ensemble").ToString
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try

        'TextBox_CD_ARTI_CLIE.Text = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Code article client")
        'TextBox_NM_ARTI_CLIE.Text = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Désignation article client")
        'Dim sOption_OF As String = DIG_FACT_SQL_GET_PARA(sClient, "OF")
        'If sOption_OF Is Nothing Then sOption_OF = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "OF")
        'If sOption_OF = "1" Then
        '    CheckBox_SAI_OF.Checked = True
        'Else
        '    CheckBox_SAI_OF.Checked = False
        'End If
        'Dim sOption_BL As String = DIG_FACT_SQL_GET_PARA(sClient, "BL")
        'If sOption_BL Is Nothing Then sOption_BL = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "BL")
        'If sOption_BL = "1" Then
        '    CheckBox_SAI_BL.Checked = True
        'Else
        '    CheckBox_SAI_BL.Checked = False
        'End If
        'Dim sOption_numerocarton As String = DIG_FACT_SQL_GET_PARA(sClient, "Numéro de carton")
        'If sOption_numerocarton Is Nothing Then sOption_numerocarton = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Numéro de carton")
        'If sOption_numerocarton = "1" Then
        '    CheckBox_SAI_NU_CART.Checked = True
        'Else
        '    CheckBox_SAI_NU_CART.Checked = False
        'End If
        'Dim sOption_numeroeolane As String = DIG_FACT_SQL_GET_PARA(sClient, "Numéro de série Eolane")
        'If sOption_numeroeolane Is Nothing Then sOption_numeroeolane = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Numéro de série Eolane")
        'If sOption_numeroeolane = "1" Then
        '    CheckBox_SAI_NU_SER_ECO.Checked = True
        'Else
        '    CheckBox_SAI_NU_SER_ECO.Checked = False
        'End If
        'Dim sOption_numeroclient As String = DIG_FACT_SQL_GET_PARA(sClient, "Numéro de série client")
        'If sOption_numeroclient Is Nothing Then sOption_numeroclient = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Numéro de série client")
        'If sOption_numeroclient = "1" Then
        '    CheckBox_SAI_NU_SER_CLIE.Checked = True
        'Else
        '    CheckBox_SAI_NU_SER_CLIE.Checked = False
        'End If
        'Dim sOption_cartonspecifique As String = DIG_FACT_SQL_GET_PARA(sClient, "Carton spécifique")
        'If sOption_cartonspecifique Is Nothing Then sOption_cartonspecifique = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Carton spécifique")
        'If sOption_cartonspecifique = "1" Then
        '    CheckBox_SAI_NU_CART_SPEC.Checked = True
        'Else
        '    CheckBox_SAI_NU_CART_SPEC.Checked = False
        'End If
        'If DIG_FACT_SQL_GET_PARA(sClient, "Code fournisseur") = "1" Then
        '    CheckBox_SAI_CD_FNS.Checked = True
        'Else
        '    CheckBox_SAI_CD_FNS.Checked = False
        'End If
        'If DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Suivi numéro de série par ordre") = "1" Then
        '    CheckBox_SUIV_NU_SER_ORDR_CROIS.Checked = True
        'Else
        '    CheckBox_SUIV_NU_SER_ORDR_CROIS.Checked = False
        'End If

        'TextBox_FORM_NU_CMD.Text = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Format du numéro de commande")
        'TextBox_FORM_NU_BL.Text = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Format du numéro de BL")
        'TextBox_FORM_CD_CART.Text = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Format du code carton")
        'TextBox_FORM_CD_FNS.Text = DIG_FACT_SQL_GET_PARA(sClient, "Format du code fournisseur")
        'TextBox_FORM_NU_SER_CLIE.Text = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Format Numéro de série client")
        'TextBox_FORM_NU_SER_ECO.Text = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Format Numéro de série Eolane")
        'TextBox_QT_CART.Text = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Quantité carton")
        'TextBox_QT_FLAN.Text = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Quantité flanc")
        'TextBox_INC_NU_SER.Text = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Incrémentation flanc")

        'If DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Retour saisie OF pour colisage") = "1" Then
        '    CheckBox_SAI_OF_CART_TERM.Checked = True
        'Else
        '    CheckBox_SAI_OF_CART_TERM.Checked = False
        'End If
        'TextBox_FORM_ETIQ_CART.Text = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Format de étiquette Carton")
        'TextBox_ENCO_NU_SER_CLIE.Text = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Encodage Numéro de série client")
        'TextBox_FORM_NU_SER_FNS.Text = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Format Numéro de série Fournisseur")
        'TextBox_INDI_CLIE.Text = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Indice client")
        'TextBox_NM_DDM.Text = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Nom du DDM")
        'TextBox_SS_ENS_ASS_1.Text = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Sous-ensemble associé n°1")
        'TextBox_SS_ENS_ASS_2.Text = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Sous-ensemble associé n°2")
        'TextBox_SS_ENS_ASS_3.Text = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Sous-ensemble associé n°3")
        'TextBox_SS_ENS_ASS_4.Text = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Sous-ensemble associé n°4")
        'TextBox_SS_ENS_ASS_5.Text = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Sous-ensemble associé n°5")
        'TextBox_SS_ENS_ASS_6.Text = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Sous-ensemble associé n°6")
        'TextBox_FORM_NU_LOT.Text = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Format Numéro de lot")
        'TextBox_QT_CART_PALE.Text = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Quantité de carton dans la palette")
        'TextBox_NB_LIGN_FICH_PDF.Text = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Nombre de ligne dans le fichier PDF")
        'TextBox_NM_CHEM_FICH_PDF.Text = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Chemin de sauvegarde du fichier PDF")
        'Dim sARTI_ECO As String = TextBox_CD_ARTI_ECO.Text & StrDup(18 - Len(TextBox_CD_ARTI_ECO.Text), " ")
        'TextBox_CHEM_FICH_IMPR_NU_SER_ECO.Text = COMM_APP_WEB_GET_PARA(sARTI_ECO & "|Numéro de série Eolane", "TextBox_FICH_MODE", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx")
        'TextBox_CHEM_FICH_IMPR_NU_SER_CLIE.Text = COMM_APP_WEB_GET_PARA(sARTI_ECO & "|Numéro de série client", "TextBox_FICH_MODE", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx")
        'TextBox_CHEM_FICH_IMPR_CART.Text = COMM_APP_WEB_GET_PARA(sARTI_ECO & "|Carton", "TextBox_FICH_MODE", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx")
        'TextBox_CHEM_FICH_IMPR_PALE.Text = COMM_APP_WEB_GET_PARA(sARTI_ECO & "|Palette", "TextBox_FICH_MODE", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx")

        'If DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Impression étiquette packaging numéro de série Eolane") = "1" Then
        '    CheckBox_ETIQ_PACK_NU_SER_ECO.Checked = True
        'Else
        '    CheckBox_ETIQ_PACK_NU_SER_ECO.Checked = False
        'End If
        'If DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Impression étiquette packaging numéro de série client") = "1" Then
        '    CheckBox_ETIQ_PACK_NU_SER_CLIE.Checked = True
        'Else
        '    CheckBox_ETIQ_PACK_NU_SER_CLIE.Checked = False
        'End If
        'TextBox_PCTG_RECE_CLIE.Text = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Pourcentage pour la recette client")
        'If DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Sérialisation article") = "1" Then
        '    CheckBox_SRLS_ARTI.Checked = True
        'Else
        '    CheckBox_SRLS_ARTI.Checked = False
        'End If
        'TextBox_MAIL_SMTP.Text = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Serveur SMTP mail")
        'TextBox_DEST_MAIL.Text = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Destinataires mail")
        'TextBox_DEST_COPY_MAIL.Text = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Destinataires en copie mail")
        'TextBox_DEST_BLIND_COPY_MAIL.Text = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Destinataires en copie cachée mail")
        'TextBox_SUJE_MAIL.Text = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Sujet mail")
        'TextBox_CONT_MAIL.Text = DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, "Contenu mail")
    End Sub

    Protected Sub Button_VALI_Click(sender As Object, e As EventArgs) Handles Button_VALI.Click

        APPL_CONF_TB("Code article client", TextBox_CD_ARTI_CLIE, False)
        APPL_CONF_TB("Désignation article client", TextBox_NM_ARTI_CLIE, False)
        APPL_CONF_CB("OF", CheckBox_SAI_OF, True, True)
        APPL_CONF_CB("BL", CheckBox_SAI_BL, True, True)
        APPL_CONF_CB("Numéro de carton", CheckBox_SAI_NU_CART, True, True)
        APPL_CONF_CB("Numéro de série Eolane", CheckBox_SAI_NU_SER_ECO, True, True)
        APPL_CONF_CB("Numéro de série client", CheckBox_SAI_NU_SER_CLIE, True, True)
        APPL_CONF_CB("Carton spécifique", CheckBox_SAI_NU_CART_SPEC, True, True)
        APPL_CONF_CB("Code fournisseur", CheckBox_SAI_CD_FNS, True, False)
        APPL_CONF_CB("Suivi numéro de série par ordre", CheckBox_SUIV_NU_SER_ORDR_CROIS, False, True)
        APPL_CONF_TB("Format du numéro de commande", TextBox_FORM_NU_CMD, False)
        APPL_CONF_TB("Format du numéro de BL", TextBox_FORM_NU_BL, False)
        APPL_CONF_TB("Format du code carton", TextBox_FORM_CD_CART, False)
        APPL_CONF_TB("Format du code fournisseur", TextBox_FORM_CD_FNS, True)
        APPL_CONF_TB("Format Numéro de série client", TextBox_FORM_NU_SER_CLIE, False)
        APPL_CONF_TB("Format Numéro de série Eolane", TextBox_FORM_NU_SER_ECO, False)
        APPL_CONF_TB("Quantité carton", TextBox_QT_CART, False)
        APPL_CONF_TB("Quantité flanc", TextBox_QT_FLAN, False)
        APPL_CONF_TB("Incrémentation flanc", TextBox_INC_NU_SER, False)
        APPL_CONF_CB("Retour saisie OF pour colisage", CheckBox_SAI_OF_CART_TERM, False, True)
        APPL_CONF_TB("Format de étiquette Carton", TextBox_FORM_ETIQ_CART, False)
        APPL_CONF_TB("Encodage Numéro de série client", TextBox_ENCO_NU_SER_CLIE, False)
        APPL_CONF_TB("Format Numéro de série Fournisseur", TextBox_FORM_NU_SER_FNS, False)
        APPL_CONF_TB("Indice client", TextBox_INDI_CLIE, False)
        APPL_CONF_TB("Nom du DDM", TextBox_NM_DDM, False)
        APPL_CONF_TB("Sous-ensemble associé n°1", TextBox_SS_ENS_ASS_1, False)
        APPL_CONF_TB("Sous-ensemble associé n°2", TextBox_SS_ENS_ASS_2, False)
        APPL_CONF_TB("Sous-ensemble associé n°3", TextBox_SS_ENS_ASS_3, False)
        APPL_CONF_TB("Sous-ensemble associé n°4", TextBox_SS_ENS_ASS_4, False)
        APPL_CONF_TB("Sous-ensemble associé n°5", TextBox_SS_ENS_ASS_5, False)
        APPL_CONF_TB("Sous-ensemble associé n°6", TextBox_SS_ENS_ASS_6, False)
        APPL_CONF_TB("Format Numéro de lot", TextBox_FORM_NU_LOT, False)
        APPL_CONF_TB("Quantité de carton dans la palette", TextBox_QT_CART_PALE, False)
        APPL_CONF_TB("Nombre de ligne dans le fichier PDF", TextBox_NB_LIGN_FICH_PDF, False)
        APPL_CONF_TB("Chemin de sauvegarde du fichier PDF", TextBox_NM_CHEM_FICH_PDF, False)
        Dim sARTI_ECO As String = TextBox_CD_ARTI_ECO.Text & StrDup(18 - Len(TextBox_CD_ARTI_ECO.Text), " ")
        If TextBox_CHEM_FICH_IMPR_NU_SER_ECO.Text <> COMM_APP_WEB_GET_PARA(sARTI_ECO & "|Numéro de série Eolane", "TextBox_FICH_MODE", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx") Then COMM_APP_WEB_PARA_AFFI_SAVE(sARTI_ECO & "|Numéro de série Eolane", "TextBox_FICH_MODE", TextBox_CHEM_FICH_IMPR_NU_SER_ECO.Text, "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx")
        If TextBox_CHEM_FICH_IMPR_NU_SER_CLIE.Text <> COMM_APP_WEB_GET_PARA(sARTI_ECO & "|Numéro de série client", "TextBox_FICH_MODE", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx") Then COMM_APP_WEB_PARA_AFFI_SAVE(sARTI_ECO & "|Numéro de série client", "TextBox_FICH_MODE", TextBox_CHEM_FICH_IMPR_NU_SER_CLIE.Text, "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx")
        If TextBox_CHEM_FICH_IMPR_CART.Text <> COMM_APP_WEB_GET_PARA(sARTI_ECO & "|Carton", "TextBox_FICH_MODE", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx") Then COMM_APP_WEB_PARA_AFFI_SAVE(sARTI_ECO & "|Carton", "TextBox_FICH_MODE", TextBox_CHEM_FICH_IMPR_CART.Text, "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx")
        If TextBox_CHEM_FICH_IMPR_PALE.Text <> COMM_APP_WEB_GET_PARA(sARTI_ECO & "|Palette", "TextBox_FICH_MODE", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx") Then COMM_APP_WEB_PARA_AFFI_SAVE(sARTI_ECO & "|Palette", "TextBox_FICH_MODE", TextBox_CHEM_FICH_IMPR_PALE.Text, "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx")
        APPL_CONF_CB("Impression étiquette packaging numéro de série Eolane", CheckBox_ETIQ_PACK_NU_SER_ECO, False, True)
        APPL_CONF_CB("Impression étiquette packaging numéro de série client", CheckBox_ETIQ_PACK_NU_SER_CLIE, False, True)
        APPL_CONF_TB("Pourcentage pour la recette client", TextBox_PCTG_RECE_CLIE, False)
        APPL_CONF_CB("Sérialisation article", CheckBox_SRLS_ARTI, False, True)
        APPL_CONF_TB("Serveur SMTP mail", TextBox_MAIL_SMTP, False)
        APPL_CONF_TB("Destinataires mail", TextBox_DEST_MAIL, False)
        APPL_CONF_TB("Destinataires en copie mail", TextBox_DEST_COPY_MAIL, False)
        APPL_CONF_TB("Destinataires en copie cachée mail", TextBox_DEST_BLIND_COPY_MAIL, False)
        APPL_CONF_TB("Sujet mail", TextBox_SUJE_MAIL, False)
        APPL_CONF_TB("Contenu mail", TextBox_CONT_MAIL, False)
        APPL_CONF_TB("Chemin fichier rapport de tracabilite", TextBox_CHEM_FICH_RAPP_TCBL, False)
        APPL_CONF_TB("Vue fichier PDF", TextBox_VUE_FICH_PDF, False)
        APPL_CONF_TB("Vue numéro ensemble numéros sous-ensemble", TextBox_VUE_NU_SER_ENSE_SOUS_ENSE, False)
    End Sub

    Public Sub APPL_CONF_CB(sNM_CRIT As String, cbCheckBox As CheckBox, bClient As Boolean, bArticle As Boolean)

        If bClient = True Then
            Dim sClient As String = SAP_DATA_GET_CLIE_FROM_CD_ARTI(TextBox_CD_ARTI_ECO.Text)
            Select Case DIG_FACT_SQL_GET_PARA(sClient, sNM_CRIT)
                Case Nothing
                    If cbCheckBox.Checked = True Then DIG_FACT_SQL_SET_PARA(sClient, sNM_CRIT, "1")
                Case "1"
                    If cbCheckBox.Checked = False Then DIG_FACT_SQL_SET_PARA(sClient, sNM_CRIT, "0")
                Case "0"
                    If cbCheckBox.Checked = True Then DIG_FACT_SQL_SET_PARA(sClient, sNM_CRIT, "1")
            End Select
        End If
        If bArticle = True Then
            Select Case DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, sNM_CRIT)
                Case Nothing
                    If cbCheckBox.Checked = True Then DIG_FACT_SQL_SET_PARA(TextBox_CD_ARTI_ECO.Text, sNM_CRIT, "1")
                Case "1"
                    If cbCheckBox.Checked = False Then DIG_FACT_SQL_SET_PARA(TextBox_CD_ARTI_ECO.Text, sNM_CRIT, "0")
                Case "0"
                    If cbCheckBox.Checked = True Then DIG_FACT_SQL_SET_PARA(TextBox_CD_ARTI_ECO.Text, sNM_CRIT, "1")
            End Select
        End If

    End Sub

    Public Sub APPL_CONF_TB(sNM_CRIT As String, tbTextBox As TextBox, bClient As Boolean)
        If bClient = True Then
            Dim sClient As String = SAP_DATA_GET_CLIE_FROM_CD_ARTI(TextBox_CD_ARTI_ECO.Text)
            If DIG_FACT_SQL_GET_PARA(sClient, sNM_CRIT) <> tbTextBox.Text Then DIG_FACT_SQL_SET_PARA(sClient, sNM_CRIT, tbTextBox.Text)
        Else
            If DIG_FACT_SQL_GET_PARA(TextBox_CD_ARTI_ECO.Text, sNM_CRIT) <> tbTextBox.Text Then DIG_FACT_SQL_SET_PARA(TextBox_CD_ARTI_ECO.Text, sNM_CRIT, tbTextBox.Text)
        End If

    End Sub

End Class