using System.Collections.Generic;
using ApiCore.Helper;

namespace ApiCore.Model.MyModel
{
    public class SearchQueryModel
    {
        public string stores { get; set; } = "";
    }
    public class JtDbQueryModel 
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 100;
    }
    public class LoginQueryModel
    {
        public string code { get; set; } = "";
    }
}