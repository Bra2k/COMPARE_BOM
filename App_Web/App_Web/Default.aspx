<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" Codebehind="Default.aspx.vb" Inherits="App_Web._Default" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
       <div class="headinfo" style="width: 236px">
           <%--<asp:MultiView ID="MultiView_LOG" runat="server" ActiveViewIndex="0">
                <asp:View ID="View_Unlog" runat="server">
                    Vous n&#39;êtes pas connecté. Cliquez sur le lien Connexion pour vous inscrire.
                </asp:View>--%>
                <br />
                <table style="width:115%; height: 198px;">
                    <tr><td style="width: 296px; vertical-align: top;">Vous êtes connecté.</td></tr>
                    <tr><td><label>Bienvenue</label> <%: Session("displayname") %></td></tr>
                    <tr><td><label>Titre :</label> <%: Session("title") %></td></tr>
                    <tr><td><label>Mail :</label> <%: Session("mail") %></td></tr>
                    <tr><td><label>Session :</label> <%: Session("samaccountname") %></td></tr>
                    <tr><td><label>Nom :</label> <%: Session("sn") %></td></tr>
                    <tr><td><label>Prénom :</label> <%: Session("givenname") %></td></tr>
                    <tr><td><label>Matricule SAP :</label> <%: Session("matricule") %></td></tr>
                    <tr><td><label>Service :</label> <%: Session("department") %></td></tr>
                </table>
                <%--<asp:View ID="View_LOG" runat="server">
               <%-- </asp:View>
            </asp:MultiView>--%>
        </div>
        <br />
        <br />
            <table style="margin-top: 0px; margin-bottom: 0px; position: relative; top: -222px; left: 313px; height: 526px; width: 922px;">
                <tr>
                    <td style="width: 200px"><asp:ImageButton ID="InBox" ToolTip="Page colisage" runat="server" ImageUrl="~/App_Themes/ICON_COMPARE/parcel2.ico" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Digital_factory/CLSg" /></td>
                    <td style="width: 200px"><asp:ImageButton ID="AdressBook" ToolTip="Page adresses MAC" ImageUrl="~\App_Themes\ICON_COMPARE/computer3.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Production/ALMS/ADD_MAC_ADRE.aspx" /></td>
                    <td style="width: 200px"><asp:ImageButton ID="FixedPc" ToolTip="Page configuration" ImageUrl="~\App_Themes\ICON_COMPARE/configuration2.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Developpement/CFGR_POST.aspx" /></td>
                    <td style="width: 200px"><asp:ImageButton ID="WindTurbine" ToolTip="Page TEST" ImageUrl="~\App_Themes\ICON_COMPARE\essais.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Developpement/Page_ESSAI.aspx" /></td>
                    <td style="width: 200px"><asp:ImageButton ID="StickerPrinter" ToolTip="Page étiquettes" ImageUrl="~\App_Themes\ICON_COMPARE\etiquette3.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Digital_Factory/IMPR_ETIQ_PRN.aspx" /></td></tr>
                <tr>
                    <td style="width: 200px"><asp:ImageButton ID="ElectroniCard" ToolTip="Page article" ImageUrl="~\App_Themes\ICON_COMPARE\barcode3.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Digital_Factory/CONF_ARTI.aspx" /></td>
                    <td style="width: 200px"><asp:ImageButton ID="Bom" ToolTip="Page comparaison BOM" ImageUrl="~\App_Themes\ICON_COMPARE\comparer_fichier2.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Methodes/Comparaison_BOM.aspx" /></td>
                    <td style="width: 200px"><asp:ImageButton ID="Log" ToolTip="Page LOGS" ImageUrl="~\App_Themes\ICON_COMPARE\log_diaries4.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Developpement/JOUR_APP.aspx" /></td>
                    <td style="width: 200px"><asp:ImageButton ID="Takaya" ToolTip="TAKAYA" ImageUrl="~\App_Themes\ICON_COMPARE\test2.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Production/Test/CSTT_RESU_TEST_TAKA.aspx" /></td>
                    <td style="width: 200px"><asp:ImageButton ID="Indicateur" ToolTip="Page indicateur" ImageUrl="~\App_Themes\ICON_COMPARE\indicateur2.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Digital_Factory/IDCT_PASS.aspx" /></td></tr>
                <tr>
                    <td style="width: 200px"><asp:ImageButton ID="CodeBarre" ToolTip="Page ensemble/sous-ensemble" ImageUrl="~\App_Themes\ICON_COMPARE\ensemble.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Digital_Factory/TCBL_ESB_SS_ESB_V2.aspx" /></td>
                    <td style="width: 200px"><asp:ImageButton ID="Imprimeur" ToolTip="Page rapport STW" ImageUrl="~\App_Themes\ICON_COMPARE\imprimeur_stw.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Provisoire/IMPR_RAPP_TCBL_STW_V2.aspx" /></td>
                    <td style="width: 200px"><asp:ImageButton ID="BancTest" ToolTip="Page banc de test" ImageUrl="~\App_Themes\ICON_COMPARE\banc_test.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Production/Test/AJOU_EMPL_MAJ_FICH_BANC_TEST.aspx" /></td>
                    <td style="width: 200px"><asp:ImageButton ID="Composant" ToolTip="Page composants" ImageUrl="~\App_Themes\ICON_COMPARE\composant2.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Digital_Factory/TCBL_COMP.aspx" /></td>
                    <td style="width: 200px"><asp:ImageButton ID="Batterie" ToolTip="Page batterie" ImageUrl="~\App_Themes\ICON_COMPARE\batterie2.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Provisoire/TEST_BATT_TUCK.aspx" /></td></tr>
            </table>
</asp:Content>
