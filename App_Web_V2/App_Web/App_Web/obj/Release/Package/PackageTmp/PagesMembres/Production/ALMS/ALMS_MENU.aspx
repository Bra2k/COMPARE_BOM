<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ALMS_MENU.aspx.vb" Inherits="App_Web._ALMS_MENU" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id="tabeau" class="row">
        <div class="col-md-4">
            <a href="ADD_MAC_ADRE.aspx" class="btn btn-default"><img src="/App_Themes/ICON_COMPARE/computer3.ico"/>Entrer des adresses MAC  
</a>
        </div>
        <div class="col-md-4">
            <a href="ALMS_IMPORT" class="btn btn-primary">Importer un séquenceur de test</a>
        </div>
        <div class="col-md-4">
            <a href="GEST_ANML_NC_DRGT" class="btn btn-default">
            Gérer les anomalies</a>
        </div>
    </div>




</asp:Content>
