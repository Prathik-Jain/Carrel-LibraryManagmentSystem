
Imports System.Configuration
Imports System.Data.SqlClient

Public Class AdminService
    Public Function MatchPin(uid As String, pin As Integer) As Integer
        Dim connection =
                New SqlConnection(ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim command = New SqlCommand("Select count(*) from Admin where UID=@UID and PIN=@PIN", connection)
        command.Parameters.Add(New SqlParameter("@UID", UID))
        command.Parameters.Add(New SqlParameter("@PIN", PIN))
        Try
            connection.Open()
            MatchPIN = command.ExecuteScalar()
        Catch ex As Exception
            MsgBox(ex.ToString())
            Return False
        Finally
            command.connection.Close()
        End Try
    End Function

    Public Function CheckUser(uid As String, name As String) As Integer
        Dim connection =
                New SqlConnection(ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim command = New SqlCommand("Select count(*) from Admin where UID=@UID and FName=@Name", connection)
        command.Parameters.Add(New SqlParameter("@UID", UID))
        command.Parameters.Add(New SqlParameter("@Name", Name))
        Try
            connection.Open()
            CheckUser = command.ExecuteScalar()
        Catch ex As Exception
            Msgbox(ex.toString)
            Return False
        Finally
            command.connection.Close()
        End Try
    End Function
End Class
