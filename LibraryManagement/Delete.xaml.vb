Public Class Delete
    public Dim cat, id as String
    Dim _dashboard as dashboard 
    Public Sub New()
        InitializeComponent()
    End Sub

    ''' <summary>
    ''' This sub procedure sets the category and the Unique ID of the Item to be deleted; either form <c>Book</c> or <c>Member</c> table.
    ''' </summary>
    ''' <param name="category">The category of the item to be deleted.</param>
    ''' <param name="uid">Unique ID of the Item</param>
    Friend Sub SetData(category As String, uid As String)
        cat = category
        id =uid
        For Each window As Window In Application.Current.Windows
            If window.GetType() = GetType(DashBoard)
                _dashBoard = window
            End If
        Next
    End Sub
    ''' <summary>
    ''' This function calls <see cref="MemberService.Delete(String)"/> or <see cref="BookService.Delete(String)"/> depending on the category set.
    ''' </summary>
    ''' <seealso cref="BookService"/>
    ''' <seealso cref="MemberService"/>
    Private Async Sub BtnDelete_Click() Handles BtnDelete.Click
        if cat.ToString() = "MEM"
          If await MemberService.Delete(id) Then
                  DashBoard.SnackBarMessageQueue.Enqueue("Deleted Member.")
                  _dashboard.MemberPopupDialog.IsOpen=False
                End If
        ElseIf  cat.ToString() = "BOOK"
           if await BookService.Delete(id)
               DashBoard.SnackBarMessageQueue.Enqueue("Deleted Book. " + id )
               _dashboard.ViewBookDialog.IsOpen=False
           End If
        End If
    End Sub

End Class
