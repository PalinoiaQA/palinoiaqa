<%@ Page title="Defects" Language="C#" AutoEventWireup="true" CodeBehind="Defects.aspx.cs" Inherits="Palinoia.UI.Defects.Defects" 
    MasterPageFile="~/Site.master"%>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h1>Defects</h1>
    <div id="divSearch" class="searchDIV" runat="server" style="position:relative;">
            <div>
            <asp:LinkButton ID="lbSearch" cssclass="searchLink" runat="server" Text="Advanced Search" onclientclick="return false;"/>
            <div id="divBasicSearch" style="display:none;">
                <table>
                    <tr>
                        <td style="width:100%">
                            <asp:TextBox ID="txtBasicSearch" runat="server" class = "txtBasicSearchValue" cssclass="textBox"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btnBasicSearch" runat="server" Text="Search" 
                                onclick="btnBasicSearch_Click" cssclass="RegularControlButton"/>
                        </td>
                        <td>
                            <asp:Button ID="btnClearBasicSearch" runat="server" Text="Clear" 
                                onclick="btnClearBasicSearch_Click" cssclass="RegularControlButton"/>
                        </td>
                    </tr>
                </table>
            </div>
                
            <div id="divAdvancedSearch" >
                <table>
                    <tr>
                        <td style="width:10%">[and/or]</td>
                        <td style="width:20%">
                            <asp:DropDownList ID="ddlSearchObject1" runat="server" class="searchObject1" CssClass="dropDownList">

                            </asp:DropDownList>
                        </td>
                        <td style="width:15%">
                            <asp:DropDownList ID="ddlOperator1" runat="server" class="searchOperator1" CssClass="dropDownList">

                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="txtValue1" runat="server" style="display:none;" CssClass="textBox txtSearchValue1"></asp:TextBox>
                            <select id="ddlSearchValue1" style="width:100%;display:none;" class="ddlSearchValue1"></select>
                            <input type="text" id="datepicker1" class="calTextValue1" style="width:100%;display:none;"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="ddlConnector2" runat="server" CssClass="dropDownList searchConnector2">

                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSearchObject2" runat="server" CssClass="dropDownList searchObject2">

                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlOperator2" runat="server" CssClass="dropDownList searchOperator2">

                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="txtValue2" runat="server" style="display:none;" 
                                 CssClass="textBox txtSearchValue2"></asp:TextBox>
                            <select id="ddlSearchValue2" style="width:100%;display:none;" class="ddlSearchValue2"></select>
                            <input type="text" id="datepicker2" class="calTextValue2" style="width:100%;display:none;"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="ddlConnector3" runat="server" CssClass="dropDownList searchConnector3">

                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSearchObject3" runat="server" CssClass="dropDownList searchObject3">

                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlOperator3" runat="server" CssClass="dropDownList searchOperator3">

                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="txtValue3" runat="server" style="display:none;" 
                                 CssClass="textBox txtSearchValue3"></asp:TextBox>
                            <select id="ddlSearchValue3" style="width:100%;display:none;" class="ddlSearchValue3"></select>
                            <input type="text" id="datepicker3" class="calTextValue3" style="width:100%;display:none;"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="ddlConnector4" runat="server" CssClass="dropDownList searchConnector4">

                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSearchObject4" runat="server" CssClass="dropDownList searchObject4">

                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlOperator4" runat="server" CssClass="dropDownList searchOperator4">

                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="txtValue4" runat="server" style="display:none;" 
                                 CssClass="textBox txtSearchValue4"></asp:TextBox>
                            <select id="ddlSearchValue4" style="width:100%;display:none;" class="ddlSearchValue4"></select>
                            <input type="text" id="datepicker4" class="calTextValue4" style="width:100%;display:none;"/>
                            
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="right">
                            <asp:Button ID="btnClearAdvancedSearch" runat="server" Text="Clear" 
                                onclientclick="return false;" cssclass="RegularControlButton"/>
                            <asp:Button ID="btnGridAdvancedSearch" runat="server" Text="Search" 
                                onclick="btnGridAdvancedSearch_Click" cssclass="RegularControlButton"/>
                            <asp:Button ID="btnTreeAdvancedSearch" runat="server" Text="Search" 
                                onclientclick="return false;" cssclass="RegularControlButton"/>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <br />
    <div id="searchResultsDIV">
        <asp:Label ID="lblNoResults" runat="server" cssclass="labelText" Text=""></asp:Label>
        <table>
            <tr>
                <td align="right">
                    <asp:Button ID="btnAddDefect" runat="server" Text="Add" 
                                onclientclick="return false;" cssclass="RegularControlButton"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="pnlDefectsGrid" runat="server" style="width:100%;position:relative;">
                        <asp:GridView ID="grdDefects" runat="server" AutoGenerateColumns="False" 
                            style="width:100%;" onrowediting="grdDefects_RowEditing" cssClass="gridview defectsGrid"
                            onrowdatabound="grdDefects_RowDataBound" 
                            onrowdeleting="grdDefects_RowDeleting">
                            <AlternatingRowStyle CssClass="even" />
                            <Columns>
                                <asp:BoundField AccessibleHeaderText="ID" DataField="ID" HeaderText="ID">
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                <ItemStyle Width="50px" />
                                </asp:BoundField>
                                <asp:BoundField AccessibleHeaderText="Status" DataField="DefectStatusID" 
                                    HeaderText="Status" >
                                <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField AccessibleHeaderText="Type" DataField="DefectTypeID" 
                                    HeaderText="Type" >
                                <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField AccessibleHeaderText="Owner" DataField="OwnerID" 
                                    HeaderText="Owner" >
                                <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField AccessibleHeaderText="Name" DataField="Name" 
                                    HeaderText="Name" >
                                <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField AccessibleHeaderText="Date Created" DataField="DateCreated" 
                                    HeaderText="Date Created" >
                                <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:CommandField EditText="View" ShowCancelButton="False" 
                                    ShowEditButton="True" CancelText="" InsertText="" NewText="" 
                                    SelectText="" UpdateText="" ShowDeleteButton="True" />
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
    <div id="defectDetailsDIV" >
        <table>
            <tr>
                <td style="width:10%">
                    <asp:Label ID="lblID" runat="server" cssclass="labelText IDControl" Text="ID: "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtID" runat="server" DISABLED="true" CssClass="textBox IDControl"></asp:TextBox>
                </td>
                <td align="right">
                    <asp:Button ID="btnSave" runat="server" Text="Save" onclick="btnSave_Click" cssclass="RegularControlButton"/>
                    <asp:Button ID="btnClose" runat="server" Text="Cancel" onclick="btnClose_Click" cssclass="RegularControlButton"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblName" runat="server" cssclass="labelText" Text="Name: "></asp:Label>
                </td>
                <td colspan = "2">
                    <asp:TextBox ID="txtName" runat="server" cssclass="textBox"></asp:TextBox>
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblDescription" runat="server" cssclass="labelText" Text="Description: "></asp:Label>
                </td>
                <td colspan = "2">
                    <%--<asp:TextBox ID="txtDescription" runat="server" style="height:150px" 
                        TextMode="MultiLine" CssClass="textBox descriptionControl"></asp:TextBox>--%>
                    <CKEditor:CKEditorControl ID="CKEditor1" runat="server" class = "textedit descriptionControl"
                        Toolbar="
                        Bold|Italic|Underline|-|NumberedList|BulletedList|-|Outdent|Indent|Font|FontSize|TextColor" 
                        ResizeDir="Vertical" ResizeEnabled="False" style="margin-left: 5px; height: 125px;" >
                    </CKEditor:CKEditorControl>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblOwner" runat="server" cssclass="labelText" Text="Owner: "></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlOwner" runat="server" CssClass="dropDownList detailDDL ddlOwner">
                    </asp:DropDownList>
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblPriority" runat="server" cssclass="labelText" Text="Priority: "></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlPriority" runat="server" CssClass="dropDownList detailDDL ddlPriority">
                    </asp:DropDownList>
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblType" runat="server" cssclass="labelText" Text="Type: "></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddltype" runat="server" CssClass="dropDownList detailDDL ddlType">
                    </asp:DropDownList>
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblStatus" runat="server" cssclass="labelText" Text="Status: "></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="dropDownList detailDDL ddlStatus">
                    </asp:DropDownList>
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" cssclass="labelText calDateCreated" Text="Created: "></asp:Label></td>
                <td>
                    <input type="text" id="txtCreated" class="calDateCreated" style="width:25%;" />
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" cssclass="labelText" Text="Completed: "></asp:Label>
                </td>
                <td>
                    <input type="text" id="txtCompleted" class="calDateCompleted" style="width:25%;" />
                </td>
                <td>&nbsp;</td>
            </tr>
        </table>
        <br />
        <table width="75%">
            <tr>
                <td>
                <asp:Label ID="lblComments" runat="server" cssclass="labelText CommentControl" Text="Comments:"></asp:Label>
                </td>
                <td align="right">
                    <asp:Button ID="btnAddComment" class="CommentControl" runat="server" Text="Add Comment" OnClientClick="return false;" />
                </td>
                
            </tr>
            <tr>
                <td colspan = "2">
                    <%--<asp:TextBox ID="txtComments" class="CommentControl" runat="server" style="width:100%;height:200px" 
                        TextMode="MultiLine"></asp:TextBox>--%>
                    <%--<CKEditor:CKEditorControl ID="CKEditorComments" runat="server" class = "textedit descriptionControl"
                        Toolbar="
                        Bold|Italic|Underline|-|NumberedList|BulletedList|-|Outdent|Indent|Font|FontSize|TextColor" 
                        ResizeDir="Vertical" ResizeEnabled="False" style="margin-left: 5px; height: 200px;" >
                    </CKEditor:CKEditorControl>--%>
                    <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" style="height: 155px">
                    <div border="1" style="height: 200px;" runat="server" id="divComments"></div>
                    </asp:Panel>
                </td>
            </tr>
        
        </table>
    </div>
    
    <div id="dlgAddComment" title="Add Comment" style="display:none;">
        <fieldset> 
            <table>
                <tr>
                    <td>
                        <asp:TextBox ID="txtComment" cssclass="addCommentTextbox" 
                            style="height: 200px;width: 380px;" runat="server" TextMode="MultiLine">
                        </asp:TextBox><br />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Button ID="btnSaveComment" runat="server" Text="Save" 
                            onclick="btnSaveComment_Click"  CssClass="RegularControlButton"/>
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" onclientclick="return false;" CssClass="RegularControlButton"/>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <asp:HiddenField ID="hdnDefectID" runat="server" />
    <asp:HiddenField ID="hdnDateCompleted" runat="server" />
    <asp:HiddenField ID="hdnDateCreated" runat="server" />
    <asp:HiddenField ID="hdnProjectID" runat="server" />
    <asp:HiddenField ID="hdnSearchObjectDDL1" runat="server" Value="0"/>
    <asp:HiddenField ID="hdnSearchObjectDDL2" runat="server" Value="0"/>
    <asp:HiddenField ID="hdnSearchObjectDDL3" runat="server" Value="0"/>
    <asp:HiddenField ID="hdnSearchObjectDDL4" runat="server" Value="0"/>
    <asp:HiddenField ID="hdnConnector2" runat="server" Value="0" />
    <asp:HiddenField ID="hdnConnector3" runat="server" Value="0"/>
    <asp:HiddenField ID="hdnConnector4" runat="server" Value="0"/>
    <asp:HiddenField ID="hdnOperator1" runat="server" Value="0"/>
    <asp:HiddenField ID="hdnOperator2" runat="server" Value="0"/>
    <asp:HiddenField ID="hdnOperator3" runat="server" Value="0"/>
    <asp:HiddenField ID="hdnOperator4" runat="server" Value="0"/>
    <asp:HiddenField ID="hdnTextValue1" runat="server" Value="0"/>
    <asp:HiddenField ID="hdnTextValue2" runat="server" Value="0"/>
    <asp:HiddenField ID="hdnTextValue3" runat="server" Value="0"/>
    <asp:HiddenField ID="hdnTextValue4" runat="server" Value="0"/>
    <asp:HiddenField ID="hdnDDLValue1" runat="server" Value="0"/>
    <asp:HiddenField ID="hdnDDLValue2" runat="server" Value="0"/>
    <asp:HiddenField ID="hdnDDLValue3" runat="server" Value="0"/>
    <asp:HiddenField ID="hdnDDLValue4" runat="server" Value="0"/>
    <asp:HiddenField ID="calTextValue3" runat="server" Value="0"/>
    <asp:HiddenField ID="hdnCalendarValue1" runat="server" Value="0"/>
    <asp:HiddenField ID="hdnCalendarValue2" runat="server" Value="0"/>
    <asp:HiddenField ID="hdnCalendarValue3" runat="server" Value="0"/>
    <asp:HiddenField ID="hdnCalendarValue4" runat="server" Value="0"/>
    <asp:HiddenField ID="hdnDataType1" runat="server" Value="0"/>
    <asp:HiddenField ID="hdnDataType2" runat="server" Value="0"/>
    <asp:HiddenField ID="hdnDataType3" runat="server" Value="0"/>
    <asp:HiddenField ID="hdnDataType4" runat="server" Value="0"/>
    <asp:HiddenField ID="hdnSearchResults" runat="server" />
    <asp:HiddenField ID="hdnSearchToggle" runat="server" />
    <asp:HiddenField ID="hdnSearchTypeID" runat = "server" Value="3" />
    <asp:HiddenField ID="hdnBasicSearchValue" runat="server" />
</asp:Content>
