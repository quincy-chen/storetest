<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="StoreTest.Home" %>


<!DOCTYPE html>

<html>
<head runat="server">
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width,initial-scale=1.0">
  <title>Test</title>
  <style type="text/css">
    html,
    body {
      height: 100%;
      width: 100%
    }
  </style>
    <link href="<%=ResolveClientUrl("~/scripts/style.css") %>" rel='stylesheet' type='text/css' />
</head>
<body> <form id="form1" runat="server">
  <div id="app">
      <div id="login">
          <div class="m-login-bg">
              <div class="m-login">
                  <h3>登录</h3> 
                  <div class="m-login-warp">

                          <div class="layui-form-item">
                                <a href="https://github.com/login/oauth/authorize?client_id=your_github_clientid&redirect_uri=http://localhost:9090/home">
                                    <img src="Content/logo.jpg" style="width:100%;text-align:center" />
                                </a>
                          </div> 
                          <div class="layui-form-item">
                                <p style="text-align: center; font-size: 12px;">请点击上图使用GitHub账号授权登录</p>
                          </div>
                          <div class="layui-form-item">
                              <asp:Label ID="lblError" runat="server" ForeColor="Red" style="text-align: center; font-size: 12px;"></asp:Label>                                
                          </div>
                      <div class="toast" style="display: none;">          
                      </div>

                  </div> 
                  <p class="copyright"></p>
              </div>
          </div>
      </div>
  </div>
</form>
</body></html>