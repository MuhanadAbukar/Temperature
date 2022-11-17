<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Temperature.Default" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="https://code.jquery.com/jquery-3.6.1.min.js" integrity="sha256-o88AwQnZB+VDvE9tvIXrMQaPlFFSUTR+nldQm1LuPXQ=" crossorigin="anonymous"></script>
    <script src="Scripts/highcharts/7.1.2/highcharts.js"></script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:DropDownList ID="DropDownList2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ChangedDropDown">
             </asp:DropDownList>
            <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ChangedDropDown">
              </asp:DropDownList>
            
            <asp:DropDownList ID="DropDownList3" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ChangedDropDown">
             </asp:DropDownList>

            <asp:GridView ID="GridView1" runat="server">
            </asp:GridView>
            <%--<asp:Chart ID="Chart1" runat="server" Width="488px">  
        <series>  
            <asp:Series Name="Series1" XValueMember="hour" YValueMembers="temperature" ChartType="Line">  
            </asp:Series>  
        </series>  
        <chartareas>  
            <asp:ChartArea Name="ChartArea1">  
            </asp:ChartArea>  
        </chartareas>  
</asp:Chart>--%>
            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>

        </div>
    </form>
</body>
</html>
