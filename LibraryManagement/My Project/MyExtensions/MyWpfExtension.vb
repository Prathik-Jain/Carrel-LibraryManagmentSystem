Imports System.ComponentModel
Imports System.Diagnostics.CodeAnalysis
Imports System.Reflection
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.VisualBasic.Devices
Imports Microsoft.VisualBasic.Logging

#If _MyType <> "Empty" Then

Namespace My
    ''' <summary>
    '''     Module used to define the properties that are available in the My Namespace for WPF
    ''' </summary>
    ''' <remarks></remarks>
    <HideModuleName>
    Module MyWpfExtension
        Private ReadOnly SComputer As New ThreadSafeObjectProvider(Of Computer)
        Private ReadOnly SUser As New ThreadSafeObjectProvider(Of User)
        Private ReadOnly SWindows As New ThreadSafeObjectProvider(Of MyWindows)
        Private ReadOnly SLog As New ThreadSafeObjectProvider(Of Log)

        ''' <summary>
        '''     Returns the application object for the running application
        ''' </summary>
        <SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>
        Friend ReadOnly Property Application As Application
            Get
                Return CType(System.Windows.Application.Current, Application)
            End Get
        End Property

        ''' <summary>
        '''     Returns information about the host computer.
        ''' </summary>
        <SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>
        Friend ReadOnly Property Computer As Computer
            Get
                Return SComputer.GetInstance()
            End Get
        End Property

        ''' <summary>
        '''     Returns information for the current user.  If you wish to run the application with the current
        '''     Windows user credentials, call My.User.InitializeWithWindowsUser().
        ''' </summary>
        <SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>
        Friend ReadOnly Property User As User
            Get
                Return SUser.GetInstance()
            End Get
        End Property

        ''' <summary>
        '''     Returns the application log. The listeners can be configured by the application's configuration file.
        ''' </summary>
        <SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>
        Friend ReadOnly Property Log As Log
            Get
                Return SLog.GetInstance()
            End Get
        End Property

        ''' <summary>
        '''     Returns the collection of Windows defined in the project.
        ''' </summary>
        <SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>
        Friend ReadOnly Property Windows As MyWindows
            <DebuggerHidden>
            Get
                Return SWindows.GetInstance()
            End Get
        End Property

        <EditorBrowsable(EditorBrowsableState.Never)> _
        <
            MyGroupCollection _
                ("System.Windows.Window", "Create__Instance__", "Dispose__Instance__",
                 "My.MyWpfExtenstionModule.Windows")>
        Friend NotInheritable Class MyWindows
            <DebuggerHidden>
            Private Shared Function Create__Instance__ (Of T As {New, Window})(instance As T) As T
                If Instance Is Nothing Then
                    If _sWindowBeingCreated IsNot Nothing Then
                        If _sWindowBeingCreated.ContainsKey(GetType(T)) = True Then
                            Throw _
                                New InvalidOperationException(
                                    "The window cannot be accessed via My.Windows from the Window constructor.")
                        End If
                    Else
                        _sWindowBeingCreated = New Hashtable()
                    End If
                    _sWindowBeingCreated.Add(GetType(T), Nothing)
                    Return New T()
                    _sWindowBeingCreated.Remove(GetType(T))
                Else
                    Return Instance
                End If
            End Function

            <SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>  _
            <DebuggerHidden>
            Private Sub Dispose__Instance__ (Of T As Window)(ByRef instance As T)
                instance = Nothing
            End Sub

            <DebuggerHidden> _
            <EditorBrowsable(EditorBrowsableState.Never)>
            Public Sub New()
                MyBase.New()
            End Sub

            <ThreadStatic> Private Shared _sWindowBeingCreated As Hashtable

            <EditorBrowsable(EditorBrowsableState.Never)>
            Public Overrides Function Equals(o As Object) As Boolean
                Return MyBase.Equals(o)
            End Function

            <EditorBrowsable(EditorBrowsableState.Never)>
            Public Overrides Function GetHashCode() As Integer
                Return MyBase.GetHashCode
            End Function

            <SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>  _
            <EditorBrowsable(EditorBrowsableState.Never)>
            Friend Overloads Function [GetType]() As Type
                Return GetType(MyWindows)
            End Function

            <EditorBrowsable(EditorBrowsableState.Never)>
            Public Overrides Function ToString() As String
                Return MyBase.ToString
            End Function
        End Class
    End Module
End Namespace

Partial Class Application
    Inherits Windows.Application

    <SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")> _
    <SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")>
    Friend ReadOnly Property Info As AssemblyInfo
        <DebuggerHidden>
        Get
            Return New AssemblyInfo(Assembly.GetExecutingAssembly())
        End Get
    End Property
End Class

#End If