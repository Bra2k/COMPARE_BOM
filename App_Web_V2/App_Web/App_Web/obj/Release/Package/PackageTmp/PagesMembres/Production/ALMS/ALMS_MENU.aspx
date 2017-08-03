<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ALMS_MENU.aspx.vb" Inherits="App_Web._ALMS_MENU" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="tile-container">
        <a class="tile" data-role="tile" data-effect="slideUpDown" href="/PagesMembres/Production/ALMS/ADD_MAC_ADRE.aspx" style="background-color: #002F60;">
            <div class="tile-content slide-up-2">
                <div class="slide-over text-small" style="background-color: #002F60; text-align: center; color: #FFFFFF;">
                    Entrer des adresses MAC dans la base de données<br />
                    <br />
                </div>
                <span class="tile-label bold text-bold" style="color: #FFFFFF;">Entrer des adresses MAC</span>
            </div>
        </a>
        <a class="tile" data-role="tile" data-effect="slideUpDown" href="/PagesMembres/Production/ALMS/ALMS_IMPORT.aspx" style="background-color: #002F60;">
            <div class="tile-content slide-up-2">
                <div class="slide-over text-small" style="background-color: #002F60; text-align: center; color: #FFFFFF;">
                    Importer des séquenceur de test dans la base de données<br />
                    <br />
                </div>
                <span class="tile-label bold text-bold" style="color: #FFFFFF;">Import séquenceur de test</span>
            </div>
        </a>
        <a class="tile" data-role="tile" data-effect="slideUpDown" href="/PagesMembres/Production/ALMS/GEST_ANML_NC_DRGT.aspx" style="background-color: #002F60;">
            <div class="tile-content slide-up-2">
                <div class="slide-over text-small" style="background-color: #002F60; text-align: center; color: #FFFFFF;">
                    Gérer les anomalies<br />
                    <br />
                </div>
                <span class="tile-label bold text-bold" style="color: #FFFFFF;">Gestion Anomalie</span>
            </div>
        </a>
        <a class="tile" data-role="tile" data-effect="slideUpDown" href="/PagesMembres/Production/ALMS/AMNS_DROI_ALMS.aspx" style="background-color: #002F60;">
            <div class="tile-content slide-up-2">
                <div class="slide-over text-small" style="background-color: #002F60; text-align: center; color: #FFFFFF;">
                    Administration des droiots d'accès et habilitation des opérateurs<br />
                    <br />
                </div>
                <span class="tile-label bold text-bold" style="color: #FFFFFF;">Administration Habilitation</span>
            </div>
        </a>
        <a class="tile" data-role="tile" data-effect="slideUpDown" href="/PagesMembres/Production/ALMS/IPSO_ETQT_NU_SER.aspx" style="background-color: #002F60;">
            <div class="tile-content slide-up-2">
                <div class="slide-over text-small" style="background-color: #002F60; text-align: center; color: #FFFFFF;">
                    Imprimer des étiquettes numéros de série<br />
                    <br />
                </div>
                <span class="tile-label bold text-bold" style="color: #FFFFFF;">Imprimer des étiquettes numéros de série</span>
            </div>
        </a>
    </div>

</asp:Content>
