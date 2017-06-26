<%@ Page Title="Gérer le compte" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Manage.aspx.vb" Inherits="App_Web.Manage" %>

<%@ Import Namespace="App_Web" %>
<%@ Import Namespace="Microsoft.AspNet.Identity" %>
<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>

    <div>
        <asp:PlaceHolder runat="server" ID="SuccessMessagePlaceHolder" Visible="false" ViewStateMode="Disabled">
            <p class="text-success"><%: SuccessMessage %></p>
        </asp:PlaceHolder>
    </div>

    <div class="row">
        <div class="col-md-12">
            <div class="form-horizontal">
                <h4>Modifier vos paramètres de compte</h4>
                <hr />
                <dl class="dl-horizontal">
                    <dt>Mot de passe :</dt>
                    <dd>
                        <asp:HyperLink NavigateUrl="/Account/ManagePassword" Text="[Changer]" Visible="false" ID="ChangePassword" runat="server" />
                        <asp:HyperLink NavigateUrl="/Account/ManagePassword" Text="[Créer]" Visible="false" ID="CreatePassword" runat="server" />
                    </dd>
                    <dt>Connexions externes :</dt>
                    <dd><%:LoginsCount %>
                        <asp:HyperLink NavigateUrl="/Account/ManageLogins" Text="[Gérer]" runat="server" />

                    </dd>
                    <%--
                        Les numéros de téléphone peuvent être utilisés comme second facteur de vérification dans un système d’authentification à 2 facteurs.
                        Voir <a href="http://go.microsoft.com/fwlink/?LinkId=403804">cet article</a>
                        pour des détails sur la configuration de cette application ASP.NET pour prendre en charge l’utilisation de l’authentification à 2 facteurs à l’aide de SMS.
                        Décommentez les blocs suivants après avoir configuré l’authentification à 2 facteurs
                    --%>
                    <%--
                    <dt>Numéro de téléphone :</dt>
                     --%>
                    <% If HasPhoneNumber Then %>
                    <%--
                    <dd>
                        <asp:HyperLink NavigateUrl="/Account/AddPhoneNumber" runat="server" Text="[Ajouter]" />
                    </dd>
                    --%>
                    <% Else %>
                    <%--
                    <dd>
                    
                        <asp:Label Text="" ID="PhoneNumber" runat="server" />
                        <asp:HyperLink NavigateUrl="/Account/AddPhoneNumber" runat="server" Text="[Changer]" /> &nbsp;|&nbsp;
                        <asp:LinkButton Text="[Supprimer]" OnClick="RemovePhone_Click" runat="server" />
                    </dd>
                    --%>
                    <% End If %>
                    <dt>Authentification à 2 facteurs :</dt>
                    <dd>
                        <p>
                            Aucun fournisseur d’authentification à 2 facteurs n’est configuré. Voir <a href="http://go.microsoft.com/fwlink/?LinkId=403804">cet article</a>
                            pour des détails sur la configuration de cette application ASP.NET pour prendre en charge l’authentification à 2 facteurs.
                        </p>                        
                        <% If TwoFactorEnabled Then %>
                        <%--
                        Activé
                        <asp:LinkButton Text="[Désactiver]" runat="server" CommandArgument="false" OnClick="TwoFactorDisable_Click" />
                        --%>
                        <% Else %>
                       	<%--
                       	Désactivé
                        <asp:LinkButton Text="[Activer]" CommandArgument="true" OnClick="TwoFactorEnable_Click" runat="server" />
                        --%>
                        <% End If %>
                    </dd>
                </dl>
            </div>
        </div>
    </div>
</asp:Content>

