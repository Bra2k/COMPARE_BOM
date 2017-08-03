<%@ Page Title="Administration du site Web" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="AMNS_APP_WEB.aspx.vb" Inherits="App_Web.AMNS_APP_WEB" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<label>Saisir le login/identifiant ou le service : </label>
    <asp:TextBox ID="TextBox_LOG" runat="server" AutoPostBack="True"></asp:TextBox>
    <br />
    <br />
    <label>Liste des pages :</label>
    <br />
    <asp:CheckBoxList ID="CheckBoxList_PAGE" runat="server">
    </asp:CheckBoxList>
    <br />
    <asp:Button ID="Button_VALI" runat="server" Text="Valider" CssClass="btn-primary" />
</asp:Content>
