<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="AJOU_EMPL_MAJ_FICH_BANC_TEST.aspx.vb" Inherits="App_Web.AJOU_EMPL_MAJ_FICH_BANC_TEST" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    Entrer le n° du banc :
    <asp:TextBox ID="TextBox_NU_BANC" runat="server"></asp:TextBox>
    <br />
    Entrer l&#39;emplacement des fichiers de test (le dossier doit être paratgé) :
    <asp:TextBox ID="TextBox_EMPL_FICH_TEST" runat="server" Width="689px"></asp:TextBox>
    <br />
    <br />
    <asp:Button ID="Button_EMPL_FICH_TEST" runat="server" Text="Valider" /><asp:GridView ID="GridView_EMPL_FICH_TEST" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource_EMPL_FICH_TEST">
        <Columns>
            <asp:BoundField DataField="Banc" HeaderText="Banc" SortExpression="Banc" />
            <asp:BoundField DataField="Emplacement des fichiers de test" HeaderText="Emplacement des fichiers de test" SortExpression="Emplacement des fichiers de test" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource_EMPL_FICH_TEST" runat="server" ConnectionString="<%$ ConnectionStrings:SERCELConnectionString %>" SelectCommand="SELECT [ID_BANC] AS Banc
      ,[NM_CHEM_EMPL] AS [Emplacement des fichiers de test]
  FROM [MES_Digital_Factory].[dbo].[V_DTM_TEST_FONC_REF_EMPL_RES]
ORDER BY [ID_BANC]"></asp:SqlDataSource>
</asp:Content>
