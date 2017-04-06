Imports System
Imports System.Data
Imports System.Collections
Imports System.Web.UI
Imports Telerik.Web.UI

Public Class csp_header
    Inherits System.Web.UI.UserControl

    Private _dataItem As Object = Nothing

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        usr_login.Value = Context.User.Identity.Name
    End Sub

#Region "Web Form Designer generated code"

    Protected Overrides Sub OnInit(ByVal e As EventArgs)
        '
        ' CODEGEN: This call is required by the ASP.NET Web Form Designer.
        '
        InitializeComponent()
        MyBase.OnInit(e)
    End Sub 'OnInit


    '/ <summary>
    '/          Required method for Designer support - do not modify
    '/          the contents of this method with the code editor.
    '/ </summary>
    Private Sub InitializeComponent()
        AddHandler DataBinding, AddressOf Me.cspHeader_DataBinding
    End Sub 'InitializeComponent

#End Region

    Public Property DataItem() As Object
        Get
            Return Me._dataItem
        End Get
        Set(ByVal value As Object)
            Me._dataItem = value
        End Set
    End Property

    Protected Sub cspHeader_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
        If Not (TypeOf DataItem Is Telerik.Web.UI.GridInsertionObject) Then
            Dim teacherVal As Object = DataBinder.Eval(DataItem, "usr_uuid")
            Dim oItem As New RadListBoxItem
            cmbTeacher.DataBind()
            If Not cmbTeacher.FindItemByValue(teacherVal.ToString) Is Nothing Then
                cmbTeacher.SelectedValue = teacherVal.ToString
            Else
                For intCounter = 2 To 5
                    If Not IsDBNull(DataBinder.Eval(DataItem, "teacher_" & intCounter & "_uuid")) Then
                        cmbTeacher.SelectedValue = DataBinder.Eval(DataItem, "teacher_" & intCounter & "_uuid").ToString
                        Exit For
                    End If
                Next

            End If

            Dim schoolVal As Object = DataBinder.Eval(DataItem, "school_id")
            cmbSchool.DataBind()
            cmbSchool.SelectedValue = schoolVal.ToString

            ddStudent.DataBind()

            tbSubject.Text = DataBinder.Eval(DataItem, "iep_subject")
            tbMaterials.Text = DataBinder.Eval(DataItem, "iep_materials")
            tbYear.Text = DataBinder.Eval(DataItem, "iep_year")

        Else
            If Month(Now) >= 7 Then
                tbYear.Text = Year(Now).ToString & " - " & (Year(Now) + 1).ToString
            Else
                tbYear.Text = (Year(Now) - 1).ToString & " - " & Year(Now).ToString
            End If
            ddStudent.DataBind()

        End If

    End Sub

    Private Sub cmbTeacher_DataBound(sender As Object, e As EventArgs) Handles cmbTeacher.DataBound
        If (TypeOf DataItem Is Telerik.Web.UI.GridInsertionObject) Then
            If Not cmbTeacher.FindItemByValue(DirectCast(Me.Page, csp_list).usrUuid) Is Nothing Then
                cmbTeacher.SelectedValue = DirectCast(Me.Page, csp_list).usrUuid
            End If
        ElseIf Not DataItem Is Nothing Then
            Dim oItem As New RadListBoxItem
            If Not cmbAllTeachers.FindItemByValue(DataBinder.Eval(DataItem, "usr_uuid").ToString) Is Nothing Then
                If Not IsDBNull(DataBinder.Eval(DataItem, "usr_uuid").ToString) Then
                    oItem.Value = DataBinder.Eval(DataItem, "usr_uuid").ToString
                    oItem.Text = cmbAllTeachers.FindItemByValue(oItem.Value).Text
                    lbTeachers.Items.Add(oItem)
                End If
            End If

            For intCounter = 2 To 5
                If Not IsDBNull(DataBinder.Eval(DataItem, "teacher_" & intCounter & "_uuid")) Then
                    If Not cmbAllTeachers.FindItemByValue(DataBinder.Eval(DataItem, "teacher_" & intCounter & "_uuid").ToString) Is Nothing Then
                        oItem = New RadListBoxItem
                        oItem.Value = DataBinder.Eval(DataItem, "teacher_" & intCounter & "_uuid").ToString
                        oItem.Text = cmbAllTeachers.FindItemByValue(oItem.Value).Text
                        lbTeachers.Items.Add(oItem)
                    End If
                End If
            Next
        End If

    End Sub

    Private Sub ddStudent_DataBound(sender As Object, e As EventArgs) Handles ddStudent.DataBound

        If (Not TypeOf DataItem Is Telerik.Web.UI.GridInsertionObject) Then
            Dim studentVal As Object = DataBinder.Eval(DataItem, "student_uuid")
            Try
                ddStudent.SelectedValue = studentVal.ToString
            Catch
            End Try
        End If

        ddStudent.ExpandAllDropDownNodes()

    End Sub

    Protected Sub ddSubjects_DataBound(sender As Object, e As EventArgs) Handles ddSubjects.DataBound

        If (Not TypeOf DataItem Is Telerik.Web.UI.GridInsertionObject) Then
            Dim subjectVal As Object = DataBinder.Eval(DataItem, "subject_uuid")
            Try
                ddSubjects.SelectedValue = subjectVal.ToString
                If ddSubjects.SelectedText Like "*Other (specify subject name below)" Then
                    tbSubject_Div.Style.Item("display") = "block"
                End If

            Catch
            End Try
        End If

        'ddSubjects.ExpandAllDropDownNodes()
    End Sub
End Class