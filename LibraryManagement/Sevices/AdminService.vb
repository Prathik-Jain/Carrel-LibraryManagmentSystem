
Imports System.Data.SqlClient

Public Class AdminService
    Public Function MatchPIN(UID As String, PIN As Integer) As Integer
        Dim connection = New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("CarrelConnectionString").ConnectionString)
        Dim command = New SqlCommand("Select count(*) from Admin where UID=@UID and PIN=@PIN", connection)
        command.Parameters.Add(New SqlParameter("@UID", UID))
        command.Parameters.Add(New SqlParameter("@PIN", PIN))
        connection.Open()
        Try
            MatchPIN = command.ExecuteScalar()
        Finally
            command.Connection.Close()
        End Try
    End Function
    Public Function CheckUser(UID As String, Name As String) As Integer
        Dim connection = New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("CarrelConnectionString").ConnectionString)
        Dim command = New SqlCommand("Select count(*) from Admin where UID=@UID and Name=@Name", connection)
        command.Parameters.Add(New SqlParameter("@UID", UID))
        command.Parameters.Add(New SqlParameter("@Name", Name))
        connection.Open()
        Try
            CheckUser = command.ExecuteScalar()
        Catch e As Exception
            MsgBox("Database error")
        Finally
            command.Connection.Close()
        End Try

    End Function

End Class
