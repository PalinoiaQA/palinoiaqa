<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="adminTestSteps.aspx.cs" Inherits="Palinoia.UI.Admin.adminTestSteps" MasterPageFile="~/Site.master" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <%--<link href="../../Styles/jquery-ui-1.10.2.custom.min.css" rel="stylesheet" type="text/css" /> --%> 
      
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h1>Administration</h1>
    <br />
    <h2>Test Steps</h2>
    <asp:panel ID="Panel1" runat="server" ScrollBars="Auto" cssclass="adminGridPanel">
    <asp:GridView ID="grdTestSteps" runat="server" 
        AutoGenerateColumns="False" cssClass="gridview"
        AlternatingRowStyle-CssClass="even" 
         Width="100%" onrowdatabound="grdTestSteps_RowDataBound" 
            onrowdeleting="grdTestSteps_RowDeleting" 
            onrowediting="grdTestSteps_RowEditing" >
        <AlternatingRowStyle CssClass="even" />
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="ID">
            <ItemStyle Width="50px" />
            </asp:BoundField>
            <asp:BoundField DataField="Name" HeaderText="TestStep" 
                SortExpression="Text" />
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
    <asp:Button ID="btnAddTestStep" runat="server" Text="Add TestStep" Width="140px" 
         cssclass="LongControlButton" onclientclick="return false;" />
    <br />

    <div id="dlgTestStep" title="Test Step" >
        <fieldset> 
            <asp:Label runat="server" ID="lblTestStepText" CssClass="labelText">Test Step Text: </asp:Label><br />
            <asp:TextBox ID="txtAddTestStep" name="txtTestStep" runat="server" class="required specialChar" cssclass="textBox"></asp:TextBox><br />
            <asp:CheckBox ID="chkActive" runat="server" Text="Active" style="padding: 10px, 5px, 10px, 5px"/>
            <br />
            <div runat="server" id="modalButtonsDIV" style="float:right">
                <asp:Button ID="btnSaveTestStep" runat="server" text="Save" 
                    cssclass="RegularControlButton" onclick="btnSaveTestStep_Click"  />
                <asp:Button ID="btnCancel" runat="server"  
                    text="Cancel" cssclass="RegularControlButton"/>
            </div>
        </fieldset>
    </div>
    <asp:HiddenField ID="hdnTestStepID" runat="server" />
    <asp:HiddenField ID="hdnProjectID" runat="server" />
</asp:Content>

