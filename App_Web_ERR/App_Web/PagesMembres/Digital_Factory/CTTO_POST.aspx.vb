Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports App_Web.Class_SQL
Imports App_Web.Class_SAP_DATA
Public Class CTTO_POST
    Inherits System.Web.UI.Page
    Dim sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=" & Replace(Replace(My.Computer.Name, "CEDB03", "MES_Digital_Factory_DEV"), "CEAPP03", "MES_Digital_Factory") & ";Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=7200;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'MultiView_SAIS_CTTO.SetActiveView(View_NMCT_TPLG)
    End Sub

    Protected Sub Button_VALI_MTRE_Click(sender As Object, e As EventArgs) Handles Button_VALI_MTRE.Click
        Dim sQuery As String = ""
        Try
            'Type matériel
            sQuery = "INSERT INTO [dbo].[DTM_REF_PARA]([NM_CRIT],[NM_PARA],[VAL_PARA],[DT_PARA])
                           VALUES ('" & TextBox_ID_MTRE.Text & "','Type matériel','" & DropDownList_TYPE_MTRE.SelectedValue & "',GETDATE())"
            SQL_REQ_ACT(sQuery, sChaineConnexion)
            'Référence matériel
            sQuery = "INSERT INTO [dbo].[DTM_REF_PARA]([NM_CRIT],[NM_PARA],[VAL_PARA],[DT_PARA])
                           VALUES ('" & TextBox_ID_MTRE.Text & "','Référence matériel','" & TextBox_RFRC_MTRE.Text & "',GETDATE())"
            SQL_REQ_ACT(sQuery, sChaineConnexion)
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try

    End Sub

    Protected Sub TextBox_ID_MTRE_TextChanged(sender As Object, e As EventArgs) Handles TextBox_ID_MTRE.TextChanged
        Dim sQuery As String = ""
        Dim dt As New DataTable
        Try
            sQuery = "SELECT [NM_TYPE_MTRE]
                            ,[NM_RFRC_MTRE]
                        FROM [dbo].[V_POST_LIST_MTRE]
                       WHERE ID_MTRE = '" & TextBox_ID_MTRE.Text & "'"
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            If Not dt Is Nothing Then
                DropDownList_TYPE_MTRE.SelectedValue = dt(0)("NM_TYPE_MTRE").ToString
                TextBox_RFRC_MTRE.Text = dt(0)("NM_RFRC_MTRE").ToString
            End If
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try

    End Sub

    Protected Sub Button_AJOU_MTRE_Click(sender As Object, e As EventArgs) Handles Button_AJOU_MTRE.Click
        DropDownList_TYPE_MTRE_TPLG.Enabled = True
        Button_VALI_MTRE_TPLG.Enabled = True
    End Sub

    Protected Sub Button_VALI_MTRE_TPLG_Click(sender As Object, e As EventArgs) Handles Button_VALI_MTRE_TPLG.Click
        Dim sQuery As String = ""
        Try
            'Référence matériel
            sQuery = "INSERT INTO [dbo].[DTM_REF_PARA]([NM_CRIT],[NM_PARA],[VAL_PARA],[DT_PARA])
                           VALUES ('" & Label_CD_ARTI_ECO.Text & "|" & Label_NU_OPRT.Text & "','Type matériel associé','" & DropDownList_TYPE_MTRE_TPLG.SelectedValue & "',GETDATE())"
            SQL_REQ_ACT(sQuery, sChaineConnexion)
            'Poste dédié
            If CheckBox_ENAB_SELE_ID_MTRE.Checked = True Then
                sQuery = "INSERT INTO [dbo].[DTM_REF_PARA]([NM_CRIT],[NM_PARA],[VAL_PARA],[DT_PARA])
                               VALUES ('" & DropDownList_ID_MTRE_DEDI.SelectedValue & "','Poste dédié','" & Label_NU_POST.Text & "',GETDATE())"
                SQL_REQ_ACT(sQuery, sChaineConnexion)
            End If
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        Finally
            GridView_NMCT_TPLG.DataBind()
            DropDownList_TYPE_MTRE_TPLG.Enabled = False
            Button_VALI_MTRE_TPLG.Enabled = False
            DropDownList_ID_MTRE_DEDI.Enabled = False
            CheckBox_ENAB_SELE_ID_MTRE.Checked = False
        End Try
    End Sub

    Protected Sub TextBox_CD_ARTI_ECO_TextChanged(sender As Object, e As EventArgs) Handles TextBox_CD_ARTI_ECO.TextChanged
        Dim dt_op, dt_MAPL, dt_PLAS, dt_PLPO, dt_CRHD As New DataTable
        Try
            With dt_OP
                .Columns.Add("op", Type.GetType("System.String"))
                .Columns.Add("VORNR", Type.GetType("System.Int32"))
                dt_MAPL = SAP_DATA_READ_MAPL("MATNR LIKE '" & Trim(TextBox_CD_ARTI_ECO.Text) & "%' AND WERKS EQ 'DI31' AND LOEKZ NE 'X'")
                If dt_MAPL Is Nothing Then Throw New Exception("Pas de gamme pour le code article entré")
                dt_PLAS = SAP_DATA_READ_PLAS("PLNNR EQ '" & dt_MAPL(0)("PLNNR").ToString & "' AND PLNAL EQ '" & dt_MAPL(0)("PLNAL").ToString & "' AND LOEKZ NE 'X'")
                If dt_PLAS Is Nothing Then Throw New Exception("Pas de gamme pour le code article entré")
                For Each rPLAS As DataRow In dt_PLAS.Rows
                    dt_PLPO = SAP_DATA_READ_PLPO("PLNNR EQ '" & rPLAS("PLNNR").ToString & "' AND PLNKN EQ '" & rPLAS("ZAEHL").ToString & "'")
                    If dt_PLPO Is Nothing Then Continue For
                    dt_CRHD = SAP_DATA_READ_CRHD("OBJID EQ '" & dt_PLPO(0)("ARBID").ToString & "'")
                    If dt_CRHD Is Nothing Then Continue For
                    .Rows.Add()
                    .Rows(.Rows.Count - 1)("op") = Trim(dt_PLPO(0)("LTXA1").ToString) & " (OP:" & Convert.ToDecimal(dt_PLPO(0)("VORNR")).ToString & ")"
                    .Rows(.Rows.Count - 1)("VORNR") = Convert.ToDecimal(dt_PLPO(0)("VORNR")).ToString
                Next
                .DefaultView.Sort = "VORNR ASC"
                dt_op = .DefaultView.ToTable
            End With
            With DropDownList_NU_OPRT
                .DataSource = dt_op
                .DataTextField = "op"
                .DataValueField = "VORNR"
                .DataBind()
            End With
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try
    End Sub

    Protected Sub Button_VALI_SAIS_Click(sender As Object, e As EventArgs) Handles Button_VALI_SAIS.Click
        Dim sQuery As String = ""
        Dim dt As New DataTable
        Try
            sQuery = "SELECT CD_ARTI AS ID_POST, 
                            [Article poste] AS CD_ARTI_ECO_POST, 
                            [Opération poste] AS NU_OP_POST
                       FROM (SELECT     CD_ARTI, NM_PARA, VAL_PARA
                               FROM         dbo.V_DER_DTM_REF_PARA
                              WHERE     (CD_ARTI LIKE N'POSTE_%') AND (NM_PARA = N'Article poste' OR NM_PARA = N'Opération poste')) as a
					  pivot (max(VAL_PARA) for NM_PARA in ([Article poste], [Opération poste])) as pt
                      WHERE [Article poste] = '" & TextBox_CD_ARTI_ECO.Text & "' AND [Opération poste] = '" & DropDownList_NU_OPRT.SelectedValue & "'"
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)
            If dt Is Nothing Then
                Label_NU_POST.Text = "POSTE_" & SQL_REQ_ACT_RET_IDTT("INSERT INTO [dbo].[DTM_REF_ID]([NM_CHAM]) VALUES(1)", sChaineConnexion)
                sQuery = "INSERT INTO [dbo].[DTM_REF_PARA]([NM_CRIT],[NM_PARA],[VAL_PARA],[DT_PARA])
                           VALUES ('" & Label_NU_POST.Text & "','Article poste','" & TextBox_CD_ARTI_ECO.Text & "',GETDATE())"
                SQL_REQ_ACT(sQuery, sChaineConnexion)

                sQuery = "INSERT INTO [dbo].[DTM_REF_PARA]([NM_CRIT],[NM_PARA],[VAL_PARA],[DT_PARA])
                           VALUES ('" & Label_NU_POST.Text & "','Opération poste','" & DropDownList_NU_OPRT.SelectedValue & "',GETDATE())"
                SQL_REQ_ACT(sQuery, sChaineConnexion)
            Else
                Label_NU_POST.Text = dt(0)("ID_POST").ToString
            End If
            Label_CD_ARTI_ECO.Text = TextBox_CD_ARTI_ECO.Text
            Label_NU_OPRT.Text = DropDownList_NU_OPRT.SelectedValue
            MultiView_SAIS_CTTO.SetActiveView(View_NMCT_TPLG)
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try
    End Sub


    Protected Sub CheckBox_ENAB_SELE_ID_MTRE_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox_ENAB_SELE_ID_MTRE.CheckedChanged
        DropDownList_ID_MTRE_DEDI.Enabled = CheckBox_ENAB_SELE_ID_MTRE.Checked
    End Sub

    Protected Sub Button_AJOU_MTRE_CREE_Click(sender As Object, e As EventArgs) Handles Button_AJOU_MTRE_CREE.Click
        MultiView_SAIS_CTTO.SetActiveView(View_SAIS_NEW_MTRE)
    End Sub

    Protected Sub Button_SAIS_MTRE_Click(sender As Object, e As EventArgs) Handles Button_SAIS_MTRE.Click
        MultiView_SAIS_CTTO.SetActiveView(View_SAIS_NEW_MTRE)
    End Sub
End Class