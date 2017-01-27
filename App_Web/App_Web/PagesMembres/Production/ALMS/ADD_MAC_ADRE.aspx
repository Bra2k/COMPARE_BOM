<%@ Page Title="Importer des adresses MAC" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ADD_MAC_ADRE.aspx.vb" Inherits="App_Web.ADD_MAC_ADRE" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="App_Themes/CSS/CSS_MediaQuery.css" rel="stylesheet" type="text/css" />
    <br />
    <div class="titre_page">
        <h2>Production : MAC adresses</h2>
    </div>
    <br />
    <asp:RadioButtonList ID="RadioButtonList_CHOI_IMPO" runat="server" AutoPostBack="True">
        <asp:ListItem Value="VAL_QT">&nbsp;Par quantité</asp:ListItem>
        <asp:ListItem Value="VAL_PLAG_ADRE">&nbsp;Par plage d&#39;adresse</asp:ListItem>
        <asp:ListItem Value="VAL_FICH">&nbsp;Par fichier</asp:ListItem>
    </asp:RadioButtonList>
    <asp:MultiView ID="MultiView_CHOI_IMPO_MAC_ADRE" runat="server">
        <asp:View ID="View_QT" runat="server">
            Importer
            <asp:TextBox ID="TextBox_QT" runat="server"></asp:TextBox>
            &nbsp;MAC adresse à partir de :
            <asp:TextBox ID="TextBox_PREM_MAC_ADRE" runat="server"></asp:TextBox>
            &nbsp;<asp:Button ID="Button_QT" runat="server" Text="Valider" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Height="40px" Width="180px" />
        </asp:View>
        <asp:View ID="View_PLAG_ADRE" runat="server">
            Importer la plage d&#39;adresse de :
            <asp:TextBox ID="TextBox_PREM_MAC_ADRE_2" runat="server"></asp:TextBox>
            &nbsp;à&nbsp;<asp:TextBox ID="TextBox_DERN_MAC_ADRE" runat="server"></asp:TextBox>
            &nbsp;<asp:Button ID="Button_PLAG_ADRE" runat="server" Text="Valider" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Height="40px" Width="180px" />
        </asp:View>
        <asp:View ID="View_FICH" runat="server">
            <asp:MultiView ID="MultiView_FICH" runat="server" ActiveViewIndex="0">
                <asp:View ID="View_UP_FICH" runat="server">
                    Sélectionner le fichier à importer :<br /><asp:FileUpload ID="FileUpload_FICH" runat="server" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" /><br />
                    <asp:Button ID="Button_UP_FICH" runat="server" Text="Valider" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Height="40px" Width="180px" />
                </asp:View>
                <asp:View ID="View_ONGL" runat="server">
                    Sélectionner l&#39;onglet dans lequel se trouve les données :<asp:DropDownList ID="DropDownList_ONGL" runat="server">
                    </asp:DropDownList>
                    &nbsp;<asp:Button ID="Button_ONGL" runat="server" Text="Valider" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Height="40px" Width="180px" />
                </asp:View>
                <asp:View ID="View_SEL_DONN" runat="server">
                    Saisir le numéro de la colonne à importer :
                    <asp:TextBox ID="TextBox_NU_COLO" runat="server"></asp:TextBox>
                    &nbsp;<asp:Button ID="Button_SAI_NU_COLO" runat="server" Text="Valider" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Height="40px" Width="180px" />
                    <asp:GridView ID="GridView_SEL_DONN" runat="server">
                    </asp:GridView>
                </asp:View>
                <asp:View ID="View_RESU" runat="server">
                </asp:View>
            </asp:MultiView>
            <br />
        </asp:View>
    </asp:MultiView>
</asp:Content>
