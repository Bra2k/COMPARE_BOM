<%@ Page Title="Test batterie Tucky" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="TEST_BATT_TUCK.aspx.vb" Inherits="App_Web.TEST_BATT_TUCK" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <table>
        <tr>
            <td><label>Entrer le numéro de série du produit :</label></td>
            <td><asp:TextBox ID="TextBox_NU_SER_PROD" runat="server" AutoPostBack="True" Width="485px"></asp:TextBox></td></tr>
        <tr>
            <td><label>Entrer la valeur de la batterie :</label></td>
            <td><asp:TextBox ID="TextBox_VALE_BATT" runat="server"></asp:TextBox></td></tr>
        <tr>
            <td><br /><asp:Button ID="Button_VALI" runat="server" Text="Valider" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Height="40px" Width="180px" /></td></tr>
    </table>
</asp:Content>
