<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="App_Web._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link href="App_Themes/CSS/CSS_MediaQuery.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Scripts/snow.js"></script>
    <script type="text/javascript">
        var cdate = new Date();
        var cmois = cdate.getMonth() + 1;
        if (cmois == '12' || cmois == '1' || cmois == '2') {
            window.onload = function () {
                snow.init(80);
            };
        };
    </script>
    <br />

    <div id="tableau" class="row">
        <div class="col-md-3">
            <div id="info" class="row">
                <div class="img-thumbnail col-xs-12 centered-text">
                    <asp:Label ID="Label_PHOT" runat="server" Text=""></asp:Label>
                </div>
                <div class="bg-info col-xs-4">
                    <label>Bienvenue</label>
                </div>
                <div class="col-xs-8">
                    <%: Session("displayname") %></>
                </div>
                <div class="col-xs-4">
                    <label>Titre :</label>
                </div>
                <div class="col-xs-8">
                    <%: Session("title") %>
                </div>
                <div class="col-xs-4">
                    <label>Mail :</label>
                </div>
                <div class="col-xs-8">
                    <%: Session("mail") %>
                </div>
                <div class="col-xs-4">
                    <label>Session :</label>
                </div>
                <div class="col-xs-8">
                    <%: Session("samaccountname") %>
                </div>
                <div class="col-xs-4">
                    <label>Nom :</label>
                </div>
                <div class="col-xs-8">
                    <%: Session("sn") %>
                </div>
                <div class="col-xs-4">
                    <label>Prénom :</label>
                </div>
                <div class="col-xs-8">
                    <%: Session("givenname") %>
                </div>
                <div class="col-md-4">
                    <label>Matricule SAP :</label>
                </div>
                <div class="col-md-8">
                    <%: Session("matricule") %>
                </div>
                <div class="col-xs-4">
                    <label>Service :</label>
                </div>
                <div class="col-xs-8">
                    <%: Session("department") %>
                </div>
            </div>
        </div>
        <div id="menu" class="col-md-9 container-fluid centered-text">
            <% If session("department") = "Informatique" Then %>

            <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
                <div class="panel panel-default">
                    <div class="panel-heading" role="tab" id="headingOne">
                        <h4 class="panel-title">
                            <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne">Développement</a>
                        </h4>
                    </div>
                    <div id="collapseOne" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
                        <div class="panel-body row">
                            <div class="col-md-3">
                                <a class="info" href="#">
                                    <asp:ImageButton ID="ImageButton4" ImageUrl="" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Developpement/UPLO_FICH_TO_CAB.aspx" /><span>Vers page Envoyer fichier sur CAB</span></a>
                            </div>
                            <div class="col-md-3">
                                <a class="info" href="#">
                                    <asp:ImageButton ID="ImageButton5" ImageUrl="~\App_Themes\ICON_COMPARE\log_diaries4.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Developpement/JOUR_APP.aspx" /><span>Vers page LOGS</span></a>
                            </div>
                            <div class="col-md-3">
                                <a class="info" href="#">
                                    <asp:ImageButton ID="ImageButton6" ImageUrl="~\App_Themes\ICON_COMPARE\serveur.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Developpement/Fonctions_Class_SAP_DATA.aspx" /><span>Vers page SAP</span></a>
                            </div>
                            <div class="col-md-3">
                                <a class="info" href="#">
                                    <asp:ImageButton ID="Admin" ImageUrl="~\App_Themes\ICON_COMPARE\admin.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Developpement/AMNS_APP_WEB.aspx" /><span>Vers page Administration</span></a>
                            </div>
                            <div class="col-md-3">
                                <a class="info" href="#">
                                    <asp:ImageButton ID="INDICE" ImageUrl="~\App_Themes\ICON_COMPARE\indice.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Developpement/ADD_VERS_APCT.aspx" /><span>Vers page APCT</span></a>
                            </div>
                            <div class="col-md-3">
                                <a class="info" href="#">
                                    <asp:ImageButton ID="Eolienne" ImageUrl="~\App_Themes\ICON_COMPARE\essais.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Developpement/Page_ESSAI.aspx" /><span>Vers page essai</span></a>
                            </div>
                            <div class="col-md-3">
                                <a class="info" href="#">
                                    <asp:ImageButton ID="FixedPc" ImageUrl="~\App_Themes\ICON_COMPARE/configuration2.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Developpement/CFGR_POST.aspx" /><span>Vers page configuration</span></a>
                            </div>
                            <div class="col-md-3">
                                <a class="info" href="#">
                                    <asp:ImageButton ID="Log" ImageUrl="~\App_Themes\ICON_COMPARE\log_diaries4.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Developpement/JOUR_APP.aspx" /><span>Vers page LOGS</span></a>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel panel-default">
                    <div class="panel-heading" role="tab" id="headingTwo">
                        <h4 class="panel-title">
                            <a class="collapsed" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="true" aria-controls="collapseTwo">Digital Factory</a>
                        </h4>
                    </div>
                    <div id="collapseTwo" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingTwo">
                        <div class="panel-body row">
                            <div class="col-md-3">
                                <a class="info" href="#">
                                    <asp:ImageButton ID="InBox" runat="server" ImageUrl="~/App_Themes/ICON_COMPARE/parcel2.ico" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Digital_factory/CLSg.aspx" /><span>Vers page colisage</span></a>
                            </div>

                            <div class="col-md-3">
                                <a class="info" href="#">
                                    <asp:ImageButton ID="ElectroniCard" ImageUrl="~\App_Themes\ICON_COMPARE\barcode3.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Digital_Factory/CONF_ARTI.aspx" /><span>Vers page articles</span></a>
                            </div>
                            <div class="col-md-3">
                                <a class="info" href="#">
                                    <asp:ImageButton ID="ImageButton3" ImageUrl="" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Digital_Factory/CTTO_POST.aspx" /><span>Vers page Constitution des postes</span></a>
                            </div>

                            <div class="col-md-3">
                                <a class="info" href="#">
                                    <asp:ImageButton ID="ImageButton7" ImageUrl="" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Digital_Factory/IDCT_GLOB_LIGN.aspx" /><span>Vers page Indicateur global de ligne</span></a>
                            </div>
                            <div class="col-md-3">
                                <a class="info" href="#">
                                    <asp:ImageButton ID="Indicateur" ImageUrl="~\App_Themes\ICON_COMPARE\indicateur2.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Digital_Factory/IDCT_PASS.aspx" /><span>Vers page indicateur</span></a>
                            </div>
                            <div class="col-md-3">
                                <a class="info" href="#">
                                    <asp:ImageButton ID="StickerPrinter" ImageUrl="~\App_Themes\ICON_COMPARE\etiquette3.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Digital_Factory/IMPR_ETIQ_PRN.aspx" /><span>Vers page étiquettes</span></a>
                            </div>
                            <div class="col-md-3">
                                <a class="info" href="#">
                                    <asp:ImageButton ID="ImageButton1" ImageUrl="~\App_Themes\ICON_COMPARE\tracabilite_ope.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Digital_Factory/TCBL_OPRT.aspx" /><span>Vers page traçabilité opération Digital Factory</span></a>
                            </div>
                            <div class="col-md-3">
                                <a class="info" href="#">
                                    <asp:ImageButton ID="Composant" ImageUrl="~\App_Themes\ICON_COMPARE\composant2.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Digital_Factory/TCBL_COMP.aspx" /><span>Vers page composants</span></a>
                            </div>
                            <div class="col-md-3">
                                <a class="info btn btn-default"  href="/PagesMembres/Digital_Factory/TCBL_ESB_SS_ESB_V2.aspx">
                                    <img src="/App_Themes/ICON_COMPARE/ensemble.ico"  style="max-width : 80%;"/>
                                    <br />Saisie ensemble / sous-ensemble
<%--                                    <span>Vers page ensembles</span>--%>
                                </a>
<%--                                    <asp:ImageButton ID="CodeBarre" ImageUrl="~\App_Themes\ICON_COMPARE\ensemble.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Digital_Factory/TCBL_ESB_SS_ESB_V2.aspx" /><span>Vers page ensembles</span></a>--%>
                            </div>
                            <div class="col-md-3">
                                <a class="info btn btn-default" href="/PagesMembres/Digital_Factory/IPTT_ADRE_MAC.aspx">
                                    <img src="/App_Themes/ICON_COMPARE/computer3.ico" style="max-width : 80%;"/>
                                    <br /><label>Importation des adresses MAC</label/>
                                    <%--<span>Vers page importation des adresses MAC</span>--%>
                                </a>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel panel-default">
                    <div class="panel-heading" role="tab" id="headingThree">
                        <h4 class="panel-title">
                            <a class="collapsed" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseThree" aria-expanded="false" aria-controls="collapseThree">Méthodes</a>
                        </h4>
                    </div>
                    <div id="collapseThree" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingThree">
                        <div class="panel-body row">
                            <div class="col-md-3">
                                <a class="info" href="#">
                                    <asp:ImageButton ID="Bom" ImageUrl="~\App_Themes\ICON_COMPARE\comparer_fichier2.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Methodes/Comparaison_BOM.aspx" /><span>Vers page compare BOM</span></a>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="panel panel-default">
                    <div class="panel-heading" role="tab" id="headingFour">
                        <h4 class="panel-title">
                            <a class="collapsed" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseFour" aria-expanded="false" aria-controls="collapseThree">Production</a>
                        </h4>
                    </div>
                    <div id="collapseFour" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFour">
                        <div class="panel-body row">
                            <div class="col-md-3">
                                <a class="info" href="#">
                                    <asp:ImageButton ID="FoncTest" ImageUrl="~\App_Themes\ICON_COMPARE\tester_fonc.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Production/Test/CSTT_RESU_TEST_FONC.aspx" /><span>Vers test fonctionnel</span></a>
                            </div>
                            <div class="col-md-3">
                                <a class="info" href="#">
                                    <asp:ImageButton ID="Takaya" ImageUrl="~\App_Themes\ICON_COMPARE\test2.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Production/Test/CSTT_RESU_TEST_TAKA.aspx" /><span>Vers page TAKAYA</span></a>
                            </div>

                            <div class="col-md-3">
                                <a class="info" href="#">
                                    <asp:ImageButton ID="Imprimeur" ImageUrl="~\App_Themes\ICON_COMPARE\imprimeur_stw.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Provisoire/IMPR_RAPP_TCBL_STW_V2.aspx" /><span>Vers page rapport STW</span></a>
                            </div>
                            <div class="col-md-3">
                                <a class="info" href="#">
                                    <asp:ImageButton ID="BancTest" ImageUrl="~\App_Themes\ICON_COMPARE\banc_test.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Production/Test/AJOU_EMPL_MAJ_FICH_BANC_TEST.aspx" /><span>Vers page banc de test</span></a>
                            </div>

                            <div class="col-md-3">
                                <a class="info" href="#">
                                    <asp:ImageButton ID="EASY" ImageUrl="~\App_Themes\ICON_COMPARE\easy.ico" runat="server" Height="100px" Width="100px" PostBackUrl="http://10.100.13.22/" /><span>Vers portail EASY</span></a>
                            </div>
                            <div class="col-md-3">
                                <a class="info" href="#">
                                    <asp:ImageButton ID="HELLO" ImageUrl="~\App_Themes\ICON_COMPARE\intranet.ico" runat="server" Height="100px" Width="100px" PostBackUrl="http://10.100.13.17/Intranet/accueil.html" /><span>Vers portail HELLO</span></a>
                            </div>
                            <div class="col-md-3">
                                <a class="info" href="#">
                                    <asp:ImageButton ID="ALMS_button" ImageUrl="~\App_Themes\ICON_COMPARE\injecter-seringue.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Production/ALMS/ALMS_MENU.aspx" /><span>Vers page ALMS</span></a>
                            </div>
                            <div class="col-md-3">
                                <a class="info" href="#">
                                    <asp:ImageButton ID="SENSING_button" ImageUrl="~\App_Themes\ICON_COMPARE\sensing.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Provisoire/SENSING.aspx" /><span>Vers page SENSING L.</span></a>
                            </div>

                        </div>
                    </div>
                </div>
            </div>

            <% Else %>
            <table class="tab_menu table">
                <tr>
                    <td style="width: 200px"><a class="info" href="#">
                        <asp:ImageButton ID="Parcel6" runat="server" ImageUrl="~/App_Themes/ICON_COMPARE/parcel2.ico" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Digital_factory/CLSg.aspx" /><span>Vers page colisage</span></a></td>
                    <td style="width: 200px"><a class="info" href="#"></a></td>
                    <%--                    <td style="width: 200px"><a class="info" href="#"><asp:ImageButton ID="Computer" ImageUrl="~\App_Themes\ICON_COMPARE/computer3.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Production/ALMS/ADD_MAC_ADRE.aspx" /><span>Vers page MAC adresses</span></a></td>--%>
                    <td style="width: 200px"><a class="info" href="#">
                        <asp:ImageButton ID="Test_Fonc" ImageUrl="~\App_Themes\ICON_COMPARE\tester_fonc.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Production/Test/CSTT_RESU_TEST_FONC.aspx" /><span>Vers test fonctionnel</span></a></td>
                    <td style="width: 200px"><a class="info" href="#">
                        <asp:ImageButton ID="Sticker3" ImageUrl="~\App_Themes\ICON_COMPARE\etiquette3.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Digital_Factory/IMPR_ETIQ_PRN.aspx" /><span>Vers page étiquettes</span></a></td>
                    <td style="width: 200px"><a class="info" href="#">
                        <asp:ImageButton ID="Compare_file2" ImageUrl="~\App_Themes\ICON_COMPARE\comparer_fichier2.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Methodes/Comparaison_BOM.aspx" /><span>Vers page compare BOM</span></a></td>
                </tr>
                <tr>
                    <td style="width: 200px"><a class="info" href="#">
                        <asp:ImageButton ID="test4" ImageUrl="~\App_Themes\ICON_COMPARE\test2.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Production/Test/CSTT_RESU_TEST_TAKA.aspx" /><span>Vers page TAKAYA</span></a></td>
                    <td style="width: 200px"><a class="info" href="#">
                        <asp:ImageButton ID="Indic2" ImageUrl="~\App_Themes\ICON_COMPARE\indicateur2.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Digital_Factory/IDCT_PASS.aspx" /><span>Vers page indicateur</span></a></td>
                    <td style="width: 200px"><a class="info" href="#">
                        <asp:ImageButton ID="Ensemble2" ImageUrl="~\App_Themes\ICON_COMPARE\ensemble.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Digital_Factory/TCBL_ESB_SS_ESB_V2.aspx" /><span>Vers page ensembles</span></a></td>
                    <td style="width: 200px"><a class="info" href="#">
                        <asp:ImageButton ID="Imp_STW2" ImageUrl="~\App_Themes\ICON_COMPARE\imprimeur_stw.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Provisoire/IMPR_RAPP_TCBL_STW_V2.aspx" /><span>Vers page rapport STW</span></a></td>
                    <td style="width: 200px"><a class="info" href="#">
                        <asp:ImageButton ID="Banc_Test" ImageUrl="~\App_Themes\ICON_COMPARE\banc_test.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Production/Test/AJOU_EMPL_MAJ_FICH_BANC_TEST.aspx" /><span>Vers page banc de test</span></a></td>
                </tr>
                <tr>
                    <td style="width: 200px"><a class="info" href="#">
                        <asp:ImageButton ID="Composant5" ImageUrl="~\App_Themes\ICON_COMPARE\composant2.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Digital_Factory/TCBL_COMP.aspx" /><span>Vers page composants</span></a></td>
                    <td style="width: 200px"><a class="info" href="#">
                        <asp:ImageButton ID="Battery3" ImageUrl="~\App_Themes\ICON_COMPARE\batterie2.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Provisoire/TEST_BATT_TUCK.aspx" /><span>Vers page batteries</span></a></td>
                    <td style="width: 200px"><a class="info" href="#">
                        <asp:ImageButton ID="Sensing2" ImageUrl="~\App_Themes\ICON_COMPARE\sensing.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Provisoire/SENSING.aspx" /><span>Vers page SENSING L.</span></a></td>
                    <td style="width: 200px"><a class="info" href="#">
                        <asp:ImageButton ID="ALMS2" ImageUrl="~\App_Themes\ICON_COMPARE\injecter-seringue.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Production/ALMS/ALMS_MENU.aspx" /><span>Vers page import ALMS</span></a></td>
                    <td style="width: 200px"><a class="info" href="#">
                        <asp:ImageButton ID="ImageButton_TCBL_OPRT" ImageUrl="~\App_Themes\ICON_COMPARE\tracabilite_ope.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Digital_Factory/TCBL_OPRT.aspx" /><span>Vers page traçabilité opération Digital Factory</span></a></td>
                </tr>
                <tr>
                    <td style="width: 200px"><a class="info" href="#">
                        <asp:ImageButton ID="ImageButton2" ImageUrl="~\App_Themes\ICON_COMPARE/configuration2.ico" runat="server" Height="100px" Width="100px" PostBackUrl="~/PagesMembres/Developpement/CFGR_POST.aspx" /><span>Vers page configuration</span></a></td>
                    <td style="width: 200px"><a class="info" href="#" /></td>
                    <td style="width: 200px"><a class="info" href="#" /></td>
                    <td style="width: 200px"><a class="info" href="#" /></td>
                    <td style="width: 200px"><a class="info" href="#" /></td>
                </tr>


            </table>
            <% End If %>
        </div>

    </div>
</asp:Content>
