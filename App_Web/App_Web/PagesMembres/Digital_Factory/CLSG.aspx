<%@ Page Title="Colisage" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="CLSG.aspx.vb" Inherits="App_Web.CLSG" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width:100%;">
        <tr>
            <td>
                <br />
                <br />
                <br />
                <br />
            </td>
            <td>
            <asp:MultiView ID="MultiView_SAIS" runat="server" ActiveViewIndex="0">
                <asp:View ID="View_OF" runat="server">
                    <table style="width:100%;">
                        <tr>
                            <td><label>Saisir l&#39;OF hello:</label>
                                <asp:TextBox ID="TextBox_NU_OF" runat="server" AutoPostBack="True" Width="128px"></asp:TextBox>
                            </td>
                            <td style="vertical-align: middle"><label>
                                Associer un n° de palette à un BL :</label>
                                <asp:ImageButton ID="Button_SAI_PALE_BL" runat="server" Height="63px" ImageUrl="~/App_Themes/PIC_CHAR/AjoutBonDeLivraison.png" ToolTip="Associer un n° de palette à un BL"  Width="63px"/>
                            </td>
                            <td>
                                <label>Associer des cartons à un BL :</label>
                                <asp:ImageButton ID="Button_SAI_CART_BL_OF" runat="server" Height="63px" ImageUrl="~/App_Themes/PIC_CHAR/depositphotos_111867916-stock-illustration-cargo-boxes-pallet-icon.jpg" ToolTip="Associer des cartons à un BL" Width="63px" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="View_NU_SER" runat="server">
                    <table style="width:100%;">
                        <tr>
                            <td rowspan="2">
                               
                                <asp:ImageButton ID="Button_CLOR_CART" runat="server" Enabled="False" Height="63px" Width="63px"  ToolTip="Clore le carton" ImageUrl="~/App_Themes/PIC_CHAR/39344.png"/>
                                <br />
                                <br />
                                <br />
                                <asp:ImageButton ID="Button_SAI_CART_BL" runat="server" Enabled="False" Height="63px" ToolTip="Associer des cartons à un BL" Width="63px"  ImageUrl="~/App_Themes/PIC_CHAR/depositphotos_111867916-stock-illustration-cargo-boxes-pallet-icon.jpg" Visible="False" />
                                <br />
                                <br />
                                <br />
                                <asp:ImageButton ID="ImageButton_CLOT_PALE" runat="server" Height="63px" ImageUrl="~/App_Themes/PIC_CHAR/depositphotos_107060668-stock-illustration-forklift-truck-with-boxes-icon.jpg" ToolTip="Cloture de la palette" Width="63px" />
                                <br />
                                <br />
                                <br />
                                <asp:ImageButton ID="Button_ANNU_SAIS_CART" runat="server" Height="63px" ToolTip="Annuler la saisie d'un carton" Width="63px" ImageUrl="~/App_Themes/PIC_CHAR/Custom-Icon-Design-Pretty-Office-9-Delete-file.ico" />
                            </td>
                            <td>
                                <asp:MultiView ID="MultiView_BASC_SAI_SEL" runat="server" ActiveViewIndex="0">
                                    <asp:View ID="View_SAI_ENS" runat="server">
                                        <label>N° de carton :</label>
                                        <asp:TextBox ID="TextBox_NU_CART_SCFQ" runat="server" AutoPostBack="True" Enabled="False"></asp:TextBox>
                                        <br />
                                        <br />
                                        <label>Numéro de série Eolane :</label>
                                        <asp:TextBox ID="TextBox_NU_SER_ECO" runat="server" AutoPostBack="True" Enabled="False"></asp:TextBox>
                                        <br />
                                        <br />
                                        <label>Numéro de série Client :</label>
                                        <asp:TextBox ID="TextBox_NU_SER_CLIE" runat="server" AutoPostBack="True" Enabled="False"></asp:TextBox>
                                    </asp:View>
                                    <asp:View ID="View_SEL" runat="server">
                                        Sélectionner le sous-ensemble :
                                    </asp:View>
                                    <asp:View ID="View_SAI" runat="server">
                                        Entrer le numéro du sous-ensemble
                                        <asp:Label ID="Label_CD_SS_ENS" runat="server"></asp:Label>
                                        :
                                        <asp:TextBox ID="TextBox_SS_ENS" runat="server" AutoPostBack="True"></asp:TextBox>
                                    </asp:View>
                                </asp:MultiView>
                                <asp:GridView ID="GridView_REPE" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" SkinID="GV_AFF_DONN" Font-Names="Verdana" Font-Size="8pt">
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
                                <br />
                            </td>
                            <td style="vertical-align: top">OF :
                                <asp:Label ID="Label_NU_OF" runat="server"></asp:Label>
                                <br />
                                Client :
                                <asp:Label ID="Label_NM_CLIE" runat="server"></asp:Label>
                                <br />
                                Code Article :
                                <asp:Label ID="Label_CD_ARTI_ECO" runat="server"></asp:Label>
                                <br />
                                Désignation Article :
                                <asp:Label ID="Label_NM_DSGT_ARTI" runat="server"></asp:Label>
                                <br />
                                Quantité de l&#39;OF :
                                <asp:Label ID="Label_QT_OF" runat="server"></asp:Label>
                                <br />
                                <br />
                                Quantité restante dans l&#39;OF :
                                <asp:Label ID="Label_QT_REST_OF" runat="server"></asp:Label>
                                <br />
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>Numéro de carton :
                                <asp:Label ID="Label_NU_CART" runat="server"></asp:Label>
                                <br />
                                Nombre de scan :
                                <asp:Label ID="Label_NB_CART" runat="server"></asp:Label>
                                <br />
                                <br />
                                Numéros de série déjà scannés :
                                <asp:GridView ID="GridView_NU_SER_SCAN" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical">
                                    <AlternatingRowStyle BackColor="#DCDCDC" />
                                    <Columns>
                                        <asp:CommandField SelectText="Sél" ShowSelectButton="True" />
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
                                <asp:ImageButton ID="Button_SUPP_LIGN" runat="server" Height="36px" ImageUrl="~/App_Themes/PIC_CHAR/imgres.jpg" tooltip="Supprimer un enregistrement" Width="36px" />
                                <br />
                                
                            </td>
                            <td style="vertical-align: top">Numéro de la palette :
                                <asp:Label ID="Label_NU_PALE_NU_V_NU_SER" runat="server"></asp:Label>
                                &nbsp;(ne pas oublier de reporter le numéro suivant sur le BL manuel)<br />
                                Nombre de cartons :
                                <asp:Label ID="Label_NB_CART_PALE" runat="server"></asp:Label>
                                <br />
                                <br />
                                Liste de carton :
                                <asp:GridView ID="GridView_LIST_CART" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical">
                                    <AlternatingRowStyle BackColor="#DCDCDC" />
                                    <Columns>
                                        <asp:CommandField SelectText="Sél" ShowSelectButton="True" />
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
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="View_PALE" runat="server">
                    <table style="width:100%;">
                        <tr>
                            <td style="vertical-align: middle">
                                <br />
                                <asp:imageButton ID="Button_CLOR_LIVR" runat="server" Height="63px" Width="63px" ImageUrl="~/App_Themes/PIC_CHAR/delivery_256.png" />
                                <br />
                                <br />
                            </td>
                            <td>Numéro de BL :
                                <asp:TextBox ID="TextBox_NU_BL_V_PALE" runat="server" AutoPostBack="True"></asp:TextBox>
                                <br />
                                <br />
                                Numéro de palette :
                                <asp:TextBox ID="TextBox_NU_PALE" runat="server" AutoPostBack="True" Enabled="False"></asp:TextBox>
                                <br />
                                <br />
                                Liste des palettes déjà entrées :<br /> <br />
                                <asp:GridView ID="GridView_LIST_PALE_ENTR" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical">
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
                            <td style="vertical-align: top">Client :
                                <asp:Label ID="Label_NM_CLIE_V_PALE" runat="server"></asp:Label>
                                <br />
                                Code Article :
                                <asp:Label ID="Label_CD_ARTI_ECO_V_PALE" runat="server"></asp:Label>
                                <br />
                                Désignation Article :
                                <asp:Label ID="Label_NM_DSGT_ARTI_V_PALE" runat="server"></asp:Label>
                                <br />
                                <br />
                                Liste des palettes potentielles:<br />
                                <br />
                                <asp:GridView ID="GridView_LIST_PALE_LIBR" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical">
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
                </asp:View>
                <asp:View ID="View_CART" runat="server">
                    <table style="width:100%;">
                        <tr>
                            <td rowspan="2" style="vertical-align: middle">
                                <br />
                                <asp:Button ID="Button_CLOR_PALE" runat="server" Height="63px" Text="Clore la palette" Width="63px" />
                                <br />
                                <br />
                                <br />
                                <asp:ImageButton ID="Button_ANNU_SAIS_PALE" runat="server" Height="63px" Text="Annuler la saisie d'un carton" Width="63px" ImageUrl="~/App_Themes/PIC_CHAR/Custom-Icon-Design-Pretty-Office-9-Delete-file.ico" />
                                <br />
                                <br />
                            </td>
                            <td>Numéro de BL :
                                <asp:TextBox ID="TextBox_NU_BL" runat="server" AutoPostBack="True"></asp:TextBox>
                                <br />
                                <br />
                                Numéro de carton : 
                                <asp:TextBox ID="TextBox_NU_CART" runat="server" Enabled="False" AutoPostBack="True"></asp:TextBox>
                            </td>
                            <td style="vertical-align: top">Numéro de palette : 
                                <asp:Label ID="Label_NU_PALE" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Nombre de scan :
                                <asp:Label ID="Label_NB_CART_SCAN" runat="server"></asp:Label>
                                <br />
                                <br />
                                Liste de carton :
                                <asp:GridView ID="GridView_NU_CART" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical">
                                    <AlternatingRowStyle BackColor="#DCDCDC" />
                                    <Columns>
                                        <asp:CommandField SelectText="Sél" ShowSelectButton="True" />
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
                                <asp:ImageButton ID="Button_SUPP_LIGN_V_CART" runat="server" Height="36px" ImageUrl="~/App_Themes/PIC_CHAR/imgres.jpg" tooltip="Supprimer un enregistrement" Width="36px" />
                            </td>
                            <td style="vertical-align: top">Client :
                                <asp:Label ID="Label_NM_CLIE_V_CART" runat="server"></asp:Label>
                                <br />
                                Code Article :
                                <asp:Label ID="Label_CD_ARTI_ECO_V_CART" runat="server"></asp:Label>
                                <br />
                                Désignation Article : 
                                <asp:Label ID="Label_NM_DSGT_ARTI_V_CART" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                  
                </asp:View>
                <asp:View ID="View_TCBL_SS_ENSE" runat="server">
                    <br />
                    <br />
                </asp:View>
            </asp:MultiView>
            </td>         
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <asp:Label ID="Label_NM_DSGT_ARTI_ECO" runat="server" Visible="False">Colisage</asp:Label>
                <asp:Label ID="Label_NU_OP" runat="server" Visible="False"></asp:Label>
            </td>
        </tr>
        </table>

    <iframe id="pdf" name = "pdf" src ="02.pdf" style="width: 768px; height: 1024px;" hidden="hidden"></iframe>
</asp:Content>

