<%@ Page title="Error Log" Language="C#" AutoEventWireup="true" CodeBehind="adminErrorLog.aspx.cs" Inherits="Palinoia.UI.Admin.adminErrorLog" MasterPageFile="~/Site.master"%>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">

</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    <h1>Administration</h1>
    <br />
    <h2>Error Log</h2>
    <div style="position:relative;">
        <asp:Label ID="lblNoErrors" runat="server" Text=""></asp:Label>
         <table>
            <tr>
                <td align="right">
                    
                </td>
            </tr>
            <tr>
                <td>
                <asp:panel ID="Panel1" runat="server" ScrollBars="Auto" height="500px">
                <asp:GridView ID="grdErrors" runat="server" autopostback = "false"
                    AutoGenerateColumns="False" cssClass="gridview"
                    AlternatingRowStyle-CssClass="even" Width="100%" 
                        onrowdatabound="grdErrors_RowDataBound" >
                    <AlternatingRowStyle CssClass="even" />
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" >
                            <ItemStyle Width="40px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ProjectID" HeaderText="PID">
                            <ItemStyle Width="40px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Date" HeaderText="Date" />
                        <asp:BoundField DataField="Source" HeaderText="Source" />
                        <asp:BoundField DataField="Message" HeaderText="Message" />
                    </Columns>
                </asp:GridView>
                </asp:panel>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <input id="btnClearErrors" type="button" value="Clear Errors" cssclass="longButton"/>
                </td>
            </tr>
        </table>
    </div>
    <div id="dlgErrorDetail" title="Error Detail" >
        <fieldset> 
            <table>
                <tr>
                    <td style="width: 10%">
                        <asp:Label runat="server" ID="lblDate">Date:</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDate" runat="server" style="width: 100%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblProjectID">Project:</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtProject" runat="server" style="width: 100%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblUser">User:</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtUser" runat="server" style="width: 100%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblSource">Source:</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSource" class="SourceTextbox" runat="server" style="width: 100%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan = "2">
                        <asp:Label runat="server" ID="lblMessage">Message:</asp:Label>
                        <br />
                        <asp:TextBox ID="txtMessage" class="MessageTextbox" style="width:100%;height:100px" runat="server" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan = "2">
                        <asp:Label runat="server" ID="lblInnerException">Inner Exception:</asp:Label>
                        <br />
                        <asp:TextBox ID="txtInnerException" class="InnerExceptionTextbox" style="width:100%;height:100px" runat="server" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan = "2">
                        <asp:Label runat="server" ID="lblStackTrace">Stack Trace:</asp:Label>
                        <br />
                        <asp:TextBox ID="txtStackTrace" class="StackTraceTextbox" style="width:100%;height:100px" runat="server" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align = "right" colspan = "2">
                        <asp:Button ID="btnClose" runat="server" Text="Close" 
                            cssclass="standardButton" onclientclick="return false;"/>
                    </td>

                </tr>
            </table>
        </fieldset>
    </div>
    
</asp:Content>

