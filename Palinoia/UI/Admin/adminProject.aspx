<%@ Page title="Projects" Language="C#" AutoEventWireup="true" CodeBehind="adminProject.aspx.cs" Inherits="Palinoia.UI.Admin.adminProject" MasterPageFile="~/Site.master"%>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <%--<link href="../../Styles/jquery-ui-1.10.2.custom.min.css" rel="stylesheet" type="text/css" />--%>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h1>Administration</h1>
    <br />
    <h2>Projects</h2>
    <table>
        <tr>
            <td align="right">
                <asp:Button ID="btnNewProject" runat="server" Text="New Project" cssclass="standardButton" 
                     Width="95px" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:panel ID="Panel1" runat="server" ScrollBars="Auto" cssclass="adminGridPanel">
                    <asp:GridView ID="grdProjects" runat="server" cssClass="gridview" width="100%" 
                        AutoGenerateColumns="False" AlternatingRowStyle-CssClass="even" 
                        onrowdeleting="grdProjects_RowDeleting" 
                        onrowediting="grdProjects_RowEditing" 
                        onselectedindexchanging="grdProjects_SelectedIndexChanging" 
                        onrowdatabound="grdProjects_RowDataBound">
                        <Columns>
                            <asp:BoundField AccessibleHeaderText="ID" DataField="ID" HeaderText="ID">
                                <ItemStyle Width="50px" />
                            </asp:BoundField>
                            <asp:BoundField AccessibleHeaderText="Name" DataField="Name" 
                                    HeaderText="Name">
                                    <ItemStyle Width="200px" />
                            </asp:BoundField>
                            <asp:BoundField AccessibleHeaderText="DataSource" DataField="DataSource" 
                                HeaderText="DataSource">
                            <ItemStyle Width="350px" />
                            </asp:BoundField>
                            <asp:CommandField ShowCancelButton="False" 
                                ShowDeleteButton="True" ShowEditButton="True" ShowSelectButton="True">
                            </asp:CommandField>
                        </Columns>
                    </asp:GridView>
                </asp:panel>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hdnProjectID" runat="server" />
    <div id="dlgProject" title="Add/Edit Project" style="display: none;">
        <fieldset> 
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblNewProjectName" runat="server" Text="Project Name: " CssClass="labelText"></asp:Label>
                        <asp:TextBox ID="txtNewProjectName" runat="server" width="75%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align = "right">
                        <asp:Button ID="btnSaveProject" runat="server" Text="Save" cssclass="standardButton" onclick="btnSaveProject_Click" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" cssclass="standardButton" onclientclick="return false;" />
                    </td>

                </tr>
            </table>
        </fieldset>
    </div>
</asp:Content>
