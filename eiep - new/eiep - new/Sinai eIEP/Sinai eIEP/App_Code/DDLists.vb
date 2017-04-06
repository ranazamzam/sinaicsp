Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient

Public Class DDLists

    Private __dtGrades As New DataTable
    Private __dtInitDates As New DataTable
    Private __dtGender As New DataTable

    Public Sub New()

        Dim drGender As DataRow

        __dtGender.Columns.Add("student_gender")

        'drGender = __dtGender.NewRow()
        'drGender("student_gender") = "F"
        __dtGender.Rows.Add("F")
        __dtGender.Rows.Add("M")
        'drGender = __dtGender.NewRow()
        'drGender("student_gender") = "M"

        __dtGrades.Columns.Add("grade_id")
        __dtGrades.Columns.Add("school_id")
        __dtGrades.Columns.Add("grade_text")
        __dtGrades.Columns.Add("grade_desc")

        __dtGrades.Rows.Add(addItem(DBNull.Value, 2, ""))
        __dtGrades.Rows.Add(addItem(3, 2, "DC*"))
        __dtGrades.Rows.Add(addItem(1, 2, "M"))
        __dtGrades.Rows.Add(addItem(2, 2, "P"))

        __dtGrades.Rows.Add(addItem(DBNull.Value, 1, ""))
        __dtGrades.Rows.Add(addItem(4, 1, "1"))
        __dtGrades.Rows.Add(addItem(5, 1, "2"))
        __dtGrades.Rows.Add(addItem(6, 1, "3"))
        __dtGrades.Rows.Add(addItem(7, 1, "4"))
        __dtGrades.Rows.Add(addItem(8, 1, "5"))
        __dtGrades.Rows.Add(addItem(9, 1, "DC*"))

        __dtGrades.Rows.Add(addItem(DBNull.Value, 11, ""))
        __dtGrades.Rows.Add(addItem(11, 11, "1"))
        __dtGrades.Rows.Add(addItem(12, 11, "2"))
        __dtGrades.Rows.Add(addItem(13, 11, "3"))
        __dtGrades.Rows.Add(addItem(14, 11, "4"))
        __dtGrades.Rows.Add(addItem(15, 11, "5"))
        __dtGrades.Rows.Add(addItem(10, 11, "DC*"))

        __dtGrades.Rows.Add(addItem(DBNull.Value, 12, ""))
        __dtGrades.Rows.Add(addItem(17, 12, "1"))
        __dtGrades.Rows.Add(addItem(26, 12, "10"))
        __dtGrades.Rows.Add(addItem(18, 12, "2"))
        __dtGrades.Rows.Add(addItem(19, 12, "3"))
        __dtGrades.Rows.Add(addItem(20, 12, "4"))
        __dtGrades.Rows.Add(addItem(21, 12, "5"))
        __dtGrades.Rows.Add(addItem(22, 12, "6"))
        __dtGrades.Rows.Add(addItem(23, 12, "7"))
        __dtGrades.Rows.Add(addItem(24, 12, "8"))
        __dtGrades.Rows.Add(addItem(25, 12, "9"))
        __dtGrades.Rows.Add(addItem(16, 12, "DC*"))

        __dtGrades.Rows.Add(addItem(DBNull.Value, 13, ""))
        __dtGrades.Rows.Add(28, 13, "1", "1  - Test 1")
        __dtGrades.Rows.Add(29, 13, "2", "2  - Test 2")
        __dtGrades.Rows.Add(30, 13, "3", "3  - Test 3")
        __dtGrades.Rows.Add(31, 13, "4", "4  - Test 4")
        __dtGrades.Rows.Add(32, 13, "5", "5  - Test 5")
        __dtGrades.Rows.Add(27, 13, "DC*", "DC*  - Test DC")

        '__dtGrades.Rows.Add(addItem(DBNull.Value, 1, 1))

        'dr = __dtGrades.NewRow()
        'dr("grade_id") = DBNull.Value
        'dr("school_id") = "2"
        'dr("grade_text") = ""
        '__dtGrades.Rows.Add(dr)

        'dr = __dtGrades.NewRow()
        'dr("grade_id") = "1"
        'dr("school_id") = "2"
        'dr("grade_text") = "M"
        '__dtGrades.Rows.Add(dr)

        'dr = __dtGrades.NewRow()
        'dr("grade_id") = "2"
        'dr("school_id") = "2"
        'dr("grade_text") = "P"
        '__dtGrades.Rows.Add(dr)

        'dr = __dtGrades.NewRow()
        'dr("grade_id") = "3"
        'dr("school_id") = "2"
        'dr("grade_text") = "DC"
        '__dtGrades.Rows.Add(dr)

        'dr = __dtGrades.NewRow()
        'dr("grade_id") = "4"
        'dr("school_id") = "1"
        'dr("grade_text") = "1"
        '__dtGrades.Rows.Add(dr)

        'dr = __dtGrades.NewRow()
        'dr("grade_id") = "5"
        'dr("school_id") = "1"
        'dr("grade_text") = "2"
        '__dtGrades.Rows.Add(dr)

        'dr = __dtGrades.NewRow()
        'dr("grade_id") = "6"
        'dr("school_id") = "1"
        'dr("grade_text") = "3"
        '__dtGrades.Rows.Add(dr)

        'dr = __dtGrades.NewRow()
        'dr("grade_id") = "7"
        'dr("school_id") = "1"
        'dr("grade_text") = "4"
        '__dtGrades.Rows.Add(dr)

        'dr = __dtGrades.NewRow()
        'dr("grade_id") = "8"
        'dr("school_id") = "1"
        'dr("grade_text") = "5"
        '__dtGrades.Rows.Add(dr)

        Dim dr As DataRow

        __dtInitDates.Columns.Add("init_date")

        Dim strYearStart = Right(Year(Now), 2)
        If Month(Now) < 7 Then
            strYearStart = strYearStart - 1
        End If

        dr = __dtInitDates.NewRow()
        dr("init_date") = ""
        __dtInitDates.Rows.Add(dr)
        For i As Integer = 6 To 17
            dr = __dtInitDates.NewRow()
            dr("init_date") = ((i Mod 12) + 1).ToString & "/" & IIf(i >= 12, strYearStart + 1, strYearStart).ToString
            __dtInitDates.Rows.Add(dr)
        Next



    End Sub

    Public Function getGrades(intSchoolId As Integer) As DataTable
        'Return __dtGrades

        Dim tmpGrades As New DataTable
        Dim arGrades() As DataRow = __dtGrades.Select("[school_id] = '" & intSchoolId.ToString() & "'")
        Dim dr As DataRow
        Dim drTmp As DataRow

        tmpGrades.Columns.Add("grade_id")
        tmpGrades.Columns.Add("school_id")
        tmpGrades.Columns.Add("grade_text")

        For Each dr In arGrades
            drTmp = tmpGrades.NewRow()
            drTmp("grade_id") = dr("grade_id")
            drTmp("school_id") = dr("school_id")
            drTmp("grade_text") = dr("grade_text")
            tmpGrades.Rows.Add(drTmp)
        Next

        Return tmpGrades

    End Function

    Public Function getGCCategories() As DataTable
        Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
        Dim sqlAdapter As New SqlDataAdapter()

        sqlAdapter.SelectCommand = New SqlCommand("SELECT * FROM gc_categories ORDER BY gc_category_order", sqlConn)

        Dim gcCaetories As New DataTable()

        sqlConn.Open()
        sqlAdapter.Fill(gcCaetories)

        sqlConn.Close()

        Return gcCaetories

    End Function

    Public Function getInitDates() As DataTable
        Return __dtInitDates
    End Function

    Public Function getInitDatesYr(strYearStart As String) As DataTable
        Dim dr As DataRow

        __dtInitDates.Clear()

        strYearStart = Right(strYearStart, 2) - 1

        dr = __dtInitDates.NewRow()
        dr("init_date") = ""
        __dtInitDates.Rows.Add(dr)
        For i As Integer = 6 To 17
            dr = __dtInitDates.NewRow()
            dr("init_date") = ((i Mod 12) + 1).ToString & "/" & IIf(i >= 12, strYearStart + 1, strYearStart).ToString
            __dtInitDates.Rows.Add(dr)
        Next
        Return __dtInitDates

    End Function

    Public Function getGenderList() As DataTable
        Return __dtGender
    End Function

    Private Function addItem(intGradeId As Object, intSchoolId As Integer, strValue As String) As DataRow
        Dim dr As DataRow
        dr = __dtGrades.NewRow()
        dr("grade_id") = intGradeId
        dr("school_id") = intSchoolId
        dr("grade_text") = strValue
        Return dr
    End Function

End Class