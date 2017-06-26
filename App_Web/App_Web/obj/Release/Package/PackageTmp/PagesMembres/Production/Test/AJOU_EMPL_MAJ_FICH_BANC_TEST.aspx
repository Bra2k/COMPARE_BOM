<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="AJOU_EMPL_MAJ_FICH_BANC_TEST.aspx.vb" Inherits="App_Web.AJOU_EMPL_MAJ_FICH_BANC_TEST" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="App_Themes/CSS/CSS_MediaQuery.css" rel="stylesheet" type="text/css" />
    <br />
    <div class="titre_page">
        <h2>Production : Banc de test</h2>
    </div>
    <br />
    <table>
        <tr>
            <td><label>Entrer le n° du banc :</label></td>
            <td><asp:TextBox ID="TextBox_NU_BANC" runat="server" Width="690px"></asp:TextBox></td></tr>
        <tr>
            <td style="height: 102px"><label>Entrer l&#39;emplacement des fichiers de test :</label></td>
            <td style="height: 102px"><asp:TextBox ID="TextBox_EMPL_FICH_TEST" runat="server" Width="689px"></asp:TextBox></td>
            <td style="color: #FF0000; height: 102px;">&nbsp; * le dossier doit être partagé</td></tr>
        <tr>
            <td style="height: 102px"><label> Entrer le code article Eolane :</label></td>
            <td style="height: 102px">
                <asp:TextBox ID="TextBox_CD_ARTI_ECO" runat="server" Width="688px" AutoPostBack="True"></asp:TextBox>
            </td>
            <td style="color: #FF0000; height: 102px;">&nbsp;</td></tr>
        <tr>
            <td style="height: 102px">&nbsp;</td>
            <td style="height: 102px">
                &nbsp;</td>
            <td style="color: #FF0000; height: 102px;">&nbsp;</td></tr>
        <tr>
            <td><Label>Sélectionner l'opération de la gamme auquel correpond le test :</Label>&nbsp;</td>
            <td>
                <asp:DropDownList ID="DropDownList_OP" runat="server" Height="17px" Width="334px">
                </asp:DropDownList>
            </td>
            <td style="color: #FF0000">&nbsp;</td></tr>
        <tr>
            <td><br /><asp:Button ID="Button_EMPL_FICH_TEST" runat="server" Text="Valider" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Height="40px" Width="180px" /></td></tr>
    </table>
    <br />
    <asp:GridView ID="GridView_EMPL_FICH_TEST" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource_EMPL_FICH_TEST" BorderColor="#002F60" BorderStyle="Solid" BorderWidth="3px">
        <Columns>
            <asp:BoundField DataField="Banc" HeaderText="Banc" SortExpression="Banc" />
            <asp:BoundField DataField="Emplacement des fichiers de test" HeaderText="Emplacement des fichiers de test" SortExpression="Emplacement des fichiers de test" />
        </Columns>
        <SelectedRowStyle BackColor="#FFFFCC" />
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource_EMPL_FICH_TEST" runat="server" ConnectionString="<%$ ConnectionStrings:SERCELConnectionString %>" 
        SelectCommand="SELECT [ID_BANC] AS Banc, [NM_CHEM_EMPL] AS [Emplacement des fichiers de test]
                        FROM [MES_Digital_Factory].[dbo].[V_DTM_TEST_FONC_REF_EMPL_RES]
                        ORDER BY [ID_BANC]"></asp:SqlDataSource>
</asp:Content>
