<%@ Page Title="Contact" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.vb" Inherits="App_Web.Contact" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="titre_page">
        <h2><%: Title %></h2>
    </div>
    <address>
        éolane Combrée<br />
        ZA de l'Ombrée, Bld Jean Baptiste Colbert, <br />
        49520 Combrée, France<br />
        <abbr title="Phone">Tél: +33 2 4194 6363</abbr>
    </address>
    <address>
        <table>
            <tr>
                <td><strong>Support:</strong></td><td><a href="mailto:vincent.sperperini@eolane.com">vincent.sperperini@eolane.com</a></td></tr>
            <tr>
                <td>&nbsp;</td><td><a href="mailto:florian.acker@eolane.com">florian.acker@eolane.com</a></td></tr>
        </table>
        <%--<strong>Marketing:</strong><a href="mailto:Marketing@example.com">Marketing@example.com</a>--%>
    </address>
</asp:Content>
