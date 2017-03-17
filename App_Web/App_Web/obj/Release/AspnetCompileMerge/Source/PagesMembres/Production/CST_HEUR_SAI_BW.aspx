<%@ Page Title="Consulter les heures de saisie" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="CST_HEUR_SAI_BW.aspx.vb" Inherits="App_Web.CST_HEUR_SAI_BW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View_SAI_CHEF_EQUI" runat="server">
            Entrer votre marticule :
            <asp:TextBox ID="TextBox_Matricule" runat="server"></asp:TextBox>
            <br />
        </asp:View>
        <asp:View ID="View_HEUR" runat="server">
            <table style="width: 100%; vertical-align: top;">
                <tr>
                    <td style="width: 201px">
                        <asp:CheckBoxList ID="CheckBoxList_LIST_OP" runat="server">
                        </asp:CheckBoxList>
                    </td>
                    <td>
                        <asp:GridView ID="GridView_HEUR" runat="server">
                        </asp:GridView>
                    </td>
                </tr>
            </table>
            <br />
            <br />
        </asp:View>
    </asp:MultiView>
</asp:Content>
