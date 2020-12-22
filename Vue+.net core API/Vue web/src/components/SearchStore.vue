<template>
  <div class="app-container">
    <!-- 提示块 -->
    <Alert show-icon closable>
      <span style="color:green">友情提示:</span>
      <template slot="desc">
        <span style="color:green;font-size:15px">请输入地址，然后从下列列表中选中项，以便获取地址的准确坐标。<br /> 选择列表项后会列出以选中位置为中心，半径5公里内的所有药房，按距离由近到远排序，并将在数据库中记录第一项即最近的药房。</span>
      </template>
    </Alert>

    <Button type="success" >搜索</Button>

    <!-- 数据表格 -->
    <table style="margin-top:10px;width:100%">
      <!-- 文本框 -->
      <tr id="searchInput" style="width:100%">
        
        <td style="width:100%">
          
          <el-autocomplete
      v-model="mapLocation.address"
      :fetch-suggestions="querySearch"
      placeholder="请输入详细地址"
      style="width: 100%"
      :trigger-on-focus="false"
      @select="handleSelect"
    />
        </td>
        
      </tr>
    </table>
    <div style="width:100% !important;margin: 5px;">
      <div style="min-width:400px;padding-left:2px; float:right;" class="col-md-5">
        <baidu-map class="bm-view" :center="mapCenter" :zoom="mapZoom" :scroll-wheel-zoom="true" ak="baidu-ak" @ready="handlerBMap" />
      </div>
      <div id="divResult" class="col-md-7" style="min-width:300px;float:left;padding-right:2px;height:600px;overflow-y:auto;">
      
      </div>
    </div>
  </div>
</template>

<script>
import BaiduMap from 'vue-baidu-map/components/map/Map.vue'
export default {
  name: 'BaiduMapDemo',
  components: {
    BaiduMap
  },
  data() {
    return {
      serviceUrl: this.baseURL,
      mapZoom: 15,
      mapCenter: { lng: 121.453429, lat: 31.233845 },
      mapLocation: {
        address: '上海静安寺',
        coordinate: { lng: 121.453429, lat: 31.233845 }
      },
      searchPoint: { lng: 121.453429, lat: 31.233845 },
      searchParameter: {
        stores: ''
      },
      dataList: {}
    }
  },
  created () {
    if (sessionStorage.getItem('UserID') == null) {
      this.$router.push({path: '/home'})
    }
  },
  methods: {
    handlerBMap ({ BMap, map }) {
      this.BMap = BMap
      this.map = map
      if (this.mapLocation.coordinate && this.mapLocation.coordinate.lng) {
        this.mapCenter.lng = this.mapLocation.coordinate.lng
        this.mapCenter.lat = this.mapLocation.coordinate.lat
        this.mapZoom = 15
        map.addOverlay(new this.BMap.Marker(this.mapLocation.coordinate))
      } else {
        this.mapCenter.lng = 113.271429
        this.mapCenter.lat = 23.135336
        this.mapZoom = 10
      }
    },
    created () {
      if (sessionStorage.getItem('UserID') == null) {
        //this.$router.push({path: '/home'})
      }
    },
    querySearch (queryString, cb) {
      var that = this
      var myGeo = new this.BMap.Geocoder()
      myGeo.getPoint(queryString, function (point) {
        if (point) {
          that.searchPoint = point
          that.mapLocation.coordinate = point
          that.makerCenter(point)
        } else {
          that.mapLocation.coordinate = null
        }
      }, this.locationCity)
      var options = {
        onSearchComplete: function (results) {
          if (local.getStatus() === 0) {
            // 判断状态是否正确
            var s = []
            for (var i = 0; i < results.getCurrentNumPois(); i++) {
              var x = results.getPoi(i)
              var item = { value: x.address + x.title, point: x.point }
              s.push(item)
              cb(s)
            }
          } else {
            cb()
          }
        }
      }
      var local = new this.BMap.LocalSearch(this.map, options)
      local.search(queryString)
    },
    handleSelect (item) {
      var { point } = item
      this.mapLocation.coordinate = point
      this.makerCenter(point)

      this.searchPoint = point

      var circle = new this.BMap.Circle(point, 5000, { fillColor: 'blue', strokeWeight: 1, fillOpacity: 0.3, strokeOpacity: 0.3 })
      this.map.addOverlay(circle)
      var local = new this.BMap.LocalSearch(this.map, {
        renderOptions: {
          map: this.map,
          autoViewport: false
        }
      })

      //设置一个条件检索最多15条数据
      local.setPageCapacity(200);

      //搜索范围1500--搜索周边5公里附近的...
      local.searchNearby('药房', point, 5000)

      local.setSearchCompleteCallback(results => {
        var res_t = results
        //this指向
        //var that=this
        //接收参数
        var arr = []
        //请求携带json参数
        var petShop_context = []
        var base1 = this.serviceUrl
        if(res_t.Hr.length != null) {
          for(var i = 0; i < res_t.Hr.length; i++) {
              arr.push(res_t.Hr[i])
          }
        }   

        // 将检索遍历的数据取出后，循环他们将它们装起做请求携带参数
        for(var k = 0; k < arr.length; k++) {
          //this.makerPostion(arr[k].point)
          //makerPostion(arr[k].point)
          this.map.addOverlay(new this.BMap.Marker(arr[k].point))
          var distance = this.map.getDistance(arr[k].point, point).toFixed(2)
          //var userid = sessionStorage.getItem('UserID')
          //将检索UUID&经纬度信息封装成一个json数组，方便后台调用
          petShop_context.push({
            address: arr[k].address,
            lat: arr[k].point.lat,
            lng: arr[k].point.lng,
            title: arr[k].title,
            distance: distance,
            user: 'userid'
          })
        }

        this.searchParameter.stores = JSON.stringify(petShop_context)

        try {
          this.$axios({
            method: "post",
            dataType: "jsonp",
            url: base1 + "/SQLPad/SaveNearestStore",
            data: this.searchParameter
          }).then(response => {
            $("#divResult").html("")
                        
            var table = document.createElement("table");
            var tr = document.createElement("tr");

            var th = document.createElement("th");
            th.innerText = "门店";
            tr.appendChild(th);
            var th = document.createElement("th");
            th.innerText = "地址";
            tr.appendChild(th);
            var th = document.createElement("th");
            th.setAttribute("width", "80");
            th.innerText = "距离(米)";
            tr.appendChild(th);
            var th = document.createElement("th");
            th.innerText = "备注";
            tr.appendChild(th);
            table.appendChild(tr);

            //数据
            var dataRes = response.data.dataRes;
            if (dataRes != undefined && dataRes != '') {
              dataRes = dataRes.replace(/undefined/g, "");
              var test = JSON.parse(dataRes);
              var index = 0;
              test.forEach(element => {
                var tr = document.createElement("tr");
                // 通过 for in 循环遍历对象,得到对象的属性,给每行添加内容
                var td = document.createElement("td");
                td.innerText = element.title;
                tr.appendChild(td);
                var td = document.createElement("td");
                td.innerText = element.address;
                tr.appendChild(td);
                var td = document.createElement("td");
                td.innerText = element.distance;
                tr.appendChild(td);
                var td = document.createElement("td");
                if (index == 0)
                    td.innerText = "最近门店,已记录入库";
                else
                    td.innerText = "";

                tr.appendChild(td);
                table.appendChild(tr);

                index++
              });

              table.setAttribute("text-align", "right");
              table.setAttribute("border", "1");
              table.setAttribute("width", "100%");

              $("#divResult").html(table);
            }

            //this.dataCount = response.data.dataCount;
          });
        } catch (error) {
          this.msg = error.message;
          this.$Loading.error();
        }
      })
    },
    makerCenter (point) {
      if (this.map) {
        this.map.clearOverlays()
        this.map.addOverlay(new this.BMap.Marker(point))
        this.mapCenter.lng = point.lng
        this.mapCenter.lat = point.lat
        this.mapZoom = 15
      }
    }
  }
}
</script>

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
.bm-view {
  width: 100%;
  height: 500px;
}
</style>
