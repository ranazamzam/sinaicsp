Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports System.Xml
Imports System.Xml.Xsl
Imports System.Drawing.Text
Imports System.Drawing
Imports Fonet
Imports Fonet.Render
Imports Fonet.Render.Pdf

Public Class GeneratePDF2
    Inherits System.Web.UI.Page


    ';WITH Reorder AS (SELECT iep_data_uuid, row_number() OVER (ORDER BY iep_data_sequence ) AS seq FROM iep_data WHERE iep_uuid = 'BD2C33F8-6710-4134-AAA9-AB22C3B361AB' AND iep_data_parent_uuid IS NULL)
    'UPDATE dataTable SET iep_data_sequence = seq FROM iep_data dataTable INNER JOIN Reorder ON dataTable.iep_data_uuid = Reorder.iep_data_uuid

    'SELECT iep_data_sequence, char(64 + iep_sub_data_sequence) AS iep_sub_data_sequence, iep_data_text, iep_date_initiated, grade_1, grade_2, grade_3 FROM vw_get_iep_data WHERE iep_uuid = 'BD2C33F8-6710-4134-AAA9-AB22C3B361AB' ORDER BY 1, 2

    '    SELECT display_name AS student_name, school_name, iep_subject, iep_materials, usr_display_name AS teacher_name, iep_year, iep_comments, iep_feb_notes, iep_june_notes, iep_data_sequence, char(64 + iep_sub_data_sequence) AS iep_sub_data_sequence, iep_data_text, iep_date_initiated, grade_1, grade_2, grade_3
    '	FROM vw_list_iep AS sinai_iep INNER JOIN vw_get_iep_data AS iep_goals ON sinai_iep.iep_uuid = iep_goals.iep_uuid
    'WHERE sinai_iep.iep_uuid = 'BD2C33F8-6710-4134-AAA9-AB22C3B361AB'
    'ORDER BY iep_data_sequence, iep_sub_data_sequence
    'FOR XML AUTO

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack Then Exit Sub

        'Get iep_uuid from url
        Dim pdf_uuid As String = Request.QueryString("pdf_uuid")
        
        Dim strQuery As String = "getPdfHebrew @pdf_uuid"
        
        Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
        Dim sqlComm As New SqlCommand()

        sqlComm = New SqlCommand(strQuery, sqlConn)

        sqlComm.Parameters.Add(New SqlParameter("@pdf_uuid", SqlDbType.UniqueIdentifier))
        sqlComm.Parameters("@pdf_uuid").Value = New Guid(pdf_uuid)

        sqlConn.Open()

        Dim sqlAdapter As New SqlDataAdapter()

        sqlAdapter.SelectCommand = sqlComm

        Dim tblData As New DataTable()

        sqlAdapter.Fill(tblData)

        Dim foDriver As FonetDriver = FonetDriver.Make()
        Dim foOptions As New PdfRendererOptions

        Response.Write("<html><head><title></title></head><body>")

        AddHandler foDriver.OnError, AddressOf FonetError

        foOptions.FontType = FontType.Subset
        foOptions.AddPrivateFont(New FileInfo(Server.MapPath("ref/ARIAL.TTF")))
        foOptions.Kerning = True
        foDriver.Options = foOptions

        Dim foXml As New XmlDocument
        Dim tmpXml As New XmlDocument

        foXml.LoadXml(tblData.Rows(0).Item("pdf_fo"))

        Dim pdfMemStream As New MemoryStream

        'foDriver.Render(foXml, "")
        foDriver.Render(foXml, pdfMemStream)

        Dim pdfBuffer As Byte() = pdfMemStream.ToArray
        Dim pdfLength As Integer = pdfBuffer.Length

        Response.Clear()
        Response.ContentType = "application/pdf"
        Response.AddHeader("Content-Length", pdfLength.ToString())
        'Response.AddHeader("Accept-Ranges", "bytes")
        Response.AddHeader("Accept-Header", pdfLength.ToString())

        Response.OutputStream.Write(pdfBuffer, 0, pdfLength)
        'Response.Write("</body></html>")
        Response.End()

        sqlConn.Close()

    End Sub

    Public Sub FonetError(Driver As Object, e As FonetEventArgs)
        ' Log message to disk
        Response.Write("[ERROR] " & e.GetMessage() & "<br/>")
    End Sub

End Class