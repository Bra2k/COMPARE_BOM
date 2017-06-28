<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ALMS_MENU.aspx.vb" Inherits="App_Web._ALMS_MENU" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id="tabeau" class="row">
        <div class="col-md-4">
            <button type="button" runat="server" Class="btn btn-primary"><img src="~/App_Themes/ICON_COMPARE/computer3.ico"/>Entrer des adresses MAC  
</button>
        </div>
        <div class="col-md-4">
            <button  type="button" runat="server" Class="btn btn-primary">Importer un séquenceur de test</button>
        </div>
        <div class="col-md-4">
            <button type="button" runat="server" Class="btn btn-primary">Gérer les anomalies</button>
        </div>
    </div>




</asp:Content>
