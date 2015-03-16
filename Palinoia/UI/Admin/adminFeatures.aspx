<%@ Page title="Features" Language="C#" AutoEventWireup="true" CodeBehind="adminFeatures.aspx.cs" Inherits="Palinoia.UI.Admin.adminFeatures" MasterPageFile="~/Site.master" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h1>Administration</h1>
    <br />
    <h2>Features</h2>
    <div style="position:relative;">
        <table>
            <tr>
                <td align="right">
                    <asp:Button ID="btnAddFeature" runat="server" Text="Add Feature" Width="140px" 
                        cssclass="standardButton" onclientclick="return false;" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:panel ID="Panel1" runat="server" ScrollBars="Auto" cssclass="adminGridPanel">
                        <asp:GridView ID="grdFeatures" runat="server" 
                            AutoGenerateColumns="False" cssClass="gridview"
                            AlternatingRowStyle-CssClass="even" 
                            onrowdatabound="grdFeatures_RowDataBound" 
                            onrowdeleting="grdFeatures_RowDeleting" onrowediting="grdFeatures_RowEditing" 
                            width="100%" >
                            <AlternatingRowStyle CssClass="even"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID" >
                                <ItemStyle Width="50px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Text" HeaderText="Feature" />
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
    
    <div id="dlgFeature" title="Add/Edit Feature" style="display: none;">
        <fieldset> 
            <table>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblFeatureText" CssClass="labelText">Feature Text: </asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAddFeature" runat="server" cssclass="textBox"></asp:TextBox><br />
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
                        <asp:Button ID="btnSaveFeature" runat="server" Text="Save" 
                            cssclass="standardButton" onclick="btnSaveFeature_Click"  />
                        <asp:Button ID="btnCancel" runat="server"  
                            Text="Cancel" cssclass="standardButton" onclientclick="return false;" />
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <asp:HiddenField ID="hdnFeatureID" runat="server" />
</asp:Content>

