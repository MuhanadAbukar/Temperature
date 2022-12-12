<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Day.aspx.cs" Inherits="Temperature.Default" %>

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
            <h1>Temperatures of today</h1>
            
            <asp:Button ID="Daily" runat="server" Text="Data from any day" OnClick="Month_Click" />
            <asp:Button ID="Month" runat="server" Text="Data from the last 30 days" OnClick="Month_Click" />
            <asp:Button ID="Monthly" runat="server" Text="Data from any month" OnClick="Month_Click" />
            <asp:Button ID="Year" runat="server" Text="Yearly Data" OnClick="Month_Click" />
            <br />
            <h1 runat="server" id="h1">Current Temperature: </h1>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px" CellPadding="4" CellSpacing="2" ForeColor="Black">
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
                    <asp:BoundField DataField="Hour" HeaderText="Hour" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Temperature" HeaderText="Temperature" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>
                    </asp:BoundField>
                    
                    <asp:BoundField DataField="Precipitation" HeaderText="Precipitation" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Humidity" HeaderText="Humidity" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="WindSpeed" HeaderText="Wind Speed" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="WindSpeedGust" HeaderText="Wind Speed (Gust)" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>
                        </asp:BoundField>
                    <asp:BoundField DataField="WindDirection" HeaderText="Wind Direction" ItemStyle-Width="150" >
<ItemStyle Width="150px"></ItemStyle>                        
                    </asp:BoundField>
                    <asp:ImageField DataImageUrlField="image" HeaderText="Sky">
                </asp:ImageField>
                </Columns>
            </asp:GridView>
        </div>
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

    </form>
    
</body>
</html>
