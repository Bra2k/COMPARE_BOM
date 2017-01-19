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
                    <tr><td>Bienvenue <%: Session("displayname") %></td></tr>
                    <tr><td>Titre : <%: Session("title") %></td></tr>
                    <tr><td>Mail : <%: Session("mail") %></td></tr>
                    <tr><td>Session : <%: Session("samaccountname") %></td></tr>
                    <tr><td>Nom : <%: Session("sn") %></td></tr>
                    <tr><td>Prénom : <%: Session("givenname") %></td></tr>
                    <tr><td>Matricule SAP : <%: Session("matricule") %></td></tr>
                    <tr><td>Service : <%: Session("department") %></td></tr>
                </table>
                <%--<asp:View ID="View_LOG" runat="server">
               <%-- </asp:View>
            </asp:MultiView>--%>
        </div>
        <br />
        <br />
            <table style="margin-top: 0px; margin-bottom: 0px; position: relative; top: -222px; left: 313px; height: 298px; width: 676px;">
                <tr>
                    <td style="width: 200px">coucou<asp:ImageButton ID="InBox" runat="server" Height="100px" ImageUrl="~/App_Themes/PIC_CHAR/14740.png" Width="100px" BorderColor="#3333CC" BorderStyle="Solid" BorderWidth="3px" PostBackUrl="~/PagesMembres/Digital_factory/CLSg" /></td>
                    <td style="width: 200px"><asp:ImageButton ID="AdressBook" ImageUrl="~\App_Themes\PIC_CHAR\21257.png" runat="server" Height="100px" Width="100px" BorderColor="#3333CC" BorderStyle="Solid" BorderWidth="3px" PostBackUrl="~/PagesMembres/Production/ALMS/ADD_MAC_ADRE.aspx" /></td>
                    <td style="width: 200px"><asp:ImageButton ID="FixedPc" ImageUrl="~\App_Themes\PIC_CHAR\2699117232_small_1.png" runat="server" Height="100px" Width="100px" BorderColor="#3333CC" BorderStyle="Solid" BorderWidth="3px" PostBackUrl="~/PagesMembres/Developpement/CFGR_POST.aspx" /></td>
                    <td style="width: 200px"><asp:ImageButton ID="WindTurbine" ImageUrl="~\App_Themes\PIC_CHAR\eolienneanimee.gif" runat="server" Height="100px" Width="100px" BorderColor="#3333CC" BorderStyle="Solid" BorderWidth="3px" PostBackUrl="~/PagesMembres/Developpement/Page_ESSAI.aspx" /></td>
                    <td style="width: 200px"><asp:ImageButton ID="StickerPrinter" ImageUrl="~\App_Themes\PIC_CHAR\eos1_800.jpg" runat="server" Height="100px" Width="100px" BorderColor="#3333CC" BorderStyle="Solid" BorderWidth="3px" PostBackUrl="~/PagesMembres/Digital_Factory/IMPR_ETIQ_PRN.aspx" /></td></tr>
                    <tr><td style="width: 200px"><asp:ImageButton ID="ElectroniCard" ImageUrl="~\App_Themes\PIC_CHAR\icones_01356.png" runat="server" Height="100px" Width="100px" BorderColor="#3333CC" BorderStyle="Solid" BorderWidth="3px" PostBackUrl="~/PagesMembres/Digital_Factory/CONF_ARTI.aspx" /></td>
                    <td style="width: 200px"><asp:ImageButton ID="Bom" ImageUrl="~\App_Themes\PIC_CHAR\image_bommanage.png" runat="server" Height="100px" Width="100px" BorderColor="#3333CC" BorderStyle="Solid" BorderWidth="3px" PostBackUrl="~/PagesMembres/Methodes/Comparaison_BOM.aspx" /></td>
                    <td style="width: 200px"><asp:ImageButton ID="Log" ImageUrl="~\App_Themes\PIC_CHAR\log-file-29824.jpg" runat="server" Height="100px" Width="100px" BorderColor="#3333CC" BorderStyle="Solid" BorderWidth="3px" PostBackUrl="~/PagesMembres/Developpement/JOUR_APP.aspx" /></td>
                    <td style="width: 200px"><asp:ImageButton ID="Takaya" ImageUrl="~\App_Themes\PIC_CHAR\TAKAYA.jpg" runat="server" Height="100px" Width="100px" AlternateText="Colisage" BorderColor="#3333CC" BorderStyle="Solid" BorderWidth="3px" PostBackUrl="~/PagesMembres/Production/Test/CSTT_RESU_TEST_TAKA.aspx" /></td></tr>
            </table>
</asp:Content>
