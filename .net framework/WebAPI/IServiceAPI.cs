using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace StoreTest.WebAPI
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IServiceAPI”。
    [ServiceContract]
    [ServiceKnownType(typeof(Store))]
    public interface IServiceAPI
    {
        [OperationContract]
        void DoWork();

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "SaveNearestStore")]
        //[WebGet(RequestFormat = WebMessageFormat.Json)]
        string SaveNearestStore(List<Store> store);
    }
}
