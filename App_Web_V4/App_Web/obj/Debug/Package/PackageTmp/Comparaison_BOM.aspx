<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Comparaison_BOM.aspx.vb" Inherits="App_Web.Comparaison_BOM" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    Sélectionner le nom du client :<br />
    <asp:DropDownList ID="DropDownList_Client" runat="server">
    </asp:DropDownList>
    <br />
<br />
Sélectionner l&#39;article :
    <br />
    <asp:DropDownList ID="DropDownList1" runat="server">
</asp:DropDownList>
<br />
    <br />
    Sélectionner le fichier BOM à contrôler :<asp:FileUpload ID="FileUpload_BOM" runat="server" />
    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    <br />
    <asp:Button ID="Button_Importer" runat="server" Text="Importer" />
    <br />
    <asp:GridView ID="GridView_Matricules" runat="server">
        <Columns>
            
        </Columns>
    </asp:GridView>
<asp:GridView ID="GridView_Nomenclature" runat="server">
</asp:GridView>
</asp:Content>
