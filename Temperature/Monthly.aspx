<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Monthly.aspx.cs" Inherits="Temperature.Monthly" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
      <form id="form1" runat="server">
         <div>
            <h1>Get data from any month</h1>
            <asp:Button ID="Day" runat="server" Text="Data from the last 24 hours" OnClick="Month_Click" />
            <asp:Button ID="Month" runat="server" Text="Monthly Data" OnClick="Month_Click" />
            <asp:Button ID="Year" runat="server" Text="Yearly Data" OnClick="Month_Click" />
            <br />
            <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ChangedDropDown">
            </asp:DropDownList>
            <asp:DropDownList ID="DropDownList3" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ChangedDropDown">
            </asp:DropDownList>
            <br />
            <asp:Button ID="Increment" runat="server" Text="<" OnClick="Increment_Click" />
            <asp:Button ID="Descend" runat="server" Text=">" OnClick="Descend_Click" />
            <asp:GridView ID="GridView2" runat="server" BackColor="#CCCCCC" AutoGenerateColumns="false" BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px" CellPadding="4" CellSpacing="2" ForeColor="Black">
               <FooterStyle BackColor="#CCCCCC" />
               <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
               <PagerStyle BackColor="#CCCCCC" ForeColor="Black" HorizontalAlign="Left" />
               <RowStyle BackColor="White" />
               <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
               <SortedAscendingCellStyle BackColor="#F1F1F1" />
               <SortedAscendingHeaderStyle BackColor="#808080" />
               <SortedDescendingCellStyle BackColor="#CAC9C9" />
               <SortedDescendingHeaderStyle BackColor="#383838" />
                 <Columns>
                    <asp:BoundField DataField="Day" HeaderText="Day" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Max" HeaderText="Highest Temperature of the day" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="MaxHour" HeaderText="Time of the highest temperature" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Min" HeaderText="Lowest Temperature of the day" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="MinHour" HeaderText="Time of the lowest temperature" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Average" HeaderText="Average Temperature of the day" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>
                    </asp:BoundField>
                     </Columns>
            </asp:GridView>
            <asp:Label ID="Max" runat="server" Text=""></asp:Label>
            <br/>
            <asp:Label ID="MaxHour" runat="server" Text=""></asp:Label>
            <br />
            <br />
            <asp:Label ID="Min" runat="server" Text=""></asp:Label>
            <br/>
            <asp:Label ID="MinHour" runat="server" Text=""></asp:Label>
            <br />
            <br />
            <asp:Label ID="Average" runat="server" Text=""></asp:Label>
         </div>
      </form>
   </body>
</html>
