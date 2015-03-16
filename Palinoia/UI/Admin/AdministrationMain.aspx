<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdministrationMain.aspx.cs" Inherits="Palinoia.UI.Admin.AdministrationMain" MasterPageFile="~/Site.master"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <link href="../../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.validate.js" type="text/javascript"></script>
    <script src="../../Scripts/Palinoia/utility.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
    <h1>Administration</h1>
    <br />
    <div id="adminMenuDIV">
    
     <%--<ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" 
            Width="950px" Height="735px">
        <cc1:TabPanel runat="server" HeaderText="Projects" ID="pnlProjects">
            <ContentTemplate> 
                <iframe src="adminProject.aspx" width=100%></iframe>    
            </ContentTemplate>
        </cc1:TabPanel>
        <cc1:TabPanel runat="server" HeaderText="CSM Response Types" ID="pnlCSMResponseTypes">
            <ContentTemplate> 
                <iframe src="adminCSMResponseTypes.aspx" width=100%></iframe>    
            </ContentTemplate>
        </cc1:TabPanel>
        <cc1:TabPanel runat="server" HeaderText="Status" ID="pnlStatus">
            <ContentTemplate> 
                <iframe src="adminStatus.aspx" width=100%></iframe>    
            </ContentTemplate>
        </cc1:TabPanel>
    </ajaxToolkit:TabContainer>--%>
<%--    </div>

    <asp:Menu ID="NavigationMenu" runat="server" CssClass="menu" EnableViewState="false" IncludeStyleBlock="false" Orientation="Horizontal">
                    <Items>
                        <asp:MenuItem NavigateUrl="adminProject.aspx" Text = "Projects" />
                        <asp:MenuItem NavigateUrl="adminCSMResponseTypes.aspx" Text="CSM Response Types"/>
                        <asp:MenuItem NavigateUrl="adminStatus.aspx" Text="Status"/>
                    </Items>
                </asp:Menu>
    </div>
</asp:Content>--%>
