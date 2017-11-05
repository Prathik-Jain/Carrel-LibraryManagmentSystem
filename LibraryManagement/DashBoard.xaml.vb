Imports System.ComponentModel
Imports System.Windows.Threading
Imports MaterialDesignThemes.Wpf
Imports Newtonsoft.Json

Public Class DashBoard
	Public Shared SnackBarMessageQueue As SnackbarMessageQueue
	WithEvents _sendImage As DispatcherTimer
	Dim _camera As Camera
	Public memberAccount As New MemberPopup

	Public Sub New()
		' This call is required by the designer.
		InitializeComponent()
		StartCameraAndTimer()
	End Sub
	''' <summary>
	''' Function used to start camera  and  timer which ticks every <c>0.2 seconds</c>
	''' <example>
	''' <para>The interval can be changed by chaning the following line.</para>
	''' <code>
	'''		.Interval = New TimeSpan(0, 0, X) 'Where X is the number of seconds. 
	''' </code>
	''' </example>
	''' </summary>
	''' <seealso cref="Carrel.Camera"/>
	Public Sub StartCameraAndTimer()
		_camera = New Camera
		_camera.StartCamera()
		_sendImage = New DispatcherTimer With {
			.Interval = New TimeSpan(0, 0, 0.2)
			}
		_sendImage.Start()
	End Sub

	''' <summary>
	''' This Function is used to stop the camera and timer.
	''' </summary>
	''' <seealso cref="Carrel.Camera"/>
	Public Sub StopCameraAndTimer()
		_camera.StopCamera()
		_camera.Camera.WaitForStop()
		_sendImage.Stop()
		_camera.Dispose()
	End Sub

	''' <summary>
	''' This function is executed everytime the <c>_sendImage</c> timer ticks. This takes the Frame form camera and then calls the QR decoder to extract the sting. Once the string is extracted <see cref="QrScanned(String)"/> is called.
	''' </summary>
	Public Sub SendImage_Tick() Handles _sendImage.Tick
		Dim qrDecoder As New QrDecoder
		Dim qrString = ""
		qrString = qrDecoder.ScanQr(_camera.Frame)
		Try
			If qrString <> "" Then
				StopCameraAndTimer()
				QrScanned(qrString)
				qrDecoder.Dispose()
			End If
		Catch ex As Exception
			MsgBox(ex.ToString)
		End Try
	End Sub
	''' <summary>
	''' This function takes in the extracted string and perform the operations accordingly.
	''' </summary>
	''' <param name="qrString">String extracted from the QR code</param>
	Private Async Sub QrScanned(qrString As String)
		My.Computer.Audio.Play(My.Resources.ScannerBeep, AudioPlayMode.Background)
		If MemberPopupDialog.IsOpen Then

			Try
				If qrString.Contains("BOOK") Then
					If Await memberAccount.CheckBookInList(qrString) Then 'Checks and returns book if found
						memberAccount.GetData(memberAccount.LblUID.Content)
						StartCameraAndTimer()
					ElseIf Await BookService.Borrowed(qrString, memberAccount.LblUID.Content) Then
						memberAccount.GetData(memberAccount.LblUID.Content)
						StartCameraAndTimer()
					Else
						MsgBox("Failed to Update database")
					End If
				ElseIf qrString.Contains("MEM") Then
					MemberPopupDialog.IsOpen = False
				Else
					MsgBox("Please scan a book to add or return")
				End If
			Catch ex As Exception
				MsgBox(ex.ToString())
			End Try
		ElseIf AdminPopupDialog.IsOpen Then

			Try
				If qrString.Contains("ADM") Then
					StopCameraAndTimer()
					Me.Close()
				End If
			Catch ex As Exception
				MsgBox(ex.Message.ToString())
			End Try
		ElseIf ViewBookDialog.IsOpen Then

			Try
				If qrString.Contains("BOOK") Then
					ViewBookDialog.IsOpen = False
				End If
			Catch ex As Exception

			End Try
		Else
			If qrString.Contains("MEM") Then
				memberAccount.GetData(qrString)
			ElseIf qrString.Contains("ADM") Then
				Dim _admin = JsonConvert.DeserializeObject(Of Admin)(qrString)
				AdminPopup.GetData(_admin.Uid)
			ElseIf qrString.Contains("BOOK") Then
				Dim viewBook As New ViewBook
				viewBook.ViewBookById(qrString)
			End If
		End If

	End Sub


	''' <summary>
	''' This sub procedure is called when Add Book Button is clicked.
	''' The snackbar is assigned, Book Form is opened, The <c>FAB</c> is disable and the camera and timer are sotpped.
	''' </summary>
	Public Sub BtnAddBook_Click() Handles BtnAddBook.Click
		SnackBarMessageQueue = Snackbar.MessageQueue
		BookFormDialog.IsOpen = True
		BookForm.ClearAll()
		FAB.IsEnabled = False
		StopCameraAndTimer()
	End Sub
	''' <summary>
	'''This sub procedure is called when Add Book Button is clicked.
	''' The snackbar is assigned, Member Form is opened, The <c>FAB</c> is disable and the camera and timer are sotpped. 
	''' </summary>
	Private Sub BtnAddMember_Click() Handles BtnAddMember.Click
		SnackBarMessageQueue = Snackbar.MessageQueue
		MemberFormDialog.IsOpen = True
		MemberForm.clearAll()
		FAB.IsEnabled = False
		StopCameraAndTimer()
	End Sub
	''' <summary>
	''' This sub procedure is called when Member And Book form dialogs are closed.
	''' - The FAB is enabled; And the camera is also refreshed.
	''' </summary>
	Private Sub FormDialog_DialogClosing() _
		Handles MemberFormDialog.DialogClosing,
				BookFormDialog.DialogClosing
		If BookFormDialog.IsOpen = False Or MemberFormDialog.IsOpen = False Then
			FAB.IsEnabled = True
		End If
		StopCameraAndTimer()
		StartCameraAndTimer()
	End Sub
	''' <summary>
	''' Snackbar is assigned , FAB is disabled and the Camera and timer is started.
	''' </summary>
	Private Sub Dialog_DialogOpened() _
		Handles MemberPopupDialog.DialogOpened,
				AdminPopupDialog.DialogOpened,
				ViewBookDialog.DialogOpened
		SnackBarMessageQueue = Snackbar.MessageQueue
		FAB.IsEnabled = False
		StartCameraAndTimer()
	End Sub

''' <summary>
''' Camera and timer is stoped- A delay of <c>100 ms</c> is added and the camera and timer is started again.
''' <para>FAB is enabled</para>
''' </summary>
	Public Async Sub Dialog_DialogClosing() _
		Handles ViewBookDialog.DialogClosing,
				BookView.Unloaded,
				MemberPopupDialog.DialogClosing,
				MemberPopup.Unloaded,
				AdminPopupDialog.DialogClosing,
				AdminPopup.Unloaded
		StopCameraAndTimer()
		Await Task.Delay(100)
		StartCameraAndTimer()
		FAB.IsEnabled = True
	End Sub
	''' <summary>
	''' This procedure Stops the camera and timer when the dashboard is unloaded or closed.
	''' </summary>
	Private Sub CloseEvent () _
		Handles Me.Unloaded,
				Me.Closing
		StopCameraAndTimer()
	End Sub
	''' <summary>
	''' This function is used to update the cards on the dashboard everytime any of the Dialogs is closed.
	''' </summary>
	Private Async Sub DashBoard_GotFocus() _
		Handles Me.Loaded,
				MemberPopupDialog.DialogClosing,
				MemberPopup.Unloaded,
				MemberFormDialog.DialogClosing,
				MemberForm.Unloaded,
				BookFormDialog.DialogClosing,
				BookForm.Unloaded,
				ViewBookDialog.DialogClosing,
				BookView.Unloaded
		Me.LblTotalMembers.Text = Await MemberService.TotalMembers()
		Me.LblTotalIssued.Text = Await BookService.TotalBooksIssued()
		Me.LblTotalBooks.Text = Await BookService.TotalBooks()
	End Sub
	''' <summary>
	''' This procedure is implimented to disable the function of closing the window by pressing <c>Alt+F4</c>
	''' </summary>
Private Sub Window_KeyDown(sender As Object, e As System.Windows.Input.KeyEventArgs) Handles Me.KeyDown
	If Keyboard.Modifiers = ModifierKeys.Alt AndAlso e.SystemKey = Key.F4 Then
		e.handled = True
	End If
End Sub

End Class