Imports System.Windows.Forms
Imports System.IO
Imports System.Reflection
Imports System.Web.UI.WebControls
Imports App_Web.SiteMaster
Imports System.Security.Principal
Imports System.Data.SqlClient

Public Class LOG
    ' Inherits MasterPage
    Public Shared Sub LOG_Msg(mbFunction As MethodBase, sMessage As String)

        Dim ip As String = System.Web.HttpContext.Current.Request.UserHostAddress
        Dim sPoste_User As String = System.Net.Dns.GetHostEntry(ip).HostName() & " - " & Replace(Replace(System.Web.HttpContext.Current.User.Identity.Name, Environment.UserDomainName, ""), "\", "")

        Dim sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=APP_WEB_ECO;Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
        Dim sQuery As String = "INSERT INTO [APP_WEB_ECO].[dbo].[DTM_LOG_APP_WEB]([NM_POST], [DT_LOG], [NM_FONC], [NM_MSG], [NM_CTCT])
                                VALUES ('" & sPoste_User & "', GETDATE(), '" & mbFunction.Name & "', '" & Replace(sMessage, "'", "''") & "', 'Information')"
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        Dim pageHandler As Page = HttpContext.Current.CurrentHandler
        Dim MP_Label_PGS_TACH As WebControls.Label
        Dim MP_Label_ERR_MSG As WebControls.Label

        Try
            con.ConnectionString = sChaineConnexion
            con.Open()
            cmd.Connection = con
            cmd.CommandText = sQuery
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            Dim sw As New StreamWriter("C:\sources\App_Web_LOG\" & sPoste_User & ".log", True)
            sw.WriteLine(DateTime.Now.ToString() & ex.Message)
            sw.WriteLine(DateTime.Now.ToString() & " - Fonction : " & mbFunction.Name & " - Message : " & sMessage)
            sw.Close()
        Finally
            con.Close()
            con = Nothing
        End Try
        If My.Computer.Name = "CEDB03" Then
            MP_Label_PGS_TACH = CType(pageHandler.Master.FindControl("Label_PGS_TACH"), WebControls.Label)
            MP_Label_PGS_TACH.Text = "Fonction : " & mbFunction.Name & " - Message : " & sMessage
            MP_Label_ERR_MSG = CType(pageHandler.Master.FindControl("Label_ERR_MSG"), WebControls.Label)
            MP_Label_ERR_MSG.Text = ""
        End If

    End Sub
    Public Shared Sub LOG_Erreur(mbFunction As MethodBase, sMessage As String)

        Dim ip As String = System.Web.HttpContext.Current.Request.UserHostAddress
        Dim sPoste_User As String = System.Net.Dns.GetHostEntry(ip).HostName() & " - " & Replace(Replace(System.Web.HttpContext.Current.User.Identity.Name, Environment.UserDomainName, ""), "\", "")

        Dim sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=APP_WEB_ECO;Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
        Dim sQuery As String = "INSERT INTO [APP_WEB_ECO].[dbo].[DTM_LOG_APP_WEB]([NM_POST], [DT_LOG], [NM_FONC], [NM_MSG], [NM_CTCT])
                                VALUES ('" & sPoste_User & "', GETDATE(), '" & mbFunction.Name & "', '" & Replace(sMessage, "'", "''") & "', 'Erreur')"
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand

        Dim pageHandler As Page = HttpContext.Current.CurrentHandler
        Dim MP_Label_ERR_MSG As WebControls.Label

        Try
            con.ConnectionString = sChaineConnexion
            con.Open()
            cmd.Connection = con
            cmd.CommandText = sQuery
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            Dim swLOG As New StreamWriter("C:\sources\App_Web_LOG\" & sPoste_User & ".log", True)
            Dim swLOG_ERR As New StreamWriter("C:\sources\App_Web_LOG\" & sPoste_User & "_erreur.log", True)
            swLOG_ERR.WriteLine(DateTime.Now.ToString() & ex.Message)
            swLOG.WriteLine(DateTime.Now.ToString() & ex.Message)
            swLOG_ERR.WriteLine(DateTime.Now.ToString() & " - Fonction : " & mbFunction.Name & " - Message : " & sMessage)
            swLOG.WriteLine(DateTime.Now.ToString() & " - Erreur à la fonction : " & mbFunction.Name & " - Message d'erreur : " & sMessage)
            swLOG_ERR.Close()
            swLOG.Close()
        Finally
            con.Close()
            con = Nothing
        End Try
        If My.Computer.Name = "CEDB03" Then
            MP_Label_ERR_MSG = CType(pageHandler.Master.FindControl("Label_ERR_MSG"), WebControls.Label)
            MP_Label_ERR_MSG.Text = "Erreur à la fonction : " & mbFunction.Name & " - Message d'erreur : " & sMessage
        End If

    End Sub

    Public Shared Sub LOG_MESS_UTLS(mbFunction As MethodBase, sMessage As String, Optional sType As String = "Information", Optional bAFCG_FENE As Boolean = True)

        Dim ip As String = System.Web.HttpContext.Current.Request.UserHostAddress
        Dim sPoste_User As String = System.Net.Dns.GetHostEntry(ip).HostName() & " - " & Replace(Replace(System.Web.HttpContext.Current.User.Identity.Name, Environment.UserDomainName, ""), "\", "")

        Dim sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=APP_WEB_ECO;Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
        Dim sQuery As String = "INSERT INTO [APP_WEB_ECO].[dbo].[DTM_LOG_APP_WEB]([NM_POST], [DT_LOG], [NM_FONC], [NM_MSG], [NM_CTCT])
                                VALUES ('" & sPoste_User & "', GETDATE(), '" & mbFunction.Name & "', '" & Replace(sMessage, "'", "''") & "', '" & sType & "')"
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        Dim pageHandler As Page = HttpContext.Current.CurrentHandler
        'HttpContext.Current.Response
        'Dim MP_Label_PGS_TACH As WebControls.Label
        'Dim MP_Label_ERR_MSG As WebControls.Label

        Try
            con.ConnectionString = sChaineConnexion
            con.Open()
            cmd.Connection = con
            cmd.CommandText = sQuery
            cmd.ExecuteNonQuery()
            If bAFCG_FENE = True Then HttpContext.Current.Response.Write("<body><script type=""text/javascript""> alert('" & Replace(sMessage, "'", "\'") & "');</script></body>")
        Catch ex As Exception
            Dim sw As New StreamWriter("C:\sources\App_Web_LOG\" & sPoste_User & ".log", True)
            sw.WriteLine(DateTime.Now.ToString() & ex.Message)
            sw.WriteLine(DateTime.Now.ToString() & " - Fonction : " & mbFunction.Name & " - Message : " & sMessage)
            sw.Close()
            If sType = "Erreur" Then
                Dim swLOG_ERR As New StreamWriter("C:\sources\App_Web_LOG\" & sPoste_User & "_erreur.log", True)
                swLOG_ERR.WriteLine(DateTime.Now.ToString() & ex.Message)
                swLOG_ERR.WriteLine(DateTime.Now.ToString() & " - Fonction : " & mbFunction.Name & " - Message : " & sMessage)
                swLOG_ERR.Close()
            End If
        Finally
            con.Close()
            con = Nothing
        End Try

        'MP_Label_PGS_TACH = CType(pageHandler.Master.FindControl("Label_PGS_TACH"), WebControls.Label)
        'MP_Label_PGS_TACH.Text = "Fonction : " & mbFunction.Name & " - Message : " & sMessage
        'MP_Label_ERR_MSG = CType(pageHandler.Master.FindControl("Label_ERR_MSG"), WebControls.Label)
        'MP_Label_ERR_MSG.Text = ""
    End Sub

End Class

