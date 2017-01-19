<%@ Page Title="Contact" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.vb" Inherits="App_Web.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>

    <address>
        éolane Combrée<br />
        ZA de l'Ombrée, Bld Jean Baptiste Colbert, <br />
        49520 Combrée, France<br />
        <abbr title="Phone">Tél: +33 2 4194 6363</abbr>
    </address>

    <address>
        <strong>Support : </strong><a href="mailto:vincent.sperperini@eolane.com">vincent.sperperini@eolane.com</a><br />
        <%--<strong>Marketing:</strong><a href="mailto:Marketing@example.com">Marketing@example.com</a>--%>
    </address>
</asp:Content>
