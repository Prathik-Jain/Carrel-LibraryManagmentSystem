Public Class Delete
    public Dim cat, id as String
    Dim _dashboard as dashboard 
    Public Sub New()
        InitializeComponent()
    End Sub

    
    Friend Sub SetData(category As String, uid As String)
        cat = category
        id =uid
        For Each window As Window In Application.Current.Windows
            If window.GetType() = GetType(DashBoard)
                _dashBoard = window
            End If
        Next
    End Sub
    Private ASYNC Sub BtnDelete_Click(sender As Object, e As RoutedEventArgs) Handles BtnDelete.Click
        if cat.ToString() = "MEM"
          If await MemberService.Delete(id) Then
                  DashBoard.SnackBarMessageQueue.Enqueue("Deleted Member.")
                  _dashboard.MemberPopupDialog.IsOpen=False
                End If
        ElseIf  cat.ToString() = "BOOK"
            'BookService.Delete(id)
        End If
    End Sub

End Class
