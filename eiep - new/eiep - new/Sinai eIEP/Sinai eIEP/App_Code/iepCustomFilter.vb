Imports Microsoft.VisualBasic
Imports System.Data
Imports Telerik.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System
Imports System.Web
Imports System.Configuration
Imports System.Data.SqlClient


' <summary>
' Custom column that shows filtering dropdown instead of textbox
' </summary>
'
Public Class iepCustomFilter
    Inherits GridBoundColumn

    Public Shared ReadOnly Property ConnectionString() As String
        Get
            Return ConfigurationManager.ConnectionStrings("eiep_conn").ConnectionString
        End Get
    End Property

    'RadGrid will call this method when it initializes the controls inside the filtering item cells
    Protected Overrides Sub SetupFilterControls(ByVal cell As TableCell)
        MyBase.SetupFilterControls(cell)
        cell.Controls.RemoveAt(0)
        Dim combo As RadComboBox = New RadComboBox
        combo.ID = ("RadComboBox1" + Me.DataField)
        combo.ShowToggleImage = False
        combo.Skin = "Office2007"
        combo.EnableLoadOnDemand = True
        combo.AutoPostBack = True
        combo.MarkFirstMatch = True
        combo.Height = Unit.Pixel(100)
        AddHandler combo.ItemsRequested, AddressOf Me.list_ItemsRequested
        AddHandler combo.SelectedIndexChanged, AddressOf Me.list_SelectedIndexChanged
        cell.Controls.AddAt(0, combo)
        cell.Controls.RemoveAt(1)
    End Sub

    'RadGrid will call this method when the value should be set to the filtering input control(s)
    Protected Overrides Sub SetCurrentFilterValueToControl(ByVal cell As TableCell)
        MyBase.SetCurrentFilterValueToControl(cell)
        Dim combo As RadComboBox = CType(cell.Controls(0), RadComboBox)
        If (Me.CurrentFilterValue <> String.Empty) Then
            combo.Text = Me.CurrentFilterValue
        End If
    End Sub

    'RadGrid will cal this method when the filtering value should be extracted from the filtering input control(s)
    Protected Overrides Function GetCurrentFilterValueFromControl(ByVal cell As TableCell) As String
        Dim combo As RadComboBox = CType(cell.Controls(0), RadComboBox)
        Return combo.Text
    End Function

    Private Sub list_ItemsRequested(ByVal o As Object, ByVal e As RadComboBoxItemsRequestedEventArgs)
        CType(o, RadComboBox).DataTextField = Me.DataField
        CType(o, RadComboBox).DataValueField = Me.DataField
        Select Case Me.UniqueName
            Case "display_name"
                CType(o, RadComboBox).DataSource = GetDataTable("getStudentList " & System.Web.HttpContext.Current.User.Identity.Name & ", '" & e.Text & "%', NULL")
            Case "usr_display_name"
                CType(o, RadComboBox).DataSource = GetDataTable("getTeacherList " & System.Web.HttpContext.Current.User.Identity.Name & ", '%" & e.Text & "%', NULL")
        End Select

        CType(o, RadComboBox).DataBind()
    End Sub

    Private Sub list_SelectedIndexChanged(ByVal o As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs)
        Dim filterItem As GridFilteringItem = CType(CType(o, RadComboBox).NamingContainer, GridFilteringItem)
        If (Me.UniqueName = "Index") Then
            'this is filtering for integer column type
            filterItem.FireCommandEvent("Filter", New Pair("EqualTo", Me.UniqueName))
        End If
        'filtering for string column type
        filterItem.FireCommandEvent("Filter", New Pair("Contains", Me.UniqueName))
    End Sub

    Public Shared Function GetDataTable(ByVal query As String) As DataTable
        Dim conn As New SqlConnection(ConnectionString)
        Dim adapter As New SqlDataAdapter
        adapter.SelectCommand = New SqlCommand(query, conn)

        Dim myDataTable As New DataTable

        conn.Open()
        Try
            adapter.Fill(myDataTable)
        Finally
            conn.Close()
        End Try
        Return myDataTable
    End Function
End Class
