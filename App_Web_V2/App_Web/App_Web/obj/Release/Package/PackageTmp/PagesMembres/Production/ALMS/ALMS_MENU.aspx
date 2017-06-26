<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ALMS_MENU.aspx.vb" Inherits="App_Web.ALMS_MENU" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-4">
            <asp:HyperLink ID="HyperLinkADD_MAC_ADRE" runat="server" CssClass="btn btn-primary">Entrer des adresses MAC</asp:HyperLink>
        </div>
        <div class="col-md-4">
            <asp:HyperLink ID="HyperLinkALMS_IMPORT" runat="server" CssClass="btn btn-primary">Importer un séquenceur de test</asp:HyperLink>
        </div>
        <div class="col-md-4">
            <asp:HyperLink ID="HyperLinkGEST_ANML_NC_DRGT" runat="server" CssClass="btn btn-primary">Gérer les anomalies</asp:HyperLink>
        </div>

    </div>




</asp:Content>
