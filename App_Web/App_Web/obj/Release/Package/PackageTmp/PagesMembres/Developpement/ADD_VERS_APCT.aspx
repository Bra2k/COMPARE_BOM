<%@ Page Title="Ajouter une nouvelle version" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ADD_VERS_APCT.aspx.vb" Inherits="App_Web.ADD_VERS_APCT" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    Sélectionner le programme : <asp:DropDownList ID="DropDownList_PROG" runat="server" DataSourceID="SqlDataSource_PROG" DataTextField="NM_APP" DataValueField="NM_APP">
    </asp:DropDownList>
    <br />
    <asp:SqlDataSource ID="SqlDataSource_PROG" runat="server" ConnectionString="<%$ ConnectionStrings:SERCELConnectionString %>" SelectCommand="SELECT [NM_APP]
  FROM [APP_WEB_ECO].[dbo].[DTM_GEST_VERS_PROG]
GROUP BY [NM_APP]
ORDER BY [NM_APP]"></asp:SqlDataSource>
    Sélectionner le niveau de l&#39;evolution : <asp:DropDownList ID="DropDownList_NIVE" runat="server">
        <asp:ListItem Selected="True" Value="VAL_MINE">Mineure</asp:ListItem>
        <asp:ListItem Value="VAL_MAJE">Majeure</asp:ListItem>
    </asp:DropDownList>  
    <br />
    Nom de la page (si APP_WEB) :
    <asp:TextBox ID="TextBox_PAGE" runat="server"></asp:TextBox>
    <br />
    Contenu :
    <asp:TextBox ID="TextBox_CONT" runat="server" Height="212px" Width="631px"></asp:TextBox>
    <br />
    <asp:Button ID="Button_VALI" runat="server" Text="Valider" />
    <br />
</asp:Content>
