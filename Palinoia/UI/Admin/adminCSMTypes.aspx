<%@ Page Title="CSM Types" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="adminCSMTypes.aspx.cs" Inherits="Palinoia.UI.Admin.adminCSMTypes" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <%--<link href="../../Styles/jquery-ui-1.10.2.custom.min.css" rel="stylesheet" type="text/css" />--%>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h1>Administration</h1>
    <br />
    <h2>CSM Types</h2>
    <div style="position:relative;">
        <table>
            <tr>
                <td align="right">
                    <asp:Button ID="btnAddCSMType" runat="server" Text="Add CSM Type" Width="140px" 
                        cssclass="standardButton" onclientclick="return false;"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:panel ID="Panel1" runat="server" ScrollBars="Auto" CssClass="adminGridPanel">
                        <asp:GridView ID="grdCSMTypes" runat="server" 
                            AutoGenerateColumns="False" cssClass="gridview"
                            AlternatingRowStyle-CssClass="even" 
                            onrowdeleting="grdCSMTypes_RowDeleting" onrowediting="grdCSMTypes_RowEditing" 
                            onrowdatabound="grdCSMTypes_RowDataBound" Width="100%" >
                            <AlternatingRowStyle CssClass="even" />
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID">
                                <ItemStyle Width="50px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Text" HeaderText="CSM Type" />
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
    <div id="dlgCSMTypes" title="Add/Edit CSM Type" style="display: none;">
        <fieldset> 
            <table>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblCSMTypeText">CSM Type Text: </asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAddCSMType" name="txtCSMType" runat="server" 
                            class="required specialChar"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkActive" runat="server" Text="Active" />
                    </td>
                    <td>
                    
                    </td>
                </tr>
                <tr>
                    <td>
                
                    </td>
                    <td>
                        <asp:Button ID="btnSaveCSMType" runat="server" Text="Save" 
                            cssclass="standardButton" onclick="btnSaveCSMType_Click" />
                        <asp:Button ID="btnCancel" runat="server" onclientclick="return false;" 
                            Text="Cancel" cssclass="standardButton" />
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <asp:HiddenField ID="hdnCSMTypeID" runat="server" />
</asp:Content>
