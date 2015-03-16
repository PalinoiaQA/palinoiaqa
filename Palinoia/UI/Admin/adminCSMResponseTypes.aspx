<%@ Page title="CSM Response Types" Language="C#" AutoEventWireup="true" CodeBehind="adminCSMResponseTypes.aspx.cs" 
Inherits="Palinoia.UI.Admin.adminCSMResponseTypes" MasterPageFile="~/Site.master" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <link href="../../Styles/jquery-ui-1.10.2.custom.min.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h1>Administration</h1>
    <br />
    <h2>CSM Response Types</h2>
    <div style="position:relative;">
        <table>
            <tr>
                <td align="right">
                    <asp:Button ID="btnAddResponseType" runat="server" Text="Add Response Type" Width="140px" 
                        onclientclick="return false;" cssclass="standardButton"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:panel ID="Panel1" runat="server" ScrollBars="Auto" CssClass="adminGridPanel">
                    <asp:GridView ID="grdCSMResponseTypes" runat="server" 
                        AutoGenerateColumns="False" cssClass="gridview"
                        onrowdeleting="grdCSMResponseTypes_RowDeleting" 
                        onrowediting="grdCSMResponseTypes_RowEditing" 
                        AlternatingRowStyle-CssClass="even" 
                        onrowdatabound="grdCSMResponseTypes_RowDataBound" width="100%">
                        <AlternatingRowStyle CssClass="even" />
                        <Columns>
                            <asp:BoundField DataField="ID" HeaderText="ID">
                            <ItemStyle Width="50px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Text" HeaderText="Response Type" />
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
    <div id="dlgResponseType" title="Add/Edit CMS Response Type" style="display: none;">
        <fieldset> 
            <table>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblResponseTypeText">Response Type Text: </asp:Label>
                        <asp:TextBox ID="txtAddResponseType" name="txtAddResponseType" runat="server" 
                            class="required specialChar">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkActive" runat="server" Text="Active" />
                    </td>
                </tr>
                <tr>
                    <td align = "right">
                        <asp:Button ID="btnSaveCSMResponseType" runat="server" Text="Save" 
                            onclick="btnSaveCSMResponseType_Click" cssclass="standardButton" />
                        <asp:Button ID="btnCancel" runat="server" onclientclick="return false;" 
                            Text="Cancel" cssclass="standardButton" />
                    </td>

                </tr>
            </table>
        </fieldset>
    </div>
    <asp:HiddenField ID="hdnEditResponseTypeID" runat="server" />
</asp:Content>

