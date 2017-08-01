<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="App_Web._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <%--    <link href="App_Themes/CSS/CSS_MediaQuery.css" rel="stylesheet" type="text/css" />
    <link href="Content/metro.css" rel="stylesheet" />--%>
    <%--    <script type="text/javascript" src="Scripts/snow.js"></script>--%>
    <%--   <script type="text/javascript">
        var cdate = new Date();
        var cmois = cdate.getMonth() + 1;
        if (cmois == '12' || cmois == '1' || cmois == '2') {
            window.onload = function () {
                snow.init(80);
            };
        };
    </script>--%>
    <%--    <script src="Scripts/jquery-1.10.2.js"></script>
    <script src="Scripts/metro.js"></script>--%>
    <br />

    <%--    <div id="tableau" class="row">--%>
    <%--        <div class="col-md-3" style="background-color: #002F60; font-family: verdana; color: #FFFFFF;">
            <div class="img-thumbnail">
                <asp:Label ID="Label_PHOT" runat="server" Text=""></asp:Label>
            </div>
            <ol class="simple-list dark-bullet" style="background-color: #002F60; font-family: verdana; color: #FFFFFF;">
                <li>
                    <label>Bienvenue</label></li>
                <%: Session("displayname") %>
                <li>
                    <label>Titre :</label></li>
                <%: Session("title") %>
                <li>
                    <label>Matricule SAP :</label></li>
                <%: Session("matricule") %>
                <li>
                    <label>Service :</label></li>
                <%: Session("department") %>
            </ol>
        </div>
        <br />--%>
    <div id="menu" class="container-fluid centered-text">
        <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true" style="font-family: verdana">
            <div class="panel panel-default">
                <div class="panel-heading" role="tab" id="headingOne">
                    <h4 class="panel-title">
                        <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="false" aria-controls="collapseOne">Développement</a>
                    </h4>
                </div>
                <div id="collapseOne" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
                    <div class="panel-body tile-container">
                        <a class="tile" data-role="tile" data-effect="slideUpDown" href="/PagesMembres/Developpement/UPLO_FICH_TO_CAB.aspx" style="background-color: #002F60;">
                            <div class="tile-content slide-up-2">
                                <div class="slide-over text-small" style="background-color: #002F60; text-align: center; color: #FFFFFF;">
                                    Accéder à la copie de fichier (police, images) sur des imprimantes d'étiquettes de marque CAB<br />
                                    <br />
                                </div>
                                <span class="tile-label bold text-bold" style="color: #FFFFFF;">Copie Fichier CAB</span>
                            </div>
                        </a>
                        <a class="tile" data-role="tile" data-effect="slideUpDown" href="/PagesMembres/Developpement/JOUR_APP.aspx" style="background-color: #002F60;">
                            <div class="tile-content slide-up-2">
                                <div class="slide-over text-small" style="background-color: #002F60; text-align: center; color: #FFFFFF;">
                                    Accéder aux LOG (journaux) de l'application<br />
                                    <br />
                                    <img alt="Product_Box-03-WF" src="/App_Themes/Menu_Defaut/Log_file_icon_2.png" style="max-height: 50%;">
                                </div>
                                <span class="tile-label bold text-bold" style="color: #FFFFFF;">Journal Application</span>
                            </div>
                        </a>
                        <a class="tile" data-role="tile" data-effect="slideUpDown" href="/PagesMembres/Developpement/Fonctions_Class_SAP_DATA.aspx" style="background-color: #002F60;">
                            <div class="tile-content slide-up-2">
                                <div class="slide-over text-small" style="background-color: #002F60; text-align: center; color: #FFFFFF;">
                                    Utiliser les fonctions de la classe Class_SAP_DATA<br />
                                    <br />
                                    <img alt="Product_Box-03-WF" src="/App_Themes/Menu_Defaut/Software-SAP.png" style="max-height: 50%;">
                                </div>
                                <span class="tile-label bold text-bold" style="color: #FFFFFF;">Fonctions SAP</span>
                            </div>
                        </a>
                        <a class="tile" data-role="tile" data-effect="slideUpDown" href="/PagesMembres/Developpement/AMNS_APP_WEB.aspx" style="background-color: #002F60;">
                            <div class="tile-content slide-up-2">
                                <div class="slide-over text-small" style="background-color: #002F60; text-align: center; color: #FFFFFF;">
                                    Administration des accès aux applications<br />
                                    <br />
                                    <img alt="Product_Box-03-WF" src="/App_Themes/Menu_Defaut/Assign.png" style="max-height: 50%;">
                                </div>
                                <span class="tile-label bold text-bold" style="color: #FFFFFF;">Administration Application</span>
                            </div>
                        </a>
                        <a class="tile" data-role="tile" data-effect="slideUpDown" href="/PagesMembres/Developpement/ADD_VERS_APCT.aspx" style="background-color: #002F60;">
                            <div class="tile-content slide-up-2">
                                <div class="slide-over text-small" style="background-color: #002F60; text-align: center; color: #FFFFFF;">
                                    Accéder aux indices des différents applications<br />
                                    <br />
                                </div>
                                <span class="tile-label bold text-bold" style="color: #FFFFFF;">Indice Application</span>
                            </div>
                        </a>
                        <a class="tile" data-role="tile" data-effect="slideUpDown" href="/PagesMembres/Developpement/Page_ESSAI.aspx" style="background-color: #002F60;">
                            <div class="tile-content slide-up-2">
                                <div class="slide-over text-small" style="background-color: #002F60; text-align: center; color: #FFFFFF;">
                                    Accéder aux essais de fonctionnalité de l'application<br />
                                    <br />
                                    <img alt="Product_Box-03-WF" src="/App_Themes/Menu_Defaut/Windmill-01.png" style="max-height: 50%;">
                                </div>
                                <span class="tile-label bold text-bold" style="color: #FFFFFF;">Essais Application</span>
                            </div>
                        </a>
                        <a class="tile" data-role="tile" data-effect="slideUpDown" href="/PagesMembres/Developpement/CFGR_POST.aspx" style="background-color: #002F60;">
                            <div class="tile-content slide-up-2">
                                <div class="slide-over text-small" style="background-color: #002F60; text-align: center; color: #FFFFFF;">
                                    Accéder à la configuration des postes pour les applications<br />
                                    <br />
                                    <img alt="Product_Box-03-WF" src="/App_Themes/Menu_Defaut/Computer-IT-01-WF.png" style="max-height: 50%;">
                                </div>
                                <span class="tile-label bold text-bold" style="color: #FFFFFF;">Configuration Application</span>
                            </div>
                        </a>
                        <a class="tile" data-role="tile" data-effect="slideUpDown" href="https://barcode.tec-it.com/en" style="background-color: #002F60;">
                            <div class="tile-content slide-up-2">
                                <div class="slide-over text-small" style="background-color: #002F60; text-align: center; color: #FFFFFF;">
                                    Générer un code à barre, datamatrix, QR code, etc... en image<br />
                                    <br />
                                </div>
                                <span class="tile-label bold text-bold" style="color: #FFFFFF;">Générer Code à barre</span>
                            </div>
                        </a> 
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
                    <div class="panel-body tile-container" style="color: #FFFFFF">
                        <a class="tile" data-role="tile" data-effect="slideUpDown" href="/PagesMembres/Digital_Factory/CLSg.aspx" style="background-color: #002F60;">
                            <div class="tile-content slide-up-2">
                                <div class="live-slide">
                                    <img src="/App_Themes/Menu_Defaut/box-palette-2.png" data-role="fitImage" data-format="fill">
                                </div>
                                <div class="live-slide">
                                    <img src="/App_Themes/Menu_Defaut/colis.jpg" data-role="fitImage" data-format="fill">
                                </div>
                                <div class="slide-over text-small" style="background-color: #002F60; text-align: center; color: #FFFFFF;">
                                    Tracer les produits / cartes envoyés<br />
                                    <br />
                                    <img alt="Product_Box-03-WF" src="/App_Themes/Menu_Defaut/Product_Box-03-WF.png" style="max-height: 50%;">
                                </div>
                                <span class="tile-label bold text-bold" style="color: #FFFFFF;">Colisage / Palletisation</span>
                            </div>
                        </a>
                        <a class="tile" data-role="tile" data-effect="slideUpDown" href="/PagesMembres/Digital_Factory/CONF_ARTI.aspx" style="background-color: #002F60;">
                            <div class="tile-content slide-up-2">
                                <div class="slide-over text-small" style="background-color: #002F60; text-align: center; color: #FFFFFF;">
                                    Accéder à la configuration d'un article dans la base digital factory<br />
                                    <br />
                                    <img alt="Product_Box-03-WF" src="/App_Themes/Menu_Defaut/Setting_2-WF.png" style="max-height: 50%;">
                                </div>
                                <span class="tile-label bold text-bold" style="color: #FFFFFF;">Configuration Article</span>
                            </div>
                        </a>
                        <a class="tile" data-role="tile" data-effect="slideUpDown" href="/PagesMembres/Digital_Factory/CTTO_POST.aspx" style="background-color: #002F60;">
                            <div class="tile-content slide-up-2">
                                <div class="slide-over text-small" style="background-color: #002F60; text-align: center; color: #FFFFFF;">
                                    Accéder à la configuration du ou des matériels d'un poste donné<br />
                                    <br />
                                    <img alt="Product_Box-03-WF" src="/App_Themes/Menu_Defaut/Student-Laptop.png" style="max-height: 50%;">
                                </div>
                                <span class="tile-label bold text-bold" style="color: #FFFFFF;">Constitution Postes</span>
                            </div>
                        </a>
                        <a class="tile" data-role="tile" data-effect="slideUpDown" href="/PagesMembres/Digital_Factory/IDCT_PASS.aspx" style="background-color: #002F60;">
                            <div class="tile-content slide-up-2">
                                <div class="slide-over text-small" style="background-color: #002F60; text-align: center; color: #FFFFFF;">
                                    Accéder aux indicateurs de passage<br />
                                    <br />
                                    <img alt="Product_Box-03-WF" src="/App_Themes/Menu_Defaut/Graph-03.png" style="max-height: 50%;">
                                </div>
                                <span class="tile-label bold text-bold" style="color: #FFFFFF;">Indicateur Passage</span>
                            </div>
                        </a>
                        <a class="tile" data-role="tile" data-effect="slideUpDown" href="/PagesMembres/Digital_Factory/IMPR_ETIQ_PRN.aspx" style="background-color: #002F60;">
                            <div class="tile-content slide-up-2">
                                <div class="slide-over text-small" style="background-color: #002F60; text-align: center; color: #FFFFFF;">
                                    Imprimer une étiquette<br />
                                    <br />
                                    <img alt="Product_Box-03-WF" src="/App_Themes/Menu_Defaut/Tag-WF.png" style="max-height: 50%;">
                                </div>
                                <span class="tile-label bold text-bold" style="color: #FFFFFF;">Impression Etiquette</span>
                            </div>
                        </a>
                        <a class="tile" data-role="tile" data-effect="slideUpDown" href="/PagesMembres/Digital_Factory/TCBL_OPRT.aspx" style="background-color: #002F60;">
                            <div class="tile-content slide-up-2">
                                <div class="slide-over text-small" style="background-color: #002F60; text-align: center; color: #FFFFFF;">
                                    Accéder à la page de traçabilité des opérations<br />
                                    <br />
                                    <img alt="Product_Box-03-WF" src="/App_Themes/Menu_Defaut/Operation-WF.png" style="max-height: 50%;">
                                </div>
                                <span class="tile-label bold text-bold" style="color: #FFFFFF;">Traçabilité Opération</span>
                            </div>
                        </a>
                        <a class="col-md-3 tile" data-role="tile" data-effect="slideUpDown" href="/PagesMembres/Digital_Factory/TCBL_COMP.aspx" style="background-color: #002F60;">
                            <div class="tile-content slide-up-2">
                                <div class="live-slide">
                                    <img src="/App_Themes/Menu_Defaut/63a.jpg" data-role="fitImage" data-format="fill">
                                </div>
                                <div class="live-slide">
                                    <img src="/App_Themes/Menu_Defaut/electronique-02-1.jpg" data-role="fitImage" data-format="fill">
                                </div>
                                <div class="live-slide">
                                    <img src="/App_Themes/Menu_Defaut/proto-process-success-story.jpg" data-role="fitImage" data-format="fill">
                                </div>
                                <div class="slide-over text-small" style="background-color: #002F60; text-align: center; color: #FFFFFF;">
                                    Tracer les composants<br />
                                    <br />
                                    <img alt="Product_Box-03-WF" src="/App_Themes/Menu_Defaut/CPU_1-WF.png" style="max-height: 50%;">
                                </div>
                                <span class="tile-label bold text-bold" style="color: #FFFFFF;">Traçabilité Composant</span>
                            </div>
                        </a>
                        <a class="tile" data-role="tile" data-effect="slideUpDown" href="/PagesMembres/Digital_Factory/TCBL_ESB_SS_ESB_V2.aspx" style="background-color: #002F60;">
                            <div class="tile-content slide-up-2">
                                <div class="slide-over text-small" style="background-color: #002F60; text-align: center; color: #FFFFFF;">
                                    Accéder à la traçabilité des ensembles / sous-ensembles<br />
                                    <br />
                                </div>
                                <span class="tile-label bold text-bold" style="color: #FFFFFF;">Traçabilité sous-ensemble</span>
                            </div>
                        </a>
                        <a class="tile" data-role="tile" data-effect="slideUpDown" href="/PagesMembres/Digital_Factory/IPTT_ADRE_MAC.aspx" style="background-color: #002F60;">
                            <div class="tile-content slide-up-2">
                                <div class="slide-over text-small" style="background-color: #002F60; text-align: center; color: #FFFFFF;">
                                    Accéder à l'ajout des adresses MAC dans la base<br />
                                    <br />
                                    <img alt="Product_Box-03-WF" src="/App_Themes/Menu_Defaut/News-Add.png" style="max-height: 50%;">
                                </div>
                                <span class="tile-label bold text-bold" style="color: #FFFFFF;">Importation adresse MAC</span>
                            </div>
                        </a>
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
                    <div class="panel-body tile-container">
                        <a class="tile" data-role="tile" data-effect="slideUpDown" href="/PagesMembres/Methodes/Comparaison_BOM.aspx" style="background-color: #002F60;">
                            <div class="tile-content slide-up-2">
                                <div class="slide-over text-small" style="background-color: #002F60; text-align: center; color: #FFFFFF;">
                                    Accéder à la page de l'application comparaison de BOM<br />
                                    <br />
                                </div>
                                <span class="tile-label bold text-bold" style="color: #FFFFFF;">Comparaison BOM</span>
                            </div>
                        </a>
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
                    <div class="panel-body tile-container">
                        <a class="tile" data-role="tile" data-effect="slideUpDown" href="/PagesMembres/Production/Test/CSTT_RESU_TEST_FONC.aspx" style="background-color: #002F60;">
                            <div class="tile-content slide-up-2">
                                <div class="slide-over text-small" style="background-color: #002F60; text-align: center; color: #FFFFFF;">
                                    Consulter les résultats du test fonctionnel<br />
                                    <br />
                                </div>
                                <span class="tile-label bold text-bold" style="color: #FFFFFF;">Consultation Test Fonctionnel</span>
                            </div>
                        </a>
                        <a class="tile" data-role="tile" data-effect="slideUpDown" href="/PagesMembres/Production/Test/CSTT_RESU_TEST_TAKA.aspx" style="background-color: #002F60;">
                            <div class="tile-content slide-up-2">
                                <div class="slide-over text-small" style="background-color: #002F60; text-align: center; color: #FFFFFF;">
                                    Consulter les résultats du test takaya<br />
                                    <br />
                                </div>
                                <span class="tile-label bold text-bold" style="color: #FFFFFF;">Consultation Test Takaya</span>
                            </div>
                        </a>
                        <a class="tile" data-role="tile" data-effect="slideUpDown" href="/PagesMembres/Provisoire/IMPR_RAPP_TCBL_STW_V2.aspx" style="background-color: #002F60;">
                            <div class="tile-content slide-up-2">
                                <div class="slide-over text-small" style="background-color: #002F60; text-align: center; color: #FFFFFF;">
                                    Imprimer un rapport STW<br />
                                    <br />
                                </div>
                                <span class="tile-label bold text-bold" style="color: #FFFFFF;">Rapport STW</span>
                            </div>
                        </a>
                        <a class="tile" data-role="tile" data-effect="slideUpDown" href="/PagesMembres/Production/Test/AJOU_EMPL_MAJ_FICH_BANC_TEST.aspx" style="background-color: #002F60;">
                            <div class="tile-content slide-up-2">
                                <div class="slide-over text-small" style="background-color: #002F60; text-align: center; color: #FFFFFF;">
                                    Ajouter un banc de test<br />
                                    <br />
                                </div>
                                <span class="tile-label bold text-bold" style="color: #FFFFFF;">Banc de test</span>
                            </div>
                        </a>                                               
                        <a class="tile" data-role="tile" data-effect="slideUpDown" href="/PagesMembres/Production/ALMS/ALMS_MENU.aspx" style="background-color: #002F60;">
                            <div class="tile-content slide-up-2">
                                <div class="slide-over text-small" style="background-color: #002F60; text-align: center; color: #FFFFFF;">
                                    Accéder à la page des applications dédiées à ALMS<br />
                                    <br />
                                    <img alt="Product_Box-03-WF" src="/App_Themes/Menu_Defaut/Heart-04.png" style="max-height: 50%;">
                                </div>
                                <span class="tile-label bold text-bold" style="color: #FFFFFF;">Applications ALMS</span>
                            </div>
                        </a>
                        <a class="tile" data-role="tile" data-effect="slideUpDown" href="/PagesMembres/Provisoire/SENSING.aspx" style="background-color: #002F60;">
                            <div class="tile-content slide-up-2">
                                <div class="slide-over text-small" style="background-color: #002F60; text-align: center; color: #FFFFFF;">
                                    Accéder à la page de l'application d'enregistrement des résultats de test Sensing LABS au niveau produit<br />
                                    <br />
                                    <img alt="Product_Box-03-WF" src="/App_Themes/Menu_Defaut/Wireless-03.png" style="max-height: 50%;">
                                </div>
                                <span class="tile-label bold text-bold" style="color: #FFFFFF;">Tests Sensing LABS</span>
                            </div>
                        </a>
                    </div>
                </div>
            </div>
            <div class="panel panel-default">
                <div class="panel-heading" role="tab" id="headingFive">
                    <h4 class="panel-title">
                        <a class="collapsed" role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseFive" aria-expanded="true" aria-controls="collapseTwo">Utilitaire externe</a>
                    </h4>
                </div>
                <div id="collapseFive" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingFive">
                    <div class="panel-body tile-container">
                        <a class="tile" data-role="tile" data-effect="slideUpDown" href="http://groupe-eolane.mailinblack.com" style="background-color: #002F60;">
                            <div class="tile-content slide-up-2">
                                <div class="slide-over text-small" style="background-color: #002F60; text-align: center; color: #FFFFFF;">
                                    Accéder à MailInBlack<br />
                                    <br />
                                </div>
                                <span class="tile-label bold text-bold" style="color: #FFFFFF;">MailInBlack</span>
                            </div>
                        </a>
                        <a class="tile" data-role="tile" data-effect="slideUpDown" href="http://10.100.13.17/Intranet/accueil.html" style="background-color: #002F60;">
                            <div class="tile-content slide-up-2">
                                <div class="slide-over text-small" style="background-color: #002F60; text-align: center; color: #FFFFFF;">
                                    Accéder à l'intranet Eolane Combrée<br />
                                    <br />
                                </div>
                                <span class="tile-label bold text-bold" style="color: #FFFFFF;">Intranet Eolane Combrée</span>
                            </div>
                        </a>
                        <a class="tile" data-role="tile" data-effect="slideUpDown" href="http://hello.eolane.com" style="background-color: #002F60;">
                            <div class="tile-content slide-up-2">
                                <div class="slide-over text-small" style="background-color: #002F60; text-align: center; color: #FFFFFF;">
                                    Accéder à Hello<br />
                                    <br />
                                </div>
                                <span class="tile-label bold text-bold" style="color: #FFFFFF;">Hello</span>
                            </div>
                        </a>
                        <a class="tile" data-role="tile" data-effect="slideUpDown" href="http://nqi/cpms/" style="background-color: #002F60;">
                            <div class="tile-content slide-up-2">
                                <div class="slide-over text-small" style="background-color: #002F60; text-align: center; color: #FFFFFF;">
                                    Accéder à NQI<br />
                                    <br />
                                </div>
                                <span class="tile-label bold text-bold" style="color: #FFFFFF;">NQI</span>
                            </div>
                        </a>                        
                        <a class="tile" data-role="tile" data-effect="slideUpDown" href="http://10.100.13.22" style="background-color: #002F60;">
                            <div class="tile-content slide-up-2">
                                <div class="slide-over text-small" style="background-color: #002F60; text-align: center; color: #FFFFFF;">
                                    Accéder à l'interface EASY<br />
                                    <br />
                                </div>
                                <span class="tile-label bold text-bold" style="color: #FFFFFF;">EASY</span>
                            </div>
                        </a>
                                               
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%--    </div>--%>
</asp:Content>
