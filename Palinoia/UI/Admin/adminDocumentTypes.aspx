<%@ Page title="Document Types" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master"
    CodeBehind="adminDocumentTypes.aspx.cs" Inherits="Palinoia.UI.Admin.adminDocumentTypes" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h1>Administration</h1>
    <br />
    <h2>Document Types</h2>
    <div style="position:relative;">
        <table>
            <tr>
                <td align="right">
                    <asp:Button ID="btnAddDocumentType" runat="server" Text="Add Document Type" Width="140px" 
                        cssclass="standardButton" onclientclick="return false;"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:panel ID="Panel1" runat="server" ScrollBars="Auto" height="250px">
                        <asp:GridView ID="grdDocumentTypes" runat="server" 
                            AutoGenerateColumns="False" cssClass="gridview"
                            AlternatingRowStyle-CssClass="even" 
                            onrowdeleting="grdDocumentTypes_RowDeleting" onrowediting="grdDocumentTypes_RowEditing" 
                            onrowdatabound="grdDocumentTypes_RowDataBound" Width="100%" >
                            <AlternatingRowStyle CssClass="even" />
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID">
                                <ItemStyle Width="50px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Text" HeaderText="Document Type" >
                                <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField AccessibleHeaderText="Active" DataField="Active" 
                                    HeaderText="Active">
                                <ItemStyle Width="50px" />
                                </asp:BoundField>
                                <asp:CommandField ShowCancelButton="False" ShowDeleteButton="True" 
                                    ShowEditButton="True">
                                <ItemStyle Width="50px" />
                                </asp:CommandField>
                            </Columns>
                        </asp:GridView>
                    </asp:panel>
                </td>
            </tr>
        </table>
    </div>
    <div id="dlgDocumentType" title="Add/Edit Document Type" style="display: none;">
        <fieldset> 
            <table>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblDocumentTypeText">Document Type Text: </asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAddDocumentType" name="txtDocumentType" runat="server" class="required specialChar">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:CheckBox ID="chkActive" runat="server" Text="Active" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:CheckBox ID="chkShowSummary" runat="server" Text="Display chapter Business Rule/CSM Summary table(s)" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <asp:Button ID="btnSaveDocumentType" runat="server" Text="Save" 
                            cssclass="standardButton" onclick="btnSaveDocumentType_Click" />
                        <asp:Button ID="btnCancel" runat="server" onclientclick="return false;" 
                            Text="Cancel" cssclass="standardButton" />
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <asp:HiddenField ID="hdnDocumentTypeID" runat="server" />
</asp:Content>
