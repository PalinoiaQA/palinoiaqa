<%@ Page Title="Project Home" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Palinoia._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <%--<script src="Scripts/Palinoia/Default.js" type="text/javascript"></script>--%>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        <asp:Label ID="lblProject" runat="server" Text="Project: "></asp:Label>
        <asp:Label ID="lblProjectName" runat="server" Text="lblProjectName"></asp:Label>
    </h2>
    <%--<br />

    <h3>
        <asp:Label ID="lblProjectDescription" runat="server" Text="lblProjectDescription"></asp:Label>
    </h3>
    <br />--%>
    <br />
    <h4>
        <asp:Label ID="lblBusinessRules" runat="server" Text="Total Business Rules: "></asp:Label>
        <asp:Label ID="lblBusinessRulesTotal" runat="server" Text="lblBusinessRulesTotal"></asp:Label>
        <br />
        <br />
        <asp:Label ID="lblCustomerServiceMessages" runat="server" Text="Total Customer Service Messages: "></asp:Label>
        <asp:Label ID="lblCustomerServiceMessagesTotal" runat="server" Text="lblCustomerServiceMessagesTotal"></asp:Label>
        <br />
        <br />
        <asp:Label ID="lblTestCases" runat="server" Text="Total Test Cases: "></asp:Label>
        <asp:Label ID="lblTestCasesTotal" runat="server" Text="lblTestCasesTotal"></asp:Label>
        <br />
    </h4>
        <div style="padding-left: 20px">
            <asp:Label ID="lblPassed" runat="server" Text="Passed: "></asp:Label>
            <asp:Label ID="lblPassedTotal" runat="server" Text="...calculating" class = "totalPassed"></asp:Label>
            <br />
            <asp:Label ID="lblFailed" runat="server" Text="Failed: "></asp:Label>
            <asp:Label ID="lblFailedTotal" runat="server" Text="...calculating" class = "totalFailed"></asp:Label>
            <br />
            <asp:Label ID="lblUntested" runat="server" Text="Untested: "></asp:Label>
            <asp:Label ID="lblUntestedTotal" runat="server" Text="...calculating" class = "totalUntested"></asp:Label>
            <br />
        </div>
        <br />
        <asp:Label ID="lblAssociatedDocuments" runat="server" Text="Associated Documents: "></asp:Label>
        <br />
        <br />
        <asp:ListBox ID="lbAssociatedDocuments" runat="server" style="width:75%;height:300px"></asp:ListBox>

    <asp:HiddenField ID="hdnProjectID" runat="server" />
    
</asp:Content>
