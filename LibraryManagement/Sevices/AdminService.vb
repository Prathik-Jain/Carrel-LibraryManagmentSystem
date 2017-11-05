Imports System.Configuration
Imports System.Data.SqlClient
Imports Newtonsoft.Json
''' <summary>
''' This class is used to working with operations related to an Admin.
''' <para>  This normally affects the <c>Admin</c> and <c>Member</c> table </para>
''' </summary>
Public Class AdminService
    ''' <summary>
    ''' 
    ''' This function is to count the number of records where the entered PIN is matched with the username(UID) on the scanned QR-code; Using the <c>Admin</c> table.
    ''' </summary>
    ''' <param name="uid">Unique ID of Admin scanned from the QR Code.</param>
    ''' <param name="pin">PIN entered to login scanned from the QR Code</param>
    ''' <returns>The number of records where First name is matched with UID; It should be 1 or 0</returns>
    Public Function MatchPin(uid As String, pin As Integer) As Integer
        Dim connection =
                New SqlConnection(ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim command = New SqlCommand("Select count(*) from Admin where UID=@UID and PIN=@PIN", connection)
        command.Parameters.Add(New SqlParameter("@UID", uid))
        command.Parameters.Add(New SqlParameter("@PIN", pin))
        Try
            connection.Open()
            MatchPin = command.ExecuteScalar()
        Catch ex As Exception
            MsgBox(ex.ToString())
            Return False
        Finally
            command.Connection.Close()
        End Try
    End Function
    ''' <summary>
    ''' This function is used to check if the UID and First Name on the QR-Code matches with the data on <c>Admin</c> table
    ''' </summary>
    ''' <param name="uid">Unique ID of Admin scanned from the QR Code</param>
    ''' <param name="name">First Name of Admin scanned from the QR Code</param>
    ''' <returns>This function returns a positive integer if UID matches with the First Name</returns>
    Public Function CheckUser(uid As String, name As String) As Integer
        Dim connection =
                New SqlConnection(ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim command = New SqlCommand("Select count(*) from Admin where UID=@UID and FName=@Name", connection)
        command.Parameters.Add(New SqlParameter("@UID", uid))
        command.Parameters.Add(New SqlParameter("@Name", name))
        Try
            connection.Open()
            CheckUser = command.ExecuteScalar()
        Catch ex As Exception
            MsgBox(ex.ToString)
            Return False
        Finally
            command.Connection.Close()
        End Try
    End Function
    ''' <summary>
    ''' This function is used to get all the details of the Admin with the unique ID from <c>Admin</c> table.
    ''' </summary>
    ''' <param name="uid">Unique ID of Admin</param>
    ''' <returns>An ArrayList of data</returns>
    Friend Shared Function GetAdmin(uid As String) As ArrayList
        Dim connection = New SqlConnection(ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = New SqlCommand("SELECT * FROM ADMIN WHERE UID = @UID", connection)
        query.Parameters.Add(New SqlParameter("@UID", uid))
        Try
            connection.Open()
            Dim reader As SqlDataReader
            reader = query.ExecuteReader
            Dim data As New ArrayList
            reader.Read()
            data.Add(reader("FName").ToString)
            data.Add(reader("LName").ToString)
            data.Add(reader("Phone").ToString)
            data.Add(reader("Pin").ToString)
            data.Add(reader("SecurityQuestion").ToString)
            data.Add(reader("Answer").ToString)
            Return data
        Catch ex As Exception
            Throw
        Finally
            query.Connection.Close()
        End Try
    End Function
    ''' <summary>
    ''' This function is used to add new Admin to the Database by inserting the value in <c>JSON</c> table. The value will then be added to the <c>Admin</c> table by the Tigger on JSON table.
    ''' </summary>
    ''' <param name="admin">An object of Admin</param>
    ''' <returns>The number of rows affected.</returns>
    Public Shared Async Function AddAdmin(admin As Object) As Task(Of Integer)
        Dim connection =
                New SqlConnection(ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = New SqlCommand("INSERT INTO JSON (CATEGORY,DATA) VALUES ('ADM',@JSONString)", connection)
        Try
            'Converts admin object to a json string and adds that as a parameter to the query
            query.Parameters.Add(New SqlParameter("@JSONString", JsonConvert.SerializeObject(admin)))
            connection.Open()
            Return Await query.ExecuteNonQueryAsync()
        Catch ex As Exception
            MsgBox(ex.ToString())
            Throw
        Finally
            query.Connection.Close()
        End Try
    End Function
    ''' <summary>
    ''' This function is used to edit Admin Details in <c>Admin</c> table.
    ''' </summary>
    ''' <param name="admin">An object of Admin - containing all the details</param>
    ''' <param name="uid">Unique ID of the Admin</param>
    ''' <returns>The number of rows affected.</returns>
    Friend Shared Async Function EditAdmin(admin As Object, uid As String) As Task(Of Integer)
        Dim connection = New SqlConnection(ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = New SqlCommand("UPDATE Admin 
                                    SET FNAME = @FName, LNAME = @LName, PHONE = @Phone, PIN = @Pin, SECURITYQUESTION = @SecurityQ,  ANSWER = @Answer
                                    WHERE UID = @UID", connection)
        query.Parameters.Add(New SqlParameter("@FName", admin("FName").ToString))
        query.Parameters.Add(New SqlParameter("@LName", admin("LName").ToString))
        query.Parameters.Add(New SqlParameter("@Phone", admin("Phone").ToString))
        query.Parameters.Add(New SqlParameter("@Pin", admin("Pin").ToString))
        query.Parameters.Add(New SqlParameter("@SecurityQ", admin("SecurityQuestion").ToString))
        query.Parameters.Add(New SqlParameter("@Answer", admin("Answer").ToString))
        query.Parameters.Add(New SqlParameter("@UID", uid))
        Try
            connection.Open()
            Return Await query.ExecuteNonQueryAsync()
        Finally
            query.Connection.Close()
        End Try
    End Function
    ''' <summary>
    ''' This function is responsible to get the <c>UID</c> in the last record of the <c>Admin</c> table.
    ''' The Unique ID is then used to Generate QR.
    ''' </summary>
    ''' <returns>Last Unique ID in Admin Table.</returns>
    Friend Shared Function GetLastUid() As String
        Dim connection = New SqlConnection(ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = New SqlCommand("SELECT UID FROM ADMIN WHERE ID = (SELECT MAX(ID) FROM ADMIN)", connection)
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
    ''' This function is used to change the semester of students, called when Change semester Button on <c>Admin Popup</c> is clicked.
    ''' <para>Adds one to members of semester less than 6 and deletes all the members  who are in sixth semester - This can be changed as the institutions requiement</para>
    ''' </summary>
    ''' <example>
    ''' To change the semester and remove members after 'X' semisters
    ''' <code>
    '''        Dim query = New SqlCommand("DELETE Member
    '''                    Where SEM = x",connection) ' X = maximun number of semisters in the institution.
    ''' </code>
    ''' </example>
    Public Shared Sub ChangeSem()
        Dim connection = New SqlConnection(ConfigurationManager.ConnectionStrings("Carrel").ConnectionString)
        Dim query = New SqlCommand("DELETE Member
                                    Where SEM = 6", connection)

        Dim query2 = New SqlCommand("UPDATE Member
                                    SET SEM = SEM+1
                                    WHERE SEM > 0 ", connection)
        Try
            connection.Open()
            query.ExecuteNonQuery()
            query2.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.Message)
            Throw
        Finally
            query.Connection.Close()
        End Try
    End Sub
End Class
