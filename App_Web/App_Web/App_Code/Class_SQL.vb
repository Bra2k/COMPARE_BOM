Imports App_Web.LOG
Imports System.Reflection.MethodBase
Imports System.Data.SqlClient
Public Class Class_SQL

    Public Shared Function SQL_SELE_TO_DT(sQuery As String, sChaineConnexion As String) As DataTable

        Dim dt As New DataTable
        Dim con As New SqlConnection

        Try
            con = SQL_CONN(sChaineConnexion)
            Using dad As New SqlDataAdapter(sQuery, con)
                dad.SelectCommand.CommandTimeout = 7200
                dad.Fill(dt)
            End Using
            If dt.Rows.Count = 0 Then Throw New Exception("la requête " & sQuery & " n'a retourné aucun résultat.")
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod(), ex.Message)
            Return Nothing
        Finally
            con = SQL_CLOS(con)
        End Try

        LOG_Msg(GetCurrentMethod, "la requête " & sQuery & " a été exécutée.")
        Return dt

    End Function

    Public Shared Sub SQL_REQ_ACT(sQuery As String, sChaineConnexion As String)
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand

        Try
            con = SQL_CONN(sChaineConnexion)
            cmd.Connection = con
            cmd.CommandText = sQuery
            If cmd.ExecuteNonQuery() = 0 Then Throw New Exception("la requête " & sQuery & " n'a retourné aucun résultat.")
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
        Finally
            con = SQL_CLOS(con)
        End Try
        LOG_Msg(GetCurrentMethod, "la requête " & sQuery & " a été exécutée.")

    End Sub

    Public Shared Function SQL_REQ_ACT_RET_IDTT(sQuery As String, sChaineConnexion As String) As String
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        Dim sQuery_2 As String = "SELECT @@IDENTITY"
        Dim ID As Integer

        Try
            con = SQL_CONN(sChaineConnexion)
            cmd.Connection = con
            cmd.CommandText = sQuery
            If cmd.ExecuteNonQuery() = 0 Then Throw New Exception("la requête " & sQuery & " n'a retourné aucun résultat.")
            cmd.CommandText = sQuery_2
            ID = cmd.ExecuteScalar()
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        Finally
            con = SQL_CLOS(con)
        End Try
        LOG_Msg(GetCurrentMethod, "la requête " & sQuery & " a été exécutée.")
        Return ID

    End Function

    Public Shared Function SQL_CONN(Optional sCONN_STR As String = "Data Source=cedb03,1433;Initial Catalog=SERCEL;Persist Security Info=True;User ID=sa;Password=mdpsa@SQL") As SqlConnection

        Dim SQL_Connexion = New SqlConnection()

        Try
            SQL_Connexion.ConnectionString = sCONN_STR
            SQL_Connexion.Open()
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try

        'LOG_Msg(GetCurrentMethod, "La connexion " & sCONN_STR & " a été exécutée.")
        Return SQL_Connexion

    End Function

    Public Shared Function SQL_CLOS(ByRef cn As SqlConnection) As SqlConnection

        Try
            cn.Close()
            cn = Nothing
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try

        'LOG_Msg(GetCurrentMethod, "La connexion a été fermée.")
        Return cn

    End Function

    Public Shared Function SQL_CALL_STOR_PROC(ByRef cn As SqlConnection, SqlCommand_String As String) As SqlCommand

        Dim cmd As SqlCommand

        Try
            cmd = New SqlCommand(SqlCommand_String, cn)
            cmd.CommandType = CommandType.StoredProcedure
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try

        LOG_Msg(GetCurrentMethod, "La procédure stockée " & SqlCommand_String & " a été appelée.")
        Return cmd

    End Function

    Public Shared Sub SQL_EXEC_STOR_PROC(ByRef cmd As SqlCommand)

        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Exit Sub
        End Try

        LOG_Msg(GetCurrentMethod, "La procédure stockée a été exécutée.")

    End Sub

    Public Shared Sub SQL_ADD_PARA_STOR_PROC(ByRef cmd As SqlCommand, paramètre As String, type As Integer, taille As Integer, valeur As String, Optional direction As String = "")

        Try
            cmd.Parameters.Add(New SqlParameter(paramètre, type, taille))
            cmd.Parameters(paramètre).Value = valeur
            If direction = "Output" Then cmd.Parameters(paramètre).Direction = ParameterDirection.InputOutput
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Exit Sub
        End Try

        LOG_Msg(GetCurrentMethod, "La valeur " & valeur & " a été appliquée sur le paramètre " & paramètre & ".")

    End Sub

    Public Shared Function SQL_GET_PARA_VAL(ByRef cmd As SqlCommand, paramètre As String) As String

        Dim resultat As String

        Try
            If cmd.Parameters(paramètre).Value Is DBNull.Value Then
                resultat = ""
            Else
                resultat = cmd.Parameters(paramètre).Value
            End If
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try

        LOG_Msg(GetCurrentMethod, "Le paramètre " & paramètre & " est égal à " & resultat)
        Return resultat

    End Function

    Public Shared Sub SQL_BULK_COPY_DT(sChaineConnexion As String, sTable As String, dtDonnées As DataTable)

        Dim bulkCopy As New SqlBulkCopy(sChaineConnexion, SqlBulkCopyOptions.KeepIdentity)

        Try
            For Each col As DataColumn In dtDonnées.Columns
                bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName)
            Next
            bulkCopy.BulkCopyTimeout = 600
            bulkCopy.DestinationTableName = sTable
            bulkCopy.WriteToServer(dtDonnées)
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Exit Sub
        End Try

        LOG_Msg(GetCurrentMethod, "La datatable a été copiée dans la base")

    End Sub

    Public Shared Function SQL_GET_DT_FROM_STOR_PROC(ByRef cmd As SqlCommand) As DataTable
        Dim da = New SqlDataAdapter(cmd)
        Dim dt As New DataTable

        Try
            da.Fill(dt)
            If dt.Rows.Count = 0 Then Throw New Exception("Aucun résultat retourné dans la procédure stockée")
        Catch ex As Exception
            LOG_Erreur(GetCurrentMethod, ex.Message)
            Return Nothing
        End Try
        LOG_Msg(GetCurrentMethod, "La datatable a été copiée dans la base")
        Return dt
    End Function
End Class
