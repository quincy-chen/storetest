<template>
  <div class="layout">
    <Layout>
      <!-- 头部Start -->
      <Header>
        <Menu mode="horizontal" theme="light" active-name="1">
          <div class="layout-logo">
            <Icon type="logo-windows" size="40"/>
          </div>
          <div class="layout-nav">
            <MenuItem name="1">
              <Icon type="md-aperture"/>
              <Dropdown @on-click="itemClick" trigger="click">
                <a href="javascript:void(0)">工具
                  <Icon type="ios-arrow-down"></Icon>
                </a>
                <DropdownMenu slot="list">
                  <DropdownItem name="/Home">返回登录</DropdownItem>
                  <DropdownItem name="/SearchStore">查询药房</DropdownItem>
                  <DropdownItem name="/TopStores">最频繁查询药房</DropdownItem>
                </DropdownMenu>
              </Dropdown>
            </MenuItem><div style="float:right; padding-right:10px;">{{UserName}}</div>
          </div>
        </Menu>
      </Header>
      <!-- 头部End -->
      <Layout>
        <Layout :style="{padding: '0 '}">
          <!-- 内容start -->
          <Content :style="{padding: '20px', minHeight: '800px', background: '#fff'}">
            <!-- 加载子组件start -->
            <router-view/>

            <!-- 加载子组件end -->
            <BackTop :height="100" :bottom="200">
              <div class="moveToTop">返回顶端</div>
            </BackTop>
          </Content>
          <!-- 内容end -->
        </Layout>
      </Layout>
    </Layout>
  </div>
</template>

<script>
export default {
  data() {
    return {
      UserName: ''
    };
  },
  methods: {
    //导航单击跳转
    itemClick: function(itemName) {
      this.$router.push({
        path: itemName
      }); //跳转页面
    }
  },
  created () {
    if (sessionStorage.getItem('UserID') != null) {
      this.UserName = sessionStorage.getItem('UserName') + ',欢迎访问！'
    }
  },
  computed: {

  }
};
</script>

<style scoped>
.layout {
  border: 1px solid #d7dde4;
  background: #f5f7f9;
  position: relative;
  border-radius: 4px;
  overflow: hidden;
}

.layout-logo {
  width: 100px;
  height: 30px;
  /* background: #5b6270; */
  border-radius: 3px;
  float: left;
  position: relative;
  top: 2px;
  left: 20px;
}

.layout-con {
  height: 100%;
  width: 100%;
}

.menu-item span {
  display: inline-block;
  overflow: hidden;
  width: 69px;
  text-overflow: ellipsis;
  white-space: nowrap;
  vertical-align: bottom;
  transition: width 0.2s ease 0.2s;
}

.menu-item i {
  transform: translateX(0px);
  transition: font-size 0.2s ease, transform 0.2s ease;
  vertical-align: middle;
  font-size: 16px;
}

.collapsed-menu {
  display: none;
}

.moveToTop {
  padding: 10px;
  background: rgba(0, 153, 229, 0.7);
  color: #fff;
  text-align: center;
  border-radius: 2px;
}
</style>
