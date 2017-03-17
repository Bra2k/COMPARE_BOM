<%@ Page Title="Connexion" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Login.aspx.vb" Inherits="App_Web.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
    <p>

			<asp:Label ID="Label2" Runat="server">Username:</asp:Label>
			<asp:TextBox ID="txtUsername" Runat="server"></asp:TextBox><br>
			<asp:Label ID="Label3" Runat="server">Password:</asp:Label>
			<asp:TextBox ID="txtPassword" Runat="server" TextMode="Password"></asp:TextBox><br>
			<asp:Button ID="btnLogin" Runat="server" Text="Login"></asp:Button><br>
			<asp:Label ID="errorLabel" Runat="server" ForeColor="#ff3300"></asp:Label><br>
			<asp:CheckBox ID="chkPersist" Runat="server" Text="Persist Cookie" />

    </asp:Content>

