﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ALMS_IMPORT.aspx.vb" Inherits="App_Web.ALMS_IMPORT" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="App_Themes/CSS/CSS_MediaQuery.css" rel="stylesheet" type="text/css" />
    <br />
    <asp:MultiView ID="MultiView_ALMS" runat="server" ActiveViewIndex="0">
        <asp:View ID="View_ALMS_DATA_ENTR" runat="server">
            <div class="titre_page">
                <h2>Production : ALMS</h2>
            </div>
            <br />
            <div class="ALMS_upload">
            <label>Séléctionner un fichier à importer :</label>
            <br />
            <asp:FileUpload ID="FileUpload_ALMS" runat="server" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" />
            </div>
            <br />
            <table style="width:100%;">
                <tr>
                    <td><label>Sélectionner une date de validation :</label><br />
                        <asp:Calendar ID="Calendar_DEB" runat="server" BackColor="White" BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="180px" Width="200px">
                        <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
                        <NextPrevStyle VerticalAlign="Bottom" />
                        <OtherMonthDayStyle ForeColor="#808080" />
                        <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                        <SelectorStyle BackColor="#CCCCCC" />
                        <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
                        <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                        <WeekendDayStyle BackColor="#FFFFCC" />
                        </asp:Calendar>        
                    </td>
                    <td><label>Sélectionner une date d'application :</label><br />
                        <asp:Calendar ID="Calendar_FIN" runat="server" BackColor="White" BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="180px" Width="200px">
                        <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
                        <NextPrevStyle VerticalAlign="Bottom" />
                        <OtherMonthDayStyle ForeColor="#808080" />
                        <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                        <SelectorStyle BackColor="#CCCCCC" />
                        <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
                        <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                        <WeekendDayStyle BackColor="#FFFFCC" />
                        </asp:Calendar>
                    </td>
                </tr>
            </table>
            <br />
            <div class="envoyer">
                <asp:Button ID="Button_ENVOYER" runat="server" Text="ENVOYER" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Height="40px" Width="180px"/>
            </div>
        </asp:View>
        <asp:View ID="GridView_ALMS" runat="server">
            <label>Aperçu du fichier CSV :</label>
                    <div  style="vertical-align:top;  width: 1198px; height: 539px; overflow: auto;">
                        <asp:GridView ID="GridView_CSV_ALMS" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" SkinID="GV_AFF_DONN" ShowFooter="True">
                        </asp:GridView>
                    </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>