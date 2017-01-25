<%@ Page Title="Test batterie Tucky" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="TEST_BATT_TUCK.aspx.vb" Inherits="App_Web.TEST_BATT_TUCK" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <div style="font-family: Verdana; color: #002F60">
        <h2>Provisoire : Batterie&nbsp;&nbsp;&nbsp;</h2>
        <asp:Label ID="Label_LOAD" runat="server" Text="Chargement en cours ..." Visible="False"></asp:Label>
    </div><br />
    <table>
        <tr>
            <td><label>Entrer le numéro de série du produit :</label></td>
            <td><asp:TextBox ID="TextBox_NU_SER_PROD" runat="server" AutoPostBack="True" Width="485px"></asp:TextBox></td></tr>
        <tr>
            <td><label>Entrer la valeur de la batterie :</label></td>
            <td><asp:TextBox ID="TextBox_VALE_BATT" runat="server" Width="485px"></asp:TextBox></td></tr>
        <tr>
            <td><br /><asp:Button ID="Button_VALI" runat="server" Text="Valider" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Height="40px" Width="180px" /></td></tr>
    </table>
</asp:Content>
