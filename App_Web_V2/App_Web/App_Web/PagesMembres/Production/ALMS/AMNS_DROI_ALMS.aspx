<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="AMNS_DROI_ALMS.aspx.vb" Inherits="App_Web.AMNS_DROI_ALMS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Administration des droits et des habilitations ALMS</h1>
    <div class="row">
        <div class="col-md-5">
            <label>Sélectionner le matricule de l'opérateur :</label>
        </div>
        <div class="col-md-3">
            <asp:DropDownList ID="DropDownList_OPRT" runat="server" DataSourceID="SqlDataSource_OPRT" DataTextField="ID_MTCL_ECO" DataValueField="ID_MTCL_ECO" AutoPostBack="True">
                <asp:ListItem></asp:ListItem>
            </asp:DropDownList><asp:SqlDataSource ID="SqlDataSource_OPRT" runat="server" ConnectionString="<%$ ConnectionStrings:ALMS_PROD_PRDConnectionString %>" SelectCommand="SELECT [ID_MTCL_ECO] FROM [DTM_REF_OPRT]"></asp:SqlDataSource>
        </div>
        <div class="col-md-3">
            <asp:Label ID="Label_NM_OPRT" runat="server" Text=""></asp:Label>
        </div>
        <div class="col-md-1">
            <asp:Button ID="Button_AJOU_OPRT" runat="server" Text="Ajouter" CssClass="btn-primary" />
        </div>
    </div>
    <asp:MultiView ID="MultiView_AMNS_DROI_ALMS" runat="server" ActiveViewIndex="0">
        <asp:View ID="View_MDFC_DROI" runat="server">
            <div class="row">
                <div class="col-md-5">
                    <label>Initiales : </label>
                </div>
                <div class="col-md-7">
                    <asp:Label ID="Label_INIT" runat="server" Text=""></asp:Label>
                </div>
                <div class="col-md-5">
                    <label>Habilitation : </label>
                </div>
                <div class="col-md-7">
                    <asp:TextBox ID="TextBox_HBLT" runat="server"></asp:TextBox>
                </div>
                <div class="col-md-5">
                    <label>Activité : </label>
                </div>
                <div class="col-md-7">
                    <asp:CheckBox ID="CheckBox_ATVT" runat="server" Text="Actif ?" />
                </div>
                <div class="col-md-12">
                    <asp:Button ID="Button_VALI" runat="server" Text="Valider" CssClass="btn-primary" />
                </div>
            </div>
        </asp:View>
        <asp:View ID="View_AJOU_OPRT" runat="server">
            <div class="row">
                <div class="col-md-5">
                    <label>Entrer le matricule : </label>
                </div>
                <div class="col-md-7">
                    <asp:TextBox ID="TextBox_MTCL" runat="server" AutoPostBack="True"></asp:TextBox>
                </div>
            </div>
        </asp:View>
    </asp:MultiView>


</asp:Content>
