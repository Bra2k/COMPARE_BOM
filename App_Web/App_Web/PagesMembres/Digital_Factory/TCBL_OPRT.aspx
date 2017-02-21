<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="TCBL_OPRT.aspx.vb" Inherits="App_Web.TCBL_OPRT" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width:100%;">
        <tr>
            <td>
                <asp:MultiView ID="MultiView_SAIS_OPRT" runat="server" ActiveViewIndex="0">
                    <asp:View ID="View_SAIS_OF_OP" runat="server">
                        <div class="titre_page">
                            <h2>Digital Factory : Opérations</h2>
                        </div>
                        <br />
                        <label>
                        Entrer l&#39;OF :</label>
                        <asp:TextBox ID="TextBox_OF" runat="server" AutoPostBack="True"></asp:TextBox>
                        <br />
                        <br />
                        <label>
                        Entrer l&#39;OP :</label>
                        <asp:DropDownList ID="DropDownList_OP" runat="server">
                        </asp:DropDownList>
                        <br />
                        <br />
                        <asp:Button ID="Button_VALI_ENTER" runat="server" BackColor="#002F60" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Font-Bold="True" ForeColor="White" Height="40px" Text="Valider" Width="180px" />
                    </asp:View>
                    <asp:View ID="View_SAIS_OTLG" runat="server">

  
                        Poste : [Label_POST]<br />
                        <br />
                        Saisir le&nbsp;<asp:Label ID="Label_TYPE_MTRE" runat="server" Text=""></asp:Label>
                        &nbsp;:<br />
                        <asp:TextBox ID="TextBox_MTRE_GNRQ" runat="server"></asp:TextBox>
                        <br />
                        <br />
                        <asp:GridView ID="GridView_LIST_MTRE" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical">
                            <AlternatingRowStyle BackColor="#DCDCDC" />
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
                        <br />
                        <asp:Button ID="Button_VALI_ENTER_OTLG" runat="server" BackColor="#002F60" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Font-Bold="True" ForeColor="White" Height="40px" Text="Valider" Width="180px" />
                        <br />

                    </asp:View>
                    <asp:View ID="View_SAIS_NU_SER_CHEC" runat="server">

                        Saisir le numéro de série :
                        <asp:TextBox ID="TextBox_NU_SER" runat="server"></asp:TextBox>
                        <br />
                        <br />
                        <asp:MultiView ID="MultiView_ETAP" runat="server">
                            <asp:View ID="View_SAIS_TCBL_COMP" runat="server">

                    </asp:View>
                        </asp:MultiView>

                    </asp:View>
                    
                </asp:MultiView>
            </td>
            <td style="vertical-align: top"><label>OF :</label>
                    <asp:Label ID="Label_OF" runat="server"></asp:Label>
                    <br />
                    <label>Client :</label>
                    <asp:Label ID="Label_CLIE" runat="server"></asp:Label>
                    <br />
                    <label>Code article :</label>
                    <asp:Label ID="Label_CD_ARTI" runat="server"></asp:Label>
                    <br />
                    <label>Désignation Article :</label>
                    <asp:Label ID="Label_DES_ARTI" runat="server"></asp:Label>
                    <br />
                    <label>Opération :</label>
                    <asp:Label ID="Label_OP" runat="server"></asp:Label>
                    <br />
                    <label>Désignation de l&#39;opération :</label>
                    <asp:Label ID="Label_DES_OP" runat="server"></asp:Label>
                    <br />
                    <label>Quantité :</label>
                    <asp:Label ID="Label_QT_OF" runat="server"></asp:Label>
                    <br />
                    <br />
                    <label>Numéros de série tracés :</label>
                    <br />
                    <asp:GridView ID="GridView_SN_TRAC" runat="server" SkinID="GV_AFF_DONN" PageSize="7" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" Font-Size="10pt">
                        <AlternatingRowStyle BackColor="#DCDCDC" />
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
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
    </table>
</asp:Content>
