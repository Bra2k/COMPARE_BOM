<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="CHX_REPE_SIMI.aspx.vb" Inherits="App_Web.CHX_REPE_SIMI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    Repère :    <asp:TextBox ID="TextBox_Repère" runat="server"></asp:TextBox>
    <br />
    OF:
    <asp:TextBox ID="TextBox_OF" runat="server"></asp:TextBox>
    <br />
    <br />
    <asp:Button ID="Button_Valider" runat="server" Text="Valider" />
    <br />
    <asp:Label ID="Label_résultat" runat="server" Text=""></asp:Label>
        <br />
        <asp:GridView ID="GridView1" runat="server">
        </asp:GridView>
</asp:Content>
