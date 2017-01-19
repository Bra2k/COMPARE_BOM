Imports PdfSharp
Imports PdfSharp.Drawing
Imports PdfSharp.Drawing.Layout
Imports PdfSharp.Pdf
Imports System.IO
Imports App_Web.LOG
Imports App_Web.Class_COMM_APP_WEB
Imports System.Reflection.MethodBase
Imports System
Imports BarcodeLib.Barcode

Public Class Class_PDF

    Public Shared Sub PDF_CREA_TEXT(ByRef gfx As XGraphics, iLeft As Integer, iTop As Integer, iWidth As Integer, iHeight As Integer, sFont As String, iSize As Integer, sString As String)
        Dim font As New XFont(sFont, iSize, XFontStyle.Regular)
        Dim tf As New XTextFormatter(gfx)
        Dim rect As New XRect(iLeft, iTop, iWidth, iHeight)

        Try
            gfx.DrawRectangle(XBrushes.Transparent, rect)
            tf.DrawString(sString, font, XBrushes.Black, rect, XStringFormats.TopLeft)
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        End Try

    End Sub

    Public Shared Sub PDF_CHAR_CONF(ByRef gfx As XGraphics, sfich_config As String, dt_var_pdf_replace As DataTable)
        Dim sr As StreamReader, sData As String, sloctext As String(), sText As String = ""
        Try
            sr = New StreamReader(sfich_config, System.Text.Encoding.UTF8)
            Do While sr.Peek() >= 0
                sData = sr.ReadLine
                Select Case Left(sData, 1)
                    Case "t", "T" 'text
                        sloctext = Split(COMM_APP_WEB_STRI_TRIM_LEFT(sData, sData.IndexOf(";")), ",")
                        sText = COMM_APP_WEB_STRI_TRIM_LEFT(sloctext(5), sloctext(5).IndexOf(";"))
                        For Each rdt_var_replace As DataRow In dt_var_pdf_replace.Rows
                            sText = Replace(sText, rdt_var_replace("Variable").ToString, rdt_var_replace("Valeur").ToString)
                        Next
                        PDF_CREA_TEXT(gfx,
                                      Convert.ToDecimal(Replace(sloctext(0), ".", ",")),
                                      Convert.ToDecimal(Replace(sloctext(1), ".", ",")),
                                      Convert.ToDecimal(Replace(sloctext(2), ".", ",")),
                                      Convert.ToDecimal(Replace(sloctext(3), ".", ",")),
                                      sloctext(4),
                                      Convert.ToDecimal(Replace(Left(sloctext(5), sloctext(5).IndexOf(";")), "pt", "")),
                                      sText)
                    Case "i", "I" 'image
                    Case "g", "G" 'graphic
                    Case "b", "B" 'code à barre (datamatrix)
                        sloctext = Split(COMM_APP_WEB_STRI_TRIM_LEFT(sData, sData.IndexOf(";")), ",")
                        sText = COMM_APP_WEB_STRI_TRIM_LEFT(sloctext(5), sloctext(5).IndexOf(";"))
                        For Each rdt_var_replace As DataRow In dt_var_pdf_replace.Rows
                            sText = Replace(sText, rdt_var_replace("Variable").ToString, rdt_var_replace("Valeur").ToString)
                        Next
                        Dim spng As String = My.Settings.RPTR_TPRR & "\" & CInt(Int((1000 * Rnd()) + 1)) & ".bmp"
                        Select Case sloctext(4)
                            Case "DATAMATRIX"
                                'Dim spng As String = My.Settings.RPTR_TPRR & "\" & CInt(Int((1000 * Rnd()) + 1)) & ".bmp"

                                Dim barcode As DataMatrix = New DataMatrix()
                                With barcode
                                    .Data = sText
                                    .UOM = UnitOfMeasure.PIXEL
                                    .ModuleSize = 3
                                    .LeftMargin = 0
                                    .RightMargin = 0
                                    .TopMargin = 0
                                    .BottomMargin = 0
                                    .Encoding = BarcodeLib.Barcode.DataMatrixEncoding.ASCII
                                    .Format = BarcodeLib.Barcode.DataMatrixFormat.Format_10X10
                                    .ImageFormat = System.Drawing.Imaging.ImageFormat.Bmp
                                    .drawBarcode(spng)
                                End With
                                'Dim myImage = XImage.FromFile(spng)
                                ''Dim myXPoint = New XPoint(Convert.ToDecimal(Replace(sloctext(0), ".", ",")), Convert.ToDecimal(Replace(sloctext(1), ".", ",")))

                                'Dim rect As New XRect(Convert.ToDecimal(Replace(sloctext(0), ".", ",")),
                                '              Convert.ToDecimal(Replace(sloctext(1), ".", ",")),
                                '              Convert.ToDecimal(Replace(sloctext(2), ".", ",")),
                                '              Convert.ToDecimal(Replace(sloctext(3), ".", ",")))
                                'gfx.DrawRectangle(XBrushes.Black, rect)
                                'gfx.DrawImage(XImage.FromFile(spng), rect)
                            Case "Code3of9Standard"
                                Dim myXSize = New XSize(Convert.ToDecimal(Replace(sloctext(2), ".", ",")), Convert.ToDecimal(Replace(sloctext(3), ".", ",")))
                                Dim myNewCode = New PdfSharp.Drawing.BarCodes.Code3of9Standard(sText, myXSize)
                                Dim myXPoint = New XPoint(Convert.ToDecimal(Replace(sloctext(0), ".", ",")), Convert.ToDecimal(Replace(sloctext(1), ".", ",")))
                                gfx.DrawBarCode(myNewCode, myXPoint)
                            Case "Code2of5Interleaved"
                                Dim myXSize = New XSize(Convert.ToDecimal(Replace(sloctext(2), ".", ",")), Convert.ToDecimal(Replace(sloctext(3), ".", ",")))
                                Dim myNewCode = New PdfSharp.Drawing.BarCodes.Code2of5Interleaved(COMM_APP_WEB_ENCO_2OF5_ITLV(sText, True))
                                Dim myXPoint = New XPoint(Convert.ToDecimal(Replace(sloctext(0), ".", ",")), Convert.ToDecimal(Replace(sloctext(1), ".", ",")))
                                gfx.DrawBarCode(myNewCode, myXPoint)
                            Case "Code128"

                                Dim barcode As Linear = New Linear()
                                With barcode
                                    .Type = BarcodeType.CODE128
                                    .Data = sText
                                    .UOM = UnitOfMeasure.PIXEL
                                    .BarWidth = 1
                                    .BarHeight = Convert.ToDecimal(Replace(sloctext(3), ".", ","))
                                    .LeftMargin = 0
                                    .RightMargin = 0
                                    .TopMargin = 0
                                    .BottomMargin = 0
                                    .ImageFormat = System.Drawing.Imaging.ImageFormat.Bmp
                                    .drawBarcode(spng)
                                End With
                        End Select
                        Dim myImage = XImage.FromFile(spng)
                        'Dim myXPoint = New XPoint(Convert.ToDecimal(Replace(sloctext(0), ".", ",")), Convert.ToDecimal(Replace(sloctext(1), ".", ",")))

                        Dim rect As New XRect(Convert.ToDecimal(Replace(sloctext(0), ".", ",")),
                                      Convert.ToDecimal(Replace(sloctext(1), ".", ",")),
                                      Convert.ToDecimal(Replace(sloctext(2), ".", ",")),
                                      Convert.ToDecimal(Replace(sloctext(3), ".", ",")))

                        gfx.DrawRectangle(XBrushes.Black, rect)
                        gfx.DrawImage(XImage.FromFile(spng), rect)

                End Select
            Loop
            'dt_var_pdf_replace.Clear()
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        Finally
            sr.Close()
        End Try

    End Sub

    Public Shared Function PDF_REPL_VAR(dt_var_pdf_replace As DataTable, sA_Replacer As String, sDe_Remplacement As String) As DataTable
        Dim row As DataRow
        Try
            If Not dt_var_pdf_replace.Columns.Contains("Variable") Then dt_var_pdf_replace.Columns.Add("Variable", Type.GetType("System.String"))
            If Not dt_var_pdf_replace.Columns.Contains("Valeur") Then dt_var_pdf_replace.Columns.Add("Valeur", Type.GetType("System.String"))
            dt_var_pdf_replace.NewRow()
            row = dt_var_pdf_replace.NewRow()
            row("Variable") = sA_Replacer
            row("Valeur") = sDe_Remplacement
            dt_var_pdf_replace.Rows.Add(row)
            Return dt_var_pdf_replace
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try
        LOG_Msg(GetCurrentMethod, sA_Replacer & "|" & sDe_Remplacement)
    End Function
    Public Shared Function PDF_CCTN_FICH(sFichier1 As String, sFichier2 As String) As String 'concaténer ou fusionner plusieurs fichiers PDF
        Dim inputDocument, outputDocument As Pdf.PdfDocument
        Try
            If File.Exists(sFichier1) Then
                outputDocument = PdfSharp.Pdf.IO.PdfReader.Open(sFichier1)
            Else
                outputDocument = New Pdf.PdfDocument
            End If
            inputDocument = PdfSharp.Pdf.IO.PdfReader.Open(sFichier2, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Import)
            Dim page As PdfPage = inputDocument.Pages.Item(0)
            outputDocument.AddPage(page)
            outputDocument.Save(sFichier1)
            outputDocument.Close()
            Return sFichier1
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try
    End Function
End Class
