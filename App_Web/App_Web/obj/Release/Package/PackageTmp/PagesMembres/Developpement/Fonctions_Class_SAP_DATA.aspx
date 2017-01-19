<%@ Page Title="Utilisation des fonctions de la CLASS_SAP_DATA" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Fonctions_Class_SAP_DATA.aspx.vb" Inherits="App_Web.Fonctions_Class_SAP_DATA" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:RadioButtonList ID="RadioButtonList_SAP_DATA" runat="server" AutoPostBack="True" CellPadding="10" RepeatDirection="Horizontal">
        <asp:ListItem>SAP_DATA_MATR</asp:ListItem>
        <asp:ListItem>SAP_DATA_LIST_CLIE</asp:ListItem>
        <asp:ListItem>SAP_DATA_NMCT_ARTI</asp:ListItem>
        <asp:ListItem>SAP_DATA_LIST_ARTI_CLIE</asp:ListItem>
        <asp:ListItem>SAP_DATA_READ_TBL</asp:ListItem>
        <asp:ListItem>SAP_DATA_Z_LOG_CONF_GET</asp:ListItem>
        <asp:ListItem>SAP_DATA_Z_LOG_ACT_GET</asp:ListItem>
    </asp:RadioButtonList>
    <asp:MultiView ID="MultiView_SAP_DATA" runat="server">
        <asp:View ID="View_SAP_DATA_MATR" runat="server">
            sMatricule :
            <asp:TextBox ID="TextBox_SAP_DATA_MATR" runat="server"></asp:TextBox>
        </asp:View>
        <asp:View ID="View_SAP_DATA_LIST_CLIE" runat="server">
            sArticle :
            <asp:TextBox ID="TextBox_SAP_DATA_LIST_CLIE" runat="server"></asp:TextBox>
        </asp:View>
        <asp:View ID="View_SAP_DATA_NMCT_ARTI" runat="server">
            sFiltre :
            <asp:TextBox ID="TextBox_SAP_DATA_NMCT_ARTI" runat="server"></asp:TextBox>
         </asp:View>
        
        <asp:View ID="View_SAP_DATA_LIST_ARTI_CLIE" runat="server">
            sClient :
            <asp:TextBox ID="TextBox_SAP_DATA_LIST_ARTI_CLIE_sClient" runat="server"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; sType_produit :
            <asp:TextBox ID="TextBox_SAP_DATA_LIST_ARTI_CLIE_sType_produit" runat="server"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="Button_SAP_DATA_LIST_ARTI_CLIE" runat="server" Text="Valider" />
        </asp:View>
        <asp:View ID="View_SAP_DATA_READ_TBL" runat="server">
            Table :
            <asp:TextBox ID="TextBox_SAP_DATA_READ_TBL_Table" runat="server"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Delimiter :
            <asp:TextBox ID="TextBox_SAP_DATA_READ_TBL_Delimiter" runat="server" Text="|"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; NoData :
            <asp:TextBox ID="TextBox_SAP_DATA_READ_TBL_NoData" runat="server"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Field :
            <asp:TextBox ID="TextBox_SAP_DATA_READ_TBL_Field" runat="server"></asp:TextBox>
            <br />
            option_req :
            <asp:TextBox ID="TextBox_SAP_DATA_READ_TBL_option_req" runat="server"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; fieldinarray :
            <asp:TextBox ID="TextBox_SAP_DATA_READ_TBL_fieldinarray" runat="server" Text="0"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ligneskip :
            <asp:TextBox ID="TextBox_SAP_DATA_READ_TBL_ligneskip" runat="server" Text="0"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <br />
            nbrlignes :
            <asp:TextBox ID="TextBox_SAP_DATA_READ_TBL_nbrlignes" runat="server">0</asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="Button_SAP_DATA_READ_TBL_Valid" runat="server" Text="Valider" />
        </asp:View>
        <asp:View ID="View_SAP_DATA_Z_LOG_CONF_GET" runat="server">
            V_AUFNR :
            <asp:TextBox ID="TextBox_V_AUFNR" runat="server"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;V_VORNR :
            <asp:TextBox ID="TextBox_V_VORNR" runat="server"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;V_PERNR :
            <asp:TextBox ID="TextBox_V_PERNR" runat="server"></asp:TextBox>
            <br />
            <br />
            DATE_DEBUT :<asp:Calendar ID="Calendar_DATE_DEBUT_1" runat="server"></asp:Calendar>
            DATE_FIN :<asp:Calendar ID="Calendar_DATE_FIN_2" runat="server"></asp:Calendar>
            <asp:Button ID="Button_SAP_DATA_Z_LOG_CONF_GET_Valid" runat="server" Text="Valider" />
        </asp:View>
        <asp:View ID="View_SAP_DATA_Z_LOG_ACT_GET" runat="server">
            V_PERNR :
            <asp:TextBox ID="TextBox_V_PERNR_1" runat="server"></asp:TextBox>
            <br />
            <br />
            DATE_DEBUT :<asp:Calendar ID="Calendar_DATE_DEBUT_2" runat="server"></asp:Calendar>
            DATE_FIN :<asp:Calendar ID="Calendar_DATE_FIN_3" runat="server" SelectedDate="08/09/2016 15:01:38"></asp:Calendar>
            <asp:Button ID="Button_SAP_DATA_Z_LOG_ACT_GET_Valid" runat="server" Text="Valider" />
        </asp:View>
    </asp:MultiView>
    <asp:GridView ID="GridView_SAP_DATA" runat="server">
    </asp:GridView>
</asp:Content>
