<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="IDCT_GLOB_LIGN.aspx.vb" Inherits="App_Web.IDCT_GLOB_LIGN" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Digital Factory : Indicateur global de ligne</h2>
    <br />
    <table style="width:100%;">
            <tr>
                <td colspan="2"><label>Sélectionner une date de début :</label><br />
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
                <td>OU</td>
                <td><br /><label>Entrer le code article (optionnel) :</label>
                    <asp:TextBox ID="TextBox_CD_ARTI_ECO" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="3" rowspan="3">
                    <br />
                   <label> Objectif de production : </label><br />
                    <label> Tendance / semaine précédente : </label><br />
                    <label> Pannes à traiter </label><br />
                    <label> En réparation </label><br />
                    <label> En retest </label><br />
        <asp:Button ID="Button_FILT" runat="server" Text="Valider" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Height="40px" Width="180px" />
                    <br />
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
            </tr>
            </table>  <p>
        <asp:Label ID="Label_DEB" runat="server" Visible="False"></asp:Label>
    </p>
    <p>
        <asp:Label ID="Label_FIN" runat="server" Visible="False"></asp:Label>
        <br />
    </p>
        <br />
    <br />
</asp:Content>
