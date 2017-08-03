<%@ Page Title="Configurer un nouvel article pour le colisage" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="CONF_ARTI.aspx.vb" Inherits="App_Web.CONF_ARTI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <div class="titre_page">
        <h2>Digital Factory : Article</h2>
    </div>
    <br />
    <table class="article_cli">
        <tr>
            <td>
                <label>Entrer le code article :</label></td>
            <td>
                <asp:TextBox ID="TextBox_CD_ARTI_ECO" runat="server" Width="900px" ToolTip="Code article Eolane" AutoPostBack="True" CausesValidation="True"></asp:TextBox></td>
        </tr>

    </table>
    <table class="tab_CHBX">


        <tr>
            <td style="width: 528px">
                <asp:CheckBox ID="CheckBox_SAI_NU_CART_SPEC" runat="server" Text="&nbsp;Saisie du numéro de carton spécifique" ToolTip="Saisir le numéro du carton spécifique au colisage" /></td>
        </tr>
        <tr>
            <td style="width: 528px">
                <asp:CheckBox ID="CheckBox_SAI_CD_FNS" runat="server" Text="&nbsp;Saisie du code fournisseur" ToolTip="Saisir le code fournisseur au colisage" /></td>
        </tr>
    </table>
    <table class="tab_format_qte">
        <tr>
            <td>
                <label>Format du numéro de commande :</label></td>
            <td>
                <asp:TextBox ID="TextBox_FORM_NU_CMD" runat="server" Width="842px"></asp:TextBox></td>
        </tr>
        <tr>
            <td>
                <label>Format du numéro de BL :</label></td>
            <td>
                <asp:TextBox ID="TextBox_FORM_NU_BL" runat="server" Width="842px"></asp:TextBox></td>
        </tr>
        <tr>
            <td>
                <label>Format du code carton :</label></td>
            <td>
                <asp:TextBox ID="TextBox_FORM_CD_CART" runat="server" Width="842px"></asp:TextBox></td>
        </tr>
        <tr>
            <td>
                <label>Quantité dans un flanc :</label></td>
            <td>
                <asp:TextBox ID="TextBox_QT_FLAN" runat="server" Width="842px">1</asp:TextBox></td>
        </tr>

    </table>

    <br />
    <table class="tab_format_etique_num_serie">
        <tr>
            <td>
                <label>Format de l&#39;étiquette carton :</label></td>
            <td>
                <asp:TextBox ID="TextBox_FORM_ETIQ_CART" runat="server" Width="850px"></asp:TextBox></td>
        </tr>
        <tr>
            <td>
        </tr>
    </table>

    <table class="tab_01">
        <tr>
            <td>
                <label>Nombre de ligne dans le fichier PDF :</label></td>
            <td>
                <asp:TextBox ID="TextBox_NB_LIGN_FICH_PDF" runat="server" ToolTip="Nombre maximal de ligne dans le fichier PDF (surtout utilisé pour le colisage)" Width="797px"></asp:TextBox></td>
        </tr>
    </table>
    <table class="tab_chemin_reseau">
        <tr>
            <td>
                <label>Chemin réseau de la sauvegarde du fichier PDF :</label></td>
            <td>
                <asp:TextBox ID="TextBox_NM_CHEM_FICH_PDF" runat="server" Width="350px" ToolTip="Chemin réseau de la sauvegarde du fichier PDF surtout utilisé pour le colisage" TextMode="MultiLine"></asp:TextBox></td>
        </tr>
        <tr>
        </tr>
    </table>

    <br />
    <table class="tab_02">
        <tr>
            <td>
                <label>Chemin du fichier de rapport de traçabilité :</label></td>
            <td style="width: 584px">
                <asp:TextBox ID="TextBox_CHEM_FICH_RAPP_TCBL" runat="server" Width="350px" TextMode="MultiLine"></asp:TextBox></td>
        </tr>
        <tr>
            <td>
                <label>Nom de la vue pour l&#39;extration du fichier PDF :</label></td>
            <td style="width: 584px">
                <asp:TextBox ID="TextBox_VUE_FICH_PDF" runat="server" Width="790px"></asp:TextBox></td>
        </tr>
        <tr>
            <td>
                <label>Nom de la vue de recherche du numéro d&#39;ensemble en fonction des numéros de sous-ensemble :</label></td>
            <td style="width: 584px">
                <asp:TextBox ID="TextBox_VUE_NU_SER_ENSE_SOUS_ENSE" runat="server" Width="790px"></asp:TextBox></td>
        </tr>

    </table>
    <br />

    <div class="accordion" data-role="accordion" data-close-any="true">
        <div class="frame active">
            <div class="heading">Général Digital Factory</div>
            <div class="content">
                <div class="row">
                    <div class="col-md-5">
                        <label>Code article client :</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_CD_ARTI_CLIE" runat="server" Width="900px" ToolTip="Dénomination du code article client"></asp:TextBox><br />
                    </div>
                    <div class="col-md-5">
                        <label>Désignation client de l&#39;article :</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_NM_ARTI_CLIE" runat="server" ToolTip="Désignation client de l'article" Width="900px"></asp:TextBox>
                    </div>
                    <div class="col-md-12">
                        <asp:CheckBox ID="CheckBox_SAI_NU_SER_ECO" runat="server" Text="&nbsp;Saisie du numéro de série Eolane" ToolTip="Saisir le numéro de série Eolane au colisage" />
                    </div>
                    <div class="col-md-12">
                        <asp:CheckBox ID="CheckBox_SAI_NU_SER_CLIE" runat="server" Text="&nbsp;Saisie du numéro de série Client" ToolTip="Saisir le numéro de série du client au colisage" />
                    </div>
                    <div class="col-md-5">
                        <label>Format du code fournisseur :</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_FORM_CD_FNS" runat="server" Width="842px"></asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <label>Format du numéro de série Client :</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_FORM_NU_SER_CLIE" runat="server" Width="350px" TextMode="MultiLine"></asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <label>Format du numéro de série Eolane :</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_FORM_NU_SER_ECO" runat="server" Width="350px" TextMode="MultiLine"></asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <label>Format du numéro d'UDI</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_FORM_UDI" runat="server" Width="790px"></asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <label>GTIN</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_GTIN" runat="server" Width="790px"></asp:TextBox>
                    </div>
                    <div class="col-md-12">
                        <asp:CheckBox ID="CheckBox_FORM_NU_SER_ECO_OF6INC" runat="server" Text="Format numéro de série Eolane OF6INC" />
                    </div>
                    <div class="col-md-5">
                        <label>Indice Client :</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_INDI_CLIE" runat="server" Width="909px"></asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <label>Nom du DDM :</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_NM_DDM" runat="server" Width="909px"></asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <label>Format du numéro de série fournisseur (sous-ensemble spécifique par exemple pipette STW) :</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_FORM_NU_SER_FNS" runat="server" Width="449px"></asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <label>Format du numéro de lot :</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_FORM_NU_LOT" runat="server" Width="797px"></asp:TextBox>
                    </div>
                    <div class="col-md-12">
                        <asp:CheckBox ID="CheckBox_SRLS_ARTI" runat="server" Checked="True" Text="&nbsp;Sérialisation de l'article" />
                    </div>
                    <div class="col-md-5">
                        <label>Pourcentage de la quantité de l&#39;OF requis pour le présentation en recette client :</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_PCTG_RECE_CLIE" runat="server" Height="22px" Width="545px"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="frame">
            <div class="heading">Génération des numéros de série</div>
            <div class="content">
                <div class="row">
                    <div class="col-md-5">
                        <label>Encodage du numéro de série :</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_ENCO_NU_SER_CLIE" runat="server" Width="850px">10</asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <label>Incrémentation des numéros de série : </label>
                    </div>
                    <div class="col-md-7" style="max-width: 100%">
                        <asp:TextBox ID="TextBox_INC_NU_SER" runat="server">1</asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <label>Format des numéros de série : </label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_FORM_NU_SER_GNRT" runat="server" Width="842px"></asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <label>Critère de génération :</label>
                    </div>
                    <div class="col-md-7">
                        <asp:DropDownList ID="DropDownList_CRIT_GENE_NU_SER" runat="server" DataSourceID="SqlDataSource_CRIT_GENE_NU_SER" DataTextField="NM_CRIT" DataValueField="NM_CRIT">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource_CRIT_GENE_NU_SER" runat="server" ConnectionString="<%$ ConnectionStrings:APP_WEB_ECOConnectionString %>"
                            SelectCommand="SELECT [NM_CRIT]
                                         FROM [dbo].[DTM_REF_LIST_CRIT_GENE_NUM]"></asp:SqlDataSource>
                    </div>
                    <div class="col-md-5">
                        <label>Nombre d'étiquettes sur la largeur :</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_NB_ETQT_LARG" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <label>Opération de la gamme à laquelle on génére impression numéro de série</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_GNRT_IPSO_NU_SER" runat="server" Width="790px"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="frame">
            <div class="heading">Colisage</div>
            <div class="content">
                <asp:CheckBox ID="CheckBox_SAI_OF" runat="server" Text="&nbsp;Saisie de l'OF au colisage" ToolTip="Saisir l'OF au colisage" /><br />
                <asp:CheckBox ID="CheckBox_SAI_BL" runat="server" Text="&nbsp;Saisie du BL au colisage" ToolTip="Saisir le N° de BL au colisage (implique aussi une association à un numéro de palette)" />
                <br />
                <asp:CheckBox ID="CheckBox_SAI_NU_CART" runat="server" Text="&nbsp;Saisie du numéro de carton au colisage" ToolTip="Saisir le numéro du carton Eolane au colisage" />
                <br />
                <asp:CheckBox ID="CheckBox_ETIQ_PACK_NU_SER_ECO" runat="server" Text="&nbsp;Impression d'une étiquette packaging avec numéro de série Eolane" />
                <br />
                <asp:CheckBox ID="CheckBox_ETIQ_PACK_NU_SER_CLIE" runat="server" Text="&nbsp;Impression d'une étiquette packaging avec numéro de série client" />
                <br />
                <asp:CheckBox ID="CheckBox_SAI_OF_CART_TERM" runat="server" Text="&nbsp;Saisie de l'OF pour chaque carton terminé au colisage" /><br />
                <asp:CheckBox ID="CheckBox_MISE_DPST_DOCU" runat="server" Text="Mise à disposition des documents à la fin de la saisie BL/Palette (FCGF, DHR, etc...)" />
                <asp:CheckBox ID="CheckBox_SUIV_NU_SER_ORDR_CROIS" runat="server" Text="Suivi des numéros de série dans l'ordre croissant" ToolTip="Suivi des numéros de série dans l'ordre croissant au colisage" />
                <label>Quantité dans un carton :</label><asp:TextBox ID="TextBox_QT_CART" runat="server" Width="842px">9999</asp:TextBox>
                <label>Quantité maximale de carton dans la palette :</label><asp:TextBox ID="TextBox_QT_CART_PALE" runat="server" Width="842px">9999</asp:TextBox>
                <table class="tab_02">
                    <tr>
                        <td colspan="3" style="text-align: center; font-size: large;"><strong>COLISAGE</strong></td>
                    </tr>
                    <tr>
                        <td rowspan="5">Document Digital Factory</td>
                        <td style="height: 22px" colspan="2">
                            <asp:CheckBox ID="CheckBox_DOCU_CART_DF" runat="server" Text="Carton" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:CheckBox ID="CheckBox_DOCU_PALE_DF" runat="server" Text="Palette" />
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 22px" colspan="2">
                            <asp:CheckBox ID="CheckBox_DOCU_BL_DF" runat="server" Text="BL" />
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 22px">Requête liste produits dans le carton\palette</td>
                        <td style="height: 22px">
                            <asp:TextBox ID="TextBox_REQU_PROD_DOCU_DF" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 22px">Contenu du code à barre</td>
                        <td style="height: 22px">
                            <asp:TextBox ID="TextBox_CONT_CAB_DF" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td colspan="2"></td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="frame">
            <div class="heading">Opération</div>
            <div class="content">
                <div class="row">
                    <div class="col-md-12">
                        <asp:CheckBox ID="CheckBox_SCAN_NU_SER_FIN_OPRT" runat="server" Text="Scan numéro de série fin opération" />
                    </div>
                    <div class="col-md-12">
                        <asp:CheckBox ID="CheckBox_VRFC_CHRC_OF_NU_SER_CLIE" runat="server" Text="Vérification cohérence OF par numéro de série" />
                    </div>
                    <div class="col-md-12">
                        <asp:CheckBox ID="CheckBox_VRFC_CHRC_OF_NU_SER_ECO" runat="server" Text="Vérification cohérence OF numero serie eolane" />
                    </div>
                    <div class="col-md-5">
                        <label>WORKFLOW étape 1</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_WF_ETAP_1" runat="server" Width="790px"></asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <label>WORKFLOW étape 2</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_WF_ETAP_2" runat="server" Width="790px"></asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <label>WORKFLOW étape 3</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_WF_ETAP_3" runat="server" Width="790px"></asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <label>WORKFLOW étape 4</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_WF_ETAP_4" runat="server" Width="790px"></asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <label>WORKFLOW étape 5</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_WF_ETAP_5" runat="server" Width="790px"></asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <label>WORKFLOW étape 6</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_WF_ETAP_6" runat="server" Width="790px"></asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <label>WORKFLOW étape 7</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_WF_ETAP_7" runat="server" Width="790px"></asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <label>WORKFLOW étape 8</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_WF_ETAP_8" runat="server" Width="790px"></asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <label>WORKFLOW étape 9</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_WF_ETAP_9" runat="server" Width="790px"></asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <label>WORKFLOW étape 10</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_WF_ETAP_10" runat="server" Width="790px"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="frame">
            <div class="heading">Composant</div>
            <div class="content">
                <div class="row">
                    <div class="col-md-5">
                        <label>Sous-ensemble n°1 associé :</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_SS_ENS_ASS_1" runat="server" Width="909px"></asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <label>Sous-ensemble n°2 associé :</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_SS_ENS_ASS_2" runat="server" Width="909px"></asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <label>Sous-ensemble n°3 associé :</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_SS_ENS_ASS_3" runat="server" Width="909px"></asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <label>Sous-ensemble n°4 associé :</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_SS_ENS_ASS_4" runat="server" Width="909px"></asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <label>Sous-ensemble n°5 associé :</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_SS_ENS_ASS_5" runat="server" Width="909px"></asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <label>Sous-ensemble n°6 associé :</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_SS_ENS_ASS_6" runat="server" Width="909px"></asp:TextBox>
                    </div>

                </div>
            </div>
        </div>
        <div class="frame">
            <div class="heading">Configuration d'envoi de données par mail</div>
            <div class="content">
                <div class="row">
                    <div class="col-md-5">
                        <label>Serveur SMTP pour l&#39;envoi de Mail :</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_MAIL_SMTP" runat="server" Width="765px"></asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <label>Destinataire de l&#39;envoi des mail :</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_DEST_MAIL" runat="server" Width="765px"></asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <label>Destinataire en copie de l&#39;envoi des mail :</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_DEST_COPY_MAIL" runat="server" Width="765px"></asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <label>Destinataire en copie cachée de l&#39;envoi des mail :</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_DEST_BLIND_COPY_MAIL" runat="server" Width="765px"></asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <label>Sujet des mail :</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_SUJE_MAIL" runat="server" Width="765px"></asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <label>Contenu du mail :</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_CONT_MAIL" runat="server" Width="765px"></asp:TextBox>
                    </div>
                    <div class="col-md-12">
                        <asp:CheckBox ID="CheckBox_MAIL_PJ" runat="server" Text="&nbsp;Mettre en pièce jointe des fichiers" />
                    </div>
                </div>
            </div>
        </div>
        <div class="frame">
            <div class="heading">Configuration des chemin des script d'étiquette</div>
            <div class="content">
                <div class="row">
                    <div class="col-md-5">
                        <label>Chemin réseau du fichier d&#39;impression d&#39;étiquette d&#39;un numéro de série Eolane :</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_CHEM_FICH_IMPR_NU_SER_ECO" runat="server" Width="350px" TextMode="MultiLine"></asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <label>Chemin réseau du fichier d&#39;impression d&#39;étiquette d&#39;un numéro de série Client :</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_CHEM_FICH_IMPR_NU_SER_CLIE" runat="server" Width="350px" TextMode="MultiLine"></asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <label>Chemin réseau du fichier d&#39;impression d&#39;étiquette d&#39;un carton:</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_CHEM_FICH_IMPR_CART" runat="server" Width="350px" TextMode="MultiLine"></asp:TextBox>
                    </div>
                    <div class="col-md-5">
                        <label>Chemin réseau du fichier d&#39;impression d&#39;étiquette d&#39;une palette :</label>
                    </div>
                    <div class="col-md-7">
                        <asp:TextBox ID="TextBox_CHEM_FICH_IMPR_PALE" runat="server" Width="350px" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <%--    <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true" style="font-family: verdana">
        <div class="panel panel-default">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne">Général Digital Factory</a>
                </h4>
            </div>
            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne">
            </div>
        </div>
        <div class="panel panel-default">
            <div class="panel-heading" role="tab" id="headingTwo">
                <h4 class="panel-title">
                    <a class="collapsed" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">Génération des numéros de série</a>
                </h4>
            </div>
            <div id="collapseTwo" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo">
                <div class="panel-body">
                </div>
            </div>
        </div>
        <div class="panel panel-default">
            <div class="panel-heading" role="tab" id="headingThree">
                <h4 class="panel-title">
                    <a class="collapsed" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseThree" aria-expanded="false" aria-controls="collapseThree"></a>
                </h4>
            </div>
            <div id="collapseThree" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingThree">
            </div>
        </div>

        <div class="panel panel-default">
            <div class="panel-heading" role="tab" id="headingFour">
                <h4 class="panel-title">
                    <a class="collapsed" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseFour" aria-expanded="false" aria-controls="collapseFour">Opération</a>
                </h4>
            </div>
            <div id="collapseFour" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingFour">
            </div>
        </div>
    </div>--%>

        <br />
        <br />
        <asp:Button ID="Button_VALI" runat="server" Text="Valider" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Height="40px" Width="180px" />
        <br />
</asp:Content>
