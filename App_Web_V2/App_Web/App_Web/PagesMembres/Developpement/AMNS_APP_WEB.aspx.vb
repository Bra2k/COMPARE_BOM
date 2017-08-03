Imports System.IO
Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports App_Web.Class_COMM_APP_WEB
Imports App_Web.Class_SQL

Public Class AMNS_APP_WEB
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("displayname") = "" Then
                Context.GetOwinContext().Authentication.SignOut(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie)
            Else
                If App_Web.Class_COMM_APP_WEB.COMM_APP_WEB_GET_DROI_PAGE(HttpContext.Current.CurrentHandler.ToString, Session("department"), Session("User_Name")) = False Then Response.Redirect("~/PagesMembres/RDRC_PAGE_MEMB.aspx")
            End If
        End If
    End Sub

    Protected Sub TextBox_LOG_TextChanged(sender As Object, e As EventArgs) Handles TextBox_LOG.TextChanged
        Try
            Dim info = New DirectoryInfo(Server.MapPath($"~/PagesMembres/"))
            Dim listeDesDossiers = info.GetDirectories()
            Using dt As New DataTable
                dt.Columns.Add("Page", Type.GetType("System.String"))
                dt.Columns.Add("URL", Type.GetType("System.String"))
                For Each dossier In listeDesDossiers
                    Dim subinfo = New DirectoryInfo(Server.MapPath($"~/PagesMembres/{dossier.Name}"))
                    Dim listeDesFichiers = subinfo.GetFiles
                    For Each fichier In listeDesFichiers
                        dt.Rows.Add()
                        dt.Rows(dt.Rows.Count - 1)("Page") = $"ASP.pagesmembres_{LCase(dossier.Name)}_{Replace(LCase(fichier.Name), ".", "_")}"
                        dt.Rows(dt.Rows.Count - 1)("URL") = $"{dossier.Name}/{fichier.Name}"
                    Next
                    Dim listeDesSousDossiers = subinfo.GetDirectories()
                    For Each sousdossier In listeDesSousDossiers
                        Dim subsubinfo = New DirectoryInfo(Server.MapPath($"~/PagesMembres/{dossier.Name}/{sousdossier.Name}"))
                        Dim listeDesSousFichiers = subsubinfo.GetFiles
                        For Each fichier In listeDesSousFichiers
                            dt.Rows.Add()
                            dt.Rows(dt.Rows.Count - 1)("Page") = $"ASP.pagesmembres_{LCase(dossier.Name)}_{LCase(sousdossier.Name)}_{Replace(LCase(fichier.Name), ".", "_")}"
                            dt.Rows(dt.Rows.Count - 1)("URL") = $"{dossier.Name}/{sousdossier.Name}/{fichier.Name}"
                        Next
                    Next
                Next
                CheckBoxList_PAGE.DataSource = dt
                CheckBoxList_PAGE.DataTextField = "URL"
                CheckBoxList_PAGE.DataValueField = "Page"
                CheckBoxList_PAGE.DataBind()
            End Using
            CheckBoxList_PAGE.ClearSelection()
            Using dt = SQL_SELE_TO_DT($"SELECT [ID_SERV_SESS],[NM_URL_PAGE] FROM [dbo].[DTM_REF_AFCG_PAGE_DEFA] WHERE [ID_SERV_SESS] = '{TextBox_LOG.Text}'", CS_APP_WEB_ECO)
                For Each rdt In dt.Rows
                    CheckBoxList_PAGE.Items.FindByValue(rdt("NM_URL_PAGE").ToString).Selected = True
                Next
            End Using
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "error")
            Exit Sub
        End Try
    End Sub

    Protected Sub Button_VALI_Click(sender As Object, e As EventArgs) Handles Button_VALI.Click
        Try
            For Each item In CheckBoxList_PAGE.Items
                If item.selected = True Then
                    Using dt = SQL_SELE_TO_DT($"SELECT [ID_SERV_SESS],[NM_URL_PAGE] FROM [dbo].[DTM_REF_AFCG_PAGE_DEFA] WHERE [ID_SERV_SESS] = '{TextBox_LOG.Text}' AND [NM_URL_PAGE] = '{item.value}'", CS_APP_WEB_ECO)
                        If dt Is Nothing Then SQL_REQ_ACT($"INSERT INTO [dbo].[DTM_REF_AFCG_PAGE_DEFA] ([ID_SERV_SESS], [NM_URL_PAGE]) VALUES ('{TextBox_LOG.Text}','{item.value}')", CS_APP_WEB_ECO)
                    End Using
                Else
                    SQL_REQ_ACT($"DELETE FROM [dbo].[DTM_REF_AFCG_PAGE_DEFA] WHERE [ID_SERV_SESS] = '{TextBox_LOG.Text}' AND [NM_URL_PAGE] = '{item.value}'", CS_APP_WEB_ECO)
                End If
            Next
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "error")
            Exit Sub
        End Try
    End Sub
End Class