Imports System.Windows.Forms
Imports System.IO
Imports System.Reflection
Imports System.Web.UI.WebControls
Imports App_Web.SiteMaster
Imports System.Security.Principal
Imports System.Data.SqlClient
Imports System
Imports System.Web
Imports System.Web.UI
Imports Microsoft.VisualBasic.Strings


Public Class LOG
    ' Inherits MasterPage
    Public Shared Sub LOG_Msg(mbFunction As MethodBase, sMessage As String)

        Dim ip As String = System.Web.HttpContext.Current.Request.UserHostAddress
        Dim sPoste_User As String = $"{System.Net.Dns.GetHostEntry(ip).HostName()} - {Replace(Replace(System.Web.HttpContext.Current.User.Identity.Name, Environment.UserDomainName, ""), "\", "")}"

        Dim sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=APP_WEB_ECO;Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
        Dim sQuery As String = $"INSERT INTO [dbo].[DTM_LOG_APP_WEB]([NM_POST], [DT_LOG], [NM_FONC], [NM_MSG], [NM_CTCT]) VALUES ('{sPoste_User}', GETDATE(), '{mbFunction.Name}', '{Replace(sMessage, "'", "''")}', 'Information')"

        Try
            Using con As New SqlConnection
                con.ConnectionString = sChaineConnexion
                con.Open()
                Using cmd As New SqlCommand
                    cmd.Connection = con
                    cmd.CommandText = sQuery
                    cmd.ExecuteNonQuery()
                End Using
                con.Close()
            End Using
        Catch ex As Exception
            Using sw As New StreamWriter($"C:\sources\App_Web_LOG\{sPoste_User}.log", True)
                sw.WriteLine($"{DateTime.Now.ToString()} - {ex.Message}")
                sw.WriteLine($"{DateTime.Now.ToString()} - Fonction : {mbFunction.Name} - Message : {sMessage}")
                sw.Close()
            End Using

        End Try
        If My.Computer.Name = "CEDB03" Then
            Using pageHandler As Page = HttpContext.Current.CurrentHandler
                Using MP_Label_PGS_TACH As WebControls.Label = CType(pageHandler.Master.FindControl("Label_PGS_TACH"), WebControls.Label)
                    MP_Label_PGS_TACH.Text = $"Fonction : {mbFunction.Name} - Message : {sMessage}"
                End Using
                Using MP_Label_ERR_MSG As WebControls.Label = CType(pageHandler.Master.FindControl("Label_ERR_MSG"), WebControls.Label)
                    MP_Label_ERR_MSG.Text = ""
                End Using
            End Using
        End If

    End Sub
    Public Shared Sub LOG_Erreur(mbFunction As MethodBase, sMessage As String)

        Dim ip As String = System.Web.HttpContext.Current.Request.UserHostAddress
        Dim sPoste_User As String = $"{System.Net.Dns.GetHostEntry(ip).HostName()} - {Replace(Replace(System.Web.HttpContext.Current.User.Identity.Name, Environment.UserDomainName, ""), "\", "")}"

        Dim sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=APP_WEB_ECO;Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
        Dim sQuery As String = $"INSERT INTO [dbo].[DTM_LOG_APP_WEB]([NM_POST], [DT_LOG], [NM_FONC], [NM_MSG], [NM_CTCT]) VALUES ('{sPoste_User}', GETDATE(), '{mbFunction.Name}', '{Replace(sMessage, "'", "''")}', 'Erreur')"
        'Dim con As New SqlConnection
        'Dim cmd As New SqlCommand

        'Dim pageHandler As Page = HttpContext.Current.CurrentHandler
        'Dim MP_Label_ERR_MSG As WebControls.Label

        Try
            'con.ConnectionString = sChaineConnexion
            'con.Open()
            'cmd.Connection = con
            'cmd.CommandText = sQuery
            'cmd.ExecuteNonQuery()
            Using con As New SqlConnection
                con.ConnectionString = sChaineConnexion
                con.Open()
                Using cmd As New SqlCommand
                    cmd.Connection = con
                    cmd.CommandText = sQuery
                    cmd.ExecuteNonQuery()
                End Using
                con.Close()
            End Using
        Catch ex As Exception
            Using swLOG As New StreamWriter($"C:\sources\App_Web_LOG\{sPoste_User}.log", True)
                swLOG.WriteLine($"{DateTime.Now.ToString()} - {ex.Message}")
                swLOG.WriteLine($"{DateTime.Now.ToString()} - Erreur à la fonction : {mbFunction.Name} - Message d'erreur : {sMessage}")
                swLOG.Close()
            End Using
            Using swLOG_ERR As New StreamWriter($"C:\sources\App_Web_LOG\{sPoste_User}_erreur.log", True)
                swLOG_ERR.WriteLine($"{DateTime.Now.ToString()}{ex.Message}")
                swLOG_ERR.WriteLine($"{DateTime.Now.ToString()} - Fonction : {mbFunction.Name} - Message : {sMessage}")
                swLOG_ERR.Close()
            End Using
        Finally
            'cmd.Dispose()
            'con.Close()
            'con.Dispose()
            'con = Nothing

        End Try
        If My.Computer.Name = "CEDB03" Then
            Using pageHandler As Page = HttpContext.Current.CurrentHandler
                Using MP_Label_ERR_MSG = CType(pageHandler.Master.FindControl("Label_ERR_MSG"), WebControls.Label)
                    MP_Label_ERR_MSG.Text = $"Erreur à la fonction : {mbFunction.Name} - Message d'erreur : {sMessage}"
                End Using
            End Using
        End If

    End Sub

    Public Shared Sub LOG_MESS_UTLS(mbFunction As MethodBase, sMessage As String, Optional sType As String = "Information", Optional bAFCG_FENE As Boolean = True)

        Dim ip As String = System.Web.HttpContext.Current.Request.UserHostAddress
        Dim sPoste_User As String = $"{System.Net.Dns.GetHostEntry(ip).HostName()} - {Replace(Replace(System.Web.HttpContext.Current.User.Identity.Name, Environment.UserDomainName, ""), "\", "")}"

        Dim sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=APP_WEB_ECO;Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
        Dim sQuery As String = $"INSERT INTO [dbo].[DTM_LOG_APP_WEB]([NM_POST], [DT_LOG], [NM_FONC], [NM_MSG], [NM_CTCT]) VALUES ('{sPoste_User}', GETDATE(), '{mbFunction.Name}', '{Replace(sMessage, "'", "''")}', '{sType}')"
        'Dim con As New SqlConnection
        'Dim cmd As New SqlCommand
        'Dim pageHandler As Page = HttpContext.Current.CurrentHandler
        'pageHandler.Dispose()
        'HttpContext.Current.Response
        'Dim MP_Label_PGS_TACH As WebControls.Label
        'Dim MP_Label_ERR_MSG As WebControls.Label

        Try
            'Using db = New APP_WEB_ECOEntities2()
            'Dim Newlog = New DTM_LOG_APP_WEB() With {.NM_POST = sPoste_User, .DT_LOG = "000", .NM_FONC = mbFunction.Name, .NM_MSG = Replace(sMessage, "'", "''"), .NM_CTCT = sType}
            'db.DTM_LOG_APP_WEB.Add(Newlog)
            'db.SaveChanges()
            'End Using

            Using con As New SqlConnection
                con.ConnectionString = sChaineConnexion
                con.Open()
                Using cmd As New SqlCommand
                    cmd.Connection = con
                    cmd.CommandText = sQuery
                    cmd.ExecuteNonQuery()
                End Using
                con.Close()
            End Using
            If bAFCG_FENE = True Then HttpContext.Current.Response.Write($"<body><script type=""text/javascript""> alert('{Replace(sMessage, "'", "\'")}');</script></body>")
        Catch ex As Exception
            Using sw As New StreamWriter($"C:\sources\App_Web_LOG\{sPoste_User}.log", True)
                sw.WriteLine($"{DateTime.Now.ToString()} - {ex.Message}")
                sw.WriteLine($"{DateTime.Now.ToString()} - Fonction : {mbFunction.Name} - Message : {sMessage}")
                sw.Close()
            End Using
            If sType = "Erreur" Then
                Using swLOG_ERR As New StreamWriter($"C:\sources\App_Web_LOG\{sPoste_User}_erreur.log", True)
                    swLOG_ERR.WriteLine($"{DateTime.Now.ToString()} - {ex.Message}")
                    swLOG_ERR.WriteLine($"{DateTime.Now.ToString()} - Fonction : {mbFunction.Name} - Message : {sMessage}")
                    swLOG_ERR.Close()
                End Using
            End If
            'Finally
            '    'pageHandler.Dispose()
            '    cmd.Dispose()
            '    con.Close()
            '    con.Dispose()
            '    con = Nothing
        End Try
        If My.Computer.Name = "CEDB03" Then
            Using pageHandler As Page = HttpContext.Current.CurrentHandler
                If sType = "Erreur" Then
                    Using MP_Label_ERR_MSG As WebControls.Label = CType(pageHandler.Master.FindControl("Label_ERR_MSG"), WebControls.Label)
                        MP_Label_ERR_MSG.Text = $"Erreur à la fonction : {mbFunction.Name} - Message d'erreur : {sMessage}"
                    End Using
                Else
                    Using MP_Label_PGS_TACH As WebControls.Label = CType(pageHandler.Master.FindControl("Label_PGS_TACH"), WebControls.Label)
                        MP_Label_PGS_TACH.Text = $"Fonction : {mbFunction.Name} - Message : {sMessage}"
                    End Using
                End If
            End Using
        End If
    End Sub

End Class

