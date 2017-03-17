<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Page_ESSAI.aspx.vb" Inherits="App_Web.Page_ESSAI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<br />
    <div style="font-family: Verdana; color: #002F60">
        <h2>Développement : Essais&nbsp;&nbsp;&nbsp;</h2>
        <asp:Label ID="Label_LOAD" runat="server" Text="Chargement en cours ..." Visible="False"></asp:Label>
    </div><br />
    <asp:Button ID="Button1" runat="server" Text="Button" />
    <asp:Button ID="Button2" runat="server" Text="Button2" />
    <asp:Button ID="Button4" runat="server" Text="Button" />
    <asp:GridView ID="GridView1" runat="server"></asp:GridView>
<br />
    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/PagesMembres/Digital_Factory/FICH_PDF.aspx" Target="_blank">HyperLink</asp:HyperLink>
<br />
<br />
    <asp:Image ID="Image1" runat="server" /> <img id="coucou" src= 'data:image/jpeg;base64, " <%: Session("thumbnailphoto")  %>"' alt='photo' /> 
    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    <asp:Button ID="Button3" runat="server" Text="afficher PDF" />
    <iframe id="pdf" name = "pdf" src ="02.pdf" style="width: 1011px; height: 1728px;"></iframe>
<br />
</asp:Content>
