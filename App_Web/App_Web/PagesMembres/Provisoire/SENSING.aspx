<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="SENSING.aspx.vb" Inherits="App_Web.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <link href="App_Themes/CSS/CSS_MediaQuery.css" rel="stylesheet" type="text/css" />
        <div class="titre_page">
                <h2>Provisoire : SENSING LABS</h2>
        </div>
        <br />
        <asp:MultiView ID="MultiView_Sensing" runat="server" ActiveViewIndex="0">
            <asp:View ID="View_DATA_ENTR" runat="server">
                <div class="data_entries">
                    <label>Saisir l&#39;OF :</label>
                          <asp:TextBox ID="TextBox_NU_OF" runat="server" AutoPostBack="True" Width="128px"></asp:TextBox>
                    <br />
                    <br />
                    <label>Choisissez l&#39;OP :</label>
                          <asp:DropDownList ID="DropDownList_OP" runat="server"></asp:DropDownList>
                    <br />
                    <br />
                    <label>Article associé :</label>
                                <asp:Label ID = "Label_NU_ARTI" runat="server" BackColor="#002F60" ForeColor="White"></asp:Label>
                    <br />
                    <br />
                    <label>Scanner le n° de série :</label>
                    <asp:TextBox ID="TextBox_NU_SER" runat="server" AutoPostBack="True" Width="128px"></asp:TextBox>
                </div>
                    <br />
                    <asp:Label ID = "Label_CHECK_WF" runat="server" ForeColor="red"></asp:Label>
                    <br />
                <div class="button_raz">
                    <asp:Button ID="Button_RAZ" runat="server" Text="RAZ" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Height="40px" Width="90px" PostBackUrl="~/PagesMembres/Provisoire/SENSING.aspx"/>
                </div>
          </asp:View>
            <asp:View ID="View_SAVE_TO_DB" runat="server">
                <div class="tab_recap">
                    <table style="border: solid black 2px">
                        <caption><label>RECAPITULATIF</label></caption>
                        <tr>
                            <td><label>OF :</label></td><td><asp:Label ID = "Label_OF" runat="server"></asp:Label></td></tr>
                        <tr>
                            <td><label>OP en cours :</label></td><td><asp:Label ID = "Label_OP" runat="server"></asp:Label></td></tr>
                        <tr>
                            <td><label>Article :</label></td><td><asp:Label ID = "Label_ART" runat="server"></asp:Label></td></tr>
                        <tr>
                            <td><label>N° série :</label></td><td><asp:Label ID = "Label_NU_SER" runat="server"></asp:Label></td></tr>
                        <tr>
                            <td><label>DEVEUI :</label></td><td><asp:Label ID = "Label_DEVEUI" runat="server"></asp:Label></td></tr>
                    </table>
                <br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                     <asp:Button ID="Button_RAZ2" runat="server" Text="RAZ" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Height="40px" Width="90px" PostBackUrl="~/PagesMembres/Provisoire/SENSING.aspx"/>
                </div>
                <br />
                <div class="saving">
                    <label>Validation du passage test ?</label>
                    <br />
                    <br />
                    <asp:Button ID="Button_OK" runat="server" Text="Succès ☑" BackColor="#28BF4A" Font-Bold="True" ForeColor="White" Height="40px" Width="180px"/>      
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="Button_NON" runat="server" Text="Echec ☒" BackColor="#D04063" Font-Bold="True" ForeColor="White" Height="40px" Width="180px"/>
                </div>
                <br />
            </asp:View>
        </asp:MultiView>
</asp:Content>