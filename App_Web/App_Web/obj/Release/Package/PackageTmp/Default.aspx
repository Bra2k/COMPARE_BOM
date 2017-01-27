<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" Codebehind="Default.aspx.vb" Inherits="App_Web._Default" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link href="App_Themes/CSS/CSS_MediaQuery.css" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="Scripts/snow.js"></script>
        <script type="text/javascript">
            window.onload = function () {
                snow.init(40);
            };
        </script>
        <div class="headinfo" style="width: 236px">
           <%--<asp:MultiView ID="MultiView_LOG" runat="server" ActiveViewIndex="0">
                <asp:View ID="View_Unlog" runat="server">
                    Vous n&#39;êtes pas connecté. Cliquez sur le lien Connexion pour vous inscrire.
                </asp:View>--%>
                <br />
                <table class="tab_infos_con">
                    <tr><td>&nbsp;&nbsp;Vous êtes connecté.</td></tr>
                    <tr><td><label>&nbsp;&nbsp;Bienvenue</label> <%: Session("displayname") %></td></tr>
                    <tr><td><label>&nbsp;&nbsp;Titre :</label> <%: Session("title") %></td></tr>
                    <tr><td><label>&nbsp;&nbsp;Mail :</label> <%: Session("mail") %></td></tr>
                    <tr><td><label>&nbsp;&nbsp;Session :</label> <%: Session("samaccountname") %></td></tr>
                    <tr><td><label>&nbsp;&nbsp;Nom :</label> <%: Session("sn") %></td></tr>
                    <tr><td><label>&nbsp;&nbsp;Prénom :</label> <%: Session("givenname") %></td></tr>
                    <tr><td><label>&nbsp;&nbsp;Matricule SAP :</label> <%: Session("matricule") %></td></tr>
                    <tr><td><label>&nbsp;&nbsp;Service :</label> <%: Session("department") %></td></tr>
                </table>
                <%--<asp:View ID="View_LOG" runat="server">
               <%-- </asp:View>
            </asp:MultiView>--%>
        </div>
        <table class="tab_menu">
                <tr>
                    <td style="width: 200px"><a class="info" href="#"><asp:ImageButton ID="InBox" runat="server" ImageUrl="~/App_Themes/ICON_COMPARE/parcel2.ico" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Digital_factory/CLSg" /><span>Vers page colisage</span></a></td>
                    <td style="width: 200px"><a class="info" href="#"><asp:ImageButton ID="AdressBook" ImageUrl="~\App_Themes\ICON_COMPARE/computer3.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Production/ALMS/ADD_MAC_ADRE.aspx" /><span>Vers page MAC adresses</span></a></td>
                    <td style="width: 200px"><a class="info" href="#"><asp:ImageButton ID="FixedPc" ImageUrl="~\App_Themes\ICON_COMPARE/configuration2.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Developpement/CFGR_POST.aspx" /><span>Vers page configuration</span></a></td>
                    <td style="width: 200px"><a class="info" href="#"><asp:ImageButton ID="WindTurbine" ImageUrl="~\App_Themes\ICON_COMPARE\essais.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Developpement/Page_ESSAI.aspx" /><span>Vers page essai</span></a></td>
                    <td style="width: 200px"><a class="info" href="#"><asp:ImageButton ID="StickerPrinter" ImageUrl="~\App_Themes\ICON_COMPARE\etiquette3.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Digital_Factory/IMPR_ETIQ_PRN.aspx" /><span>Vers page étiquettes</span></a></td></tr>
                <tr>
                    <td style="width: 200px"><a class="info" href="#"><asp:ImageButton ID="ElectroniCard" ImageUrl="~\App_Themes\ICON_COMPARE\barcode3.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Digital_Factory/CONF_ARTI.aspx" /><span>Vers page articles</span></a></td>
                    <td style="width: 200px"><a class="info" href="#"><asp:ImageButton ID="Bom" ImageUrl="~\App_Themes\ICON_COMPARE\comparer_fichier2.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Methodes/Comparaison_BOM.aspx" /><span>Vers page compare BOM</span></a></td>
                    <td style="width: 200px"><a class="info" href="#"><asp:ImageButton ID="Log" ImageUrl="~\App_Themes\ICON_COMPARE\log_diaries4.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Developpement/JOUR_APP.aspx" /><span>Vers page LOGS</span></a></td>
                    <td style="width: 200px"><a class="info" href="#"><asp:ImageButton ID="Takaya" ImageUrl="~\App_Themes\ICON_COMPARE\test2.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Production/Test/CSTT_RESU_TEST_TAKA.aspx" /><span>Vers page TAKAYA</span></a></td>
                    <td style="width: 200px"><a class="info" href="#"><asp:ImageButton ID="Indicateur" ImageUrl="~\App_Themes\ICON_COMPARE\indicateur2.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Digital_Factory/IDCT_PASS.aspx" /><span>Vers page indicateur</span></a></td></tr>
                <tr>
                    <td style="width: 200px"><a class="info" href="#"><asp:ImageButton ID="CodeBarre" ImageUrl="~\App_Themes\ICON_COMPARE\ensemble.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Digital_Factory/TCBL_ESB_SS_ESB_V2.aspx" /><span>Vers page ensembles</span></a></td>
                    <td style="width: 200px"><a class="info" href="#"><asp:ImageButton ID="Imprimeur" ImageUrl="~\App_Themes\ICON_COMPARE\imprimeur_stw.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Provisoire/IMPR_RAPP_TCBL_STW_V2.aspx" /><span>Vers page rapport STW</span></a></td>
                    <td style="width: 200px"><a class="info" href="#"><asp:ImageButton ID="BancTest" ImageUrl="~\App_Themes\ICON_COMPARE\banc_test.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Production/Test/AJOU_EMPL_MAJ_FICH_BANC_TEST.aspx" /><span>Vers page banc de test</span></a></td>
                    <td style="width: 200px"><a class="info" href="#"><asp:ImageButton ID="Composant" ImageUrl="~\App_Themes\ICON_COMPARE\composant2.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Digital_Factory/TCBL_COMP.aspx" /><span>Vers page composants</span></a></td>
                    <td style="width: 200px"><a class="info" href="#"><asp:ImageButton ID="Batterie" ImageUrl="~\App_Themes\ICON_COMPARE\batterie2.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Provisoire/TEST_BATT_TUCK.aspx" /><span>Vers page batteries</span></a></td></tr>
            </table>
</asp:Content>
