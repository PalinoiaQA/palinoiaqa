<%@ Page title="Roles" Language="C#" AutoEventWireup="true" CodeBehind="adminRoles.aspx.cs" Inherits="Palinoia.UI.Admin.adminRoles"  MasterPageFile="~/Site.master" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
   <%--<link href="../../Styles/jquery-ui-1.10.2.custom.min.css" rel="stylesheet" type="text/css" />--%>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h1>Administration</h1>
    <br />
    <h2>Roles</h2>
    <div style="position:relative;">
        <table>
            <tr>
                <td align="right">
                    <asp:Button ID="btnAddRole" runat="server" Text="Add Role" Width="140px" 
                        cssclass="standardButton" onclientclick="return false;"  />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:panel runat="server" ScrollBars="Auto" cssclass="adminGridPanel">
                        <asp:GridView ID="grdRoles" runat="server" 
                            AutoGenerateColumns="False" cssClass="gridview"
                            AlternatingRowStyle-CssClass="even" onrowdatabound="grdRoles_RowDataBound" 
                            onrowdeleting="grdRoles_RowDeleting" onrowediting="grdRoles_RowEditing" width="100%">
                            <AlternatingRowStyle CssClass="even"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID" >
                                <ItemStyle Width="50px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Text" HeaderText="Role" />
                                <asp:BoundField AccessibleHeaderText="Active" DataField="Active" 
                                    HeaderText="Active">
                                <ItemStyle Width="50px" />
                                </asp:BoundField>
                                <asp:CommandField ShowCancelButton="False" ShowDeleteButton="True" 
                                    ShowEditButton="True" >
                                <ItemStyle Width="50px" />
                                </asp:CommandField>
                            </Columns>
                        </asp:GridView>
                    </asp:panel>
                </td>
            </tr>
        </table>
    </div>
    <div id="dlgRole" title="Add/Edit Status" style="display: none;">
        <fieldset> 
            <table>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblRoleText" CssClass="labelText">Role Name: </asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAddRole" name="txtRole" runat="server" 
                            cssclass="textBox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkActive" runat="server" Text="Active" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                    <td align="right">
                        <asp:Button ID="btnSetFeatures" runat="server" Text="Set Features" 
                            cssclass="longButton" onclick="btnSetFeatures_Click" />
                        <asp:Button ID="btnSaveRole" runat="server" Text="Save" 
                            cssclass="standardButton" onclick="btnSaveRole_Click"   />
                        <asp:Button ID="btnCancel" runat="server"  
                            Text="Cancel" cssclass="standardButton" onclientclick="return false;" />
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>>
    <asp:HiddenField ID="hdnRoleID" runat="server" />
</asp:Content>


