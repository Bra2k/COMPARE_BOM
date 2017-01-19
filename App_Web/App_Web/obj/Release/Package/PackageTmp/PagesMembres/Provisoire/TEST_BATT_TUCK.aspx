<%@ Page Title="Test batterie Tucky" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="TEST_BATT_TUCK.aspx.vb" Inherits="App_Web.TEST_BATT_TUCK" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    Entrer le numéro de série du produit :
    <asp:TextBox ID="TextBox_NU_SER_PROD" runat="server" AutoPostBack="True" Width="485px"></asp:TextBox>
    <br />
    Entrer la valeur de la batterie : <asp:TextBox ID="TextBox_VALE_BATT" runat="server"></asp:TextBox>
    <br />
    <asp:Button ID="Button_VALI" runat="server" Text="Valider" />
</asp:Content>
