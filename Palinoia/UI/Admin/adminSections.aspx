<%@ Page title="Sections" Language="C#" AutoEventWireup="true" CodeBehind="adminSections.aspx.cs" Inherits="Palinoia.UI.Admin.adminSections" MasterPageFile="~/Site.master" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h1>Administration</h1>
    <br />
    <h2>Sections</h2>
    <div style="position:relative;">
        <table>
            <tr>
                <td align="right">
                    <asp:Button ID="btnAddSection" runat="server" Text="Add Section" cssclass="standardButton"
                        onclientclick="return false;" width="140px"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:panel ID="Panel1" runat="server" ScrollBars="Auto" cssclass="adminGridPanel">
                        <asp:GridView ID="grdSections" runat="server" 
                            AutoGenerateColumns="False" cssClass="gridview"
                            AlternatingRowStyle-CssClass="even" 
                            width="100%" onrowdatabound="grdSections_RowDataBound" 
                            onrowdeleting="grdSections_RowDeleting" onrowediting="grdSections_RowEditing" >
                            <AlternatingRowStyle CssClass="even"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID" >
                                <ItemStyle Width="50px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Text" HeaderText="Section" />
                                <asp:BoundField AccessibleHeaderText="Abbreviation" DataField="Abbreviation" 
                                    HeaderText="Abbreviation">
                                <ItemStyle Width="75px" />
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
    
     <div id="dlgSection" title="Add/Edit Section" style="display: none;">
        <fieldset> 
            <table>
                <tr>
                    <td>   
                        <asp:Label runat="server" ID="lblSectionName" CssClass="labelText">Section Name: </asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSectionName" name="txtSectionName" runat="server" 
                            cssclass="textBox"></asp:TextBox><br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblSectionAbbreviation" CssClass="labelText">Abbreviation: </asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSectionAbbreviation" name="txtSectionAbbreviation" runat="server" 
                            cssclass="textBox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:CheckBox ID="chkActive" runat="server" Text="Active" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Button ID="btnSaveSection" runat="server" Text="Save" 
                            cssclass="standardButton" onclick="btnSaveSection_Click" />
                        <asp:Button ID="btnCancel" runat="server"  
                            Text="Cancel" cssclass="standardButton" onclientclick="return false;" />
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <asp:HiddenField ID="hdnSectionID" runat="server" />
</asp:Content>


