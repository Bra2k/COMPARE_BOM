Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports System.Data
Imports System.Data.SqlClient
Imports App_Web.Class_SQL
Imports App_Web.Class_SAP_DATA
Imports App_Web.Class_COMM_APP_WEB
Imports App_Web.Class_PDF
Imports PdfSharp
Imports PdfSharp.Drawing
Imports PdfSharp.Drawing.Layout
Imports PdfSharp.Pdf
Imports System.IO
Imports System
Imports Microsoft.VisualBasic.Strings
Public Class Class_DIG_FACT
    Public Shared Function DIG_FACT_VERI_FORM_NU_SER(sCD_ARTI As String, sNM_PARA As String, sNU_SER As String) As Boolean

        Dim sFORM As String = "", sMESS_ERR As String = ""

        Try
            sFORM = Class_DIG_FACT_SQL.DIG_FACT_SQL_GET_PARA(sCD_ARTI, sNM_PARA)
            If sFORM Is Nothing Then Throw New Exception("La base n'a pas été configurée")

            'comparer
            If Len(sFORM) <> Len(sNU_SER) Then Throw New Exception($"Le nombre de caractère du numéro de série {sNU_SER} n'est pas identique au nombre de caractère du format {sFORM}")

            For iChar As Integer = 1 To Len(sFORM)
                Select Case Mid(sFORM, iChar, 1)
                    Case "%"
                        If Char.IsDigit(Mid(sNU_SER, iChar, 1)) = False Then Throw New Exception($"le caractère n°{iChar} du numéro de série {sNU_SER} n'est pas un chiffre.")
                    Case "£"
                        If Char.IsLetter(Mid(sNU_SER, iChar, 1)) = False Then Throw New Exception($"le caractère n°{iChar} du numéro de série {sNU_SER} n'est pas une lettre.")
                    Case "#"
                        If Char.IsLetterOrDigit(Mid(sNU_SER, iChar, 1)) = False Then Throw New Exception($"le caractère n°{iChar} du numéro de série {sNU_SER} n'est ni un chiffre ni une lettre.")
                    Case <> Mid(sNU_SER, iChar, 1)
                        Throw New Exception($"Le caractère {Mid(sNU_SER, iChar, 1)} du numéro de série {sNU_SER} ne se correspondent pas.")
                End Select
            Next
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return False
        End Try
        Return True

    End Function

    Public Shared Sub DIG_FACT_IMPR_ETIQ(sFichier As String, sNU_OF As String, sNU_BL As String, sTYPE_ETIQ As String,
                                         sNU_CLIE As String, sNU_ECO As String, sNU_CART As String, sNB_QT As String,
                                         sNB_CART As String, dtVar As DataTable, Optional sMTCL As String = "")
        Dim sData As String = "", sCD_ARTI_ECO As String = "", sNM_CLIE As String = ""
        Dim sData_Crit As String = "", sDT_PROD As String = Now
        Dim dtAFKO, dtMARA, dtT179T, dtMAKT, dtLIPS, dtLIPSUP, dt_CFGR_ARTI_ECO, dt_ETAT_CTRL As New DataTable
        Dim sr As StreamReader
        Randomize()
        Dim sfich As String = $"{My.Settings.RPTR_TPRR}\{CInt(Int((1000 * Rnd()) + 1))}_{Path.GetFileName(sFichier)}"
        Dim sw As StreamWriter

        'Génération des étiquettes AVALUN
        dtAFKO = SAP_DATA_READ_AFKO($"AUFNR LIKE '%{sNU_OF}'")
        If Not dtAFKO Is Nothing And Trim(dtAFKO(0)("PLNBEZ").ToString) = "AVAE947700$" And sFichier = "\\ceapp03\Sources\Digital Factory\Etiquettes\AVALUN\AVALUN.prn" Then
            Dim sfich_AVA As String = $"{My.Settings.RPTR_TPRR}\{CInt(Int((1000 * Rnd()) + 1))}_{Path.GetFileName("\\ceapp03\Sources\Digital Factory\Etiquettes\AVALUN\AVALUN.prn")}"
            COMM_APP_WEB_COPY_FILE("\\ceapp03\Sources\Digital Factory\Etiquettes\AVALUN\AVALUN.prn", sfich_AVA, True)
            Dim sr_AVA = New StreamReader(sfich_AVA, Encoding.UTF8)
            sData = sr_AVA.ReadToEnd()
            sr_AVA.Close()
            sData = Replace(sData, "#REF", "03760097080008")
            sNU_CART = Convert.ToDecimal(sNU_CART) + 1
            Dim length As Integer = sNU_CART.Length

            Select Case length
                Case 1
                    sData = Replace(sData, "#OF_SER_NUM", $"{sNU_OF}00000{sNU_CART}")
                Case 2
                    sData = Replace(sData, "#OF_SER_NUM", $"{sNU_OF}0000{sNU_CART}")
                Case 3
                    sData = Replace(sData, "#OF_SER_NUM", $"{sNU_OF}000{sNU_CART}")
                Case 4
                    sData = Replace(sData, "#OF_SER_NUM", $"{sNU_OF}00{sNU_CART}")
                Case 5
                    sData = Replace(sData, "#OF_SER_NUM", $"{sNU_OF}0{sNU_CART}")
                Case 6
                    sData = Replace(sData, "#OF_SER_NUM", $"{sNU_OF}{sNU_CART}")
            End Select

            Dim ser_num_Query As String = $"INSERT INTO [dbo].[DTM_NU_SER]
									            ([NM_CRIT], [NU_SER], [NM_TYPE], [DT_CREA])
								               VALUES
									            ('{sNU_OF}', '{sNU_CART}', 'Numéro de série Eolane', getdate())"
            Try
                SQL_REQ_ACT(ser_num_Query, $"Data Source=cedb03,1433;Initial Catalog={Replace(Replace(My.Computer.Name, "CEDB03", "MES_Digital_Factory_DEV"), "CEAPP03", "MES_Digital_Factory")};Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=7200;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")
                sw = New StreamWriter(sfich, False, System.Text.Encoding.UTF8)
                sw.WriteLine(sData)
                sw.Close()
                COMM_APP_WEB_IMPR_ETIQ_PRN(sfich, App_Web.Class_DIG_FACT_SQL.DIG_FACT_SQL_GET_PARA(System.Net.Dns.GetHostEntry(System.Web.HttpContext.Current.Request.UserHostAddress).HostName(), "Imprimante étiquette"))
            Catch ex As Exception
                LOG_Erreur(GetCurrentMethod, ex.Message)
            End Try
            Exit Sub
            'End If
        End If

        Try
            If File.Exists(sfich) Then My.Computer.FileSystem.DeleteFile(sfich)
            COMM_APP_WEB_COPY_FILE(sFichier, sfich, True)
            sr = New StreamReader(sfich, System.Text.Encoding.UTF8)
            sData = sr.ReadToEnd()
            sr.Close()

            If sNU_OF <> "" Then
                sData = Replace(sData, "#NU_OF", sNU_OF)
                dtAFKO = SAP_DATA_READ_AFKO($"AUFNR Like '%{sNU_OF}'")
                If Not dtAFKO Is Nothing Then
                    dtMAKT = SAP_DATA_READ_MAKT($"MATNR EQ '{dtAFKO(0)("PLNBEZ").ToString}'")
                    If dtMAKT Is Nothing Then Throw New Exception($"La désignation d'aticle de l'OF {sNU_OF} n'a pas été trouvé dans SAP.")
                    sCD_ARTI_ECO = dtAFKO(0)("PLNBEZ").ToString
                    sData = Replace(sData, "#NM_DSGT_ARTI", Trim(dtMAKT(0)("MAKTX").ToString))
                    sDT_PROD = dtAFKO(0)("GSTRS").ToString.Insert(6, "/").Insert(4, "/")
                End If
            ElseIf sNU_BL <> "" Then
                dtLIPSUP = SAP_DATA_READ_LIPSUP($"VBELN EQ {sNU_BL}")
                If Not dtLIPSUP Is Nothing Then
                    sCD_ARTI_ECO = SAP_DATA_CONV_CHAM(dtLIPSUP(0)("MATNR").ToString, "Code article", " ", "après")
                    sData = Replace(sData, "#NM_DSGT_ARTI", Trim(dtLIPSUP(0)("ARKTX").ToString))
                End If
                dtLIPS = SAP_DATA_READ_LIPS($"VBELN EQ {sNU_BL}")
                If Not dtLIPS Is Nothing Then sData = Replace(sData, "#NU_CMDE", dtLIPS(0)("VGBEL").ToString)
            Else
                Throw New Exception("Erreur d'appel à la fonction, pas de données d'entrée")
            End If
            dtMARA = SAP_DATA_READ_MARA($"MATNR EQ '{sCD_ARTI_ECO}'")
            dtT179T = SAP_DATA_READ_T179T($"PRODH EQ '{dtMARA(0)("PRDHA").ToString}'")
            'client
            sNM_CLIE = dtT179T(0)("VTEXT").ToString
            sData = Replace(sData, "#NM_CLIE", sNM_CLIE)
            sData = Replace(sData, "#CD_ARTI_ECO", Trim(sCD_ARTI_ECO))
            dt_CFGR_ARTI_ECO = App_Web.Class_DIG_FACT_SQL.DIG_FACT_SQL_CFGR_ARTI_ECO(Trim(sCD_ARTI_ECO))
            If dt_CFGR_ARTI_ECO Is Nothing Then Throw New Exception($"La base Digital Factory n'a pas été configurée pour l'article {Trim(sCD_ARTI_ECO)}")
            dt_ETAT_CTRL = COMM_APP_WEB_ETAT_CTRL($"{sCD_ARTI_ECO}|{sTYPE_ETIQ}", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx")
            If dt_ETAT_CTRL Is Nothing Then Throw New Exception($"La base App_Web_Eco n'as pas été configurée pour l'article {Trim(sCD_ARTI_ECO)}")

            'sData = Replace(sData, "#CD_ARTI_CLIE", App_Web.Class_DIG_FACT_SQL.DIG_FACT_SQL_GET_PARA(Trim(sCD_ARTI_ECO), "Code article client"))
            sData = Replace(sData, "#CD_ARTI_CLIE", dt_CFGR_ARTI_ECO(0)("Code article client").ToString)
            sData = Replace(sData, "#NM_ARTI_CLIE", dt_CFGR_ARTI_ECO(0)("Désignation article client").ToString)
            sData = Replace(sData, "#NU_BL", sNU_BL)
            sData = Replace(sData, "#NU_CART", sNU_CART)
            sData = Replace(sData, "#CD_FNS", App_Web.Class_DIG_FACT_SQL.DIG_FACT_SQL_GET_PARA(sNM_CLIE, "Format du code fournisseur"))

            If dt_ETAT_CTRL.Columns.Contains("DropDownList_CRIT_GENE_NU_SER") Then
                Select Case dt_ETAT_CTRL(0)("DropDownList_CRIT_GENE_NU_SER").ToString
            'Select Case COMM_APP_WEB_GET_PARA(sCD_ARTI_ECO}|{sTYPE_ETIQ, "DropDownList_CRIT_GENE_NU_SER", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx")
                    Case "OF"
                        sData_Crit = sNU_OF
                    Case "Code Article"
                        sData_Crit = sCD_ARTI_ECO
                    Case "Client"
                        sData_Crit = sNM_CLIE
                End Select
            End If
            'If COMM_APP_WEB_GET_PARA(sCD_ARTI_ECO}|{sTYPE_ETIQ, "CheckBox_NU_SER_CLIE_GENE_AUTO", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx") = "True" Then
            If dt_ETAT_CTRL.Columns.Contains("CheckBox_NU_SER_CLIE_GENE_AUTO") And dt_ETAT_CTRL.Columns.Contains("TextBox_FORM_NU_CLIE") And dt_ETAT_CTRL.Columns.Contains("CheckBox_REPR_NU_SER_REBU") Then
                If dt_ETAT_CTRL(0)("CheckBox_NU_SER_CLIE_GENE_AUTO").ToString = "True" Then
                    sNU_CLIE = App_Web.Class_DIG_FACT_SQL.DIG_FACT_SQL_GENE_NU_SER_CLIE(dt_CFGR_ARTI_ECO(0)("Encodage Numéro de série client").ToString,
                                                                                        dt_CFGR_ARTI_ECO(0)("Incrémentation flanc").ToString,
                                                                                        dt_ETAT_CTRL(0)("TextBox_FORM_NU_CLIE").ToString,
                                                                                        sData_Crit,
                                                                                        sTYPE_ETIQ,
                                                                                        dt_ETAT_CTRL(0)("CheckBox_REPR_NU_SER_REBU").ToString,
                                                                                        sMTCL,
                                                                                        sNU_OF)
                    If sNU_CLIE Is Nothing Then Throw New Exception("Erreur dans la génération du numéro de série client")
                End If
            End If
            sData = Replace(sData, "#NU_SER_CLIE", sNU_CLIE)
            sData = Replace(sData, "#NU_SER_ECO", sNU_ECO)
            If dt_ETAT_CTRL.Columns.Contains("CheckBox_EXTR_NU_SER_ECO") Then
                If dt_ETAT_CTRL(0)("CheckBox_EXTR_NU_SER_ECO").ToString = "True" And sNU_ECO.IndexOf(sNU_OF) + 1 > 0 Then sData = Replace(sData, "#EXTR_NU_SER_ECO", Mid(sNU_ECO, sNU_ECO.IndexOf(sNU_OF) + 1, 13))
            End If
            sData = Replace(sData, "#NB_QT", sNB_QT)
            sData = Replace(sData, "#NB_CART", sNB_CART)

            If dt_ETAT_CTRL.Columns.Contains("TextBox_FT_DT_PROD") Then sData = Replace(sData, "#DT_PROD", COMM_APP_WEB_CONV_FORM_DATE(sDT_PROD, dt_ETAT_CTRL(0)("TextBox_FT_DT_PROD").ToString))
            ''''''''''''''''''''''''''''condition'''''''''''''''''''''''''''''''
            If Trim(sCD_ARTI_ECO) = "STAGE950200$" Then
                If dt_ETAT_CTRL.Columns.Contains("TextBox_FT_DT_EXP") Then sData = Replace(sData, "#DT_EXP", COMM_APP_WEB_CONV_FORM_DATE(App_Web.Class_DIG_FACT_SQL.EXTRACT_DATE(sNU_ECO), dt_ETAT_CTRL(0)("TextBox_FT_DT_EXP").ToString))
            Else
                If dt_ETAT_CTRL.Columns.Contains("TextBox_FT_DT_EXP") Then sData = Replace(sData, "#DT_EXP", COMM_APP_WEB_CONV_FORM_DATE(Now, dt_ETAT_CTRL(0)("TextBox_FT_DT_EXP").ToString))
            End If

            sData = Replace(sData, "#IND_CLIE", dt_CFGR_ARTI_ECO(0)("Indice client").ToString)
            sData = Replace(sData, "#DDM", dt_CFGR_ARTI_ECO(0)("Nom du DDM").ToString)

            If Not dtVar Is Nothing Then
                For Each rdtVar As DataRow In dtVar.Rows
                    sData = Replace(sData, $"#var{Right(COMM_APP_WEB_CONV_DEC_2_BASE_N(dtVar.Rows.IndexOf(rdtVar) + 1, 36, 2), 1)}", rdtVar(1).ToString)
                Next
            End If
            For iVar = 1 To 15
                sData = Replace(sData, $"#var{Right(COMM_APP_WEB_CONV_DEC_2_BASE_N(iVar, 36, 2), 1)}", "")
            Next
            sw = New StreamWriter(sfich, False, System.Text.Encoding.UTF8)
            sw.WriteLine(sData)
            sw.Close()
            COMM_APP_WEB_IMPR_ETIQ_PRN(sfich, App_Web.Class_DIG_FACT_SQL.DIG_FACT_SQL_GET_PARA(System.Net.Dns.GetHostEntry(System.Web.HttpContext.Current.Request.UserHostAddress).HostName(), "Imprimante étiquette"))

        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Exit Sub
        End Try
    End Sub
    Public Shared Sub DIG_FACT_IMPR_ETIQ_V2(sFichier As String, sNM_IPMT As String, sNU_OF As String, sNU_BL As String, sTYPE_ETIQ As String,
                                            sID_NU_SER As String, sNU_CART As String, sNB_QT As String,
                                            sNB_CART As String, dtVar As DataTable, Optional sMTCL As String = "")
        Dim sData As String = "", sCD_ARTI_ECO As String = "", sNM_CLIE As String = "", sNU_CLIE As String = "", sNU_ECO As String = "", sQuery As String = ""
        Dim sData_Crit As String = "", sDT_PROD As String = Now

        Try
            Randomize()
            Dim sfich As String = $"{My.Settings.RPTR_TPRR}\{CInt(Int((1000 * Rnd()) + 1))}_{Path.GetFileName(sFichier)}"
            If File.Exists(sfich) Then My.Computer.FileSystem.DeleteFile(sfich)
            COMM_APP_WEB_COPY_FILE(sFichier, sfich, True)
            Using sr = New StreamReader(sfich, System.Text.Encoding.UTF8)
                sData = sr.ReadToEnd()
                sr.Close()
            End Using
            If sNU_OF <> "" Then
                sData = Replace(sData, "#NU_OF", sNU_OF)
                Using dtAFKO = SAP_DATA_READ_AFKO($"AUFNR LIKE '%{sNU_OF}'")
                    If Not dtAFKO Is Nothing Then
                        Using dtMAKT = SAP_DATA_READ_MAKT($"MATNR EQ '{dtAFKO(0)("PLNBEZ").ToString}'")
                            If dtMAKT Is Nothing Then Throw New Exception($"La désignation d'aticle de l'OF {sNU_OF} n'a pas été trouvé dans SAP.")
                            sCD_ARTI_ECO = dtAFKO(0)("PLNBEZ").ToString
                            sData = Replace(sData, "#NM_DSGT_ARTI", Trim(dtMAKT(0)("MAKTX").ToString))
                            sDT_PROD = dtAFKO(0)("GSTRS").ToString.Insert(6, "/").Insert(4, "/")
                        End Using
                    End If
                End Using
            ElseIf sNU_BL <> "" Then
                Using dtLIPSUP = SAP_DATA_READ_LIPSUP($"VBELN EQ {sNU_BL}")
                    If Not dtLIPSUP Is Nothing Then
                        sCD_ARTI_ECO = SAP_DATA_CONV_CHAM(dtLIPSUP(0)("MATNR").ToString, "Code article", " ", "après")
                        sData = Replace(sData, "#NM_DSGT_ARTI", Trim(dtLIPSUP(0)("ARKTX").ToString))
                    End If
                End Using
                Using dtLIPS = SAP_DATA_READ_LIPS($"VBELN EQ {sNU_BL}")
                    If Not dtLIPS Is Nothing Then sData = Replace(sData, "#NU_CMDE", dtLIPS(0)("VGBEL").ToString)
                End Using
            Else
                Throw New Exception("Erreur d'appel à la fonction, pas de données d'entrée")
            End If
            Select Case sNU_OF 'todo à effacer quand l'impression des étiquettes hibiscus sera validé ou que des of seront créés
                Case "9000001"
                    sCD_ARTI_ECO = "TAED966200$"
                Case "9000002"
                    sCD_ARTI_ECO = "TAED979300$"
                Case "9000003"
                    sCD_ARTI_ECO = "TAED979200$"
                Case "9000004"
                    sCD_ARTI_ECO = "TAED966100$"
                Case "9000005"
                    sCD_ARTI_ECO = "TAED966300$"
                Case "9000006"
                    sCD_ARTI_ECO = "TAED977800$"
                Case "9000007"
                    sCD_ARTI_ECO = "TAED977700$"
                Case "9000008"
                    sCD_ARTI_ECO = "TAED980400$"
                Case "9000009"
                    sCD_ARTI_ECO = "TAED980500$"
            End Select
            sCD_ARTI_ECO = SAP_DATA_CONV_CHAM(sCD_ARTI_ECO, "Code article", " ", "après")
            Using dtMARA = SAP_DATA_READ_MARA($"MATNR EQ '{sCD_ARTI_ECO}'")
                Using dtT179T = SAP_DATA_READ_T179T($"PRODH EQ '{dtMARA(0)("PRDHA").ToString}'")
                    'client
                    sNM_CLIE = dtT179T(0)("VTEXT").ToString
                End Using
            End Using
            sData = Replace(sData, "#NM_CLIE", sNM_CLIE)
            sData = Replace(sData, "#CD_ARTI_ECO", Trim(sCD_ARTI_ECO))
            Using dt_CFGR_ARTI_ECO = App_Web.Class_DIG_FACT_SQL.DIG_FACT_SQL_CFGR_ARTI_ECO(Trim(sCD_ARTI_ECO))
                If dt_CFGR_ARTI_ECO Is Nothing Then Throw New Exception($"La base Digital Factory n'a pas été configurée pour l'article {Trim(sCD_ARTI_ECO)}")
                Using dt_ETAT_CTRL = COMM_APP_WEB_ETAT_CTRL($"{sCD_ARTI_ECO}|{sTYPE_ETIQ}", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx")
                    If dt_ETAT_CTRL Is Nothing Then Throw New Exception($"La base App_Web_Eco n'as pas été configurée pour l'article {Trim(sCD_ARTI_ECO)}")

                    sData = Replace(sData, "#CD_ARTI_CLIE", dt_CFGR_ARTI_ECO(0)("Code article client").ToString)
                    sData = Replace(sData, "#NM_ARTI_CLIE", dt_CFGR_ARTI_ECO(0)("Désignation article client").ToString)
                    sData = Replace(sData, "#NU_BL", sNU_BL)
                    sData = Replace(sData, "#NU_CART", sNU_CART)
                    sData = Replace(sData, "#CD_FNS", App_Web.Class_DIG_FACT_SQL.DIG_FACT_SQL_GET_PARA(sNM_CLIE, "Format du code fournisseur"))

                    If dt_ETAT_CTRL.Columns.Contains("DropDownList_CRIT_GENE_NU_SER") Then
                        Select Case dt_ETAT_CTRL(0)("DropDownList_CRIT_GENE_NU_SER").ToString
                            Case "OF"
                                sData_Crit = sNU_OF
                            Case "Code Article"
                                sData_Crit = sCD_ARTI_ECO
                            Case "Client"
                                sData_Crit = sNM_CLIE
                        End Select
                    End If
                    If dt_ETAT_CTRL.Columns.Contains("CheckBox_NU_SER_CLIE_GENE_AUTO") And dt_ETAT_CTRL.Columns.Contains("TextBox_FORM_NU_CLIE") And dt_ETAT_CTRL.Columns.Contains("CheckBox_REPR_NU_SER_REBU") Then
                        If dt_ETAT_CTRL(0)("CheckBox_NU_SER_CLIE_GENE_AUTO").ToString = "True" Then
                            sNU_CLIE = App_Web.Class_DIG_FACT_SQL.DIG_FACT_SQL_GENE_NU_SER_CLIE(dt_CFGR_ARTI_ECO(0)("Encodage Numéro de série client").ToString,
                                                                                    dt_CFGR_ARTI_ECO(0)("Incrémentation flanc").ToString,
                                                                                    dt_ETAT_CTRL(0)("TextBox_FORM_NU_CLIE").ToString,
                                                                                    sData_Crit,
                                                                                    sTYPE_ETIQ,
                                                                                    dt_ETAT_CTRL(0)("CheckBox_REPR_NU_SER_REBU").ToString,
                                                                                    sMTCL,
                                                                                    sNU_OF)
                            If sNU_CLIE Is Nothing Then Throw New Exception("Erreur dans la génération du numéro de série client")
                        End If
                    End If

                    sQuery = $"SELECT [NU_OF] + ISNULL([NU_SER_ECO], '') AS NU_SER_ECO
                                     ,ISNULL([NU_SER_CLIE], '') AS NU_SER_CLIE
                                     ,ISNULL([NU_UDI], '') AS NU_UDI
                                     ,ISNULL([NU_ADRE_MAC], '') AS NU_ADRE_MAC
                                     ,ISNULL([NU_DEV_EUI], '') AS NU_DEV_EUI
                                 FROM [dbo].[V_LIAIS_NU_SER]
                                WHERE [ID_NU_SER] = {sID_NU_SER}"
                    Using dt = SQL_SELE_TO_DT(sQuery, CS_MES_Digital_Factory)
                        If Not dt Is Nothing Then
                            For Each cdt As DataColumn In dt.Columns
                                sData = Replace(sData, $"#{cdt.ColumnName}", dt(0)(cdt).ToString)
                            Next
                        End If
                    End Using
                    If dt_ETAT_CTRL.Columns.Contains("CheckBox_EXTR_NU_SER_ECO") Then
                        If dt_ETAT_CTRL(0)("CheckBox_EXTR_NU_SER_ECO").ToString = "True" And sNU_ECO.IndexOf(sNU_OF) + 1 > 0 Then sData = Replace(sData, "#EXTR_NU_SER_ECO", Mid(sNU_ECO, sNU_ECO.IndexOf(sNU_OF) + 1, 13))
                    End If
                    sData = Replace(sData, "#NB_QT", sNB_QT)
                    sData = Replace(sData, "#NB_CART", sNB_CART)

                    If dt_ETAT_CTRL.Columns.Contains("TextBox_FT_DT_PROD") Then sData = Replace(sData, "#DT_PROD", COMM_APP_WEB_CONV_FORM_DATE(sDT_PROD, dt_ETAT_CTRL(0)("TextBox_FT_DT_PROD").ToString))
                    ''''''''''''''''''''''''''''condition'''''''''''''''''''''''''''''''
                    If Trim(sCD_ARTI_ECO) = "STAGE950200$" Then
                        If dt_ETAT_CTRL.Columns.Contains("TextBox_FT_DT_EXP") Then sData = Replace(sData, "#DT_EXP", COMM_APP_WEB_CONV_FORM_DATE(App_Web.Class_DIG_FACT_SQL.EXTRACT_DATE(sNU_ECO), dt_ETAT_CTRL(0)("TextBox_FT_DT_EXP").ToString))
                    Else
                        If dt_ETAT_CTRL.Columns.Contains("TextBox_FT_DT_EXP") Then sData = Replace(sData, "#DT_EXP", COMM_APP_WEB_CONV_FORM_DATE(Now, dt_ETAT_CTRL(0)("TextBox_FT_DT_EXP").ToString))
                    End If

                    sData = Replace(sData, "#IND_CLIE", dt_CFGR_ARTI_ECO(0)("Indice client").ToString)
                    sData = Replace(sData, "#DDM", dt_CFGR_ARTI_ECO(0)("Nom du DDM").ToString)

                    If Not dtVar Is Nothing Then
                        For Each rdtVar As DataRow In dtVar.Rows
                            sData = Replace(sData, $"#var{Right(COMM_APP_WEB_CONV_DEC_2_BASE_N(dtVar.Rows.IndexOf(rdtVar) + 1, 36, 2), 1)}", rdtVar(1).ToString)
                        Next
                    End If
                    For iVar = 1 To 15
                        sData = Replace(sData, $"#var{Right(COMM_APP_WEB_CONV_DEC_2_BASE_N(iVar, 36, 2), 1)}", "")
                    Next
                End Using
            End Using
            Using sw = New StreamWriter(sfich, False, System.Text.Encoding.UTF8)
                sw.WriteLine(sData)
                sw.Close()
            End Using
            COMM_APP_WEB_IMPR_ETIQ_PRN(sfich, sNM_IPMT)

        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Exit Sub
        End Try
    End Sub
    Public Shared Function DIG_FACT_IMPR_PDF(sFichier As String, sNU_OF As String, sNU_BL As String, sTYPE_ETIQ As String,
                                         sNU_CLIE As String, sNU_ECO As String, sNU_CART As String, sNB_QT As String,
                                         sNB_CART As String, sNU_PAL As String, dtVar As DataTable,
                                         Optional dtLIST_DATA As DataTable = Nothing, Optional i_DEBU_LIGN As Integer = 0,
                                         Optional sDossier As String = "vide") As String

        Dim sData As String = "", sCD_ARTI_ECO As String = "", sNM_CLIE As String = "", sDSGT_ARTI As String = ""
        Dim sData_Crit As String = "", sDT_PROD As String = Now
        Dim dtAFKO, dtMARA, dtT179T, dtMAKT, dtLIPS, dtLIPSUP, dt, dt_CFGR_ARTI_ECO, dt_ETAT_CTRL As New DataTable
        Dim sr As StreamReader
        Dim document As Pdf.PdfDocument, page As PdfPage, gfx As XGraphics
        Dim sfich As String
        Dim icount As Integer = 0

        Randomize()
        If sDossier = "vide" Then
            sfich = $"{My.Settings.RPTR_TPRR}\delivery_form_{CInt(Int((1000 * Rnd()) + 1))}.pdf"
        Else
            sfich = $"{sDossier}\delivery_form_{CInt(Int((1000 * Rnd()) + 1))}.pdf"
        End If

        Try
            If File.Exists(sfich) Then My.Computer.FileSystem.DeleteFile(sfich)

            COMM_APP_WEB_COPY_FILE(sFichier, sfich, True)
            sr = New StreamReader(sfich, System.Text.Encoding.UTF8)
            sData = sr.ReadToEnd()
            sr.Close()

            If sNU_OF <> "" Then
                dt = PDF_REPL_VAR(dt, "#NU_OF", sNU_OF)
                dtAFKO = SAP_DATA_READ_AFKO($"AUFNR Like '%{sNU_OF}'")
                If Not dtAFKO Is Nothing Then
                    dtMAKT = SAP_DATA_READ_MAKT($"MATNR EQ '{dtAFKO(0)("PLNBEZ").ToString}'")
                    If dtMAKT Is Nothing Then Throw New Exception($"La désignation d'aticle de l'OF {sNU_OF} n'a pas été trouvé dans SAP.")
                    sCD_ARTI_ECO = dtAFKO(0)("PLNBEZ").ToString
                    sDSGT_ARTI = Trim(dtMAKT(0)("MAKTX").ToString)
                    sDT_PROD = dtAFKO(0)("GSTRS").ToString.Insert(6, "/").Insert(4, "/")
                End If
            ElseIf sNU_BL <> "" Then
                dtLIPSUP = SAP_DATA_READ_LIPSUP($"VBELN EQ {sNU_BL}")
                If Not dtLIPSUP Is Nothing Then
                    sCD_ARTI_ECO = SAP_DATA_CONV_CHAM(dtLIPSUP(0)("MATNR").ToString, "Code article", " ", "après")
                    sDSGT_ARTI = Trim(dtLIPSUP(0)("ARKTX").ToString)
                End If
                dtLIPS = SAP_DATA_READ_LIPS($"VBELN EQ {sNU_BL}")
                If Not dtLIPS Is Nothing Then dt = PDF_REPL_VAR(dt, "#NU_CMDE", dtLIPS(0)("VGBEL").ToString)
            Else
                Throw New Exception("Erreur d'appel à la fonction, pas de données d'entrée")
            End If
            dtMARA = SAP_DATA_READ_MARA($"MATNR EQ '{sCD_ARTI_ECO}'")
            dtT179T = SAP_DATA_READ_T179T($"PRODH EQ '{dtMARA(0)("PRDHA").ToString}'")
            'client
            sNM_CLIE = dtT179T(0)("VTEXT").ToString
            dt = PDF_REPL_VAR(dt, "#NM_CLIE", sNM_CLIE)
            dt_CFGR_ARTI_ECO = App_Web.Class_DIG_FACT_SQL.DIG_FACT_SQL_CFGR_ARTI_ECO(Trim(sCD_ARTI_ECO))
            If dt_CFGR_ARTI_ECO Is Nothing Then Throw New Exception($"La base Digital Factory n'a pas été configurée pour l'article {Trim(sCD_ARTI_ECO)}")
            dt_ETAT_CTRL = COMM_APP_WEB_ETAT_CTRL($"{sCD_ARTI_ECO}|{sTYPE_ETIQ}", "ASP.pagesmembres_digital_factory_impr_etiq_prn_aspx")
            If dt_ETAT_CTRL Is Nothing Then Throw New Exception($"La base App_Web_Eco n'as pas été configurée pour l'article {Trim(sCD_ARTI_ECO)}")
            dt = PDF_REPL_VAR(dt, "#CD_ARTI_ECO", Trim(sCD_ARTI_ECO))
            dt = PDF_REPL_VAR(dt, "#CD_ARTI_CLIE", dt_CFGR_ARTI_ECO(0)("Code article client").ToString)
            dt = PDF_REPL_VAR(dt, "#NU_BL", sNU_BL)
            dt = PDF_REPL_VAR(dt, "#NU_CART", sNU_CART)
            dt = PDF_REPL_VAR(dt, "#CD_FNS", App_Web.Class_DIG_FACT_SQL.DIG_FACT_SQL_GET_PARA(sNM_CLIE, "Format du code fournisseur"))

            'fiche palette
            'For iVar = 1 To Convert.ToDecimal(App_Web.Class_DIG_FACT_SQL.DIG_FACT_SQL_GET_PARA(Trim(sCD_ARTI_ECO), "Nombre de ligne dans le fichier PDF"))
            If Not dtLIST_DATA Is Nothing Then
                For iVar = 1 To Convert.ToDecimal(dt_CFGR_ARTI_ECO(0)("Nombre de ligne dans le fichier PDF").ToString)
                    Dim iINS_VAR As String = Right(COMM_APP_WEB_CONV_DEC_2_BASE_N(iVar, 36, 2), 1)
                    If (iVar + i_DEBU_LIGN) <= dtLIST_DATA.Rows.Count Then
                        dt = PDF_REPL_VAR(dt, $"#LINE{iINS_VAR}", iVar + i_DEBU_LIGN)

                        dt = PDF_REPL_VAR(dt, $"#NU_PALE{iINS_VAR}", sNU_PAL)
                        dt = PDF_REPL_VAR(dt, $"#NM_ARTI_CLIE{iINS_VAR}", dt_CFGR_ARTI_ECO(0)("Désignation article client").ToString)
                        If dt_ETAT_CTRL.Columns.Contains("TextBox_FT_DT_EXP") Then dt = PDF_REPL_VAR(dt, $"#DT_EXP{iINS_VAR}", COMM_APP_WEB_CONV_FORM_DATE(Now, dt_ETAT_CTRL(0)("TextBox_FT_DT_EXP").ToString))

                        For Each cdtLIST_DATA As DataColumn In dtLIST_DATA.Columns
                            dt = PDF_REPL_VAR(dt, $"#{cdtLIST_DATA.ColumnName}{iINS_VAR}", dtLIST_DATA(iVar + i_DEBU_LIGN - 1)(cdtLIST_DATA.ColumnName).ToString)
                        Next
                        dt = PDF_REPL_VAR(dt, $"#NM_DSGT_ARTI{iINS_VAR}", Trim(sDSGT_ARTI))
                        If dt_ETAT_CTRL.Columns.Contains("TextBox_FT_DT_EXP") Then dt = PDF_REPL_VAR(dt, $"#DT_GARA{iINS_VAR}", COMM_APP_WEB_CONV_FORM_DATE(DateAdd(DateInterval.Year, 1, Now), dt_ETAT_CTRL(0)("TextBox_FT_DT_EXP").ToString))
                    Else
                        dt = PDF_REPL_VAR(dt, $"#LINE{iINS_VAR}", "")
                        dt = PDF_REPL_VAR(dt, $"#NU_PALE{iINS_VAR}", "")
                        dt = PDF_REPL_VAR(dt, $"#NM_ARTI_CLIE{iINS_VAR}", "")
                        dt = PDF_REPL_VAR(dt, $"#DT_EXP{iINS_VAR}", "")
                        For Each cdtLIST_DATA As DataColumn In dtLIST_DATA.Columns
                            dt = PDF_REPL_VAR(dt, $"#{cdtLIST_DATA.ColumnName}{iINS_VAR}", "")
                        Next
                        dt = PDF_REPL_VAR(dt, $"#NM_DSGT_ARTI{iINS_VAR}", "")
                        dt = PDF_REPL_VAR(dt, $"#DT_GARA{iINS_VAR}", "")
                    End If
                Next
            End If
            dt = PDF_REPL_VAR(dt, "#NM_DSGT_ARTI", sDSGT_ARTI)
            dt = PDF_REPL_VAR(dt, "#NU_SER_CLIE", sNU_CLIE)
            dt = PDF_REPL_VAR(dt, "#NU_SER_ECO", sNU_ECO)
            If dt_ETAT_CTRL.Columns.Contains("CheckBox_EXTR_NU_SER_ECO") Then
                If dt_ETAT_CTRL(0)("CheckBox_EXTR_NU_SER_ECO").ToString = "True" And sNU_ECO.IndexOf(sNU_OF) + 1 > 0 Then dt = PDF_REPL_VAR(dt, "#EXTR_NU_SER_ECO", Mid(sNU_ECO, sNU_ECO.IndexOf(sNU_OF) + 1, 13))
            End If

            dt = PDF_REPL_VAR(dt, "#NB_QT", sNB_QT)
            dt = PDF_REPL_VAR(dt, "#NB_CART", sNB_CART)
            If dt_ETAT_CTRL.Columns.Contains("TextBox_FT_DT_PROD") Then dt = PDF_REPL_VAR(dt, "#DT_PROD", COMM_APP_WEB_CONV_FORM_DATE(sDT_PROD, dt_ETAT_CTRL(0)("TextBox_FT_DT_PROD").ToString))
            If dt_ETAT_CTRL.Columns.Contains("TextBox_FT_DT_EXP") Then dt = PDF_REPL_VAR(dt, "#DT_EXP", COMM_APP_WEB_CONV_FORM_DATE(Now, dt_ETAT_CTRL(0)("TextBox_FT_DT_EXP").ToString))
            dt = PDF_REPL_VAR(dt, "#IND_CLIE", dt_CFGR_ARTI_ECO(0)("Indice client").ToString)
            dt = PDF_REPL_VAR(dt, "#DDM", dt_CFGR_ARTI_ECO(0)("Nom du DDM").ToString)
            If Not dtVar Is Nothing Then
                For Each rdtVar As DataRow In dtVar.Rows
                    dt = PDF_REPL_VAR(dt, $"#var{Right(COMM_APP_WEB_CONV_DEC_2_BASE_N(dtVar.Rows.IndexOf(rdtVar) + 1, 36, 2), 1)}", rdtVar(1).ToString)
                    icount += 1
                Next
            End If
            For iVar = icount To 15
                dt = PDF_REPL_VAR(dt, $"#var{Right(COMM_APP_WEB_CONV_DEC_2_BASE_N(iVar, 36, 2), 1)}", "")
            Next

            document = PdfSharp.Pdf.IO.PdfReader.Open(sfich)
            page = document.Pages.Item(0)
            gfx = XGraphics.FromPdfPage(page)
            '  page.
            PDF_CHAR_CONF(gfx, $"{sFichier}_config", dt)
            document.Save(sfich)
            document.Close()
            If Not dt_CFGR_ARTI_ECO(0)("Chemin de sauvegarde du fichier PDF").ToString = "" Then COMM_APP_WEB_COPY_FILE(sfich, $"{dt_CFGR_ARTI_ECO(0)("Chemin de sauvegarde du fichier PDF").ToString}\delivery_form_{COMM_APP_WEB_CONV_FORM_DATE(Now, "yyyyMMddHHmmss")}.pdf", True)  'sauvegarde réseau

        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try
        Return sfich
    End Function

End Class

Public Class Class_DIG_FACT_SQL

    Public Shared Function DIG_FACT_SQL_P_ADD_TCBL_MAT_NS_ENS_V2(iID_Passage As Long, sID_PF As String, sRepere As String, sEtape As String, sCode_SAP_Cpt As String, sNS_SENS As String, sOF As String, sLB_MOYN As String, sLB_PROG As String, sNM_MATR As String, sID_CPT As String) As String

        'Dim SQL_Connexion = New SqlConnection()
        'Dim cmd As New SqlCommand
        'Dim resultat As String = ""

        Try
            If sNM_MATR = "" Then sNM_MATR = "0"
            'SQL_Connexion = SQL_CONN(CS_MES_Digital_Factory)
            'cmd = SQL_CALL_STOR_PROC(SQL_Connexion, "P_ADD_TCBL_MAT_NS_ENS_V4")
            ''            For Each rNS_CD_ART As DataRow In dtNS_CD_ART.Rows
            'SQL_ADD_PARA_STOR_PROC(cmd, "ID_PF", SqlDbType.VarChar, 100, sID_PF)
            'SQL_ADD_PARA_STOR_PROC(cmd, "Repere", SqlDbType.VarChar, 10, sRepere)
            'SQL_ADD_PARA_STOR_PROC(cmd, "Code_SAP_Cpt", SqlDbType.VarChar, 20, sCode_SAP_Cpt)
            'SQL_ADD_PARA_STOR_PROC(cmd, "NS_SENS", SqlDbType.VarChar, 100, sNS_SENS)
            'SQL_ADD_PARA_STOR_PROC(cmd, "Etape", SqlDbType.VarChar, 255, sEtape)
            'SQL_ADD_PARA_STOR_PROC(cmd, "LB_MOYN", SqlDbType.VarChar, 4000, sLB_MOYN)
            'SQL_ADD_PARA_STOR_PROC(cmd, "LB_PROG", SqlDbType.VarChar, 4000, sLB_PROG)
            'SQL_ADD_PARA_STOR_PROC(cmd, "NM_MATR", SqlDbType.Int, 100000, sNM_MATR)
            'SQL_ADD_PARA_STOR_PROC(cmd, "Of", SqlDbType.VarChar, 4000, sOF)
            'SQL_ADD_PARA_STOR_PROC(cmd, "ID_CPT", SqlDbType.VarChar, 4000, sID_CPT)
            'SQL_ADD_PARA_STOR_PROC(cmd, "ID_Passage", SqlDbType.BigInt, 200, iID_Passage, "Output")
            'SQL_ADD_PARA_STOR_PROC(cmd, "Result", SqlDbType.VarChar, 4000, "", "Output")
            'SQL_EXEC_STOR_PROC(cmd)
            'iID_Passage = SQL_GET_PARA_VAL(cmd, "ID_Passage")
            'resultat = SQL_GET_PARA_VAL(cmd, "Result")
            'If resultat = "Erreur" Then Throw New Exception("Erreur")
            'cmd.Parameters.Clear()
            '           Next
            Dim iID_PSG = New Entity.Core.Objects.ObjectParameter("ID_Passage", iID_Passage)
            Dim sRes = New Entity.Core.Objects.ObjectParameter("Result", GetType(String))
            Using db As New MES_Digital_FactoryEntities
                db.P_ADD_TCBL_MAT_NS_ENS_V4(sID_PF, sRepere, sCode_SAP_Cpt, sNS_SENS, sEtape, sLB_MOYN, sLB_PROG, Convert.ToInt16(sNM_MATR), sOF, sID_CPT, iID_PSG, sRes)
                If sRes.Value = "Erreur" Then Throw New Exception(sRes.Value)
                Return iID_PSG.Value
            End Using

        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
            'Finally
            '    SQL_Connexion = SQL_CLOS(SQL_Connexion)
        End Try


    End Function

    Public Shared Function DIG_FACT_SQL_GET_PARA(sCRIT As String, sNM_PARA As String) As String

        Dim sQuerySql As String = $"Select [CD_ARTI], [NM_PARA], [VAL_PARA]
                                      From [dbo].[V_DER_DTM_REF_PARA]
                                     Where [CD_ARTI] = '{sCRIT}' AND [NM_PARA] = '{sNM_PARA}'"
        Try
            Using dt_PARA = SQL_SELE_TO_DT(sQuerySql, CS_MES_Digital_Factory)
                If dt_PARA Is Nothing Then Throw New Exception("La base n'a pas été configurée")
                Return dt_PARA(0)("VAL_PARA").ToString
            End Using
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try

    End Function

    Public Shared Function DIG_FACT_SQL_P_GET_TRAC_NU_SER_ENS_NU_SER_SS_ENS(sOF As String, sEtape As String) As DataTable

        Dim SQL_Connexion = New SqlConnection()
        Dim cmd As New SqlCommand
        Dim dt_result As New DataTable
        Dim ID_Passage As Integer = 0

        Try

            SQL_Connexion = SQL_CONN(CS_MES_Digital_Factory)
            cmd = SQL_CALL_STOR_PROC(SQL_Connexion, "P_GET_TRAC_NU_SER_ENS_NU_SER_SS_ENS")
            SQL_ADD_PARA_STOR_PROC(cmd, "OF", SqlDbType.VarChar, 4000, sOF)
            SQL_ADD_PARA_STOR_PROC(cmd, "ETAP", SqlDbType.VarChar, 4000, sEtape)
            SQL_ADD_PARA_STOR_PROC(cmd, "RET", SqlDbType.VarChar, 4000, "", "Output")
            dt_result = SQL_GET_DT_FROM_STOR_PROC(cmd)
            SQL_EXEC_STOR_PROC(cmd)
            If SQL_GET_PARA_VAL(cmd, "RET") <> "PASS" Then Throw New Exception(SQL_GET_PARA_VAL(cmd, "RET"))
            If dt_result Is Nothing Then Throw New Exception("vide")
            cmd.Parameters.Clear()

            'Dim sRes = New Entity.Core.Objects.ObjectParameter("RET", GetType(String))
            'Using db As New MES_Digital_FactoryEntities
            '    Dim rs As IQueryable = db.P_GET_TRAC_NU_SER_ENS_NU_SER_SS_ENS(sOF, sEtape, sRes)
            '    If sRes.Value <> "PASS" Then Throw New Exception(sRes.Value)
            '    If rs Is Nothing Then Throw New Exception("Le résultat est nul")
            '    For Each item In rs

            '    Next
            'End Using
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        Finally
            SQL_Connexion = SQL_CLOS(SQL_Connexion)
        End Try

        Return dt_result

    End Function

    Public Shared Function DIG_FACT_SQL_P_ADD_PSG_DTM_PSG(sLB_ETP As String, dDT_DEB As Date, dDT_FIN As Date, sLB_MOYN As String, sLB_PROG As String, siNM_MATR As Integer, sLB_DOC As String, sNM_NS_EOL As String, sLB_SCTN As String, sNM_OF As Integer) As String
        Dim SQL_Connexion = New SqlConnection()
        Dim cmd As New SqlCommand
        Dim sid_passage As String = ""

        Try
            SQL_Connexion = SQL_CONN(CS_MES_Digital_Factory)
            cmd = SQL_CALL_STOR_PROC(SQL_Connexion, "P_ADD_PSG_DTM_PSG")
            SQL_ADD_PARA_STOR_PROC(cmd, "LB_ETP", SqlDbType.VarChar, 255, sLB_ETP)
            SQL_ADD_PARA_STOR_PROC(cmd, "DT_DEB", SqlDbType.DateTime2, 255, dDT_DEB)
            SQL_ADD_PARA_STOR_PROC(cmd, "DT_FIN", SqlDbType.DateTime2, 255, dDT_FIN)
            SQL_ADD_PARA_STOR_PROC(cmd, "LB_MOYN", SqlDbType.VarChar, 255, sLB_MOYN)
            SQL_ADD_PARA_STOR_PROC(cmd, "LB_PROG", SqlDbType.VarChar, 255, sLB_PROG)
            'SQL_ADD_PARA_STOR_PROC(cmd, "NM_MATR", SqlDbType.SmallInt, 4, siNM_MATR)
            SQL_ADD_PARA_STOR_PROC(cmd, "NM_MATR", SqlDbType.Int, 100000, siNM_MATR)
            SQL_ADD_PARA_STOR_PROC(cmd, "LB_DOC", SqlDbType.VarChar, 255, sLB_DOC)
            SQL_ADD_PARA_STOR_PROC(cmd, "NM_NS_EOL", SqlDbType.VarChar, 100, sNM_NS_EOL)
            SQL_ADD_PARA_STOR_PROC(cmd, "LB_SCTN", SqlDbType.VarChar, 1, sLB_SCTN)
            SQL_ADD_PARA_STOR_PROC(cmd, "NM_OF", SqlDbType.Int, 1000000, sNM_OF)
            SQL_ADD_PARA_STOR_PROC(cmd, "Result", SqlDbType.VarChar, 100, "", "Output")
            SQL_EXEC_STOR_PROC(cmd)

            If SQL_GET_PARA_VAL(cmd, "Result") = "Erreur" Then Throw New Exception(SQL_GET_PARA_VAL(cmd, "Result"))
            sid_passage = Replace(SQL_GET_PARA_VAL(cmd, "Result"), "OK-", "")
            cmd.Parameters.Clear()

        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        Finally
            SQL_Connexion = SQL_CLOS(SQL_Connexion)
        End Try
        Return sid_passage
    End Function

    Public Shared Sub DIG_FACT_SQL_SET_PARA(sCRIT As String, sNM_PARA As String, sVAL_PARA As String)

        Dim sQuerySql As String = $"INSERT INTO [dbo].[DTM_REF_PARA]([NM_CRIT],[NM_PARA],[VAL_PARA],[DT_PARA])
                                         VALUES ('{Replace(sCRIT, "'", "''")}', '{Replace(sNM_PARA, "'", "''")}','{Replace(sVAL_PARA, "'", "''")}' ,GETDATE())"
        Try
            SQL_REQ_ACT(sQuerySql, CS_MES_Digital_Factory)
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try
    End Sub

    Public Shared Function DIG_FACT_SQL_GET_ASCT_ENS_SS_ENS_BY_CD_ART_SAP(sCD_ART_ENS As String) As DataTable
        Dim sQuerySql As String = $"SELECT [CD_SAP_SENS]
                                         ,[CD_SAP_ENS]
                                     FROM [dbo].[V_DTM_REF_ENS]
                                    WHERE [CD_SAP_ENS] = '{sCD_ART_ENS}'"
        'Dim sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=MES_Digital_Factory;Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
        Dim dt As New DataTable

        Try
            dt = SQL_SELE_TO_DT(sQuerySql, CS_MES_Digital_Factory)
            If dt Is Nothing Then Throw New Exception("La base n'a pas été configurée")
        Catch ex As Exception
            'LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try
        Return dt
    End Function

    Public Shared Function DIG_FACT_SQL_GENE_NU_SER_CLIE(sBase As String, sInc As String, sFormat As String, sCritère As String, sTypeEtiquette As String, bREPR_NU_SER_REBU As Boolean,
                                                         sMTCL As String, sNU_OF As String) As String

        'Dim sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=MES_Digital_Factory;Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
        Dim sQuerySql As String = "", sNU_SER_BASE_N As String = "", sNU_SER_GENE As String = "", sNU_SER_INC As String = ""
        'Dim dtDER_NU_SER, dtNU_REGE, dt_NU_SER As New DataTable
        Dim iNU_SER_DEC, iNB_CARA As Integer, iPos_Car As Integer = 1

        Try
            If sBase = "" Or sInc = "" Or sFormat = "" Then Throw New Exception("un champ de configuration du numéro de série client est vide")
            'cas particulier pour médria MEDEM2IAX$, incrémentation unique pour les codes articles MEDEM2IAX-EU$ et MEDEM2IAX-US$
            sCritère = Replace(sCritère, "M2IAX-EU$", "M2IAX$")
            sCritère = Replace(sCritère, "M2IAX-US$", "M2IAX$")

            'récupérer le premier numéro de série rebuté à réimprimer
            'sQuerySql = "SELECT     MIN(NU_SER) AS [NU_REGE]
            '               FROM     dbo.V_NU_SER_A_RGNR
            '             GROUP BY   NM_CRIT
            '             HAVING     (NM_CRIT = '{sCritère}')"
            'dtNU_REGE = SQL_SELE_TO_DT(sQuerySql, sChaineConnexion)
            'If bREPR_NU_SER_REBU = True And Not dtNU_REGE Is Nothing Then
            '    sNU_SER_GENE = dtNU_REGE(0)("NU_REGE").ToString
            'Else
            'récupérer le dernier généré
            sQuerySql = $"SELECT [NU_SER_DERN]
                            FROM [dbo].[V_DER_DTM_NU_SER]
                           WHERE [NM_CRIT] = '{sCritère} ' AND [NM_TYPE] = '{sTypeEtiquette}'"
            Using dtDER_NU_SER = SQL_SELE_TO_DT(sQuerySql, CS_MES_Digital_Factory)            '
                'convertir en base 10
                If dtDER_NU_SER Is Nothing Then
                    iNU_SER_DEC = COMM_APP_WEB_CONV_BASE_N_2_DEC("1", Convert.ToDecimal(sBase))
                Else
                    Dim sb As New StringBuilder
                    For iChar As Integer = 1 To Len(sFormat) 'extraction de la partie incrémentale
                        If Mid(sFormat, iChar, 1) = "%" Then sb.Append(Mid(dtDER_NU_SER(0)("NU_SER_DERN").ToString, iChar, 1)) 'sNU_SER_INC &= Mid(dtDER_NU_SER(0)("NU_SER_DERN").ToString, iChar, 1)
                    Next
                    iNU_SER_DEC = COMM_APP_WEB_CONV_BASE_N_2_DEC(sb.ToString, Convert.ToDecimal(sBase)) 'sNU_SER_INC, Convert.ToDecimal(sBase))
                End If
            End Using
            'incrémenter
            iNU_SER_DEC += Convert.ToDecimal(sInc)
            'convertir en base N

            For iChar As Integer = 1 To Len(sFormat)
                If Mid(sFormat, iChar, 1) = "%" Then iNB_CARA += 1
            Next
            LOG_Msg(GetCurrentMethod, iNU_SER_DEC)
            sNU_SER_BASE_N = COMM_APP_WEB_CONV_DEC_2_BASE_N(iNU_SER_DEC, Convert.ToDecimal(sBase), iNB_CARA)
            LOG_Msg(GetCurrentMethod, sNU_SER_BASE_N)

            'appliquer le format
            Dim sb2 As New StringBuilder
            For iChar As Integer = 1 To Len(sFormat)
                If Mid(sFormat, iChar, 1) = "%" Then
                    sb2.Append(Mid(sNU_SER_BASE_N, iPos_Car, 1))
                    'sNU_SER_GENE = sNU_SER_GENE & Mid(sNU_SER_BASE_N, iPos_Car, 1)
                    iPos_Car += 1
                Else
                    sb2.Append(Mid(sFormat, iChar, 1))
                    'sNU_SER_GENE = sNU_SER_GENE & Mid(sFormat, iChar, 1)
                End If
            Next
            'End If
            'Enregistrer dans la base
            'sQuerySql = $"INSERT INTO [dbo].[DTM_NU_SER] ([NM_CRIT], [NU_SER], [NM_TYPE], [DT_CREA])
            '                  VALUES ('{sCritère} ', '{sNU_SER_GENE}', '{sTypeEtiquette}', GETDATE())"
            sQuerySql = $"INSERT INTO [dbo].[DTM_NU_SER] ([NM_CRIT], [NU_SER], [NM_TYPE], [DT_CREA])
                              VALUES ('{sCritère} ', '{sb2.ToString}', '{sTypeEtiquette}', GETDATE())"
            SQL_REQ_ACT(sQuerySql, CS_MES_Digital_Factory)
            'Récupération de 'ID_NU_SER pour enregistrer la liaison OF <--> Numéro de série
            sQuerySql = $"SELECT [ID_NU_SER]
                            FROM [dbo].[DTM_NU_SER]
                           WHERE [NM_CRIT] = '{sCritère} '
                             And [NU_SER] = '{sb2.ToString}'
                             And [NM_TYPE] = '{sTypeEtiquette}'"
            Using dt_NU_SER = SQL_SELE_TO_DT(sQuerySql, CS_MES_Digital_Factory)
                If dt_NU_SER Is Nothing Then Throw New Exception("Pas d'ID_NU_SER trouvé")
                sQuerySql = $"INSERT INTO [dbo].[DTM_NU_SER] ([NM_CRIT], [NU_SER], [NM_TYPE], [DT_CREA], [ID_NU_SER])
                               VALUES ('{sCritère} ', '{sNU_OF}', 'OF', GETDATE(), '{dt_NU_SER(0)("ID_NU_SER").ToString}')"
                SQL_REQ_ACT(sQuerySql, CS_MES_Digital_Factory)
            End Using
            'DIG_FACT_SQL_P_ADD_PSG_DTM_PSG("Impression numéro de série client", Now(), Now(), System.Net.Dns.GetHostEntry(System.Web.HttpContext.Current.Request.UserHostAddress).HostName(), HttpContext.Current.CurrentHandler.ToString, sMTCL, "", sNU_SER_GENE, "P", sNU_OF)
            LOG_Msg(GetCurrentMethod, $"Le numéro de série client {sb2.ToString} a été généré.")
            Return sb2.ToString
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try

    End Function

    Public Shared Function DIG_FACT_SQL_CFGR_ARTI_ECO(sCD_ARTI_ECO As String) As DataTable
        Try
            Using dt = SQL_SELE_TO_DT($"SELECT *
                                         FROM [dbo].[V_CFGR_ARTI_ECO]
                                        WHERE [CD_ARTI] = '{sCD_ARTI_ECO}'", CS_MES_Digital_Factory)
                Return dt
            End Using
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try
    End Function
    Public Shared Function DIG_FACT_SQL_CFGR_CLIE(sNM_CLIE As String) As DataTable
        Try
            Using dt = SQL_SELE_TO_DT($"SELECT *
                                          FROM [dbo].[V_CFGR_CLIE]
                                         WHERE [NM_CLIE] = '{sNM_CLIE}'", CS_MES_Digital_Factory)
                Return dt
            End Using
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try
    End Function
    Public Shared Function DIG_FACT_SQL_VRFC_WF(sNU_SER_ECO As String, sNU_SER_CLIE As String, sNU_OF As String, sLB_NU_OPRT As String, Optional sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog=MES_Digital_Factory;Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False") As Boolean
        Dim sQuery As String = "", sQuery_WF As String = ""
        Try
            Using dtAFKO = SAP_DATA_READ_AFKO($"AUFNR LIKE '%{sNU_OF}'")
                If dtAFKO Is Nothing Then Throw New Exception($"La désignation d'aticle de l'OF {sNU_OF} n'a pas été trouvé dans SAP.")

                'vérification workflow
                sQuery = $"SELECT REPLACE(NM_PARA, 'WORKFLOW étape ', '') AS NU_ETAP
                             FROM dbo.V_DER_DTM_REF_PARA
                            WHERE NM_PARA LIKE N'WORKFLOW étape%'
                              AND CD_ARTI = '{Trim(dtAFKO(0)("PLNBEZ").ToString)}'
                              AND VAL_PARA = '{sLB_NU_OPRT}'"
                Using dt = SQL_SELE_TO_DT(sQuery, CS_MES_Digital_Factory)
                    If dt Is Nothing Then Throw New Exception("Pas de Workflow configuré dans la base. Prévenir un Méthode")

                    If dt(0)("NU_ETAP").ToString <> "1" Then 'Première opération dans le workflow --> pas de vérification
                        sQuery_WF = $"SELECT [NM_ETAP_PCDT]      
                                        FROM [dbo].[V_WORK_PASS]
                                       WHERE [NM_ETAP] = '{sLB_NU_OPRT} ' 
                                         And [NM_OF] = '{sNU_OF}' 
                                         And ([NU_SER_ECO] Like '%{sNU_SER_ECO}%' OR [NU_SER_ECO] Like '%{Right(sNU_SER_ECO, 13)}%')
                                         And [NU_SER_CLIE] Like '%{sNU_SER_CLIE}%'"
                        'LOG_Msg(GetCurrentMethod, sQuery_WF)
                        Using dt_WF = SQL_SELE_TO_DT(sQuery_WF, sChaineConnexion)
                            If dt_WF Is Nothing Then Throw New Exception($"Le numéro de série {sNU_SER_ECO}{sNU_SER_CLIE} n'est pas passé à l'étape précédente")
                        End Using
                    End If
                End Using
            End Using
            Return True
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return False
        End Try
    End Function
    Public Shared Function EXTRACT_DATE(sNU_ECO As String) As String
        Dim sEntree = sNU_ECO.Substring(22, 6)
        Dim sMid = sEntree.Insert(2, "-")
        Dim sFinal = sMid.Insert(5, "-")
        Return $"20{sFinal}"
    End Function

    Public Shared Function DIG_FACT_SQL_ERGT_LIAI_ID_NU_SER(sNU_OF As String, sNU_SER As String) As String
        'Dim dtAFKO, dt_CFGR_ARTI_ECO, dt As New DataTable
        Dim sQuery As String = "", sNU_SER_ECO As String = "", sNU_SER_CLIE As String = "" ', sChaineConnexion As String = "Data Source=cedb03,1433;Initial Catalog={Replace(Replace(My.Computer.Name, "CEDB03", "MES_Digital_Factory_DEV"), "CEAPP03", "MES_Digital_Factory")};Integrated Security=False;User ID=sa;Password=mdpsa@SQL;Connect Timeout=7200;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
        Try
            Using dtAFKO = SAP_DATA_READ_AFKO($"AUFNR Like '%{sNU_OF}'")
                If dtAFKO Is Nothing Then Throw New Exception($"L'OF n° {sNU_OF} n'existe pas dans SAP.")
                Using dt_CFGR_ARTI_ECO = DIG_FACT_SQL_CFGR_ARTI_ECO(Trim(dtAFKO(0)("PLNBEZ").ToString))
                    If dt_CFGR_ARTI_ECO Is Nothing Then Throw New Exception($"L'article {Trim(dtAFKO(0)("PLNBEZ").ToString)} de l'OF n° {sNU_OF} n'existe pas dans SAP.")
                    Select Case "1"
                        Case dt_CFGR_ARTI_ECO(0)("Numéro de série Eolane").ToString
                            sNU_SER_ECO = sNU_SER
                            sQuery = $"SELECT [ID_NU_SER]
	                             FROM [dbo].[V_LIAIS_NU_SER]
	                            WHERE [NU_SER_ECO] LIKE '%{sNU_SER_ECO}%'	                    
	                              And [NU_OF] = '{sNU_OF}'"
                        Case dt_CFGR_ARTI_ECO(0)("Numéro de série client").ToString
                            sNU_SER_CLIE = sNU_SER
                            sQuery = $"SELECT [ID_NU_SER]
	                                     FROM [dbo].[V_LIAIS_NU_SER]
	                                    WHERE [NU_SER_CLIE] = '{sNU_SER_CLIE} '
	                                      And [NU_OF] = '{sNU_OF}'"
                    End Select
                    Using dt = SQL_SELE_TO_DT(sQuery, CS_MES_Digital_Factory)
                        If dt Is Nothing Then
                            Dim dt_CREA As DateTime = Now
                            sQuery = $"INSERT INTO [dbo].[DTM_NU_SER] ([NM_CRIT],[NU_SER],[NM_TYPE],[DT_CREA])
                                            VALUES ('{sNU_OF} ', '{sNU_OF}', 'OF', '{dt_CREA}')"
                            SQL_REQ_ACT(sQuery, CS_MES_Digital_Factory)
                            sQuery = $"SELECT [ID_NU_SER]
	                                     FROM [dbo].[DTM_NU_SER]
	                                    WHERE [NM_CRIT] = '{sNU_OF} '
	                                      And [NU_SER] = '{sNU_OF}'
	                                      And [NM_TYPE] = 'OF'
                                          And [DT_CREA] = '{dt_CREA}'"
                            Using dt2 = SQL_SELE_TO_DT(sQuery, CS_MES_Digital_Factory)
                                Select Case "1"
                                    Case dt_CFGR_ARTI_ECO(0)("Numéro de série Eolane").ToString
                                        sQuery = $"INSERT INTO [dbo].[DTM_NU_SER] ([NM_CRIT],[NU_SER],[NM_TYPE],[DT_CREA],[ID_NU_SER])
                                       VALUES ('{sNU_OF}', '{sNU_SER_ECO}', 'Numéro de série Eolane', '{dt_CREA}', {dt2(0)("ID_NU_SER").ToString})"
                                    Case dt_CFGR_ARTI_ECO(0)("Numéro de série client").ToString
                                        sQuery = $"INSERT INTO [dbo].[DTM_NU_SER] ([NM_CRIT],[NU_SER],[NM_TYPE],[DT_CREA],[ID_NU_SER])
                                       VALUES ('{sNU_OF} ', '{sNU_SER_CLIE}', 'Numéro de série client', '{dt_CREA}', {dt2(0)("ID_NU_SER").ToString})"
                                End Select
                                SQL_REQ_ACT(sQuery, CS_MES_Digital_Factory)
                                Return dt2(0)("ID_NU_SER").ToString
                            End Using
                        Else
                            Return dt(0)("ID_NU_SER").ToString
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try
    End Function
End Class
