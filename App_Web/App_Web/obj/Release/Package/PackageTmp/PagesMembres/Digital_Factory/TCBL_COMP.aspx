<%@ Page Title="Saisie des composants sur une carte" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="TCBL_COMP.aspx.vb" Inherits="App_Web.TCBL_COMP" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server"> 
        <br />
        <table style="width:100%;">
            <tr>
                <td style="vertical-align: top;">
        <asp:MultiView ID="MultiView_Tracabilité" runat="server" ActiveViewIndex="0">
            <asp:View ID="View_DATA_ENTR" runat="server">
                <div style="font-family: Verdana; color: #002F60">
                    <h2>Digital Factory : Composants&nbsp;&nbsp;&nbsp;</h2>
                    <asp:Label ID="Label_LOAD" runat="server" Text="Chargement en cours ..." Visible="False"></asp:Label>
                </div><br />
                <label>Entrer l&#39;OF :</label>
                <asp:TextBox ID="TextBox_OF" runat="server" AutoPostBack="True"></asp:TextBox>
                <br />
                <br />
                <label>Entrer l&#39;OP :</label>
                <asp:DropDownList ID="DropDownList_OP" runat="server">
                </asp:DropDownList>
                <br />
                <br />
                <asp:CheckBox ID="CheckBox_GENE_ETI_ENS" runat="server" AutoPostBack="True" Text="&nbsp;Générer les étiquettes d'ensemble" />
                <br />
                <asp:CheckBox ID="CheckBox_SS_ENS_FERT" runat="server" AutoPostBack="True" Checked="True" Text="&nbsp;Sous-ensemble déclaré dans SAP" />
                <br />
                <br />
                <asp:Button ID="Button_VALI_ENTER" runat="server" Text="Valider" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Height="40px" Width="180px"/>
            </asp:View>
            <asp:View ID="View_SAIS_OTLG" runat="server">
            </asp:View>
            <asp:View ID="View_CONT_LOT_ID_COMP" runat="server">
                <asp:RadioButtonList ID="RadioButtonList_SELE_COMP" runat="server" AutoPostBack="True" RepeatDirection="Horizontal" Width="500px">
                    <asp:ListItem Selected="True">Bac</asp:ListItem>
                    <asp:ListItem>ID Composant</asp:ListItem>
                    <asp:ListItem>Code lot</asp:ListItem>
                </asp:RadioButtonList>
                <asp:MultiView ID="MultiView_SELE_COMP" runat="server" ActiveViewIndex="0">
                    <asp:View ID="View_BAC" runat="server">Scanner un n° de BAC :
                        <asp:TextBox ID="TextBox_NU_BAC" runat="server"></asp:TextBox>
                    </asp:View>
                    <asp:View ID="View_ID_COMP" runat="server">Scanner un ID composant :
                        <asp:TextBox ID="TextBox_ID_COMP" runat="server"></asp:TextBox>
                    </asp:View>
                    <asp:View ID="View_CD_LOT" runat="server">
                        <asp:MultiView ID="MultiView_SAIS_CD_LOT" runat="server">
                            <asp:View ID="View_SAIS_CD_LOT_SLCN" runat="server">
                                Sélectionner le composant :
                            </asp:View>
                            <asp:View ID="View_SAIS_CD_LOT_SAIS" runat="server">
                                Entrer le code lot du composant
                                <asp:Label ID="Label_CD_COMP" runat="server"></asp:Label>
                                &nbsp;:
                                <asp:TextBox ID="TextBox_CD_LOT_COMP" runat="server" AutoPostBack="True"></asp:TextBox>
                            </asp:View>
                        </asp:MultiView>
                    </asp:View>
                </asp:MultiView>
            </asp:View>
            <asp:View ID="View_SAIS_ENS" runat="server">
                Entrer le numéro d&#39;ensemble :
                <asp:TextBox ID="TextBox_ENS" runat="server" AutoPostBack="True"></asp:TextBox>
                <br />
                <asp:MultiView ID="MultiView_BASC_SAI_SEL" runat="server" ActiveViewIndex="0">
                    <asp:View ID="View_VOID" runat="server">
                    </asp:View>
                    <asp:View ID="View_SAI" runat="server">
                        Entrer le numéro du sous-ensemble
                        <asp:Label ID="Label_CD_SS_ENS" runat="server"></asp:Label>
                        :
                        <asp:TextBox ID="TextBox_SS_ENS" runat="server" AutoPostBack="True"></asp:TextBox>
                    </asp:View>
                </asp:MultiView>
                <br />
            </asp:View>
        </asp:MultiView><asp:GridView ID="GridView_REPE" runat="server" SkinID="GV_AFF_DONN" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" Font-Size="8pt">
                        <AlternatingRowStyle BackColor="#DCDCDC" />
                    <Columns>
                        <asp:CommandField ShowSelectButton="True" />
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
                    <asp:Label ID="Label_RES" runat="server" Text=""></asp:Label>
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
                    <label>Numéros de série tracés :</label><br />
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
        </table>
        <br />
</asp:Content>
