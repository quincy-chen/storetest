using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Web;

namespace StoreTest.WebAPI
{
    [Serializable]
    [DataContract(Namespace = "http://www.dennydotnet.com/", Name = "Store")]
    [ServiceKnownType(typeof(Store))]
    public class Store
    {
        private string _user = "";
        private string _address = "";
        private decimal _lat = System.Decimal.MinValue;
        private decimal _lng = System.Decimal.MinValue;
        private string _title = "";
        private decimal _distance = System.Decimal.MinValue;

        [DataMember(IsRequired = true, Name = "user")]
        public string user
        {
            get { return _user; }
            set { _user = value; }
        }
        [DataMember(IsRequired = true, Name = "address")]
        public string address
        {
            get { return _address; }
            set { _address = value; }
        }
        [DataMember(IsRequired = true, Name = "lat")]
        public decimal lat
        {
            get { return _lat; }
            set { _lat = value; }
        }
        [DataMember(IsRequired = true, Name = "lng")]
        public decimal lng
        {
            get { return _lng; }
            set { _lng = value; }
        }
        [DataMember(IsRequired = true, Name = "title")]
        public string title
        {
            get { return _title; }
            set { _title = value; }
        }
        [DataMember(IsRequired = true, Name = "distance")]
        public decimal distance
        {
            get { return _distance; }
            set { _distance = value; }
        }
    }
}