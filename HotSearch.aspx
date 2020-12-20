<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HotSearch.aspx.cs" Inherits="StoreTest.HotSearch" MasterPageFile="~/Site.Master" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="cphMain" runat="server"> 
<div class="ivu-layout-content" style="background: rgb(255, 255, 255); padding: 20px; min-height: 800px;" data-v-063de322="">
    <div class="app-container" data-v-063de322="" data-v-f71ce620="">
        <div class="ivu-alert ivu-alert-info ivu-alert-with-icon ivu-alert-with-desc" data-v-f71ce620="">
            <span class="ivu-alert-icon" style="padding-top:10px;"><img src="Content/warning.JPG" /></span> 
            <span class="ivu-alert-message">
                <span style="color: green;" data-v-f71ce620="">友情提示:</span>
            </span> 
            <span class="ivu-alert-desc">
                <span style="color: green; font-size: 15px;" data-v-f71ce620="">以下列表显示每个用户查询最频繁的药房。<br />根据每次查询后记录的最近药房进行统计，列出每个用户查询最频繁的药房。</span>

            </span> 
        </div> 

        <table style="width: 100%; margin-top: 10px;" border="1">
            <tr style="width: 100%;" data-v-f71ce620="">
                <th style="width: 15%;" data-v-f71ce620="">
                     用户
                </th>
                <th style="width: 25%;" data-v-f71ce620="">
                     门店
                </th>
                <th style="width: 45%;" data-v-f71ce620="">
                     门店地址
                </th>
                <th style="width: 15%;" data-v-f71ce620="">
                     次数
                </th>
            </tr>
            <asp:Repeater id="rpList" runat="server" ClientIDMode="Static">
                <ItemTemplate>
                    <tr>
                        <td><%#Eval("UserName")%></td>
                        <td><%#Eval("NearestStoreTitle")%></td>
                        <td><%#Eval("NearestStoreAddress")%></td>
                        <td><%#Eval("StoreSreachedCount")%></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:Panel class="table-row-group" runat="server" Visible='<%#bool.Parse((rpList.Items.Count==0).ToString())%>'>
                        <tr>
                            <td colspan="4">暂时无数据</td>
                        </tr>
                    </asp:Panel>
                </FooterTemplate> 
            </asp:Repeater>
        </table> 
    </div>
</div>
</asp:Content>