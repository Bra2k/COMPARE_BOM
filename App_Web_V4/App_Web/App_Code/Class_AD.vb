Imports System.DirectoryServices
Imports System.DirectoryServices.AccountManagement
Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports System.Web

Public Class Class_AD
    Const adDomainName As String = "eolane.com"
    Const adDefaultOU As String = "DC=eolane,DC=com"
    Const adUserAccount As String = "ee_trombi"
    Const adUserAccountPassword As String = "*Eol49!"
    Dim ctx As PrincipalContext = New PrincipalContext(ContextType.Domain, adDomainName, adDefaultOU, adUserAccount, adUserAccountPassword)

    If txtUsername.Text <> "" Then
    Using userAd As UserPrincipal = UserPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, txtUsername.Text)
    Dim DirectoryEntry As DirectoryEntry = userAd.GetUnderlyingObject()
        Session("Username") = txtUsername.Text
        Session("displayname") = DirectoryEntry.Properties("displayname").Value.ToString()
        Session("title") = DirectoryEntry.Properties("title").Value.ToString()
        Session("mail") = DirectoryEntry.Properties("mail").Value.ToString()
        Session("telephonenumber") = DirectoryEntry.Properties("telephonenumber").Value.ToString()
        'Session("facsimiletelephonenumber") = DirectoryEntry.Properties("facsimiletelephonenumber").Value.ToString()
        'Session("company") = DirectoryEntry.Properties("company").Value.ToString()
        'Session("thumbnailphoto") = DirectoryEntry.Properties("thumbnailphoto").Value.ToString()
        'Session("samaccountname") = DirectoryEntry.Properties("samaccountname").Value.ToString()
        'Session("sn") = DirectoryEntry.Properties("sn").Value.ToString()
        'Session("givenname") = DirectoryEntry.Properties("givenname").Value.ToString()
        'Session("Mobile") = DirectoryEntry.Properties("Mobile").Value.ToString()
        'Session("l") = DirectoryEntry.Properties("l").Value.ToString()
        'Session("st") = DirectoryEntry.Properties("st").Value.ToString()
        'Session("postalcode") = DirectoryEntry.Properties("postalcode").Value.ToString()
        'Session("streetaddress") = DirectoryEntry.Properties("streetaddress").Value.ToString()
        End Using
    End If
End Class
