using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCore.Helper
{
    public class LoginUser
    {
        private string _UserLoginID = "";
        private string _UserName = "";
        public string UserLoginID
        {
            get { return _UserLoginID; }
            set { _UserLoginID = value; }
        }

        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }
    }
        public class TopStore
    {
        private string _UserLoginID = "";
        private string _UserName = "";
        private string _NearestStoreTitle = "";
        private string _NearestStoreAddress = "";
        private string _StoreSreachedCount = "";

        public string UserLoginID
        {
            get { return _UserLoginID; }
            set { _UserLoginID = value; }
        }

        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }
        public string NearestStoreTitle
        {
            get { return _NearestStoreTitle; }
            set { _NearestStoreTitle = value; }
        }
        public string NearestStoreAddress
        {
            get { return _NearestStoreAddress; }
            set { _NearestStoreAddress = value; }
        }
        public string StoreSreachedCount
        {
            get { return _StoreSreachedCount; }
            set { _StoreSreachedCount = value; }
        }
    }
}
