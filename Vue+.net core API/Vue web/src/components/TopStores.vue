<template>
  <div @keyup.enter="queryData()">
    <!-- 提示块 -->
    <Alert show-icon closable>
      <span style="color:green">友情提示:</span>
      <template slot="desc">
        <span style="color:green;font-size:15px">以下列表显示每个用户查询最频繁的药房。<br />根据每次查询后记录的最近药房进行统计，列出每个用户查询最频繁的药房。</span>
      </template>
    </Alert>

    <Button type="success" @click="getData()">搜索</Button>
    <!-- 顶部分页 -->
    <Page
      align="left"
      :current="searchParameter.pageIndex"
      :total="dataCount"
      :page-size="searchParameter.pageSize"
      :page-size-opts="[20, 40, 60, 80,100,120,140,160,180,200,220,240,260,280,300]"
      @on-change="pageIndexChange"
      @on-page-size-change="pageSizeChange"
      show-sizer
      show-elevator
      show-total
    />
    <!-- 数据表格 -->
    <table
      class="table table-striped table-responsive table-condensed"
      style="margin-top:10px; width:100%;"
    >
      <!-- 文本框 -->
      <tr id="searchInput" style="width:100%;">
        <td style="width:15%;">
          账号ID
        </td>
        <td style="width:15%;">
          账号名
        </td>
        <td style="width:15%;">
          药店名
        </td>
        <td style="width:45%;">
          药店地址
        </td>
        <td style="width:10%;">
          搜索次数
        </td>
      </tr>
      <!-- 数据 -->
      <tr v-for="item in dataList" :key="item" style="width:100%">
        <td v-for="child in item" :key="child.序号">{{child}}</td>
      </tr>
    </table>
    <!-- 底部分页 -->
    <Page
      align="left"
      :current="searchParameter.pageIndex"
      :total="dataCount"
      :page-size="searchParameter.pageSize"
      :page-size-opts="[20, 40, 60, 80,100,120,140,160,180,200,220,240,260,280,300]"
      @on-change="pageIndexChange"
      @on-page-size-change="pageSizeChange"
      show-sizer
      show-elevator
      show-total
    />
  </div>
</template>

<script>

export default {
  name: 'TopStores',
  data() {
    return {
      // api地址
      serviceUrl: this.baseURL,
      //数据
      dataList: {},
      //搜索框参数
      searchParameter: {
        pageIndex: 1,
        pageSize: 40
      },
      //数据的总数量
      dataCount: 1
    };
  },
  methods: {
    //查询
    queryData: function() {
      this.searchParameter.pageIndex = 1;
      this.getData();
    },
    //请求数据
    getData: function() {
      // alert(this.baseURL);
      this.$Loading.start();
      try {
        this.$axios({
          method: "post",
          dataType: "jsonp",
          url: this.serviceUrl + "/SQLPad/GetHotStoreList",
          data: this.searchParameter
        }).then(response => {
          //数据
          var dataRes = response.data.dataRes;
          if (dataRes != undefined) {
            dataRes = dataRes.replace(/undefined/g, "");
            this.dataList = JSON.parse(dataRes);
          }

          this.dataCount = response.data.dataCount;
          this.$Loading.finish();
        });
      } catch (error) {
        this.msg = error.message;
        this.$Loading.error();
      }
    },
    //iview页码改变回调
    pageIndexChange: function(resPageIndex) {
      this.searchParameter.pageIndex = resPageIndex;
    },
    //iview 每页 数据量改变回调
    pageSizeChange: function(resPageSize) {
      this.searchParameter.pageSize = resPageSize;
    }    
  },
  //属性 监控
  watch: {
    "searchParameter.pageIndex": function(val, oldVal) {
      this.getData();
    },
    "searchParameter.pageSize": function(val, oldVal) {
      this.getData();
    }
  },
  created () {
    if (sessionStorage.getItem('UserID') == null) {
      this.$router.push({path: '/home'})
    }
  },
  //页面初始化加载
  mounted: function() {
  }
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->

<style scoped>
h1,
h2 {
  font-weight: normal;
}

ul {
  list-style-type: none;
  padding: 0;
}

li {
  display: inline-block;
  margin: 0 10px;
}

a {
  color: #42b983;
}

.red {
  color: red;
}

.title {
  background-color: #2d8cf0;
  color: white;
  font-size: 15px;
}
</style>
