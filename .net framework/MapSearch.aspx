<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MapSearch.aspx.cs" Inherits="StoreTest.MapSearch" MasterPageFile="~/Site.Master" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="cphMain" runat="server"> 
    <script src="//api.map.baidu.com/api?v=3.0&amp;ak=your_baidu_api_key" type="text/javascript"></script>
    <script src="http://api.map.baidu.com/getscript?v=3.0&amp;ak=your_baidu_api_key&amp;services=&amp;t=20201208133530" type="text/javascript"></script>
    <style>
        .BMap_pop {
            display: none !important;
        }
        .BMap_shadow{
            display: none !important;
        }
    </style>
    <asp:HiddenField ID="hdUser" ClientIDMode="Static" runat="server" />
    <div class="ivu-layout-content" style="background: rgb(255, 255, 255); padding: 20px; min-height: 800px;" data-v-063de322="">
        <div class="app-container" style="padding-bottom:20px;">
            <div class="ivu-alert ivu-alert-info ivu-alert-with-icon ivu-alert-with-desc" data-v-f71ce620="">
                <span class="ivu-alert-icon" style="padding-top:10px;"><img src="Content/warning.JPG" /></span> 
                <span class="ivu-alert-message">
                    <span style="color: green;" data-v-f71ce620="">友情提示:</span>
                </span> 
                <span class="ivu-alert-desc">
                    <span style="color: green; font-size: 15px;" data-v-f71ce620="">请输入地址，然后从下列列表中选中项，以便获取地址的准确坐标。<br /> 选择列表项后会列出以选中位置为中心，半径5公里内的所有药房，按距离由近到远排序，并将在数据库中记录第一项即最近的药房。</span>

                </span> 
            </div> 
            <table style="width: 100%; margin-top: 10px;" data-v-f71ce620="">
                <tr id="searchInput" style="width: 100%;" data-v-f71ce620="">
                    <td style="width: 100%;" data-v-f71ce620="">
                                          
                       <asp:TextBox id="txtNewLocation" runat="server" placeholder="输入地址" ClientIDMode="Static" class="form-control"></asp:TextBox>
                        <asp:HiddenField ID="hdNewLocationX" ClientIDMode="Static" runat="server" />
                        <asp:HiddenField ID="hdNewLocationY" ClientIDMode="Static" runat="server" />
                    </td>
                </tr>
            </table> 
        </div>
        <div id="searchResultPanelTo" style="border:1px solid #C0C0C0;width:150px;height:auto; display:none;"></div>  
        <div style="width:100% !important;">
            <div  class="col-md-5" style="min-width:400px;padding-left:2px; float:right;">
                <div id="2-map" style="width:100%;height:600px;padding: 0;"></div>
            </div>
            <div id="divResult" class="col-md-7" style="min-width:300px;float:left;padding-right:2px;height:600px;overflow-y:auto;">
            </div>
            <div class="clearfix"></div>
        </div>
        
        <script type="text/javascript">
                // 百度地图API功能
                function G(id) {
                    return document.getElementById(id);
                }
                var circle;
                var mapTo = new BMap.Map("2-map", { enableMapClick: false });
                mapTo.centerAndZoom("上海", 12);                   // 初始化地图,设置城市和地图级别。
                mapTo.enableScrollWheelZoom(true);
                //mapTo.setDefaultCursor("pointer");
                //mapTo.disableDoubleClickZoom() //禁止缩放
                //mapTo.disableScrollWheelZoom(); //开启鼠标滚轮缩放
                //mapTo.disableDragging();     //禁止拖拽
                
                var geoc = new BMap.Geocoder();
                mapTo.addEventListener("click", function (e) {
                    var pt = e.point;

                    $("#hdNewLocationX").val(pt.lat);
                    $("#hdNewLocationY").val(pt.lng);

                    geoc.getLocation(pt, function (rs) {
                        var addComp = rs.addressComponents;
                        $("#txtNewLocation").val(addComp.province + ", " + addComp.city + ", " + addComp.district + ", " + addComp.street + ", " + addComp.streetNumber);
                    });
                });

                var acTo = new BMap.Autocomplete(    //建立一个自动完成的对象
                    {
                        "input": "txtNewLocation"
                        , "location": mapTo
                    });

                acTo.addEventListener("onhighlight", function (e) {  //鼠标放在下拉列表上的事件
                    var str = "";
                    var _value = e.fromitem.value;
                    var value = "";
                    if (e.fromitem.index > -1) {
                        value = _value.province + _value.city + _value.district + _value.street + _value.business;
                    }
                    str = "FromItem<br />index = " + e.fromitem.index + "<br />value = " + value;

                    value = "";
                    if (e.toitem.index > -1) {
                        _value = e.toitem.value;
                        value = _value.province + _value.city + _value.district + _value.street + _value.business;
                    }
                    str += "<br />ToItem<br />index = " + e.toitem.index + "<br />value = " + value;
                    G("searchResultPanelTo").innerHTML = str;
                });

                var myValueTo;
                acTo.addEventListener("onconfirm", function (e) {    //鼠标点击下拉列表后的事件
                    var _value = e.item.value;
                    myValueTo = _value.province + _value.city + _value.district + _value.street + _value.business;
                    G("searchResultPanelTo").innerHTML = "onconfirm<br />index = " + e.item.index + "<br />myValue = " + myValueTo;

                    setPlaceTo();
                });
                var pp;
                function setPlaceTo() {
                    
                    mapTo.clearOverlays();    //清除地图上所有覆盖物
                    function myFunTo() {
                        pp = localTo.getResults().getPoi(0).point;    //获取第一个智能搜索的结果

                        //$("#hdNewLocationX").val(pp.lat);
                        //$("#hdNewLocationY").val(pp.lng);
                        //mapTo.centerAndZoom(pp, 18);
                        mapTo.addOverlay(new BMap.Marker(pp));    //添加标注                        

                        var circle = new BMap.Circle(pp, 5000, { fillColor: "blue", strokeWeight: 1, fillOpacity: 0.3, strokeOpacity: 0.3 });

                        mapTo.addOverlay(circle);

                        //circle.disableMassClear();
                        local1.searchNearby('药店', pp, 4500);                        
                    }
                    var localTo = new BMap.LocalSearch(mapTo, { //智能搜索
                        onSearchComplete: myFunTo
                    });

                    localTo.search(myValueTo);  
                }

                var changedValuesArr = new Array();
                function changedValues(user, address, lat, lng, title, distance) {
                    this.user = user;
                    this.address = address;
                    this.lat = lat;
                    this.lng = lng;
                    this.title = title;
                    this.distance = distance;
                }
                function GetInfo(jsondata) {
                    $.ajax({
                        type: "POST",
                        url: "http://localhost:9090/WebAPI/ServiceAPI.svc/SaveNearestStore",
                        data: JSON.stringify(changedValuesArr),
                        contentType: "application/json",
                        dataType: "json",
                        processData: true,
                        success: function (data, status, jqXHR) {
                            //alert("success…" + data);
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

                            var json = eval(data)
                            for (var i = 0; i < json.length; i++) {                             
                                // 通过 forEach 循环遍历对象数组,为表格添加行

                                var tr = document.createElement("tr");
                                // 通过 for in 循环遍历对象,得到对象的属性,给每行添加内容

                                var td = document.createElement("td");
                                td.innerText = json[i].title;
                                tr.appendChild(td);
                                var td = document.createElement("td");
                                td.innerText = json[i].address;
                                tr.appendChild(td);
                                var td = document.createElement("td");
                                td.innerText = json[i].distance;
                                tr.appendChild(td);
                                var td = document.createElement("td");
                                if (i == 0)
                                    td.innerText = "最近门店,已记录入库";
                                else
                                    td.innerText = "";

                                tr.appendChild(td);
                                table.appendChild(tr);
                            }

                            //设置表格的对齐属性,和边框属性
                            table.setAttribute("text-align", "right");
                            table.setAttribute("border", "1");

                            $("#divResult").html(table);

                            //if ($(".BMap_pop").length > 0)
                            //    $(".BMap_pop")[0].style.display = 'none !important';
                        },
                        error: function (xhr) {
                            //alert(xhr.responseText);
                        }
                    });
                }

                var ResultArray = [];

                var local1 = new BMap.LocalSearch(
                    mapTo,
                    {
                        renderOptions: {
                            map: mapTo,
                            // panel : "content"
                        }, onMarkersSet: function (array) {
                            //console.log(array);
                        },
                        onInfoHtmlSet: function (LocalResultPoi) {
                            //console.log(LocalResultPoi);
                        },
                        onResultsHtmlSet: function (element) {
                            //console.log(element);
                        },
                        onSearchComplete: function (results) {

                            // 需要获取当前搜索总共有多少条结果
                            var totalPages = results.getNumPages();
                            var currPage = results.getPageIndex();// 获取当前是第几页数据

                            for (var i = 0; i < results.getCurrentNumPois(); i++) {
                                var poi = results.getPoi(i);
                                //var marker = new BMap.Marker(poi.point);
                                //mapTo.addOverlay(marker)

                                var user = $("#hdUser").val();
                                var address = poi.address;
                                var lat = poi.point.lat;
                                var lng = poi.point.lng;
                                var title = poi.title;
                                //var pointA = new BMap.Point(121.436043, 29.294384);
                                //var pointB = new BMap.Point(116.450111, 39.927669); 
                                var distance = mapTo.getDistance(pp, poi.point).toFixed(2);
                                var ch = new changedValues(user, address, lat, lng, title, distance);
                                changedValuesArr[changedValuesArr.length] = ch;
                            }
                            
                            var json = JSON.stringify(changedValuesArr);

                            GetInfo(json);

                            
                        },

                        pageCapacity: 50
                    });
            </script>
    </div>
</asp:Content>