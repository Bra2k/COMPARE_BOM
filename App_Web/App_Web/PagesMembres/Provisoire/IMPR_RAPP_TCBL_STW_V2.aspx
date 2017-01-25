﻿<%@ Page Title="Générer un rapport de traçabilité STW" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="IMPR_RAPP_TCBL_STW_V2.aspx.vb" Inherits="App_Web.IMPR_RAPP_TCBL_STW_V2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <div style="font-family: Verdana; color: #002F60">
        <h2>Provisoire : Rapport STW&nbsp;&nbsp;&nbsp;</h2>
        <asp:Label ID="Label_LOAD" runat="server" Text="Chargement en cours ..." Visible="False"></asp:Label>
    </div><br />
    <table class="STW">
        <tr>
            <td><label>Entrer le numéro de série de l&#39;ensemble STW :</label></td>
            <td><asp:TextBox ID="TextBox_NS_ENS" runat="server"></asp:TextBox></td></tr>
        <tr>
            <td><br /><asp:Button ID="Button_GENE_RAPP" runat="server" Text="Générer" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Height="40px" Width="180px"/></td></tr>
    </table>
    <iframe id="pdf" name = "pdf" src ="01.pdf" style="width: 1011px; height: 1728px;" hidden="hidden"></iframe>
    <asp:Label ID="Label_FICH_SORT" runat="server" Text=""></asp:Label>
</asp:Content>
