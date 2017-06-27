Imports Microsoft.AspNet.Identity
Imports System.DirectoryServices
Imports System.DirectoryServices.AccountManagement
Imports App_Web.Class_SAP_DATA
Imports App_Web.LOG
Imports System.Reflection.MethodBase

Public Class SiteMaster
    Inherits MasterPage
    Private Const AntiXsrfTokenKey As String = "__AntiXsrfToken"
    Private Const AntiXsrfUserNameKey As String = "__AntiXsrfUserName"
    Private _antiXsrfTokenValue As String

    Protected Sub Page_Init(sender As Object, e As EventArgs)
        ' Le code ci-dessous vous aide à vous protéger des attaques XSRF
        Dim requestCookie = Request.Cookies(AntiXsrfTokenKey)
        Dim requestCookieGuidValue As Guid
        If requestCookie IsNot Nothing AndAlso Guid.TryParse(requestCookie.Value, requestCookieGuidValue) Then
            ' Utiliser le jeton Anti-XSRF à partir du cookie
            _antiXsrfTokenValue = requestCookie.Value
            Page.ViewStateUserKey = _antiXsrfTokenValue
        Else
            ' Générer un nouveau jeton Anti-XSRF et l'enregistrer dans le cookie
            _antiXsrfTokenValue = Guid.NewGuid().ToString("N")
            Page.ViewStateUserKey = _antiXsrfTokenValue

            Dim responseCookie = New HttpCookie(AntiXsrfTokenKey) With {.HttpOnly = True, .Value = _antiXsrfTokenValue}
            If FormsAuthentication.RequireSSL AndAlso Request.IsSecureConnection Then
                responseCookie.Secure = True
            End If
            Response.Cookies.[Set](responseCookie)
        End If

        AddHandler Page.PreLoad, AddressOf master_Page_PreLoad
    End Sub

    Protected Sub master_Page_PreLoad(sender As Object, e As EventArgs)
        If Not IsPostBack Then
            ' Définir un jeton Anti-XSRF
            ViewState(AntiXsrfTokenKey) = Page.ViewStateUserKey
            ViewState(AntiXsrfUserNameKey) = If(Context.User.Identity.Name, [String].Empty)
        Else
            ' Valider le jeton Anti-XSRF
            If DirectCast(ViewState(AntiXsrfTokenKey), String) <> _antiXsrfTokenValue OrElse DirectCast(ViewState(AntiXsrfUserNameKey), String) <> (If(Context.User.Identity.Name, [String].Empty)) Then
                Throw New InvalidOperationException("Échec de la validation du jeton Anti-XSRF.")
            End If
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack And Session("displayname") = "" Then
            Try
                Session("User_Name") = Replace(Replace(System.Web.HttpContext.Current.User.Identity.Name, Environment.UserDomainName, ""), "\", "")
                _AD_GET_USER(IdentityType.SamAccountName, Session("User_Name"))
            Catch ex As Exception
                LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
                Exit Sub
            Finally
                If Session("matricule") = "" Then MultiView_Master.SetActiveView(View_LOG_SAP)
            End Try
        End If
    End Sub

    Protected Sub Unnamed_LoggingOut(sender As Object, e As LoginCancelEventArgs)
        Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie)
    End Sub
    Protected Sub APP_WEB_LoggedOut(sender As Object, e As EventArgs)

        Try
            LOG_MESS_UTLS(GetCurrentMethod, $"Session {Session("User_Name")} terminée")
            Session("User_Name") = Replace(Replace(System.Web.HttpContext.Current.User.Identity.Name, Environment.UserDomainName, ""), "\", "")
            _AD_GET_USER(IdentityType.SamAccountName, Session("User_Name"))
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try
    End Sub
    Protected Sub TextBox_LOG_SAP_TextChanged(sender As Object, e As EventArgs) Handles TextBox_LOG_SAP.TextChanged
        Try
            Session("matricule") = TextBox_LOG_SAP.Text
            Using dt_matr = SAP_DATA_READ_PA0002($"PERNR LIKE '%{Session("matricule")}'")
                If dt_matr Is Nothing Then Throw New Exception($"Pas de nom et prénom trouvés pour le matricule {Session("matricule")}")
                Session("sn") = Trim(dt_matr(0)("NACHN").ToString)
                Session("givenname") = Trim(dt_matr(0)("VORNA").ToString)
                Session("displayname") = $"{Session("sn")} {Session("givenname")}"
            End Using
            MultiView_Master.SetActiveView(View_Master)
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try
    End Sub

    Protected Sub _GET_AD_PROPERTY(de As DirectoryEntry, sproperty As String)
        Try
            If sproperty = "thumbnailphoto" Then
                Dim data As Byte() = de.Properties(sproperty).Value
                System.Convert.ToBase64String(de.Properties(sproperty).Value).ToString()
                'Session("thumbnailphoto") = data
                Session(sproperty) = $"<img src='data:image/jpeg;base64, {System.Convert.ToBase64String(data)}' alt='photo' />"
            Else
                Session(sproperty) = de.Properties(sproperty).Value.ToString
            End If
            LOG_Msg(GetCurrentMethod, $"{sproperty} : {Session(sproperty)}")
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, $"{sproperty} : {ex.Message}")
            Exit Sub
        End Try
    End Sub

    Protected Sub _AD_GET_USER(ID_TYPE As IdentityType, ID_VAL As String)
        Const adDomainName As String = "eolane.com"
        Const adDefaultOU As String = "DC=eolane,DC=com"
        Const adUserAccount As String = "ee_trombi"
        Const adUserAccountPassword As String = "*Eol49!"
        Try
            Using ctx As PrincipalContext = New PrincipalContext(ContextType.Domain, adDomainName, adDefaultOU, adUserAccount, adUserAccountPassword)
                If Session("User_Name") = "" Then Throw New Exception("User nom existant")
                Using UserAd As UserPrincipal = UserPrincipal.FindByIdentity(ctx, ID_TYPE, ID_VAL)
                    Using DirectoryEntry As DirectoryEntry = UserAd.GetUnderlyingObject()
                        _GET_AD_PROPERTY(DirectoryEntry, "displayname")
                        _GET_AD_PROPERTY(DirectoryEntry, "title")
                        _GET_AD_PROPERTY(DirectoryEntry, "mail")
                        _GET_AD_PROPERTY(DirectoryEntry, "department")
                        _GET_AD_PROPERTY(DirectoryEntry, "samaccountname")
                        _GET_AD_PROPERTY(DirectoryEntry, "sn")
                        _GET_AD_PROPERTY(DirectoryEntry, "givenname")
                        _GET_AD_PROPERTY(DirectoryEntry, "thumbnailphoto")
                    End Using
                End Using
            End Using
            Dim sb_m, sb_p As New StringBuilder()
            Dim som As String = Session("sn")
            For ich As Integer = 0 To som.Length - 1
                If Asc(som(ich)) < 65 Or Asc(som(ich)) > 122 Then
                    sb_m.Append("_")
                Else
                    sb_m.Append(som(ich))
                End If
            Next
            Dim sop As String = Session("givenname")
            For ich As Integer = 0 To sop.Length - 1
                If Asc(sop(ich)) < 65 Or Asc(sop(ich)) > 122 Then
                    sb_p.Append("_")
                Else
                    sb_p.Append(sop(ich))
                End If
            Next
            Using dt_matr = SAP_DATA_READ_PA0002($"NACHN LIKE '{sb_m.ToString}' AND VORNA LIKE '{sb_p.ToString}'")
                If Not dt_matr Is Nothing Then Session("matricule") = Convert.ToDecimal(Trim(dt_matr(0)("PERNR").ToString)).ToString
            End Using
            Session.Timeout = 240

        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Exit Sub
        End Try

    End Sub
End Class