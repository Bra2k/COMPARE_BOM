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
    Const adDomainName As String = "eolane.com"
    Const adDefaultOU As String = "DC=eolane,DC=com"
    Const adUserAccount As String = "ee_trombi"
    Const adUserAccountPassword As String = "*Eol49!"
    Public Property ClientScript As Object

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

            Dim responseCookie = New HttpCookie(AntiXsrfTokenKey) With {
                 .HttpOnly = True,
                 .Value = _antiXsrfTokenValue
            }
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
        Dim dt_matr As New DataTable
        'Dim ctx As PrincipalContext
        'Dim DirectoryEntry As New DirectoryEntry
        Try
            'If Len(Label_LOG_SAP.Text) > 1 Then
            '    dt_matr = SAP_DATA_READ_PA0002("PERNR LIKE '%" & Label_LOG_SAP.Text & "'")
            '    If dt_matr Is Nothing Then Throw New Exception("pas de nom/prénom trouvés pour le matricule " & Label_LOG_SAP.Text)
            '    Session("matricule") = Label_LOG_SAP.Text
            '    Session("sn") = Trim(dt_matr(0)("NACHN").ToString)
            '    Session("givenname") = Trim(dt_matr(0)("VORNA").ToString)
            '    Session("displayname") = Session("sn") & " " & Session("givenname")
            '    AD_GET_USER(IdentityType.SamAccountName, "ce_" & LCase(Left(Session("sn"), 3)) & LCase(Left(Session("givenname"), 2)))
            'ctx = New PrincipalContext(ContextType.Domain, adDomainName, adDefaultOU, adUserAccount, adUserAccountPassword)
            'Using UserAd As UserPrincipal = UserPrincipal.FindByIdentity(ctx, IdentityType.UserPrincipalName, Session("displayname"))
            '    DirectoryEntry = UserAd.GetUnderlyingObject()
            '    With DirectoryEntry
            '        Session("displayname") = .Properties("displayname").Value.ToString
            '        Session("title") = .Properties("title").Value.ToString
            '        Session("mail") = .Properties("mail").Value.ToString
            '        Session("telephonenumber") = .Properties("telephonenumber").Value.ToString
            '        Session("company") = .Properties("company").Value.ToString
            '        Session("thumbnailphoto") = .Properties("thumbnailphoto").Value.ToString
            '        Session("samaccountname") = .Properties("samaccountname").Value.ToString
            '        Session("sn") = .Properties("sn").Value.ToString
            '        Session("givenname") = .Properties("givenname").Value.ToString
            '        Session("l") = .Properties("l").Value.ToString
            '        Session("st") = .Properties("st").Value.ToString
            '        Session("postalcode") = .Properties("postalcode").Value.ToString
            '        Session("streetaddress") = .Properties("streetaddress").Value.ToString
            '        Session("department") = .Properties("department").Value.ToString
            '    End With
            'End Using
            'Else
            If Not IsPostBack And Session("displayname") = "" And HttpContext.Current.Request.Url.AbsolutePath.ToString() <> "/Account/Login_SAP" Then
                Session("User_Name") = Replace(Replace(System.Web.HttpContext.Current.User.Identity.Name, Environment.UserDomainName, ""), "\", "")
                AD_GET_USER(IdentityType.SamAccountName, Session("User_Name"))
                'ctx = New PrincipalContext(ContextType.Domain, adDomainName, adDefaultOU, adUserAccount, adUserAccountPassword)
                'Try
                '    If Session("User_Name") = "" Then Throw New Exception("User nom existant")
                '    Using userAd As UserPrincipal = UserPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, Session("User_Name"))
                '        DirectoryEntry = userAd.GetUnderlyingObject()
                '        Try
                '            Session("displayname") = DirectoryEntry.Properties("displayname").Value.ToString

                '        Catch ex As Exception
                '            LOG_Erreur(GetCurrentMethod, ex.Message)
                '        End Try
                '        Try
                '            Session("title") = DirectoryEntry.Properties("title").Value.ToString()
                '        Catch ex As Exception
                '            'LOG_Erreur(GetCurrentMethod, "title : " & ex.Message)
                '        End Try
                '        Try
                '            Session("mail") = DirectoryEntry.Properties("mail").Value.ToString()
                '        Catch ex As Exception
                '            'LOG_Erreur(GetCurrentMethod, "mail : " & ex.Message)
                '        End Try
                '        Try
                '            Session("telephonenumber") = DirectoryEntry.Properties("telephonenumber").Value.ToString()
                '        Catch ex As Exception
                '            '    LOG_Erreur(GetCurrentMethod, ex.Message)
                '        End Try
                '        'Try
                '        '    Session("facsimiletelephonenumber") = DirectoryEntry.Properties("facsimiletelephonenumber").Value.ToString()
                '        'Catch ex As Exception
                '        '    LOG_Erreur(GetCurrentMethod, ex.Message)
                '        'End Try
                '        Try
                '            Session("company") = DirectoryEntry.Properties("company").Value.ToString()
                '        Catch ex As Exception
                '            'LOG_Erreur(GetCurrentMethod, ex.Message)
                '        End Try
                '        'Try
                '        Session("thumbnailphoto") = DirectoryEntry.Properties("thumbnailphoto").Value.ToString()
                '        'Catch ex As Exception
                '        '    LOG_Erreur(GetCurrentMethod, ex.Message)
                '        'End Try
                '        Try
                '            Session("samaccountname") = DirectoryEntry.Properties("samaccountname").Value.ToString()
                '        Catch ex As Exception
                '            LOG_Erreur(GetCurrentMethod, "samaccountname : " & ex.Message)
                '        End Try
                '        Try
                '            Session("sn") = DirectoryEntry.Properties("sn").Value.ToString()
                '        Catch ex As Exception
                '            LOG_Erreur(GetCurrentMethod, "sn : " & ex.Message)
                '        End Try
                '        Try
                '            Session("givenname") = DirectoryEntry.Properties("givenname").Value.ToString()
                '        Catch ex As Exception
                '            LOG_Erreur(GetCurrentMethod, "givenname : " & ex.Message)
                '        End Try
                '        'Try
                '        '    Session("Mobile") = DirectoryEntry.Properties("Mobile").Value.ToString()
                '        'Catch ex As Exception
                '        '    LOG_Erreur(GetCurrentMethod, ex.Message)
                '        'End Try
                '        Try
                '            Session("l") = DirectoryEntry.Properties("l").Value.ToString()
                '        Catch ex As Exception
                '            '    LOG_Erreur(GetCurrentMethod, ex.Message)
                '        End Try
                '        Try
                '            Session("st") = DirectoryEntry.Properties("st").Value.ToString()
                '        Catch ex As Exception
                '            '    LOG_Erreur(GetCurrentMethod, ex.Message)
                '        End Try
                '        Try
                '            Session("postalcode") = DirectoryEntry.Properties("postalcode").Value.ToString()
                '        Catch ex As Exception
                '            '    LOG_Erreur(GetCurrentMethod, ex.Message)
                '        End Try
                '        Try
                '            Session("streetaddress") = DirectoryEntry.Properties("streetaddress").Value.ToString()
                '        Catch ex As Exception
                '            '    LOG_Erreur(GetCurrentMethod, ex.Message)
                '        End Try
                '        Try
                '            Session("department") = DirectoryEntry.Properties("department").Value.ToString()
                '        Catch ex As Exception
                '            LOG_Erreur(GetCurrentMethod, "samaccountname : " & ex.Message)
                '        End Try

            End If
            'End If
            'Session.Timeout = 240
            'End Using
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            'End Try
            'End If
            'End If
            'Catch ex As Exception
            '    LOG_Erreur(GetCurrentMethod, ex.Message)
        Finally
            Label_LOG_SAP.Text = ""
        End Try
    End Sub

    Protected Sub Unnamed_LoggingOut(sender As Object, e As LoginCancelEventArgs)
        Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie)
    End Sub
    Protected Sub APP_WEB_LoggedOut(sender As Object, e As EventArgs)
        Session.Abandon()
        LOG_Msg(GetCurrentMethod, "Session terminée")
        'Dim ctx As PrincipalContext = New PrincipalContext(ContextType.Domain, adDomainName, adDefaultOU, adUserAccount, adUserAccountPassword)
        Try
            Session("User_Name") = Replace(Replace(System.Web.HttpContext.Current.User.Identity.Name, Environment.UserDomainName, ""), "\", "")
            AD_GET_USER(IdentityType.SamAccountName, Session("User_Name"))

            '    If Session("User_Name") = "" Then Throw New Exception("User nom existant")
            '    Using userAd As UserPrincipal = UserPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, Session("User_Name"))
            '        Dim DirectoryEntry As DirectoryEntry = userAd.GetUnderlyingObject()
            '        Try
            '            Session("displayname") = DirectoryEntry.Properties("displayname").Value.ToString()
            '        Catch ex As Exception
            '            LOG_Erreur(GetCurrentMethod, "displayname :    " & ex.Message)
            '        End Try
            '        Try
            '            Session("title") = DirectoryEntry.Properties("title").Value.ToString()
            '        Catch ex As Exception
            '            'LOG_Erreur(GetCurrentMethod, "title : " & ex.Message)
            '        End Try
            '        Try
            '            Session("mail") = DirectoryEntry.Properties("mail").Value.ToString()
            '        Catch ex As Exception
            '            'LOG_Erreur(GetCurrentMethod, "mail : " & ex.Message)
            '        End Try
            '        'Try
            '        '    Session("telephonenumber") = DirectoryEntry.Properties("telephonenumber").Value.ToString()
            '        'Catch ex As Exception
            '        '    LOG_Erreur(GetCurrentMethod, ex.Message)
            '        'End Try
            '        'Try
            '        '    Session("facsimiletelephonenumber") = DirectoryEntry.Properties("facsimiletelephonenumber").Value.ToString()
            '        'Catch ex As Exception
            '        '    LOG_Erreur(GetCurrentMethod, ex.Message)
            '        'End Try
            '        'Try
            '        '    Session("company") = DirectoryEntry.Properties("company").Value.ToString()
            '        'Catch ex As Exception
            '        '    LOG_Erreur(GetCurrentMethod, ex.Message)
            '        'End Try
            '        Try
            '            Session("thumbnailphoto") = System.Convert.ToBase64String(DirectoryEntry.Properties("thumbnailphoto").Value) '.ToString()
            '        Catch ex As Exception
            '            LOG_Erreur(GetCurrentMethod, ex.Message)
            '        End Try
            '        Try
            '            Session("samaccountname") = DirectoryEntry.Properties("samaccountname").Value.ToString()
            '        Catch ex As Exception
            '            LOG_Erreur(GetCurrentMethod, "samaccountname : " & ex.Message)
            '        End Try
            '        Try
            '            Session("sn") = DirectoryEntry.Properties("sn").Value.ToString()
            '        Catch ex As Exception
            '            LOG_Erreur(GetCurrentMethod, "sn : " & ex.Message)
            '        End Try
            '        Try
            '            Session("givenname") = DirectoryEntry.Properties("givenname").Value.ToString()
            '        Catch ex As Exception
            '            LOG_Erreur(GetCurrentMethod, "givenname : " & ex.Message)
            '        End Try
            '        'Try
            '        '    Session("Mobile") = DirectoryEntry.Properties("Mobile").Value.ToString()
            '        'Catch ex As Exception
            '        '    LOG_Erreur(GetCurrentMethod, ex.Message)
            '        'End Try
            '        'Try
            '        '    Session("l") = DirectoryEntry.Properties("l").Value.ToString()
            '        'Catch ex As Exception
            '        '    LOG_Erreur(GetCurrentMethod, ex.Message)
            '        'End Try
            '        'Try
            '        '    Session("st") = DirectoryEntry.Properties("st").Value.ToString()
            '        'Catch ex As Exception
            '        '    LOG_Erreur(GetCurrentMethod, ex.Message)
            '        'End Try
            '        'Try
            '        '    Session("postalcode") = DirectoryEntry.Properties("postalcode").Value.ToString()
            '        'Catch ex As Exception
            '        '    LOG_Erreur(GetCurrentMethod, ex.Message)
            '        'End Try
            '        'Try
            '        '    Session("streetaddress") = DirectoryEntry.Properties("streetaddress").Value.ToString()
            '        'Catch ex As Exception
            '        '    LOG_Erreur(GetCurrentMethod, ex.Message)
            '        'End Try
            '        Try
            '            Session("department") = DirectoryEntry.Properties("department").Value.ToString()
            '        Catch ex As Exception
            '            LOG_Erreur(GetCurrentMethod, "samaccountname : " & ex.Message)
            '        End Try

            '        Dim dt_matr As DataTable = SAP_DATA_READ_PA0002("NACHN EQ '" & Session("sn") & "' AND VORNA = '" & Session("givenname") & "'")
            '        If dt_matr Is Nothing Then
            '            Session("UrlReferrer") = HttpContext.Current.Request.Url.AbsolutePath.ToString()
            '            Response.Redirect("~/Account/Login_SAP.aspx")
            '        Else
            '            Session("matricule") = Convert.ToDecimal(dt_matr(0)("PERNR").ToString).ToString
            '        End If
            '        Session.Timeout = 240
            '    End Using
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try
    End Sub

    Protected Sub AD_GET_USER(ID_TYPE As IdentityType, ID_VAL As String)
        Dim dt_matr As New DataTable
        Dim ctx As PrincipalContext
        Dim DirectoryEntry As New DirectoryEntry
        Try
            ctx = New PrincipalContext(ContextType.Domain, adDomainName, adDefaultOU, adUserAccount, adUserAccountPassword)
            'If ID_VAL = "SO_TECHN" Then ID_VAL = "SO_SPEVI"
            Using UserAd As UserPrincipal = UserPrincipal.FindByIdentity(ctx, ID_TYPE, ID_VAL)
                DirectoryEntry = UserAd.GetUnderlyingObject()
                With DirectoryEntry
                    Try
                        Session("displayname") = .Properties("displayname").Value.ToString()
                    Catch ex As Exception
                        LOG_Erreur(GetCurrentMethod, "displayname :    " & ex.Message)
                    End Try
                    Try
                        Session("title") = .Properties("title").Value.ToString()
                    Catch ex As Exception
                        LOG_Erreur(GetCurrentMethod, "title : " & ex.Message)
                    End Try
                    Try
                        Session("mail") = .Properties("mail").Value.ToString()
                    Catch ex As Exception
                        LOG_Erreur(GetCurrentMethod, "mail : " & ex.Message)
                    End Try
                    'Try
                    '    Session("telephonenumber") = DirectoryEntry.Properties("telephonenumber").Value.ToString()
                    'Catch ex As Exception
                    '    LOG_Erreur(GetCurrentMethod, ex.Message)
                    'End Try
                    'Try
                    '    Session("facsimiletelephonenumber") = DirectoryEntry.Properties("facsimiletelephonenumber").Value.ToString()
                    'Catch ex As Exception
                    '    LOG_Erreur(GetCurrentMethod, ex.Message)
                    'End Try
                    'Try
                    '    Session("company") = DirectoryEntry.Properties("company").Value.ToString()
                    'Catch ex As Exception
                    '    LOG_Erreur(GetCurrentMethod, ex.Message)
                    'End Try
                    Try
                        Session("thumbnailphoto") = System.Convert.ToBase64String(.Properties("thumbnailphoto").Value) '.ToString()
                    Catch ex As Exception
                        LOG_Erreur(GetCurrentMethod, ex.Message)
                    End Try
                    Try
                        Session("samaccountname") = .Properties("samaccountname").Value.ToString()
                    Catch ex As Exception
                        LOG_Erreur(GetCurrentMethod, "samaccountname : " & ex.Message)
                    End Try
                    Try
                        Session("sn") = .Properties("sn").Value.ToString()
                    Catch ex As Exception
                        LOG_Erreur(GetCurrentMethod, "sn : " & ex.Message)
                    End Try
                    Try
                        Session("givenname") = .Properties("givenname").Value.ToString()
                    Catch ex As Exception
                        LOG_Erreur(GetCurrentMethod, "givenname : " & ex.Message)
                    End Try
                    'Try
                    '    Session("Mobile") = DirectoryEntry.Properties("Mobile").Value.ToString()
                    'Catch ex As Exception
                    '    LOG_Erreur(GetCurrentMethod, ex.Message)
                    'End Try
                    Try
                        Session("l") = .Properties("l").Value.ToString()
                    Catch ex As Exception
                        LOG_Erreur(GetCurrentMethod, ex.Message)
                    End Try
                    Try
                        Session("st") = .Properties("st").Value.ToString()
                    Catch ex As Exception
                        LOG_Erreur(GetCurrentMethod, ex.Message)
                    End Try
                    Try
                        Session("postalcode") = .Properties("postalcode").Value.ToString()
                    Catch ex As Exception
                        LOG_Erreur(GetCurrentMethod, ex.Message)
                    End Try
                    Try
                        Session("streetaddress") = .Properties("streetaddress").Value.ToString()
                    Catch ex As Exception
                        LOG_Erreur(GetCurrentMethod, ex.Message)
                    End Try
                    Try
                        Session("department") = .Properties("department").Value.ToString()
                    Catch ex As Exception
                        LOG_Erreur(GetCurrentMethod, "samaccountname : " & ex.Message)
                    End Try

                    If Session("sn") = "Technicien" Then
                        Session("sn") = "SPERPERINI"
                        Session("givenname") = "Vincent"
                    End If

                    dt_matr = SAP_DATA_READ_PA0002("NACHN EQ '" & Session("sn") & "' AND VORNA = '" & Session("givenname") & "'")
                    If dt_matr Is Nothing Then
                        Session("UrlReferrer") = HttpContext.Current.Request.Url.AbsolutePath.ToString()
                        Response.Redirect("~/Account/Login_SAP.aspx")

                        'Dim pageHandler As Page = HttpContext.Current.CurrentHandler
                        'pageHandler.RegisterStartupScript("LOG_SAP", "<script type = ""text/javascript"">  LoginSAP.setmatricule(); </script>")
                        'Response.Redirect(Request.RawUrl)

                        'Session("matricule") = Label_LOG_SAP.Text

                        'dt_matr = SAP_DATA_MATR(Label_LOG_SAP.Text)
                        'If dt_matr Is Nothing Then Throw New Exception("pas de nom/prénom trouvés pour le matricule " & Label_LOG_SAP.Text)
                    Else
                        Session("matricule") = Trim(dt_matr(0)("PERNR").ToString)
                    End If
                End With
            End Using
            Session.Timeout = 240
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Exit Sub
        End Try

    End Sub
End Class