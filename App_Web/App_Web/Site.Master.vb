Imports Microsoft.AspNet.Identity
Imports System.DirectoryServices
Imports System.DirectoryServices.AccountManagement
Imports App_Web.Class_SAP_DATA
Imports App_Web.LOG
Imports System.Reflection.MethodBase
'Imports System.Globalization

Public Class SiteMaster
    Inherits MasterPage
    'Implements IPostBackEventHandler
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

        Dim dt As New DataTable
        Try
            If Not IsPostBack And Session("displayname") = "" And HttpContext.Current.Request.Url.AbsolutePath.ToString() <> "/Account/Login_SAP" Then
                Session("User_Name") = Replace(Replace(System.Web.HttpContext.Current.User.Identity.Name, Environment.UserDomainName, ""), "\", "")
                AD_GET_USER(IdentityType.SamAccountName, Session("User_Name"))
            End If

        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        Finally
            If Session("matricule") = "" Then MultiView_Master.SetActiveView(View_LOG_SAP)
        End Try
    End Sub

    Protected Sub Unnamed_LoggingOut(sender As Object, e As LoginCancelEventArgs)
        Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie)
    End Sub
    Protected Sub APP_WEB_LoggedOut(sender As Object, e As EventArgs)


        Try
            Session.Abandon()
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try
        LOG_Msg(GetCurrentMethod, "Session terminée")
    End Sub

    Protected Sub AD_GET_USER(ID_TYPE As IdentityType, ID_VAL As String)
        Dim ctx As PrincipalContext
        Dim DirectoryEntry As New DirectoryEntry
        Try
            ctx = New PrincipalContext(ContextType.Domain, adDomainName, adDefaultOU, adUserAccount, adUserAccountPassword)
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
                        Dim data As Byte() = .Properties("thumbnailphoto").Value
                        System.Convert.ToBase64String(.Properties("thumbnailphoto").Value) '.ToString()
                        Session("thumbnailphoto") = $"<img src='data:image/jpeg;base64, {System.Convert.ToBase64String(data)}' alt='photo' />"
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
                End With
            End Using
            Session.Timeout = 240
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
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
        End Try
    End Sub
End Class

