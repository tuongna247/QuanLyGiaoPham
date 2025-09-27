<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewReport.aspx.cs" Inherits="HTTLVN.QLTLH.Views.ViewReport" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:DropDownList ID="ddlSource" runat="server">
                <asp:ListItem Text="" > </asp:ListItem>
                <asp:ListItem> </asp:ListItem>
                <asp:ListItem> </asp:ListItem>
                <asp:ListItem> </asp:ListItem>
                <asp:ListItem> </asp:ListItem>
            </asp:DropDownList>
            <asp:Button ID="btnSearch" runat="server" Text="Button" />
        </div>
        <div>
            <CR:CrystalReportViewer ID="rpView" runat="server" AutoDataBind="true" />
        </div>
    </form>
</body>
</html>
