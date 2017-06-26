Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports App_Web.Class_SQL
Imports App_Web.Class_SAP_DATA

Public Class AJOU_EMPL_MAJ_FICH_BANC_TEST
    Inherits System.Web.UI.Page
    Dim sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=MES_Digital_Factory;Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Button_EMPL_FICH_TEST_Click(sender As Object, e As EventArgs) Handles Button_EMPL_FICH_TEST.Click
        Dim sQuery As String = ""
        Try
            sQuery = "INSERT INTO [dbo].[DTM_REF_PARA] ([NM_CRIT],[NM_PARA],[VAL_PARA],[DT_PARA]) VALUES ('" & TextBox_NU_BANC.Text & "','Emplacement fichier test','" & TextBox_EMPL_FICH_TEST.Text & "', GETDATE())"
            With TextBox_EMPL_FICH_TEST
                If Left(.Text, 2) <> "\\" Then
                    .Text = ""
                    .Focus()
                    Throw New Exception("L'emplacement défini n'a pas un format correct.")
                End If
            End With
            With TextBox_NU_BANC
                If .Text = "" Then
                    .Focus()
                    Throw New Exception("Le n° du banc n'est pas renseigné.")
                End If
            End With
            SQL_REQ_ACT(sQuery, sChaineConnexion)

            'sQuery = "INSERT INTO [dbo].[DTM_REF_PARA] ([NM_CRIT],[NM_PARA],[VAL_PARA],[DT_PARA]) VALUES ('" & TextBox_NU_BANC.Text & "','Emplacement fichier test','" & TextBox_EMPL_FICH_TEST.Text & "', GETDATE())"
            'With TextBox_EMPL_FICH_TEST
            '    If Left(.Text, 2) <> "\\" Then
            '        .Text = ""
            '        .Focus()
            '        Throw New Exception("L'emplacement défini n'a pas un format correct.")
            '    End If
            'End With
            'With TextBox_NU_BANC
            '    If .Text = "" Then
            '        .Focus()
            '        Throw New Exception("Le n° du banc n'est pas renseigné.")
            '    End If
            'End With
            'SQL_REQ_ACT(sQuery, sChaineConnexion)
            GridView_EMPL_FICH_TEST.DataBind()
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message)
            Exit Sub
        End Try

    End Sub

    Protected Sub TextBox_CD_ARTI_ECO_TextChanged(sender As Object, e As EventArgs) Handles TextBox_CD_ARTI_ECO.TextChanged
        Dim dt_MAPL, dt_PLAS, dt_PLPO, dt_CRHD, dt_OP As New DataTable

        Try
            With dt_OP
                .Columns.Add("op", Type.GetType("System.String"))
                .Columns.Add("VORNR", Type.GetType("System.String"))
                dt_MAPL = SAP_DATA_READ_MAPL("MATNR LIKE '" & Trim(TextBox_CD_ARTI_ECO.Text) & "%' AND WERKS EQ 'DI31' AND LOEKZ NE 'X'")
                If dt_MAPL Is Nothing Then
                    Throw New Exception("Pas de gamme pour le code article entré")
                Else
                    dt_PLAS = SAP_DATA_READ_PLAS("PLNNR EQ '" & dt_MAPL(0)("PLNNR").ToString & "' AND PLNAL EQ '" & dt_MAPL(0)("PLNAL").ToString & "' AND LOEKZ NE 'X'")
                    If dt_PLAS Is Nothing Then Throw New Exception("Pas de gamme pour le code article entré")
                    For Each rPLAS As DataRow In dt_PLAS.Rows
                        dt_PLPO = SAP_DATA_READ_PLPO("PLNNR EQ '" & rPLAS("PLNNR").ToString & "' AND PLNKN EQ '" & rPLAS("ZAEHL").ToString & "'")
                        If dt_PLPO Is Nothing Then Continue For
                        'dt_CRHD = SAP_DATA_READ_CRHD("OBJID EQ '" & dt_PLPO(0)("ARBID").ToString & "'")
                        'If dt_CRHD Is Nothing Then Continue For
                        'If Not (Left(dt_CRHD(0)("ARBPL").ToString, 3) = "TFC" Or Left(dt_CRHD(0)("ARBPL").ToString, 3) = "VRT") Then Continue For
                        .Rows.Add()
                        .Rows(.Rows.Count - 1)("op") = Trim(dt_PLPO(0)("LTXA1").ToString) & " (OP:" & Convert.ToDecimal(dt_PLPO(0)("VORNR")).ToString & ")"
                        .Rows(.Rows.Count - 1)("VORNR") = dt_PLPO(0)("VORNR").ToString
                    Next
                    .DefaultView.Sort = "VORNR ASC"
                    dt_OP = .DefaultView.ToTable
                End If
            End With
            With DropDownList_OP
                .DataSource = dt_OP
                .DataTextField = "op" '"VORNR"
                .DataValueField = "op"
                .DataBind()
            End With
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message)
            Exit Sub
        End Try
    End Sub
End Class