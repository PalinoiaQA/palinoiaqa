<%@ Page title="Users" Language="C#" AutoEventWireup="true" CodeBehind="adminUsers.aspx.cs" Inherits="Palinoia.UI.Admin.adminUsers" MasterPageFile="~/Site.master"%>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
   <%-- <link href="../../Styles/jquery-ui-1.10.2.custom.min.css" rel="stylesheet" type="text/css" /> --%>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h1>Administration</h1>
    <br />
    <h2>Users</h2>
    <div style="position:relative;">
        <table>
            <tr>
                <td align="right">
                    <asp:Button ID="btnAddUser" runat="server" Text="Add User" Width="140px" 
                        cssclass="standardButton" onclientclick="return false;"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:panel ID="Panel1" runat="server" ScrollBars="Auto" height="250px">
                    <asp:GridView ID="grdUsers" runat="server" 
                        AutoGenerateColumns="False" cssClass="gridview"
                        AlternatingRowStyle-CssClass="even" 
                        Width="100%" onrowdatabound="grdUsers_RowDataBound" 
                            onrowdeleting="grdUsers_RowDeleting" onrowediting="grdUsers_RowEditing" >
                        <AlternatingRowStyle CssClass="even" />
                        <Columns>
                            <asp:BoundField DataField="ID" HeaderText="ID">
                            <ItemStyle Width="50px" />
                            </asp:BoundField>
                            <asp:BoundField AccessibleHeaderText="RoleID" DataField="UserRoleID" 
                                HeaderText="RoleID" Visible="False" />
                            <asp:BoundField AccessibleHeaderText="Role" DataField="RoleName" 
                                HeaderText="Role" />
                            <asp:BoundField AccessibleHeaderText="First Name" DataField="FirstName" 
                                HeaderText="First Name" />
                            <asp:BoundField AccessibleHeaderText="Last Name" DataField="LastName" 
                                HeaderText="Last Name" />
                            <asp:BoundField AccessibleHeaderText="Middle Initial" DataField="MiddleInitial" 
                                HeaderText="Middle Initial" />
                            <asp:BoundField AccessibleHeaderText="Email" DataField="Email" 
                                HeaderText="Email" />
                            <asp:BoundField AccessibleHeaderText="Password" DataField="PW" 
                                HeaderText="Password" Visible="False" />
                            <asp:BoundField AccessibleHeaderText="Active" DataField="Active" 
                                HeaderText="Active" />
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
    <div id="dlgUser" title="Add/Edit User" style="display: none;">
        <fieldset> 
            <table>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblFirstName">First Name: </asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFirstName" name="txtUser" runat="server" 
                            class="required specialChar"></asp:TextBox><br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblLastName">Last Name: </asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtLastName" name="txtLastName" runat="server" 
                            class="required specialChar"></asp:TextBox><br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblMiddleInitial">Middle Initial: </asp:Label>    
                    </td>
                    <td>
                        <asp:TextBox ID="txtMiddleInitial" name="txtMiddleInitial" runat="server" 
                            class="specialChar"></asp:TextBox><br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblEmail">Email: </asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail" name="txtEmail" runat="server" 
                            class="required"></asp:TextBox><br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblPassword">Password: </asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPassword" name="txtPassword" runat="server"></asp:TextBox><br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblRole">Role: </asp:Label> 
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlUserRole" runat="server"> 
                        </asp:DropDownList>
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
                        <asp:Button ID="btnSaveUser" runat="server" Text="Save" 
                            cssclass="standardButton" onclick="btnSaveUser_Click" />
                        <asp:Button ID="btnCancel" runat="server"  
                            Text="Cancel" cssclass="standardButton" />
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <asp:HiddenField ID="hdnUserID" runat="server" />
</asp:Content>

