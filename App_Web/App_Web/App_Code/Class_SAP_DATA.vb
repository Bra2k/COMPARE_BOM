Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports System.Data
Imports App_Web.Class_COMM_APP_WEB


Public Class Class_SAP_DATA

    Public Shared Function SAP_DATA_MATR(sMatricule As String) As DataTable 'Obtenir le nom & prénom à partir du matricule
        Dim dtPA0002 As New DataTable
        Try
            dtPA0002 = SAP_DATA_READ_TBL("PA0002", "|", "", "PERNR NACHN VORNA", "PERNR EQ '*" & sMatricule & "'")
            If dtPA0002 Is Nothing Then Throw New Exception("pas de nom/prénom trouvés pour le matricule " & sMatricule)
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try
        LOG_Msg(GetCurrentMethod, "La liste de client a été extraite de SAP.")

        Return dtPA0002

    End Function

    Public Shared Function SAP_DATA_LIST_CLIE(Optional sFiltre As String = "") As DataTable

        Dim dtT179T As New DataTable

        Try
            dtT179T = SAP_DATA_READ_T179T(sFiltre)
            If dtT179T Is Nothing Then Throw New Exception("Pas de client trouvé dans la table T179T, filtre : " & sFiltre)
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try

        LOG_Msg(GetCurrentMethod, "La liste de client a été extraite de SAP.")
        Return dtT179T

    End Function

    Public Shared Function SAP_DATA_NMCT_ARTI(sArticle As String) As DataTable

        Dim dtNMCT_ARTI, dtMAST, dtSTAS_LKENZ_NE_X, dtSTAS_LKENZ_EQ_X, dtSTPO, dtMAKT, dtMARC, dtNMCT_ARTI_2, dtSTPU, dtNMCT_ARTI_3, dtAFKO As New DataTable
        'Dim rAFKO As DataRow
        'Dim MaxOF As Object
        Try
            With dtNMCT_ARTI.Columns
                .Add("N° Nomenclature", Type.GetType("System.String"))
                .Add("N° Noeud", Type.GetType("System.String"))
                .Add("Compteur", Type.GetType("System.String"))
                .Add("Composant", Type.GetType("System.String"))
                .Add("Désignation", Type.GetType("System.String"))
                .Add("Qté", Type.GetType("System.String"))
                .Add("Fab. Interne/Externe", Type.GetType("System.String"))
            End With

            'Extraction de N° de Nomenclature de la table AFKO
            dtMAST = SAP_DATA_READ_TBL("MAST", "|", "", "STLNR STLAL", "MATNR EQ '" & sArticle & "' and WERKS EQ 'DI31'", 1)
            If dtMAST Is Nothing Then Throw New Exception("Pas de résultats dans la table MAST pour l'article " & sArticle)
            'dtAFKO = SAP_DATA_READ_AFKO("PLNBEZ EQ '" & sArticle & "'")
            'MaxOF = dtAFKO.Compute("MAX(AUFNR)", "")
            'rAFKO = dtAFKO.Select("AUFNR = '" & MaxOF.ToString() & "'").FirstOrDefault

            'Extraction des données des postes de la gamme
            dtSTAS_LKENZ_NE_X = SAP_DATA_READ_TBL("STAS", "|", "", "STLNR STLKN STVKN", "STLNR EQ '" & dtMAST.Rows(0)("STLNR") & "' and STLAL EQ '" & dtMAST.Rows(0)("STLAL") & "' and  LKENZ NE 'X'", 1)
            'dtSTAS_LKENZ_NE_X = SAP_DATA_READ_TBL("STAS", "|", "", "STLNR STLKN STVKN", "STLNR EQ '" & rAFKO("STLNR") & "' and STLAL EQ '" & rAFKO("STLAL") & "' and  LKENZ NE 'X'", 1)
            If dtSTAS_LKENZ_NE_X Is Nothing Then Throw New Exception("Pas de résultats dans la table STAS pour l'article " & sArticle)

            dtSTAS_LKENZ_EQ_X = SAP_DATA_READ_TBL("STAS", "|", "", "STLNR STLKN STVKN", "STLNR EQ '" & dtMAST.Rows(0)("STLNR") & "' and STLAL EQ '" & dtMAST.Rows(0)("STLAL") & "' and  LKENZ EQ 'X'", 1)
            'dtSTAS_LKENZ_EQ_X = SAP_DATA_READ_TBL("STAS", "|", "", "STLNR STLKN STVKN", "STLNR EQ '" & rAFKO("STLNR") & "' and STLAL EQ '" & rAFKO("STLAL") & "' and  LKENZ EQ 'X'", 1)
            If dtSTAS_LKENZ_EQ_X Is Nothing Then Throw New Exception("Pas de résultats dans la table STAS pour l'article " & sArticle)

            For Each rSTAS_LKENZ_NE_X As DataRow In dtSTAS_LKENZ_NE_X.Rows
                Dim rdtSTAS_LKENZ_EQ_X As DataRow = dtSTAS_LKENZ_EQ_X.Select("STLKN = '" & rSTAS_LKENZ_NE_X("STLKN").ToString & "'").FirstOrDefault()
                If rdtSTAS_LKENZ_EQ_X Is Nothing Then
                    dtNMCT_ARTI.Rows.Add()
                    dtNMCT_ARTI.Rows(dtNMCT_ARTI.Rows.Count - 1)("N° Nomenclature") = rSTAS_LKENZ_NE_X("STLNR").ToString
                    dtNMCT_ARTI.Rows(dtNMCT_ARTI.Rows.Count - 1)("N° Noeud") = rSTAS_LKENZ_NE_X("STLKN").ToString
                End If
            Next
            Dim sQuery As String = ""
            For Each rNMCT_ARTI As DataRow In dtNMCT_ARTI.Rows
                sQuery = sQuery & SAP_DATA_CREA_FILT_TABL("(STLNR EQ '" & rNMCT_ARTI("N° Nomenclature").ToString & "' and STLKN EQ '" & rNMCT_ARTI("N° Noeud").ToString & "')", " Or ")
            Next
            sQuery = Right(sQuery, Len(sQuery) - 5)
            dtSTPO = SAP_DATA_READ_TBL("STPO", "|", "", "STLNR STLKN STPOZ IDNRK MENGE", sQuery, 1)

            For Each rNMCT_ARTI As DataRow In dtNMCT_ARTI.Rows
                Dim rdtSTPO As DataRow = dtSTPO.Select("STLNR = '" & rNMCT_ARTI("N° Nomenclature").ToString & "' AND STLKN = '" & rNMCT_ARTI("N° Noeud").ToString & "'").FirstOrDefault()
                If Not rdtSTPO Is Nothing Then
                    rNMCT_ARTI("Compteur") = rdtSTPO("STPOZ").ToString
                    rNMCT_ARTI("Composant") = rdtSTPO("IDNRK").ToString
                    rNMCT_ARTI("Qté") = rdtSTPO("MENGE").ToString
                End If
            Next

            sQuery = ""
            For Each rNMCT_ARTI As DataRow In dtNMCT_ARTI.Rows
                If Not IsDBNull(rNMCT_ARTI("Composant")) Then sQuery = sQuery & SAP_DATA_CREA_FILT_TABL("(MATNR EQ '" & rNMCT_ARTI("Composant").ToString & "' and SPRAS EQ 'F')", " Or ")
            Next
            sQuery = Right(sQuery, Len(sQuery) - 5)
            dtMAKT = SAP_DATA_READ_TBL("MAKT", "|", "", "MATNR MAKTX", sQuery, 1)

            sQuery = ""
            For Each rNMCT_ARTI As DataRow In dtNMCT_ARTI.Rows
                If Not IsDBNull(rNMCT_ARTI("Composant")) Then sQuery = sQuery & SAP_DATA_CREA_FILT_TABL("(MATNR EQ '" & rNMCT_ARTI("Composant").ToString & "' and WERKS EQ 'DI31')", " Or ")
            Next
            sQuery = Right(sQuery, Len(sQuery) - 5)
            dtMARC = SAP_DATA_READ_TBL("MARC", "|", "", "MATNR BESKZ", sQuery, 1)  ';Extraction Fabircation Interne ou externe

            For Each rNMCT_ARTI As DataRow In dtNMCT_ARTI.Rows
                If Not IsDBNull(rNMCT_ARTI("Composant")) Then
                    Dim rdtMAKT As DataRow = dtMAKT.Select("MATNR = '" & rNMCT_ARTI("Composant").ToString & "'").FirstOrDefault()
                    If Not rdtMAKT Is Nothing Then rNMCT_ARTI("Désignation") = rdtMAKT("MAKTX").ToString
                    Dim rdtMARC As DataRow = dtMARC.Select("MATNR = '" & rNMCT_ARTI("Composant").ToString & "'").FirstOrDefault()
                    If Not rdtMARC Is Nothing Then rNMCT_ARTI("Fab. Interne/Externe") = rdtMARC("BESKZ").ToString
                Else
                    rNMCT_ARTI("Composant") = ""
                End If
            Next

            With dtNMCT_ARTI_2.Columns
                .Add("N° Nomenclature", Type.GetType("System.String"))
                .Add("N° Noeud", Type.GetType("System.String"))
                .Add("Compteur Poste", Type.GetType("System.String"))
                .Add("Fab. Interne/Externe", Type.GetType("System.String"))
                .Add("Compteur Sous-Poste", Type.GetType("System.String"))
                .Add("Repère", Type.GetType("System.String"))
                .Add("Composant", Type.GetType("System.String"))
                .Add("Désignation", Type.GetType("System.String"))
                .Add("Qté", Type.GetType("System.String"))
            End With
            'Dim qNMCT_ARTI_GROUP = From rNMCT_ARTI In dtNMCT_ARTI
            '                       Group rNMCT_ARTI By N_NMCT = rNMCT_ARTI.Field(Of String)("N° Nomenclature") Into N_NMCT_GROUP = Group
            '                       Select New With {Key N_NMCT}

            'sQuery = ""
            'For Each rNMCT_ARTI In qNMCT_ARTI_GROUP
            '    If Not IsDBNull(rNMCT_ARTI.N_NMCT) Then sQuery = sQuery & SAP_DATA_CREA_FILT_TABL("(STLNR EQ '" & rNMCT_ARTI.N_NMCT.ToString & "')", " Or ")
            'Next
            'sQuery = Right(sQuery, Len(sQuery) - 5)
            'LOG_Msg(GetCurrentMethod, sQuery)
            'dtSTPU = SAP_DATA_READ_TBL("STPU", "|", "", "STLNR STLKN STPOZ UPOSZ EBORT UPMNG", sQuery, 1)


            For Each rNMCT_ARTI As DataRow In dtNMCT_ARTI.Rows
                dtSTPU = SAP_DATA_READ_TBL("STPU", "|", "", "STLNR STLKN STPOZ UPOSZ EBORT UPMNG", "STLNR EQ '" & rNMCT_ARTI("N° Nomenclature").ToString & "' and STLKN EQ '" & rNMCT_ARTI("N° Noeud").ToString & "' and STPOZ EQ '" & rNMCT_ARTI("Compteur").ToString & "'", 1)
                If Not dtSTPU Is Nothing Then
                    For Each rSTPU As DataRow In dtSTPU.Rows
                        'LOG_Msg(GetCurrentMethod, "STLNR = " & rSTPU("STLNR").ToString)
                        dtNMCT_ARTI_2.Rows.Add()
                        dtNMCT_ARTI_2(dtNMCT_ARTI_2.Rows.Count - 1)("N° Nomenclature") = rSTPU("STLNR").ToString
                        dtNMCT_ARTI_2(dtNMCT_ARTI_2.Rows.Count - 1)("N° Noeud") = rSTPU("STLKN").ToString
                        dtNMCT_ARTI_2(dtNMCT_ARTI_2.Rows.Count - 1)("Compteur Poste") = rSTPU("STPOZ").ToString
                        dtNMCT_ARTI_2(dtNMCT_ARTI_2.Rows.Count - 1)("Fab. Interne/Externe") = ""
                        dtNMCT_ARTI_2(dtNMCT_ARTI_2.Rows.Count - 1)("Compteur Sous-Poste") = rSTPU("UPOSZ").ToString
                        dtNMCT_ARTI_2(dtNMCT_ARTI_2.Rows.Count - 1)("Repère") = rSTPU("EBORT").ToString
                        dtNMCT_ARTI_2(dtNMCT_ARTI_2.Rows.Count - 1)("Qté") = rSTPU("UPMNG").ToString
                    Next
                End If
            Next

            For Each rNMCT_ARTI_2 As DataRow In dtNMCT_ARTI_2.Rows
                Dim rNMCT_ARTI As DataRow = dtNMCT_ARTI.Select("[N° Nomenclature] = '" & rNMCT_ARTI_2("N° Nomenclature").ToString & "' AND [N° Noeud] = '" & rNMCT_ARTI_2("N° Noeud") & "' AND [Compteur] = '" & rNMCT_ARTI_2("Compteur Poste") & "'").FirstOrDefault()
                If Not rNMCT_ARTI Is Nothing Then
                    'For Each rNMCT_ARTI As DataRow In dtNMCT_ARTI.Rows
                    'If rNMCT_ARTI_2("N° Nomenclature") = rNMCT_ARTI("N° Nomenclature") And rNMCT_ARTI_2("N° Noeud") = rNMCT_ARTI("N° Noeud") And rNMCT_ARTI_2("Compteur Poste") = rNMCT_ARTI("Compteur") Then
                    rNMCT_ARTI_2("Composant") = rNMCT_ARTI("Composant").ToString
                    rNMCT_ARTI_2("Désignation") = rNMCT_ARTI("Désignation").ToString
                    'Exit For
                End If
                ' Next
            Next

            With dtNMCT_ARTI_3.Columns
                .Add("N° Nomenclature", Type.GetType("System.String"))
                .Add("N° Noeud", Type.GetType("System.String"))
                .Add("Compteur Poste", Type.GetType("System.String"))
                .Add("Fab. Interne/Externe", Type.GetType("System.String"))
                .Add("Compteur Sous-Poste", Type.GetType("System.String"))
                .Add("Repère", Type.GetType("System.String"))
                .Add("Composant", Type.GetType("System.String"))
                .Add("Désignation", Type.GetType("System.String"))
                .Add("Qté", Type.GetType("System.String"))
            End With

            For Each rNMCT_ARTI As DataRow In dtNMCT_ARTI.Rows
                dtNMCT_ARTI_3.Rows.Add()
                dtNMCT_ARTI_3(dtNMCT_ARTI_3.Rows.Count - 1)("Fab. Interne/Externe") = rNMCT_ARTI("Fab. Interne/Externe").ToString                                '	;E: fabrictation interne - F: fabrictation externe
                dtNMCT_ARTI_3(dtNMCT_ARTI_3.Rows.Count - 1)("Composant") = rNMCT_ARTI("Composant").ToString
                dtNMCT_ARTI_3(dtNMCT_ARTI_3.Rows.Count - 1)("Désignation") = rNMCT_ARTI("Désignation").ToString
                dtNMCT_ARTI_3(dtNMCT_ARTI_3.Rows.Count - 1)("Qté") = Convert.ToDecimal(Replace(rNMCT_ARTI("Qté").ToString, ".", ","))

                For Each rNMCT_ARTI_2 As DataRow In dtNMCT_ARTI_2.Rows
                    If rNMCT_ARTI_2("Composant") = rNMCT_ARTI("Composant") Then
                        dtNMCT_ARTI_3.Rows.Add()
                        dtNMCT_ARTI_3(dtNMCT_ARTI_3.Rows.Count - 1)("N° Nomenclature") = rNMCT_ARTI_2("N° Nomenclature").ToString
                        dtNMCT_ARTI_3(dtNMCT_ARTI_3.Rows.Count - 1)("N° Noeud") = rNMCT_ARTI_2("N° Noeud").ToString
                        dtNMCT_ARTI_3(dtNMCT_ARTI_3.Rows.Count - 1)("Compteur Poste") = rNMCT_ARTI_2("Compteur Poste").ToString
                        dtNMCT_ARTI_3(dtNMCT_ARTI_3.Rows.Count - 1)("Fab. Interne/Externe") = rNMCT_ARTI_2("Fab. Interne/Externe").ToString
                        dtNMCT_ARTI_3(dtNMCT_ARTI_3.Rows.Count - 1)("Compteur Sous-Poste") = rNMCT_ARTI_2("Compteur Sous-Poste").ToString
                        dtNMCT_ARTI_3(dtNMCT_ARTI_3.Rows.Count - 1)("Repère") = rNMCT_ARTI_2("Repère").ToString
                        dtNMCT_ARTI_3(dtNMCT_ARTI_3.Rows.Count - 1)("Composant") = rNMCT_ARTI_2("Composant").ToString
                        dtNMCT_ARTI_3(dtNMCT_ARTI_3.Rows.Count - 1)("Désignation") = rNMCT_ARTI_2("Désignation").ToString
                        dtNMCT_ARTI_3(dtNMCT_ARTI_3.Rows.Count - 1)("Qté") = Convert.ToDecimal(Replace(rNMCT_ARTI_2("Qté").ToString, ".", ","))
                    End If
                Next
            Next
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try

        LOG_Msg(GetCurrentMethod, "La nomenclature de l'article " & sArticle & " a été extraite de SAP.")
        Return dtNMCT_ARTI_3

    End Function

    Public Shared Function SAP_DATA_LIST_ARTI_CLIE(sClient As String, sType_produit As String) As DataTable

        Dim dtT179T, dtMARA As New DataTable

        Try
            dtT179T = SAP_DATA_READ_T179T("VTEXT EQ '" & sClient & "'")
            If dtT179T Is Nothing Then Throw New Exception("Le client " & sClient & " n'est pas présent dans la table dtT179T")
            dtMARA = SAP_DATA_READ_MARA("PRDHA EQ '" & dtT179T(0)("PRODH") & "' and (MATKL EQ '" & sType_produit & "')")
            If dtMARA Is Nothing Then Throw New Exception("Pas d'article pour le client " & sClient)
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try

        LOG_Msg(GetCurrentMethod, "La liste d'article du client " & sClient & " a été extraite de SAP.")
        Return dtMARA

        'SEMI-FINI
        'PRODUIT 
    End Function

    Public Shared Function SAP_DATA_CONN(Optional User As String = "RFCUSER",
                                         Optional Password As String = "saprfc!",
                                         Optional Codeclient As String = "600",
                                         Optional Serveur As String = "SAP_PEO",
                                         Optional SystemNumber As String = "00") As Object

        Dim oSAP As Object = CreateObject("SAP.Functions")
        Dim oConn As Object = oSAP.Connection

        Try
            With oConn
                .AutoLogon = True
                .Client = Codeclient
                .User = User
                .Password = Password
                .ApplicationServer = Serveur
                .SystemNumber = SystemNumber
            End With
            If oConn.logon(0, True) <> True Then Throw New Exception("ERREUR - connexion à SAP a échoué")
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            oSAP = SAP_DATA_DECO(oSAP)
            Return 0
        End Try

        'LOG_Msg(GetCurrentMethod, "Connexion réussie")
        Return oSAP

    End Function

    Public Shared Function SAP_DATA_DECO(ByRef oFunctionSAP As Object) As Object

        Try
            oFunctionSAP.Connection.logoff()
            oFunctionSAP = 0
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return 0
        End Try

        'LOG_Msg(GetCurrentMethod(), "déconnexion réussie")
        Return oFunctionSAP
    End Function

    Public Shared Function SAP_DATA_READ_TBL(Table As String,
                                             Optional Delimiter As String = "|",
                                             Optional NoData As String = "",
                                             Optional Field As String = "",
                                             Optional option_req As String = "",
                                             Optional fieldinarray As Integer = 0,
                                             Optional ligneskip As Integer = 0,
                                             Optional nbrlignes As Integer = 0) As DataTable

        Dim dt As New DataTable
        Dim oSAP, RFC, olinedata, olinefield As New Object
        Dim FIELDS As Object
        Dim array_champ() As String
        Dim option_table As Object
        Dim asOPTION_REQ As String()

        Try
            oSAP = SAP_DATA_CONN()
            RFC = oSAP.Add("RFC_READ_TABLE")
            RFC.exports("DELIMITER").value = Delimiter
            RFC.exports("NO_DATA").value = NoData
            RFC.exports("ROWSKIPS").value = ligneskip
            RFC.exports("ROWCOUNT").value = nbrlignes
            FIELDS = RFC.Tables("FIELDS")
            If Field = "" Then Throw New Exception("Pas de champs déclarés dans les options")
            array_champ = Split(Field, " ")
            For iArray As Integer = 0 To UBound(array_champ)
                FIELDS.AppendRow()
                FIELDS(iArray + 1, "FIELDNAME") = array_champ(iArray)
            Next
            option_table = RFC.Tables("OPTIONS")
            option_table.Appendrow()
            option_table(1, "TEXT") = option_req
            If option_req.IndexOf("|") > -1 Then
                asOPTION_REQ = Split(option_req, "|")
                For i = 0 To UBound(asOPTION_REQ) - 1
                    option_table.Appendrow()
                    option_table(i + 1, "TEXT") = asOPTION_REQ(i)
                Next
            Else
                option_table.Appendrow()
                option_table(1, "TEXT") = option_req
            End If
            RFC.Exports("QUERY_TABLE").Value = Table
            If RFC.Call <> -1 Then Throw New Exception(RFC.exception)

            olinedata = RFC.tables.Item("DATA")
            If olinedata.rowcount = 0 Then Throw New Exception("0 ligne trouvée.")
            olinefield = RFC.tables.Item("FIELDS")
            If olinefield.rowcount = 0 Then Throw New Exception("0 champ trouvé.")

            For z As Integer = 1 To olinefield.rowcount
                dt.Columns.Add(olinefield.value(z, 1), Type.GetType("System.String"))
            Next

            Dim ligne() As String
            For i As Integer = 1 To olinedata.rowcount
                dt.Rows.Add()
                ligne = Split(olinedata.value(i, 1), Delimiter)
                For p As Integer = 0 To UBound(ligne)
                    dt.Rows(dt.Rows.Count - 1)(olinefield.value(p + 1, 1)) = ligne(p)
                Next
            Next
            If dt Is Nothing Then Throw New Exception("Aucune donnée dans la table " & Table)
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        Finally
            oSAP = SAP_DATA_DECO(oSAP)
        End Try

        'LOG_Msg(GetCurrentMethod, "Lecture de la table " & Table & " réussie. " & olinedata.rowcount & " lignes trouvées.")
        Return dt

    End Function

    Public Shared Function SAP_DATA_CREA_FILT_TABL(sfiltre As String, sOPERATEUR As String) As String

        sfiltre = "|" & sOPERATEUR & sfiltre
        Return sfiltre

    End Function

    Public Shared Function SAP_DATA_Z_LOG_CONF_GET(V_AUFNR As String, V_VORNR As String, V_PERNR As String, DATE_DEBUT As String, DATE_FIN As String) As DataTable

        Dim dt_T_LOG_CONF As New DataTable
        Dim oSAP, RFC, oRFC_RET, oRFC_MSG, oT_LOG_CONF As New Object

        Try
            oSAP = SAP_DATA_CONN()
            RFC = oSAP.Add("Z_LOG_CONF_GET")
            If V_AUFNR <> "" Then V_AUFNR = StrDup(12 - Len(V_AUFNR), "0") & V_AUFNR
            RFC.exports("V_AUFNR") = V_AUFNR
            If V_VORNR <> "" Then V_VORNR = StrDup(4 - Len(V_VORNR), "0") & V_VORNR
            RFC.exports("V_VORNR") = V_VORNR
            If V_PERNR <> "" Then V_PERNR = StrDup(8 - Len(V_PERNR), "0") & V_PERNR
            RFC.exports("V_PERNR") = V_PERNR
            RFC.exports("DATE_DEBUT") = COMM_APP_WEB_CONV_FORM_DATE(DATE_DEBUT, "yyyyMMdd")
            RFC.exports("DATE_FIN") = COMM_APP_WEB_CONV_FORM_DATE(DATE_FIN, "yyyyMMdd")

            oT_LOG_CONF = RFC.Tables("T_LOG_CONF") 'ZLOG_CONF

            If RFC.Call <> -1 Then Throw New Exception(RFC.exception)

            For Each o_COL_T_LOG_CONF As Object In oT_LOG_CONF.Columns
                dt_T_LOG_CONF.Columns.Add(o_COL_T_LOG_CONF.Name, Type.GetType("System.String"))
            Next
            For iRowIndex = 1 To oT_LOG_CONF.RowCount
                dt_T_LOG_CONF.Rows.Add()
                For iIndex = 1 To oT_LOG_CONF.ColumnCount - 1
                    dt_T_LOG_CONF.Rows(dt_T_LOG_CONF.Rows.Count - 1)(iIndex - 1) = oT_LOG_CONF.Value(iRowIndex, iIndex)
                Next
            Next
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        Finally
            oSAP = SAP_DATA_DECO(oSAP)
        End Try

        LOG_Msg(GetCurrentMethod, "Exécution de la fonction Z_LOG_CONF_GET réussie. " & oT_LOG_CONF.RowCount & " lignes trouvées.")
        Return dt_T_LOG_CONF

    End Function

    Public Shared Function SAP_DATA_Z_LOG_ACT_GET(V_PERNR As String, DATE_DEBUT As String, DATE_FIN As String) As DataTable

        Dim dt_T_LOG_ACT As New DataTable
        Dim oSAP, RFC, oRFC_RET, oRFC_MSG, oT_LOG_ACT As New Object

        Try
            oSAP = SAP_DATA_CONN()
            RFC = oSAP.Add("Z_LOG_ACT_GET")
            If V_PERNR <> "" Then V_PERNR = StrDup(8 - Len(V_PERNR), "0") & V_PERNR
            RFC.exports("V_PERNR") = V_PERNR
            RFC.exports("DATE_DEBUT") = COMM_APP_WEB_CONV_FORM_DATE(DATE_DEBUT, "yyyyMMdd")
            RFC.exports("DATE_FIN") = COMM_APP_WEB_CONV_FORM_DATE(DATE_FIN, "yyyyMMdd")

            oT_LOG_ACT = RFC.Tables("T_LOG_ACT")

            If RFC.Call <> -1 Then Throw New Exception(RFC.exception)

            For Each o_COL_T_LOG_ACT As Object In oT_LOG_ACT.Columns
                dt_T_LOG_ACT.Columns.Add(o_COL_T_LOG_ACT.Name, Type.GetType("System.String"))
            Next
            For iRowIndex = 1 To oT_LOG_ACT.RowCount
                dt_T_LOG_ACT.Rows.Add()
                For iIndex = 1 To oT_LOG_ACT.ColumnCount - 1
                    dt_T_LOG_ACT.Rows(dt_T_LOG_ACT.Rows.Count - 1)(iIndex - 1) = oT_LOG_ACT.Value(iRowIndex, iIndex)
                Next
            Next
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        Finally
            oSAP = SAP_DATA_DECO(oSAP)
        End Try

        LOG_Msg(GetCurrentMethod, "Exécution de la fonction Z_LOG_ACT_GET réussie. " & dt_T_LOG_ACT.Rows.Count & " lignes trouvées.")
        Return dt_T_LOG_ACT

    End Function

    Public Shared Function SAP_DATA_READ_MAKT(Optional sFILT As String = "") As DataTable
        Dim dtMAKT As New DataTable

        Try
            dtMAKT = SAP_DATA_READ_TBL("MAKT", "|", "", "MANDT MATNR SPRAS MAKTX MAKTG", sFILT)
            If dtMAKT Is Nothing Then Throw New Exception("Problème de lecture de la table MAKT avec le filtre : " & sFILT)
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try

        LOG_Msg(GetCurrentMethod, "Lecture de la table MAKT effectuée avec le filtre : " & sFILT)
        Return dtMAKT
    End Function

    Public Shared Function SAP_DATA_READ_AFKO(Optional sFILT As String = "") As DataTable
        Dim dtAFKO As New DataTable

        Try
            dtAFKO = SAP_DATA_READ_TBL("AFKO", "|", "", "RSNUM GAMNG PLNBEZ AUFNR AUFPL REVLV STLNR STLAL GSTRS", sFILT)
            If dtAFKO Is Nothing Then Throw New Exception("Problème de lecture de la table AFKO avec le filtre : " & sFILT)
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try

        LOG_Msg(GetCurrentMethod, "Lecture de la table AFKO effectuée avec le filtre : " & sFILT)
        Return dtAFKO
    End Function

    Public Shared Function SAP_DATA_READ_AFPO(Optional sFILT As String = "") As DataTable

        Dim dtAFPO As New DataTable

        Try
            dtAFPO = SAP_DATA_READ_TBL("AFPO", "|", "", "AUFNR MATNR MATNR MATNR PSMNG WEMNG PWERK DAUAT DNREL DGLTP ELIKZ LTRMI", sFILT)
            If dtAFPO Is Nothing Then Throw New Exception("Problème de lecture de la table AFPO avec le filtre : " & sFILT)
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try

        LOG_Msg(GetCurrentMethod, "Lecture de la table AFPO effectuée avec le filtre : " & sFILT)
        Return dtAFPO
    End Function

    Public Shared Function SAP_DATA_READ_VRESB(Optional sFILT As String = "") As DataTable

        Dim dtVRESB As New DataTable

        Try
            dtVRESB = SAP_DATA_READ_TBL("VRESB", "|", "", "STLNR STLKN STPOZ MTART RSPOS MATNR BDMNG MAKTX MATKL POTX1 VORNR XLOEK", sFILT)
            If dtVRESB Is Nothing Then Throw New Exception("Problème de lecture de la table VRESB avec le filtre : " & sFILT)
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try

        LOG_Msg(GetCurrentMethod, "Lecture de la table VRESB effectuée avec le filtre : " & sFILT)
        Return dtVRESB
    End Function

    Public Shared Function SAP_DATA_READ_T179T(Optional sFILT As String = "") As DataTable

        Dim dtT179T As New DataTable

        Try
            dtT179T = SAP_DATA_READ_TBL("T179T", "|", "", "PRODH VTEXT", sFILT)
            If dtT179T Is Nothing Then Throw New Exception("Problème de lecture de la table T179T avec le filtre : " & sFILT)
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try

        LOG_Msg(GetCurrentMethod, "Lecture de la table T179T effectuée avec le filtre : " & sFILT)
        Return dtT179T
    End Function

    Public Shared Function SAP_DATA_READ_MARA(Optional sFILT As String = "") As DataTable

        Dim dtMARA As New DataTable

        Try
            dtMARA = SAP_DATA_READ_TBL("MARA", "|", "", "MATNR PRDHA MATKL MTART", sFILT)
            If dtMARA Is Nothing Then Throw New Exception("Problème de lecture de la table MARA avec le filtre : " & sFILT)
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try

        LOG_Msg(GetCurrentMethod, "Lecture de la table MARA effectuée avec le filtre : " & sFILT)
        Return dtMARA
    End Function

    Public Shared Function SAP_DATA_READ_AFVC(Optional sFILT As String = "") As DataTable

        Dim dtAFVC As New DataTable

        Try
            dtAFVC = SAP_DATA_READ_TBL("AFVC", "|", "", "AUFPL PLNNR ZAEHL VORNR LTXA1 STEUS ARBID BEDZL PLNFL", sFILT)
            If dtAFVC Is Nothing Then Throw New Exception("Problème de lecture de la table AFVC avec le filtre : " & sFILT)
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try

        LOG_Msg(GetCurrentMethod, "Lecture de la table AFVC effectuée avec le filtre : " & sFILT)
        Return dtAFVC
    End Function

    Public Shared Function SAP_DATA_READ_STPU(Optional sFILT As String = "") As DataTable

        Dim dtSTPU As New DataTable

        Try
            dtSTPU = SAP_DATA_READ_TBL("STPU", "|", "", "STLNR STLKN STPOZ UPOSZ EBORT UPMNG", sFILT)
            If dtSTPU Is Nothing Then Throw New Exception("Problème de lecture de la table STPU avec le filtre : " & sFILT)
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try

        LOG_Msg(GetCurrentMethod, "Lecture de la table STPU effectuée avec le filtre : " & sFILT)
        Return dtSTPU
    End Function

    Public Shared Function SAP_DATA_READ_PA0002(Optional sFILT As String = "") As DataTable

        Dim dtPA0002 As New DataTable

        Try
            dtPA0002 = SAP_DATA_READ_TBL("PA0002", "|", "", "PERNR NACHN VORNA", sFILT)
            If dtPA0002 Is Nothing Then Throw New Exception("Problème de lecture de la table PA0002 avec le filtre : " & sFILT)
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try

        LOG_Msg(GetCurrentMethod, "Lecture de la table PA0002 effectuée avec le filtre : " & sFILT)
        Return dtPA0002
    End Function

    Public Shared Function SAP_DATA_READ_LIFNR(Optional sFILT As String = "") As DataTable

        Dim dtLIFNR As New DataTable

        Try
            dtLIFNR = SAP_DATA_READ_TBL("LIFNR", "|", "", "LIFNR NAME1 LAND1 STRAS PSTLZ ORT01 NAME2 TELF1 TELF2 TELFX", sFILT)
            If dtLIFNR Is Nothing Then Throw New Exception("Problème de lecture de la table LIFNR avec le filtre : " & sFILT)
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try

        LOG_Msg(GetCurrentMethod, "Lecture de la table LIFNR effectuée avec le filtre : " & sFILT)
        Return dtLIFNR
    End Function

    Public Shared Function SAP_DATA_GET_CLIE_FROM_CD_ARTI(sCodeArticle As String) As String

        Dim dtMARA, dtT179T As New DataTable

        Try
            dtMARA = SAP_DATA_READ_MARA("MATNR EQ '" & sCodeArticle & "'")
            If dtMARA Is Nothing Then Throw New Exception("Le code article " & sCodeArticle & " n'est pas connu dans SAP.")
            dtT179T = SAP_DATA_READ_T179T("PRODH EQ '" & dtMARA(0)("PRDHA").ToString & "'")
            If dtMARA Is Nothing Then Throw New Exception("Le code article " & sCodeArticle & " n'est pas connu dans SAP.")
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try
        LOG_Msg(GetCurrentMethod, "Le client pour le code article " & sCodeArticle & " est " & dtT179T(0)("VTEXT").ToString())
        Return dtT179T(0)("VTEXT").ToString()

    End Function

    Public Shared Function SAP_DATA_READ_S034(Optional sFILT As String = "") As DataTable

        Dim dtS034 As New DataTable

        Try
            dtS034 = SAP_DATA_READ_TBL("S034", "|", "", "MANDT SSOUR VRSIO SPMON SPTAG SPWOC SPBUP WERKS LGORT MATNR CHARG PERIV VWDAT BASME HWAER CMZUBB CWZUBB CAZUBB CMZUKB CAZUKB CMAGBB", sFILT)
            If dtS034 Is Nothing Then Throw New Exception("Problème de lecture de la table S034 avec le filtre : " & sFILT)
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try

        LOG_Msg(GetCurrentMethod, "Lecture de la table S034 effectuée avec le filtre : " & sFILT)
        Return dtS034
    End Function

    Public Shared Function SAP_DATA_READ_LIPS(Optional sFILT As String = "") As DataTable

        Dim dtLIPS As New DataTable

        Try
            dtLIPS = SAP_DATA_READ_TBL("LIPS", "|", "", "VBELN POSNR VGBEL VGPOS", sFILT)
            If dtLIPS Is Nothing Then Throw New Exception("Problème de lecture de la table LIPS avec le filtre : " & sFILT)
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try

        LOG_Msg(GetCurrentMethod, "Lecture de la table LIPS effectuée avec le filtre : " & sFILT)
        Return dtLIPS
    End Function
    Public Shared Function SAP_DATA_READ_LIPSUP(Optional sFILT As String = "") As DataTable

        Dim dtLIPSUP As New DataTable

        Try
            dtLIPSUP = SAP_DATA_READ_TBL("LIPSUP", "|", "", "VBELN POSNR MATNR ARKTX", sFILT)
            If dtLIPSUP Is Nothing Then Throw New Exception("Problème de lecture de la table LIPSUP avec le filtre : " & sFILT)
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try

        LOG_Msg(GetCurrentMethod, "Lecture de la table LIPSUP effectuée avec le filtre : " & sFILT)
        Return dtLIPSUP
    End Function

    Public Shared Function SAP_DATA_READ_CRHD(Optional sFILT As String = "") As DataTable

        Dim dtCRHD As New DataTable

        Try
            dtCRHD = SAP_DATA_READ_TBL("CRHD", "|", "", "ARBPL OBJID", sFILT)
            If dtCRHD Is Nothing Then Throw New Exception("Problème de lecture de la table CRHD avec le filtre : " & sFILT)
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try

        LOG_Msg(GetCurrentMethod, "Lecture de la table LIPSUP effectuée avec le filtre : " & sFILT)
        Return dtCRHD
    End Function

    Public Shared Function SAP_DATA_LECT_OF(sOF As String) As DataTable
        Dim dtAFKO, dtMARA, dtVRESB, dtT179T, dtMAKT, dt As New DataTable

        Try
            With dt.Columns
                .Add("NU_OF", Type.GetType("System.String"))
                .Add("CD_ARTI_ECO", Type.GetType("System.String"))
                .Add("NM_CLIE", Type.GetType("System.String"))
                .Add("NM_DSGT_ARTI", Type.GetType("System.String"))
                .Add("QT_OF", Type.GetType("System.String"))
            End With

            dtAFKO = SAP_DATA_READ_AFKO("AUFNR LIKE '%" & sOF & "'")
            If dtAFKO Is Nothing Then Throw New Exception("L'OF n°" & sOF & " n'a pas été trouvé dans SAP.")
            dtMARA = SAP_DATA_READ_MARA("MATNR EQ '" & dtAFKO(0)("PLNBEZ").ToString & "'")
            If dtMARA Is Nothing Then Throw New Exception("Larticle de l'OF " & sOF & " n'a pas été trouvé dans SAP.")
            dtT179T = SAP_DATA_READ_T179T("PRODH EQ '" & dtMARA(0)("PRDHA").ToString & "'")
            If dtT179T Is Nothing Then Throw New Exception("Le client de l'OF " & sOF & " n'a pas été trouvé dans SAP.")
            dtMAKT = SAP_DATA_READ_MAKT("MATNR EQ '" & dtAFKO(0)("PLNBEZ").ToString & "'")
            If dtMAKT Is Nothing Then Throw New Exception("La désignation d'aticle de l'OF " & sOF & " n'a pas été trouvé dans SAP.")

            dt.Rows.Add()
            dt.Rows(dt.Rows.Count - 1)("NU_OF") = sOF
            dt.Rows(dt.Rows.Count - 1)("CD_ARTI_ECO") = dtAFKO(0)("PLNBEZ").ToString
            dt.Rows(dt.Rows.Count - 1)("NM_CLIE") = Trim(dtT179T(0)("VTEXT").ToString)
            dt.Rows(dt.Rows.Count - 1)("NM_DSGT_ARTI") = Trim(dtMAKT(0)("MAKTX").ToString)
            dt.Rows(dt.Rows.Count - 1)("QT_OF") = Trim(dtAFKO(0)("GAMNG").ToString)
            Return dt
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try

    End Function
    Public Shared Function SAP_DATA_READ_MSEG(Optional sFILT As String = "") As DataTable

        Dim dtMSEG As New DataTable
        Try
            dtMSEG = SAP_DATA_READ_TBL("MSEG", "|", "", "MBLNR MJAHR ZEILE MATNR CHARG MENGE ELIKZ", sFILT)
            If dtMSEG Is Nothing Then Throw New Exception("Problème de lecture de la table MSEG avec le filtre : " & sFILT)
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try

        LOG_Msg(GetCurrentMethod, "Lecture de la table MSEG effectuée avec le filtre : " & sFILT)
        Return dtMSEG
    End Function

    Public Shared Function SAP_DATA_READ_STPO(Optional sFILT As String = "") As DataTable

        Dim dtSTPO As New DataTable

        Try
            dtSTPO = SAP_DATA_READ_TBL("STPO", "|", "", "STLNR STLKN STPOZ IDNRK MENGE", sFILT)
            If dtSTPO Is Nothing Then Throw New Exception("Problème de lecture de la table STPO avec le filtre : " & sFILT)
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try

        LOG_Msg(GetCurrentMethod, "Lecture de la table STPO effectuée avec le filtre : " & sFILT)
        Return dtSTPO
    End Function
End Class
