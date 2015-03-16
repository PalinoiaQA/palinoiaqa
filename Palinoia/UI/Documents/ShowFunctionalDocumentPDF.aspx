<%@ Page title="Functional Document" Language="C#" AutoEventWireup="true" CodeBehind="ShowFunctionalDocumentPDF.aspx.cs" Inherits="Palinoia.UI.Documents.ShowFunctionalDocumentPDF" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        $(document).ready(function () {
            //$(document).attr("title", "New Title");
            $('title').text('New title');
        });
        
    </script>
</head>
    <body>
        <form id="form1" runat="server">
            <div>
                <asp:Label ID="lblError" runat="server" Text="" cssclass="Error"></asp:Label>
            </div>
        </form>
    </body>
</html>
