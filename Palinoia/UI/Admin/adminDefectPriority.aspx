<%@ Page title="Defect Priority" Language="C#" AutoEventWireup="true" CodeBehind="adminDefectPriority.aspx.cs" 
Inherits="Palinoia.UI.Admin.adminDefectPriority" MasterPageFile="~/Site.Master"%>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h1>Administration</h1>
    <br />
    <h2>Defect Priority</h2>
    <div style="position:relative;">
        <table>
            <tr>
                <td align="right">
                    <asp:Button ID="btnAddDefectPriority" runat="server" Text="Add Defect Priority" Width="140px" 
                        cssclass="standardButton" onclientclick="return false;"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:panel ID="Panel1" runat="server" ScrollBars="Auto" CssClass="adminGridPanel">
                        <asp:GridView ID="grdDefectPriority" runat="server" 
                            AutoGenerateColumns="False" cssClass="gridview"
                            AlternatingRowStyle-CssClass="even" 
                            onrowdeleting="grdDefectPriority_RowDeleting" onrowediting="grdDefectPriority_RowEditing" 
                            onrowdatabound="grdDefectPriority_RowDataBound" Width="100%" >
                            <AlternatingRowStyle CssClass="even" />
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID">
                                <ItemStyle Width="50px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Text" HeaderText="Defect Priority" >
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
    <div id="dlgDefectPriority" title="Add/Edit Priority">
        <fieldset> 
            <table>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblDefectPriorityText" CssClass="labelText">Defect Priority Text: </asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAddDefectPriority" name="txtDefectPriority" runat="server" class="required specialChar">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:CheckBox ID="chkActive" runat="server" Text="Active" CssClass="labelText"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <asp:Button ID="btnSaveDefectPriority" runat="server" Text="Save" 
                            cssclass="standardButton" onclick="btnSaveDefectPriority_Click" />
                        <asp:Button ID="btnCancel" runat="server" onclientclick="return false;" 
                            Text="Cancel" cssclass="standardButton" />
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <asp:HiddenField ID="hdnDefectPriorityID" runat="server" />
</asp:Content>

