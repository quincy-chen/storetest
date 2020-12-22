using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web.Script.Serialization;

namespace StoreTest.WebAPI
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“ServiceAPI”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 ServiceAPI.svc 或 ServiceAPI.svc.cs，然后开始调试。
    public class ServiceAPI : IServiceAPI
    {
        public void DoWork()
        {
        }

        public string SaveNearestStore(List<Store> store)
        {
            string result = "";

            if (store.Count > 0)
            {
                var storeOrderBy = store.OrderBy(x => x.distance);
                result = new JavaScriptSerializer().Serialize(storeOrderBy);

                try
                {
                    // 存最近的门店到数据库
                    SQLiteHelper.SaveSearchResule(storeOrderBy.ToList()[0]);
                }
                catch
                {

                }
            }

            return result;
        }
    }
}
