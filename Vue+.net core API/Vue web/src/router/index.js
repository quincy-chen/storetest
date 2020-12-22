import Vue from 'vue'
import Router from 'vue-router'

// 模板页
import home from '@/components/home.vue'

// 模板页
import Layout from '@/components/layout/Layout.vue'

import TopStores from '@/components/TopStores.vue'

import SearchStore from '@/components/SearchStore.vue'

Vue.use(Router)

export default new Router({
  mode: 'history',
  routes: [
    {
      path: '/home',
      name: 'home',
      component: home
    },
    {
      path: '',
      name: 'layout',
      component: Layout,
      children: [
        {
          path: '/SearchStore',
          name: 'SearchStore',
          meta: {
            title: '查询药房'
          },
          component: SearchStore
        },
        {
          path: '/TopStores',
          name: 'TopStores',
          meta: {
            title: '搜索最频繁的药房'
          },
          component: TopStores
        }

      ]
    }
  ]
})
