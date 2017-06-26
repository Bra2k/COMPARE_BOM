Imports App_Web.Class_SQL
Imports App_Web.Class_DIG_FACT_SQL
Imports App_Web.LOG
Imports System.Reflection.MethodBase

Public Class TEST_BATT_TUCK
    Inherits System.Web.UI.Page
    Dim sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=" & Replace(Replace(My.Computer.Name, "CEDB03", "MES_Digital_Factory_DEV"), "CEAPP03", "MES_Digital_Factory") & ";Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=7200;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Button_VALI_Click(sender As Object, e As EventArgs) Handles Button_VALI.Click

        Dim sQuery As String = ""
        Dim dt, dt_CFGR_ARTI_ECO, dtMSEG, dt_var As New DataTable

        Try
            sQuery = "SELECT [NEW_ID_PSG]
                        FROM [dbo].[V_NEW_ID_PSG_DTM_PSG]"
            dt = SQL_SELE_TO_DT(sQuery, sChaineConnexion)

            'Enregistrement
            sQuery = "INSERT INTO [dbo].[DTM_PSG] ([ID_PSG], [LB_ETP], [DT_DEB] , [LB_MOYN], [LB_PROG], [NM_MATR], [NM_NS_EOL], [LB_SCTN], [NM_OF])
                           VALUES (" & dt(0)("NEW_ID_PSG").ToString & ", 'Test de la batterie', GETDATE(), '" & System.Net.Dns.GetHostEntry(System.Web.HttpContext.Current.Request.UserHostAddress).HostName() & "', '" & HttpContext.Current.CurrentHandler.ToString & "', '" & Session("matricule") & "', '', 'P', '1162063')"
            SQL_REQ_ACT(sQuery, sChaineConnexion)

            sQuery = "INSERT INTO [dbo].[DTM_TR_CPT] ([NM_NS_EOL],[NM_NS_CLT],[ID_PSG],[DT_PSG], [ID_CPT])
                               VALUES ('', '" & TextBox_NU_SER_PROD.Text & "', " & dt(0)("NEW_ID_PSG").ToString & ", GETDATE(), '-')"
            SQL_REQ_ACT(sQuery, sChaineConnexion)

            sQuery = "INSERT INTO [dbo].[DTM_TEST_FONC_VAL] ([ID_EXEC],[ID_TEST],[NB_VAL])
                           VALUES (" & dt(0)("NEW_ID_PSG").ToString & ",26000," & TextBox_VALE_BATT.Text & ")"
            SQL_REQ_ACT(sQuery, sChaineConnexion)



        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try
        TextBox_NU_SER_PROD.Focus()
        TextBox_NU_SER_PROD.Text = ""
        TextBox_VALE_BATT.Text = ""

    End Sub

    Protected Sub TextBox_NU_SER_PROD_TextChanged(sender As Object, e As EventArgs) Handles TextBox_NU_SER_PROD.TextChanged
        TextBox_VALE_BATT.Focus()
    End Sub
End Class