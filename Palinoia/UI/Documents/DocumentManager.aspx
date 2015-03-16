<%@ Page title="Document Manager" Language="C#" AutoEventWireup="true" CodeBehind="DocumentManager.aspx.cs" 
    Inherits="Palinoia.UI.Documents.DocumentManager" MasterPageFile="~/Site.master" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <style type="text/css">
        a { white-space:normal !important; height: auto; padding:1px 2px; } 
        li > ins { vertical-align:top; }
        .jstree-hovered, .jstree-clicked { border:0; }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="enableViewDIV" runat="server">
        <table>
            <tr>
                <td>
                    <h1>Document Manager</h1>
                </td>
            </tr>
        </table>
        <div id="divSearch" class="searchDIV" runat="server" style="position:relative; display: none">
                <div>
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
                
                <div id="divAdvancedSearch" style="display: none">
                    <table style="width:100%">
                        <tr>
                            <td style="width:10%">[and/or]</td>
                            <td style="width:20%">
                                <asp:DropDownList ID="ddlSearchObject1" runat="server" CssClass="dropDownList searchObject1">

                                </asp:DropDownList>
                            </td>
                            <td style="width:15%">
                                <asp:DropDownList ID="ddlOperator1" runat="server" CssClass="dropDownList searchOperator1">

                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="txtValue1" runat="server" style="display:none;" cssclass="textBox txtSearchValue1"></asp:TextBox>
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
                                <asp:DropDownList ID="ddlOperator3" runat="server" CssClass="dropDownList searchOperator3">

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
                                <asp:Button ID="btnClearAdvancedSearch" runat="server" Text="Clear" cssclass="RegularControlButton" 
                                    onclientclick="return false;" />
                                <asp:Button ID="btnGridAdvancedSearch" runat="server" Text="Search" cssclass="RegularControlButton" 
                                    onclientclick="return false;" />
                                <asp:Button ID="btnTreeAdvancedSearch" runat="server" Text="Search" cssclass="RegularControlButton" 
                                    onclientclick="return false;" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <div id="documentTreesDIV" style="display: none">
        <table>
            <tr>
                <td align="right" valign="bottom">
                    <asp:Button ID="btnAddDocument" runat="server" Text="Add Document" onclientclick="return false;"  
                                cssclass="LongControlButton" ClientIDMode="Static" />
                </td>
            </tr>
        </table>
        <div id="TreePanelDIV">
            <div id="DocumentTree"  class="treeFull" style="height:500px; overflow-x: hidden; overflow-y: auto">
            
            </div>
        </div>
        <div id="searchResultTreePanelDIV">
            <asp:Panel ID="SearchResultsPanel1" runat="server" width = "100%" cssclass="adminGridPanel" 
                scrollBars="auto" style="float:left; padding-bottom: 10px;">
                SEARCH RESULTS TREE
                <div id="SearchResultsTree" class="treeSearchResults docSearch" style="height:500px;">
                </div>
            </asp:Panel>
        </div>
    </div>
    <div id="chapterEditDIV" style="display: none">
        <table>
        <tr>
            <td>
                <div>
                    <asp:Label ID="lblChapterName" runat="server" Text="Chapter Title: " CssClass="textBox"></asp:Label>
                    <asp:TextBox ID="txtChapterName" runat="server" cssclass="textBox"></asp:TextBox>
                    <a href="../BusinessRules/BRBrowser.aspx" target="_blank" style="float:right"
                        >Business Rules
                    </a>
                    <br />
                    <a href="../CustomerServiceMessages/CSMBrowser.aspx" target="_blank" style="float:right"
                        >Customer Service Messages
                    </a>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div id="editor">
	            </div>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Button ID="btnSaveChapter" runat="server" Text="Save" 
                    cssclass="standardButton"/>
                <asp:Button ID="btnDone" runat="server"  
                    Text="Done" cssclass="standardButton" onclientclick="return false;" />
            </td>
        </tr>
        </table>
    </div>
    <div id="dlgAddImage" title="Add Image" style="display: none">
        <fieldset> 
            <table>
                <tr>
                    <td align="right" style="float: left;">
                        <div id="div AddImageControlButtons" style="float: left;">
                            <asp:FileUpload ID="fileAddImage" class="FileUpload" runat="server" width="450px"/>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblImageDescription" runat="server" cssclass="labelText" Text="Description:"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtImageDescription" runat="server" TextMode="MultiLine" cssclass="textBox imageDescText"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        
                            <asp:Button ID="btnSaveAddImage" runat="server" Text="Load Image" onclick="btnSaveAddImage_Click" 
                                cssclass="LongControlButton" ClientIDMode="Static">
                            </asp:Button>
                            <asp:Button value="Cancel" id="btnAddImageCancel" runat="server" Text="Cancel"
                                style="float:right" onClientClick="return false;" CssClass="RegularControlButton"/>
                            
                    </td>
                </tr>
            </table>
            
        </fieldset>
    </div>
    <div id="dlgAddDocument" title="Add/Edit Document" style="display: none"> 
        <fieldset> 
            <div id="divDocumentControls"> 
                <asp:Label ID="lblDocTitle" runat="server" Text="Title:" ></asp:Label>  
                <asp:Textbox id="txtDocumentTitle" runat="server" class="docTitle" CssClass="textBox"/>
                <br />
                <asp:Label ID="Label1" runat="server" cssclass="labelText" Text="Type:"></asp:Label>  
                <asp:DropDownList ID="ddlDocumentType" runat="server" Width="85%">
                </asp:DropDownList>
                <br />
                <asp:Label ID="lblDocDescription" runat="server" cssclass="labelText" Text="Description:"></asp:Label>
                <asp:Textbox id="txtDocumentDescription" runat="server" Height="50px" 
                    TextMode="MultiLine" CssClass="textBox docDescription"/>
            </div>
            <table width="100%">
                <tr>
                    <td align="right" style="float: right;">
                        <div>
                            <div id="divAddDocumentControlButtons" style="float: left;">
                                <input type="button" name="btnNewDocumentSave" value="Save" id="btnNewDocumentSave" class="RegularControlButton" />
                                <input type="button" name="btnNewDocumentCancel" value="Cancel" id="btnNewDocumentCancel" class="RegularControlButton" />    
                                <asp:Button ID="btnEditDocumentSave" runat="server" Text="" onclientclick="return false;"  
                                    ClientIDMode="Static" style="display: none" cssclass="longButton"/>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
    <asp:HiddenField ID="hdnUserID" runat="server" />
    <asp:HiddenField ID="hdnDocumentID" runat="server" />
    <asp:HiddenField ID="hdnProjectID" runat="server" />
    <asp:HiddenField ID="hdnDocumentTypeID" runat="server" />
    <asp:HiddenField ID="hdnTestCaseID" runat="server" />
    <asp:HiddenField ID="hdnDisableDocumentAdd" runat="server" />
    <asp:HiddenField ID="hdnDisableDocumentEdit" runat="server" />
    <asp:HiddenField ID="hdnDisableDocumentDelete" runat="server" />
    <asp:HiddenField ID="hdnDisableDocumentView" runat="server" />
    <asp:HiddenField ID="hdnChapterID" runat="server" />
    <asp:HiddenField ID="hdnDisableChapterAdd" runat="server" />
    <asp:HiddenField ID="hdnDisableChapterEdit" runat="server" />
    <asp:HiddenField ID="hdnDisableChapterDelete" runat="server" />
    <asp:HiddenField ID="hdnDisableChapterView" runat="server" />
    <asp:HiddenField ID="hdnChapterSequence" runat="server" />
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
    <asp:HiddenField ID="hdnSearchTypeID" runat="server" Value="5"/>
    <asp:HiddenField ID="hdnSearchResults" runat="server" />
    <asp:HiddenField ID="hdnBasicSearchValue" runat="server" />
    <asp:HiddenField ID="hdnSearchToggle" runat="server" />
    <asp:HiddenField ID="hdnChapterEditMode" runat="server" />
    <asp:Button ID="btnEditDocument" runat="server" Text="" onclick="btnEditDocument_Click"  
                                    ClientIDMode="Static" style="display: none" />
    <asp:Button ID="btnDeleteDocument" runat="server" Text="" onclick="btnDeleteDocument_Click"  
                                    ClientIDMode="Static" style="display: none" />
    <asp:Button ID="btnDeleteChapter" runat="server" Text="" style="display: none" 
        ClientIDMode="Static"  onclick="btnDeleteChapter_Click"  />
</asp:Content>