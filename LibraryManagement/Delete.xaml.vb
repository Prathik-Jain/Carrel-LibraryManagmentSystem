Public Class Delete
    public Dim cat, id as String
    Dim dashboard as dashboard = Application.Current.Windows(0)
    Public Sub New()
        InitializeComponent()
    End Sub

    
    Friend Sub SetData(category As String, uid As String)
        cat = category
        id =uid
    End Sub
    Private ASYNC Sub BtnDelete_Click(sender As Object, e As RoutedEventArgs) Handles BtnDelete.Click
        if cat.ToString() = "MEM"
          If await MemberService.Delete(id) Then
                  DashBoard.SnackBarMessageQueue.Enqueue("Deleted Member.")
                  dashboard.ViewMemberDialog.IsOpen=False
                End If
        ElseIf  cat.ToString() = "BOOK"
            'BookService.Delete(id)
        End If
    End Sub

End Class
