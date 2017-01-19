<%@ Page Title="Configuration des postes" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="CFGR_POST.aspx.vb" Inherits="App_Web.CFGR_POST" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <table>
        <tr>
            <td><label>Numéro du poste :</label></td><td><asp:TextBox ID="TextBox_POST" runat="server" AutoPostBack="True" Width="855px"></asp:TextBox></td></tr>
        <tr>
            <td><label>Imprimante de document (papier A4) :</label></td><td>
        <asp:TextBox ID="TextBox_IPMT_DOCU" runat="server" AutoPostBack="True" Width="736px"></asp:TextBox></td></tr>
        <tr>
            <td><label>Imprimante d&#39;étiquette (Type CAB) :</label></td><td>
        <asp:TextBox ID="TextBox_IPMT_ETQT" runat="server" AutoPostBack="True" Width="745px"></asp:TextBox></td></tr>
        <tr>
            <td><label>Localisation :</label></td><td>
        <asp:TextBox ID="TextBox_LCLS" runat="server" AutoPostBack="True" ToolTip="Localisation du poste (Par exemple : Magasin ou Finition)" Width="889px"></asp:TextBox></td></tr>
        <tr>
            <td><label>Moyen associé :</label></td><td>
        <asp:TextBox ID="TextBox_MOYE_ASSO" runat="server" AutoPostBack="True" ToolTip="Moyen associé au poste (Par exemple : Genrad)" Width="865px"></asp:TextBox></td></tr>
    </table>
</asp:Content>