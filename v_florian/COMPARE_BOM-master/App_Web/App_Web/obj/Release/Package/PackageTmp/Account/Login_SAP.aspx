<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Login_SAP.aspx.vb" Inherits="App_Web.Login_SAP" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<link href="App_Themes/CSS/CSS_MediaQuery.css" rel="stylesheet" type="text/css" />
    <br /><br /><br />
    <div style="background-color:#002F60; width:30%; color:#ffffff; box-shadow : 0 4px 8px 0 rgba(0, 0, 0, 0.2), 0 6px 20px 0 rgba(0, 0, 0, 0.19); border-radius: 10px;">
        <br />
        &nbsp;&nbsp;Entrer le matricule :
        <asp:TextBox ID="TextBox_LOG_SAP" runat="server" AutoPostBack="True"></asp:TextBox>
        <br /><br />
    </div>
</asp:Content>
