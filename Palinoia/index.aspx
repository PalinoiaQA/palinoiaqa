<%@ Page title = "PalinoiaQA Home" Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="Palinoia.index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="Scripts/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script src="Scripts/Palinoia/index.js" type="text/javascript"></script>
    <script>
        (function (i, s, o, g, r, a, m) {
            i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
                (i[r].q = i[r].q || []).push(arguments)
            }, i[r].l = 1 * new Date(); a = s.createElement(o),
  m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
        })(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');

        ga('create', 'UA-59326288-1', 'auto');
        ga('send', 'pageview');

</script>
    <title>PalinoiaQA</title>
    
    <style type="text/css">

h2
	{margin-top:10.0pt;
	margin-right:0in;
	margin-bottom:0in;
	margin-left:0in;
	margin-bottom:.0001pt;
	line-height:115%;
	page-break-after:avoid;
	font-size:13.0pt;
	font-family:"Cambria","serif";
	color:#4F81BD;
	}
    </style>
    
</head>
<body bgcolor="White">
    <form id="form1" runat="server">
    <div>
        <table id="Header1" style="width:100%;" 
            bgcolor="#336699">
            <tr>
                <td align="left" valign="top">
                    <h1 style="color:White;padding-left:5px;">PalinoiaQA</h1>
    
                </td>
                <td align="right" valign="bottom">
                    <p style="color:White;font-style:italic;padding-right:5px;"> design...development...documentation...testing...perfection!</p>
                </td>
            </tr>
        </table>
        <br />
        <table ID="Table1" runat="server" style="width: 100%">
            <tr>
                <td style="width:10%">
                    <p>
            
                    </p>
                </td>
                <td style="width:80%">
                    <p>
                        &nbsp;</p>
                    <p class="MsoNormal">
                        PalinoiaQA is a suite of database driven tools that bring together all the 
                        building blocks of a software application:<span style="mso-spacerun:yes">&nbsp;
                        </span>Design, Documentation, Development Tracking, Testing, and Defect 
                        Tracking.<span style="mso-spacerun:yes">&nbsp; </span>Palinoia maintains the 
                        close relationships these building blocks have with each other.<span 
                            style="mso-spacerun:yes">&nbsp; </span>For example, business rules 
                        describing the basic functionality of a new application are related to actual 
                        code that provides that functionality.<span style="mso-spacerun:yes">&nbsp;
                        </span>After the code is developed, it must be tested.<span 
                            style="mso-spacerun:yes">&nbsp; </span>Test Cases are created by the 
                        developer as the code is created.<span style="mso-spacerun:yes">&nbsp; </span>
                        Those test cases are related to the code and the code is related to business 
                        rules.<span style="mso-spacerun:yes">&nbsp; </span>Palinoia allows for a ‘chain 
                        of custody’ so to speak throughout the design, development, and testing of 
                        software.<o:p></o:p></p>
                    <h2>
                        Business Rule and Customer Service Message creation and management<o:p></o:p></h2>
                    <p class="MsoNormal">
                        <o:p>&nbsp;Business Rules are the building blocks of software design.&nbsp; Palinoia 
                        allows for the creation, editing, and grouping of rules into sections or 
                        &quot;screens&quot; that details the actions that the software is to perform based on user 
                        or internal events.&nbsp; These business rules are then linked to developers, 
                        test steps, defects, and documents within the project.&nbsp; As business rules 
                        are added or edited, users can be notified, test case results changed.&nbsp; All 
                        documents that reference that business rule will be automatically updated as 
                        they are dynamically generated.</o:p></p>
                    <p class="MsoNormal">
                        Customer<o:p> Service Messages are how an application communicates with the 
                        user.&nbsp; Palinoia allows for the creation and editing of this text and, as 
                        with business rules, all documents will be up to date every time they are 
                        generated.</o:p></p>
                    <h2>
                        Defect Tracking Management<o:p></o:p></h2>
                    <p class="MsoNormal">
                        In Palinoia, Defects are more than just errors that result in failed tests.<span 
                            style="mso-spacerun:yes">&nbsp; </span>Defects could be enhancement 
                        requests, review requests, or any other task that needs to be tracked and 
                        completed in the course of a software project.<span style="mso-spacerun:yes">&nbsp;
                        </span>A defect can also refer to a brand new development need.<span 
                            style="mso-spacerun:yes">&nbsp; </span>When a Business Rule is created there 
                        will eventually be code written, tested, and implemented to fulfill the needs of 
                        the business rule.<span style="mso-spacerun:yes">&nbsp; </span>Palinoia 
                        automatically creates and assigns a defect whenever a new business rule is 
                        created.<span style="mso-spacerun:yes">&nbsp; </span>That defect represents a 
                        need for implementation of code and the defect is associated with the developer 
                        who will be creating that code.<span style="mso-spacerun:yes">&nbsp; </span>
                        Since the defect is related to or “owned” by a developer, the associated 
                        business rule is also related to that developer.<span style="mso-spacerun:yes">&nbsp;
                        </span>If the business rule is updated, the developer will receive an email that 
                        the rule was changes and detail what the change actually was.<span 
                            style="mso-spacerun:yes">&nbsp; </span>If a test case involving the business 
                        rule fails, the developer is notified of the failure.<o:p></o:p></p>
                    <h2>
                        Test Case creation<o:p></o:p></h2>
                    <p class="MsoNormal">
                        Test cases are created simply in the Palinoia UI and test case documents are 
                        generated automatically on demand when they are needed.<span 
                            style="mso-spacerun:yes">&nbsp; </span>When implementing a business rule, 
                        developers typically test their code numerous times during the course of 
                        development.<span style="mso-spacerun:yes">&nbsp; </span>Palinoia provides an 
                        interface where the developer can document the steps they take to verify that 
                        their code is working and these test steps are the building blocks of test 
                        cases.<o:p></o:p></p>
                    <h2>
                        Test Runner<o:p></o:p></h2>
                    <p class="MsoNormal">
                        Palinoia contains a Test Runner app that allows QA testers to test any and all 
                        test cases created for the project.<span style="mso-spacerun:yes">&nbsp; </span>
                        Test Runner records the success and failure of each test step and automatically 
                        creates defects and assigns the defect owner when tests fail.<o:p>&nbsp; The 
                        Test Runner application runs is a seperate floating window that can be 
                        positioned and resized anywhere on the screen so that it can run alongside the 
                        software being tested.&nbsp; All testing results are updated dynamically as the 
                        QA tester actively tests the application being developed. </o:p>
                    </p>
                    <h2>
                        Documentation creation and management<o:p></o:p></h2>

                    <p class="MsoNormal">
                        Documentation is automated as much as possible.<span style="mso-spacerun:yes">&nbsp;
                        </span>All documents are created dynamically when needed with the most up to 
                        date information in the database.<span style="mso-spacerun:yes">&nbsp; </span>
                        Generated documents are never out of date.<span style="mso-spacerun:yes">&nbsp;
                        </span>The latest business rule references and text are used for each document 
                        each time it is needed.<span style="mso-spacerun:yes">&nbsp; </span>The Palinoia 
                        Document Manager can be used for any document that could benefit from 
                        referencing business rules or customer service messages including Functional 
                        documents, Technical documents, and even User Manuals.<span 
                            style="mso-spacerun:yes">&nbsp; </span><o:p></o:p>
                    </p>
                </td>
                <td style="width:10%">
                    <p>
            
                    </p>
                </td>
            </tr>
            <tr >
                <td colspan = "3" align="center" style="padding-top:20px;padding-bottom:20px;">
                    <asp:Button ID="btnDemo2" runat="server" Text="Live Demo" OnClientClick="return false;" />
                </td>
            </tr>
        </table>
        <table ID="Table2" runat="server" style="width: 100%">
            <tr>
                <td style="width:10%"></td>
                <td style="width:80%">
                    PalinoiaQA is an OpenSource project hosted by <a href="http://code.google.com">Google Code</a>
                </td>
                <td style="width:10%"></td>
            </tr>
            <tr>
                <td style="width:10%"></td>
                <td style="width:80%">
                    PalinoiaQA Source: <a href="https://code.google.com/p/palinoia/">https://code.google.com/p/palinoia/</a> 
                </td>
                <td style="width:10%"></td>
            </tr>
            <tr>
                <td style="width:10%"></td>
                <td style="width:80%">
                    PalinoiaQA utilizes the following technologies:
                </td>
                <td style="width:10%"></td>
            </tr>
            <tr>
                <td style="width:10%"></td>
                <td style="padding-left:10px;width:80%">
                    * C# ASP.NET 4.0 
                </td>
                <td style="padding-top:0px;padding-bottom:0px;width:10%"></td>
            </tr>
            <tr>
                <td style="width:10%"></td>
                <td style="padding-left:10px;width:80%">
                    * jQuery 1.8.3
                </td>
                <td style="width:10%"></td>
            </tr>
            <tr>
                <td style="width:10%"></td>
                <td style="padding-left:10px;width:80%">
                    * jQuery UI 1.9.0
                </td>
                <td style="width:10%"></td>
            </tr>
            <tr>
                <td style="width:10%"></td>
                <td style="padding-left:10px;width:80%">
                    * jQuery.json 2.4
                </td>
                <td style="width:10%"></td>
            </tr>
            <tr>
                <td style="width:10%"></td>
                <td style="padding-left:10px;width:80%">
                    * <a href="http://www.jstree.com/">jsTree</a> jquery tree plugin
                </td>
                <td style="width:10%"></td>
            </tr>
            <tr>
                <td style="width:10%"></td>
                <td style="padding-left:10px;width:80%">
                    * <a href="http://ckeditor.com/">CKEditor</a> open source license
                </td>
                <td style="width:10%"></td>
            </tr>
            <tr>
                <td style="width:10%"></td>
                <td style="padding-left:10px;width:80%">
                    * <a href="http://www.sqlite.org/">SQLite</a> open source database
                </td>
                <td style="width:10%"></td>
            </tr>
            <tr>
                <td style="width:10%"></td>
                <td style="padding-left:10px;width:80%">
                    * <a href="http://system.data.sqlite.org/index.html/doc/trunk/www/index.wiki">System.Data.SQLite</a> an ADO.NET provider for SQLite 
                </td>
                <td style="width:10%"></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
