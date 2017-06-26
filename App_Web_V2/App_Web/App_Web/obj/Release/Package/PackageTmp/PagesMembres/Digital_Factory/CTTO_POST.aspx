<%@ Page Title="Consitution des postes" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="CTTO_POST.aspx.vb" Inherits="App_Web.CTTO_POST" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <table style="width:100%;">
                <tr>
                    <td>
                        <div class="titre_page">
                    <h2>Constitution des postes Digital Factory</h2>
                </div><br />
    <asp:MultiView ID="MultiView_SAIS_CTTO" runat="server" ActiveViewIndex="0">
        <asp:View ID="View_SAIS" runat="server">
            <table style="width:100%;">
                <tr>
                    <td>Entrer le code article : </td>
                    <td>
                        <asp:TextBox ID="TextBox_CD_ARTI_ECO" runat="server" AutoPostBack="True"></asp:TextBox>
                    </td>
                    
                </tr>
                <tr>
                    <td>Sélectionner l&#39;opération : </td>
                    <td>
                        <asp:DropDownList ID="DropDownList_NU_OPRT" runat="server">
                        </asp:DropDownList>
                    </td>
                    
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="Button_VALI_SAIS" runat="server" BackColor="#002F60" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Font-Bold="True" ForeColor="White" Height="40px" Text="Valider" Width="180px" />
                        <br />
                        <br />
                        OU<br />
                        <br />
                        <asp:Button ID="Button_SAIS_MTRE" runat="server" Text="Nouveau Matériel" />
                    </td>
                   
                </tr>
            </table>
        </asp:View>
        <asp:View ID="View_NMCT_TPLG" runat="server">
            <table style="width:100%;">
                <tr>
                    <td colspan="5">
                        <asp:GridView ID="GridView_NMCT_TPLG" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" AutoGenerateColumns="False" DataSourceID="SqlDataSource_NMCT_TPLG">
                            <AlternatingRowStyle BackColor="#DCDCDC" />
                            <Columns>
                                <asp:BoundField DataField="Matériel" HeaderText="Matériel" SortExpression="Matériel" />
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
                        <asp:SqlDataSource ID="SqlDataSource_NMCT_TPLG" runat="server" ConnectionString="<%$ ConnectionStrings:MES_Digital_FactoryConnectionString %>" SelectCommand="SELECT NM_TYPE_MTRE AS Matériel FROM V_POST_NMCT_TPLG WHERE (CD_ARTI_ECO_POST = @CD_ARTI_ECO_POST) AND (NU_OP_POST = @NU_OP_POST)">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="Label_CD_ARTI_ECO" Name="CD_ARTI_ECO_POST" PropertyName="Text" />
                                <asp:ControlParameter ControlID="Label_NU_OPRT" Name="NU_OP_POST" PropertyName="Text" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top">
                        <asp:Button ID="Button_AJOU_MTRE" runat="server" Text="+" />
                    </td>
                    <td style="vertical-align: top">Sélectionner un matériel :
                        <asp:DropDownList ID="DropDownList_TYPE_MTRE_TPLG" runat="server" DataSourceID="SqlDataSource_TYPE_MTRE" DataTextField="VAL_PARA" DataValueField="VAL_PARA" AutoPostBack="True" Enabled="False">
                        </asp:DropDownList>
                    </td>
                    <td style="vertical-align: top">
                        <asp:CheckBox ID="CheckBox_ENAB_SELE_ID_MTRE" runat="server" AutoPostBack="True" />
                    </td>
                    <td style="vertical-align: top">Sélectionner un id matériel qui sera dédié à ce poste :
                        <br />
                        <asp:SqlDataSource ID="SqlDataSource_ID_MTRE_DEDI" runat="server" ConnectionString="<%$ ConnectionStrings:MES_Digital_FactoryConnectionString %>" SelectCommand="SELECT ID_MTRE FROM V_POST_LIST_MTRE WHERE (NM_TYPE_MTRE = @NM_TYPE_MTRE)">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="DropDownList_TYPE_MTRE_TPLG" Name="NM_TYPE_MTRE" PropertyName="SelectedValue" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <asp:DropDownList ID="DropDownList_ID_MTRE_DEDI" runat="server" DataSourceID="SqlDataSource_ID_MTRE_DEDI" DataTextField="ID_MTRE" DataValueField="ID_MTRE" Enabled="False">
                        </asp:DropDownList>
                        <br />
                        <asp:Button ID="Button_AJOU_MTRE_CREE" runat="server" Text="+" />
                    </td>
                    <td style="vertical-align: top">
                        <asp:Button ID="Button_VALI_MTRE_TPLG" runat="server" BackColor="#002F60" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Font-Bold="True" ForeColor="White" Height="40px" Text="Valider" Width="180px" />
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="View_SAIS_NEW_MTRE" runat="server">
            
            
            <table style="width:100%;">
                <tr>
                    <td>Saisir l&#39;id du matériel : </td>
                    <td><asp:TextBox ID="TextBox_ID_MTRE" runat="server" AutoPostBack="True"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Sélectionner le type de matériel :</td>
                    <td><asp:DropDownList ID="DropDownList_TYPE_MTRE" runat="server" DataSourceID="SqlDataSource_TYPE_MTRE" DataTextField="VAL_PARA" DataValueField="VAL_PARA"></asp:DropDownList>
            <asp:SqlDataSource ID="SqlDataSource_TYPE_MTRE" runat="server" ConnectionString="<%$ ConnectionStrings:MES_Digital_FactoryConnectionString %>" SelectCommand="SELECT VAL_PARA FROM DTM_REF_PARA WHERE (NM_PARA = N'Type matériel') GROUP BY VAL_PARA ORDER BY VAL_PARA"></asp:SqlDataSource></td>
                </tr>
                <tr>
                    <td>Saisir la référence du matériel (facultatif) : </td>
                    <td>
                        <asp:TextBox ID="TextBox_RFRC_MTRE" runat="server"></asp:TextBox></td>
                </tr>
            </table>
            <br />
            <asp:Button ID="Button_VALI_MTRE" runat="server" BackColor="#002F60" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Font-Bold="True" ForeColor="White" Height="40px" Text="Valider" Width="180px" />
            
        </asp:View>
    </asp:MultiView></td>
                    <td>POSTE n° :
                        <asp:Label ID="Label_NU_POST" runat="server" Text=""></asp:Label>
                        <br />
                        Code article Eolane :
                        <asp:Label ID="Label_CD_ARTI_ECO" runat="server" Text=""></asp:Label>
                        <br />
                        Opération : 
                        <asp:Label ID="Label_NU_OPRT" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
    <br />
    <br />
</asp:Content>
