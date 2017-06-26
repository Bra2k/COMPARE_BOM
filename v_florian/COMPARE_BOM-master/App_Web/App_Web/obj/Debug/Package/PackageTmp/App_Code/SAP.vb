
Namespace App_Code

    Public Class SAP


        Public Shared Function SAP_Connexion(Optional User As String = "RFCUSER", Optional Password As String = "saprfc!", Optional Codeclient As String = "600", Optional Serveur As String = "SAP_PEO", Optional SystemNumber As String = "00") As Object

            Dim oFunctionSAP As Object
            'Dim LogonControl As Object
            Dim oConn As Object

            'LogonControl = CreateObject("SAP.LogonControl.1")
            oFunctionSAP = CreateObject("SAP.Functions")
            oConn = oFunctionSAP.Connection
            oConn.autologon = True
            oConn.Client = Codeclient
            oConn.User = User
            oConn.Password = Password
            oConn.ApplicationServer = Serveur
            oConn.SystemNumber = SystemNumber
            'Conn = LogonControl.NewConnection

            'Conn.SystemNumber = 0
            'Conn.ApplicationServer = "10.10.10.10" 'PRD 
            'Conn.Client = "900"
            ' Conn.User = "XXXXX"
            'Conn.Language = "EN"
            'Conn.SNC = True
            'Conn.SNCName = "p: SAPServicePRD @ corp.XXX.com "
            'Conn.SNCQuality = 3

            If oConn.logon(0, True) <> True Then
                'MsgBox("ERREUR - connexion à SAP a échoué")
                Return 0
            Else
                Return oFunctionSAP
            End If
        End Function


        Public Shared Function SAP_ReadTable(ByRef oFunctionSAP As Object, Table As String, Optional Delimiter As String = "|", Optional NoData As String = "", Optional Field As String = "", Optional option_req As String = "", Optional fieldinarray As Integer = 0, Optional ligneskip As Integer = 0, Optional nbrlignes As Integer = 0) As String(,)



            'func lecturetable($table,$delimit="|",$nodata="",$champ="",$option_req="",$fieldinarray=0,$ligneskip=0,$nbrlignes=0)

            'local $array_champ,$array_sap[1],$RFC,$functionCtrl,$strExport1, $option

            'Dim functionCtrl As Object = CreateObject("SAP.Functions")
            'functionCtrl.Connection = oConn
            Dim RFC As Object = oFunctionSAP.Add("RFC_READ_TABLE") ' limité à 512 en longueurs zone de champ sinon crash DATA_BUFFER_EXCEEDED

            Dim strExport1 As Object = RFC.Exports("QUERY_TABLE")

            RFC.exports("DELIMITER").value = Delimiter
            RFC.exports("NO_DATA").value = NoData

            RFC.exports("ROWSKIPS").value = ligneskip
            RFC.exports("ROWCOUNT").value = nbrlignes

            Dim FIELDS As Object = RFC.Tables("FIELDS")
            If Field <> "" Then
                Dim array_champ() As String = Split(Field, " ")
                Dim iArray As Integer

                For iArray = 0 To UBound(array_champ)
                    FIELDS.AppendRow()
                    FIELDS(iArray + 1, "FIELDNAME") = array_champ(iArray)
                Next
            End If

            Dim option_table As Object = RFC.Tables("OPTIONS")
            option_table.Appendrow()
            option_table(1, "TEXT") = option_req

            RFC.Exports("QUERY_TABLE").Value = Table

            If RFC.Call <> -1 Then
                Return RFC.exception
            End If

            Dim olinedata As Object = RFC.tables.Item("DATA")
            Dim olinefield As Object = RFC.tables.Item("FIELDS")
            'MsgBox("nbr de champ : " & olinefield.rowcount)
            Dim data_array(olinedata.rowcount + 1, olinefield.rowcount + 1) As String
            Dim z As Integer
            For z = 1 To olinefield.rowcount
                'MsgBox("champ n° : " & olinefield.value(z, 1))
                data_array(0, z) = olinefield.value(z, 1)
            Next


            'If fieldinarray = 1 Then
            'Dim p As Integer
            'For p = 1 To UBound(array_champ) - 1
            'array(0)(p) = array_champ(p)
            'Next
            'End If

            Dim i As Integer
            Dim p As Integer
            Dim ligne() As String
            data_array(0, 0) = olinedata.rowcount
            For i = 1 To olinedata.rowcount
                'MsgBox(olinedata.value(i, 1))
                ligne = Split(olinedata.value(i, 1), Delimiter)
                For p = 0 To UBound(ligne)
                    data_array(i, p) = ligne(p)
                Next
            Next
            Return data_array
        End Function


        Public Shared Function SAP_Deconnexion(ByRef oFunctionSAP As Object) As Object
            oFunctionSAP.Connection.logoff()
            Return oFunctionSAP
        End Function





    End Class
End Namespace
