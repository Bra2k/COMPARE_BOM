Imports App_Web.Class_SAP_DATA
'Imports App_Web.Class_COMM_APP_WEB
Public Class Fonctions_Class_SAP_DATA
    Inherits System.Web.UI.Page

    Protected Sub RadioButtonList_SAP_DATA_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RadioButtonList_SAP_DATA.SelectedIndexChanged
        Select Case RadioButtonList_SAP_DATA.SelectedValue
            Case "SAP_DATA_MATR"
                MultiView_SAP_DATA.SetActiveView(View_SAP_DATA_MATR)
            Case "SAP_DATA_LIST_CLIE"
                MultiView_SAP_DATA.SetActiveView(View_SAP_DATA_LIST_CLIE)
            Case "SAP_DATA_NMCT_ARTI"
                MultiView_SAP_DATA.SetActiveView(View_SAP_DATA_NMCT_ARTI)
            Case "SAP_DATA_LIST_ARTI_CLIE"
                MultiView_SAP_DATA.SetActiveView(View_SAP_DATA_LIST_ARTI_CLIE)
            Case "SAP_DATA_READ_TBL"
                MultiView_SAP_DATA.SetActiveView(View_SAP_DATA_READ_TBL)
            Case "SAP_DATA_Z_LOG_CONF_GET"
                MultiView_SAP_DATA.SetActiveView(View_SAP_DATA_Z_LOG_CONF_GET)
            Case "SAP_DATA_Z_LOG_ACT_GET"
                MultiView_SAP_DATA.SetActiveView(View_SAP_DATA_Z_LOG_ACT_GET)
        End Select
    End Sub

    Protected Sub TextBox_SAP_DATA_MATR_TextChanged(sender As Object, e As EventArgs) Handles TextBox_SAP_DATA_MATR.TextChanged
        Dim dt As DataTable = SAP_DATA_MATR(TextBox_SAP_DATA_MATR.Text)
        GridView_SAP_DATA.DataSource = dt
        GridView_SAP_DATA.DataBind()
    End Sub

    Protected Sub TextBox_SAP_DATA_LIST_CLIE_TextChanged(sender As Object, e As EventArgs) Handles TextBox_SAP_DATA_LIST_CLIE.TextChanged
        Dim dt As DataTable = SAP_DATA_LIST_CLIE(TextBox_SAP_DATA_LIST_CLIE.Text)
        GridView_SAP_DATA.DataSource = dt
        GridView_SAP_DATA.DataBind()
    End Sub

    Protected Sub TextBox_SAP_DATA_NMCT_ARTI_TextChanged(sender As Object, e As EventArgs) Handles TextBox_SAP_DATA_NMCT_ARTI.TextChanged
        Dim dt As DataTable = SAP_DATA_NMCT_ARTI(TextBox_SAP_DATA_NMCT_ARTI.Text)
        GridView_SAP_DATA.DataSource = dt
        GridView_SAP_DATA.DataBind()
    End Sub

    Protected Sub Button_SAP_DATA_LIST_ARTI_CLIE_Click(sender As Object, e As EventArgs) Handles Button_SAP_DATA_LIST_ARTI_CLIE.Click
        Dim dt As DataTable = SAP_DATA_LIST_ARTI_CLIE(TextBox_SAP_DATA_LIST_ARTI_CLIE_sClient.Text, TextBox_SAP_DATA_LIST_ARTI_CLIE_sType_produit.Text)
        GridView_SAP_DATA.DataSource = dt
        GridView_SAP_DATA.DataBind()
    End Sub

    Protected Sub Button_SAP_DATA_READ_TBL_Valid_Click(sender As Object, e As EventArgs) Handles Button_SAP_DATA_READ_TBL_Valid.Click
        Dim dt As DataTable = SAP_DATA_READ_TBL(TextBox_SAP_DATA_READ_TBL_Table.Text,
                                                TextBox_SAP_DATA_READ_TBL_Delimiter.Text,
                                                TextBox_SAP_DATA_READ_TBL_NoData.Text,
                                                TextBox_SAP_DATA_READ_TBL_Field.Text,
                                                TextBox_SAP_DATA_READ_TBL_option_req.Text,
                                                TextBox_SAP_DATA_READ_TBL_fieldinarray.Text,
                                                TextBox_SAP_DATA_READ_TBL_ligneskip.Text,
                                                TextBox_SAP_DATA_READ_TBL_nbrlignes.Text)
        GridView_SAP_DATA.DataSource = dt
        GridView_SAP_DATA.DataBind()
    End Sub

    Protected Sub Button_SAP_DATA_Z_LOG_CONF_GET_Valid_Click(sender As Object, e As EventArgs) Handles Button_SAP_DATA_Z_LOG_CONF_GET_Valid.Click

        Dim dt As DataTable = SAP_DATA_Z_LOG_CONF_GET(TextBox_V_AUFNR.Text, TextBox_V_VORNR.Text, TextBox_V_PERNR.Text, Calendar_DATE_DEBUT_1.SelectedDate, Calendar_DATE_FIN_2.SelectedDate)
        GridView_SAP_DATA.DataSource = dt
        GridView_SAP_DATA.DataBind()
    End Sub

    Protected Sub Button_SAP_DATA_Z_LOG_ACT_GET_Valid_Click(sender As Object, e As EventArgs) Handles Button_SAP_DATA_Z_LOG_ACT_GET_Valid.Click
        Dim dt As DataTable = SAP_DATA_Z_LOG_ACT_GET(TextBox_V_PERNR.Text, Calendar_DATE_DEBUT_2.SelectedDate, Calendar_DATE_FIN_3.SelectedDate)
        GridView_SAP_DATA.DataSource = dt
        GridView_SAP_DATA.DataBind()
    End Sub
End Class