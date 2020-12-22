
// vue组件
import Vue from 'vue'
import App from './App'
// vue路由
import router from './router'
import $ from 'jquery'

// 接口访问(同jq的 ajax)
import axios from 'axios'
import qs from 'qs'
Vue.prototype.$axios = axios
Vue.prototype.qs = qs

// Bootstrap_Vue
// eslint-disable-next-line import/first
import BootstrapVue from 'bootstrap-vue'
// eslint-disable-next-line import/first
import 'bootstrap/dist/css/bootstrap.css'
// eslint-disable-next-line import/first
import 'bootstrap-vue/dist/bootstrap-vue.css'
Vue.use(BootstrapVue)

// layer_Vue
// eslint-disable-next-line import/first
import layer from 'vue-layer'
Vue.prototype.$layer = layer(Vue, {
  msgtime: 3
})

// api 接口地址
Vue.config.productionTip = false
Vue.prototype.baseURL = process.env.API_ROOT
Vue.prototype.baseURL2 = process.env.API_ROOT2

// iview  UI库导入
// eslint-disable-next-line import/first
import iView from 'iview'
// eslint-disable-next-line import/first
import 'iview/dist/styles/iview.css'

import ElementUI from 'element-ui'
import 'element-ui/lib/theme-chalk/index.css'

Vue.use(iView)
Vue.use(ElementUI)
// vue路由跳转前
router.beforeEach((to, from, next) => {
  iView.LoadingBar.start()// iview 加载进度条开始
  next()
})

// vue路由跳转后
router.afterEach(route => {
  iView.LoadingBar.finish()// iview 加载进度条结束
})

// eslint-disable-next-line no-new
new Vue({
  el: '#app',
  router,
  components: {
    App
  },
  template: '<App/>'
})
