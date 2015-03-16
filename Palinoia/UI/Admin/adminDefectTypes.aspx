<%@ Page title="Defect Types" Language="C#" AutoEventWireup="true" CodeBehind="adminDefectTypes.aspx.cs" 
Inherits="Palinoia.UI.Admin.adminDefectTypes" MasterPageFile="~/Site.Master"%>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h1>Administration</h1>
    <br />
    <h2>Defect Types</h2>
    <div style="position:relative;">
        <table>
            <tr>
                <td align="right">
                    <asp:Button ID="btnAddDefectType" runat="server" Text="Add Defect Type" Width="140px" 
                        cssclass="standardButton" onclientclick="return false;"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:panel ID="Panel1" runat="server" ScrollBars="Auto" height="250px">
                        <asp:GridView ID="grdDefectTypes" runat="server" 
                            AutoGenerateColumns="False" cssClass="gridview"
                            AlternatingRowStyle-CssClass="even" 
                            onrowdeleting="grdDefectTypes_RowDeleting" onrowediting="grdDefectTypes_RowEditing" 
                            onrowdatabound="grdDefectTypes_RowDataBound" Width="100%" >
                            <AlternatingRowStyle CssClass="even" />
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID">
                                <ItemStyle Width="50px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Text" HeaderText="Defect Type" >
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
    <div id="dlgDefectType" title="Add/Edit Status" style="display: none;">
        <fieldset> 
            <table>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblDefectTypeText">Defect Type Text: </asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAddDefectType" name="txtDefectType" runat="server" class="required specialChar">
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
                        <asp:Button ID="btnSaveDefectType" runat="server" Text="Save" 
                            cssclass="standardButton" onclick="btnSaveDefectType_Click" />
                        <asp:Button ID="btnCancel" runat="server" onclientclick="return false;" 
                            Text="Cancel" cssclass="standardButton" />
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <asp:HiddenField ID="hdnDefectTypeID" runat="server" />
</asp:Content>

