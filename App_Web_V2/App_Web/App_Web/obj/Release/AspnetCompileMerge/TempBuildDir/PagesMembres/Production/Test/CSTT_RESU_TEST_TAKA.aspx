﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="CSTT_RESU_TEST_TAKA.aspx.vb" Inherits="App_Web.CSTT_RESU_TEST_TAKA" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="App_Themes/CSS/CSS_MediaQuery.css" rel="stylesheet" type="text/css" />
    <br />
    <div class="titre_page">
        <h2>Production : TAKAYA</h2>
    </div>
    <br />
    <label>Consulter résultats des tests TAKAYA (N° de série) :&nbsp;</label><asp:TextBox ID="TextBox_NU_SER" runat="server" BackColor="#989000" ForeColor="White" ></asp:TextBox>
    <br />
    <asp:GridView ID="GridView_RESU_TAKA" runat="server">
    </asp:GridView>
</asp:Content>