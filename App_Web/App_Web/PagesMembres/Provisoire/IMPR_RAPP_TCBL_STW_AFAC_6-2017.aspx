﻿<%@ Page Title="Générer un rapport de traçabilité STW" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="IMPR_RAPP_TCBL_STW_AFAC_6-2017.aspx.vb" Inherits="App_Web.IMPR_RAPP_TCBL_STW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        Entrer le numéro de série de l&#39;ensemble STW :
        <asp:TextBox ID="TextBox_NS_ENS" runat="server"></asp:TextBox>
    </p>
    <p>
        Imprimante :
        <asp:TextBox ID="TextBox_IMPR" runat="server"></asp:TextBox>
    </p>

    <p>
_        <asp:Button ID="Button_GENE_RAPP" runat="server" Text="Générer" />
    </p>
    <iframe id="pdf" name = "pdf" src ="01.pdf" style="width: 1011px; height: 1728px;" hidden="hidden"></iframe>
</asp:Content>
