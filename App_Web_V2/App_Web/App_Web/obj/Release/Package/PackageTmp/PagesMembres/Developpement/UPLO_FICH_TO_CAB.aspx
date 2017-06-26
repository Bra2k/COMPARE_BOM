<%@ Page Title="Charger des fichier dans une imprimante de type CAB" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="UPLO_FICH_TO_CAB.aspx.vb" Inherits="App_Web.UPLO_FICH_TO_CAB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:RadioButtonList ID="RadioButtonList_TYPE_IPMT" runat="server" RepeatDirection="Horizontal">
        <asp:ListItem>EOS1</asp:ListItem>
        <asp:ListItem Value="MACH4">MACH4 / A4+</asp:ListItem>
    </asp:RadioButtonList>
    Entrer l&#39;adresse IP de la machine :
    <asp:TextBox ID="TextBox_IP" runat="server"></asp:TextBox>
    <br />
    Sélectionner le fichier :
    <asp:FileUpload ID="FileUpload_FICH" runat="server" AllowMultiple="True" />
    <br />
    <asp:Button ID="Button_VALI_SAIS" runat="server" BackColor="#002F60" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Font-Bold="True" ForeColor="White" Height="40px" Text="Valider" Width="180px" />

</asp:Content>
