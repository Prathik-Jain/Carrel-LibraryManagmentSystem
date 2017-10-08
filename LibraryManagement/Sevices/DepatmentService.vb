Imports System.Data.SqlClient

Public Class DepatmentService

    Public Shared Function GetDept() As ArrayList
        Dim connection = new SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim DR as SqlDataReader
        Dim query = new SqlCommand("SELECT * FROM DEPT", connection)
        connection.Open()
        DR = query.ExecuteReader()
        dim Dept = New ArrayList
        While(DR.Read())
            Dept.Add(DR.GetString(1))
        End While
        DR.Close()
        connection.Close()
        Return Dept
    End Function

     Shared Sub AddDepatment(DEPT As String)
        Dim connection = New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = New SqlCommand("INSERT INTO DEPT VALUES (@DEPT)", connection)
        query.Parameters.Add(New SqlParameter("@DEPT",DEPT))
        connection.Open()
        query.ExecuteNonQueryAsync()
        query.Connection.Close()
    End Sub
End Class
