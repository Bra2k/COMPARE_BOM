<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ALMS_menu.aspx.vb" Inherits="App_Web.WebForm2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="App_Themes/CSS/CSS_MediaQuery.css" rel="stylesheet" type="text/css"/>
    <style>
        a.info {
    position:relative;
    z-index:24;
    color:#000;
    text-decoration: none;
    border-radius: 10px;
}

a.info:hover {
    z-index: 25;
    background-color: #002F60;
    border-radius: 10px;
}
 
a.info span {
    display: none;
}
 
a.info:hover span {
    display: block;
    position: absolute;
    top:2em; left:2em; width:13em;
    background-color: #002F60;
    border-radius: 10px;
    color: #ffffff;
    text-align: justify;
    padding: 5px;
    font-weight: 700;
}
    </style>
        <script type="text/javascript" src="~/Scripts/snow.js"></script>
        <script type="text/javascript">
            var cdate = new Date();
            var cmois = cdate.getMonth() + 1;
            if (cmois == '12' || cmois == '1' || cmois == '2') {
                window.onload = function () {
                    snow.init(80);
                };
            };
        </script>
        <div class="headinfo" style="width: 236px">
           <%--<asp:MultiView ID="MultiView_LOG" runat="server" ActiveViewIndex="0">
                <asp:View ID="View_Unlog" runat="server">
                    Vous n&#39;êtes pas connecté. Cliquez sur le lien Connexion pour vous inscrire.
                </asp:View>--%>
                <br />
                <table  style="background-color: #002F60; box-shadow : 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19); color : #ffffff; border-radius: 10px;">
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
                    <td style="width: 200px"><a class="info" href="#"><asp:ImageButton ID="Sticker3" ImageUrl="~\App_Themes\ICON_COMPARE\computer3.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Production/ALMS/ADD_MAC_ADRE.aspx" /><span>Vers page MAC Adresse</span></a></td>
                    <td style="width: 200px"><a class="info" href="#"><asp:ImageButton ID="Compare_file2" ImageUrl="~\App_Themes\ICON_COMPARE\comparer_fichier2.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Production/ALMS/ALMS_IMPORT.aspx" /><span>Vers page ALMS IMPORT</span></a></td></tr>
            </table>
</asp:Content>
