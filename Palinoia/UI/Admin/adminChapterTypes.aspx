<%@ Page title="Chapter Types" Language="C#" AutoEventWireup="true" CodeBehind="adminChapterTypes.aspx.cs" MasterPageFile="~/Site.Master"
    Inherits="Palinoia.UI.Admin.adminChapterType" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
     
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <h1>Administration</h1>
    <br />
    <h2>Chapter Types</h2>
    <div style="position:relative;">
        <table>
            <tr>
                <td align="right">
                    <asp:Button ID="btnAddChapterType" runat="server" Text="Add Chapter Type"  
                        cssclass="longButton" onclientclick="return false;"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:panel ID="Panel1" runat="server" ScrollBars="Auto" CssClass="adminGridPanel">
                        <asp:GridView ID="grdChapterTypes" runat="server" 
                            AutoGenerateColumns="False" cssClass="gridview"
                            AlternatingRowStyle-CssClass="even" 
                            onrowdeleting="grdChapterTypes_RowDeleting" onrowediting="grdChapterTypes_RowEditing" 
                            onrowdatabound="grdChapterTypes_RowDataBound" Width="100%" >
                            <AlternatingRowStyle CssClass="even" />
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID">
                                <ItemStyle Width="50px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Text" HeaderText="Chapter Type" >
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
    <div id="dlgChapterType" title="Add/Edit Status" style="display: none;">
        <fieldset> 
            <table>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblChapterTypeText">Chapter Type Text: </asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAddChapterType" name="txtChapterType" runat="server" class="required specialChar">
                        </asp:TextBox><br />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:CheckBox ID="chkActive" runat="server" Text="Active" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <asp:Button ID="btnSaveChapterType" runat="server" Text="Save" 
                            cssclass="standardButton" onclick="btnSaveChapterType_Click" />
                        <asp:Button ID="btnCancel" runat="server" onclientclick="return false;" 
                            Text="Cancel" cssclass="standardButton" />
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <asp:HiddenField ID="hdnChapterTypeID" runat="server" />
</asp:Content>

