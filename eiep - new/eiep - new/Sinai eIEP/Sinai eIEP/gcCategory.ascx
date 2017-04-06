<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="gcCategory.ascx.vb" Inherits="Sinai_eIEP.gcCategory" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

    <asp:HiddenField runat="server" ID="hdnGCCatId" />

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;"
        SelectCommand="SELECT gc_data_uuid, CASE ISNULL(gc_sub_data_sequence, 0) WHEN 0 THEN gc_data_text ELSE '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + gc_data_text END AS gc_data_text FROM gc_data WHERE gc_subject_uuid = @gc_subject_uuid ORDER BY gc_data_sequence, gc_sub_data_sequence">
        <SelectParameters>
            <asp:ControlParameter ControlID="hdnGCCatId" Name="gc_subject_uuid" PropertyName="Value" />
        </SelectParameters>
    </asp:SqlDataSource>
    
    <telerik:RadListBox ID="lbGoalCat" runat="server" CheckBoxes="true"  AutoPostBack="false" EnableViewState="false" OnClientSelectedIndexChanged="doCheck" OnClientContextMenu="clipboardContextMenu" Width="100%" DataSourceID="SqlDataSource1" DataTextField="gc_data_text" DataValueField="gc_data_uuid" />
        
                         