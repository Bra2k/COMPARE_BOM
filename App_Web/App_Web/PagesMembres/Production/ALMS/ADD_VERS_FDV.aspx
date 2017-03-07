<%@ Page Title="Renseigner la version et le nom de la FDV" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ADD_VERS_FDV.aspx.vb" Inherits="App_Web.ADD_VERS_FDV" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="titre_page">
        <h2>Gestion des FDV</h2>
    </div>
    Saisir l'article ALMS du produit : <asp:TextBox ID="TextBox_CD_ARTI_ECO" runat="server" AutoPostBack="True"></asp:TextBox>

    <br />

    <br />
    <table style="width:100%;">
        <tr>
            <td>Saisir le nom de la FDV :
                <asp:TextBox ID="TextBox_NM_RFRC_FDV" runat="server" Enabled="False"></asp:TextBox>
                <br />
                <br />
                Saisir la version de la FDV :
                <asp:TextBox ID="TextBox_NU_VERS_FDV" runat="server" Enabled="False"></asp:TextBox>
                <br />
                <br />
                Saisir l&#39;OF à partir duquel La FDV et la version sont applicables :
                <asp:TextBox ID="TextBox_NU_OF_ACPT" runat="server" Enabled="False"></asp:TextBox>
                <br />
                <br />
                        <asp:Button ID="Button_VALI_ENTER" runat="server" BackColor="#002F60" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Font-Bold="True" ForeColor="White" Height="40px" Text="Valider" Width="180px" Enabled="False" />
                    </td>
            <td>
    <asp:GridView ID="GridView_LIST_FDV" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical">
        <AlternatingRowStyle BackColor="#DCDCDC" />
        <Columns>
            <asp:CommandField ShowEditButton="True" />
        </Columns>
        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
        <HeaderStyle BackColor="#002F60" Font-Bold="True" ForeColor="#989000" />
        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
        <SortedAscendingCellStyle BackColor="#F1F1F1" />
        <SortedAscendingHeaderStyle BackColor="#0000A9" />
        <SortedDescendingCellStyle BackColor="#CAC9C9" />
        <SortedDescendingHeaderStyle BackColor="#000065" />
    </asp:GridView>

            </td>
        </tr>
    </table>

    
    <br />
</asp:Content>
