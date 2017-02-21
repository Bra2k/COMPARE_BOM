Imports App_Web.Class_SAP_DATA
Imports App_Web.LOG
Imports System.Reflection.MethodBase
Public Class TCBL_OPRT
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Protected Sub TextBox_OF_TextChanged(sender As Object, e As EventArgs) Handles TextBox_OF.TextChanged
        Dim dtAFKO, dtVRESB, dt_OP As New DataTable
        Dim rdt_OP As DataRow
        Try
            'extraction données de l'OF
            dtAFKO = SAP_DATA_READ_AFKO("AUFNR LIKE '%" & TextBox_OF.Text & "'")
            If dtAFKO Is Nothing Then Throw New Exception("L'OF n°" & TextBox_OF.Text & " n'a pas été trouvé dans SAP.")
            'Extraction de la gamme
            dtVRESB = SAP_DATA_READ_VRESB("RSNUM EQ '" & dtAFKO(0)("RSNUM").ToString & "'")
            dt_OP.Columns.Add("Opération", Type.GetType("System.String"))

            For Each rVRESB As DataRow In dtVRESB.Rows 'Select(sFiltre)
                rdt_OP = dt_OP.Select("[Opération] = '" & rVRESB("VORNR").ToString & "'").FirstOrDefault
                If Not rdt_OP Is Nothing Then Continue For
                dt_OP.Rows.Add()
                dt_OP.Rows(dt_OP.Rows.Count - 1)("Opération") = rVRESB("VORNR").ToString
            Next
            DropDownList_OP.DataSource = dt_OP
            DropDownList_OP.DataTextField = "Opération"
            DropDownList_OP.DataValueField = "Opération"
            DropDownList_OP.DataBind()

        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try
    End Sub

    Protected Sub Button_VALI_ENTER_Click(sender As Object, e As EventArgs) Handles Button_VALI_ENTER.Click
        Dim dt As DataTable = SAP_DATA_Z_ORDOPEINFO_GET(TextBox_OF.Text, DropDownList_OP.SelectedValue)

        'extraction de la config dédiée du poste
        'affiche config dédiée
        'extraction nomeclature type pour matériel génériaque
        'saisir matériel génériques

        ' MultiView_ETAP.Views.Add(newViewObject)
    End Sub

    Protected Sub Button_VALI_ENTER_OTLG_Click(sender As Object, e As EventArgs) Handles Button_VALI_ENTER_OTLG.Click

        'vérifier que tout est bon
        'enregistrer

    End Sub


End Class