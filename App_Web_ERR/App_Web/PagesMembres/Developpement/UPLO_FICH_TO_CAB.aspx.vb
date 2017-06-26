﻿Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports App_Web.Class_COMM_APP_WEB
Imports System.IO
Public Class UPLO_FICH_TO_CAB
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Button_VALI_SAIS_Click(sender As Object, e As EventArgs) Handles Button_VALI_SAIS.Click

        Dim sAdresseConnection As String = ""
        Try
            Dim flImages As HttpFileCollection = Request.Files
            For Each key As String In flImages.Keys
                Dim flfile As HttpPostedFile = flImages(key)
                flfile.SaveAs($"{My.Settings.RPTR_TPRR}\{flfile.FileName}")
                Select Case RadioButtonList_TYPE_IPMT.SelectedValue
                    Case "EOS1"
                        sAdresseConnection = $"ftp://ftpcard:card@{TextBox_IP.Text}"
                        If LCase(Path.GetExtension(flfile.FileName)) = ".ttf" Then
                            My.Computer.Network.UploadFile($"{My.Settings.RPTR_TPRR}\{flfile.FileName}", $"{sAdresseConnection}/fonts/{flfile.FileName}")
                            'LOG_Msg(GetCurrentMethod, System.IO.File.Exists(sAdresseConnection & "/fonts/" & sFichier))
                        Else
                            My.Computer.Network.UploadFile($"{My.Settings.RPTR_TPRR}\{flfile.FileName}", $"{sAdresseConnection}/images/{flfile.FileName}")
                            'LOG_Msg(GetCurrentMethod, System.IO.File.Exists(sAdresseConnection & "/images/" & sFichier))
                        End If
                    Case "MACH4"
                        sAdresseConnection = $"ftp://root:0000@{TextBox_IP.Text}"
                        My.Computer.Network.UploadFile($"{My.Settings.RPTR_TPRR}\{flfile.FileName}", $"{sAdresseConnection}/card/{flfile.FileName}")
                        'LOG_Msg(GetCurrentMethod, System.IO.File.Exists(sAdresseConnection & "/card/" & sFichier))
                End Select
            Next
        Catch ex As Exception

        End Try


    End Sub
End Class