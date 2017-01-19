<%@ Page Title="Configurer un nouvel article pour le colisage" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="CONF_ARTI.aspx.vb" Inherits="App_Web.CONF_ARTI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <table class="article_cli">
        <tr>
            <td><label>Entrer le code article :</label></td><td><asp:TextBox ID="TextBox_CD_ARTI_ECO" runat="server" Width="707px" ToolTip="Code article Eolane" AutoPostBack="True"></asp:TextBox></td></tr>
        <tr>
            <td><label>Code article client :</label></td><td><asp:TextBox ID="TextBox_CD_ARTI_CLIE" runat="server" Width="725px" ToolTip="Dénomination du code article client"></asp:TextBox></td></tr>
        <tr>
            <td><label>Désignation client de l&#39;article :</label></td><td><asp:TextBox ID="TextBox_NM_ARTI_CLIE" runat="server" ToolTip="Désignation client de l'article" Width="657px"></asp:TextBox></td></tr>
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
            <td><asp:TextBox ID="TextBox_FORM_NU_CMD" runat="server" Width="627px"></asp:TextBox></td></tr>
        <tr>
            <td><label>Format du numéro de BL :</label></td>
            <td><asp:TextBox ID="TextBox_FORM_NU_BL" runat="server" Width="679px"></asp:TextBox></td></tr>
        <tr>
            <td><label>Format du code carton :</label></td>
            <td><asp:TextBox ID="TextBox_FORM_CD_CART" runat="server" Width="693px"></asp:TextBox></td></tr>
        <tr>
            <td><label>Format du code fournisseur :</label></td>
            <td><asp:TextBox ID="TextBox_FORM_CD_FNS" runat="server" Width="662px"></asp:TextBox></td></tr>
        <tr>
            <td><label>Format du numéro de série Client :</label></td>
            <td><asp:TextBox ID="TextBox_FORM_NU_SER_CLIE" runat="server" Width="625px"></asp:TextBox></td></tr>
        <tr>
            <td><label>Format du numéro de série Eolane :</label></td>
            <td><asp:TextBox ID="TextBox_FORM_NU_SER_ECO" runat="server" Width="617px"></asp:TextBox></td></tr>
        <tr>
            <td><label>Quantité dans un carton :</label></td>
            <td><asp:TextBox ID="TextBox_QT_CART" runat="server" Width="684px">9999</asp:TextBox></td></tr>
        <tr>
            <td><label>Quantité dans un flanc :</label></td>
            <td><asp:TextBox ID="TextBox_QT_FLAN" runat="server" Width="694px">1</asp:TextBox></td></tr>
        <tr>
            <td><label>Incrémentation des numéros de série :</label></td>
            <td><asp:TextBox ID="TextBox_INC_NU_SER" runat="server" Width="603px">1</asp:TextBox></td></tr>
    </table>
    <br />
    <asp:CheckBox ID="CheckBox_SAI_OF_CART_TERM" runat="server" Text="Saisie de l'of pour chaque carton terminé" />
    <br />
    Format de l&#39;étiquette carton : <asp:TextBox ID="TextBox_FORM_ETIQ_CART" runat="server" Width="663px"></asp:TextBox>
    <br />
    Encodage du numéro de série client :
    <asp:TextBox ID="TextBox_ENCO_NU_SER_CLIE" runat="server" Width="609px">10</asp:TextBox>
    <br />
    Format du numéro de série fournisseur (sous-ensemble spécifique par exemple pipette STW) :
    <asp:TextBox ID="TextBox_FORM_NU_SER_FNS" runat="server" Width="249px"></asp:TextBox>
    <br />
    Indice Client :
    <asp:TextBox ID="TextBox_INDI_CLIE" runat="server" Width="760px"></asp:TextBox>
    <br />
    Nom du DDM :
    <asp:TextBox ID="TextBox_NM_DDM" runat="server" Width="754px"></asp:TextBox>
    <br />
    Sous-ensemble n°1 associé :
    <asp:TextBox ID="TextBox_SS_ENS_ASS_1" runat="server" Width="661px"></asp:TextBox>
    <br />
    Sous-ensemble n°2 associé :
    <asp:TextBox ID="TextBox_SS_ENS_ASS_2" runat="server" Width="661px"></asp:TextBox>
    <br />
    Sous-ensemble n°3 associé :
    <asp:TextBox ID="TextBox_SS_ENS_ASS_3" runat="server" Width="661px"></asp:TextBox>
    <br />
    Sous-ensemble n°4 associé :
    <asp:TextBox ID="TextBox_SS_ENS_ASS_4" runat="server" Width="661px"></asp:TextBox>
    <br />
    Sous-ensemble n°5 associé :
    <asp:TextBox ID="TextBox_SS_ENS_ASS_5" runat="server" Width="661px"></asp:TextBox>
    <br />
    Sous-ensemble n°6 associé :
    <asp:TextBox ID="TextBox_SS_ENS_ASS_6" runat="server" Width="661px"></asp:TextBox>
    <br />
    Format du numéro de lot :
    <asp:TextBox ID="TextBox_FORM_NU_LOT" runat="server" Width="681px"></asp:TextBox>
    <br />
    Quantité maximale de carton dans la palette :
    <asp:TextBox ID="TextBox_QT_CART_PALE" runat="server" ToolTip="Quantité maximale de carton contenue dans une palette au colisage" Width="553px"></asp:TextBox>
    <br />
    Nombre de ligne dans le fichier PDF :
    <asp:TextBox ID="TextBox_NB_LIGN_FICH_PDF" runat="server" ToolTip="Nombre maximal de ligne dans le fichier PDF (surtout utilisé pour le colisage)" Width="613px"></asp:TextBox>
    <br />
    Chemin réseau de la sauvegarde du fichier PDF :
    <asp:TextBox ID="TextBox_NM_CHEM_FICH_PDF" runat="server" Width="534px" ToolTip="Chemin réseau de la sauvegarde du fichier PDF surtout utilisé pour le colisage"></asp:TextBox>
    <br />
    Chemin réseau du fichier d&#39;impression d&#39;étiquette d&#39;un numéro de série Eolane :
    <asp:TextBox ID="TextBox_CHEM_FICH_IMPR_NU_SER_ECO" runat="server" Width="344px"></asp:TextBox>
    <br />
    Chemin réseau du fichier d&#39;impression d&#39;étiquette d&#39;un numéro de série Client :
    <asp:TextBox ID="TextBox_CHEM_FICH_IMPR_NU_SER_CLIE" runat="server" Width="346px"></asp:TextBox>
    <br />
    Chemin réseau du fichier d&#39;impression d&#39;étiquette d&#39;un carton:
    <asp:TextBox ID="TextBox_CHEM_FICH_IMPR_CART" runat="server" Width="460px"></asp:TextBox>
    <br />
    Chemin réseau du fichier d&#39;impression d&#39;étiquette d&#39;une palette :
    <asp:TextBox ID="TextBox_CHEM_FICH_IMPR_PALE" runat="server" Width="443px"></asp:TextBox>
    <br />
    <asp:CheckBox ID="CheckBox_ETIQ_PACK_NU_SER_ECO" runat="server" Text="Impression d'une étiquette packaging avec numéro de série Eolane" />
    <br />
    <asp:CheckBox ID="CheckBox_ETIQ_PACK_NU_SER_CLIE" runat="server" Text="Impression d'une étiquette packaging avec numéro de série client" />
    <br />
    Pourcentage de la quantité de l&#39;OF requis pour le présentation en recette client :
    <asp:TextBox ID="TextBox_PCTG_RECE_CLIE" runat="server" Height="22px" Width="344px"></asp:TextBox>
    <br />
    <asp:CheckBox ID="CheckBox_SRLS_ARTI" runat="server" Checked="True" Text="Sérialisation de l'article" />
    <br />
    Serveur SMTP pour l&#39;envoi de Mail :
    <asp:TextBox ID="TextBox_MAIL_SMTP" runat="server" Width="623px"></asp:TextBox>
    <br />
    Destinataire de l&#39;envoi des mail :
    <asp:TextBox ID="TextBox_DEST_MAIL" runat="server" Width="645px"></asp:TextBox>
    <br />
    Destinataire en copie de l&#39;envoi des mail :
    <asp:TextBox ID="TextBox_DEST_COPY_MAIL" runat="server" Width="587px"></asp:TextBox>
    <br />
    Destinataire en copie cachée de l&#39;envoi des mail :
    <asp:TextBox ID="TextBox_DEST_BLIND_COPY_MAIL" runat="server" Width="537px"></asp:TextBox>
    <br />
    Sujet des mail :
    <asp:TextBox ID="TextBox_SUJE_MAIL" runat="server" Width="753px"></asp:TextBox>
    <br />
    Contenu du mail :
    <asp:TextBox ID="TextBox_CONT_MAIL" runat="server" Width="736px"></asp:TextBox>
    <br />
    <asp:CheckBox ID="CheckBox_MAIL_PJ" runat="server" Text="Mettre eb pièce jointe des fichiers" />
    <br />
    Chemin du fichier de rapport de traçabilité :
    <asp:TextBox ID="TextBox_CHEM_FICH_RAPP_TCBL" runat="server" Width="574px"></asp:TextBox>
    <br />
    Nom de la vue pour l&#39;extration du fichier PDF :
    <asp:TextBox ID="TextBox_VUE_FICH_PDF" runat="server" Width="562px"></asp:TextBox>
    <br />
    Nom de la vue de recherche du numéro d&#39;ensemble en fonction des numéros de sous-ensemble :
    <asp:TextBox ID="TextBox_VUE_NU_SER_ENSE_SOUS_ENSE" runat="server" Width="234px"></asp:TextBox>
    <br />
    <asp:Button ID="Button_VALI" runat="server" Text="Valider" />
    <br />
</asp:Content>
