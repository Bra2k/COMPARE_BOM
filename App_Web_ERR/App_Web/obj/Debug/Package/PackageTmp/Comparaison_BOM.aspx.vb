'Imports App_Web
Imports App_Web.App_Code.SAP
'Using App_Web
Public Class Comparaison_BOM
    Inherits Page

    Dim oConnSAP As Object

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load



    End Sub



    Protected Sub Button_Importer_Click(sender As Object, e As EventArgs) Handles Button_Importer.Click

        oConnSAP = SAP_Connexion("so_tech2", "test12")
        'Dim sTableSAP As String(,) = SAP_ReadTable(oConnSAP, "PA0002", "|", "", "PERNR NACHN VORNA")
        Dim sTableSAP As String(,) = SAP_ReadTable(oConnSAP, "VRESB", "|", "", "STLNR STLKN STPOZ MATKL RSPOS MATNR BDMNG MAKTX VORNR", "SPRAS EQ 'F' and XLOEK NE 'X'")
        'Label1.Text = sTableSAP(1, 1)
        'For Each i In sTableSAP
        'MsgBox(i)
        'GridView_Matricules.Rows.Item(i).Cells(1).Text = sTableSAP(i, 1)
        'GridView_Matricules.Rows.Item(i).Cells(2).Text = sTableSAP(i, 2)
        'GridView_Matricules.Rows.Item(i).Cells(3).Text = sTableSAP(i, 3)
        'Dim arrList As New ArrayList()

        Dim dt As New DataTable()


        'dt.Columns.Add("Matricule", Type.GetType("System.String"))
        'dt.Columns.Add("Nom", Type.GetType("System.String"))
        ' dt.Columns.Add("Prénom", Type.GetType("System.String"))
        ' For i As Integer = 1 To UBound(sTableSAP, 1)
        ' dt.Rows.Add()
        ' dt.Rows(dt.Rows.Count - 1)("Matricule") = sTableSAP(i, 0)
        ''     dt.Rows(dt.Rows.Count - 1)("Nom") = sTableSAP(i, 1)
        'dt.Rows(dt.Rows.Count - 1)("Prénom") = sTableSAP(i, 2)
        ' Next
        ' GridView_Matricules.DataSource = dt
        'GridView_Matricules.DataBind()

        dt.Columns.Add("1", Type.GetType("System.String"))
        dt.Columns.Add("2", Type.GetType("System.String"))
        dt.Columns.Add("3", Type.GetType("System.String"))
        dt.Columns.Add("4", Type.GetType("System.String"))
        dt.Columns.Add("5", Type.GetType("System.String"))
        dt.Columns.Add("6", Type.GetType("System.String"))
        dt.Columns.Add("7", Type.GetType("System.String"))
        dt.Columns.Add("8", Type.GetType("System.String"))
        dt.Columns.Add("9", Type.GetType("System.String"))
        For i As Integer = 1 To UBound(sTableSAP, 1)
            dt.Rows.Add()
            dt.Rows(dt.Rows.Count - 1)("1") = sTableSAP(i, 0)
            dt.Rows(dt.Rows.Count - 1)("2") = sTableSAP(i, 1)
            dt.Rows(dt.Rows.Count - 1)("3") = sTableSAP(i, 2)
            dt.Rows(dt.Rows.Count - 1)("4") = sTableSAP(i, 3)
            dt.Rows(dt.Rows.Count - 1)("5") = sTableSAP(i, 4)
            dt.Rows(dt.Rows.Count - 1)("6") = sTableSAP(i, 5)
            dt.Rows(dt.Rows.Count - 1)("7") = sTableSAP(i, 6)
            dt.Rows(dt.Rows.Count - 1)("8") = sTableSAP(i, 7)
            dt.Rows(dt.Rows.Count - 1)("9") = sTableSAP(i, 8)
        Next

        GridView_Nomenclature.DataSource = dt
        GridView_Nomenclature.DataBind()
        'Next

        'MsgBox("coucou")

        oConnSAP = SAP_Deconnexion(oConnSAP)
    End Sub

    ' Set theFunc = functionCtrl.Add("Z_EXPORT_PALMARES")

End Class