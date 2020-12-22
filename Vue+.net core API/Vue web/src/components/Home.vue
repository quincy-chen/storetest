<template>
  <div id="login">
    <div class="m-login-bg">
      <div class="m-login">
        <h3>Test</h3>
        <div class="m-login-warp">
          <form class="layui-form">
            <div class="layui-form-item">
              <input type="text" name="title" required lay-verify="required" placeholder="用户名" autocomplete="off"
                     class="layui-input">
            </div>
            <div class="layui-form-item">
              点击下面小图标使用github登录
            </div>

            <div class="layui-form-item">
              <div class="layui-inline">
                <img class="login-img-third" v-on:click="loginByGithub()" title="Github"
                     src="@/assets/images/login/github.jpg"/>
              </div>
            </div>
            <div class="layui-form-item">
              <p style="font-size: 12px; text-align: center; color:red;">{{loginStatus}}</p>
            </div>
          </form>
          <div class="toast" v-show="toastShow">
            {{toastText}}
          </div>
        </div>
        <p class="copyright"></p>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  data () {
    return {
      serviceUrl: this.baseURL,
      toastShow: false,
      toastText: '',
      loginParameter: {
        code: ''
      },
      loginStatus: ''
    }
  },
  created () {
    this.loginParameter.code = this.getUrlKey('code')

    if (this.loginParameter.code != null) {
      try {
        this.$axios({
          method: 'post',
          dataType: 'jsonp',
          url: this.serviceUrl + '/SQLPad/GetAuth',
          data: this.loginParameter
        }).then(response => {
          //alert(response)
          console.log(response.data)
          var dataRes = response.data.dataRes
          var dataList = JSON.parse(dataRes)
          console.log(dataList.UserLoginID)
          // eslint-disable-next-line eqeqeq
          if (dataList.UserLoginID != '') {
            sessionStorage.setItem('UserID', dataList.UserLoginID)
            sessionStorage.setItem('UserName', dataList.UserName)

            this.$router.push({path: '/SearchStore'})
          } else {
            this.loginStatus = '登录失败'
          }
        })
      } catch (error) {
        this.msg = error.message
        this.$Loading.error()
      }
    }
  },
  methods: {
    getUrlKey: function (name) {
      // eslint-disable-next-line no-sparse-arrays
      return decodeURIComponent((new RegExp('[?|&]' + name + '=' + '([^&;]+?)(&|#|;|$)').exec(location.href) || [, ''])[1].replace(/\+/g, '%20')) || null
    },
    loginByGithub: function () {
      window.location.href = 'https://github.com/login/oauth/authorize?client_id=your_client_id&redirect_uri=http://localhost:9090/home'
    },
    facelogin: function () {
      this.$router.push('/face')
    },
    toast (e) {
      let self = this
      self.toastText = e
      self.toastShow = true
      setTimeout(function () {
        self.toastShow = false
      }, 1500)
    }
  }
}
</script>

<style>
  @import "../assets/css/login.css";
  @import "../assets/css/tools.css";
  @import "../assets/layui/css/layui.css";
</style>
