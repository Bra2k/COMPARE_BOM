Imports System
Imports System.Collections.Generic
Imports System.DirectoryServices
Imports System.DirectoryServices.AccountManagement
Imports System.Linq
Imports System.Text
Imports System.Threading
Imports System.Security.Principal
Imports App_Web.LOG
Imports System.Reflection.MethodBase

Public Class Login
    Inherits System.Web.UI.Page
    Const adDomainName As String = "eolane.com"
    Const adDefaultOU As String = "DC=eolane,DC=com"
    Const adUserAccount As String = "ee_trombi"
    Const adUserAccountPassword As String = "*Eol49!"

    ' VB.NET
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Check for authentication and write into Cookie
        'Dim sLDAPCN As String = (a.GetCNByLDAP(p.Identity.Name))
        'Check for LDAP Authentication First
        'If sLDAPCN <> "" And
        '(a.blnFindCNinGroups(a.GetCNByLDAP(p.Identity.Name),
        ' "AppAUserGroups")) Then
        'objCook.Value = (sLDAPCN)
        'Response.Cookies.Add(objCook)
        'Check for LDAP User Existence in Application Database
        'If User exists in Application DB, Start Application
        'If (FindLDAPCNinDB(sLDAPCN)) Then
        'DROP Authentication Cookie token
        'Compute a hashed value of the current session ID
        'into a cookie to signal authentication success
        'Be sure to check for this cookie for every page load
        'if value doesnt match or cookie doesnt exists,
        'Re-do the whole authentication process again.
        'Single-sign-on should continue, the cookies drop
        'prevent authenticated LDAP Users from not having
        'their CN mapped with other application-specific
        'fields or data.
        'Response.Redirect("Welcome.aspx")
        'Else 'If False, do Database mapping
        'Response.Redirect("CollectOtherInfo.aspx")
        '  End If
        '   Else
        'Response.Redirect("LoginError.htm")
        '  End If
    End Sub


    'Protected Sub Login_Click(sender As Object, e As EventArgs) Handles btnLogin.Click

    '    Dim adPath As String = "LDAP://" & adDomainName & "/" & adDefaultOU 'Path to your LDAP directory server
    '    Dim adAuth As LdapAuthentication = New LdapAuthentication(adPath)
    '    Try
    '        If (True = adAuth.IsAuthenticated(adDomainName, txtUsername.Text, txtPassword.Text)) Then
    '            Dim groups As String = adAuth.GetGroups()

    '            'Create the ticket, and add the groups.
    '            Dim isCookiePersistent As Boolean = chkPersist.Checked
    '            Dim authTicket As FormsAuthenticationTicket = New FormsAuthenticationTicket(1,
    '            txtUsername.Text, DateTime.Now, DateTime.Now.AddMinutes(60), isCookiePersistent, groups)

    '            'Encrypt the ticket.
    '            Dim encryptedTicket As String = FormsAuthentication.Encrypt(authTicket)

    '            'Create a cookie, and then add the encrypted ticket to the cookie as data.
    '            Dim authCookie As HttpCookie = New HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)

    '            If (isCookiePersistent = True) Then
    '                authCookie.Expires = authTicket.Expiration
    '            End If
    '            'Add the cookie to the outgoing cookies collection.
    '            Response.Cookies.Add(authCookie)

    '            Dim PositionUser As String = "Undefined"
    '            Dim ctx As PrincipalContext = New PrincipalContext(ContextType.Domain, adDomainName, adDefaultOU, adUserAccount, adUserAccountPassword)
    '            If txtUsername.Text <> "" Then
    '                Using userAd As UserPrincipal = UserPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, txtUsername.Text)
    '                    Dim DirectoryEntry As DirectoryEntry = userAd.GetUnderlyingObject()
    '                    Session("Username") = txtUsername.Text
    '                    Session("displayname") = DirectoryEntry.Properties("displayname").Value.ToString()
    '                    Session("title") = DirectoryEntry.Properties("title").Value.ToString()
    '                    Session("mail") = DirectoryEntry.Properties("mail").Value.ToString()
    '                    Session("telephonenumber") = DirectoryEntry.Properties("telephonenumber").Value.ToString()
    '                    'Session("facsimiletelephonenumber") = DirectoryEntry.Properties("facsimiletelephonenumber").Value.ToString()
    '                    'Session("company") = DirectoryEntry.Properties("company").Value.ToString()
    '                    'Session("thumbnailphoto") = DirectoryEntry.Properties("thumbnailphoto").Value.ToString()
    '                    'Session("samaccountname") = DirectoryEntry.Properties("samaccountname").Value.ToString()
    '                    'Session("sn") = DirectoryEntry.Properties("sn").Value.ToString()
    '                    'Session("givenname") = DirectoryEntry.Properties("givenname").Value.ToString()
    '                    'Session("Mobile") = DirectoryEntry.Properties("Mobile").Value.ToString()
    '                    'Session("l") = DirectoryEntry.Properties("l").Value.ToString()
    '                    'Session("st") = DirectoryEntry.Properties("st").Value.ToString()
    '                    'Session("postalcode") = DirectoryEntry.Properties("postalcode").Value.ToString()
    '                    'Session("streetaddress") = DirectoryEntry.Properties("streetaddress").Value.ToString()
    '                End Using
    '            End If
    '        Else
    '            Throw New Exception("Authentication did not succeed. Check user name and password.")
    '        End If

    '    Catch ex As Exception
    '        LOG_Erreur(GetCurrentMethod, "Error authenticating. " & ex.Message)
    '        Exit Sub
    '    End Try
    '    'You can redirect now.
    '    LOG_Msg(GetCurrentMethod, "Authentification : " & txtUsername.Text)
    '    Response.Redirect(FormsAuthentication.GetRedirectUrl(txtUsername.Text, False))

    'End Sub

End Class