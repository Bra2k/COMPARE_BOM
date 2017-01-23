<%@ Page Title="Configurer un nouvel article pour le colisage" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="CONF_ARTI.aspx.vb" Inherits="App_Web.CONF_ARTI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <table class="article_cli">
        <tr>
            <td><label>Entrer le code article :</label></td><td><asp:TextBox ID="TextBox_CD_ARTI_ECO" runat="server" Width="900px" ToolTip="Code article Eolane" AutoPostBack="True"></asp:TextBox></td></tr>
        <tr>
            <td><label>Code article client :</label></td><td><asp:TextBox ID="TextBox_CD_ARTI_CLIE" runat="server" Width="900px" ToolTip="Dénomination du code article client"></asp:TextBox></td></tr>
        <tr>
            <td><label>Désignation client de l&#39;article :</label></td><td><asp:TextBox ID="TextBox_NM_ARTI_CLIE" runat="server" ToolTip="Désignation client de l'article" Width="900px"></asp:TextBox></td></tr>
    </table>
    <table class="tab_CHBX">
        <tr>
            <td><asp:CheckBox ID="CheckBox_SAI_OF" runat="server" Text="&nbsp;Saisie de l'OF" ToolTip="Saisir l'OF au colisage" /></td></tr>
        <tr>
            <td><asp:CheckBox ID="CheckBox_SAI_BL" runat="server" Text="&nbsp;Saisie du BL" ToolTip="Saisir le N° de BL au colisage (implique aussi une association à un numéro de palette)" /></td></tr>
        <tr>
            <td><asp:CheckBox ID="CheckBox_SAI_NU_CART" runat="server" Text="&nbsp;Saisie du numéro de carton" ToolTip="Saisir le numéro du carton Eolane au colisage" /></td></tr>
        <tr>
            <td><asp:CheckBox ID="CheckBox_SAI_NU_SER_ECO" runat="server" Text="&nbsp;Saisie du numéro de série Eolane" ToolTip="Saisir le numéro de série Eolane au colisage" /></td></tr>
        <tr>
            <td><asp:CheckBox ID="CheckBox_SAI_NU_SER_CLIE" runat="server" Text="&nbsp;Saisie du numéro de série Client" ToolTip="Saisir le numéro de série du client au colisage" /></td></tr>
        <tr>
            <td><asp:CheckBox ID="CheckBox_SAI_NU_CART_SPEC" runat="server" Text="&nbsp;Saisie du numéro de carton spécifique" ToolTip="Saisir le numéro du carton spécifique au colisage" /></td></tr>
        <tr>
            <td><asp:CheckBox ID="CheckBox_SAI_CD_FNS" runat="server" Text="&nbsp;Saisie du code fournisseur" ToolTip="Saisir le code fournisseur au colisage" /></td></tr>
        <tr>
            <td><asp:CheckBox ID="CheckBox_SUIV_NU_SER_ORDR_CROIS" runat="server" Text="&nbsp;Suivi des numéros de série dans l'ordre croissant" ToolTip="Suivi des numéros de série dans l'ordre croissant au colisage" /></td></tr>
    </table>
    <table class="tab_format_qte">
        <tr>
            <td><label>Format du numéro de commande :</label></td>
            <td><asp:TextBox ID="TextBox_FORM_NU_CMD" runat="server" Width="842px"></asp:TextBox></td></tr>
        <tr>
            <td><label>Format du numéro de BL :</label></td>
            <td><asp:TextBox ID="TextBox_FORM_NU_BL" runat="server" Width="842px"></asp:TextBox></td></tr>
        <tr>
            <td><label>Format du code carton :</label></td>
            <td><asp:TextBox ID="TextBox_FORM_CD_CART" runat="server" Width="842px"></asp:TextBox></td></tr>
        <tr>
            <td><label>Format du code fournisseur :</label></td>
            <td><asp:TextBox ID="TextBox_FORM_CD_FNS" runat="server" Width="842px"></asp:TextBox></td></tr>
        <tr>
            <td><label>Format du numéro de série Client :</label></td>
            <td><asp:TextBox ID="TextBox_FORM_NU_SER_CLIE" runat="server" Width="350px" TextMode="MultiLine"></asp:TextBox></td></tr>
        <tr>
            <td><label>Format du numéro de série Eolane :</label></td>
            <td><asp:TextBox ID="TextBox_FORM_NU_SER_ECO" runat="server" Width="350px" TextMode="MultiLine"></asp:TextBox></td></tr>
        <tr>
            <td><label>Quantité dans un carton :</label></td>
            <td><asp:TextBox ID="TextBox_QT_CART" runat="server" Width="842px">9999</asp:TextBox></td></tr>
        <tr>
            <td><label>Quantité dans un flanc :</label></td>
            <td><asp:TextBox ID="TextBox_QT_FLAN" runat="server" Width="842px">1</asp:TextBox></td></tr>
        <tr>
            <td><label>Incrémentation des numéros de série :</label></td>
            <td><asp:TextBox ID="TextBox_INC_NU_SER" runat="server" Width="842px">1</asp:TextBox></td></tr>
    </table>
            <asp:CheckBox ID="CheckBox_SAI_OF_CART_TERM" runat="server" Text="&nbsp;Saisie de l'OF pour chaque carton terminé" />
        <br/>
    <table class="tab_format_etique_num_serie">
        <tr>
            <td><label>Format de l&#39;étiquette carton :</label></td> 
            <td><asp:TextBox ID="TextBox_FORM_ETIQ_CART" runat="server" Width="850px"></asp:TextBox></td></tr>
        <tr>
            <td><label>Encodage du numéro de série client :</label></td>
            <td><asp:TextBox ID="TextBox_ENCO_NU_SER_CLIE" runat="server" Width="850px">10</asp:TextBox></td></tr>
    </table>
            <label>Format du numéro de série fournisseur (sous-ensemble spécifique par exemple pipette STW) :</label>
            <asp:TextBox ID="TextBox_FORM_NU_SER_FNS" runat="server" Width="449px"></asp:TextBox>
    <table class="tab_sous_ensemble">
        <tr>
            <td><label>Indice Client :</label></td>
            <td><asp:TextBox ID="TextBox_INDI_CLIE" runat="server" Width="909px"></asp:TextBox></td></tr>
        <tr>
            <td><label>Nom du DDM :</label></td>
            <td><asp:TextBox ID="TextBox_NM_DDM" runat="server" Width="909px"></asp:TextBox></td></tr>
        <tr>
            <td><label>Sous-ensemble n°1 associé :</label></td>
            <td><asp:TextBox ID="TextBox_SS_ENS_ASS_1" runat="server" Width="909px"></asp:TextBox></td></tr>
        <tr>
            <td><label>Sous-ensemble n°2 associé :</label></td>
            <td><asp:TextBox ID="TextBox_SS_ENS_ASS_2" runat="server" Width="909px"></asp:TextBox></td></tr>
        <tr>
            <td><label>Sous-ensemble n°3 associé :</label></td>
            <td><asp:TextBox ID="TextBox_SS_ENS_ASS_3" runat="server" Width="909px"></asp:TextBox></td></tr>
        <tr>
            <td><label>Sous-ensemble n°4 associé :</label></td>
            <td><asp:TextBox ID="TextBox_SS_ENS_ASS_4" runat="server" Width="909px"></asp:TextBox></td></tr>
        <tr>
            <td><label>Sous-ensemble n°5 associé :</label></td>
            <td><asp:TextBox ID="TextBox_SS_ENS_ASS_5" runat="server" Width="909px"></asp:TextBox></td></tr>
        <tr>
            <td><label>Sous-ensemble n°3 associé :</label></td>
            <td><asp:TextBox ID="TextBox_SS_ENS_ASS_6" runat="server" Width="909px"></asp:TextBox></td></tr>
    </table>
    <table class="tab_01">
        <tr>
            <td><label>Format du numéro de lot :</label></td>
            <td><asp:TextBox ID="TextBox_FORM_NU_LOT" runat="server" Width="797px"></asp:TextBox></td></tr>
        <tr>
            <td><label>Quantité maximale de carton dans la palette :</label></td>
            <td><asp:TextBox ID="TextBox_QT_CART_PALE" runat="server" ToolTip="Quantité maximale de carton contenue dans une palette au colisage" Width="797px"></asp:TextBox></td></tr>
        <tr>
            <td><label>Nombre de ligne dans le fichier PDF :</label></td>
            <td><asp:TextBox ID="TextBox_NB_LIGN_FICH_PDF" runat="server" ToolTip="Nombre maximal de ligne dans le fichier PDF (surtout utilisé pour le colisage)" Width="797px"></asp:TextBox></td></tr>
    </table>
    <table class="tab_chemin_reseau">
        <tr>
            <td><label>Chemin réseau de la sauvegarde du fichier PDF :</label></td>
            <td><asp:TextBox ID="TextBox_NM_CHEM_FICH_PDF" runat="server" Width="500px" ToolTip="Chemin réseau de la sauvegarde du fichier PDF surtout utilisé pour le colisage" TextMode="MultiLine"></asp:TextBox></td></tr>
        <tr>
            <td><label>Chemin réseau du fichier d&#39;impression d&#39;étiquette d&#39;un numéro de série Eolane :</label></td>
            <td><asp:TextBox ID="TextBox_CHEM_FICH_IMPR_NU_SER_ECO" runat="server" Width="500px" TextMode="MultiLine"></asp:TextBox></td></tr>
        <tr>
            <td><label>Chemin réseau du fichier d&#39;impression d&#39;étiquette d&#39;un numéro de série Client :</label></td>
            <td><asp:TextBox ID="TextBox_CHEM_FICH_IMPR_NU_SER_CLIE" runat="server" Width="500px" TextMode="MultiLine"></asp:TextBox></td></tr>
        <tr>
            <td><label>Chemin réseau du fichier d&#39;impression d&#39;étiquette d&#39;un carton:</label></td>
            <td><asp:TextBox ID="TextBox_CHEM_FICH_IMPR_CART" runat="server" Width="500px" TextMode="MultiLine"></asp:TextBox></td></tr>
        <tr>
            <td><label>Chemin réseau du fichier d&#39;impression d&#39;étiquette d&#39;une palette :</label></td>
            <td><asp:TextBox ID="TextBox_CHEM_FICH_IMPR_PALE" runat="server" Width="500px" TextMode="MultiLine"></asp:TextBox></td></tr>
    </table>
    <asp:CheckBox ID="CheckBox_ETIQ_PACK_NU_SER_ECO" runat="server" Text="&nbsp;Impression d'une étiquette packaging avec numéro de série Eolane" />
        <br />
    <asp:CheckBox ID="CheckBox_ETIQ_PACK_NU_SER_CLIE" runat="server" Text="&nbsp;Impression d'une étiquette packaging avec numéro de série client" />
        <br />
            <label>Pourcentage de la quantité de l&#39;OF requis pour le présentation en recette client :</label>
            <asp:TextBox ID="TextBox_PCTG_RECE_CLIE" runat="server" Height="22px" Width="545px"></asp:TextBox>
        <br />
            <asp:CheckBox ID="CheckBox_SRLS_ARTI" runat="server" Checked="True" Text="&nbsp;Sérialisation de l'article" />
        <br />
    <table class="tab_mail">
        <tr>
            <td><label>Serveur SMTP pour l&#39;envoi de Mail :</label></td>
            <td><asp:TextBox ID="TextBox_MAIL_SMTP" runat="server" Width="765px"></asp:TextBox></td></tr>
        <tr>
            <td><label>Destinataire de l&#39;envoi des mail :</label></td>
            <td><asp:TextBox ID="TextBox_DEST_MAIL" runat="server" Width="765px"></asp:TextBox></td>
        <tr>
            <td><label>Destinataire en copie de l&#39;envoi des mail :</label></td>
            <td><asp:TextBox ID="TextBox_DEST_COPY_MAIL" runat="server" Width="765px"></asp:TextBox></td></tr>
        <tr>
            <td><label>Destinataire en copie cachée de l&#39;envoi des mail :</label></td>
            <td><asp:TextBox ID="TextBox_DEST_BLIND_COPY_MAIL" runat="server" Width="765px"></asp:TextBox></td></tr>
        <tr>
            <td><label>Sujet des mail :</label></td>
            <td><asp:TextBox ID="TextBox_SUJE_MAIL" runat="server" Width="765px"></asp:TextBox></td>
        <tr>
            <td><label>Contenu du mail :</label></td>
            <td><asp:TextBox ID="TextBox_CONT_MAIL" runat="server" Width="765px"></asp:TextBox></td></tr>
    </table>
    <asp:CheckBox ID="CheckBox_MAIL_PJ" runat="server" Text="&nbsp;Mettre en pièce jointe des fichiers" />
    <br />
    <table class="tab_02">
        <tr>
            <td><label>Chemin du fichier de rapport de traçabilité :</label></td>
            <td style="width: 584px"><asp:TextBox ID="TextBox_CHEM_FICH_RAPP_TCBL" runat="server" Width="500px" TextMode="MultiLine"></asp:TextBox></td></tr>
        <tr>
            <td><label>Nom de la vue pour l&#39;extration du fichier PDF :</label></td>
            <td style="width: 584px"><asp:TextBox ID="TextBox_VUE_FICH_PDF" runat="server" Width="790px"></asp:TextBox></td></tr>
    </table>
        <label>Nom de la vue de recherche du numéro d&#39;ensemble en fonction des numéros de sous-ensemble :</label>
        <asp:TextBox ID="TextBox_VUE_NU_SER_ENSE_SOUS_ENSE" runat="server" Width="427px"></asp:TextBox>
    <br />
    <br />
        <asp:Button ID="Button_VALI" runat="server" Text="Valider" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Height="40px" Width="180px" />
    <br />
</asp:Content>
