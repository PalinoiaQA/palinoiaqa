<%@ Page title="Business Rules" Language="C#" AutoEventWireup="true" CodeBehind="BusinessRules.aspx.cs" 
    Inherits="Palinoia.UI.BusinessRules.BusinessRules" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Frameset//EN" "http://www.w3.org/TR/html4/frameset.dtd">
<html>
    <head>
        <title>Business Rules</title>
        <script src="../../Scripts/jquery-1.8.3.min.js" type="text/javascript"></script>
        <script src="../../Scripts/jquery.validate.min.js" type="text/javascript"></script>
        <script src="../../Scripts/jquery.json-2.4.min.js" type="text/javascript"></script>
        <script src="../../Scripts/JSTree/jquery.jstree.js" type="text/javascript"></script>
        <script src="../../Scripts/jquery-ui-1.9.0.min.js" type="text/javascript"></script>
        <script src="../../Scripts/jquery-cookie-1.4.0/jquery.cookie.js" type="text/javascript"></script>
        <script src="../../Scripts/Palinoia/utility.js" type="text/javascript"></script>
        <script src="../../Scripts/Palinoia/BusinessRules/BusinessRules.js" type="text/javascript"></script>
        <script src="../../ckeditor/ckeditor.js" type="text/javascript"></script>
        <script src="../../Scripts/Palinoia/search.js" type="text/javascript"></script>
        <style type="text/css">
            a { white-space:normal !important; height: auto; padding:1px 2px; } 
            li > ins { vertical-align:top; }
            .jstree-hovered, .jstree-clicked { border:0; }
        </style>
    </head>

    <body>
        <form id="Form1" runat="server">
        <div id="enableViewDIV" runat="server">
        <h1 id="PageTitle">Business Rules</h1>
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
                                onclientclick="return false;" cssclass="RegularControlButton"/>
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
                        <td style="width:15%">
                            <asp:DropDownList ID="ddlSearchObject1" runat="server" CssClass="dropDownList searchObject1">
                            </asp:DropDownList>
                        </td>
                        <td style="width:15%">
                            <asp:DropDownList ID="ddlOperator1" runat="server" CssClass="dropDownList searchOperator1">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="txtValue1" runat="server" style="display:none;" 
                                cssclass="textBox txtSearchValue1"></asp:TextBox>
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
                            <asp:TextBox ID="txtValue2" runat="server" style="display:none;" CssClass="textBox txtSearchValue2"></asp:TextBox>
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
                            <asp:DropDownList ID="ddlOperator3" runat="server" style="width:100%" class="searchOperator3">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="txtValue3" runat="server" style="display:none;" CssClass="textBox txtSearchValue3"></asp:TextBox>
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
                            <asp:TextBox ID="txtValue4" runat="server" style="display:none;" CssClass="textBox txtSearchValue4"></asp:TextBox>
                            <select id="ddlSearchValue4" style="width:100%;display:none;" class="ddlSearchValue4"></select>
                            <input type="text" id="datepicker4" class="calTextValue4" style="width:100%;display:none;"/>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="right">
                            <asp:Button ID="btnClearAdvancedSearch" runat="server" Text="Clear" 
                                onclientclick="return false;" cssclass="RegularControlButton"/>
                            <asp:Button ID="btnGridAdvancedSearch" runat="server" Text="Search" 
                                onclientclick="return false;" cssclass="RegularControlButton"/>
                            <asp:Button ID="btnTreeAdvancedSearch" runat="server" Text="Search" 
                                onclientclick="return false;" cssclass="RegularControlButton"/>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div id="BusinessRuleTreeDIV" >
        <asp:Label ID="lblNoResults" runat="server" Text="" CssClass="label"></asp:Label>
        <table>
            <tr>
                <td width="40%"></td>
                <td align="right" width="60%"></td>
            </tr>
            <tr>
                <td >
                    <div id="TreePanelDIV">
                        <asp:Panel ID="BusinessRulesTreePanel" runat="server" cssclass="adminGridPanel"  
                            scrollBars="auto" style="float:left; padding-bottom: 10px;">
                            All Business Rules
                            <div id="BusinessRulesTree" class="treeFull" style="height:500px;">
                            </div>
                        </asp:Panel>
                        </div>
                        <div id="searchResultTreePanelDIV">
                        <asp:Panel ID="SearchResultsPanel1" runat="server" cssclass="adminGridPanel" 
                            scrollBars="auto" style="float:left; padding-bottom: 10px;">
                            SEARCH RESULTS TREE
                            <div id="SearchResultsTree" class="treeSearchResults" style="height:500px;">
                            </div>
                        </asp:Panel>
                        </div>
                    </td>
                    <td valign="top">
                        <div id="ObjTextDIV" class="ObjText" style = "vertical-align:top;"; ></div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="BusinessRuleEditDIV" style="float:left; width:100%;">
            <br />
            <div style="float:left;">
                <asp:Label runat="server" ID="lblBRName" cssclass="BRNameTitle"></asp:Label>
                <br />
                <asp:CheckBox ID="chkActive" runat="server" Text="Active" ClientIDMode="Static"/>
            </div>
            <div style="float:right;">
                <div>
                <asp:TextBox ID="txtBRName1" cssclass="BRNameText" runat="server" ></asp:TextBox>
                <asp:Label ID="lblBRName1" runat="server" cssclass="BRNameDot" >.</asp:Label>
                <asp:DropDownList ID="ddlBRNameSection" cssclass="BRNameSectionDDL" runat="server"></asp:DropDownList>
                <asp:Label ID="lblBRName3" runat="server" cssclass="BRNameDot" >.</asp:Label>
                <asp:TextBox ID="txtBRName3" cssclass="BRNameText" runat="server" ></asp:TextBox>
                <asp:Label ID="lblBRName4" runat="server" cssclass="BRNameDot">.</asp:Label>
                <asp:TextBox ID="txtBRName4" cssclass="BRNameText" runat="server"></asp:TextBox>
                <br />
                </div>
                <div style="padding-top: 5px">
                <asp:Label ID="lblStatus" runat="server" style="padding-top: 5px" cssclass="label">Status: </asp:Label>
                <asp:DropDownList ID="ddlBusinessRuleStatus" runat="server" cssclass="dropDownList"></asp:DropDownList>
                </div>
            </div>
            <br />
            <table frame="void">
                <tr>
                    <td>
                        <CKEditor:CKEditorControl ID="CKEditor1" runat="server" Toolbar="
                            Bold|Italic|Underline|Strike|-|Subscript|Superscript
                            NumberedList|BulletedList|-|Outdent|Indent
                            /
                            Styles|Format|Font|FontSize|TextColor|BGColor|-|About" 
                            ResizeDir="Vertical" ResizeEnabled="False" style="margin-left: 5px" Width="">
                        </CKEditor:CKEditorControl>
                    </td>
                </tr>
            </table>
            <br />
            <table frame="void">
                <tr>
                    <td align="right">
                        <asp:Button ID="btnSaveAddBusinessRule" runat="server" Text="Save" 
                            cssclass="standardButton" onclick="btnSaveAddBusinessRule_Click"  />
                        <asp:Button ID="btnSaveEditBusinessRule" runat="server" Text="Save" 
                            cssclass="standardButton" onclick="btnSaveEditBusinessRule_Click"  />
                        <asp:Button ID="btnCancel" runat="server"  
                            Text="Cancel" cssclass="standardButton" onclientclick="return false;"  />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="dlgDefectOwner" title="Select Defect Owner" style="display: none;">
        <fieldset> 
            <table>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblDefectOwner" CssClass="labelText">Owner: </asp:Label>
                    </td>
                    <td>
                         <asp:DropDownList ID="ddlDefectOwner" runat="server" cssclass="dropDownList defectOwnerDDL">
                         </asp:DropDownList>
                    </td>
                    
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <asp:Button ID="btnSaveDefectOwner" runat="server" Text="Save" 
                            cssclass="standardButton" onclientclick="return false" />
                        <asp:Button ID="btnCancelDefectOwner" runat="server" onclientclick="return false;" 
                            Text="Cancel" cssclass="standardButton" />
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <asp:CheckBox ID="chkAddDefect" runat="server" Text="Add Defect" ClientIDMode="Static" style="display: none"/>
        <asp:Button ID="btnEditBR" runat="server" Text="" style="display: none" 
            onclick="btnEditBR_Click" />
        <asp:Button ID="btnDeleteBR" runat="server" Text="" style="display: none" 
            onclick="btnDeleteBR_Click" />
        <asp:HiddenField ID="hdnBusinessRuleID" runat="server" />
        <asp:HiddenField ID="hdnProjectID" runat="server" />
        <asp:HiddenField ID="hdnMode" runat="server"/>
        <asp:HiddenField ID="hdnSectionID" runat="server"/>
        <asp:HiddenField ID="hdnDisableEdit" runat="server"/>
        <asp:HiddenField ID="hdnDisableDelete" runat="server"/>
        <asp:HiddenField ID="hdnDisableAdd" runat="server"/>
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
        <asp:HiddenField ID="hdnSearchTypeID" runat="server" Value="1" />
        <asp:HiddenField ID="hdnSearchResults" runat="server" />
        <asp:HiddenField ID="hdnBasicSearchValue" runat="server" />
        <asp:HiddenField ID="hdnSearchToggle" runat="server" />
        <asp:HiddenField ID="hdnBrowserOnly" runat="server" Value="1" />
        <asp:HiddenField ID="hdnDefectOwnerID" runat="server" />
        </form>
    </body>
</html>