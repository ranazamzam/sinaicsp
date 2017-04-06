Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports System.Xml
Imports System.Xml.Xsl
Imports Fonet
Imports Fonet.Render
Imports Fonet.Render.Pdf

Public Class GeneratePDF
    Inherits System.Web.UI.Page


    ';WITH Reorder AS (SELECT iep_data_uuid, row_number() OVER (ORDER BY iep_data_sequence ) AS seq FROM iep_data WHERE iep_uuid = 'BD2C33F8-6710-4134-AAA9-AB22C3B361AB' AND iep_data_parent_uuid IS NULL)
    'UPDATE dataTable SET iep_data_sequence = seq FROM iep_data dataTable INNER JOIN Reorder ON dataTable.iep_data_uuid = Reorder.iep_data_uuid

    'SELECT iep_data_sequence, char(64 + iep_sub_data_sequence) AS iep_sub_data_sequence, iep_data_text, iep_date_initiated, grade_1, grade_2, grade_3 FROM vw_get_iep_data WHERE iep_uuid = 'BD2C33F8-6710-4134-AAA9-AB22C3B361AB' ORDER BY 1, 2

    '    SELECT display_name AS student_name, school_name, iep_subject, iep_materials, usr_display_name AS teacher_name, iep_year, iep_comments, iep_feb_notes, iep_june_notes, iep_data_sequence, char(64 + iep_sub_data_sequence) AS iep_sub_data_sequence, iep_data_text, iep_date_initiated, grade_1, grade_2, grade_3
    '	FROM vw_list_iep AS sinai_iep INNER JOIN vw_get_iep_data AS iep_goals ON sinai_iep.iep_uuid = iep_goals.iep_uuid
    'WHERE sinai_iep.iep_uuid = 'BD2C33F8-6710-4134-AAA9-AB22C3B361AB'
    'ORDER BY iep_data_sequence, iep_sub_data_sequence
    'FOR XML AUTO
    Dim blnIsHebrew As Boolean = False

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack Then Exit Sub

        'Get iep_uuid from url
        Dim iep_uuid As String = Request.QueryString("iep_uuid")
        Dim student_uuid As String = Request.QueryString("student_uuid")
        Dim strIepUuid As String = ""
        Dim tblRow As DataRow
        
        'If Not student_uuid Is Nothing Then
        '    Response.Write("PDF all student CSPs")
        '    Exit Sub
        'End If

        Dim strQuery As String = "getIepForPrinting @usr_login, @iep_uuid, @student_uuid"
        '" FOR XML AUTO"

        Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
        Dim sqlComm As New SqlCommand()

        Dim foDriver As FonetDriver = FonetDriver.Make()
        Dim foOptions As New PdfRendererOptions

        foOptions.FontType = FontType.Subset
        foOptions.AddPrivateFont(New FileInfo(Server.MapPath("ref/ARIAL.TTF")))
        foOptions.Kerning = False
        foDriver.Options = foOptions

        Dim foXml As New XmlDocument
        Dim tmpXml As New XmlDocument
        Dim tblData As New DataTable()


        Dim sqlAdapter As New SqlDataAdapter()
        sqlConn.Open()


        Dim s As String = ""
        If student_uuid Is Nothing Then
            foXml.Load(Server.MapPath("templates/document_shell_no_cover.xml"))
        Else
            foXml.Load(Server.MapPath("templates/document_shell.xml"))
            'Get student header info
            sqlComm = New SqlCommand("getStudentHeader @student_uuid", sqlConn)
            sqlComm.Parameters.Add(New SqlParameter("@student_uuid", SqlDbType.UniqueIdentifier))
            sqlComm.Parameters("@student_uuid").Value = New Guid(student_uuid)
            sqlAdapter.SelectCommand = sqlComm

            sqlAdapter.Fill(tblData)
            tblRow = tblData.Rows(0)
            tmpXml.Load(Server.MapPath("templates/" & tblData.Rows(0).Item("school_name") & "_cover.xml"))
            s = tmpXml.InnerXml
            s = s.Replace("@@student_name@@", tblRow.Item("student_name"))
            s = s.Replace("@@school_year@@", tblRow.Item("school_year"))
            s = s.Replace("@@parents@@", tblRow.Item("parents"))
            s = s.Replace("@@dob@@", tblRow.Item("date_of_birth"))
            s = s.Replace("@@age@@", tblRow.Item("age"))
            s = s.Replace("@@grade@@", tblRow.Item("student_grade"))
            s = s.Replace("@@address@@", tblRow.Item("home_address"))
            s = s.Replace("@@gender@@", tblRow.Item("gender"))
            s = s.Replace("@@city_state@@", tblRow.Item("home_city"))
            s = s.Replace("@@phone@@", tblRow.Item("home_phone"))
            s = s.Replace("@@father_cell@@", tblRow.Item("father_cell"))
            s = s.Replace("@@mother_cell@@", tblRow.Item("mother_cell"))

            Dim oldServices As Boolean = (tblRow.Item("school_year") = "2013 - 2014")

            tmpXml.LoadXml(s)
            'Get services (if any)
            'Get inclusions (if any)
            Dim newNode As XmlNode = foXml.ImportNode(tmpXml.DocumentElement, True)
            foXml.DocumentElement.AppendChild(newNode)
            Dim t As String

            tblData = New DataTable()

            If oldServices Then
                sqlComm = New SqlCommand("SELECT * from student_services WHERE student_uuid = '" & student_uuid & "' ORDER BY services_name, services_parent, CONVERT(SMALLDATETIME, REPLACE(session_start_date, '/', '/1/'))", sqlConn)
            Else
                sqlComm = New SqlCommand("SELECT services.*, student_uuid, provider_name FROM services INNER JOIN service_providers ON service_providers.providers_uuid = services.provider_uuid INNER JOIN services_students ON services_students.services_uuid = services.services_uuid WHERE student_uuid = '" & student_uuid & "' ORDER BY services_name, services_parent, CONVERT(SMALLDATETIME, REPLACE(session_start_date, '/', '/1/'))", sqlConn)
            End If
            sqlAdapter.SelectCommand = sqlComm

            sqlAdapter.Fill(tblData)
            If tblData.Rows.Count <> 0 Then
                t = "<fo:page-sequence master-reference=""PageMaster-CoverSheet"" xmlns:fo=""http://www.w3.org/1999/XSL/Format"">"
                t = t & s.Substring(s.IndexOf("<fo:static-content"), s.IndexOf("</fo:static-content>") - s.IndexOf("<fo:static-content") + 20)
                t = t.Replace("</fo:static-content>", "<fo:block font-size=""14pt"" font-weight=""bold"" keep-together=""always"" text-align=""center"" padding-top=""20pt"">Services for " & tblRow.Item("student_name") & "</fo:block></fo:static-content>")
                t = t & "<fo:flow flow-name=""xsl-region-body""><fo:block font-family=""Helvetica"" font-weight=""normal"" font-size=""10pt"" padding-top=""30pt"">"
                t = t & GetServices(tblData)
                t = t & "</fo:block></fo:flow>"
                t = t & "</fo:page-sequence>"

                tmpXml.LoadXml(t)
                newNode = foXml.ImportNode(tmpXml.DocumentElement, True)
                foXml.DocumentElement.AppendChild(newNode)
            End If

            tblData = New DataTable()

            sqlComm = New SqlCommand("SELECT * from student_inclusions WHERE student_uuid = '" & student_uuid & "' ORDER BY subject_name", sqlConn)
            sqlAdapter.SelectCommand = sqlComm

            sqlAdapter.Fill(tblData)
            If tblData.Rows.Count <> 0 Then
                t = "<fo:page-sequence master-reference=""PageMaster-CoverSheet"" xmlns:fo=""http://www.w3.org/1999/XSL/Format"">"
                t = t & s.Substring(s.IndexOf("<fo:static-content"), s.IndexOf("</fo:static-content>") - s.IndexOf("<fo:static-content") + 20)
                t = t.Replace("</fo:static-content>", "<fo:block font-size=""14pt"" font-weight=""bold"" keep-together=""always"" text-align=""center"" padding-top=""20pt"">Inclusion for " & tblRow.Item("student_name") & "</fo:block></fo:static-content>")
                t = t & "   <fo:flow flow-name=""xsl-region-body""><fo:block font-family=""Helvetica"" font-weight=""normal"" font-size=""10pt"" padding-top=""30pt"">"
                t = t & GetInclusions(tblData)
                t = t & "</fo:block></fo:flow>"
                t = t & "</fo:page-sequence>"

                tmpXml.LoadXml(t)
                newNode = foXml.ImportNode(tmpXml.DocumentElement, True)
                foXml.DocumentElement.AppendChild(newNode)
            End If

            tblData = New DataTable()

            sqlComm = New SqlCommand("SELECT * from student_accommodations WHERE student_uuid = '" & student_uuid & "' ORDER BY accommodation_details", sqlConn)
            sqlAdapter.SelectCommand = sqlComm

            sqlAdapter.Fill(tblData)
            If tblData.Rows.Count <> 0 Then
                t = "<fo:page-sequence master-reference=""PageMaster-CoverSheet"" xmlns:fo=""http://www.w3.org/1999/XSL/Format"">"
                t = t & s.Substring(s.IndexOf("<fo:static-content"), s.IndexOf("</fo:static-content>") - s.IndexOf("<fo:static-content") + 20)
                t = t.Replace("</fo:static-content>", "<fo:block font-size=""14pt"" font-weight=""bold"" keep-together=""always"" text-align=""center"" padding-top=""20pt"">Accommodations for " & tblRow.Item("student_name") & "</fo:block></fo:static-content>")
                t = t & "<fo:flow flow-name=""xsl-region-body""><fo:block font-family=""Helvetica"" font-weight=""normal"" font-size=""10pt"" padding-top=""30pt"">"
                t = t & "<fo:list-block >"
                For Each rowData As DataRow In tblData.Rows
                    t = t & "<fo:list-item><fo:list-item-label end-indent=""label-end()""><fo:block><fo:inline font-size=""30pt"" font-weight=""bold"">.</fo:inline></fo:block></fo:list-item-label><fo:list-item-body start-indent=""body-start()""><fo:block>" & rowData.Item("accommodation_details").ToString.Replace("&", "&amp;") & "</fo:block></fo:list-item-body></fo:list-item>"
                Next
                t = t & "</fo:list-block >"
                t = t & "</fo:block></fo:flow>"
                t = t & "</fo:page-sequence>"

                tmpXml.LoadXml(t)
                newNode = foXml.ImportNode(tmpXml.DocumentElement, True)
                foXml.DocumentElement.AppendChild(newNode)
            End If

            tblData = New DataTable()

            sqlComm = New SqlCommand("SELECT subject_text, usr_display_name FROM vw_list_iep WHERE student_uuid = '" & student_uuid & "' AND is_active = 1 ORDER BY subject_order, subject_text", sqlConn)
            sqlAdapter.SelectCommand = sqlComm

            sqlAdapter.Fill(tblData)

            If tblData.Rows.Count <> 0 Then
                t = "<fo:page-sequence master-reference=""PageMaster-CoverSheet"" xmlns:fo=""http://www.w3.org/1999/XSL/Format"">"
                t = t & s.Substring(s.IndexOf("<fo:static-content"), s.IndexOf("</fo:static-content>") - s.IndexOf("<fo:static-content") + 20)
                t = t.Replace("</fo:static-content>", "<fo:block font-size=""14pt"" font-weight=""bold"" keep-together=""always"" text-align=""center"" padding-top=""20pt"">Table of Contents</fo:block></fo:static-content>")
                t = t & "<fo:flow flow-name=""xsl-region-body""><fo:block font-family=""Helvetica"" font-weight=""normal"" font-size=""10pt"" padding-top=""40pt"">"
                t = t & "<fo:block text-align=""center""><fo:table text-align=""center"" table-layout=""fixed""><fo:table-column column-width=""2.5cm""/><fo:table-column column-width=""4cm""/><fo:table-column column-width=""11cm""/><fo:table-body>"
                t = t & "<fo:table-row><fo:table-cell/><fo:table-cell border=""1pt solid black"" display-align=""center"" padding=""3pt""><fo:block font-weight=""bold"" >Subject</fo:block></fo:table-cell><fo:table-cell border=""1pt solid black"" display-align=""center"" padding=""3pt""><fo:block font-weight=""bold"" >Teacher</fo:block></fo:table-cell></fo:table-row>"

                For Each rowData As DataRow In tblData.Rows
                    t = t & "<fo:table-row border=""1pt solid black"" font-family=""Arial""><fo:table-cell/><fo:table-cell border=""1pt solid black"" display-align=""center"" padding=""3pt""><fo:block text-align=""left"">" & FixHebrew(rowData.Item("subject_text")).ToString.Replace("&", "&amp;") & "</fo:block></fo:table-cell><fo:table-cell border=""1pt solid black"" display-align=""center"" padding=""3pt""><fo:block text-align=""left"">" & rowData.Item("usr_display_name") & "</fo:block></fo:table-cell></fo:table-row>"
                    't = t & "<fo:list-item><fo:list-item-label end-indent=""label-end()""><fo:block><fo:inline font-size=""30pt"" font-weight=""bold"">.</fo:inline></fo:block></fo:list-item-label><fo:list-item-body start-indent=""body-start()""><fo:block>" & rowData.Item("subject_text") & "</fo:block></fo:list-item-body></fo:list-item>"
                Next
                t = t & "</fo:table-body></fo:table></fo:block>"
                t = t & "</fo:block></fo:flow>"
                t = t & "</fo:page-sequence>"

                tmpXml.LoadXml(t)
                newNode = foXml.ImportNode(tmpXml.DocumentElement, True)
                foXml.DocumentElement.AppendChild(newNode)
            End If


        End If


        s = ""
        sqlComm = New SqlCommand(strQuery, sqlConn)

        sqlComm.Parameters.Add(New SqlParameter("@usr_login", SqlDbType.VarChar))
        sqlComm.Parameters("@usr_login").Value = Context.User.Identity.Name

        sqlComm.Parameters.Add(New SqlParameter("@iep_uuid", SqlDbType.UniqueIdentifier))
        sqlComm.Parameters.Add(New SqlParameter("@student_uuid", SqlDbType.UniqueIdentifier))

        If student_uuid Is Nothing Then
            sqlComm.Parameters("@student_uuid").Value = DBNull.Value
            sqlComm.Parameters("@iep_uuid").Value = New Guid(iep_uuid)
        Else
            sqlComm.Parameters("@student_uuid").Value = New Guid(student_uuid)
            sqlComm.Parameters("@iep_uuid").Value = DBNull.Value
        End If


        sqlAdapter.SelectCommand = sqlComm

        sqlAdapter.Fill(tblData)

        For Each tblRow In tblData.Rows
            If tblRow.Item("iep_uuid").ToString <> strIepUuid Then
                strIepUuid = tblRow.Item("iep_uuid").ToString
                tmpXml.Load(Server.MapPath("templates/" & tblRow.Item("school_name") & ".xml"))

                s = tmpXml.InnerXml
                s = s.Replace("@@school_year@@", tblRow.Item("iep_year"))
                s = s.Replace("@@logo_path@@", "http://sinaicsp.azurewebsites.net/")
                s = s.Replace("@@student_name@@", tblRow.Item("student_name"))
                s = s.Replace("@@iep_subject@@", FixHebrew(tblRow.Item("iep_subject")))
                s = s.Replace("@@teacher_name@@", tblRow.Item("teacher_name"))
                s = s.Replace("@@iep_materials@@", FixHebrew(tblRow.Item("iep_materials").ToString.Replace("&", "&amp;")))
                If tblRow.Item("iep_materials").ToString.Length > 150 Then
                    s = s.Replace("<fo:table-column column-width=""4.75in"" />", "<fo:table-column column-width=""3.65in"" />")
                    s = s.Replace("<fo:table-column column-width=""3.75in"" />", "<fo:table-column column-width=""4.85in"" />")
                End If
                s = s.Replace("@@year_start@@", Mid(tblRow.Item("iep_year"), 3, 2))
                s = s.Replace("@@year_end@@", Right(tblRow.Item("iep_year"), 2))
                s = s.Replace("@@student_first_name@@", Trim(Left(tblRow.Item("student_name"), InStr(tblRow.Item("student_name"), " "))))
                s = s.Replace("@@date_stamp@@", Format(Now, "MM/dd/yyyy"))

                Dim strComments As String = ""
                Dim xmlComments As New XmlDocument
                Dim xmlCommentTemplate As New XmlDocument

                xmlCommentTemplate.Load(Server.MapPath("templates/comments_notes.xml"))
                Dim strCommentsTemplate As String = xmlCommentTemplate.InnerXml

                If Not IsDBNull(tblRow.Item("iep_comments")) Then
                    If Trim(tblRow.Item("iep_comments")) <> "" Then
                        strComments = strCommentsTemplate.Replace("@@comments@@", HtmlToFo("<div><strong>Comments: </strong><br/>" & FixHebrew(tblRow.Item("iep_comments").ToString.Replace("&nbsp;", " ")) & "</div>"))
                    End If
                End If

                If Not IsDBNull(tblRow.Item("iep_feb_notes")) Then
                    If Trim(tblRow.Item("iep_feb_notes")) <> "" Then
                        strComments = strComments & strCommentsTemplate.Replace("@@comments@@", HtmlToFo("<div><strong>February Progress Update: </strong><br/>" & FixHebrew(tblRow.Item("iep_feb_notes").ToString.Replace("&nbsp;", " ")) & "</div>"))
                    End If
                End If

                If Not IsDBNull(tblRow.Item("iep_june_notes")) Then
                    If Trim(tblRow.Item("iep_june_notes")) <> "" Then
                        strComments = strComments & strCommentsTemplate.Replace("@@comments@@", HtmlToFo("<div><strong>June Progress Update: </strong><br/>" & FixHebrew(tblRow.Item("iep_june_notes").ToString.Replace("&nbsp;", " ")) & "</div>"))
                    End If
                End If

                strComments = "<fo:block text-align=""justify"" font-size=""10pt"" font-family=""Arial"">" & strComments & "</fo:block>"

                s = s.Replace("@@COMMENTS_NOTES@@", strComments)

                If Not IsDBNull(tblRow.Item("iep_data_text")) Then
                    s = s.Replace("@@GOALTABLE@@", GetGoals(strIepUuid, tblData))
                End If
                tmpXml.LoadXml(s)
                Dim newNode As XmlNode = foXml.ImportNode(tmpXml.DocumentElement, True)
                foXml.DocumentElement.AppendChild(newNode)
            End If
        Next

        'If blnIsHebrew = True Then
        strQuery = "doPdfHebrew @pdf_uuid, @pdf_fo"
        sqlComm = New SqlCommand(strQuery, sqlConn)

        sqlComm.Parameters.Add(New SqlParameter("@pdf_fo", SqlDbType.NVarChar))
        sqlComm.Parameters("@pdf_fo").Value = foXml.InnerXml

        sqlComm.Parameters.Add(New SqlParameter("@pdf_uuid", SqlDbType.UniqueIdentifier))
        If student_uuid Is Nothing Then
            sqlComm.Parameters("@pdf_uuid").Value = New Guid(iep_uuid)
        Else
            sqlComm.Parameters("@pdf_uuid").Value = New Guid(student_uuid)
        End If

        sqlComm.ExecuteNonQuery()

        Response.Redirect(System.Configuration.ConfigurationManager.AppSettings("heburl").ToString() & "GeneratePDF2.aspx?pdf_uuid=" & sqlComm.Parameters("@pdf_uuid").Value.ToString)

        'Else
        '    Dim pdfMemStream As New MemoryStream

        '    AddHandler foDriver.OnError, AddressOf FonetError

        '    foDriver.Render(foXml, pdfMemStream)

        '    Dim pdfBuffer As Byte() = pdfMemStream.ToArray
        '    Dim pdfLength As Integer = pdfBuffer.Length

        '    Response.Clear()
        '    Response.ContentType = "application/pdf"
        '    Response.AddHeader("Content-Length", pdfLength.ToString())
        '    Response.AddHeader("Accept-Ranges", "bytes")
        '    Response.AddHeader("Accept-Header", pdfLength.ToString())

        '    Response.OutputStream.Write(pdfBuffer, 0, pdfLength)
        'End If

        Response.End()

        sqlConn.Close()

    End Sub

    Private Shared Function ToByteArray(source As [SByte]()) As Byte()
        Dim bytes As Byte() = New Byte(source.Length - 1) {}
        System.Buffer.BlockCopy(source, 0, bytes, 0, source.Length)
        Return bytes
    End Function

    Public Sub FonetError(Driver As Object, e As FonetEventArgs)
        ' Log message to disk
        Debug.Print("[ERROR] " & e.GetMessage())
    End Sub

    Private Function GetInclusions(tblData As DataTable) As String
        Dim strInclusions As String = ""
        Dim strInclusion As String = ""
        Dim xmlInclusionsTemplate As New XmlDocument
        Dim xmlInclusionsDetailsTemplate As New XmlDocument

        xmlInclusionsTemplate.Load(Server.MapPath("templates/inclusions.xml"))
        xmlInclusionsDetailsTemplate.Load(Server.MapPath("templates/inclusion_details.xml"))

        Dim strInclusionsTemplate As String = xmlInclusionsTemplate.InnerXml
        Dim strInclusionsDetailsTemplate As String = xmlInclusionsDetailsTemplate.InnerXml

        For Each rowData As DataRow In tblData.Rows
            strInclusion = strInclusionsDetailsTemplate.Replace("@@subject@@", rowData.Item("subject_name").ToString.Replace("&", "&amp;"))
            strInclusion = strInclusion.Replace("@@classes@@", rowData.Item("num_classes"))
            strInclusion = strInclusion.Replace("@@teacher@@", rowData.Item("teacher_name").ToString.Replace("&", "&amp;"))
            strInclusion = strInclusion.Replace("@@start_date@@", rowData.Item("session_start_date"))
            strInclusion = strInclusion.Replace("@@end_date@@", rowData.Item("session_end_date").ToString)
            strInclusions = strInclusions & Environment.NewLine & strInclusion
        Next

        'strInclusionsTemplate = strInclusionsTemplate.Replace("@@student_name@@", UCase(studentName))
        GetInclusions = strInclusionsTemplate.Replace("@@inclusion_details@@", strInclusions)

    End Function

    Private Function GetServices(tblData As DataTable) As String
        Dim strServices As String = ""
        Dim strService As String = ""
        Dim strLastServiceId As String = ""
        Dim xmlServiceTemplate As New XmlDocument
        Dim xmlServiceDetailsTemplate As New XmlDocument
        
        xmlServiceTemplate.Load(Server.MapPath("templates/services.xml"))
        xmlServiceDetailsTemplate.Load(Server.MapPath("templates/services_details.xml"))

        Dim strServicesTemplate As String = xmlServiceTemplate.InnerXml
        Dim strServicesDetailsTemplate As String = xmlServiceDetailsTemplate.InnerXml

        For Each rowData As DataRow In tblData.Rows
            If strLastServiceId <> rowData.Item("services_parent").ToString Then
                strLastServiceId = rowData.Item("services_parent").ToString
                strServices = strServices.Replace("@@details@@", strService)
                strService = strServicesTemplate.Replace("@@service@@", rowData.Item("services_name").ToString.Replace("&", "&amp;"))
                strService = strService.Replace("@@model@@", rowData.Item("service_model"))
                strService = strService.Replace("@@provider@@", rowData.Item("provider_name").ToString.Replace("&", "&amp;"))
                strServices = strServices & Environment.NewLine & strService
                strService = ""
            End If
            strService = strService & Environment.NewLine & strServicesDetailsTemplate
            strService = strService.Replace("@@students@@", rowData.Item("num_students"))
            strService = strService.Replace("@@length@@", rowData.Item("session_length"))
            strService = strService.Replace("@@sessions@@", rowData.Item("weekly_sessions"))
            strService = strService.Replace("@@start_date@@", rowData.Item("session_start_date"))
            strService = strService.Replace("@@end_date@@", rowData.Item("session_end_date").ToString)
        Next

        strServices = strServices.Replace("@@details@@", strService)

        GetServices = strServices

    End Function

    Private Function GetGoals(strIepUuid As String, tblData As DataTable) As String
        Dim strGoals As String = ""
        Dim strGoal As String
        Dim xmlGoal As New XmlDocument
        Dim xmlGoalTemplate As New XmlDocument
        Dim strSeq As String
        Dim dRows() As DataRow

        xmlGoalTemplate.Load(Server.MapPath("templates/goal_table.xml"))
        Dim strGoalTemplate As String = xmlGoalTemplate.InnerXml

        dRows = tblData.Select("[iep_uuid] = '" & strIepUuid & "'")

        For Each rowData As DataRow In dRows
            If Not IsDBNull(rowData.Item("iep_sub_data_sequence")) Then
                strSeq = rowData.Item("iep_sub_data_sequence")
            Else
                strSeq = rowData.Item("iep_data_sequence")
            End If
            strGoal = strGoalTemplate.Replace("@@iep_data_text@@", HtmlToFo("<div>" & strSeq & ". " & FixHebrew(rowData.Item("iep_data_text")) & "</div>"))
            If Not IsNumeric(strSeq) Then strGoal = strGoal.Replace("start-indent=""0.05in""", "start-indent=""0.25in""")
            strGoal = strGoal.Replace("@@date_initiated@@", IIf(IsDBNull(rowData.Item("iep_date_initiated")), "", rowData.Item("iep_date_initiated")))
            strGoal = strGoal.Replace("@@grade_1@@", rowData.Item("grade_1"))
            strGoal = strGoal.Replace("@@grade_2@@", rowData.Item("grade_2"))
            strGoal = strGoal.Replace("@@grade_3@@", rowData.Item("grade_3"))
            strGoals = strGoals & Environment.NewLine & strGoal
        Next

        GetGoals = strGoals

    End Function

    Private Function HtmlToFo(strOrgValue As String) As String

        Dim xmlDoc As New XmlDocument
        Dim xmlNav As XPath.XPathNavigator
        Dim xmlOut As New XmlDocument
        Dim xWriterSettings As New XmlWriterSettings
        Dim xslHtmlToFo As New XslCompiledTransform

        xslHtmlToFo.Load(Server.MapPath("templates/htmltofo.xslt"))
        xWriterSettings.ConformanceLevel = ConformanceLevel.Auto

        strOrgValue = strOrgValue.Replace("&ndash;", "-")
        strOrgValue = strOrgValue.Replace("&nbsp;", " ")
        strOrgValue = strOrgValue.Replace("&ldquo;", """")
        strOrgValue = strOrgValue.Replace("&rdquo;", """")
        strOrgValue = strOrgValue.Replace("&rsquo;", "'")
        strOrgValue = strOrgValue.Replace("&lsquo;", "'")
        strOrgValue = strOrgValue.Replace("&middot;", "")
        strOrgValue = strOrgValue.Replace("&ensp;", " ")
        strOrgValue = strOrgValue.Replace("&emsp;", " ")
        strOrgValue = strOrgValue.Replace("&divide;", "÷")
        strOrgValue = strOrgValue.Replace("&hellip;", "...")

        strOrgValue = (New Regex("&(?![a-zA-Z]{2,6};|#[0-9]{2,4};)")).Replace(strOrgValue, "&amp;")

        'strOrgValue = strOrgValue.Replace("<span style=""text-decoration: underline;"">", "<u>").Replace("</span>", "</u>")

        xmlDoc.LoadXml(strOrgValue)
        xmlNav = xmlDoc.CreateNavigator
        Using xWriter As XmlWriter = XmlWriter.Create(xmlOut.CreateNavigator().AppendChild(), xWriterSettings)
            xslHtmlToFo.Transform(xmlNav, xWriter)
        End Using

        HtmlToFo = xmlOut.InnerXml

    End Function

    Private Function FixHebrew(strData As String) As String
        Dim iCnt As Integer
        Dim tCnt As Integer
        Dim jCnt As Integer
        Dim kCnt As Integer
        
        If hasHebrew(strData) Then
            'strData = Regex.Replace(strData, "[\u0000-\u0019,\u0021-\u007F]", String.Empty)
            'strData = "@@ " & strData & " @@"
            strData = strData.Replace("<", " <").Replace(">", "> ")
            Dim hebArray As String() = strData.Split(New [Char]() {" "c})
            Dim tmpArray As String() = hebArray.Clone

            For iCnt = 0 To hebArray.Length - 1
                If hasHebrew(hebArray(iCnt)) Then
                    tCnt = tCnt + 1
                    If hasNonHebrew(hebArray(iCnt)) Then
                        hebArray(iCnt) = FlipMixedString(hebArray(iCnt))
                        tmpArray(iCnt) = hebArray(iCnt)
                    Else
                        hebArray(iCnt) = FlipString(hebArray(iCnt))
                        tmpArray(iCnt) = hebArray(iCnt)
                    End If
                Else
                    If tCnt > 1 Then
                        tCnt = tCnt - 1
                        kCnt = iCnt - 1
                        For jCnt = iCnt - tCnt - 1 To iCnt - 1
                            hebArray(jCnt) = tmpArray(kCnt)
                            kCnt = kCnt - 1
                        Next
                        tCnt = 0
                    End If
                    tCnt = 0
                End If
            Next
            iCnt = iCnt - 1
            If tCnt > 1 Then
                tCnt = tCnt - 1
                kCnt = iCnt

                For jCnt = iCnt - tCnt To iCnt
                    hebArray(jCnt) = tmpArray(kCnt)
                    kCnt = kCnt - 1
                Next

                If Right(hebArray(iCnt - tCnt), 1) = "." Then
                    hebArray(iCnt - tCnt) = Left(hebArray(iCnt - tCnt), Len(hebArray(iCnt - tCnt)) - 1)
                    hebArray(iCnt) = hebArray(iCnt) & "."
                End If
                tCnt = 0
            End If

            FixHebrew = Join(hebArray, " ")
        Else
            FixHebrew = strData
        End If
    End Function

    Private Function FlipMixedString(strData As String) As String
        Dim hebArray As Array = strData.ToCharArray()
        Dim tmpArray As Array = hebArray
        Dim iCnt As Integer
        Dim tCnt As Integer = 0
        Dim jCnt As Integer

        tmpArray = FlipString(New String(tmpArray)).ToCharArray()

        For iCnt = 0 To hebArray.Length - 1
            If Regex.Replace(hebArray(iCnt), "[^\u0000-\u007F]", String.Empty).Length = 0 Then
                'Is hebrew
                For tCnt = jCnt To tmpArray.Length - 1
                    If Regex.Replace(tmpArray(tCnt), "[^\u0000-\u007F]", String.Empty).Length = 0 Then
                        hebArray(iCnt) = tmpArray(tCnt)
                        Exit For
                    End If
                Next
                jCnt = tCnt + 1
            End If
        Next

        FlipMixedString = New String(hebArray)

    End Function

    Private Function FlipString(strData As String) As String
        Dim hebArray As Array = strData.ToCharArray()
        Array.Reverse(hebArray)
        FlipString = New String(hebArray)
    End Function

    Private Function hasHebrew(strData As String) As Boolean

        If Regex.Replace(strData, "[\u0000-\u007F]", String.Empty).Length > 0 Then
            blnIsHebrew = True
            hasHebrew = True
        Else
            hasHebrew = False
        End If

    End Function

    Private Function hasNonHebrew(strData As String) As Boolean

        If Regex.Replace(strData, "[^\u0000-\u007F]", String.Empty).Length > 0 Then
            hasNonHebrew = True
        Else
            hasNonHebrew = False
        End If

    End Function

End Class