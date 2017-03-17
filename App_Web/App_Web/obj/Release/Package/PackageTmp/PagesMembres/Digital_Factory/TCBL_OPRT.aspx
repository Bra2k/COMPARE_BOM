<%@ Page Title="Saisie des opérations" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="TCBL_OPRT.aspx.vb" Inherits="App_Web.TCBL_OPRT" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width:100%;">
        <tr>
            <td style="vertical-align: top">
                <div class="titre_page">
                            <h2>Digital Factory : Opérations</h2>
                        </div>
                <asp:MultiView ID="MultiView_SAIS_OPRT" runat="server" ActiveViewIndex="0">
                    <asp:View ID="View_SAIS_OF_OP" runat="server">
                        
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
                        <br />
                        <br />
                        <asp:GridView ID="GridView1" runat="server">
                        </asp:GridView>
                    </asp:View>
                    <asp:View ID="View_SAIS_OTLG" runat="server">

  
                        
                        <asp:MultiView ID="MultiView_POST" runat="server">
                            <asp:View ID="View_SAIS_POST_MTRE_GNRQ" runat="server">Poste : <asp:Label ID="Label_POST" runat="server"></asp:Label><br />
                        <br />
                                Saisir le&nbsp;n° de(u)
                                <asp:Label ID="Label_TYPE_MTRE" runat="server"></asp:Label>
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
                            </asp:View>
                            <asp:View ID="View_SAIS_POST_MULT" runat="server">
                                Sélectionner le poste : <asp:DropDownList ID="DropDownList_POST" runat="server" AutoPostBack="True">
                        </asp:DropDownList>
                            </asp:View>
                        </asp:MultiView>
                        <br />

                    </asp:View>
                    <asp:View ID="View_SAIS_NU_SER_CHEC" runat="server">

                        Saisir le numéro de série :
                        <asp:TextBox ID="TextBox_NU_SER" runat="server" AutoPostBack="True"></asp:TextBox>
                        <br />
                        <br />
                        <asp:MultiView ID="MultiView_ETAP" runat="server">

                            <asp:View ID="View_VOID" runat="server">
                            </asp:View>

                            <asp:View ID="View_CHEC_ETAP" runat="server">
                                <asp:Label ID="Label_CONS" runat="server" Text=""></asp:Label>
                                <br />
                                <br />
                                <asp:Image ID="Image_PHOT_ILST" runat="server" Visible="False"/>
                                <br />
                                <br />
                                <table style="width:100%;">
                                    <tr>
                                        <td>
                                            <asp:Button ID="Button_PASS" runat="server" Text="Bon" BackColor="Lime" Font-Bold="True" Visible="False" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TextBox_VALE" runat="server" AutoPostBack="True" Visible="False"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Button ID="Button_FAIL" runat="server" BackColor="Red" Font-Bold="True" Text="Mauvais" Visible="False" />
                                        </td>
                                    </tr>
                                </table>

                            </asp:View>

                        </asp:MultiView>

                    </asp:View>
                    
                    <asp:View ID="View_SAIS_TCBL_COMP" runat="server">
                        <asp:MultiView ID="MultiView_Tracabilité" runat="server" ActiveViewIndex="0">
                            <asp:View ID="View_CONT_LOT_ID_COMP" runat="server">
                                <asp:RadioButtonList ID="RadioButtonList_SELE_COMP" runat="server" AutoPostBack="True" RepeatDirection="Horizontal" Width="500px">
                                    <asp:ListItem Selected="True">Bac</asp:ListItem>
                                    <asp:ListItem>ID Composant</asp:ListItem>
                                    <asp:ListItem>Code lot</asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:MultiView ID="MultiView_SELE_COMP" runat="server" ActiveViewIndex="0">
                                    <asp:View ID="View_BAC" runat="server">
                                        <label>
                                        Scanner un n° de BAC :</label>
                                        <asp:TextBox ID="TextBox_NU_BAC" runat="server"></asp:TextBox>
                                        <br />
                                    </asp:View>
                                    <asp:View ID="View_ID_COMP" runat="server">
                                        <label>
                                        Scanner un ID composant :</label>
                                        <asp:TextBox ID="TextBox_ID_COMP" runat="server"></asp:TextBox>
                                        <br />
                                    </asp:View>
                                    <asp:View ID="View_CD_LOT" runat="server">
                                        <asp:MultiView ID="MultiView_SAIS_CD_LOT" runat="server" ActiveViewIndex="0">
                                            <asp:View ID="View_SAIS_CD_LOT_SLCN" runat="server">
                                                <label>
                                                Sélectionner le composant :</label>
                                            </asp:View>
                                            <asp:View ID="View_SAIS_CD_LOT_SAIS" runat="server">
                                                <label>
                                                Entrer le code lot du composant</label>
                                                <asp:Label ID="Label_CD_COMP" runat="server"></asp:Label>
                                                &nbsp;:
                                                <asp:TextBox ID="TextBox_CD_LOT_COMP" runat="server" AutoPostBack="True"></asp:TextBox>
                                            </asp:View>
                                        </asp:MultiView>
                                    </asp:View>
                                </asp:MultiView>
                            </asp:View>
                            <asp:View ID="View_SAI_SOUS_ENSE" runat="server">
                                <label>
                                Entrer le numéro du sous-ensemble</label>
                                <asp:Label ID="Label_CD_SS_ENS" runat="server"></asp:Label>
                                :
                                <asp:TextBox ID="TextBox_SS_ENS" runat="server" AutoPostBack="True"></asp:TextBox>
                            </asp:View>
                        </asp:MultiView>
                        <asp:GridView ID="GridView_REPE" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" Font-Size="8pt" GridLines="Vertical" SkinID="GV_AFF_DONN">
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
                    <label>Quantité restante sur l'OF:</label>
                    <asp:Label ID="Label_QT_REST_OF" runat="server"></asp:Label>
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
            <td style="vertical-align: top">
                        <asp:Label ID="Label_RES" runat="server" Text=""></asp:Label>
                    </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td style="vertical-align: top">&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
    </table>
</asp:Content>
