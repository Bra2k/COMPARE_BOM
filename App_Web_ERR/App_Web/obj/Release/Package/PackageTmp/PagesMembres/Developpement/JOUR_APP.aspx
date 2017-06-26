<%@ Page Title="Journaux de l'application" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="JOUR_APP.aspx.vb" Inherits="App_Web.JOUR_APP" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="App_Themes/CSS/CSS_MediaQuery.css" rel="stylesheet" type="text/css" />
    <br />
    <div class="titre_page">
        <h2>Développement : Table des logs</h2>
    </div>
    <br />
    <asp:GridView ID="GridView_JOUR" style="border:3px solid black" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource_JOUR" AllowPaging="True" PageSize="25">
        <Columns>
            <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" />
            <asp:BoundField DataField="Criticité" HeaderText="Criticité" SortExpression="Criticité" />
            <asp:BoundField DataField="Nom du poste" HeaderText="Nom du poste" SortExpression="Nom du poste" />
            <asp:BoundField DataField="Fonction" HeaderText="Fonction" SortExpression="Fonction" />
            <asp:BoundField DataField="Message" HeaderText="Message" SortExpression="Message" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource_JOUR" runat="server" ConnectionString="<%$ ConnectionStrings:SERCELConnectionString %>" SelectCommand="SELECT [DT_LOG] AS [Date]
      ,[NM_CTCT] AS [Criticité]
	  ,[NM_POST] AS [Nom du poste]
      ,[NM_FONC] AS [Fonction]
      ,[NM_MSG] AS [Message]
      FROM [APP_WEB_ECO].[dbo].[DTM_LOG_APP_WEB]
      ORDER BY [DT_LOG] DESC"></asp:SqlDataSource>
</asp:Content>