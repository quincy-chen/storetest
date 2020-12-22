namespace ApiCore.Helper
{
    public class Store
    {
        private string _user = "";
        private string _address = "";
        private decimal _lat = System.Decimal.MinValue;
        private decimal _lng = System.Decimal.MinValue;
        private string _title = "";
        private decimal _distance = System.Decimal.MinValue;
        
        public string user
        {
            get { return _user; }
            set { _user = value; }
        }
        
        public string address
        {
            get { return _address; }
            set { _address = value; }
        }
        
        public decimal lat
        {
            get { return _lat; }
            set { _lat = value; }
        }
        
        public decimal lng
        {
            get { return _lng; }
            set { _lng = value; }
        }
       
        public string title
        {
            get { return _title; }
            set { _title = value; }
        }
        
        public decimal distance
        {
            get { return _distance; }
            set { _distance = value; }
        }
    }
}