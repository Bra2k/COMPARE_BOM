<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="IDCT_PASS.aspx.vb" Inherits="App_Web.IDCT_PASS" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <br />
        <table style="width:100%;">
            <tr>
                <td><label>Sélectionner une date de début :</label><br />
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
                <td><label>Sélectionner une date de fin :</label><br />
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
            <tr>
                <td><br /><label>Entrer l&#39;OF (Optionnel) :</label>
                    <asp:TextBox ID="TextBox_OF" runat="server"></asp:TextBox>
                </td>
                <td><br /><label>Entrer le code article (optionnel) :</label>
                    <asp:TextBox ID="TextBox_CD_ARTI_ECO" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" rowspan="4">
                    <br />
        <asp:Button ID="Button_FILT" runat="server" Text="Valider" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Height="40px" Width="180px" />
                    <br />
        <asp:GridView ID="GridView_RES" runat="server" AllowSorting="True" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical">
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
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
        </table>
    <p>
        <asp:Label ID="Label_DEB" runat="server" Visible="False"></asp:Label>
    </p>
    <p>
        <asp:Label ID="Label_FIN" runat="server" Visible="False"></asp:Label>
        <br />
    </p>
    <p>
        &nbsp;</p>
    <p>
    </p>
</asp:Content>
