Imports iTextSharp.text.pdf
Imports iTextSharp.text
Imports iTextSharp
Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports System.IO
Imports System
Imports System.Data
Imports App_Web.Class_SQL
Imports App_Web.Class_DIG_FACT
Imports App_Web.Class_DIG_FACT_SQL
Imports App_Web.Class_SAP_DATA
Imports App_Web.Class_COMM_APP_WEB


Public Class Class_DOC_ITEXT_SHARP
    Public Shared Function DOC_ITEXT_SHARP_CREA_FCGF(sNU_SER As String, sOF As String, scd_arti As String) As String
        'Dim dt, dt_PHAS, dt_SEQU, dtKNMT, dt_RESU, dtZMOYENTYPETEST As New DataTable
        Dim sQuery As String = ""
        Dim sb_list_inci As New StringBuilder
        sb_list_inci.Append("Liste des numéros d'incident : ")
        'Dim dPDF As Document
        Dim pt As PdfPTable
        Dim c_ENTE As PdfPCell
        Randomize()
        Dim sfich As String = $"c:\sources\temp_App_Web\{CInt(Int((10000000 * Rnd()) + 1))}.pdf”
        ' Dim iLOGO As Image
        Dim para As Paragraph
        ' Dim ipagenumber As Integer
        Try
            Using dPDF = New Document(PageSize.A4, 20, 20, 30, 20)
                'Session("sNU_SER") = sNU_SER
                sQuery = $"SELECT [NM_RFRC_FDV],[NU_VERS_FDV]
                             FROM [dbo].[V_FCGF]
                            WHERE [NU_SER_SPFE] = '{sNU_SER}'
                           GROUP BY [NM_RFRC_FDV],[NU_VERS_FDV]"
                Using dt_fdv = SQL_SELE_TO_DT(sQuery, CS_ALMS_PROD_PRD)
                    'Session("sFDV") = dt(0)("NM_RFRC_FDV").ToString
                    'Session("sVers_FDV") = dt(0)("NU_VERS_FDV").ToString
                    Using dtKNMT = SAP_DATA_READ_KNMT($"MATNR LIKE '{scd_arti}%' AND KUNNR EQ '0000000451' AND VKORG EQ 'ORC3' AND VTWEG EQ 'CD'")
                        If dtKNMT Is Nothing Then Throw New Exception("Code article client introuvable sous SAP")
                        sQuery = $"SELECT [NM_DSGT_ARTI]
                                     FROM [dbo].[DTM_REF_GAMM_ARTI]
                                    WHERE [CD_ARTI_PROD] = '{Trim(dtKNMT(0)("KDMAT").ToString)}'"
                        Using dt_nm_dsgt_arti = SQL_SELE_TO_DT(sQuery, CS_ALMS_PROD_PRD)
                            'Session("sDSGT_ARTI") = dt(0)("NM_DSGT_ARTI").ToString
                            dPDF.Open()
                            Dim writer = PdfAWriter.GetInstance(dPDF, New FileStream(sfich, FileMode.Create))
                            Dim PageEventHandler As New HeaderFooter_FCGF(sNU_SER, dt_fdv(0)("NM_RFRC_FDV").ToString, dt_fdv(0)("NU_VERS_FDV").ToString, dt_nm_dsgt_arti(0)("NM_DSGT_ARTI").ToString)
                            writer.PageEvent = PageEventHandler
                        End Using

                        'numéro de série
                        pt = New PdfPTable(2)
                        pt.TotalWidth = dPDF.PageSize.Width - 80
                        pt.LockedWidth = True
                        para = New Paragraph(New Chunk($“SN Produit : {sNU_SER}", FontFactory.GetFont("ARIAL", 16, Font.BOLD)))
                        c_ENTE = New PdfPCell(para)
                        c_ENTE.Border = 0
                        pt.AddCell(c_ENTE)
                        sQuery = $"SELECT [NM_STAT_PROD]
                                     FROM [dbo].[V_FCGF]
                                    WHERE [NU_SER_SPFE] = '{sNU_SER}'
                                  GROUP BY [NU_SER_SPFE],[NM_STAT_PROD]
                                  ORDER BY [NM_STAT_PROD] DESC"
                        Using dt_RESU = SQL_SELE_TO_DT(sQuery, CS_ALMS_PROD_PRD)
                            para = New Paragraph(New Chunk($"Statut global : {dt_RESU(0)("NM_STAT_PROD").ToString}", FontFactory.GetFont("ARIAL", 14, Font.BOLD)))
                            c_ENTE = New PdfPCell(para)
                            c_ENTE.Border = 0
                            pt.AddCell(c_ENTE)
                            pt.SpacingAfter = 10
                            dPDF.Add(pt)

                            'code article
                            para = New Paragraph(New Chunk($“Code article :  {Trim(dtKNMT(0)("KDMAT").ToString)}", FontFactory.GetFont("ARIAL", 12, Font.BOLD)))
                            para.SpacingAfter = 10
                            dPDF.Add(para)

                            'of
                            para = New Paragraph(New Chunk($“N° OF : {sOF}", FontFactory.GetFont("ARIAL", 12, Font.BOLD)))
                            para.SpacingAfter = 10
                            dPDF.Add(para)

                            'liste des séquences
                            sQuery = $"SELECT [dbo].[DTM_SEQU_RFRC_LIST].[NM_RFRC_SEQU], [dbo].[DTM_SEQU_RFRC_LIST].[NU_VERS_SEQU], [dbo].[DTM_SEQU_RFRC_LIST].ID_RFRC_SEQU, [dbo].[DTM_SEQU_RFRC_LIST].DT_APCT
                                         FROM (
                                               SELECT TOP 100 PERCENT [NM_RFRC_SEQU],MAX([NU_VERS_SEQU]) AS [NU_VERS_SEQU]
                                                 FROM [dbo].[V_FCGF] 
                                                WHERE [NU_SER_SPFE] = '{sNU_SER}'
                                               GROUP BY [NM_RFRC_SEQU],[NU_OP] 
                                               ORDER BY [NU_OP]
                                              ) AS A INNER JOIN [dbo].[DTM_SEQU_RFRC_LIST] ON A.[NM_RFRC_SEQU] = [dbo].[DTM_SEQU_RFRC_LIST].[NM_RFRC_SEQU] AND A.[NU_VERS_SEQU] = [dbo].[DTM_SEQU_RFRC_LIST].[NU_VERS_SEQU]"
                            Using dt_SEQU = SQL_SELE_TO_DT(sQuery, CS_ALMS_PROD_PRD)
                                pt = New PdfPTable(3)
                                pt.TotalWidth = dPDF.PageSize.Width - 80
                                pt.LockedWidth = True
                                Dim widths_4() As Single = {3, 1, 1}
                                pt.SetWidths(widths_4)

                                para = New Paragraph(New Chunk(“Liste des séquenceurs et logiciels”, FontFactory.GetFont("ARIAL", 16, Font.BOLDITALIC)))
                                c_ENTE = New PdfPCell(para)
                                c_ENTE.Border = 0
                                pt.AddCell(c_ENTE)
                                para = New Paragraph(New Chunk(“Date d'application”, FontFactory.GetFont("ARIAL", 10)))
                                c_ENTE = New PdfPCell(para)
                                c_ENTE.Border = 0
                                c_ENTE.VerticalAlignment = Element.ALIGN_BOTTOM
                                pt.AddCell(c_ENTE)
                                para = New Paragraph(New Chunk(“ID séquence”, FontFactory.GetFont("ARIAL", 10)))
                                c_ENTE = New PdfPCell(para)
                                c_ENTE.Border = 0
                                c_ENTE.VerticalAlignment = Element.ALIGN_BOTTOM
                                pt.AddCell(c_ENTE)
                                For Each rdt As DataRow In dt_SEQU.Rows
                                    para = New Paragraph(New Chunk($"{rdt("NM_RFRC_SEQU").ToString}_V{rdt("NU_VERS_SEQU").ToString}", FontFactory.GetFont("ARIAL", 10)))
                                    c_ENTE = New PdfPCell(para)
                                    pt.AddCell(c_ENTE)
                                    para = New Paragraph(New Chunk(Left(rdt("DT_APCT").ToString, 10), FontFactory.GetFont("ARIAL", 10)))
                                    c_ENTE = New PdfPCell(para)
                                    pt.AddCell(c_ENTE)
                                    para = New Paragraph(New Chunk(rdt("ID_RFRC_SEQU").ToString, FontFactory.GetFont("ARIAL", 10)))
                                    c_ENTE = New PdfPCell(para)
                                    pt.AddCell(c_ENTE)
                                Next
                                pt.SpacingAfter = 10
                                dPDF.Add(pt)

                                'liste métrologie
                                pt = New PdfPTable(4)
                                pt.TotalWidth = dPDF.PageSize.Width - 80
                                pt.LockedWidth = True
                                Dim widths_5() As Single = {1, 1, 2, 1}
                                pt.SetWidths(widths_5)
                                para = New Paragraph(New Chunk(“Liste des appareils”, FontFactory.GetFont("ARIAL", 16, Font.BOLDITALIC)))
                                c_ENTE = New PdfPCell(para)
                                c_ENTE.Border = 0
                                c_ENTE.Colspan = 4
                                pt.AddCell(c_ENTE)
                                sQuery = $"SELECT DTM_CSTA_POST_1.ID_POST, DTM_CSTA_POST_1.ID_MTRE, ISNULL(MES_Digital_Factory.dbo.V_POST_MTLG_MTRE.DT_VLDT, N'') AS DT_VLDT, ISNULL(dt_DCPO_MTRE.LB_DCPT, MES_Digital_Factory.dbo.V_POST_MTLG_MTRE.NM_TYPE_MTRE) AS NM_TYPE_MTRE
                                             FROM (SELECT ID_POST, ID_MTRE, MAX(DT_AFCT) AS MAX_DT_AFCT, ISNULL(BL_SORT_FCGF, 0) AS BL_SORT_FCGF
                                                     FROM dbo.DTM_CSTA_POST
                                                   GROUP BY ID_POST, ID_MTRE, BL_SORT_FCGF
                                                  ) AS DT_DERN_AFCT INNER JOIN dbo.DTM_CSTA_POST AS DTM_CSTA_POST_1 ON DT_DERN_AFCT.ID_POST = DTM_CSTA_POST_1.ID_POST AND DT_DERN_AFCT.ID_MTRE = DTM_CSTA_POST_1.ID_MTRE AND DT_DERN_AFCT.MAX_DT_AFCT = DTM_CSTA_POST_1.DT_AFCT INNER JOIN
                                                  (SELECT CASE WHEN ID_POST LIKE 'P-%' THEN ID_POST ELSE 'POSTE_' + ID_POST END AS ID_POST
                                                     FROM dbo.V_FCGF
                                                    WHERE (NU_SER_SPFE = '{sNU_SER}')
                                                   GROUP BY NU_SER_SPFE, ID_POST
                                                  ) AS DT_LIST_POST ON DT_DERN_AFCT.ID_POST = DT_LIST_POST.ID_POST INNER JOIN
                                                  (SELECT ID_MTRE, LB_DCPT
                                                     FROM  dbo.DTM_REF_MTRE_LIST
                                                   GROUP BY ID_MTRE, LB_DCPT
                                                  ) AS dt_DCPO_MTRE ON DTM_CSTA_POST_1.ID_MTRE = dt_DCPO_MTRE.ID_MTRE LEFT OUTER JOIN MES_Digital_Factory.dbo.V_POST_MTLG_MTRE ON DTM_CSTA_POST_1.ID_MTRE = MES_Digital_Factory.dbo.V_POST_MTLG_MTRE.ID_MTRE
                                            WHERE (DTM_CSTA_POST_1.BL_STAT = 1) AND (DTM_CSTA_POST_1.BL_SORT_FCGF = 1)"
                                Using dt = SQL_SELE_TO_DT(sQuery, CS_ALMS_PROD_PRD)
                                    para = New Paragraph(New Chunk("Poste", FontFactory.GetFont("ARIAL", 10, Font.BOLD)))
                                    c_ENTE = New PdfPCell(para)
                                    c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                                    pt.AddCell(c_ENTE)
                                    para = New Paragraph(New Chunk("Id matériel", FontFactory.GetFont("ARIAL", 10, Font.BOLD)))
                                    c_ENTE = New PdfPCell(para)
                                    c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                                    pt.AddCell(c_ENTE)
                                    para = New Paragraph(New Chunk("Type de matériel", FontFactory.GetFont("ARIAL", 10, Font.BOLD)))
                                    c_ENTE = New PdfPCell(para)
                                    c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                                    pt.AddCell(c_ENTE)
                                    para = New Paragraph(New Chunk("Date de validité", FontFactory.GetFont("ARIAL", 10, Font.BOLD)))
                                    c_ENTE = New PdfPCell(para)
                                    c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                                    pt.AddCell(c_ENTE)
                                    For Each rdt As DataRow In dt.Rows
                                        para = New Paragraph(New Chunk(rdt("ID_POST").ToString, FontFactory.GetFont("ARIAL", 10)))
                                        c_ENTE = New PdfPCell(para)
                                        pt.AddCell(c_ENTE)
                                        para = New Paragraph(New Chunk(rdt("ID_MTRE").ToString, FontFactory.GetFont("ARIAL", 10)))
                                        c_ENTE = New PdfPCell(para)
                                        pt.AddCell(c_ENTE)
                                        'type matériel = Interface de test
                                        para = New Paragraph(New Chunk(rdt("NM_TYPE_MTRE").ToString, FontFactory.GetFont("ARIAL", 10)))
                                        c_ENTE = New PdfPCell(para)
                                        pt.AddCell(c_ENTE)
                                        If rdt("NM_TYPE_MTRE").ToString = "Interface de test" Then
                                            Using dtZMOYENTYPETEST = SAP_DATA_READ_ZMOYENTYPETEST($"NO_MOYEN EQ '{rdt("ID_MTRE").ToString}'")
                                                If dtZMOYENTYPETEST Is Nothing Then Throw New Exception($"Pas de date de validité pour le banc n°{rdt("NM_TYPE_MTRE").ToString} sous SAP.")
                                                para = New Paragraph(New Chunk(COMM_APP_WEB_CONV_FORM_DATE(Left(dtZMOYENTYPETEST(0)("PROCHVERIF").ToString, 10).Insert(6, "/").Insert(4, "/"), "dd/MM/yyyy"), FontFactory.GetFont("ARIAL", 10)))
                                            End Using
                                        Else
                                            para = New Paragraph(New Chunk(Left(rdt("DT_VLDT").ToString, 10), FontFactory.GetFont("ARIAL", 10)))
                                        End If
                                        c_ENTE = New PdfPCell(para)
                                        pt.AddCell(c_ENTE)
                                    Next
                                    pt.SpacingAfter = 10
                                    dPDF.Add(pt)
                                End Using

                                'liste opérateur
                                pt = New PdfPTable(2)
                                pt.TotalWidth = dPDF.PageSize.Width - 80
                                pt.LockedWidth = True
                                para = New Paragraph(New Chunk(“Liste des opérateurs de la FCGF”, FontFactory.GetFont("ARIAL", 16, Font.BOLDITALIC)))
                                c_ENTE = New PdfPCell(para)
                                c_ENTE.Border = 0
                                c_ENTE.Colspan = 2
                                pt.AddCell(c_ENTE)
                                sQuery = $"SELECT MAX([DT_TEST]) AS MAX_DT_TEST,[ID_OPRT]
                                             FROM [dbo].[V_FCGF]
                                            WHERE [NU_SER_SPFE] = '{sNU_SER}'
                                           GROUP BY [NU_SER_SPFE], [ID_OPRT]"
                                Using dt = SQL_SELE_TO_DT(sQuery, CS_ALMS_PROD_PRD)
                                    para = New Paragraph(New Chunk("Opérateur", FontFactory.GetFont("ARIAL", 10, Font.BOLD)))
                                    c_ENTE = New PdfPCell(para)
                                    c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                                    pt.AddCell(c_ENTE)
                                    para = New Paragraph(New Chunk("Date de phase", FontFactory.GetFont("ARIAL", 10, Font.BOLD)))
                                    c_ENTE = New PdfPCell(para)
                                    c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                                    pt.AddCell(c_ENTE)
                                    For Each rdt As DataRow In dt.Rows
                                        para = New Paragraph(New Chunk(rdt("ID_OPRT").ToString, FontFactory.GetFont("ARIAL", 10)))
                                        c_ENTE = New PdfPCell(para)
                                        pt.AddCell(c_ENTE)
                                        para = New Paragraph(New Chunk(rdt("MAX_DT_TEST").ToString, FontFactory.GetFont("ARIAL", 10)))
                                        c_ENTE = New PdfPCell(para)
                                        pt.AddCell(c_ENTE)
                                    Next
                                    pt.SpacingAfter = 10
                                    dPDF.Add(pt)
                                End Using

                                'liste observation
                                pt = New PdfPTable(1)
                                pt.TotalWidth = dPDF.PageSize.Width - 80
                                pt.LockedWidth = True
                                para = New Paragraph(New Chunk(“Liste des observations”, FontFactory.GetFont("ARIAL", 16, Font.BOLDITALIC)))
                                c_ENTE = New PdfPCell(para)
                                c_ENTE.Border = 0
                                pt.AddCell(c_ENTE)
                                sQuery = $"SELECT dbo.DTM_NC_LIST.NU_INCI
                                            FROM (SELECT NU_INCI, MAX(DT_ITVT) AS MAX_DT_ITVT, MAX(ID_STAT) AS DERN_ETAT
                                                    FROM dbo.DTM_NC_DETA AS DTM_NC_DETA_1
                                                  GROUP BY NU_INCI
                                                 ) AS DT_MAX_ITVT INNER JOIN dbo.DTM_NC_LIST ON DT_MAX_ITVT.NU_INCI = dbo.DTM_NC_LIST.NU_INCI
                                           WHERE [NU_SER_SPFE] = '{sNU_SER}' AND  (DT_MAX_ITVT.DERN_ETAT >= 4)"
                                Using dt = SQL_SELE_TO_DT(sQuery, CS_ALMS_PROD_PRD)
                                    If Not dt Is Nothing Then
                                        For Each rdt As DataRow In dt.Rows
                                            sb_list_inci.Append($"{rdt("NU_INCI").ToString}, ")
                                        Next
                                        para = New Paragraph(New Chunk(COMM_APP_WEB_STRI_TRIM_RIGHT(sb_list_inci.ToString, 2), FontFactory.GetFont("ARIAL", 10)))
                                        c_ENTE = New PdfPCell(para)
                                        pt.AddCell(c_ENTE)
                                    End If
                                    dPDF.Add(pt)
                                End Using

                                'Séquence
                                For Each rdt_SEQU As DataRow In dt_SEQU.Rows
                                    dPDF.NewPage()
                                    pt = New PdfPTable(2)
                                    pt.TotalWidth = dPDF.PageSize.Width - 80
                                    pt.LockedWidth = True
                                    Dim widths_3() As Single = {3, 2}
                                    pt.SetWidths(widths_3)
                                    para = New Paragraph(New Chunk($"{rdt_SEQU("NM_RFRC_SEQU").ToString}_V{rdt_SEQU("NU_VERS_SEQU").ToString}", FontFactory.GetFont("ARIAL", 16, Font.BOLDITALIC)))
                                    c_ENTE = New PdfPCell(para)
                                    c_ENTE.Border = 0
                                    pt.AddCell(c_ENTE)

                                    'phase
                                    '       pour chaque phase un tableau
                                    sQuery = $"SELECT [DT_TEST],[NM_PHAS],[LB_DCPT_STAT],[LB_STAT_PHAS],[ID_POST],[ID_OPRT],[NU_PHAS_FCGF]
                                                 FROM [dbo].[V_FCGF]
                                                WHERE [NU_SER_SPFE] = '{sNU_SER}' AND [NM_RFRC_SEQU] = '{rdt_SEQU("NM_RFRC_SEQU").ToString}' AND [NU_VERS_SEQU] = '{rdt_SEQU("NU_VERS_SEQU").ToString}'
                                               GROUP BY [NU_SER_SPFE],[NM_RFRC_SEQU],[NU_VERS_SEQU],[DT_TEST],[NM_PHAS],[LB_DCPT_STAT],[LB_STAT_PHAS],[ID_POST],[ID_OPRT],[NU_OP],[NU_PHAS_FCGF]
                                               ORDER BY [NU_OP],[NU_PHAS_FCGF]"
                                    Using dt_PHAS = SQL_SELE_TO_DT(sQuery, CS_ALMS_PROD_PRD)
                                        para = New Paragraph(New Chunk($"Date de test : {dt_PHAS(0)("DT_TEST").ToString}", FontFactory.GetFont("ARIAL", 10)))
                                        c_ENTE = New PdfPCell(para)
                                        c_ENTE.HorizontalAlignment = Element.ALIGN_RIGHT
                                        c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                                        c_ENTE.Border = 0
                                        pt.AddCell(c_ENTE)
                                        pt.SpacingAfter = 10
                                        dPDF.Add(pt)

                                        For Each rdt_PHAS As DataRow In dt_PHAS.Rows
                                            pt = New PdfPTable(8)
                                            pt.TotalWidth = dPDF.PageSize.Width - 80
                                            pt.LockedWidth = True
                                            Dim widths_2() As Single = {4, 20, 6, 6, 6, 5, 10, 3}
                                            pt.SetWidths(widths_2)
                                            para = New Paragraph(New Chunk(rdt_PHAS("NM_PHAS").ToString, FontFactory.GetFont("ARIAL", 12, Font.BOLD)))
                                            c_ENTE = New PdfPCell(para)
                                            c_ENTE.Border = 0
                                            c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                                            c_ENTE.Colspan = 8
                                            pt.AddCell(c_ENTE)
                                            para = New Paragraph(New Chunk($"Statut de la phase : {rdt_PHAS("LB_STAT_PHAS").ToString}", FontFactory.GetFont("ARIAL", 10)))
                                            c_ENTE = New PdfPCell(para)
                                            c_ENTE.Colspan = 3
                                            c_ENTE.Border = 0
                                            pt.AddCell(c_ENTE)
                                            para = New Paragraph(New Chunk($"Poste :   {rdt_PHAS("ID_POST").ToString}", FontFactory.GetFont("ARIAL", 10)))
                                            c_ENTE = New PdfPCell(para)
                                            c_ENTE.Colspan = 3
                                            c_ENTE.Border = 0
                                            pt.AddCell(c_ENTE)
                                            para = New Paragraph(New Chunk($"Opérateur : {rdt_PHAS("ID_OPRT").ToString}", FontFactory.GetFont("ARIAL", 10)))
                                            c_ENTE = New PdfPCell(para)
                                            c_ENTE.Colspan = 2
                                            c_ENTE.Border = 0
                                            pt.AddCell(c_ENTE)

                                            '           pour chaque sous-pahse une ligne
                                            para = New Paragraph(New Chunk("Index", FontFactory.GetFont("ARIAL", 10, Font.BOLD)))
                                            c_ENTE = New PdfPCell(para)
                                            c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                                            pt.AddCell(c_ENTE)
                                            para = New Paragraph(New Chunk("Description", FontFactory.GetFont("ARIAL", 10, Font.BOLD)))
                                            c_ENTE = New PdfPCell(para)
                                            c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                                            pt.AddCell(c_ENTE)
                                            para = New Paragraph(New Chunk("Min ≤", FontFactory.GetFont("ARIAL", 10, Font.BOLD)))
                                            c_ENTE = New PdfPCell(para)
                                            c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                                            pt.AddCell(c_ENTE)
                                            para = New Paragraph(New Chunk("Mesure", FontFactory.GetFont("ARIAL", 10, Font.BOLD)))
                                            c_ENTE = New PdfPCell(para)
                                            c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                                            pt.AddCell(c_ENTE)
                                            para = New Paragraph(New Chunk("≤ Max", FontFactory.GetFont("ARIAL", 10, Font.BOLD)))
                                            c_ENTE = New PdfPCell(para)
                                            c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                                            pt.AddCell(c_ENTE)
                                            para = New Paragraph(New Chunk("Unité", FontFactory.GetFont("ARIAL", 10, Font.BOLD)))
                                            c_ENTE = New PdfPCell(para)
                                            c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                                            pt.AddCell(c_ENTE)
                                            para = New Paragraph(New Chunk("Saisie", FontFactory.GetFont("ARIAL", 10, Font.BOLD)))
                                            c_ENTE = New PdfPCell(para)
                                            c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                                            pt.AddCell(c_ENTE)
                                            para = New Paragraph(New Chunk("OK", FontFactory.GetFont("ARIAL", 10, Font.BOLD)))
                                            c_ENTE = New PdfPCell(para)
                                            c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                                            pt.AddCell(c_ENTE)
                                            sQuery = $"SELECT [NM_SOUS_PHAS], [NU_LIMI_BASS], [NU_VALE], [LB_VALE], [NU_LIMI_HAUT], [NU_UNIT], [NU_ORDR_SEQU], Convert(Of Integer, [BL_STAT_PHAS]) AS BL_STAT_PHAS
                                                         FROM [dbo].[V_FCGF]
                                                        WHERE [NU_SER_SPFE] = '{sNU_SER}' AND [NU_PHAS_FCGF] = '{Replace(rdt_PHAS("NU_PHAS_FCGF").ToString, "'", "''")}' AND [DT_TEST] = '{rdt_PHAS("DT_TEST").ToString}' AND [NM_RFRC_SEQU] = '{rdt_SEQU("NM_RFRC_SEQU").ToString}' And [NU_VERS_SEQU] = '{rdt_SEQU("NU_VERS_SEQU").ToString}'
                                                       GROUP BY [NU_SER_SPFE],[NM_RFRC_SEQU],[NU_VERS_SEQU],[NM_PHAS],[DT_TEST],[NM_SOUS_PHAS],[NU_LIMI_BASS],[NU_VALE],[LB_VALE],[NU_LIMI_HAUT],[NU_UNIT],[NU_OP],[NU_ORDR_SEQU],[NU_PHAS_FCGF],[NU_SOUS_PHAS_FCGF],[BL_STAT_PHAS]
                                                       ORDER BY [NU_OP],[NU_PHAS_FCGF],[NU_SOUS_PHAS_FCGF]"
                                            Using dt = SQL_SELE_TO_DT(sQuery, CS_ALMS_PROD_PRD)
                                                For Each rdt As DataRow In dt.Rows
                                                    para = New Paragraph(New Chunk(rdt("NU_ORDR_SEQU").ToString, FontFactory.GetFont("ARIAL", 10)))
                                                    c_ENTE = New PdfPCell(para)
                                                    pt.AddCell(c_ENTE)
                                                    para = New Paragraph(New Chunk(rdt("NM_SOUS_PHAS").ToString, FontFactory.GetFont("ARIAL", 10)))
                                                    c_ENTE = New PdfPCell(para)
                                                    pt.AddCell(c_ENTE)
                                                    para = New Paragraph(New Chunk(rdt("NU_LIMI_BASS").ToString, FontFactory.GetFont("ARIAL", 10)))
                                                    c_ENTE = New PdfPCell(para)
                                                    c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                                                    pt.AddCell(c_ENTE)
                                                    para = New Paragraph(New Chunk(rdt("NU_VALE").ToString, FontFactory.GetFont("ARIAL", 10)))
                                                    c_ENTE = New PdfPCell(para)
                                                    c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                                                    pt.AddCell(c_ENTE)
                                                    para = New Paragraph(New Chunk(rdt("NU_LIMI_HAUT").ToString, FontFactory.GetFont("ARIAL", 10)))
                                                    c_ENTE = New PdfPCell(para)
                                                    c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                                                    pt.AddCell(c_ENTE)
                                                    para = New Paragraph(New Chunk(rdt("NU_UNIT").ToString, FontFactory.GetFont("ARIAL", 10)))
                                                    c_ENTE = New PdfPCell(para)
                                                    pt.AddCell(c_ENTE)
                                                    para = New Paragraph(New Chunk(rdt("LB_VALE").ToString, FontFactory.GetFont("ARIAL", 10)))
                                                    c_ENTE = New PdfPCell(para)
                                                    pt.AddCell(c_ENTE)
                                                    para = New Paragraph(New Chunk(rdt("BL_STAT_PHAS").ToString, FontFactory.GetFont("ARIAL", 10)))
                                                    c_ENTE = New PdfPCell(para)
                                                    pt.AddCell(c_ENTE)
                                                Next
                                                pt.KeepTogether = True
                                                pt.SpacingAfter = 10
                                                dPDF.Add(pt)
                                            End Using
                                        Next
                                    End Using
                                Next
                                para = New Paragraph(New Chunk($"Statut global :  {dt_RESU(0)("NM_STAT_PROD").ToString}", FontFactory.GetFont("ARIAL", 14, Font.BOLD)))
                                dPDF.Add(para)
                                dPDF.Close()
                            End Using
                        End Using
                    End Using
                End Using
            End Using
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "alert")
            Return Nothing
        Finally
            'dPDF.Close()
        End Try
        Return sfich
    End Function

    Public Shared Function DOC_ITEXT_SHARP_CREA_DHR(sNU_SER As String, sOF As String, sDES_ARTI As String, sCD_ARTI As String, snm_oprt As String, scheminlistetracabilité As String) As String
        Dim sQuery As String = ""
        Dim pt As PdfPTable
        Dim c_ENTE As PdfPCell
        Randomize()
        Dim para As Paragraph
        Dim phr As Phrase
        Dim sdatefcgf As String = ""
        Dim sfich As String = $"c:\sources\temp_App_Web\{CInt(Int((10000000 * Rnd()) + 1))}.pdf”
        Try
            Using dPDF = New Document(PageSize.A4, 20, 20, 30, 20)
                Dim writer = PdfAWriter.GetInstance(dPDF, New FileStream(sfich, FileMode.Create))
                Dim PageEventHandler As New HeaderFooter_DHR()
                writer.PageEvent = PageEventHandler
                dPDF.Open()

                'of
                pt = New PdfPTable(2)
                With pt
                    pt.LockedWidth = True
                    pt.TotalWidth = dPDF.PageSize.Width - 100
                    Dim widths() As Single = {18, 26}
                    pt.SetWidths(widths)
                    para = New Paragraph(New Chunk(“Référence du produit : ”, FontFactory.GetFont("CALIBRI", 14, Font.BOLD)))
                    c_ENTE = New PdfPCell(para)
                    c_ENTE.Border = 0
                    c_ENTE.HorizontalAlignment = Element.ALIGN_RIGHT
                    pt.AddCell(c_ENTE)
                    para = New Paragraph(New Chunk($"{sDES_ARTI} - {sCD_ARTI}", FontFactory.GetFont("CALIBRI", 14)))
                    c_ENTE = New PdfPCell(para)
                    c_ENTE.Border = 0
                    c_ENTE.BorderWidthBottom = 1
                    pt.AddCell(c_ENTE)
                    para = New Paragraph(New Chunk(“Référence ALMS : ”, FontFactory.GetFont("CALIBRI", 14, Font.BOLD)))
                    c_ENTE = New PdfPCell(para)
                    c_ENTE.Border = 0
                    c_ENTE.HorizontalAlignment = Element.ALIGN_RIGHT
                    pt.AddCell(c_ENTE)
                    Using dtKNMT = SAP_DATA_READ_KNMT($"MATNR LIKE '{sCD_ARTI}%' AND KUNNR EQ '0000000451' AND VKORG EQ 'ORC3' AND VTWEG EQ 'CD'")
                        If dtKNMT Is Nothing Then Throw New Exception("Code article client introuvable sous SAP")
                        para = New Paragraph(New Chunk(Trim(dtKNMT(0)("KDMAT").ToString), FontFactory.GetFont("CALIBRI", 14)))
                    End Using
                    c_ENTE = New PdfPCell(para)
                    c_ENTE.Border = 0
                    c_ENTE.BorderWidthBottom = 1
                    pt.AddCell(c_ENTE)
                    para = New Paragraph(New Chunk(“Numéro de série : ”, FontFactory.GetFont("CALIBRI", 14, Font.BOLD)))
                    c_ENTE = New PdfPCell(para)
                    c_ENTE.Border = 0
                    c_ENTE.HorizontalAlignment = Element.ALIGN_RIGHT
                    pt.AddCell(c_ENTE)
                    para = New Paragraph(New Chunk(sNU_SER, FontFactory.GetFont("CALIBRI", 14)))
                    c_ENTE = New PdfPCell(para)
                    c_ENTE.Border = 0
                    c_ENTE.BorderWidthBottom = 1
                    pt.AddCell(c_ENTE)
                    para = New Paragraph(New Chunk(“Numéro d'OF : ”, FontFactory.GetFont("CALIBRI", 14, Font.BOLD)))
                    c_ENTE = New PdfPCell(para)
                    c_ENTE.Border = 0
                    c_ENTE.HorizontalAlignment = Element.ALIGN_RIGHT
                    pt.AddCell(c_ENTE)
                    para = New Paragraph(New Chunk(sOF, FontFactory.GetFont("CALIBRI", 14)))
                    c_ENTE = New PdfPCell(para)
                    c_ENTE.Border = 0
                    c_ENTE.BorderWidthBottom = 1
                    pt.AddCell(c_ENTE)
                    para = New Paragraph(New Chunk(“Indice dossier de fabrication : ”, FontFactory.GetFont("CALIBRI", 14, Font.BOLD)))
                    c_ENTE = New PdfPCell(para)
                    c_ENTE.Border = 0
                    c_ENTE.HorizontalAlignment = Element.ALIGN_RIGHT
                    pt.AddCell(c_ENTE)
                    Using dtAFKO = SAP_DATA_READ_AFKO($"AUFNR LIKE '%{sOF}'")
                        If dtAFKO Is Nothing Then Throw New Exception("l'indice de la gamme non trouvé")
                        para = New Paragraph(New Chunk(dtAFKO(0)("REVLV").ToString, FontFactory.GetFont("CALIBRI", 14)))
                    End Using
                    c_ENTE = New PdfPCell(para)
                    c_ENTE.Border = 0
                    c_ENTE.BorderWidthBottom = 1
                    pt.AddCell(c_ENTE)
                    dPDF.Add(pt)
                End With

                dPDF.Add(New Phrase("
                "))


                pt = New PdfPTable(1)
                pt.LockedWidth = True
                pt.TotalWidth = dPDF.PageSize.Width - 250
                para = New Paragraph(New Chunk(“Acceptation du produit : Revu et approuvé”, FontFactory.GetFont("CALIBRI", 14, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.PaddingTop = 10
                c_ENTE.PaddingBottom = 10
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)

                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk($“Acceptation du produit le : {Now.ToString}", FontFactory.GetFont("CALIBRI", 12, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.PaddingTop = 15
                c_ENTE.PaddingBottom = 15
                pt.AddCell(c_ENTE)

                Dim sNOM_PREN_VISA As String = $“Nom, Prénom : {snm_oprt}"
                para = New Paragraph(New Chunk(sNOM_PREN_VISA, FontFactory.GetFont("CALIBRI", 12, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.PaddingTop = 15
                c_ENTE.PaddingBottom = 15
                pt.AddCell(c_ENTE)
                dPDF.Add(pt)

                dPDF.Add(New Phrase("
                "))
                'hist de prod
                para = New Paragraph(New Chunk(“1. HISTORIQUE DE PRODUCTION”, FontFactory.GetFont("CALIBRI", 14, Font.BOLD Or Font.UNDERLINE)))
                para.SpacingAfter = 20
                dPDF.Add(para)

                pt = New PdfPTable(5)
                pt.LockedWidth = True
                pt.TotalWidth = dPDF.PageSize.Width - 80
                Dim widths_2() As Single = {45, 15, 25, 25, 15}
                pt.SetWidths(widths_2)
                para = New Paragraph(New Chunk(“Liste des Opérations de Fabrication et de Contrôle”, FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.Rowspan = 2
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk(“Doc de réf”, FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.Rowspan = 2
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk(“Intervention”, FontFactory.GetFont("CALIBRI", 11, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.Rowspan = 2
                c_ENTE.Colspan = 2
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk(“Résultats”, FontFactory.GetFont("CALIBRI", 11, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk(“(OK/NOK)”, FontFactory.GetFont("CALIBRI", 11, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                sQuery = $"SELECT ISNULL(DT_DERN_PASS.DER_DT_DEB, '') AS DT_PASS, DT_WF.NM_OPRT, DT_WF.CD_ARTI, DT_WF.VAL_PARA, DT_WF.ID_NU_SER, CASE ISNULL(dbo.DTM_PSG.LB_SCTN, 'F') WHEN 'P' THEN 'OK' ELSE 'NOK' END AS BL_RESU, ISNULL(dbo.DTM_PSG.NM_MATR, '') AS ID_MTCL, dbo.DTM_PSG.LB_ETP, REPLACE(REPLACE(REPLACE(DT_WF.VAL_PARA, DT_WF.NM_OPRT, ''), '(OP:', ''), ')', '') AS NU_OP, dbo.DTM_PSG.[LB_MOYN]
                             FROM dbo.DTM_PSG INNER JOIN (SELECT NM_OF, LB_ETP, DER_DT_DEB, NU_SER_CLIE, NU_SER_ECO
                                                            FROM dbo.V_DER_PASS_BON
                                                           WHERE (NU_SER_CLIE = N'{sNU_SER}')
                                                         ) AS DT_DERN_PASS ON dbo.DTM_PSG.LB_ETP = DT_DERN_PASS.LB_ETP AND dbo.DTM_PSG.NM_OF = DT_DERN_PASS.NM_OF AND dbo.DTM_PSG.DT_DEB = DT_DERN_PASS.DER_DT_DEB RIGHT OUTER JOIN
                                                        (SELECT DT_WF_1.NU_ETAP, DT_WF_1.NM_OPRT, DT_WF_1.CD_ARTI, DT_WF_1.VAL_PARA, dbo.V_LIAIS_NU_SER.NU_SER_CLIE, dbo.V_LIAIS_NU_SER.ID_NU_SER
                                                           FROM (SELECT CD_ARTI, LEFT(VAL_PARA, CHARINDEX(' (OP:', VAL_PARA)) AS NM_OPRT, VAL_PARA, REPLACE(NM_PARA, 'WORKFLOW étape ', '') AS NU_ETAP
                                                                   FROM dbo.V_DER_DTM_REF_PARA
                                                                  WHERE (NM_PARA LIKE N'WORKFLOW étape%')
                                                                 ) AS DT_WF_1 INNER JOIN SAP.dbo.V_MES_DIG_FACT_IDCT ON DT_WF_1.CD_ARTI = SAP.dbo.V_MES_DIG_FACT_IDCT.PLNBEZ INNER JOIN dbo.V_LIAIS_NU_SER ON SAP.dbo.V_MES_DIG_FACT_IDCT.AUFNR = dbo.V_LIAIS_NU_SER.NU_OF
                                                          WHERE (dbo.V_LIAIS_NU_SER.NU_SER_CLIE = N'{sNU_SER}')
                                                        ) AS DT_WF ON DT_DERN_PASS.LB_ETP = DT_WF.VAL_PARA AND dbo.DTM_PSG.ID_NU_SER = DT_WF.ID_NU_SER
                            ORDER BY CONVERT(integer, DT_WF.NU_ETAP)"
                Using dt = SQL_SELE_TO_DT(sQuery, CS_MES_Digital_Factory)
                    For Each rdt As DataRow In dt.Rows
                        If Trim(rdt("NM_OPRT").ToString) = "ACCEPTATION QUALITE" Then Continue For
                        If Trim(rdt("NM_OPRT").ToString) = "PALETTISATION" Then Continue For
                        If Trim(rdt("NM_OPRT").ToString) = "CONTROLE FINAL" Then sdatefcgf = COMM_APP_WEB_CONV_FORM_DATE(rdt("DT_PASS").ToString, "ddMMyyyy_HHmmss")
                        sQuery = $"SELECT [NU_OF],[DT_DEB],[LB_ETP],[LB_CONS_ETAP],CASE WHEN [NB_VAL] BETWEEN CONVERT(FLOAT, REPLACE([NU_LIMI_IFRE_ETAP], ',', '.')) AND CONVERT(FLOAT, REPLACE([NU_LIMI_SPRE_ETAP], ',', '.')) THEN 'OK' ELSE 'NOK' END AS BL_SANC
                                     FROM [dbo].[V_RSLT_POIN_ARRE_TCBL_OPRT]
                                    WHERE NU_SER_CLIE = '{sNU_SER}' AND [DT_DEB] = '{rdt("DT_PASS").ToString}' AND [LB_ETP] = '{rdt("LB_ETP").ToString}'
                                   ORDER BY [ID_INDE]"
                        Using dt_ETAP = SQL_SELE_TO_DT(sQuery, CS_MES_Digital_Factory)
                            para = New Paragraph(New Chunk(rdt("NM_OPRT").ToString, FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                            c_ENTE = New PdfPCell(para)
                            c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                            c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                            pt.AddCell(c_ENTE)

                            Dim sdoc As String = "vide", snu_doc As String = ""
                            Using dt_T_TAB_DOC_SAP = SAP_DATA_Z_GET_DOC_INFO(sOF, rdt("NU_OP").ToString)
                                If Not dt_T_TAB_DOC_SAP Is Nothing Then
                                    snu_doc = Trim(dt_T_TAB_DOC_SAP(0)("DOKNR").ToString)
                                    sdoc = Trim(dt_T_TAB_DOC_SAP(0)("DKTXT").ToString)
                                End If
                            End Using
                            If (sdoc = "" Or sdoc = "vide") And snu_doc <> "" Then
                                Using dt2 = SAP_DATA_READ_DRAT($"DOKNR LIKE '%{snu_doc}'")
                                    If dt2 Is Nothing Then
                                        sdoc = "vide"
                                    Else
                                        sdoc = dt2(0)("DKTXT").ToString
                                    End If
                                End Using
                            End If
                            para = New Paragraph(New Chunk(sdoc, FontFactory.GetFont("CALIBRI", 10)))
                            c_ENTE = New PdfPCell(para)
                            If Not dt_ETAP Is Nothing Then c_ENTE.Rowspan = dt_ETAP.Rows.Count + 1
                            c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                            c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                            pt.AddCell(c_ENTE)

                            Using dtPA0002 = SAP_DATA_READ_TBL("PA0002", "|", "", "PERNR NACHN VORNA", $"PERNR EQ '*{rdt("ID_MTCL").ToString}'")
                                If dtPA0002 Is Nothing Then 'Throw New Exception("pas de nom/prénom trouvés pour le matricule " & rdt("ID_MTCL").ToString)
                                    para = New Paragraph(New Chunk("", FontFactory.GetFont("CALIBRI", 10)))
                                Else
                                    para = New Paragraph(New Chunk($"{Trim(dtPA0002(0)("NACHN").ToString)} {Trim(dtPA0002(0)("VORNA").ToString)}", FontFactory.GetFont("CALIBRI", 10)))
                                End If
                            End Using
                            ' Dim sNom_Prenom As String =
                            c_ENTE = New PdfPCell(para)
                            If Not dt_ETAP Is Nothing Then c_ENTE.Rowspan = dt_ETAP.Rows.Count + 1
                            c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                            c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                            pt.AddCell(c_ENTE)

                            para = New Paragraph(New Chunk(rdt("DT_PASS").ToString, FontFactory.GetFont("CALIBRI", 10)))
                            c_ENTE = New PdfPCell(para)
                            If Not dt_ETAP Is Nothing Then c_ENTE.Rowspan = dt_ETAP.Rows.Count + 1
                            c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                            c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                            pt.AddCell(c_ENTE)

                            para = New Paragraph(New Chunk(rdt("BL_RESU").ToString, FontFactory.GetFont("CALIBRI", 10)))
                            c_ENTE = New PdfPCell(para)
                            c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                            c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                            pt.AddCell(c_ENTE)

                            If Not dt_ETAP Is Nothing Then
                                For Each rdt_ETAP As DataRow In dt_ETAP.Rows
                                    para = New Paragraph(New Chunk(rdt_ETAP("LB_CONS_ETAP").ToString, FontFactory.GetFont("CALIBRI", 10)))
                                    c_ENTE = New PdfPCell(para)
                                    c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                                    pt.AddCell(c_ENTE)

                                    para = New Paragraph(New Chunk(rdt_ETAP("BL_SANC").ToString, FontFactory.GetFont("CALIBRI", 10)))
                                    c_ENTE = New PdfPCell(para)
                                    c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                                    c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                                    pt.AddCell(c_ENTE)
                                Next
                            End If
                        End Using
                    Next
                End Using
                'pt.KeepTogether = True
                pt.HeaderRows = 2
                pt.SpacingAfter = 30
                dPDF.Add(pt)

                'traçabilité des composants
                para = New Paragraph(New Chunk(“2. TRAÇABILITE DES COMPOSANTS”, FontFactory.GetFont("CALIBRI", 14, Font.BOLD Or Font.UNDERLINE)))
                para.SpacingAfter = 20
                dPDF.Add(para)
                Dim slist_tarça As String = $"Voir annexe 3 : Liste traçabilité {sNU_SER} – NM"
                para = New Paragraph(New Chunk(slist_tarça, FontFactory.GetFont("CALIBRI", 10)))
                para.SpacingAfter = 30
                dPDF.Add(para)

                'traçabilité des composants
                para = New Paragraph(New Chunk(“3. MOYENS ET OUTILLAGES”, FontFactory.GetFont("CALIBRI", 14, Font.BOLD Or Font.UNDERLINE)))
                para.SpacingAfter = 20
                dPDF.Add(para)
                pt = New PdfPTable(4)
                pt.LockedWidth = True
                pt.TotalWidth = dPDF.PageSize.Width - 80
                Dim widths_4() As Single = {20, 40, 20, 20}
                pt.SetWidths(widths_4)
                para = New Paragraph(New Chunk("N° EOLANE", FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk("Désignation", FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk("Validité", FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk("N° du poste affecté", FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                pt.SpacingAfter = 30
                sQuery = $"SELECT TOP (100) PERCENT dbo.V_LIAIS_NU_SER.NU_SER_ECO, dbo.V_LIAIS_NU_SER.NU_SER_CLIE, dbo.DTM_HIST_MTRE.ID_MTRE, dbo.V_POST_LIST_MTRE.NM_TYPE_MTRE, dbo.V_POST_MTLG_MTRE.DT_VLDT, dbo.DTM_PSG.LB_MOYN
                             FROM dbo.DTM_HIST_MTRE INNER JOIN dbo.DTM_PSG ON dbo.DTM_HIST_MTRE.ID_PSG = dbo.DTM_PSG.ID_PSG INNER JOIN dbo.V_LIAIS_NU_SER ON dbo.DTM_PSG.ID_NU_SER = dbo.V_LIAIS_NU_SER.ID_NU_SER INNER JOIN dbo.V_POST_LIST_MTRE ON dbo.DTM_HIST_MTRE.ID_MTRE = dbo.V_POST_LIST_MTRE.ID_MTRE LEFT OUTER JOIN dbo.V_POST_MTLG_MTRE ON dbo.DTM_HIST_MTRE.ID_MTRE = dbo.V_POST_MTLG_MTRE.ID_MTRE
                            WHERE dbo.V_LIAIS_NU_SER.NU_SER_CLIE = '{sNU_SER}' AND  dbo.DTM_PSG.LB_SCTN = 'P'
                           GROUP BY dbo.V_LIAIS_NU_SER.NU_SER_ECO, dbo.V_LIAIS_NU_SER.NU_SER_CLIE, dbo.DTM_HIST_MTRE.ID_MTRE, dbo.V_POST_LIST_MTRE.NM_TYPE_MTRE, dbo.V_POST_MTLG_MTRE.DT_VLDT, dbo.DTM_PSG.LB_MOYN
                           ORDER BY dbo.DTM_PSG.LB_MOYN"
                Using dt = SQL_SELE_TO_DT(sQuery, CS_MES_Digital_Factory)
                    For Each rdt As DataRow In dt.Rows
                        rdt("ID_MTRE") = Replace(Replace(Replace(LCase(rdt("ID_MTRE").ToString), ".eolane.com", ""), "\\so2k8vm07\", ""), "\\so2k3vm05\", "")
                        If rdt("NM_TYPE_MTRE").ToString = "Poste de travail" Then Continue For
                        para = New Paragraph(New Chunk(rdt("ID_MTRE").ToString, FontFactory.GetFont("CALIBRI", 10)))
                        c_ENTE = New PdfPCell(para)
                        pt.AddCell(c_ENTE)
                        para = New Paragraph(New Chunk(rdt("NM_TYPE_MTRE").ToString, FontFactory.GetFont("CALIBRI", 10)))
                        c_ENTE = New PdfPCell(para)
                        pt.AddCell(c_ENTE)

                        If rdt("NM_TYPE_MTRE").ToString = "Interface de test" Then
                            Using dtZMOYENTYPETEST = SAP_DATA_READ_ZMOYENTYPETEST($"NO_MOYEN EQ '{rdt("ID_MTRE").ToString}'")
                                If dtZMOYENTYPETEST Is Nothing Then Throw New Exception($"Pas de date de validité pour le banc n°{rdt("NM_TYPE_MTRE").ToString} sous SAP.")
                                para = New Paragraph(New Chunk(Replace(COMM_APP_WEB_CONV_FORM_DATE(dtZMOYENTYPETEST(0)("PROCHVERIF").ToString.Insert(6, "/").Insert(4, "/"), "dd/MM/yyyy"), "0001/00/00", ""), FontFactory.GetFont("ARIAL", 10)))
                            End Using
                        Else
                            If rdt("DT_VLDT").ToString = "" Then
                                'rdt("DT_VLDT") = "N/A"
                                Using dtZGMO_OUTIL_VER = SAP_DATA_READ_ZGMO_OUTIL_VER($"ID LIKE '%{rdt("ID_MTRE").ToString}'")
                                    If Not dtZGMO_OUTIL_VER Is Nothing Then
                                        para = New Paragraph(New Chunk(COMM_APP_WEB_CONV_FORM_DATE(dtZGMO_OUTIL_VER(0)("PROCHVERIF1").ToString.Insert(6, "/").Insert(4, "/"), "dd/MM/yyyy"), FontFactory.GetFont("ARIAL", 10)))
                                    Else
                                        para = New Paragraph(New Chunk("N/A", FontFactory.GetFont("ARIAL", 10)))
                                    End If
                                End Using
                            Else
                                para = New Paragraph(New Chunk(Left(rdt("DT_VLDT").ToString, 10), FontFactory.GetFont("ARIAL", 10)))
                            End If
                        End If

                        'para = New Paragraph(New Chunk(Left(rdt("DT_VLDT").ToString, 10), FontFactory.GetFont("CALIBRI", 10)))
                        c_ENTE = New PdfPCell(para)
                        pt.AddCell(c_ENTE)
                        para = New Paragraph(New Chunk(rdt("LB_MOYN").ToString, FontFactory.GetFont("CALIBRI", 10)))
                        c_ENTE = New PdfPCell(para)
                        pt.AddCell(c_ENTE)
                    Next
                End Using
                pt.HeaderRows = 1
                dPDF.Add(pt)

                'par opération
                'extract nomenclature typo
                'trouver pour chaque matériel qui était saisi dans la date
                'extraire le n° de poste physique

                dPDF.NewPage()
                'liste écart prod
                para = New Paragraph(New Chunk(“4. LISTE DES ECARTS DE PRODUCTION”, FontFactory.GetFont("CALIBRI", 14, Font.BOLD Or Font.UNDERLINE)))
                para.SpacingAfter = 20
                dPDF.Add(para)

                'nqi, dérog, hg
                para = New Paragraph(New Chunk(“4.1. NQI, Dérogation, Hors-gamme :”, FontFactory.GetFont("CALIBRI", 12, Font.BOLD Or Font.UNDERLINE)))
                para.SpacingAfter = 20
                dPDF.Add(para)
                pt = New PdfPTable(3)
                pt.LockedWidth = True
                pt.TotalWidth = dPDF.PageSize.Width - 80
                Dim widths_5() As Single = {4, 3, 3}
                pt.SetWidths(widths_5)
                para = New Paragraph(New Chunk("Evénement", FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk("Action réalisée par", FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk("Date", FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                sQuery = $"SELECT dbo.DTM_NC_LIST.NU_DRGT + ' : ' + dbo.DTM_REF_TYPE_NC.NM_DCPO_NC AS NU_DRGT, dbo.DTM_NC_DETA.DT_ITVT, dbo.DTM_NC_DETA.ID_OPRT
                             FROM dbo.DTM_NC_DETA INNER JOIN dbo.DTM_NC_LIST ON dbo.DTM_NC_DETA.NU_INCI = dbo.DTM_NC_LIST.NU_INCI INNER JOIN dbo.DTM_REF_TYPE_NC ON dbo.DTM_NC_LIST.ID_TYPE_NC = dbo.DTM_REF_TYPE_NC.ID_TYPE_NC
                            WHERE (NOT (dbo.DTM_NC_LIST.NU_DRGT IS NULL)) AND (dbo.DTM_NC_LIST.NU_SER_SPFE = N'{sNU_SER}') AND (dbo.DTM_NC_DETA.ID_STAT = 4)"
                Using dt = SQL_SELE_TO_DT(sQuery, CS_ALMS_PROD_PRD)
                    If Not dt Is Nothing Then
                        For Each rdt As DataRow In dt.Rows
                            para = New Paragraph(New Chunk(rdt("NU_DRGT").ToString, FontFactory.GetFont("CALIBRI", 10)))
                            c_ENTE = New PdfPCell(para)
                            pt.AddCell(c_ENTE)
                            Using dtPA0002 = SAP_DATA_READ_TBL("PA0002", "|", "", "PERNR NACHN VORNA", $"PERNR EQ '*{rdt("ID_OPRT").ToString}'")
                                If dtPA0002 Is Nothing Then 'Throw New Exception("pas de nom/prénom trouvés pour le matricule " & rdt("ID_MTCL").ToString)
                                    para = New Paragraph(New Chunk("", FontFactory.GetFont("CALIBRI", 10)))
                                Else
                                    para = New Paragraph(New Chunk(Trim(dtPA0002(0)("NACHN").ToString) & " " & Trim(dtPA0002(0)("VORNA").ToString), FontFactory.GetFont("CALIBRI", 10)))
                                End If
                            End Using
                            c_ENTE = New PdfPCell(para)
                            pt.AddCell(c_ENTE)
                            para = New Paragraph(New Chunk(COMM_APP_WEB_CONV_FORM_DATE(rdt("DT_ITVT").ToString, "dd/MM/yyyy"), FontFactory.GetFont("CALIBRI", 10)))
                            c_ENTE = New PdfPCell(para)
                            pt.AddCell(c_ENTE)
                        Next
                    Else
                        For i = 1 To 9
                            para = New Paragraph(New Chunk("
    ", FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                            c_ENTE = New PdfPCell(para)
                            pt.AddCell(c_ENTE)
                        Next
                    End If
                End Using
                pt.SpacingAfter = 30
                dPDF.Add(pt)

                'autre
                para = New Paragraph(New Chunk(“4.2. Autres écarts :”, FontFactory.GetFont("CALIBRI", 12, Font.BOLD Or Font.UNDERLINE)))
                para.SpacingAfter = 20
                dPDF.Add(para)

                pt = New PdfPTable(5)
                pt.LockedWidth = True
                pt.TotalWidth = dPDF.PageSize.Width - 80
                Dim widths_6() As Single = {4, 3.5, 3, 3, 2}
                pt.SetWidths(widths_6)
                para = New Paragraph(New Chunk("Description", FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk("Action à réaliser", FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                phr = New Phrase
                phr.Add(New Chunk("Action réalisée par ", FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                phr.Add(New Chunk("(nom & date)", FontFactory.GetFont("CALIBRI", 10)))
                c_ENTE = New PdfPCell(phr)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                phr = New Phrase
                phr.Add(New Chunk("Action contrôlée par ", FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                phr.Add(New Chunk("(nom & date)", FontFactory.GetFont("CALIBRI", 10)))
                c_ENTE = New PdfPCell(phr)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                phr = New Phrase
                phr.Add(New Chunk("Résultat ", FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                phr.Add(New Chunk("(OK/NOK)", FontFactory.GetFont("CALIBRI", 10)))
                c_ENTE = New PdfPCell(phr)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                sQuery = $"SELECT [NM_DCPO_ANML],[NM_ACTI_REAL],[ID_OPRT_REAL],[ID_OPRT_CONT],[ID_STAT],[DT_ACTI_CONT],[DT_ACTI_REAL]
                             FROM [dbo].[V_DHR_ECAR_PDTO]
                            WHERE [NU_SER_SPFE] = '{sNU_SER}'"
                Using dt = SQL_SELE_TO_DT(sQuery, CS_ALMS_PROD_PRD)
                    If Not dt Is Nothing Then
                        For Each rdt As DataRow In dt.Rows
                            para = New Paragraph(New Chunk(rdt("NM_DCPO_ANML").ToString, FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                            c_ENTE = New PdfPCell(para)
                            pt.AddCell(c_ENTE)
                            para = New Paragraph(New Chunk(rdt("NM_ACTI_REAL").ToString, FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                            c_ENTE = New PdfPCell(para)
                            pt.AddCell(c_ENTE)
                            Using dtPA0002 = SAP_DATA_READ_TBL("PA0002", "|", "", "PERNR NACHN VORNA", $"PERNR EQ '*{rdt("ID_OPRT_REAL").ToString}'")
                                If dtPA0002 Is Nothing Then 'Throw New Exception("pas de nom/prénom trouvés pour le matricule " & rdt("ID_MTCL").ToString)
                                    para = New Paragraph(New Chunk("", FontFactory.GetFont("CALIBRI", 10)))
                                Else
                                    para = New Paragraph(New Chunk($"{Trim(dtPA0002(0)("NACHN").ToString)} {Trim(dtPA0002(0)("VORNA").ToString)} le {COMM_APP_WEB_CONV_FORM_DATE(rdt("DT_ACTI_REAL").ToString, "dd/MM/yyyy")}", FontFactory.GetFont("CALIBRI", 10)))
                                End If
                            End Using
                            c_ENTE = New PdfPCell(para)
                            pt.AddCell(c_ENTE)
                            Using dtPA0002 = SAP_DATA_READ_TBL("PA0002", "|", "", "PERNR NACHN VORNA", $"PERNR EQ '*{rdt("ID_OPRT_CONT").ToString}'")
                                If dtPA0002 Is Nothing Then 'Throw New Exception("pas de nom/prénom trouvés pour le matricule " & rdt("ID_MTCL").ToString)
                                    para = New Paragraph(New Chunk("", FontFactory.GetFont("CALIBRI", 10)))
                                Else
                                    para = New Paragraph(New Chunk($"{Trim(dtPA0002(0)("NACHN").ToString)} {Trim(dtPA0002(0)("VORNA").ToString)} le {COMM_APP_WEB_CONV_FORM_DATE(rdt("DT_ACTI_CONT").ToString, "dd/MM/yyyy")}", FontFactory.GetFont("CALIBRI", 10)))
                                End If
                            End Using
                            c_ENTE = New PdfPCell(para)
                            pt.AddCell(c_ENTE)
                            para = New Paragraph(New Chunk(rdt("ID_STAT").ToString, FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                            c_ENTE = New PdfPCell(para)
                            pt.AddCell(c_ENTE)
                        Next
                    Else
                        For i = 1 To 15
                            para = New Paragraph(New Chunk("
    ", FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                            c_ENTE = New PdfPCell(para)
                            pt.AddCell(c_ENTE)
                        Next
                    End If
                End Using
                pt.SpacingAfter = 30
                dPDF.Add(pt)

                'liste de documents 
                para = New Paragraph(New Chunk(“5. LISTE DES DOCUMENTS ASSOCIES”, FontFactory.GetFont("CALIBRI", 14, Font.BOLD Or Font.UNDERLINE)))
                para.SpacingAfter = 20
                dPDF.Add(para)

                pt = New PdfPTable(2)
                pt.LockedWidth = True
                pt.TotalWidth = dPDF.PageSize.Width - 160
                Dim widths_7() As Single = {4, 1}
                pt.SetWidths(widths_7)
                para = New Paragraph(New Chunk("Documents du présent rapport", FontFactory.GetFont("CALIBRI", 11, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk("Nb de page", FontFactory.GetFont("CALIBRI", 11, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)

                Dim PdfReader As PdfReader
                Dim sfihcsauv As String = DIG_FACT_SQL_GET_PARA(Trim(sCD_ARTI), "Chemin de sauvegarde du fichier PDF")
                Dim sfichfcgf = $"c:\sources\temp_App_Web\{CInt(Int((10000000 * Rnd()) + 1))}.pdf”
                COMM_APP_WEB_COPY_FILE($"{sfihcsauv}\OF {sOF}\{sNU_SER}_{sdatefcgf}.pdf", sfichfcgf, True)
                PdfReader = New PdfReader(sfichfcgf)
                Dim numberOfPages As Integer = PdfReader.NumberOfPages

                para = New Paragraph(New Chunk($"Annexe 1 - Fiche de contrôle globale de fonctionnement {sNU_SER}_{sdatefcgf}.pdf", FontFactory.GetFont("CALIBRI", 11)))
                c_ENTE = New PdfPCell(para)
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk(numberOfPages.ToString, FontFactory.GetFont("CALIBRI", 11)))
                c_ENTE = New PdfPCell(para)
                pt.AddCell(c_ENTE)

                '???
                sQuery = $"SELECT MAX(CONVERT(NVARCHAR,[Date_Saisie])) AS A,[Rapport_Contrôle] 
                             FROM [SAISIES_DEFAUTS].[dbo].[T_Defauts]
                            WHERE [N°_série] = '{sNU_SER}' AND [OF] = '{sOF}'
                           GROUP BY [OF],[N°_série],[Rapport_Contrôle]"
                Using dt = SQL_SELE_TO_DT(sQuery, CS_MES_Digital_Factory)
                    If dt Is Nothing Then Throw New Exception("Pas de rapport de contrôle trouvé")
                    para = New Paragraph(New Chunk($"Annexe 2 - Rapport de contrôle n°{dt(0)("Rapport_Contrôle").ToString}", FontFactory.GetFont("CALIBRI", 11)))
                End Using
                c_ENTE = New PdfPCell(para)
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk("1", FontFactory.GetFont("CALIBRI", 11)))
                c_ENTE = New PdfPCell(para)
                pt.AddCell(c_ENTE)

                'Dim scheminlistetracabilité As String = Session("listetracabilite")
                PdfReader = New PdfReader(scheminlistetracabilité)
                numberOfPages = PdfReader.NumberOfPages
                para = New Paragraph(New Chunk($"Annexe 3 - Liste traçabilité {sNU_SER} – NM.pdf", FontFactory.GetFont("CALIBRI", 11)))
                c_ENTE = New PdfPCell(para)
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk(numberOfPages.ToString, FontFactory.GetFont("CALIBRI", 11)))
                c_ENTE = New PdfPCell(para)
                pt.AddCell(c_ENTE)
                dPDF.Add(pt)
                dPDF.Close()
            End Using
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "alert")
            Return Nothing
        Finally
            'dPDF.Close()
        End Try
        Return sfich
    End Function

    Public Shared Function DOC_ITEXT_SHARP_CREA_LIST_TCBL(sNU_SER As String, sOF As String, sdes_arti As String, scd_arti As String, dt_deb As String) As String
        Dim sQuery As String = ""
        Dim pt As PdfPTable
        Dim c_ENTE As PdfPCell
        Randomize()
        Dim para As Paragraph
        Dim sfich As String = $"c:\sources\temp_App_Web\{CInt(Int((10000000 * Rnd()) + 1))}.pdf", sCD_ARTI_PROD As String = ""
        Try
            Using dPDF = New Document(PageSize.A4, 20, 20, 30, 20)
                Dim writer = PdfAWriter.GetInstance(dPDF, New FileStream(sfich, FileMode.Create))
                Dim PageEventHandler As New HeaderFooter_LIST_TCBL(sNU_SER)
                writer.PageEvent = PageEventHandler
                dPDF.Open()

                pt = New PdfPTable(2)
                With pt
                    .LockedWidth = True
                    .TotalWidth = dPDF.PageSize.Width - 100
                    Dim widths() As Single = {18, 26}
                    .SetWidths(widths)
                    para = New Paragraph(New Chunk(“Référence du produit : ”, FontFactory.GetFont("CALIBRI", 14, Font.BOLD)))
                    c_ENTE = New PdfPCell(para)
                    c_ENTE.Border = 0
                    c_ENTE.HorizontalAlignment = Element.ALIGN_RIGHT
                    .AddCell(c_ENTE)
                    para = New Paragraph(New Chunk($"{sdes_arti} - {scd_arti}", FontFactory.GetFont("CALIBRI", 14)))
                    c_ENTE = New PdfPCell(para)
                    c_ENTE.Border = 0
                    c_ENTE.BorderWidthBottom = 1
                    .AddCell(c_ENTE)
                    para = New Paragraph(New Chunk(“Référence ALMS : ”, FontFactory.GetFont("CALIBRI", 14, Font.BOLD)))
                    c_ENTE = New PdfPCell(para)
                    c_ENTE.Border = 0
                    c_ENTE.HorizontalAlignment = Element.ALIGN_RIGHT
                    .AddCell(c_ENTE)
                    Using dtKNMT = SAP_DATA_READ_KNMT($"MATNR LIKE '{scd_arti}%' AND KUNNR EQ '0000000451' AND VKORG EQ 'ORC3' AND VTWEG EQ 'CD'")
                        If dtKNMT Is Nothing Then Throw New Exception("Code article client introuvable sous SAP")
                        sCD_ARTI_PROD = Trim(dtKNMT(0)("KDMAT").ToString)
                    End Using
                    para = New Paragraph(New Chunk(sCD_ARTI_PROD, FontFactory.GetFont("CALIBRI", 14)))
                    c_ENTE = New PdfPCell(para)
                    c_ENTE.Border = 0
                    c_ENTE.BorderWidthBottom = 1
                    .AddCell(c_ENTE)
                    para = New Paragraph(New Chunk(“Numéro de série : ”, FontFactory.GetFont("CALIBRI", 14, Font.BOLD)))
                    c_ENTE = New PdfPCell(para)
                    c_ENTE.Border = 0
                    c_ENTE.HorizontalAlignment = Element.ALIGN_RIGHT
                    .AddCell(c_ENTE)
                    para = New Paragraph(New Chunk(sNU_SER, FontFactory.GetFont("CALIBRI", 14)))
                    c_ENTE = New PdfPCell(para)
                    c_ENTE.Border = 0
                    c_ENTE.BorderWidthBottom = 1
                    .AddCell(c_ENTE)
                    .SpacingAfter = 30
                    dPDF.Add(pt)
                End With

                pt = New PdfPTable(3)
                pt.LockedWidth = True
                pt.TotalWidth = dPDF.PageSize.Width - 80
                Dim widths_2() As Single = {1, 3, 1}
                pt.SetWidths(widths_2)
                para = New Paragraph(New Chunk(“Code article”, FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk(“Désignation”, FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)
                para = New Paragraph(New Chunk(“N° de série unitaire ou n° de lot”, FontFactory.GetFont("CALIBRI", 10, Font.BOLD)))
                c_ENTE = New PdfPCell(para)
                c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                c_ENTE.BackgroundColor = New BaseColor(222, 234, 246)
                pt.AddCell(c_ENTE)

                'ajout traça lot
                Using dt_MSEG_CLEAN As New DataTable
                    dt_MSEG_CLEAN.Columns.Add("MATNR", Type.GetType("System.String"))
                    dt_MSEG_CLEAN.Columns.Add("CHARG", Type.GetType("System.String"))
                    dt_MSEG_CLEAN.Columns.Add("MENGE", Type.GetType("System.String"))
                    Using dtMSEG = SAP_DATA_READ_MSEG($"AUFNR LIKE '%{sOF}'")
                        For Each rdtMSEG As DataRow In dtMSEG.Rows
                            Dim rdt_MSEG_CLEAN As DataRow = dt_MSEG_CLEAN.Select($"MATNR LIKE '%{Trim(rdtMSEG("MATNR").ToString)}%' AND CHARG = '{rdtMSEG("CHARG").ToString}'").FirstOrDefault()
                            If rdt_MSEG_CLEAN Is Nothing Then
                                dt_MSEG_CLEAN.Rows.Add()
                                dt_MSEG_CLEAN.Rows(dt_MSEG_CLEAN.Rows.Count - 1)("MATNR") = Trim(rdtMSEG("MATNR").ToString)
                                dt_MSEG_CLEAN.Rows(dt_MSEG_CLEAN.Rows.Count - 1)("CHARG") = rdtMSEG("CHARG").ToString
                                dt_MSEG_CLEAN.Rows(dt_MSEG_CLEAN.Rows.Count - 1)("MENGE") = rdtMSEG("MENGE").ToString
                            Else
                                If rdtMSEG("BWART").ToString = "262" Then
                                    rdt_MSEG_CLEAN("MENGE") = (Convert.ToDecimal(Replace(rdt_MSEG_CLEAN("MENGE").ToString, ".", ",")) - Convert.ToDecimal(Replace(rdtMSEG("MENGE").ToString, ".", ","))).ToString
                                Else
                                    rdt_MSEG_CLEAN("MENGE") = (Convert.ToDecimal(Replace(rdt_MSEG_CLEAN("MENGE").ToString, ".", ",")) + Convert.ToDecimal(Replace(rdtMSEG("MENGE").ToString, ".", ","))).ToString
                                End If
                            End If
                        Next
                    End Using

                    sQuery = $"SELECT dbo.DTM_NMCT_PROD.NU_SER_COMP, dbo.DTM_NMCT_PROD.CD_ARTI_COMP, dbo.DTM_NMCT_PROD.NM_DSGT_COMP
                                 FROM dbo.DTM_NMCT_PROD INNER JOIN (SELECT NU_SER_SPFE, CD_ARTI_COMP, MAX(DT_AFCT) AS MAX_DT_AFCT
                                                                      FROM dbo.DTM_NMCT_PROD AS DTM_NMCT_PROD_1
                                                                    GROUP BY NU_SER_SPFE, CD_ARTI_COMP
                                                                   ) AS dt_DERN_AFCT ON dt_DERN_AFCT.NU_SER_SPFE = dbo.DTM_NMCT_PROD.NU_SER_SPFE AND dt_DERN_AFCT.CD_ARTI_COMP = dbo.DTM_NMCT_PROD.CD_ARTI_COMP AND dt_DERN_AFCT.MAX_DT_AFCT = dbo.DTM_NMCT_PROD.DT_AFCT
                                WHERE (dbo.DTM_NMCT_PROD.NU_SER_SPFE = '{sNU_SER}') AND [BL_TYPE_TCBL] = 1"
                    Using dt1 = SQL_SELE_TO_DT(sQuery, CS_ALMS_PROD_PRD)
                        For Each rdtMSEG As DataRow In dt_MSEG_CLEAN.Select("MENGE <> '0'")
                            Using dtMAKT = SAP_DATA_READ_MAKT($"MATNR LIKE '{rdtMSEG("MATNR").ToString}%'")
                                If dtMAKT Is Nothing Then Throw New Exception("Désignation article introuvable")
                                If Not dt1 Is Nothing Then
                                    If Not dt1.Select($"NM_DSGT_COMP = '{Replace(Trim(dtMAKT(0)("MAKTX").ToString), "'", "''")}'").FirstOrDefault Is Nothing Then Continue For
                                End If
                                If Trim(rdtMSEG("MATNR").ToString) = "TAEKC042100$" Then Continue For
                                Using dtKNMT = SAP_DATA_READ_KNMT($"MATNR LIKE '{Trim(rdtMSEG("MATNR").ToString)}%'")
                                    Dim scd_arti_cmop As String
                                    If dtKNMT Is Nothing Then
                                        scd_arti_cmop = Trim(rdtMSEG("MATNR").ToString)
                                    Else
                                        scd_arti_cmop = Trim(dtKNMT(0)("KDMAT").ToString)
                                    End If
                                    SQL_REQ_ACT($"INSERT INTO [dbo].[DTM_NMCT_PROD] ([NU_SER_SPFE],[CD_ARTI_PROD],[NU_SER_COMP],[CD_ARTI_COMP],[BL_TYPE_TCBL],[DT_AFCT],[NM_DSGT_COMP])
                                                       VALUES ('{sNU_SER}','{sCD_ARTI_PROD}','{Trim(rdtMSEG("CHARG").ToString)}','{scd_arti_cmop}',0,'{dt_deb}','{Replace(Trim(dtMAKT(0)("MAKTX").ToString), "'", "''")}')", CS_ALMS_PROD_PRD)
                                End Using
                            End Using
                        Next
                    End Using
                    'ajout traça unitaire
                    sQuery = $"SELECT dbo.DTM_NMCT_PROD.NU_SER_COMP, dbo.DTM_NMCT_PROD.CD_ARTI_COMP, dbo.DTM_NMCT_PROD.NM_DSGT_COMP
                                 FROM dbo.DTM_NMCT_PROD INNER JOIN (SELECT NU_SER_SPFE, CD_ARTI_COMP, MAX(DT_AFCT) AS MAX_DT_AFCT
                                                                      FROM dbo.DTM_NMCT_PROD AS DTM_NMCT_PROD_1
                                                                    GROUP BY NU_SER_SPFE, CD_ARTI_COMP) AS dt_DERN_AFCT ON dt_DERN_AFCT.NU_SER_SPFE = dbo.DTM_NMCT_PROD.NU_SER_SPFE AND dt_DERN_AFCT.CD_ARTI_COMP = dbo.DTM_NMCT_PROD.CD_ARTI_COMP AND dt_DERN_AFCT.MAX_DT_AFCT = dbo.DTM_NMCT_PROD.DT_AFCT
                                WHERE (dbo.DTM_NMCT_PROD.NU_SER_SPFE = '{sNU_SER}' AND dbo.DTM_NMCT_PROD.NU_SER_COMP <> '')
                               ORDER BY [BL_TYPE_TCBL] DESC, [CD_ARTI_COMP], NU_SER_COMP"
                    Using dt = SQL_SELE_TO_DT(sQuery, CS_ALMS_PROD_PRD)
                        For Each rdt As DataRow In dt.Rows
                            para = New Paragraph(New Chunk(rdt("CD_ARTI_COMP").ToString, FontFactory.GetFont("CALIBRI", 11)))
                            c_ENTE = New PdfPCell(para)
                            pt.AddCell(c_ENTE)
                            para = New Paragraph(New Chunk(rdt("NM_DSGT_COMP").ToString, FontFactory.GetFont("CALIBRI", 11)))
                            c_ENTE = New PdfPCell(para)
                            pt.AddCell(c_ENTE)
                            para = New Paragraph(New Chunk(rdt("NU_SER_COMP").ToString, FontFactory.GetFont("CALIBRI", 11)))
                            c_ENTE = New PdfPCell(para)
                            pt.AddCell(c_ENTE)
                        Next
                    End Using
                    pt.HeaderRows = 1
                    pt.SpacingAfter = 30
                    dPDF.Add(pt)
                    dPDF.Close()
                End Using
            End Using
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "alert")
            Return Nothing
        Finally
            'dPDF.Close() 'Ventilateur Vendome ST40 IND01X 08/12/16
        End Try
        Return sfich
    End Function
    Public Shared Function DOC_ITEXT_SHARP_LIST_CLSG_SENS_LABS(dt_list_clsg As DataTable, scd_arti As String) As String
        Dim pt As PdfPTable
        Dim c_ENTE As PdfPCell
        Randomize()
        Dim sfich As String = $"c:\sources\temp_App_Web\{CInt(Int((10000000 * Rnd()) + 1))}.pdf”
        Dim sgif_bc As String = ""
        Dim para As Paragraph
        ' Dim ipagenumber As Integer
        Try
            Using dPDF As New Document(PageSize.A4, 10, 10, 10, 10)
                Using dtKNMT = SAP_DATA_READ_KNMT($"MATNR LIKE '{scd_arti}%' AND KUNNR EQ '0000000451' AND VKORG EQ 'ORC3' AND VTWEG EQ 'CD'")
                    Dim scd_arti_clie As String = ""
                    If dtKNMT Is Nothing Then
                        scd_arti_clie = DIG_FACT_SQL_GET_PARA(scd_arti, "Code article client")
                        If scd_arti_clie Is Nothing Then Throw New Exception("Code article client introuvable sous SAP")
                    Else
                        scd_arti_clie = Trim(dtKNMT(0)("KDMAT").ToString)
                    End If
                    Dim writer = PdfAWriter.GetInstance(dPDF, New FileStream(sfich, FileMode.Create))
                    Dim PageEventHandler As New HeaderFooter_LIST_CLSG_SENS_LABS(scd_arti_clie, dt_list_clsg.Rows.Count)
                    'Dim PageEventHandler As New HeaderFooter_LIST_CLSG_SENS_LABS(scd_arti, dt_list_clsg.Rows.Count)

                    writer.PageEvent = PageEventHandler
                End Using
                dPDF.Open()

                'entête
                pt = New PdfPTable(5)
                pt.TotalWidth = dPDF.PageSize.Width - 5
                pt.LockedWidth = True
                pt.DefaultCell.Border = Rectangle.NO_BORDER
                Document.Compress = True
                Dim ijump50 = 0
                For Each rdt_list_clsg In dt_list_clsg.Rows
                    If dt_list_clsg.Rows.Count <= 50 Then
                        If ijump50 = 5 Then
                            For i = 1 To 5
                                para = New Paragraph(" ", FontFactory.GetFont("Calibri", 20))
                                para.SpacingAfter = 100
                                c_ENTE = New PdfPCell(para)
                                c_ENTE.Border = 0
                                pt.AddCell(c_ENTE)
                            Next
                            ijump50 = 1
                        Else
                            ijump50 += 1
                        End If
                    End If
                    Dim pt_cell As New PdfPTable(1)
                    Randomize()
                    sgif_bc = $"c:\sources\temp_App_Web\{CInt(Int((10000000 * Rnd()) + 1))}.gif”

                    Dim bc128 As New Barcode128()
                    bc128.CodeType = Barcode.CODE128
                    bc128.Code = rdt_list_clsg("DEV_EUI").ToString
                    Dim bc As System.Drawing.Image = bc128.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White)
                    bc.Save(sgif_bc, System.Drawing.Imaging.ImageFormat.Gif)
                    Dim iCAB = Image.GetInstance(sgif_bc)
                    'iCAB.SpacingAfter = 5
                    iCAB.Border = 0
                    iCAB.ScalePercent(50)
                    iCAB.SetAbsolutePosition(0, 0)
                    c_ENTE = New PdfPCell(iCAB)
                    c_ENTE.Border = 0
                    c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                    c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                    'c_ENTE.FixedHeight = 30
                    pt_cell.AddCell(c_ENTE)
                    para = New Paragraph(New Chunk(rdt_list_clsg("DEV_EUI").ToString, FontFactory.GetFont("Calibri", 8)))
                    c_ENTE = New PdfPCell(para)
                    c_ENTE.Border = 0
                    c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
                    c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
                    pt_cell.AddCell(c_ENTE)
                    pt_cell.SpacingAfter = 5
                    pt.AddCell(pt_cell)
                Next

                dPDF.Add(pt)
                dPDF.Close()
            End Using
        Catch ex As Exception
            LOG_MESS_UTLS(GetCurrentMethod, ex.Message, "alert")
            Return Nothing
        End Try
        Return sfich
    End Function


End Class

Public Class HeaderFooter_FCGF
    Inherits PdfPageEventHelper

    ' C'est un conteneur de texte
    Private cb As PdfContentByte

    ' we will put the final number of pages in a template
    Private template As PdfTemplate

    ' this is the BaseFont we are going to use for the header / footer
    Private bf As BaseFont = Nothing
    Private sNU_SER, sFDV, sVers_FDV, sDSGT_ARTI As String

    Sub New(ByVal sNU_SER_new As String, ByVal sFDV_new As String, ByVal sVers_FDV_new As String, ByVal sDSGT_ARTI_new As String)
        sNU_SER = sNU_SER_new
        sFDV = sFDV_new
        sVers_FDV = sVers_FDV_new
        sDSGT_ARTI = sDSGT_ARTI_new
    End Sub
    ' we override the onOpenDocument method
    Public Overrides Sub OnOpenDocument(ByVal writer As PdfWriter, ByVal document As Document)
        Try
            bf = pdf.BaseFont.CreateFont(pdf.BaseFont.HELVETICA, pdf.BaseFont.CP1252, pdf.BaseFont.NOT_EMBEDDED)
            cb = writer.DirectContent
            ' Création de la plage qui recevra le nbre total de pages du document (vu dans le pied de page).
            template = cb.CreateTemplate(50, 50)
        Catch de As DocumentException
        Catch ioe As System.IO.IOException
        End Try
    End Sub

    Public Overrides Sub OnStartPage(ByVal writer As PdfWriter, ByVal document As Document)
        MyBase.OnStartPage(writer, document)

        Dim pt_ENTE = New PdfPTable(3)
        pt_ENTE.TotalWidth = document.PageSize.Width - 40
        pt_ENTE.LockedWidth = True
        Dim widths() As Single = {10, 13, 10}
        pt_ENTE.SetWidths(widths)

        Dim pt = New PdfPTable(1)
        pt.DefaultCell.Border = Rectangle.NO_BORDER
        Dim iLOGO = Image.GetInstance("C:\sources\Digital Factory\Etiquettes\ALM\air-liquide-healthcare-logo.jpg")
        pt.AddCell(iLOGO)
        Dim para = New Paragraph(New Chunk(“Direction Industrielle”, FontFactory.GetFont("ARIAL", 12, Font.BOLD)))
        Dim c_ENTE = New PdfPCell(para)
        c_ENTE.Border = 0
        c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
        pt.AddCell(c_ENTE)
        pt_ENTE.AddCell(pt)

        para = New Paragraph(New Chunk(“FICHE DE CONTROLE GLOBAL DE FONCTIONNEMENT”, FontFactory.GetFont("ARIAL", 16, Font.BOLD)))
        c_ENTE = New PdfPCell(para)
        c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
        c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
        pt_ENTE.AddCell(c_ENTE)

        pt = New PdfPTable(2)

        'Dim sFDV As String = System.Web.HttpContext.Current.Session("sFDV")
        'Dim sVers_FDV As String = System.Web.HttpContext.Current.Session("sVers_FDV")
        'Dim sDSGT_ARTI As String = System.Web.HttpContext.Current.Session("sDSGT_ARTI")

        para = New Paragraph(New Chunk(“Référence”, FontFactory.GetFont("ARIAL", 10)))
        c_ENTE = New PdfPCell(para)
        c_ENTE.Border = 0
        pt.AddCell(c_ENTE)
        para = New Paragraph(New Chunk(sFDV, FontFactory.GetFont("ARIAL", 10, Font.BOLD))) 'FDV
        c_ENTE = New PdfPCell(para)
        c_ENTE.Border = 0
        pt.AddCell(c_ENTE)
        para = New Paragraph(New Chunk("Révision", FontFactory.GetFont("ARIAL", 10)))
        c_ENTE = New PdfPCell(para)
        c_ENTE.Border = 0
        pt.AddCell(c_ENTE)
        para = New Paragraph(New Chunk(sVers_FDV, FontFactory.GetFont("ARIAL", 10, Font.BOLD))) 'Version FDV
        c_ENTE = New PdfPCell(para)
        c_ENTE.Border = 0
        pt.AddCell(c_ENTE)
        para = New Paragraph(New Chunk("Propriétaire", FontFactory.GetFont("ARIAL", 10)))
        c_ENTE = New PdfPCell(para)
        c_ENTE.Border = 0
        pt.AddCell(c_ENTE)
        para = New Paragraph(New Chunk("ALMS - M&I", FontFactory.GetFont("ARIAL", 10, Font.BOLD)))
        c_ENTE = New PdfPCell(para)
        c_ENTE.Border = 0
        pt.AddCell(c_ENTE)

        pt_ENTE.AddCell(pt)

        para = New Paragraph(New Chunk(sDSGT_ARTI, FontFactory.GetFont("ARIAL", 18, Font.BOLD)))
        c_ENTE = New PdfPCell(para)
        c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
        c_ENTE.Colspan = 3
        pt_ENTE.AddCell(c_ENTE)
        pt_ENTE.SpacingAfter = 10

        document.Add(pt_ENTE)

    End Sub


    Private moBF As BaseFont = pdf.BaseFont.CreateFont(pdf.BaseFont.HELVETICA, pdf.BaseFont.CP1252, pdf.BaseFont.NOT_EMBEDDED)

    Public Overrides Sub OnEndPage(ByVal writer As PdfWriter, ByVal document As Document)
        MyBase.OnEndPage(writer, document)

        '*** PIED DE PAGE *************
        'Affiche le N° de page 
        Dim oTable As New PdfPTable(1)
        ' Dim coprt As TCBL_OPRT
        With oTable
            Dim iPageNumber As Integer = writer.PageNumber
            Dim sText As String = $"Page {iPageNumber.ToString} sur "

            oTable.TotalWidth = document.PageSize.Width - 70
            oTable.WriteSelectedRows(0, -1, 36, 15, writer.DirectContent)

            Dim fLen As Single = moBF.GetWidthPoint(sText, 10)
            cb.BeginText()
            cb.SetFontAndSize(moBF, 8)
            cb.SetTextMatrix(40, 16)
            '
            'Dim sNU_SER As String = System.Web.HttpContext.Current.Session("sNU_SER")
            cb.ShowText($"SN Produit : {sNU_SER}")
            cb.SetTextMatrix(document.PageSize.Width / 2, 16)
            cb.ShowText("FCGF_V1.1")
            cb.SetTextMatrix(document.PageSize.Width - 90, 16)
            cb.ShowText(sText)
            cb.EndText()
            cb.AddTemplate(template, document.PageSize.Width - 100 + fLen, 16)

            'Dim pt As New PdfPTable(4)
            'Dim para = New Paragraph(New Chunk("SN Produit : ", FontFactory.GetFont("ARIAL", 10)))
            'Dim c_ENTE = New PdfPCell(para)
            'pt.AddCell(c_ENTE)

            'para = New Paragraph(New Chunk("VE40-000114", FontFactory.GetFont("ARIAL", 10, Font.BOLD)))
            'c_ENTE = New PdfPCell(para)
            'pt.AddCell(c_ENTE)

            'para = New Paragraph(New Chunk("FCGF_V1", FontFactory.GetFont("ARIAL", 10)))
            'c_ENTE = New PdfPCell(para)
            'pt.AddCell(c_ENTE)

            'para = New Paragraph(New Chunk("coucou", FontFactory.GetFont("ARIAL", 10)))
            'c_ENTE = New PdfPCell(para)
            'pt.AddCell(c_ENTE)

            'document.Add(pt)
        End With

    End Sub

    ' Récupère le total des pages du PDF
    Public Overrides Sub OnCloseDocument(ByVal writer As PdfWriter, ByVal document As Document)
        MyBase.OnCloseDocument(writer, document)
        template.BeginText()
        template.SetFontAndSize(bf, 8)
        template.SetTextMatrix(0, 0)
        template.ShowText("" & Convert.ToString((writer.PageNumber)))
        template.EndText()
    End Sub

End Class

Public Class HeaderFooter_DHR
    Inherits PdfPageEventHelper

    ' C'est un conteneur de texte
    Private cb As PdfContentByte

    ' we will put the final number of pages in a template
    Private template As PdfTemplate

    ' this is the BaseFont we are going to use for the header / footer
    Private bf As BaseFont = Nothing

    ' we override the onOpenDocument method
    Public Overrides Sub OnOpenDocument(ByVal writer As PdfWriter, ByVal document As Document)
        Try
            bf = pdf.BaseFont.CreateFont(pdf.BaseFont.HELVETICA, pdf.BaseFont.CP1252, pdf.BaseFont.NOT_EMBEDDED)
            cb = writer.DirectContent
            ' Création de la plage qui recevra le nbre total de pages du document (vu dans le pied de page).
            template = cb.CreateTemplate(50, 50)
        Catch de As DocumentException
        Catch ioe As System.IO.IOException
        End Try
    End Sub

    Public Overrides Sub OnStartPage(ByVal writer As PdfWriter, ByVal document As Document)
        MyBase.OnStartPage(writer, document)
        Dim c_ENTE As PdfPCell
        Dim pt_ENTE = New PdfPTable(3)
        pt_ENTE.TotalWidth = document.PageSize.Width - 40
        pt_ENTE.LockedWidth = True
        Dim widths() As Single = {5, 20, 5}
        pt_ENTE.SetWidths(widths)

        Dim pt = New PdfPTable(1)
        pt.DefaultCell.Border = Rectangle.NO_BORDER

        Dim iLOGO = Image.GetInstance("C:\sources\Digital Factory\Etiquettes\Combrée-1.jpg")

        pt.AddCell(iLOGO)
        Dim para = New Paragraph(New Chunk(“I 369 - B”, FontFactory.GetFont("CALIBRI", 11, Font.BOLD)))
        c_ENTE = New PdfPCell(para)
        c_ENTE.Border = 0
        c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
        pt.AddCell(c_ENTE)
        pt_ENTE.AddCell(pt)

        para = New Paragraph(New Chunk(“Synthèse de Device History Record”, FontFactory.GetFont("CALIBRI", 20, Font.BOLD)))
        c_ENTE = New PdfPCell(para)
        c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
        c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
        pt_ENTE.AddCell(c_ENTE)
        pt_ENTE.WidthPercentage = 100

        para = New Paragraph(New Chunk(“A archiver par le contrôle final par OF”, FontFactory.GetFont("CALIBRI", 11, Font.BOLD)))
        c_ENTE = New PdfPCell(para)
        c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
        c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
        pt_ENTE.AddCell(c_ENTE)
        pt_ENTE.SpacingAfter = 30
        document.Add(pt_ENTE)

    End Sub


    Private moBF As BaseFont = pdf.BaseFont.CreateFont(pdf.BaseFont.HELVETICA, pdf.BaseFont.CP1252, pdf.BaseFont.NOT_EMBEDDED)

    Public Overrides Sub OnEndPage(ByVal writer As PdfWriter, ByVal document As Document)
        MyBase.OnEndPage(writer, document)

        '*** PIED DE PAGE *************
        'Affiche le N° de page 
        Dim oTable As New PdfPTable(1)
        With oTable
            Dim iPageNumber As Integer = writer.PageNumber
            Dim sText As String = $"Page {iPageNumber} sur "

            oTable.TotalWidth = document.PageSize.Width - 70
            oTable.WriteSelectedRows(0, -1, 36, 15, writer.DirectContent)

            Dim fLen As Single = moBF.GetWidthPoint(sText, 10)
            cb.BeginText()
            cb.SetFontAndSize(moBF, 8)
            cb.SetTextMatrix(document.PageSize.Width - 90, 16)
            cb.ShowText(sText)
            cb.EndText()
            cb.AddTemplate(template, document.PageSize.Width - 100 + fLen, 16)
        End With
    End Sub

    ' Récupère le total des pages du PDF
    Public Overrides Sub OnCloseDocument(ByVal writer As PdfWriter, ByVal document As Document)
        MyBase.OnCloseDocument(writer, document)
        template.BeginText()
        template.SetFontAndSize(bf, 8)
        template.SetTextMatrix(0, 0)
        template.ShowText("" & Convert.ToString((writer.PageNumber)))
        template.EndText()
    End Sub

End Class

Public Class HeaderFooter_LIST_TCBL
    Inherits PdfPageEventHelper

    ' C'est un conteneur de texte
    Private cb As PdfContentByte

    ' we will put the final number of pages in a template
    Private template As PdfTemplate

    ' this is the BaseFont we are going to use for the header / footer
    Private bf As BaseFont = Nothing
    Private sNU_SER As String

    Sub New(ByVal sNU_SER_new As String)
        sNU_SER = sNU_SER_new
    End Sub
    ' we override the onOpenDocument method
    Public Overrides Sub OnOpenDocument(ByVal writer As PdfWriter, ByVal document As Document)
        Try
            bf = pdf.BaseFont.CreateFont(pdf.BaseFont.HELVETICA, pdf.BaseFont.CP1252, pdf.BaseFont.NOT_EMBEDDED)
            cb = writer.DirectContent
            ' Création de la plage qui recevra le nbre total de pages du document (vu dans le pied de page).
            template = cb.CreateTemplate(50, 50)
        Catch de As DocumentException
        Catch ioe As System.IO.IOException
        End Try
    End Sub

    Public Overrides Sub OnStartPage(ByVal writer As PdfWriter, ByVal document As Document)
        MyBase.OnStartPage(writer, document)

        Dim pt_ENTE = New PdfPTable(1)
        pt_ENTE.TotalWidth = document.PageSize.Width - 40
        'Dim sNU_SER As String = System.Web.HttpContext.Current.Session("sNU_SER")
        Dim para = New Paragraph(New Chunk($"Liste traçabilité {sNU_SER} – NM", FontFactory.GetFont("CALIBRI", 16, Font.BOLD)))
        Dim c_ENTE = New PdfPCell(para)
        c_ENTE.Border = 0
        c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
        pt_ENTE.AddCell(c_ENTE)
        pt_ENTE.SpacingAfter = 30
        document.Add(pt_ENTE)

    End Sub


    Private moBF As BaseFont = pdf.BaseFont.CreateFont(pdf.BaseFont.HELVETICA, pdf.BaseFont.CP1252, pdf.BaseFont.NOT_EMBEDDED)

    Public Overrides Sub OnEndPage(ByVal writer As PdfWriter, ByVal document As Document)
        MyBase.OnEndPage(writer, document)

        '*** PIED DE PAGE *************
        'Affiche le N° de page 
        Dim oTable As New PdfPTable(1)
        With oTable
            Dim iPageNumber As Integer = writer.PageNumber
            Dim sText As String = $"Page {iPageNumber} sur "

            oTable.TotalWidth = document.PageSize.Width - 70
            oTable.WriteSelectedRows(0, -1, 36, 15, writer.DirectContent)

            Dim fLen As Single = moBF.GetWidthPoint(sText, 10)
            cb.BeginText()
            cb.SetFontAndSize(moBF, 8)
            cb.SetTextMatrix(document.PageSize.Width - 90, 16)
            cb.ShowText(sText)
            cb.EndText()
            cb.AddTemplate(template, document.PageSize.Width - 100 + fLen, 16)
        End With
    End Sub

    ' Récupère le total des pages du PDF
    Public Overrides Sub OnCloseDocument(ByVal writer As PdfWriter, ByVal document As Document)
        MyBase.OnCloseDocument(writer, document)
        template.BeginText()
        template.SetFontAndSize(bf, 8)
        template.SetTextMatrix(0, 0)
        template.ShowText("" & Convert.ToString((writer.PageNumber)))
        template.EndText()
    End Sub

End Class

Public Class HeaderFooter_LIST_CLSG_SENS_LABS
    Inherits PdfPageEventHelper

    ' C'est un conteneur de texte
    Private cb As PdfContentByte

    ' we will put the final number of pages in a template
    Private template As PdfTemplate

    ' this is the BaseFont we are going to use for the header / footer
    Private bf As BaseFont = Nothing

    Private sPROD As String, iQT As Integer

    Sub New(ByVal sproduit As String, ByVal iquantite As Integer)
        sPROD = sproduit
        iQT = iquantite
    End Sub
    ' we override the onOpenDocument method
    Public Overrides Sub OnOpenDocument(ByVal writer As PdfWriter, ByVal document As Document)
        Try
            bf = pdf.BaseFont.CreateFont(pdf.BaseFont.HELVETICA, pdf.BaseFont.CP1252, pdf.BaseFont.NOT_EMBEDDED)
            cb = writer.DirectContent
            ' Création de la plage qui recevra le nbre total de pages du document (vu dans le pied de page).
            template = cb.CreateTemplate(50, 50)
        Catch de As DocumentException
        Catch ioe As System.IO.IOException
        End Try
    End Sub

    Public Overrides Sub OnStartPage(ByVal writer As PdfWriter, ByVal document As Document)
        MyBase.OnStartPage(writer, document)
        Dim c_ENTE As PdfPCell
        Dim pt_ENTE = New PdfPTable(3)
        pt_ENTE.DefaultCell.Border = Rectangle.NO_BORDER
        pt_ENTE.TotalWidth = document.PageSize.Width - 40
        pt_ENTE.LockedWidth = True
        Dim widths() As Single = {5, 20, 5}
        pt_ENTE.SetWidths(widths)

        Dim iLOGO = Image.GetInstance("C:\sources\Digital Factory\Etiquettes\Sensing Labs\Logo-SL-HD-transparent.png")
        pt_ENTE.AddCell(iLOGO)

        Dim para = New Paragraph(New Chunk(sPROD, FontFactory.GetFont("CALIBRI", 20, Font.BOLD)))
        c_ENTE = New PdfPCell(para)
        c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
        c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
        c_ENTE.Border = 0
        pt_ENTE.AddCell(c_ENTE)
        pt_ENTE.WidthPercentage = 100

        para = New Paragraph(New Chunk($“Qté {iQT.ToString}”, FontFactory.GetFont("CALIBRI", 20)))
        c_ENTE = New PdfPCell(para)
        c_ENTE.HorizontalAlignment = Element.ALIGN_CENTER
        c_ENTE.VerticalAlignment = Element.ALIGN_MIDDLE
        c_ENTE.Border = 0
        pt_ENTE.AddCell(c_ENTE)
        pt_ENTE.SpacingAfter = 20
        document.Add(pt_ENTE)

    End Sub


    Private moBF As BaseFont = pdf.BaseFont.CreateFont(pdf.BaseFont.HELVETICA, pdf.BaseFont.CP1252, pdf.BaseFont.NOT_EMBEDDED)

    Public Overrides Sub OnEndPage(ByVal writer As PdfWriter, ByVal document As Document)
        MyBase.OnEndPage(writer, document)

        '*** PIED DE PAGE *************
        'Affiche le N° de page 
        Dim oTable As New PdfPTable(1)
        With oTable
            Dim iPageNumber As Integer = writer.PageNumber
            Dim sText As String = $"Page {iPageNumber} sur "

            oTable.TotalWidth = document.PageSize.Width - 70
            oTable.WriteSelectedRows(0, -1, 36, 15, writer.DirectContent)

            Dim fLen As Single = moBF.GetWidthPoint(sText, 10)
            cb.BeginText()
            cb.SetFontAndSize(moBF, 8)
            cb.SetTextMatrix(document.PageSize.Width - 90, 16)
            cb.ShowText(sText)
            cb.EndText()
            cb.AddTemplate(template, document.PageSize.Width - 100 + fLen, 16)
        End With
    End Sub

    ' Récupère le total des pages du PDF
    Public Overrides Sub OnCloseDocument(ByVal writer As PdfWriter, ByVal document As Document)
        MyBase.OnCloseDocument(writer, document)
        template.BeginText()
        template.SetFontAndSize(bf, 8)
        template.SetTextMatrix(0, 0)
        template.ShowText("" & Convert.ToString((writer.PageNumber)))
        template.EndText()
    End Sub

End Class