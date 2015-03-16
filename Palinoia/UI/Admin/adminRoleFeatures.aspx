<%@ Page title="Role-Features" Language="C#" AutoEventWireup="true" CodeBehind="adminRoleFeatures.aspx.cs" Inherits="Palinoia.UI.Admin.adminRoleFeatures" MasterPageFile="~/Site.master" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h1>Administration</h1>
    <table>
        <tr>
            <td >
                <h2><asp:Label ID="Label1" runat="server" cssclass="labelText" Text="Role: "></asp:Label>
                <asp:Label ID="lblRoleName" runat="server" cssclass="labelText" Text=""></asp:Label></h2>
            </td>
                <td >
                    <div id="Div1" runat="server" >
                        <asp:Button ID="btnDone" runat="server" Text="Done" onclick="btnDone_Click" cssclass="standardButton"/>
                    </div>
            </td>
        </tr>    
    </table>
    
    <div id="leftListBoxDIV" runat="server" style="float:left">
        <asp:Label ID="lblAssociatedFeatures" runat="server"  Text="Associated Features"></asp:Label>
        <asp:Panel runat="server" ScrollBars="Auto">
            <asp:ListBox ID="listAssociatedFeatures" runat="server" Width="275px" cssclass="listBox"
                height="500px" SelectionMode="Multiple"></asp:ListBox>
        </asp:Panel>
    </div>
    <div id="associationButtonsDIV" width="75px" height="500px" runat="server" style="float:left;padding-top:200px" >
        <asp:Button ID="btnAddFeature" runat="server" Text="<<<" 
             cssclass="standardButton" onclick="btnAddFeature_Click" /><br />
        <asp:Button ID="btnRemoveFeature" runat="server" Text=">>>" 
             cssclass="standardButton" onclick="btnRemoveFeature_Click" />
    </div>
    <div id="rightListBoxDIV" runat="server" cssclass="associatedFeatureslistBox" >
        <asp:Label ID="lblAvailableFeatures" runat="server" Text="Available Features"></asp:Label>
        <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto">
            <asp:ListBox ID="listAvailableFeatures" runat="server" Width="275px" cssclass="listBox"
                height="500px"  SelectionMode="Multiple"></asp:ListBox>
        </asp:Panel>
    </div>
</asp:Content>