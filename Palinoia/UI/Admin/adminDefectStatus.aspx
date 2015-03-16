<%@ Page title="Defect Status" Language="C#" AutoEventWireup="true" CodeBehind="adminDefectStatus.aspx.cs" 
Inherits="Palinoia.UI.Admin.adminDefectStatus" MasterPageFile="~/Site.Master"%>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h1>Administration</h1>
    <br />
    <h2>Defect Status</h2>
    <div style="position:relative; top: 3px; left: 1px;">
        <table style="width: 100%">
            <tr>
                <td align="right">
                    <asp:Button ID="btnAddDefectStatus" runat="server" Text="Add Defect Status" Width="140px" 
                        cssclass="standardButton" onclientclick="return false;"/>
                </td>
            </tr>
            <tr>
                <td width="100%">
                    <asp:panel ID="Panel1" runat="server" ScrollBars="Auto" CssClass="adminGridPanel" >
                        <asp:GridView ID="grdDefectStatus" runat="server" 
                            AutoGenerateColumns="False" cssClass="gridview"
                            AlternatingRowStyle-CssClass="even" 
                            onrowdeleting="grdDefectStatus_RowDeleting" onrowediting="grdDefectStatus_RowEditing" 
                            onrowdatabound="grdDefectStatus_RowDataBound" >
                            <AlternatingRowStyle CssClass="even" />
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID">
                                <ItemStyle Width="50px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Text" HeaderText="Defect Status" >
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
    <div id="dlgDefectStatus" title="Add/Edit Status" style="display: none;">
        <fieldset> 
            <table>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblDefectStatusText">Defect Status Text: </asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAddDefectStatus" name="txtDefectStatus" runat="server" class="required specialChar">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:CheckBox ID="chkActive" runat="server" Text="Active" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <asp:Button ID="btnSaveDefectStatus" runat="server" Text="Save" 
                            cssclass="standardButton" onclick="btnSaveDefectStatus_Click" />
                        <asp:Button ID="btnCancel" runat="server" onclientclick="return false;" 
                            Text="Cancel" cssclass="standardButton" />
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <asp:HiddenField ID="hdnDefectStatusID" runat="server" />
</asp:Content>
