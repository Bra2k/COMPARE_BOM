<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="IPSO_ETQT_NU_SER.aspx.vb" Inherits="App_Web.IPSO_ETQT_NU_SER" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br /><h1> Impression numéros de série ALMS</h1>
    <br />
    <br />
    <label>Saisir l'OF : </label><asp:TextBox ID="TextBox_OF" runat="server" AutoPostBack="True"></asp:TextBox>
    <asp:Label ID="Label_CD_ARTI" runat="server" Text=""></asp:Label>
    <asp:Label ID="Label_QT_OF" runat="server" Text=""></asp:Label>
    <asp:Button ID="Button_IPSO" runat="server" Text="Imprimer" CssClass="btn-primary glyphicon-print"/>
</asp:Content>
