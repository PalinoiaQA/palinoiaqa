<%@ Page Title="Reports" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" 
    Inherits="Palinoia.UI.Reports.Reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table>
        <tr>
            <td>
                <h1>Reports</h1>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <asp:LinkButton ID="LinkButton1" runat="server" onclick="LinkButton1_Click">Business Rules without a Test Case</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td>
                <asp:LinkButton ID="LinkButton2" runat="server" onclick="LinkButton2_Click">Business Rules not referenced in All Documents</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td>
                <asp:LinkButton ID="LinkButton3" runat="server" onclick="LinkButton3_Click">Business Rules not referenced in Functional Documents</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td>
                <asp:LinkButton ID="LinkButton4" runat="server" onclick="LinkButton4_Click">Business Rules not referenced in Technical Documents</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td>
                <asp:LinkButton ID="LinkButton5" runat="server" onclick="LinkButton5_Click">Business Rules not referenced in Miscellaneous Documents</asp:LinkButton>
            </td>
        </tr>

        <tr>
            <td>
                <asp:Label ID="lblBRbySection" runat="server" 
                    Text="Business Rule list by Section "></asp:Label>
                <asp:DropDownList ID="ddlSections" runat="server" CssClass = "dropDownList sectionDDL" 
                    style="width:35%">
                </asp:DropDownList>
                <asp:Button ID="btnBRbySection" runat="server" onclick="btnBRbySection_Click" 
                    Text="View" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" 
                    Text="Business Rule list by Status "></asp:Label>
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass = "dropDownList statusDDL" 
                    style="width:35%">
                </asp:DropDownList>
                <asp:Button ID="btnBRbyStatus" runat="server" onclick="btnBRbyStatus_Click" 
                    Text="View" />
            </td>
        </tr>

    </table>
    <asp:HiddenField ID="hdnSectionID" runat="server" />
    <asp:HiddenField ID="hdnStatusID" runat="server" />
</asp:Content>

