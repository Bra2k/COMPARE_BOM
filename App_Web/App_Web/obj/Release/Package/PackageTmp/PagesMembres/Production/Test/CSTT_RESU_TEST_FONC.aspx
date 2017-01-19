<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="CSTT_RESU_TEST_FONC.aspx.vb" Inherits="App_Web.CSTT_RESU_TEST_FONC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    Entrer un numéro de série :
    <asp:TextBox ID="TextBox_NU_SER" runat="server"></asp:TextBox>
    <br />
    <asp:GridView ID="GridView_RESU_FONC" runat="server" AllowSorting="True">
    </asp:GridView>
    <br />
</asp:Content>
