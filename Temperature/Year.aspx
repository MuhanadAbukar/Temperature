<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Year.aspx.cs" Inherits="Temperature.Year" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <h1>Get data from any year</h1>
        <div>
            <asp:Button ID="Day" runat="server" Text="The Last 24 hours" OnClick="Month_Click" />
            <asp:Button ID="Daily" runat="server" Text="Data from any day" OnClick="Month_Click" />
            <asp:Button ID="Monthly" runat="server" Text="Data from any month" OnClick="Month_Click" />
            <asp:Button ID="Year1" runat="server" Text="Yearly Data" OnClick="Month_Click" />
            <br />
            <asp:DropDownList ID="YearDropDown" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Year_SelectedIndexChanged" ></asp:DropDownList>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px" CellPadding="4" CellSpacing="2" ForeColor="Black">
                <Columns>
                    <asp:BoundField DataField="Month" HeaderText="Month" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Max" HeaderText="Highest Temperature of the month" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="MaxDay" HeaderText="Day of the highest temperature" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Min" HeaderText="Lowest Temperature of the month" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="MinDay" HeaderText="Day of the lowest temperature" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="AverageTempOfMonth" HeaderText="Average Temperature of the month" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>
                    </asp:BoundField>
                     <asp:ImageField DataImageUrlField="AverageSkyOfMonth" HeaderText="Average Sky">
                </asp:ImageField>
                </Columns>
                <FooterStyle BackColor="#CCCCCC" />
                <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#CCCCCC" ForeColor="Black" HorizontalAlign="Left" />
                <RowStyle BackColor="White" />
                <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                <SortedAscendingHeaderStyle BackColor="#808080" />
                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                <SortedDescendingHeaderStyle BackColor="#383838" />
            </asp:GridView>
            <br />
            <br />
            <br />
            <asp:chart id="ChartTemp" runat="server" 
               ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)" BorderWidth="2px" 
               BackGradientStyle="TopBottom" BackSecondaryColor="White" Palette="None" 
               BorderlineDashStyle="Solid" BorderColor="#38505D" Height="364px" Width="1177px" 
               EnableViewState="True">
               <legends>
                  <asp:legend Enabled="True" IsTextAutoFit="False" Name="Default" BackColor="Transparent" Font="Helvetica Neue, 8.25pt, style=Bold"></asp:legend>
               </legends>
               <borderskin skinstyle="None"></borderskin>
               <series>
                  <asp:series Name="Series1"  BorderColor="#38505d" Color="Black" 
                     ChartType="Renko" YValuesPerPoint="4" ></asp:series>
               </series>
               <chartareas>
                  <asp:chartarea  Name="ChartArea1" BorderColor="#38505d" BorderDashStyle="Solid" BackSecondaryColor="White" BackColor="white" ShadowColor="Transparent" BackGradientStyle="TopBottom">
                     <area3dstyle Rotation="10" perspective="10" Inclination="15" IsRightAngleAxes="False" wallwidth="0" IsClustered="False"></area3dstyle>
                     <axisy linecolor="#38505d" IsLabelAutoFit="False">
                        <labelstyle font="Helvetica Neue, 8.25pt, style=Bold" ForeColor="#38505d" />
                        <majorgrid linecolor="#38505d" />
                     </axisy>
                     <axisx linecolor="#38505d" IsLabelAutoFit="False">
                        <labelstyle font="Helvetica Neue, 8.25pt, style=Bold" ForeColor="#38505d" />
                        <majorgrid linecolor="#38505d" />
                     </axisx>
                  </asp:chartarea>
               </chartareas>
            </asp:chart>
        </div>
  
    </form>
</body>
</html>
