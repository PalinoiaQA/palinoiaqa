<%@ Page title="Test Runner" Language="C#" AutoEventWireup="true" CodeBehind="TestRunner.aspx.cs" Inherits="Palinoia.UI.TestCases.TestRunner" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Test Runner</title>
    <link href="../../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.8.3.min.js" type="text/javascript"></script>
    <%--<script src="../../Scripts/jquery.validate.min.js" type="text/javascript"></script>--%>
    <script src="../../Scripts/jquery.json-2.4.min.js" type="text/javascript"></script>
    <%--<script src="../../Scripts/JSTree/jquery.jstree.js" type="text/javascript"></script>--%>
    <script src="../../Scripts/jquery-ui-1.9.0.min.js" type="text/javascript"></script>
    <%--<script src="../../Scripts/jquery-cookie-1.4.0/jquery.cookie.js" type="text/javascript"></script>--%>
    <script src="../../Scripts/Palinoia/utility.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.alerts.js" type="text/javascript"></script>
    <link href="../../Styles/jquery.alerts.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.2.custom.min.css" rel="stylesheet" type="text/css" />
        <%--<script language="javascript" src='<%# Page.ResolveClientUrl("~/Scripts/jquery-1.8.3.min.js") %>' type="text/javascript"></script>
        <script language="javascript" src='<%# Page.ResolveClientUrl("~/Scripts/jquery.validate.js") %>' type="text/javascript"></script>
        <script language="javascript" src='<%# Page.ResolveClientUrl("~/Scripts/jquery.json-2.4.min.js") %>' type="text/javascript"></script>
        <script language="javascript" src='<%# Page.ResolveClientUrl("~/Scripts/jquery-ui-1.9.0.min.js") %>' type="text/javascript"></script>
        <script language="javascript" src='<%# Page.ResolveClientUrl("~/Scripts/jquery-cookie-1.4.0/jquery.cookie.js") %>' type="text/javascript"></script>
        <script language="javascript" src='<%# Page.ResolveClientUrl("~/Scripts/JSTree/jquery.jstree.js") %>' type="text/javascript"></script>
        <script language="javascript" src='<%# Page.ResolveClientUrl("~/Scripts/Palinoia/utility.js") %>' type="text/javascript"></script>
        <script language="javascript" src='<%# Page.ResolveClientUrl("~/Scripts/jquery.alerts.js") %>' type="text/javascript"></script>--%>
    <%--<style type="text/css">
        .style1
        {
            height: 23px;
            width: 80%;
        }
        .style2
        {
            height: 23px;
            width: 20%;
        }
    </style>--%>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table >
            <tr>
                <td colspan="2">
                    <asp:Label ID="Label1" runat="server" Text="Primary Test Case: " cssclass="labelText"></asp:Label>
                    <asp:Label ID="lblPrimaryTestCase" runat="server" Text="" cssclass="labelText"></asp:Label> 
                </td>
                <td>
                    <asp:Label ID="lblTestResult" runat="server" class="result" Font-Bold="True" 
                        Font-Size="Larger"></asp:Label> 
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="Label2" runat="server" Text="Current Test Case: " cssclass="labelText"></asp:Label>
                    <asp:Label ID="lblCurrentTestCase" runat="server" Text="" cssclass="labelText"></asp:Label> 
                </td>
                <td>
                
                </td>
            </tr>
            <tr>
                <td colspan = "3" >
                    <%--<asp:Label ID="Label1" runat="server" Text="Test Runner"></asp:Label>--%>
                    <asp:TextBox ID="txtTestStep" runat="server" Height="100px" 
                        TextMode="MultiLine" cssclass="textBox"></asp:TextBox>
                    <br />
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Button ID="btnBack" runat="server" Text="Back" cssclass="standardButton testRunnerButton" onclick="btnBack_Click" />
                </td>
                <td align = "center">
                    <asp:Button ID="btnPass" runat="server" Text="Pass" cssclass="standardButton testRunnerButton" onclick="btnPass_Click" />
                    <asp:Button ID="btnFail" runat="server" Text="Fail" cssclass="standardButton testRunnerButton" onclientclick="return false;" />
                </td>
                <td align="right">
                    <asp:Button ID="btnNext" runat="server" Text="Next" cssclass="standardButton testRunnerButton" onclick="btnNext_Click" />
                </td>
            </tr>
        </table>
    </div>
    <div id="dlgNotes" title="Test Fail Notes" >
        <fieldset> 
            <table>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblNotesInstruction" style="font-style: italic" cssclass="labelText">Describe the circumstances of the failure: </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtNotes" runat="server" Height="100px" 
                            TextMode="MultiLine" cssclass="textBox FailNotes"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnSaveFail" runat="server" Text="Save" cssclass="standardButton"
                            onclick="btnSaveFail_Click" OnClientClick="return saveButton_click();" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" cssclass="standardButton" onclientclick="return false;"/>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <div id="dlgSelectBR" title="Related Business Rule" >
        <fieldset> 
            <table>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="Label3" style="font-style: italic" cssclass="labelText">Select Business Rule involved with test failure: </asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlSelectFailedBR" runat="server" CssClass = "dropDownList SelectFailedBRDDL">
                        </asp:DropDownList>        
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnSaveFailedBR" runat="server" Text="Save" cssclass="standardButton"
                            onclick="btnSaveFailedBR_Click" OnClientClick="return btnSaveFailedBRButton_Click();" />
                        <asp:Button ID="Button2" runat="server" Text="Cancel" cssclass="standardButton" onclientclick="return false;"/>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <asp:HiddenField ID="hdnCurrentTestCaseID" runat="server" />
    <asp:HiddenField ID="hdnCurrentTestStepID" runat="server" />
    <asp:HiddenField ID="hdnPrimaryTestCaseID" runat="server" />
    <asp:HiddenField ID="hdnFailedBusinessRuleID" runat="server" />
    <asp:HiddenField ID="hdnShowSelectBR" runat="server" />
    </form>
</body>
</html>
