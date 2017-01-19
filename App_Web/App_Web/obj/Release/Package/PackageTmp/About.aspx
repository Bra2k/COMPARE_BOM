<%@ Page Title="About" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.vb" Inherits="App_Web.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
        Ce service web est destiné à héberger une multitude d'application pour le compte d'Eolane Combrée.<br />
    <asp:GridView ID="GridView_VERS" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource_VERS">
        <Columns>
            <asp:BoundField DataField="Page / Class" HeaderText="Page / Class" SortExpression="Page / Class" />
            <asp:BoundField DataField="Version" HeaderText="Version" SortExpression="Version" />
            <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" />
            <asp:BoundField DataField="Contenu" HeaderText="Contenu" SortExpression="Contenu" />
        </Columns>
    </asp:GridView>
    <asp:TextBox ID="TextBox_App" runat="server">APP_WEB</asp:TextBox>
    <asp:TextBox ID="TextBox_VERS" runat="server">9999</asp:TextBox>
    <asp:Button ID="Button_VERS" runat="server" Text="Valider" />
    <asp:SqlDataSource ID="SqlDataSource_VERS" runat="server" ConnectionString="<%$ ConnectionStrings:SERCELConnectionString %>" SelectCommand="SELECT NM_PAGE AS [Page / Class], NU_VERS AS Version, DT_VERS AS Date, TX_DESC_VERS AS Contenu FROM APP_WEB_ECO.dbo.DTM_GEST_VERS_PROG WHERE (NM_APP = @APP) AND (NU_VERS &lt;= @VERS) ORDER BY Date DESC">
        <SelectParameters>
            <asp:ControlParameter ControlID="TextBox_App" Name="APP" PropertyName="Text" />
            <asp:ControlParameter ControlID="TextBox_VERS" Name="VERS" PropertyName="Text" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
