Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports App_Web.Class_COMM_APP_WEB
Imports App_Web.Class_SQL
Imports App_Web.Class_EXCEL
Imports System.IO
Public Class IPTT_ADRE_MAC
    Inherits System.Web.UI.Page
    Protected Sub Button_QT_Click(sender As Object, e As EventArgs) Handles Button_QT.Click
        Try
            Dim sadresse_mac As String = ""
            If TextBox_NM_CRIT.Text = "" Then Throw New Exception("Le critère n'est pas saisi")
            If Len(TextBox_PREM_MAC_ADRE.Text) <> 12 Then Throw New Exception("Le nombre de caractère de la première adresse MAC est incorrect (12)")
            If TextBox_QT.Text = "" Then Throw New Exception("Le nombre d'adresse MAC à générer n'est pas saisi.")
            Dim spremiere_mac_adresse As String = TextBox_PREM_MAC_ADRE.Text
            spremiere_mac_adresse = Replace(spremiere_mac_adresse, ":", "")
            spremiere_mac_adresse = Replace(spremiere_mac_adresse, "-", "")
            spremiere_mac_adresse = Replace(spremiere_mac_adresse, " ", "")
            Dim s6premier_caractere As String = Left(spremiere_mac_adresse, 6), s6dernier_caractere As String = Right(spremiere_mac_adresse, 6)
            Dim ipremiere_mac_adresse As Integer = COMM_APP_WEB_CONV_BASE_N_2_DEC(s6dernier_caractere, "16")
            Dim sbQuery As New StringBuilder
            sbQuery.Append("INSERT INTO [dbo].[DTM_NU_SER] ([NM_CRIT],[NU_SER],[NM_TYPE],[DT_CREA]) VALUES ")
            For i As Integer = 0 To TextBox_QT.Text
                sadresse_mac = $"{s6premier_caractere}{COMM_APP_WEB_CONV_DEC_2_BASE_N(ipremiere_mac_adresse + i, 16, 6)}"
                Using dt = SQL_SELE_TO_DT($"SELECT [NU_ADRE_MAC]
                                              FROM [dbo].[V_LIAIS_NU_SER]
                                             WHERE [NU_ADRE_MAC] = '{sadresse_mac}'", CS_MES_Digital_Factory_DEV)
                    If Not dt Is Nothing Then Throw New Exception($"L'adresse MAC {sadresse_mac} existe déjà dans la base.")
                End Using
                sbQuery.Append($"('{TextBox_NM_CRIT.Text}', '{sadresse_mac}', 'Adresse MAC', GETDATE()), ")
            Next
            sbQuery.Append(";")
            SQL_REQ_ACT(Replace(sbQuery.ToString, "), ;", ")"), CS_MES_Digital_Factory_DEV)
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try
    End Sub

    Protected Sub Button_PLAG_ADRE_Click(sender As Object, e As EventArgs) Handles Button_PLAG_ADRE.Click

        Try
            Dim sadresse_mac As String = ""
            If TextBox_NM_CRIT.Text = "" Then Throw New Exception("Le critère n'est pas saisi")
            Dim spremiere_mac_adresse As String = TextBox_PREM_MAC_ADRE_2.Text
            If Len(TextBox_PREM_MAC_ADRE_2.Text) <> 12 Then Throw New Exception("Le nombre de caractère de la première adresse MAC est incorrect (12)")
            spremiere_mac_adresse = Replace(spremiere_mac_adresse, ":", "")
            spremiere_mac_adresse = Replace(spremiere_mac_adresse, "-", "")
            spremiere_mac_adresse = Replace(spremiere_mac_adresse, " ", "")
            Dim s6premier_caractere_premiere_mac_adresse As String = Left(spremiere_mac_adresse, 6), s6dernier_caractere_premiere_mac_adresse As String = Right(spremiere_mac_adresse, 6)
            Dim sderniere_mac_adresse As String = TextBox_DERN_MAC_ADRE.Text
            If Len(TextBox_DERN_MAC_ADRE.Text) <> 12 Then Throw New Exception("Le nombre de caractère de la dernière adresse MAC est incorrect (12)")
            sderniere_mac_adresse = Replace(sderniere_mac_adresse, ":", "")
            sderniere_mac_adresse = Replace(sderniere_mac_adresse, "-", "")
            sderniere_mac_adresse = Replace(sderniere_mac_adresse, " ", "")
            Dim s6premier_caractere_derniere_mac_adresse As String = Left(sderniere_mac_adresse, 6), s6dernier_caractere_derniere_mac_adresse As String = Right(sderniere_mac_adresse, 6)
            Dim sbQuery As New StringBuilder
            sbQuery.Append("INSERT INTO [dbo].[DTM_NU_SER] ([NM_CRIT],[NU_SER],[NM_TYPE],[DT_CREA]) VALUES ")
            For i As Integer = COMM_APP_WEB_CONV_BASE_N_2_DEC(s6dernier_caractere_premiere_mac_adresse, "16") To COMM_APP_WEB_CONV_BASE_N_2_DEC(s6dernier_caractere_derniere_mac_adresse, "16")
                sadresse_mac = $"{s6premier_caractere_premiere_mac_adresse}{COMM_APP_WEB_CONV_DEC_2_BASE_N(i, 16, 6)}"
                Using dt = SQL_SELE_TO_DT($"SELECT [NU_ADRE_MAC]
                                              FROM [dbo].[V_LIAIS_NU_SER]
                                             WHERE [NU_ADRE_MAC] = '{sadresse_mac}'", CS_MES_Digital_Factory_DEV)
                    If Not dt Is Nothing Then Throw New Exception($"L'adresse MAC {sadresse_mac} existe déjà dans la base.")
                End Using
                sbQuery.Append($"('{TextBox_NM_CRIT.Text}', '{sadresse_mac}', 'Adresse MAC', GETDATE()), ")
            Next
            sbQuery.Append(";")
            SQL_REQ_ACT(Replace(sbQuery.ToString, "), ;", ")"), CS_MES_Digital_Factory_DEV)
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try
    End Sub

    Protected Sub Button_UP_FICH_Click(sender As Object, e As EventArgs) Handles Button_UP_FICH.Click
        Try
            Dim savePath As String = "c:\sources\temp_App_Web\"

            Using FileUpload_FICH
                If Not (FileUpload_FICH.HasFile) Then Throw New Exception("pas de fichier sélectionné")
                savePath += Server.HtmlEncode(FileUpload_FICH.FileName)
                FileUpload_FICH.SaveAs(savePath)
            End Using
            Select Case Path.GetExtension(savePath)
                Case "xls", "xlsx" 'récupération des onglets du fichier excel
                    Using dt_ONGL_EXCE As DataTable = EXCE_LIST_ONGL(savePath)
                        If dt_ONGL_EXCE Is Nothing Then Throw New Exception("Aucun onglet trouvé")
                        For Each r_ONGL_EXCE As DataRow In dt_ONGL_EXCE.Rows
                            DropDownList_ONGL.Items.Add(New ListItem(r_ONGL_EXCE("Onglet").ToString, r_ONGL_EXCE("Onglet").ToString))
                        Next
                    End Using
                    MultiView_FICH.SetActiveView(View_ONGL) 'naviguer vers la vue suivante (View_ONGL)
                Case "csv" 'récupération des données d'un fichier csv
                    Session("dtMac_adresse") = COMM_APP_WEB_IMP_CSV_DT(savePath)
                    GridView_SEL_DONN.DataSource = Session("dtMac_adresse")
                    GridView_SEL_DONN.DataBind()
                    MultiView_FICH.SetActiveView(View_SEL_DONN)
                Case Else
                    Throw New Exception("Le fichier sélectionné n'a pas les extensions suivantes : xls, xlsx, csv")
            End Select
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try
    End Sub

    Protected Sub Button_ONGL_Click(sender As Object, e As EventArgs) Handles Button_ONGL.Click
        Try
            Using dtMac_adresse_2 As DataTable = EXCE_DATA_DT(Session("FICH_BOM"), DropDownList_ONGL.SelectedValue)
                Session("dtMac_adresse") = dtMac_adresse_2
                GridView_SEL_DONN.DataSource = Session("dtMac_adresse")
                GridView_SEL_DONN.DataBind()
                MultiView_FICH.SetActiveView(View_SEL_DONN)
            End Using
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
            Exit Sub
        End Try
    End Sub

    Protected Sub Button_SAI_NU_COLO_Click(sender As Object, e As EventArgs) Handles Button_SAI_NU_COLO.Click

        Try
            Using dtMac_adresse_PIVO As DataTable = Session("dtMac_adresse")
                Using dtMac_adresse_PIVO_FILT As DataTable = dtMac_adresse_PIVO.Select($"NOT [{TextBox_NU_COLO.Text}] IS NULL").CopyToDataTable
                    Dim sbQuery As New StringBuilder
                    sbQuery.Append("INSERT INTO [dbo].[DTM_NU_SER] ([NM_CRIT],[NU_SER],[NM_TYPE],[DT_CREA]) VALUES ")
                    Dim sadresse_mac As String = ""
                    For Each rdt_Mac_adresse As DataRow In dtMac_adresse_PIVO_FILT.Rows
                        sadresse_mac = rdt_Mac_adresse(TextBox_NU_COLO.Text).ToString
                        sadresse_mac = Replace(sadresse_mac, ":", "")
                        sadresse_mac = Replace(sadresse_mac, "-", "")
                        sadresse_mac = Replace(sadresse_mac, " ", "")
                        Using dt = SQL_SELE_TO_DT($"SELECT [NU_ADRE_MAC]
                                                      FROM [dbo].[V_LIAIS_NU_SER]
                                                     WHERE [NU_ADRE_MAC] = '{sadresse_mac}'", CS_MES_Digital_Factory_DEV)
                            If Not dt Is Nothing Then Throw New Exception($"L'adresse MAC {sadresse_mac} existe déjà dans la base.")
                        End Using
                        If Char.IsDigit(COMM_APP_WEB_CONV_BASE_N_2_DEC(Right(sadresse_mac, 6), "16").ToString) Then sbQuery.Append($"('{TextBox_NM_CRIT.Text}', '{sadresse_mac}', 'Adresse MAC', GETDATE()), ")
                    Next
                    sbQuery.Append(";")
                    SQL_REQ_ACT(Replace(sbQuery.ToString, "), ;", ")"), CS_MES_Digital_Factory_DEV)
                End Using
            End Using
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "Erreur")
        Exit Sub
        End Try
    End Sub

End Class