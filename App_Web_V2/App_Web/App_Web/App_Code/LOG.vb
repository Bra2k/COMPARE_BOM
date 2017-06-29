Imports System.IO
Imports System.Reflection
Imports System.Data.SqlClient
Imports System
Imports System.Web
Imports System.Web.UI
Imports Microsoft.VisualBasic.Strings


Public Class LOG
    ' Inherits MasterPage
    'Private _Fonction As MethodBase
    'Public Property Fonction() As String
    '    Get
    '        Return _Fonction.Name
    '    End Get
    '    Set(ByVal value As String)
    '        _Fonction.Name = value
    '    End Set
    'End Property

    Public Shared Sub LOG_Msg(mbFunction As MethodBase, sMessage As String)

        Dim ip As String = System.Web.HttpContext.Current.Request.UserHostAddress
        Dim sPoste_User As String = $"{System.Net.Dns.GetHostEntry(ip).HostName()} - {Replace(Replace(System.Web.HttpContext.Current.User.Identity.Name, Environment.UserDomainName, ""), "\", "")}"
        'Dim sQuery As String = $"INSERT INTO [dbo].[DTM_LOG_APP_WEB]([NM_POST], [DT_LOG], [NM_FONC], [NM_MSG], [NM_CTCT]) VALUES ('{sPoste_User}', GETDATE(), '{mbFunction.Name}', '{Replace(sMessage, "'", "''")}', 'Information')"

        Try
            Using db As New APP_WEB_ECOEntities
                Dim log As New DTM_LOG_APP_WEB
                log.NM_POST = sPoste_User
                log.DT_LOG = Now
                log.NM_MSG = sMessage
                log.NM_FONC = mbFunction.Name
                log.NM_CTCT = "Information"
                db.DTM_LOG_APP_WEB.Add(log)
                db.SaveChanges()
            End Using

            'Using con As New SqlConnection
            '    con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("APP_WEB_ECOConnectionString").ToString
            '    con.Open()
            '    Using cmd As New SqlCommand
            '        cmd.Connection = con
            '        cmd.CommandText = sQuery
            '        cmd.ExecuteNonQuery()
            '    End Using
            '    con.Close()
            'End Using
        Catch ex As Exception
            Using sw As New StreamWriter($"C:\sources\App_Web_LOG\{sPoste_User}.log", True)
                sw.WriteLine($"{DateTime.Now.ToString} - {ex.Message}")
                sw.WriteLine($"{DateTime.Now.ToString} - Fonction : {mbFunction.Name} - Message : {sMessage}")
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
        'Dim sQuery As String = $"INSERT INTO [dbo].[DTM_LOG_APP_WEB]([NM_POST], [DT_LOG], [NM_FONC], [NM_MSG], [NM_CTCT]) VALUES ('{sPoste_User}', GETDATE(), '{mbFunction.Name}', '{Replace(sMessage, "'", "''")}', 'Erreur')"

        Try
            'Using con As New SqlConnection
            '    con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("APP_WEB_ECOConnectionString").ToString
            '    con.Open()
            '    Using cmd As New SqlCommand
            '        cmd.Connection = con
            '        cmd.CommandText = sQuery
            '        cmd.ExecuteNonQuery()
            '    End Using
            '    con.Close()
            'End Using

            Using db As New APP_WEB_ECOEntities
                Dim log As New DTM_LOG_APP_WEB
                log.NM_POST = sPoste_User
                log.DT_LOG = Now
                log.NM_MSG = sMessage
                log.NM_FONC = mbFunction.Name
                log.NM_CTCT = "Erreur"
                db.DTM_LOG_APP_WEB.Add(log)
                db.SaveChanges()
            End Using
        Catch ex As Exception
            Using swLOG As New StreamWriter($"C:\sources\App_Web_LOG\{sPoste_User}.log", True)
                swLOG.WriteLine($"{DateTime.Now.ToString} - {ex.Message}")
                swLOG.WriteLine($"{DateTime.Now.ToString} - Erreur à la fonction : {mbFunction.Name} - Message d'erreur : {sMessage}")
                swLOG.Close()
            End Using
            Using swLOG_ERR As New StreamWriter($"C:\sources\App_Web_LOG\{sPoste_User}_erreur.log", True)
                swLOG_ERR.WriteLine($"{DateTime.Now.ToString}{ex.Message}")
                swLOG_ERR.WriteLine($"{DateTime.Now.ToString} - Fonction : {mbFunction.Name} - Message : {sMessage}")
                swLOG_ERR.Close()
            End Using
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
        'Dim sQuery As String = $"INSERT INTO [dbo].[DTM_LOG_APP_WEB]([NM_POST], [DT_LOG], [NM_FONC], [NM_MSG], [NM_CTCT]) VALUES ('{sPoste_User}', GETDATE(), '{mbFunction.Name}', '{Replace(sMessage, "'", "''")}', '{sType}')"

        Try
            'Using con As New SqlConnection
            '    con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("APP_WEB_ECOConnectionString").ToString
            '    con.Open()
            '    Using cmd As New SqlCommand
            '        cmd.Connection = con
            '        cmd.CommandText = sQuery
            '        cmd.ExecuteNonQuery()
            '    End Using
            '    con.Close()
            'End Using
            Using db As New APP_WEB_ECOEntities
                Dim log As New DTM_LOG_APP_WEB
                log.NM_POST = sPoste_User
                log.DT_LOG = Now
                log.NM_MSG = sMessage
                log.NM_FONC = mbFunction.Name
                log.NM_CTCT = sType
                db.DTM_LOG_APP_WEB.Add(log)
                db.SaveChanges()
            End Using
            If bAFCG_FENE = True Then HttpContext.Current.Response.Write($"<body><script type=""text/javascript""> alert('{Replace(sMessage, "'", "\'")}');</script></body>")
        Catch ex As Exception
            Using sw As New StreamWriter($"C:\sources\App_Web_LOG\{sPoste_User}.log", True)
                sw.WriteLine($"{DateTime.Now.ToString} - {ex.Message}")
                sw.WriteLine($"{DateTime.Now.ToString} - Fonction : {mbFunction.Name} - Message : {sMessage}")
                sw.Close()
            End Using
            If sType = "Erreur" Then
                Using swLOG_ERR As New StreamWriter($"C:\sources\App_Web_LOG\{sPoste_User}_erreur.log", True)
                    swLOG_ERR.WriteLine($"{DateTime.Now.ToString} - {ex.Message}")
                    swLOG_ERR.WriteLine($"{DateTime.Now.ToString} - Fonction : {mbFunction.Name} - Message : {sMessage}")
                    swLOG_ERR.Close()
                End Using
            End If
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

