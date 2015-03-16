<%@ Page title="Test Cases" Language="C#" AutoEventWireup="true" CodeBehind="TestCases.aspx.cs"  
    Inherits="Palinoia.UI.TestCases.TestCases" MasterPageFile="~/Site.master" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script src="../../Scripts/jquery-cookie-1.4.0/jquery.cookie.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h1>Test Cases</h1>
    <div id="divSearch" class="searchDIV" runat="server" style="position:relative;">
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
                
            <div id="divAdvancedSearch" >
                <table>
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
                            <asp:TextBox ID="txtValue1" runat="server" CssClass="textBox txtSearchValue1"></asp:TextBox>
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
                            <asp:TextBox ID="txtValue2" runat="server" CssClass="textBox txtSearchValue2"></asp:TextBox>
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
                            <asp:TextBox ID="txtValue3" runat="server" CssClass="textBox txtSearchValue3"></asp:TextBox>
                            <select id="ddlSearchValue3" style="width:100%;display:none;" class="ddlSearchValue3"></select>
                            <input type="text" id="datepicker3" class="calTextValue3" style="width:100%;display:none;"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="ddlConnector4" runat="server" style="width:100%" class="searchConnector4">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSearchObject4" runat="server" style="width:100%" class="searchObject4">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlOperator4" runat="server" style="width:100%" class="searchOperator4">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="txtValue4" runat="server" CssClass="textBox txtSearchValue4"></asp:TextBox>
                            <select id="ddlSearchValue4" style="width:100%;display:none;" class="ddlSearchValue4"></select>
                            <input type="text" id="datepicker4" class="calTextValue4" style="width:100%;display:none;"/>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="right">
                            <asp:Button ID="btnClearAdvancedSearch" runat="server" Text="Clear" 
                                onclientclick="return false;" cssclass="RegularControlButton"/>
                            <asp:Button ID="btnGridAdvancedSearch" runat="server" Text="Search" 
                                onclientclick="return false;" cssclass="RegularControlButton" />
                            <asp:Button ID="btnTreeAdvancedSearch" runat="server" Text="Search" 
                                onclientclick="return false;" cssclass="RegularControlButton" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div id="TestCasesTreeDIV" >
        <table>
            <tr>
                <td align="right">
                    <asp:Button ID="btnRefresh" runat="server" Text="Refresh" onclientclick = "return false;" cssclass="standardButton"/>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblNoResults" runat="server" cssclass="labelText" Text=""></asp:Label>
                    <div id="TreePanelDIV">
                        <asp:Panel ID="TestCasesTreePanel" runat="server" cssclass="treePanel" 
                            scrollBars="auto" style="float:left; padding-bottom: 10px;">
                            <div id="treeTestCasesDIV" class="treeFull">
                            </div>    
                        </asp:Panel>
                    </div>
                    <div id="searchResultTreePanelDIV">
                        <asp:Panel ID="SearchResultsPanel1" runat="server" cssclass="treePanel" 
                            scrollBars="auto" style="float:left; padding-bottom: 10px;">
                            SEARCH RESULTS TREE
                            <div id="SearchResultsTree" class="treeSearchResults" style="height:500px;">
                            </div>
                        </asp:Panel>
                   </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="EditTestCaseDIV" >
        <table>
            <tr>
                <td width="10%">
                    <asp:Label ID="lblTCName" runat="server" cssclass="labelText" Text="Name: "></asp:Label>
                </td>
                <td width="80%">
                    <asp:TextBox ID="txtTestCaseName" runat="server" cssclass="textBox TestCaseName"></asp:TextBox>
                </td>
                <td width="10%">
                    <asp:Button ID="btnTestCaseSave" runat="server" Text="Save" CssClass="RegularControlButton"
                        onclick="btnTestCaseSave_Click">
                    </asp:Button>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblSection" runat="server" cssclass="labelText" Text="Section: "></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSections" runat="server" cssclass="dropDownList SectionsDDL">
                        </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btnDone" runat="server" Text="Done" 
                        CssClass="RegularControlButton" OnClientClick="return false;">
                    </asp:Button>
                </td>
            </tr>
        </table>
        <br />
        <div id="afterSaveEditDIV" class = "afterSaveEditDIV" runat="server" >
        <table>
            <tr>
                <td align="center" colspan="2">
                    <asp:Button ID="btnNeedRetest" runat="server" Text="Needs to be Retested" onClick="btnNeedRetest_Click" CssClass="LongControlButton"/>
                </td>
                
            </tr>
            <tr>
                <td>
                    <h3>PreConditions</h3>
                </td>
            </tr>
            <tr>
                <td style="width:80%">
                    <select disabled="disabled" id="preConditionsDisplay" style="width:100%; height: 120px" 
                        class="listboxPreConditionsDisplay" multiple="true">
                    </select>
                </td>
                <td valign="top">
                    <asp:Button ID="btnEditPreConditions" runat="server" Text="Edit Pre-Conditions" 
                        cssclass="LongControlButton" onclientclick="return false;" />
                </td>
            </tr>
        </table>
        <br />
        <table>
            <tr>
                <td>
                    <h3>Test Steps</h3>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="grdTestSteps" runat="server" Width="100%" cssClass="gridview testStepGrid" 
                        AutoGenerateColumns="False" CellPadding="4"  
                        onrowdatabound="grdTestSteps_RowDataBound" 
                        onrowdeleting="grdTestSteps_RowDeleting" onrowediting="grdTestSteps_RowEditing">
                        <AlternatingRowStyle CssClass="even" />
                        <Columns>
                            <asp:BoundField AccessibleHeaderText="ID" HeaderText="ID" ReadOnly="True" 
                                DataField="ID">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Width="50px" />
                            </asp:BoundField>
                            <asp:BoundField AccessibleHeaderText="Number" DataField="SeqNum" 
                                HeaderText="Number">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Width="50px" />
                            </asp:BoundField>
                            <asp:BoundField AccessibleHeaderText="Steps" HeaderText="Steps" 
                                DataField="Name" >
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Width="40%" />
                            </asp:BoundField>
                            <asp:BoundField AccessibleHeaderText="Notes" DataField="Notes" 
                                HeaderText="Notes">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle Width="35%" />
                            </asp:BoundField>
                            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" 
                                DeleteText="Remove">
                            <ItemStyle Width="50px" />
                            </asp:CommandField>
                        </Columns>
                        <%--<EditRowStyle BackColor="#2461BF" />
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#EFF3FB" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                        <SortedDescendingHeaderStyle BackColor="#4870BE" />--%>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                    <asp:Button ID="btnMoveUp" runat="server" cssclass="StandardButton" Text="Move Up" 
                        style="float:left" onclick="btnMoveUp_Click">
                    </asp:Button>
                    <asp:Button ID="btnMoveDown" runat="server" cssclass="StandardButton" Text="Move Down" 
                        style="float:left" onclick="btnMoveDown_Click">
                    </asp:Button>
                    <asp:Button ID="btnAddTestStep" runat="server" Text="Add Test Step" cssclass="LongControlButton" 
                        onclientclick="return false;" Style="float:right">
                    </asp:Button>
                    <br />
                </td>
        </tr>
        </table>
        </div>
    </div>

    <asp:Button ID="btnEditTC" runat="server" Text="" onclick="btnEditTC_Click" style="display: none" cssclass="standardButton"/>
    <asp:Button ID="btnDeleteTC" runat="server" Text="" style="display: none" onclick="btnDeleteTC_Click" cssclass="standardButton" />
    
    <asp:Button ID="btnDeletePreCon" runat="server" Text="" style="display: none" cssclass="standardButton"/>
    <asp:Button ID="btnDeletePostCon" runat="server" Text="" style="display: none" cssclass="standardButton" />
    <asp:Button ID="btnAddTC" runat="server" Text="" onclick="btnAddTC_Click" style="display: none" cssclass="standardButton"/>
    
    <asp:HiddenField ID="hdnTestCaseID" runat="server" />
    <asp:HiddenField ID="hdnDeleteID" runat="server" />
    <asp:HiddenField ID="hdnOriginalTestStepID" runat="server" />
    <asp:HiddenField ID="hdnTestStepID" runat="server" />
    <asp:HiddenField ID="hdnSelectedTestStepID" runat="server" />
    <asp:HiddenField ID="hdnProjectID" runat="server" />
    <asp:HiddenField ID="hdnTestCaseMode" runat="server"/>
    <asp:HiddenField ID="hdnTestStepMode" runat="server"/>
    <asp:HiddenField ID="hdnEditPreConditionID" runat="server" />
    <asp:HiddenField ID="hdnEditPostConditionID" runat="server" />
    <asp:HiddenField ID="hdnSectionID" runat="server"/>
    <asp:HiddenField ID="hdnDisableTCAdd" runat="server"/>
    <asp:HiddenField ID="hdnDisableTCEdit" runat="server"/>
    <asp:HiddenField ID="hdnDisableTCDelete" runat="server"/>
    <asp:HiddenField ID="hdnDisableTSAdd" runat="server"/>
    <asp:HiddenField ID="hdnDisableTSEdit" runat="server"/>
    <asp:HiddenField ID="hdnDisableTSDelete" runat="server"/>
    <asp:HiddenField ID="hdnDisablePreCDelete" runat="server"/>
    <asp:HiddenField ID="hdnDisablePreCAdd" runat="server"/>
    <asp:HiddenField ID="hdnDisablePostCDelete" runat="server"/>
    <asp:HiddenField ID="hdnDisablePostCAdd" runat="server"/>
    <asp:HiddenField ID="hdnRelatedBusinessRules" runat="server" />
    <asp:HiddenField ID="hdnPreConditionIDs" runat="server" />
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
    <asp:HiddenField ID="hdnSearchTypeID" runat="server" Value="5" />
    <asp:HiddenField ID="hdnSearchResults" runat="server" />
    <asp:HiddenField ID="hdnBasicSearchValue" runat="server" />
    <asp:HiddenField ID="hdnSearchToggle" runat="server" />
    <div id="dlgPreCondition" title="Pre-Condition" style="display:none;">
        <fieldset> 
            <table>
                <tr>
                    <td valign="top" width="40%">
                        <table> <%--Right Hand Tree--%>
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" cssclass="labelText" Text="PreConditions:"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <select ID="listboxPreConditions" runat="server" style="width:100%; height: 200px" 
                                        class="jstree-drop listboxPreConditions" multiple="true"></select>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btnMoveTCUp" runat="server" Text="Up" onClientClick="return false;" CssClass="RegularControlButton"/>
                                    <asp:Button ID="btnMoveTCDown" runat="server" Text="Down" onClientClick="return false;" CssClass="RegularControlButton"/>
                                    <asp:Button ID="btnRemovePreCTestCase" runat="server" Text="Remove" onclientclick="return false;" CssClass="RegularControlButton"/>
                                    <asp:Button ID="btnSavePreCon" runat="server" Text="Save" onclick="btnSavePreCondition_Click" CssClass="RegularControlButton"/>
                                    <input type="button" name="btnPreCCancel" value="Cancel" id="btnPreCCancel" class="RegularControlButton" />
                                </td>
                                
                            </tr>
                            
                        </table>
                    </td>
                    <td valign="top" width="60%"> <%--Right Hand Tree--%>
                        <asp:Label ID="Label3" runat="server" cssclass="HelpText" 
                            Text="Drag Test Cases into PreConditions area to add."></asp:Label>
                        <asp:Panel runat="server" ScrollBars="Auto" Width="100%">
                        <div id="testCasesTree2" class="treeTestCases2" style="height:400px;">
                            
                        </div>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            </fieldset>
    </div>
    <div id="dlgTestStep" title="Test Step" >
        <fieldset>
            <table>
                <tr style="width:100%">
                    <td>
                        <asp:Label ID="lblTestStep" runat="server" cssclass="labelText" Text="Test Step text:">
                        </asp:Label>
                    </td>
                </tr>
                <tr style="width:100%">
                    <td>
                        <div id="divNewTestStep">  
                            <asp:Textbox id="txtTestStep" runat="server" cssclass="textBox TestStepText"/>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <div id="divTestStepButtons" >
                            <input type="button" name="btnTestStepSave" value="Save" id="btnTestStepSave" class="RegularControlButton"/>
                            <input type="button" name="btnTestStepCancel" value="Cancel" id="btnTestStepCancel" class="RegularControlButton" />
                            <asp:Button ID="btnSaveTestStep" runat="server" Text="" style="display: none" cssclass="standardButton"
                                ClientIDMode="Static" onclick="btnSaveTestStep_Click" />
                        </div>
                        
                    </td>
                </tr>
                </table>
                <table>
                <tr>
                    <td valign="top" width="40%">
                        <table> <%--Right Hand Tree--%>
                            <tr>
                                <td>
                                    <asp:Label ID="lblRelatedBusinessRules" runat="server" cssclass="labelText" Text="Related Business Rules:"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ListBox ID="lbRelatedBusinessRules" runat="server" style="height: 200px" 
                                        class="jstree-drop relatedBusinessRuleListbox listBox" SelectionMode="Multiple"></asp:ListBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btnRemoveRelatedBusinessRule" runat="server" Text="Remove" onclientclick="return false;"
                                    cssclass="longButton"/>
                                </td>
                                
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblTestStepNotes" runat="server" cssclass="labelText" Text="Notes:"></asp:Label>        
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtTestStepNotes" runat="server" Rows="8" 
                                        Height="224px" Width="100%" TextMode="MultiLine" CssClass="textBox NotesText"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top" width="450px">
                        <asp:Label ID="lblBusinessRulesTree" runat="server" cssclass="HelpText" 
                            Text="Drag Business Rules into Related Test Steps area to create relationship.">
                        </asp:Label>
                        <div id = "treeContainer" style="height:475px;max-width: 400px;overflow-y: auto;overflow-x: hidden;">
                            <div id="BusinessRulesTree" class="treeBusinessRules" style="max-width: 360px;overflow-y: hidden;overflow-x: hidden;">
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
</asp:Content>