<%@ Page title="CSM Editor" Language="C#" AutoEventWireup="true" CodeBehind="CustomerServiceMessages.aspx.cs" 
    Inherits="Palinoia.UI.CustomerServiceMessages.CustomerServiceMessages" MasterPageFile="~/Site.master" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script src="../../Scripts/jquery-cookie-1.4.0/jquery.cookie.js" type="text/javascript"></script>
    <style type="text/css">
        a { white-space:normal !important; height: auto; padding:1px 2px; } 
        li > ins { vertical-align:top; }
        .jstree-hovered, .jstree-clicked { border:0; }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <h1 id="PageTitle">Customer Service Messages</h1>
    <div id="enableViewDIV" runat="server">
        <div id="divSearch" class="searchDIV" runat="server" style="position:relative;">
            <asp:LinkButton ID="lbSearch" cssclass="searchLink" runat="server" Text="Advanced Search" onclientclick="return false;"/>
            <div id="divBasicSearch" style="display:none;">
                <table>
                    <tr>
                        <td style="width:100%">
                            <asp:TextBox ID="txtBasicSearch" runat="server" cssclass="textBox txtBasicSearchValue"></asp:TextBox>
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
                        <td style="width:25%">
                            <asp:DropDownList ID="ddlSearchObject1" runat="server" CssClass="dropDownList searchObject1">

                            </asp:DropDownList>
                        </td>
                        <td style="width:15%">
                            <asp:DropDownList ID="ddlOperator1" runat="server" CssClass="dropDownList searchOperator1">

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
                            <asp:TextBox ID="txtValue2" runat="server" style="display:none;" CssClass="textBox txtSearchValue2">
                            </asp:TextBox>
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
                            <asp:TextBox ID="txtValue4" runat="server" style="display:none;" CssClass="textBox txtSearchValue4">
                            </asp:TextBox>
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
        <div id="CSMTree" >
            <asp:Label ID="lblNoResults" runat="server" cssclass="labelText" Text=""></asp:Label>
            <div id="TreePanelDIV" style="max-width: 40%">
                ALL CUSTOMER SERVICE MESSAGES
                <div id="CSMTreeDIV" class="treeCSMs" style="width:100%; float:left; max-height:500px; overflow: auto">
                </div>
            </div>
            <div id="searchResultTreePanelDIV" style="max-width: 40%">
                SEARCH RESULTS TREE
                <div id="SearchResultsTree" class="treeSearchResults" style="width:100%; float:left; max-height:500px; overflow: auto">
                </div>
            </div>
            <div id="ObjTextDIV" class="ObjText JQueryDebug" 
                style="vertical-align:top; max-height: 500px; margin-left: 40%; padding-left:10px">
            </div>
       </div>
    <div id="CSMEditDIV" style="float:left; width:100%;">
        <div style="float:right;">
            <div>
            <asp:TextBox ID="txtCSMName1" cssclass="CSMNameText" runat="server" ></asp:TextBox>
            <asp:Label ID="lblCSMName1" runat="server" cssclass="CSMNameDot" style="padding-left: 5px; padding-right: 5px;">.</asp:Label>
            <asp:DropDownList ID="ddlCSMNameSection" cssclass="CSMNameSectionDDL" runat="server"></asp:DropDownList>
            <asp:Label ID="lblCSMName3" runat="server" cssclass="CSMNameDot" style="padding-left: 5px; padding-right: 5px;">.</asp:Label>
            <asp:TextBox ID="txtCSMName3" cssclass="CSMNameText" runat="server"></asp:TextBox>
            <asp:Label ID="lblCSMName4" runat="server" cssclass="CSMNameDot" style="padding-left: 5px; padding-right: 5px;">.</asp:Label>
            <asp:TextBox ID="txtCSMName4" cssclass="CSMNameText" runat="server" ></asp:TextBox>

            </div>
        </div>
        <div >
            <table>
                <tr>
                    <td width="25%">
                        <asp:Label runat="server" ID="lblCSMName" cssclass="CSMNameTitle"></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkActive" runat="server" Text="Active" ClientIDMode="Static"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Type" style="padding-left:10px;" cssclass="labelText"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCSMType" runat="server" CssClass="dropDownList"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label3" runat="server" cssclass="labelText" Text="Response Type" style="padding-left:10px;"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCSMResponseType" runat="server" CssClass="dropDownList"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" cssclass="labelText" style="padding-left: 10px; padding-right: 10px;">Status: </asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCSMStatus" runat="server" CssClass="dropDownList"></asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
        <table>
            <tr>
                <td>
                    <CKEditor:CKEditorControl ID="CKEditor1" runat="server" Toolbar="
                        Source|Bold|Italic|Underline|Strike|Subscript|Superscript|-|Cut|Copy|Paste|PasteFromWord|-|Find|Replace|SpellCheck|
                        NumberedList|BulletedList|Outdent|Indent|Blockquote|CreateDiv|SpecialChar|
                        /
                        Styles|Format|Font|FontSize|TextColor|BGColor|JustifyLeft|JustifyCenter|JustifyRight|JustifyBlock|-|Maximize|"
                        ResizeDir="Vertical" ResizeEnabled="False" style="margin-left: 5px" Width="">
                    </CKEditor:CKEditorControl>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td align="right">
                    <asp:Button ID="btnSaveAddCSM" runat="server" Text="Save" 
                        cssclass="RegularControlButton" onclick="btnSaveAddCSM_Click" />
                    <asp:Button ID="btnSaveEditCSM" runat="server" Text="Save" 
                        cssclass="RegularControlButton" onclick="btnSaveEditCSM_Click" />
                    <asp:Button ID="btnEditCSMCancel" runat="server"  
                        Text="Cancel" cssclass="RegularControlButton" onclientclick="return false;" />
                </td>
            </tr>
        </table>
    </div>
    <asp:Button ID="btnEditCSM" runat="server" Text="" style="display: none" 
        onclick="btnEditCSM_Click" />
    <asp:Button ID="btnDeleteCSM" runat="server" Text="" style="display: none" 
        onclick="btnDeleteCSM_Click" />
    <asp:HiddenField ID="hdnCSMID" runat="server" />
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
    <asp:HiddenField ID="hdnSearchTypeID" runat="server" Value="2" />
    <asp:HiddenField ID="hdnSearchResults" runat="server" />
    <asp:HiddenField ID="hdnBasicSearchValue" runat="server" />
    <asp:HiddenField ID="hdnSearchToggle" runat="server" />   
    </div>
</asp:Content>