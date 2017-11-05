Imports System.Data.SqlClient
Imports Newtonsoft.Json
''' <summary>
''' Hanadles function related to a Member of the library
''' </summary>
Public Class MemberService
    ''' <summary>
    ''' This function is used to add new member to the Database.
    ''' <para>This is done by adding a JSON string to <c>JSON</c> table and then the TRIGGER on the JSON table adds it to the <c>Member</c> table in the database.</para>
    ''' </summary>
    ''' <param name="member">A dynamic Object created when the <see cref="MemberForm"/> is submitted - contatining all the details about the member.</param>
    ''' <returns>The number of rows affected; A positive integer would mean true.</returns>
    Friend Async Shared Function AddMember(member As Object) As Task(Of Integer)
        Dim connection = New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = New SqlCommand("INSERT INTO JSON (CATEGORY,DATA) VALUES ('MEM',@JSONString)", connection)

        Try
            connection.Open()
            query.Parameters.Add(New SqlParameter("@JSONString", JsonConvert.SerializeObject(member)))
            Return Await query.ExecuteNonQueryAsync()
        Catch ex As Exception
            MsgBox(ex.ToString())
            Throw
        Finally
                query.connection.Close()
        End Try
    End Function
    ''' <summary>
    ''' This function is used to remove a member from the <c>Member</c> table.
    ''' </summary>
    ''' <param name="uid">Unique ID of the member to be removed.</param>
    ''' <returns>Number of rows affected. A possitive integer would mean the delete was successful.</returns>
    Friend Async Shared Function Delete(uid As String) As Task(Of Boolean)
        Dim connection = New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = new SqlCommand("DELETE FROM Member WHERE UID = @UID",connection)
        query.Parameters.Add(new SqlParameter("@UID",uid))
        Try
            Connection.Open()
            Return Await query.ExecuteNonQueryAsync()
        Catch  ex As Exception
            MsgBox(ex.ToString())
            Throw
        Finally
            query.Connection.Close()
        End Try
    End Function
    ''' <summary>
    ''' This function is used to get the Total numbers of member registered with the library.
    ''' </summary>
    ''' <returns>The count from <c>Member</c> table.</returns>
    Public Async Shared Function TotalMembers() As Task(Of String)
        Dim connection =
                New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = New SqlCommand("SELECT COUNT(*) FROM Member", connection)
        Try
            connection.Open()
            Return Await query.ExecuteScalarAsync
        Catch ex As Exception
            MsgBox(ex.ToString())
            Throw
        Finally
            query.Connection.Close()
        End Try
    End Function
    ''' <summary>
    ''' This function is used to get the Unique ID of the last Member added to the Database.
    ''' </summary>
    ''' <returns>The Unique ID of the member last added.</returns>
Friend  Shared Function GetLastUid() As String
        Dim connection = New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = New SqlCommand("SELECT UID FROM MEMBER WHERE ID = (SELECT MAX(ID) FROM MEMBER)", connection)
        Try
            connection.Open()
            Return query.ExecuteScalar()
        Catch ex As Exception
            MsgBox(ex.Message)
            Throw
        Finally
                query.Connection.Close()
        End Try
    End Function
    ''' <summary>
    ''' This function is used to edit member details
    ''' </summary>
    ''' <param name="uid">Unique ID of the member whose details are to be changed.</param>
    ''' <param name="member">A dynamic object which contains the member details.</param>
    ''' <returns>The number of rows affected - Basically a possitve integer if the function was executed properly.</returns>
    Friend Shared Async Function EditMember(uid As String, member As Object) As Task(Of Integer)
        Dim connection = New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = New SqlCommand("UPDATE Member 
                                    SET FNAME = @FName, LNAME = @LName, PHONE = @Phone, DEPT = @Dept, SEM = @Sem
                                    WHERE UID = @UID", connection)
        query.Parameters.Add(New SqlParameter("@FName", member("FName").ToString))
        query.Parameters.Add(New SqlParameter("@LName", member("LName").ToString))
        query.Parameters.Add(New SqlParameter("@Phone", member("Phone").ToString))
        query.Parameters.Add(New SqlParameter("@Dept", member("Dept").ToString))
        query.Parameters.Add(New SqlParameter("@Sem", member("Sem").ToString))
        query.Parameters.Add(New SqlParameter("@UID", UID))
        Try
            connection.Open()
            Return Await query.ExecuteNonQueryAsync()
        Finally
            query.connection.Close()
        End Try
    End Function
    ''' <summary>
    ''' This function is used to get the member details by the Unique ID from <c>Member</c> table.
    ''' </summary>
    ''' <param name="uid">Unique ID of the member whose details are to be extracted.</param>
    ''' <returns>An ArrayList contating the member details.</returns>
    Friend Shared Function GetMember(uid As String)
        Dim connection = New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = New SqlCommand("SELECT * FROM MEMBER WHERE UID = @UID",connection)
        query.Parameters.Add(New SqlParameter("@UID",UID))
        Try
            connection.Open
            Dim reader As SqlDataReader
            reader = query.ExecuteReader
            Dim data As New ArrayList
            reader.Read
                data.Add(reader("FName").ToString)
                data.Add(reader("LName").ToString)
                data.Add(reader("Phone").ToString)
                data.Add(reader("Dept").ToString)
                data.Add(reader("Sem").ToString)
                data.Add(reader("MaxBooks").ToString)
                data.Add(reader("Dues").ToString)
            Return data
            Catch ex As exception
            throw
            Finally
                query.connection.Close
        End Try
    End Function
    ''' <summary>
    ''' This function is used to get only the first name of the member by the Unique ID of the Member.
    ''' </summary>
    ''' <param name="memberId">Unique ID of the member.</param>
    ''' <returns>First Name of the Member</returns>
    Public Shared Function GetMemberById(memberId As String) As String
        Dim connection = New SqlConnection(Configuration.ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = New SqlCommand("SELECT FNAME FROM MEMBER WHERE UID = @MemberID",connection)
        query.Parameters.Add(New SqlParameter("@MemberID",memberID))
        Try
            connection.Open()
            Return query.ExecuteScalar()
        Catch ex As Exception
            Throw
            Finally
            query.connection.Close()
        End Try

    End Function
End Class
