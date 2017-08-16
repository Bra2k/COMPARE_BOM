Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports App_Web.Class_SAP_DATA
Imports App_Web.Class_SQL

Class AMNS_DROI_ALMS
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            DropDownList_OPRT.ClearSelection()
            If Session("displayname") = "" Then
                Context.GetOwinContext().Authentication.SignOut(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie)
            Else
                If App_Web.Class_COMM_APP_WEB.COMM_APP_WEB_GET_DROI_PAGE(HttpContext.Current.CurrentHandler.ToString, Session("department"), Session("User_Name")) = False Then Response.Redirect("~/PagesMembres/RDRC_PAGE_MEMB.aspx")
            End If

        End If
    End Sub

    Protected Sub DropDownList_OPRT_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList_OPRT.SelectedIndexChanged
        Try
            MultiView_AMNS_DROI_ALMS.SetActiveView(View_MDFC_DROI)

            'afficher le nom de l'opérateur
            Using dt_matr = SAP_DATA_READ_PA0002($"PERNR LIKE '%{DropDownList_OPRT.SelectedValue}'")
                If dt_matr Is Nothing Then Throw New Exception($"Pas de nom et prénom trouvés pour le matricule {DropDownList_OPRT.SelectedValue}")
                Label_NM_OPRT.Text = $"{Trim(dt_matr(0)("NACHN").ToString)} {Trim(dt_matr(0)("VORNA").ToString)}"
            End Using
            Dim squery = $"SELECT [LB_INIT]
      ,[ID_HBLT_ALMS]
      ,[BL_ATVT]
      ,[DT_ATVT]
      ,[ID_MTCL_ECO]
  FROM [dbo].[DTM_REF_OPRT]
 WHERE [ID_MTCL_ECO] = {DropDownList_OPRT.SelectedValue}"
            Using dt = SQL_SELE_TO_DT(squery, CS_ALMS_PROD_PRD)
                If dt Is Nothing Then Throw New Exception($"L'opérateur au matricule {DropDownList_OPRT.SelectedValue} n'a pas été trouvé dans la base.")
                Label_INIT.Text = dt(0)("LB_INIT").ToString
                TextBox_HBLT.Text = dt(0)("ID_HBLT_ALMS").ToString
                CheckBox_ATVT.Checked = Convert.ToBoolean(dt(0)("BL_ATVT"))
            End Using
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "alert")
            Exit Sub
        End Try
    End Sub

    Protected Sub Button_AJOU_OPRT_Click(sender As Object, e As EventArgs) Handles Button_AJOU_OPRT.Click
        MultiView_AMNS_DROI_ALMS.SetActiveView(View_AJOU_OPRT)
    End Sub

    Protected Sub Button_VALI_Click(sender As Object, e As EventArgs) Handles Button_VALI.Click
        Try
            Dim bl_atvt = "0"
            If CheckBox_ATVT.Checked = True Then bl_atvt = "1"
            Dim squery = $"UPDATE [dbo].[DTM_REF_OPRT]
   SET [LB_INIT] = '{Label_INIT.Text}'
      ,[ID_HBLT_ALMS] = {TextBox_HBLT.Text}
      ,[BL_ATVT] = {bl_atvt}
      ,[DT_ATVT] = '{Now}'
 WHERE [ID_MTCL_ECO] = {DropDownList_OPRT.SelectedValue}"
            SQL_REQ_ACT(squery, CS_ALMS_PROD_PRD)
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "alert")
            Exit Sub
        End Try
    End Sub

    Protected Sub TextBox_MTCL_TextChanged(sender As Object, e As EventArgs) Handles TextBox_MTCL.TextChanged
        Try
            Using dt_matr = SAP_DATA_READ_PA0002($"PERNR LIKE '%{TextBox_MTCL.Text}'")
                If dt_matr Is Nothing Then Throw New Exception($"Pas de nom et prénom trouvés pour le matricule {TextBox_MTCL.Text}")
                Label_NM_OPRT.Text = $"{Trim(dt_matr(0)("NACHN").ToString)} {Trim(dt_matr(0)("VORNA").ToString)}"
                Dim squery = $"SELECT [LB_INIT]
      ,[ID_HBLT_ALMS]
      ,[BL_ATVT]
      ,[DT_ATVT]
      ,[ID_MTCL_ECO]
  FROM [dbo].[DTM_REF_OPRT]
 WHERE [ID_MTCL_ECO] = {TextBox_MTCL.Text}"
                Using dt = SQL_SELE_TO_DT(squery, CS_ALMS_PROD_PRD)
                    If Not dt Is Nothing Then Throw New Exception($"L'opérateur au matricule {TextBox_MTCL.Text} est déjà créé dans la base.")
                End Using
                squery = $"INSERT INTO [dbo].[DTM_REF_OPRT]
           ([ID_OPRT]
           ,[LB_INIT]
           ,[ID_HBLT_ALMS]
           ,[BL_ATVT]
           ,[DT_ATVT]
           ,[ID_MTCL_ECO])
SELECT MAX([ID_OPRT]) + 1
,'{Left(Trim(dt_matr(0)("VORNA").ToString), 1)}{Left(Trim(dt_matr(0)("NACHN").ToString), 1)}'
,0
,0
,GETDATE()
,{TextBox_MTCL.Text}
  FROM [dbo].[DTM_REF_OPRT]"
                SQL_REQ_ACT(squery, CS_ALMS_PROD_PRD)
                DropDownList_OPRT.DataBind()
                DropDownList_OPRT.SelectedValue = TextBox_MTCL.Text
                MultiView_AMNS_DROI_ALMS.SetActiveView(View_MDFC_DROI)
                squery = $"SELECT [LB_INIT]
      ,[ID_HBLT_ALMS]
      ,[BL_ATVT]
      ,[DT_ATVT]
      ,[ID_MTCL_ECO]
  FROM [dbo].[DTM_REF_OPRT]
 WHERE [ID_MTCL_ECO] = {DropDownList_OPRT.SelectedValue}"
                Using dt = SQL_SELE_TO_DT(squery, CS_ALMS_PROD_PRD)
                    If dt Is Nothing Then Throw New Exception($"L'opérateur au matricule {DropDownList_OPRT.SelectedValue} n'a pas été trouvé dans la base.")
                    Label_INIT.Text = dt(0)("LB_INIT").ToString
                    TextBox_HBLT.Text = dt(0)("ID_HBLT_ALMS").ToString
                    CheckBox_ATVT.Checked = Convert.ToBoolean(dt(0)("BL_ATVT"))
                End Using
            End Using



        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "alert")
            DropDownList_OPRT.ClearSelection()
            MultiView_AMNS_DROI_ALMS.SetActiveView(View_MDFC_DROI)
            Exit Sub
        End Try
    End Sub
End Class