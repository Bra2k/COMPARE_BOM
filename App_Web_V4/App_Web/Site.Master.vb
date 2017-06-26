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

            Dim responseCookie = New HttpCookie(AntiXsrfTokenKey) With { _
                 .HttpOnly = True, _
                 .Value = _antiXsrfTokenValue _
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
        If Not IsPostBack And Session("displayname") = "" And HttpContext.Current.Request.Url.AbsolutePath.ToString() <> "/Account/Login_SAP" Then
            Session("User_Name") = Replace(Replace(System.Web.HttpContext.Current.User.Identity.Name, Environment.UserDomainName, ""), "\", "")
            Dim ctx As PrincipalContext = New PrincipalContext(ContextType.Domain, adDomainName, adDefaultOU, adUserAccount, adUserAccountPassword)
            Try
                If Session("User_Name") = "" Then Throw New Exception("User nom existant")
                Using userAd As UserPrincipal = UserPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, Session("User_Name"))
                    Dim DirectoryEntry As DirectoryEntry = userAd.GetUnderlyingObject()
                    Try
                        Session("displayname") = DirectoryEntry.Properties("displayname").Value.ToString()
                    Catch ex As Exception
                        LOG_Erreur(GetCurrentMethod, "displayname : " & ex.Message)
                    End Try
                    Try
                        Session("title") = DirectoryEntry.Properties("title").Value.ToString()
                    Catch ex As Exception
                        'LOG_Erreur(GetCurrentMethod, "title : " & ex.Message)
                    End Try
                    Try
                        Session("mail") = DirectoryEntry.Properties("mail").Value.ToString()
                    Catch ex As Exception
                        'LOG_Erreur(GetCurrentMethod, "mail : " & ex.Message)
                    End Try
                    Try
                        Session("telephonenumber") = DirectoryEntry.Properties("telephonenumber").Value.ToString()
                    Catch ex As Exception
                        '    LOG_Erreur(GetCurrentMethod, ex.Message)
                    End Try
                    'Try
                    '    Session("facsimiletelephonenumber") = DirectoryEntry.Properties("facsimiletelephonenumber").Value.ToString()
                    'Catch ex As Exception
                    '    LOG_Erreur(GetCurrentMethod, ex.Message)
                    'End Try
                    Try
                        Session("company") = DirectoryEntry.Properties("company").Value.ToString()
                    Catch ex As Exception
                        'LOG_Erreur(GetCurrentMethod, ex.Message)
                    End Try
            'Try
            '    Session("thumbnailphoto") = DirectoryEntry.Properties("thumbnailphoto").Value.ToString()
            'Catch ex As Exception
            '    LOG_Erreur(GetCurrentMethod, ex.Message)
            'End Try
            Try
                Session("samaccountname") = DirectoryEntry.Properties("samaccountname").Value.ToString()
            Catch ex As Exception
                LOG_Erreur(GetCurrentMethod, "samaccountname : " & ex.Message)
                    End Try
                    Try
                        Session("sn") = DirectoryEntry.Properties("sn").Value.ToString()
                    Catch ex As Exception
                        LOG_Erreur(GetCurrentMethod, "sn : " & ex.Message)
                    End Try
                    Try
                        Session("givenname") = DirectoryEntry.Properties("givenname").Value.ToString()
                    Catch ex As Exception
                        LOG_Erreur(GetCurrentMethod, "givenname : " & ex.Message)
                    End Try
                    'Try
                    '    Session("Mobile") = DirectoryEntry.Properties("Mobile").Value.ToString()
                    'Catch ex As Exception
                    '    LOG_Erreur(GetCurrentMethod, ex.Message)
                    'End Try
                    Try
                        Session("l") = DirectoryEntry.Properties("l").Value.ToString()
                    Catch ex As Exception
                        '    LOG_Erreur(GetCurrentMethod, ex.Message)
                    End Try
                    Try
                        Session("st") = DirectoryEntry.Properties("st").Value.ToString()
                    Catch ex As Exception
                        '    LOG_Erreur(GetCurrentMethod, ex.Message)
                    End Try
                    Try
                        Session("postalcode") = DirectoryEntry.Properties("postalcode").Value.ToString()
                    Catch ex As Exception
                        '    LOG_Erreur(GetCurrentMethod, ex.Message)
                    End Try
                    Try
                        Session("streetaddress") = DirectoryEntry.Properties("streetaddress").Value.ToString()
                    Catch ex As Exception
                        '    LOG_Erreur(GetCurrentMethod, ex.Message)
                    End Try
                    Try
                        Session("department") = DirectoryEntry.Properties("department").Value.ToString()
                    Catch ex As Exception
                        LOG_Erreur(GetCurrentMethod, "samaccountname : " & ex.Message)
                    End Try

                    Dim dt_matr As DataTable = SAP_DATA_READ_PA0002("NACHN EQ '" & Session("sn") & "' AND VORNA = '" & Session("givenname") & "'")
                    'Dim dt_matr As DataTable = SAP_DATA_READ_PA0002("NACHN EQ '" & Replace(Session("sn"), "é", "e") & "' AND VORNA = '" & Replace(Session("givenname"), "é", "e") & "'")
                    'Dim dt_matr As DataTable = SAP_DATA_READ_PA0002("NACHN EQ '" & Session("sn") & "' AND VORNA = '" & Replace(Session("givenname"), "é", "ë") & "'")
                    If dt_matr Is Nothing Then
                        Session("UrlReferrer") = HttpContext.Current.Request.Url.AbsolutePath.ToString()
                        Response.Redirect("~/Account/Login_SAP.aspx")
                    Else
                        Session("matricule") = Convert.ToDecimal(dt_matr(0)("PERNR").ToString).ToString
                    End If
                    Session.Timeout = 240
                End Using
            Catch ex As Exception
                LOG_Erreur(GetCurrentMethod, ex.Message)
            End Try
        End If

    End Sub

    Protected Sub Unnamed_LoggingOut(sender As Object, e As LoginCancelEventArgs)
        Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie)
    End Sub
    Protected Sub APP_WEB_LoggedOut(sender As Object, e As EventArgs)
        LOG_Msg(GetCurrentMethod, "Session terminée")
        Session("User_Name") = Replace(Replace(System.Web.HttpContext.Current.User.Identity.Name, Environment.UserDomainName, ""), "\", "")
        Dim ctx As PrincipalContext = New PrincipalContext(ContextType.Domain, adDomainName, adDefaultOU, adUserAccount, adUserAccountPassword)
        Try
            If Session("User_Name") = "" Then Throw New Exception("User nom existant")
            Using userAd As UserPrincipal = UserPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, Session("User_Name"))
                Dim DirectoryEntry As DirectoryEntry = userAd.GetUnderlyingObject()
                Try
                    Session("displayname") = DirectoryEntry.Properties("displayname").Value.ToString()
                Catch ex As Exception
                    LOG_Erreur(GetCurrentMethod, "displayname : " & ex.Message)
                End Try
                Try
                    Session("title") = DirectoryEntry.Properties("title").Value.ToString()
                Catch ex As Exception
                    'LOG_Erreur(GetCurrentMethod, "title : " & ex.Message)
                End Try
                Try
                    Session("mail") = DirectoryEntry.Properties("mail").Value.ToString()
                Catch ex As Exception
                    'LOG_Erreur(GetCurrentMethod, "mail : " & ex.Message)
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
                    Session("thumbnailphoto") = System.Convert.ToBase64String(DirectoryEntry.Properties("thumbnailphoto").Value) '.ToString()
                Catch ex As Exception
                    LOG_Erreur(GetCurrentMethod, ex.Message)
                End Try
                Try
                    Session("samaccountname") = DirectoryEntry.Properties("samaccountname").Value.ToString()
                Catch ex As Exception
                    LOG_Erreur(GetCurrentMethod, "samaccountname : " & ex.Message)
                End Try
                Try
                    Session("sn") = DirectoryEntry.Properties("sn").Value.ToString()
                Catch ex As Exception
                    LOG_Erreur(GetCurrentMethod, "sn : " & ex.Message)
                End Try
                Try
                    Session("givenname") = DirectoryEntry.Properties("givenname").Value.ToString()
                Catch ex As Exception
                    LOG_Erreur(GetCurrentMethod, "givenname : " & ex.Message)
                End Try
                'Try
                '    Session("Mobile") = DirectoryEntry.Properties("Mobile").Value.ToString()
                'Catch ex As Exception
                '    LOG_Erreur(GetCurrentMethod, ex.Message)
                'End Try
                'Try
                '    Session("l") = DirectoryEntry.Properties("l").Value.ToString()
                'Catch ex As Exception
                '    LOG_Erreur(GetCurrentMethod, ex.Message)
                'End Try
                'Try
                '    Session("st") = DirectoryEntry.Properties("st").Value.ToString()
                'Catch ex As Exception
                '    LOG_Erreur(GetCurrentMethod, ex.Message)
                'End Try
                'Try
                '    Session("postalcode") = DirectoryEntry.Properties("postalcode").Value.ToString()
                'Catch ex As Exception
                '    LOG_Erreur(GetCurrentMethod, ex.Message)
                'End Try
                'Try
                '    Session("streetaddress") = DirectoryEntry.Properties("streetaddress").Value.ToString()
                'Catch ex As Exception
                '    LOG_Erreur(GetCurrentMethod, ex.Message)
                'End Try
                Try
                    Session("department") = DirectoryEntry.Properties("department").Value.ToString()
                Catch ex As Exception
                    LOG_Erreur(GetCurrentMethod, "samaccountname : " & ex.Message)
                End Try

                Dim dt_matr As DataTable = SAP_DATA_READ_PA0002("NACHN EQ '" & Session("sn") & "' AND VORNA = '" & Session("givenname") & "'")
                If dt_matr Is Nothing Then
                    Session("UrlReferrer") = HttpContext.Current.Request.Url.AbsolutePath.ToString()
                    Response.Redirect("~/Account/Login_SAP.aspx")
                Else
                    Session("matricule") = Convert.ToDecimal(dt_matr(0)("PERNR").ToString).ToString
                End If
                Session.Timeout = 240
            End Using
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try
    End Sub
End Class