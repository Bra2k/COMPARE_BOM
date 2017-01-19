<%@ Page Title="Saisie de l'association des numéros de série Ensemble / Sous-ensemble" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="TCBL_ESB_SS_ESB_V2.aspx.vb" Inherits="App_Web.TCBL_ESB_SS_ESB_V2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
        DIGITAL FACTORY : Saisie de l&#39;association des numéros de série Ensemble / Sous-ensemble<table style="width:100%;">
            <tr>
                <td style="vertical-align: top;">
    
        <asp:MultiView ID="MultiView_Tracabilité" runat="server" ActiveViewIndex="0">
            <asp:View ID="View_DATA_ENTR" runat="server">
                Entrer l&#39;OF :
                <asp:TextBox ID="TextBox_OF" runat="server"></asp:TextBox>
                <br />
                <br />
                Entrer l&#39;OP :
                <asp:TextBox ID="TextBox_OP" runat="server"></asp:TextBox>
                <br />
                <br />
                <asp:CheckBox ID="CheckBox_GENE_ETI_ENS" runat="server" Text="Générer les étiquettes d'ensemble" AutoPostBack="True" />
                <br />
                <asp:CheckBox ID="CheckBox_SS_ENS_FERT" runat="server" Text="Sous-ensemble déclaré dans SAP" Checked="True" AutoPostBack="True" />
                <br />
                <asp:Button ID="Button_VALI_ENTER" runat="server" Text="Valider" />
            </asp:View>
            <asp:View ID="View_SAIS_ENS" runat="server">
                Entrer le numéro d&#39;ensemble :
                <asp:TextBox ID="TextBox_ENS" runat="server" AutoPostBack="True"></asp:TextBox>
                <br />
                <br />
                Options de saisie :<br />
                <br />
                <asp:CheckBox ID="CheckBox_SAI_AUTO" runat="server" Checked="True" Text="Mode saisie automatique" AutoPostBack="True" />
                <br />
                <asp:CheckBox ID="CheckBox_NU_SER_EOL" runat="server" Text="Numéro de série Eolane Combree (OF + Incrément à 6 chiffres, par exemple : 1156566000001)" AutoPostBack="True" />
                <br />
            </asp:View>
            <asp:View ID="View_SAIS_SS_ENS" runat="server">
                Numéro d&#39;ensemble :
                <asp:Label ID="Label_NS_ENS" runat="server" Text=""></asp:Label>
                <br />
                <asp:MultiView ID="MultiView_BASC_SAI_SEL" runat="server" ActiveViewIndex="0">
                    <asp:View ID="View_SEL" runat="server">
                        Sélectionner le sous-ensemble :
                    </asp:View>
                    <asp:View ID="View_SAI" runat="server">
                        Entrer le numéro du sous-ensemble
                        <asp:Label ID="Label_CD_SS_ENS" runat="server"></asp:Label>
                        :
                        <asp:TextBox ID="TextBox_SS_ENS" runat="server"></asp:TextBox>
                    </asp:View>
                </asp:MultiView>
                <br />
                <asp:GridView ID="GridView_REPE" runat="server" SkinID="GV_AFF_DONN">
                    <Columns>
                        <asp:CommandField ShowSelectButton="True" />
                    </Columns>
                </asp:GridView>
            </asp:View>
        </asp:MultiView>
                    <asp:Label ID="Label_RES" runat="server" Text=""></asp:Label>
                </td>
                <td style="vertical-align: top">OF :
                    <asp:Label ID="Label_OF" runat="server"></asp:Label>
                    <br />
                    Client :
                    <asp:Label ID="Label_CLIE" runat="server"></asp:Label>
                    <br />
                    Code article :
                    <asp:Label ID="Label_CD_ARTI" runat="server"></asp:Label>
                    <br />
                    Désignation Article :
                    <asp:Label ID="Label_DES_ARTI" runat="server"></asp:Label>
                    <br />
                    Opération :
                    <asp:Label ID="Label_OP" runat="server"></asp:Label>
                    <br />
                    Désignation de l&#39;opération :
                    <asp:Label ID="Label_DES_OP" runat="server"></asp:Label>
                    <br />
                    Quantité :
                    <asp:Label ID="Label_QT_OF" runat="server"></asp:Label>
                    <br />
                    <br />
                    Numéros de série tracés :<br />
                    <asp:GridView ID="GridView_SN_TRAC" runat="server" SkinID="GV_AFF_DONN" AllowPaging="True" PageSize="7">
                    </asp:GridView>
                </td>
            </tr>
        </table>
        <br />
    
        </asp:Content>
