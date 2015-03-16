<%@ Page title="Status" Language="C#" AutoEventWireup="true" CodeBehind="adminStatus.aspx.cs" 
    Inherits="Palinoia.UI.Admin.adminStatus" MasterPageFile="~/Site.master"%>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
   <link href="../../Scripts/ColorPicker/colorPicker.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h1>Administration</h1>
    <br />
    <h2>Status</h2>
    <asp:label runat="server" cssclass="WarningText">Warning! Deleting any of the default status values:  Approved, New, Deleted, or Revised could 
    cause errors and unexpected behaviours in the application.</asp:label>
    <div style="position:relative;">
        <table>
            <tr>
                <td align="right">
                    <asp:Button ID="btnAddStatus" runat="server" Text="Add Status" Width="140px" 
                        cssclass="standardButton" onclientclick="return false;"/>
                </td>
            </tr>
            <tr>
                <td>
                <asp:panel ID="Panel1" runat="server" ScrollBars="Auto" cssclass="adminGridPanel">
                <asp:GridView ID="grdStatus" runat="server" 
                    AutoGenerateColumns="False" cssClass="gridview"
                    AlternatingRowStyle-CssClass="even" 
                    onrowdeleting="grdStatus_RowDeleting" onrowediting="grdStatus_RowEditing" 
                    onrowdatabound="grdStatus_RowDataBound" Width="100%" >
                    <AlternatingRowStyle CssClass="even" />
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID">
                            <ItemStyle Width="50px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Text" HeaderText="Status" />
                        <asp:BoundField AccessibleHeaderText="Highlight Color" DataField="Color" 
                            HeaderText="Highlight Color">
                        <ItemStyle Width="50px" />
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
    <div id="dlgStatus" title="Add/Edit Status" >
        <fieldset> 
            <table>
                <tr>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lblStatusText" CssClass="labelText">Status Text: </asp:Label>
                        <asp:TextBox ID="txtAddStatus" name="txtStatus" runat="server" width="75%"></asp:TextBox><br />
                    </td>
                </tr>
                <tr>
                    <td style="width:30%">
                        <label for="color1" class="labelText">Highlight Color</label>
                    </td>
                    <td  align = "left">
                        
                        <input id="chooseColor" name="chooseColor" type="text" value="#333399" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:CheckBox ID="chkDisplayInSummary" runat="server" 
                            Text = "Display status in chapter summary"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:CheckBox ID="chkActive" runat="server" class="ActiveCheckbox" Text="Active" />
                    </td>
                </tr>
                <tr>
                    <td align = "right" colspan = "2">
                        <asp:Button ID="btnSaveStatus" runat="server" Text="Save" 
                            cssclass="standardButton" onclick="btnSaveStatus_Click" />
                        <asp:Button ID="btnCancel" runat="server" onclick="btnCancel_Click" 
                            Text="Cancel" cssclass="standardButton" />
                    </td>

                </tr>
            </table>
        </fieldset>
    </div>
    <asp:HiddenField ID="hdnStatusID" runat="server"  />
    <asp:HiddenField ID="hdnColor" runat="server" />
</asp:Content>
