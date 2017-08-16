<%@ Page Title="Configurer un nouvel article pour le colisage" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="CONF_ARTI.aspx.vb" Inherits="App_Web.CONF_ARTI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <div class="titre_page">
        <h2>Digital Factory : Configuration des articles</h2>
    </div>
    <br />
    <div class="grid">
        <div class="row cells2">
            <div class="cell colspan5">
                <label>Entrer le code article :</label>
            </div>
            <div class="cell colspan7">
                <asp:TextBox ID="TextBox_CD_ARTI_ECO" runat="server" Width="450px" ToolTip="Code article Eolane" AutoPostBack="True" CausesValidation="True"></asp:TextBox>
            </div>
        </div>
    </div>
    <table class="tab_CHBX">
    </table>
    <br />

    <div class="accordion" data-role="accordion" data-close-any="true">
        <div class="frame active">
            <div class="heading">Général Digital Factory</div>
            <div class="content">
                <div class="grid">
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Code article client :</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_CD_ARTI_CLIE" runat="server" ToolTip="Dénomination du code article client" Width="450px"></asp:TextBox><br />
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Désignation client de l&#39;article :</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_NM_ARTI_CLIE" runat="server" ToolTip="Désignation client de l'article" Width="450px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="cell">
                            <asp:CheckBox ID="CheckBox_SAI_NU_SER_ECO" runat="server" Text="&nbsp;Saisie du numéro de série Eolane" ToolTip="Saisir le numéro de série Eolane au colisage" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="cell">
                            <asp:CheckBox ID="CheckBox_SAI_NU_SER_CLIE" runat="server" Text="&nbsp;Saisie du numéro de série Client" ToolTip="Saisir le numéro de série du client au colisage" />
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Format du code fournisseur :</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_FORM_CD_FNS" runat="server" Width="450px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Format du numéro de série Client :</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_FORM_NU_SER_CLIE" runat="server" TextMode="MultiLine" Width="450px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Format du numéro de série Eolane :</label>
                        </div>
                        <div class="cell colspan7" style="left: 0px; top: 0px">
                            <asp:TextBox ID="TextBox_FORM_NU_SER_ECO" runat="server" TextMode="MultiLine" Width="450px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Format du numéro d'UDI</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_FORM_UDI" runat="server" Width="450px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>GTIN</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_GTIN" runat="server" Width="450px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="cell">
                            <asp:CheckBox ID="CheckBox_FORM_NU_SER_ECO_OF6INC" runat="server" Text="Format numéro de série Eolane OF6INC" />
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Indice Client :</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_INDI_CLIE" runat="server" Width="450px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Nom du DDM :</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_NM_DDM" runat="server" Width="450px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Format du numéro de série fournisseur (sous-ensemble spécifique par exemple pipette STW) :</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_FORM_NU_SER_FNS" runat="server" Width="450px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Format du numéro de lot :</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_FORM_NU_LOT" runat="server" Width="450px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="cell">
                            <asp:CheckBox ID="CheckBox_SRLS_ARTI" runat="server" Checked="True" Text="Sérialisation de l'article" />
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Pourcentage de la quantité de l&#39;OF requis pour le présentation en recette client :</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_PCTG_RECE_CLIE" runat="server" Width="450px"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="frame">
            <div class="heading">Génération des numéros de série</div>
            <div class="content">
                <div class="grid">
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Encodage du numéro de série :</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_ENCO_NU_SER_CLIE" runat="server" Width="450px">10</asp:TextBox>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Incrémentation des numéros de série : </label>
                        </div>
                        <div class="cell colspan7" style="max-width: 100%">
                            <asp:TextBox ID="TextBox_INC_NU_SER" runat="server" Width="450px">1</asp:TextBox>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Format des numéros de série : </label>
                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_FORM_NU_SER_GNRT" runat="server" Width="450px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Critère de génération :</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:DropDownList ID="DropDownList_CRIT_GENE_NU_SER" runat="server" DataSourceID="SqlDataSource_CRIT_GENE_NU_SER" DataTextField="NM_CRIT" DataValueField="NM_CRIT" Width="450px">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource_CRIT_GENE_NU_SER" runat="server" ConnectionString="<%$ ConnectionStrings:APP_WEB_ECOConnectionString %>"
                                SelectCommand="SELECT [NM_CRIT]
                                         FROM [dbo].[DTM_REF_LIST_CRIT_GENE_NUM]"></asp:SqlDataSource>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Nombre d'étiquettes sur la largeur :</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_NB_ETQT_LARG" runat="server" Width="450px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Opération de la gamme à laquelle on génére impression numéro de série</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_GNRT_IPSO_NU_SER" runat="server" Width="450px"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="frame">
            <div class="heading">Colisage</div>
            <div class="content">
                <div class="grid">
                    <div class="row">
                        <div class="cell">
                            <asp:CheckBox ID="CheckBox_SAI_OF" runat="server" Text="&nbsp;Saisie de l'OF au colisage" ToolTip="Saisir l'OF au colisage" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="cell">
                            <asp:CheckBox ID="CheckBox_SAI_BL" runat="server" Text="&nbsp;Saisie du BL au colisage" ToolTip="Saisir le N° de BL au colisage (implique aussi une association à un numéro de palette)" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="cell">
                            <asp:CheckBox ID="CheckBox_SAI_NU_CART" runat="server" Text="&nbsp;Saisie du numéro de carton au colisage" ToolTip="Saisir le numéro du carton Eolane au colisage" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="cell">
                            <asp:CheckBox ID="CheckBox_ETIQ_PACK_NU_SER_ECO" runat="server" Text="&nbsp;Impression d'une étiquette packaging avec numéro de série Eolane" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="cell">
                            <asp:CheckBox ID="CheckBox_ETIQ_PACK_NU_SER_CLIE" runat="server" Text="&nbsp;Impression d'une étiquette packaging avec numéro de série client" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="cell">
                            <asp:CheckBox ID="CheckBox_SAI_OF_CART_TERM" runat="server" Text="&nbsp;Saisie de l'OF pour chaque carton terminé au colisage" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="cell">
                            <asp:CheckBox ID="CheckBox_MISE_DPST_DOCU" runat="server" Text="Mise à disposition des documents à la fin de la saisie BL/Palette (FCGF, DHR, etc...)" />
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Chemin réseau de la sauvegarde du fichier PDF :</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_NM_CHEM_FICH_PDF" runat="server" Width="450px" ToolTip="Chemin réseau de la sauvegarde du fichier PDF surtout utilisé pour le colisage" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="cell">
                            <asp:CheckBox ID="CheckBox_SUIV_NU_SER_ORDR_CROIS" runat="server" Text="Suivi des numéros de série dans l'ordre croissant" ToolTip="Suivi des numéros de série dans l'ordre croissant au colisage" />
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Quantité dans un carton :</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_QT_CART" runat="server" Width="450px">9999</asp:TextBox>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Quantité maximale de carton dans la palette :</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_QT_CART_PALE" runat="server" Width="450px">9999</asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="cell">
                            <asp:CheckBox ID="CheckBox_DOCU_CART_DF" runat="server" Text="Editer document Digital Factory au carton" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="cell">
                            <asp:CheckBox ID="CheckBox_DOCU_PALE_DF" runat="server" Text="Editer document Digital Factory à la palette" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="cell">
                            <asp:CheckBox ID="CheckBox_DOCU_BL_DF" runat="server" Text="Editer document Digital Factory au BL" />
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Requête liste produits dans le carton\palette du document Digital Factory</label>

                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_REQU_PROD_DOCU_DF" runat="server" Width="450px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Contenu du code à barre du document Digital Factory</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_CONT_CAB_DF" runat="server" Width="450px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Format du numéro de BL :</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_FORM_NU_BL" runat="server" Width="450px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Quantité dans un flanc :</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_QT_FLAN" runat="server" Width="450px">1</asp:TextBox>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Format du numéro de commande :</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_FORM_NU_CMD" runat="server" Width="450px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="cell">
                            <asp:CheckBox ID="CheckBox_SAI_NU_CART_SPEC" runat="server" Text="Saisie du numéro de carton spécifique" ToolTip="Saisir le numéro du carton spécifique au colisage" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="cell">
                            <asp:CheckBox ID="CheckBox_SAI_CD_FNS" runat="server" Text="Saisie du code fournisseur" ToolTip="Saisir le code fournisseur au colisage" />
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Format du code carton :</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_FORM_CD_CART" runat="server" Width="450px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Format de l&#39;étiquette carton :</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_FORM_ETIQ_CART" runat="server" Width="450px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Nombre de ligne dans le fichier PDF :</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_NB_LIGN_FICH_PDF" runat="server" ToolTip="Nombre maximal de ligne dans le fichier PDF (surtout utilisé pour le colisage)" Width="450px"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="frame">
            <div class="heading">Traçabilité des opérations</div>
            <div class="content">
                <div class="grid">
                    <div class="row">
                        <div class="cell">
                            <asp:CheckBox ID="CheckBox_SCAN_NU_SER_FIN_OPRT" runat="server" Text="Scan numéro de série fin opération" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="cell">
                            <asp:CheckBox ID="CheckBox_VRFC_CHRC_OF_NU_SER_CLIE" runat="server" Text="Vérification cohérence OF par numéro de série" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="cell">
                            <asp:CheckBox ID="CheckBox_VRFC_CHRC_OF_NU_SER_ECO" runat="server" Text="Vérification cohérence OF numero serie eolane" />
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>WORKFLOW étape 1</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:DropDownList ID="DropDownList_WF_ETAP_1" runat="server" Width="450px"></asp:DropDownList>
                            <%--<asp:TextBox ID="TextBox_WF_ETAP_1" runat="server" Width="450px"></asp:TextBox>--%>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>WORKFLOW étape 2</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:DropDownList ID="DropDownList_WF_ETAP_2" runat="server" Width="450px"></asp:DropDownList>
                            <%--<asp:TextBox ID="TextBox_WF_ETAP_2" runat="server" Width="450px"></asp:TextBox>--%>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>WORKFLOW étape 3</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:DropDownList ID="DropDownList_WF_ETAP_3" runat="server" Width="450px"></asp:DropDownList>
                            <%--<asp:TextBox ID="TextBox_WF_ETAP_3" runat="server" Width="450px"></asp:TextBox>--%>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>WORKFLOW étape 4</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:DropDownList ID="DropDownList_WF_ETAP_4" runat="server" Width="450px"></asp:DropDownList>
                            <%--<asp:TextBox ID="TextBox_WF_ETAP_4" runat="server" Width="450px"></asp:TextBox>--%>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>WORKFLOW étape 5</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:DropDownList ID="DropDownList_WF_ETAP_5" runat="server" Width="450px"></asp:DropDownList>
                            <%--<asp:TextBox ID="TextBox_WF_ETAP_5" runat="server" Width="450px"></asp:TextBox>--%>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>WORKFLOW étape 6</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:DropDownList ID="DropDownList_WF_ETAP_6" runat="server" Width="450px"></asp:DropDownList>
                            <%--<asp:TextBox ID="TextBox_WF_ETAP_6" runat="server" Width="450px"></asp:TextBox>--%>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>WORKFLOW étape 7</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:DropDownList ID="DropDownList_WF_ETAP_7" runat="server" Width="450px"></asp:DropDownList>
                            <%--<asp:TextBox ID="TextBox_WF_ETAP_7" runat="server" Width="450px"></asp:TextBox>--%>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>WORKFLOW étape 8</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:DropDownList ID="DropDownList_WF_ETAP_8" runat="server" Width="450px"></asp:DropDownList>
                            <%--<asp:TextBox ID="TextBox_WF_ETAP_8" runat="server" Width="450px"></asp:TextBox>--%>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>WORKFLOW étape 9</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:DropDownList ID="DropDownList_WF_ETAP_9" runat="server" Width="450px"></asp:DropDownList>
                            <%--<asp:TextBox ID="TextBox_WF_ETAP_9" runat="server" Width="450px"></asp:TextBox>--%>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>WORKFLOW étape 10</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:DropDownList ID="DropDownList_WF_ETAP_10" runat="server" Width="450px"></asp:DropDownList>
                            <%--<asp:TextBox ID="TextBox_WF_ETAP_10" runat="server" Width="450px"></asp:TextBox>--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="frame">
            <div class="heading">Traçabilité des composants</div>
            <div class="content">
                <div class="grid">
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Sous-ensemble n°1 associé :</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_SS_ENS_ASS_1" runat="server" Width="450px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Sous-ensemble n°2 associé :</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_SS_ENS_ASS_2" runat="server" Width="450px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Sous-ensemble n°3 associé :</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_SS_ENS_ASS_3" runat="server" Width="450px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Sous-ensemble n°4 associé :</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_SS_ENS_ASS_4" runat="server" Width="450px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Sous-ensemble n°5 associé :</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_SS_ENS_ASS_5" runat="server" Width="450px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Sous-ensemble n°6 associé :</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_SS_ENS_ASS_6" runat="server" Width="450px"></asp:TextBox>
                        </div>
                        <div class="row cells2">
                            <div class="cell colspan5">
                                <label>Chemin du fichier de rapport de traçabilité :</label>
                            </div>
                            <div class="cell colspan7">
                                <asp:TextBox ID="TextBox_CHEM_FICH_RAPP_TCBL" runat="server" Width="450px" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row cells2">
                            <div class="cell colspan5">
                                <label>Nom de la vue pour l&#39;extration du fichier PDF :</label>
                            </div>
                            <div class="cell colspan7">
                                <asp:TextBox ID="TextBox_VUE_FICH_PDF" runat="server" Width="450px"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row cells2">
                            <div class="cell colspan5">
                                <label>Nom de la vue de recherche du numéro d&#39;ensemble en fonction des numéros de sous-ensemble :</label>
                            </div>
                            <div class="cell colspan7">
                                <asp:TextBox ID="TextBox_VUE_NU_SER_ENSE_SOUS_ENSE" runat="server" Width="450px"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="frame">
            <div class="heading">Configuration d'envoi de données par mail</div>
            <div class="content">
                <div class="grid">
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Serveur SMTP pour l&#39;envoi de Mail :</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_MAIL_SMTP" runat="server" Width="450px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Destinataire de l&#39;envoi des mail :</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_DEST_MAIL" runat="server" Width="450px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Destinataire en copie de l&#39;envoi des mail :</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_DEST_COPY_MAIL" runat="server" Width="450px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Destinataire en copie cachée de l&#39;envoi des mail :</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_DEST_BLIND_COPY_MAIL" runat="server" Width="450px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Sujet des mail :</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_SUJE_MAIL" runat="server" Width="450px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row cells2">
                        <div class="cell colspan5">
                            <label>Contenu du mail :</label>
                        </div>
                        <div class="cell colspan7">
                            <asp:TextBox ID="TextBox_CONT_MAIL" runat="server" Width="450px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="cell">
                            <asp:CheckBox ID="CheckBox_MAIL_PJ" runat="server" Text="&nbsp;Mettre en pièce jointe des fichiers" />
                        </div>
                    </div>

                </div>
            </div>
        </div>
        <div class="frame">
            <div class="heading">Configuration des chemin des script d'étiquette</div>
            <div class="content">
                <div class="grid">
                </div>
                <div class="row cells2">
                    <div class="cell colspan5">
                        <label>Chemin réseau du fichier d&#39;impression d&#39;étiquette d&#39;un numéro de série Eolane :</label>
                    </div>
                    <div class="cell colspan7">
                        <asp:TextBox ID="TextBox_CHEM_FICH_IMPR_NU_SER_ECO" runat="server" Width="450px" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="row cells2">
                    <div class="cell colspan5">
                        <label>Chemin réseau du fichier d&#39;impression d&#39;étiquette d&#39;un numéro de série Client :</label>
                    </div>
                    <div class="cell colspan7">
                        <asp:TextBox ID="TextBox_CHEM_FICH_IMPR_NU_SER_CLIE" runat="server" Width="450px" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="row cells2">
                    <div class="cell colspan5">
                        <label>Chemin réseau du fichier d&#39;impression d&#39;étiquette d&#39;un carton:</label>
                    </div>
                    <div class="cell colspan7">
                        <asp:TextBox ID="TextBox_CHEM_FICH_IMPR_CART" runat="server" Width="450px" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="row cells2">
                    <div class="cell colspan5">
                        <label>Chemin réseau du fichier d&#39;impression d&#39;étiquette d&#39;une palette :</label>
                    </div>
                    <div class="cell colspan7">
                        <asp:TextBox ID="TextBox_CHEM_FICH_IMPR_PALE" runat="server" Width="450px" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <br />
    <br />
    <asp:Button ID="Button_VALI" runat="server" Text="Valider" BackColor="#002F60" Font-Bold="True" ForeColor="White" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Height="40px" Width="180px" />
    <br />
</asp:Content>
