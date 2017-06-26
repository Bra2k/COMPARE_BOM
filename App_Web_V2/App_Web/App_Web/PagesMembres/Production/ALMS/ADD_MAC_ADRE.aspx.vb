Imports App_Web.Class_COMM_APP_WEB
Imports App_Web.Class_SQL
Imports App_Web.Class_EXCEL


Public Class ADD_MAC_ADRE
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Button_QT_Click(sender As Object, e As EventArgs) Handles Button_QT.Click
        Dim sQuery As String = "", sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=APP_WEB_ECO;Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"

        Dim sadresse_mac As String = ""
        Dim spremiere_mac_adresse As String = TextBox_PREM_MAC_ADRE.Text
        spremiere_mac_adresse = Replace(spremiere_mac_adresse, ":", "")
        spremiere_mac_adresse = Replace(spremiere_mac_adresse, "-", "")
        spremiere_mac_adresse = Replace(spremiere_mac_adresse, " ", "")
        Dim s6premier_caractere As String = Left(spremiere_mac_adresse, 6), s6dernier_caractere As String = Right(spremiere_mac_adresse, 6)
        Dim ipremiere_mac_adresse As Integer = COMM_APP_WEB_CONV_BASE_N_2_DEC(s6dernier_caractere, "16")
        sQuery = "INSERT INTO [ALMS_PRD].[dbo].[Tbl-Elec-Ref-Adresse_MAC-Liste] ([Adresse_MAC]) VALUES "
        For i As Integer = 0 To TextBox_QT.Text
            sadresse_mac = s6premier_caractere & COMM_APP_WEB_CONV_DEC_2_BASE_N(ipremiere_mac_adresse + i, 16, 6)
            sadresse_mac = sadresse_mac.Insert(10, ":")
            sadresse_mac = sadresse_mac.Insert(8, ":")
            sadresse_mac = sadresse_mac.Insert(6, ":")
            sadresse_mac = sadresse_mac.Insert(4, ":")
            sadresse_mac = sadresse_mac.Insert(2, ":")
            sQuery &= "('" & sadresse_mac & "'), "
        Next
        sQuery &= ";"
        sQuery = Replace(sQuery, "), ;", ")")
        SQL_REQ_ACT(sQuery, sChaineConnexion)
    End Sub

    Protected Sub Button_PLAG_ADRE_Click(sender As Object, e As EventArgs) Handles Button_PLAG_ADRE.Click
        Dim sQuery As String = "", sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=APP_WEB_ECO;Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"

        Dim sadresse_mac As String = ""
        Dim spremiere_mac_adresse As String = TextBox_PREM_MAC_ADRE_2.Text
        spremiere_mac_adresse = Replace(spremiere_mac_adresse, ":", "")
        spremiere_mac_adresse = Replace(spremiere_mac_adresse, "-", "")
        spremiere_mac_adresse = Replace(spremiere_mac_adresse, " ", "")
        Dim s6premier_caractere_premiere_mac_adresse As String = Left(spremiere_mac_adresse, 6), s6dernier_caractere_premiere_mac_adresse As String = Right(spremiere_mac_adresse, 6)
        Dim sderniere_mac_adresse As String = TextBox_DERN_MAC_ADRE.Text
        sderniere_mac_adresse = Replace(sderniere_mac_adresse, ":", "")
        sderniere_mac_adresse = Replace(sderniere_mac_adresse, "-", "")
        sderniere_mac_adresse = Replace(sderniere_mac_adresse, " ", "")
        Dim s6premier_caractere_derniere_mac_adresse As String = Left(sderniere_mac_adresse, 6), s6dernier_caractere_derniere_mac_adresse As String = Right(sderniere_mac_adresse, 6)
        sQuery = "INSERT INTO [ALMS_PRD].[dbo].[Tbl-Elec-Ref-Adresse_MAC-Liste] ([Adresse_MAC]) VALUES "
        For i As Integer = COMM_APP_WEB_CONV_BASE_N_2_DEC(s6dernier_caractere_premiere_mac_adresse, "16") To COMM_APP_WEB_CONV_BASE_N_2_DEC(s6dernier_caractere_derniere_mac_adresse, "16")
            sadresse_mac = s6premier_caractere_premiere_mac_adresse & COMM_APP_WEB_CONV_DEC_2_BASE_N(i, 16, 6)
            sadresse_mac = sadresse_mac.Insert(10, ":")
            sadresse_mac = sadresse_mac.Insert(8, ":")
            sadresse_mac = sadresse_mac.Insert(6, ":")
            sadresse_mac = sadresse_mac.Insert(4, ":")
            sadresse_mac = sadresse_mac.Insert(2, ":")
            sQuery &= "('" & sadresse_mac & "'), "
        Next
        sQuery &= ";"
        sQuery = Replace(sQuery, "), ;", ")")
        SQL_REQ_ACT(sQuery, sChaineConnexion)
    End Sub

    Protected Sub Button_UP_FICH_Click(sender As Object, e As EventArgs) Handles Button_UP_FICH.Click
        Dim savePath As String = "c:\sources\temp_App_Web\"

        If Not (FileUpload_FICH.HasFile) Then Throw New Exception("pas de fichier sélectionné")
        savePath += Server.HtmlEncode(FileUpload_FICH.FileName)
        FileUpload_FICH.SaveAs(savePath)
        FileUpload_FICH.Dispose()

        Select Case Right(savePath, 3)
            Case "xls", "lsx" 'récupération des onglets du fichier excel
                Dim dt_ONGL_EXCE As DataTable = EXCE_LIST_ONGL(savePath)
                If dt_ONGL_EXCE Is Nothing Then Throw New Exception("Aucun onglet trouvé")
                For Each r_ONGL_EXCE As DataRow In dt_ONGL_EXCE.Rows
                    DropDownList_ONGL.Items.Add(New ListItem(r_ONGL_EXCE("Onglet").ToString, r_ONGL_EXCE("Onglet").ToString))
                Next
                MultiView_FICH.SetActiveView(View_ONGL) 'naviguer vers la vue suivante (View_ONGL)
            Case "csv" 'récupération des données d'un fichier csv
                Session("dtMac_adresse") = COMM_APP_WEB_IMP_CSV_DT(savePath)
                GridView_SEL_DONN.DataSource = Session("dtMac_adresse")
                GridView_SEL_DONN.DataBind()
                MultiView_FICH.SetActiveView(View_SEL_DONN)
            Case Else
                Throw New Exception("Le fichier sélectionné n'a pas les extensions suivantes : xls, xlsx, csv")
        End Select
        Session("FICH_BOM") = savePath 'enregitrement du chemin du fichier sélectionné dans une variable de session
    End Sub

    Protected Sub Button_ONGL_Click(sender As Object, e As EventArgs) Handles Button_ONGL.Click
        Dim dtMac_adresse_2 As DataTable = EXCE_DATA_DT(Session("FICH_BOM"), DropDownList_ONGL.SelectedValue)
        Session("dtMac_adresse") = dtMac_adresse_2
        GridView_SEL_DONN.DataSource = Session("dtMac_adresse")
        GridView_SEL_DONN.DataBind()
        MultiView_FICH.SetActiveView(View_SEL_DONN)
    End Sub

    Protected Sub Button_SAI_NU_COLO_Click(sender As Object, e As EventArgs) Handles Button_SAI_NU_COLO.Click
        Dim sQuery As String = "", sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=APP_WEB_ECO;Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
        Dim dtMac_adresse_PIVO As DataTable = Session("dtMac_adresse")
        Dim dtMac_adresse_PIVO_FILT As DataTable = dtMac_adresse_PIVO.Select("NOT [" & TextBox_NU_COLO.Text & "] IS NULL").CopyToDataTable
        Dim sadresse_mac As String = ""
        For Each rdt_Mac_adresse As DataRow In dtMac_adresse_PIVO_FILT.Rows
            sadresse_mac = rdt_Mac_adresse(TextBox_NU_COLO.Text).ToString
            sadresse_mac = Replace(sadresse_mac, "-", ":")
            sadresse_mac = Replace(sadresse_mac, " ", ":")
            If Char.IsDigit(COMM_APP_WEB_CONV_BASE_N_2_DEC(Right(sadresse_mac, 6), "16").ToString) Then
                If sadresse_mac.IndexOf(":") = -1 Then
                    sadresse_mac = sadresse_mac.Insert(10, ":")
                    sadresse_mac = sadresse_mac.Insert(8, ":")
                    sadresse_mac = sadresse_mac.Insert(6, ":")
                    sadresse_mac = sadresse_mac.Insert(4, ":")
                    sadresse_mac = sadresse_mac.Insert(2, ":")
                End If
                sQuery = "INSERT INTO [ALMS_PRD].[dbo].[Tbl-Elec-Ref-Adresse_MAC-Liste] ([Adresse_MAC]) VALUES ('" & sadresse_mac & "')"
                SQL_REQ_ACT(sQuery, sChaineConnexion)
            End If
        Next
    End Sub

    Protected Sub RadioButtonList_CHOI_IMPO_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RadioButtonList_CHOI_IMPO.SelectedIndexChanged
        Select Case RadioButtonList_CHOI_IMPO.SelectedValue
            Case "VAL_QT"
                MultiView_CHOI_IMPO_MAC_ADRE.SetActiveView(View_QT)
            Case "VAL_PLAG_ADRE"
                MultiView_CHOI_IMPO_MAC_ADRE.SetActiveView(View_PLAG_ADRE)
            Case "VAL_FICH"
                MultiView_CHOI_IMPO_MAC_ADRE.SetActiveView(View_FICH)
        End Select
    End Sub
End Class