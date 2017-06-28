<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" codebehind="IPTT_ADRE_MAC.aspx.vb" Inherits="App_Web.IPTT_ADRE_MAC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="App_Themes/CSS/CSS_MediaQuery.css" rel="stylesheet" type="text/css" />
    <br />
    <div class="titre_page">
        <h2>Digital Factory : Importation des adresses MAC</h2>
    </div>
    <br />
    <label>Saisir le critère d'entrée (OF, Code article, Nom du client) :</label>
    <asp:TextBox ID="TextBox_NM_CRIT" runat="server"></asp:TextBox>

    <br />

    <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
        <div class="panel panel-default">
            <div class="panel-heading" role="tab" id="headingOne">
                <h4 class="panel-title">
                    <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne">Par quantité</a>
                </h4>
            </div>
            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne">
                <div class="panel-body row">
                    <label>Importer</label>
                    <asp:TextBox ID="TextBox_QT" runat="server"></asp:TextBox>
                    &nbsp;<label>MAC adresse à partir de :</label>
                    <asp:TextBox ID="TextBox_PREM_MAC_ADRE" runat="server"></asp:TextBox>
                    &nbsp;<asp:Button ID="Button_QT" runat="server" Text="Valider" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Height="40px" Width="180px" />
                </div>
            </div>
        </div>
        <div class="panel panel-default">
            <div class="panel-heading" role="tab" id="headingTwo">
                <h4 class="panel-title">
                    <a class="collapsed" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">Par plage d'adresse</a>
                </h4>
            </div>
            <div id="collapseTwo" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingTwo">
                <div class="panel-body row">
                    <label>Importer la plage d&#39;adresse de :</label>
                    <asp:TextBox ID="TextBox_PREM_MAC_ADRE_2" runat="server"></asp:TextBox>
                    &nbsp;<label>à</label>&nbsp;<asp:TextBox ID="TextBox_DERN_MAC_ADRE" runat="server"></asp:TextBox>
                    &nbsp;<asp:Button ID="Button_PLAG_ADRE" runat="server" Text="Valider" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Height="40px" Width="180px" />
                </div>
            </div>
        </div>
        <div class="panel panel-default">
            <div class="panel-heading" role="tab" id="headingThree">
                <h4 class="panel-title">
                    <a class="collapsed" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseThree" aria-expanded="false" aria-controls="collapseThree">Par fichier</a>
                </h4>
            </div>
            <div id="collapseThree" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingThree">
                <div class="panel-body row">
                    <asp:MultiView ID="MultiView_FICH" runat="server" ActiveViewIndex="0">
                        <asp:View ID="View_UP_FICH" runat="server">
                            <label>Sélectionner le fichier à importer :</label>
                            <br />
                            <asp:FileUpload ID="FileUpload_FICH" runat="server" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" /><br />
                            <asp:Button ID="Button_UP_FICH" runat="server" Text="Valider" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Height="40px" Width="180px" />
                        </asp:View>
                        <asp:View ID="View_ONGL" runat="server">
                            <label>Sélectionner l&#39;onglet dans lequel se trouve les données :</label>
                            <asp:DropDownList ID="DropDownList_ONGL" runat="server"></asp:DropDownList>
                            &nbsp;<asp:Button ID="Button_ONGL" runat="server" Text="Valider" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Height="40px" Width="180px" />
                        </asp:View>
                        <asp:View ID="View_SEL_DONN" runat="server">
                            <label>Saisir le numéro de la colonne à importer :</label>
                            <asp:TextBox ID="TextBox_NU_COLO" runat="server"></asp:TextBox>
                            &nbsp;<asp:Button ID="Button_SAI_NU_COLO" runat="server" Text="Valider" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Height="40px" Width="180px" />
                            <asp:GridView ID="GridView_SEL_DONN" runat="server">
                            </asp:GridView>
                        </asp:View>
                        <asp:View ID="View_RESU" runat="server">
                        </asp:View>
                    </asp:MultiView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
