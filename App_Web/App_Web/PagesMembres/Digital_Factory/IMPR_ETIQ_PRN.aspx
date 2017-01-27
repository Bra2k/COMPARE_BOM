<%@ Page Title="Imprimer une étiquette" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="IMPR_ETIQ_PRN.aspx.vb" Inherits="App_Web.IMPR_ETIQ_PRN" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="App_Themes/CSS/CSS_MediaQuery.css" rel="stylesheet" type="text/css" />
    <br />
    <div class="titre_page">
        <h2>Digital Factory : Etiquettes</h2>
    </div>
    <br />
    <table>
        <tr>
            <td><label>Etiquette :</label></td>
            <td><asp:DropDownList ID="DropDownList_NM_ETI" runat="server" DataSourceID="SqlDataSource_NM_ETI" DataTextField="NM_ETIQ" DataValueField="NM_ETIQ"></asp:DropDownList></td></tr>
        <tr>
            <td><label>OF :</label></td><td><asp:TextBox ID="TextBox_NU_OF" runat="server" AutoPostBack="True" ToolTip="#NU_OF"></asp:TextBox></td>
            <td>&nbsp;<asp:SqlDataSource ID="SqlDataSource_NM_ETI" runat="server" ConnectionString="<%$ ConnectionStrings:SERCELConnectionString %>" SelectCommand="SELECT [NM_ETIQ]
            FROM [APP_WEB_ECO].[dbo].[DTM_REF_LIST_ETIQ_PRN]"></asp:SqlDataSource></td></tr>
        <tr>
            <td><label>Numéro du BL :</label></td>
            <td><asp:TextBox ID="TextBox_NU_BL" runat="server" AutoPostBack="True" ToolTip="#NU_BL"></asp:TextBox></td></tr>
        <tr>
            <td><label>Code Article Eolane :</label></td>
            <td><asp:Label ID="Label_CD_ARTI_ECO" runat="server" ToolTip="#CD_ARTI_ECO"></asp:Label></td>
            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<label>Désignation de l&#39;article :</label></td>
            <td><asp:Label ID="Label_NM_DSGT_ARTI" runat="server" ToolTip="#NM_DSGT_ARTI"></asp:Label></td></tr>
        <tr>
            <td><label>Code article client :</label></td>
            <td><asp:TextBox ID="TextBox_CD_ARTI_CLIE" runat="server" ToolTip="#CD_ARTI_CLIE" Enabled="False"></asp:TextBox></td>
            <td>&nbsp;(modifiable via la page<asp:HyperLink ID="HyperLink_CONF_ARTI" runat="server" NavigateUrl="~/PagesMembres/Digital_Factory/CONF_ARTI.aspx">&nbsp;Configurer un nouvel article pour le colisage</asp:HyperLink>)</td></tr>
        <tr>
            <td><label>Numéro de la commande :</label></td>
            <td><asp:TextBox ID="TextBox_NU_CMDE" runat="server" ToolTip="#NU_CMDE"></asp:TextBox></td>
            <td>&nbsp;<asp:CheckBox ID="CheckBox_SAI_CMDE" runat="server" Text="&nbsp;Saisie manuelle du numéro de la commande" /></td></tr>
        <tr>
            <td><label>Numéro du carton :</label></td><td><asp:TextBox ID="TextBox_NU_CART" runat="server" ToolTip="#NU_CART"></asp:TextBox></td>
            <td>&nbsp;<asp:CheckBox ID="CheckBox_RECH_DERN_NU_CART" runat="server" Text="&nbsp;Rechercher le dernier numéro de carton" /></td></tr>
        <tr>
            <td><label>Code fournisseur :</label></td><td><asp:TextBox ID="TextBox_CD_FNS" runat="server" ToolTip="#CD_FNS" Enabled="False"></asp:TextBox></td>
            <td>&nbsp;(modifiable via la page<asp:HyperLink ID="HyperLink_CONF_ARTI0" runat="server" NavigateUrl="~/PagesMembres/Digital_Factory/CONF_ARTI.aspx">&nbsp;Configurer un nouvel article pour le colisage</asp:HyperLink>)</td></tr>
        <tr>
            <td><label>Numéro de série Client :</label></td><td><asp:TextBox ID="TextBox_NU_SER_CLIE" runat="server" ToolTip="#NU_SER_CLIE"></asp:TextBox></td>
            <td>&nbsp;<asp:CheckBox ID="CheckBox_NU_SER_CLIE_GENE_AUTO" runat="server" Text="&nbsp;Générer automatiquement le numéro de série client" AutoPostBack="True" /></td></tr>
    </table>
    <p>
        <asp:MultiView ID="MultiView_GENE_NU_SER_CLIE" runat="server">
            <asp:View ID="View_VOID_GENE_CLIE" runat="server">
            </asp:View>
            <asp:View ID="View_PARA_GENE_CLIE" runat="server">
                Base :
                <asp:TextBox ID="TextBox_BASE_NU_CLIE" runat="server" Enabled="False"></asp:TextBox>
                &nbsp;(modifiable via la page
                <asp:HyperLink ID="HyperLink_CONF_ARTI1" runat="server" NavigateUrl="~/PagesMembres/Digital_Factory/CONF_ARTI.aspx">Configurer un nouvel article pour le colisage</asp:HyperLink>
                )<br />
                Incrémentation (Flanc) :
                <asp:TextBox ID="TextBox_ICMT_NU_CLI" runat="server" Enabled="False"></asp:TextBox>
                &nbsp;(modifiable via la page
                <asp:HyperLink ID="HyperLink_CONF_ARTI2" runat="server" NavigateUrl="~/PagesMembres/Digital_Factory/CONF_ARTI.aspx">Configurer un nouvel article pour le colisage</asp:HyperLink>
                )<br /> Format :
                <asp:TextBox ID="TextBox_FORM_NU_CLIE" runat="server" AutoPostBack="True"></asp:TextBox>
                &nbsp;(Légende : % = incrémentation au delà de la base 10 des lettres seront insérées; A = 10, B = 11, etc...)<br /> Exemple : %%%%%-%%% sur une base 36 pourra donner A10E3-005<br /> Critère de génération :
                <asp:DropDownList ID="DropDownList_CRIT_GENE_NU_SER" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource_CRIT_GENE_NU_SER" DataTextField="NM_CRIT" DataValueField="NM_CRIT">
                </asp:DropDownList>
                <asp:SqlDataSource ID="SqlDataSource_CRIT_GENE_NU_SER" runat="server" ConnectionString="<%$ ConnectionStrings:SERCELConnectionString %>" SelectCommand="SELECT [NM_CRIT]
  FROM [APP_WEB_ECO].[dbo].[DTM_REF_LIST_CRIT_GENE_NUM]"></asp:SqlDataSource>
                <asp:CheckBox ID="CheckBox_REPR_NU_SER_REBU" runat="server" AutoPostBack="True" Text="Reprendre des numéros de série rebutés ou perdus" />
                <br />
            </asp:View>
        </asp:MultiView>
    </p>
    <p> 
            <b>Numéro de série Eolane :&nbsp;&nbsp;</b><asp:TextBox ID="TextBox_NU_SER_ECO" runat="server" ToolTip="#NU_SER_ECO"></asp:TextBox>
            <asp:CheckBox ID="CheckBox_NU_SER_ECO_GENE_AUTO" runat="server" Text="&nbsp;Générer automatiquement le numéro de série Eolane" AutoPostBack="True" />
    </p>
    <table>
        <tr>
            <td><asp:CheckBox ID="CheckBox_EXTR_NU_SER_ECO" runat="server" AutoPostBack="True" Text="&nbsp;Extraire le numéro de série Eolane " /></td>
            <td style="width: 138px">&nbsp;<asp:Label ID="Label_EXTR_NU_SER_ECO" runat="server" ToolTip="#EXTR_NU_SER_ECO"></asp:Label></td></tr>
        <tr>
            <td><label>Quantité (Carton ou livraison) :</label></td> 
            <td style="width: 138px"><asp:TextBox ID="TextBox_NB_QT" runat="server" ToolTip="#NB_QT"></asp:TextBox></td></tr>
        <tr><td><label>Nombre de Carton :</label></td>
            <td style="width: 138px"><asp:TextBox ID="TextBox_NB_CART" runat="server" ToolTip="#NB_CART"></asp:TextBox></td></tr>
    </table>
            <p>
                <label>Date de production (génération de l&#39;OF) :</label>
                <asp:Label ID="Label_DT_PROD" runat="server" ToolTip="#DT_PROD"></asp:Label>
                &nbsp;Format :
                <asp:TextBox ID="TextBox_FT_DT_PROD" runat="server" AutoPostBack="True"></asp:TextBox>
                &nbsp;Résultat :
                <asp:Label ID="Label_FT_DT_PROD" runat="server" Text=""></asp:Label>
                &nbsp;<asp:HyperLink ID="HyperLink_INFO_FT_DT_PROD" runat="server" NavigateUrl="https://msdn.microsoft.com/fr-fr/library/8kb3ddd4(v=vs.110).aspx"> Aide</asp:HyperLink>
        </p>
        <p>
            <label>Date d&#39;expédition :</label>
            <asp:Label ID="Label_DT_EXP" runat="server" Text="" ToolTip="#DT_EXP"></asp:Label>
            &nbsp;Format :&nbsp;<asp:TextBox ID="TextBox_FT_DT_EXP" runat="server" AutoPostBack="True"></asp:TextBox>
            &nbsp;Résultat :
            <asp:Label ID="Label_FT_DT_EXP" runat="server" Text=""></asp:Label>
            &nbsp;<asp:HyperLink ID="HyperLink_INFO_FT_DT_EXP" runat="server" NavigateUrl="https://msdn.microsoft.com/fr-fr/library/8kb3ddd4(v=vs.110).aspx"> Aide</asp:HyperLink>
        </p>
        <p>
            <label>Indice client :</label>
            <asp:TextBox ID="TextBox_IND_CLIE" runat="server" ToolTip="#IND_CLIE" Enabled="False"></asp:TextBox>
            &nbsp;(modifiable via la page
            <asp:HyperLink ID="HyperLink_CONF_ARTI3" runat="server" NavigateUrl="~/PagesMembres/Digital_Factory/CONF_ARTI.aspx">Configurer un nouvel article pour le colisage</asp:HyperLink>)
        </p>
        <p>
            <label>DDM :</label>
            <asp:Label ID="Label_DDM" runat="server" ToolTip="#DDM"></asp:Label>
            &nbsp;(modifiable via la page
            <asp:HyperLink ID="HyperLink_CONF_ARTI4" runat="server" NavigateUrl="~/PagesMembres/Digital_Factory/CONF_ARTI.aspx">Configurer un nouvel article pour le colisage</asp:HyperLink>)
        </p>
        <table>
            <tr>
                <td><label>Variable 1 :</label></td>
                <td><asp:TextBox ID="TextBox_VAR_1" runat="server" ToolTip="#var1"></asp:TextBox></td>
                <td>&nbsp;<asp:Button ID="Button_VAR_1" runat="server" Text="Sauvegarder la Variable 1" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" /></td>
            </tr>
            <tr>
                <td><label>Variable 2 :</label></td>
                <td><asp:TextBox ID="TextBox_VAR_2" runat="server" ToolTip="#var2"></asp:TextBox></td>
                <td>&nbsp;<asp:Button ID="Button_VAR_2" runat="server" Text="Sauvegarder la Variable 2" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" /></td>
            </tr>
            <tr>
                <td><label>Variable 3 :</label></td>
                <td><asp:TextBox ID="TextBox_VAR_3" runat="server" ToolTip="#var3"></asp:TextBox></td>
                <td>&nbsp;<asp:Button ID="Button_VAR_3" runat="server" Text="Sauvegarder la Variable 3" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" /></td>
            </tr>
            <tr>
                <td><label>Variable 4 :</label></td>
                <td><asp:TextBox ID="TextBox_VAR_4" runat="server" ToolTip="#var4"></asp:TextBox></td>
                <td>&nbsp;<asp:Button ID="Button_VAR_4" runat="server" Text="Sauvegarder la Variable 4" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" /></td>
            </tr>
            <tr>
                <td><label>Variable 5 :</label></td>
                <td><asp:TextBox ID="TextBox_VAR_5" runat="server" ToolTip="#var5"></asp:TextBox></td>
                <td>&nbsp;<asp:Button ID="Button_VAR_5" runat="server" Text="Sauvegarder la Variable 5" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" /></td>
            </tr>
            <tr>
                <td><label>Variable 6 :</label></td>
                <td><asp:TextBox ID="TextBox_VAR_6" runat="server" ToolTip="#var6"></asp:TextBox></td>
                <td>&nbsp;<asp:Button ID="Button_VAR_6" runat="server" Text="Sauvegarder la Variable 6" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" /></td>
            </tr>
            <tr>
                <td><label>Variable 7 :</label></td>
                <td><asp:TextBox ID="TextBox_VAR_7" runat="server" ToolTip="#var7"></asp:TextBox></td>
                <td>&nbsp;<asp:Button ID="Button_VAR_7" runat="server" Text="Sauvegarder la Variable 7" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" /></td>
            </tr>
            <tr>
                <td><label>Variable 8 :</label></td>
                <td><asp:TextBox ID="TextBox_VAR_8" runat="server" ToolTip="#var8"></asp:TextBox></td>
                <td>&nbsp;<asp:Button ID="Button_VAR_8" runat="server" Text="Sauvegarder la Variable 8" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" /></td>
            </tr>
            <tr>
               <td><label>Variable 9 :</label></td>
               <td><asp:TextBox ID="TextBox_VAR_9" runat="server" ToolTip="#var9"></asp:TextBox></td>
               <td>&nbsp;<asp:Button ID="Button_VAR_9" runat="server" Text="Sauvegarder la Variable 9" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" /></td>
            </tr>
            <tr>
                <td><label>Variable 10 :</label></td>
                <td><asp:TextBox ID="TextBox_VAR_10" runat="server" ToolTip="#varA"></asp:TextBox></td>
                <td>&nbsp;<asp:Button ID="Button_VAR_10" runat="server" Text="Sauvegarder la Variable 10" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" /></td>
            </tr>
            <tr>
                <td><label>Variable 11 :</label></td>
                <td><asp:TextBox ID="TextBox_VAR_11" runat="server" ToolTip="#varB"></asp:TextBox></td>
                <td>&nbsp;<asp:Button ID="Button_VAR_11" runat="server" Text="Sauvegarder la Variable 11" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" /></td>
            </tr>
            <tr>
                <td><label>Variable 12 :</label></td>
                <td><asp:TextBox ID="TextBox_VAR_12" runat="server" ToolTip="#varC"></asp:TextBox></td>
                <td>&nbsp;<asp:Button ID="Button_VAR_12" runat="server" Text="Sauvegarder la Variable 12" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" /></td>
            </tr>
            <tr>
                <td><label>Variable 13 :</label></td>
                <td><asp:TextBox ID="TextBox_VAR_13" runat="server" ToolTip="#varD"></asp:TextBox></td>
                <td>&nbsp;<asp:Button ID="Button_VAR_13" runat="server" Text="Sauvegarder la Variable 13" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" /></td>
            </tr>
            <tr>
                <td><label>Variable 14 :</label></td>
                <td><asp:TextBox ID="TextBox_VAR_14" runat="server" ToolTip="#varE"></asp:TextBox></td>
                <td>&nbsp;<asp:Button ID="Button_VAR_14" runat="server" Text="Sauvegarder la Variable 14" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" /></td>
            </tr>
            <tr>
                <td><label>Variable 15 :</label></td>
                <td><asp:TextBox ID="TextBox_VAR_15" runat="server" ToolTip="#varF"></asp:TextBox></td>
                <td>&nbsp;<asp:Button ID="Button_VAR_15" runat="server" Text="Sauvegarder la Variable 15" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" /></td>
            </tr>
        </table>
        <table>
            <tr>
                <td><label>Quantité à imprimer :</label></td>
                <td><asp:TextBox ID="TextBox_QT_GENE" runat="server" Text="1"></asp:TextBox>
            <asp:CheckBox ID="CheckBox_GENE_ETIQ_IDEN" runat="server" Text="&nbsp;Générer des étiquettes identiques" />
            &nbsp;<asp:CheckBox ID="CheckBox_GENE_QT_TOTA_OF" runat="server" AutoPostBack="True" Text="&nbsp;Générer la quantité totale de l'OF" /></td></tr>
        <tr>
            <td><label>Fichier modèle :</label></td>
            <td><asp:TextBox ID="TextBox_FICH_MODE" runat="server" Width="629px" AutoPostBack="True"></asp:TextBox></td>
        </tr>
        <tr>
            <td><label>Imprimante :</label></td>
            <td><asp:TextBox ID="TextBox_IMPR_RESE" runat="server" Width="650px"></asp:TextBox>
            &nbsp;(Modifiable via la page<asp:HyperLink ID="HyperLink_CFGR_POST" runat="server">&nbsp;Configuration des postes</asp:HyperLink>)</td></tr>
        </table>
            <br />
            <asp:Button ID="Button_IMPR" runat="server" Text="Imprimer" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Height="40px" Width="180px" />
            <br />
</asp:Content>
