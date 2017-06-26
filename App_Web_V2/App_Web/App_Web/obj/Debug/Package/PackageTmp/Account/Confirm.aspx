<%@ Page Title="Confirmation du compte" Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Confirm.aspx.vb" Inherits="App_Web.Confirm" Async="true" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2><%: Title %>.</h2>

    <div>
        <asp:PlaceHolder runat="server" ID="successPanel" ViewStateMode="Disabled" Visible="true">
            <p>
                Merci d'avoir confirmé votre compte. Cliquez <asp:HyperLink ID="login" runat="server" NavigateUrl="~/Account/Login">ici</asp:HyperLink>  se connecter             
            </p>
        </asp:PlaceHolder>
        <asp:PlaceHolder runat="server" ID="errorPanel" ViewStateMode="Disabled" Visible="false">
            <p class="text-danger">
                Une erreur s'est produite.
            </p>
        </asp:PlaceHolder>
    </div>
</asp:Content>
