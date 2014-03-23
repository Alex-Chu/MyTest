using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Drawing;
using System.Reflection;

namespace MvcApplication1.Controllers
{
    public class TestController2Controller : ApiController
    {
        //模擬資料物件
        public class SimMemberInfo
        {
            public string UserNo; //會員編號
            public string UserName; //會員名稱
            public DateTime RegDate; //註冊日期
            public int Points; //累積點數
        }
        static List<SimMemberInfo> _SimuDataStore = null;
        //結果物件
        public class ResultData
        {
            public object Data;
            public int TotalCount;
        }

        // GET api/testcontroller2
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/testcontroller2/5
        public string Get(int id)
        {
            return "value";
        }

        public class MyValue
        {
            //public string keywd { get; set; }
            //public string sort { get; set; }
            public string page { get; set; }
            public string pageSize { get; set; }
            public string skip { get; set; }
            public string take { get; set; }
        }

        // POST api/testcontroller2
        public void Post([FromBody]MyValue Value)
        {
            //---------------------------------------------------------------------------------
            string sTemp = ""; 
            //---------------------------------------------------------------------------------
            //-------------------------------------------------
            // * 啟始資料來源
            //-------------------------------------------------
            if (_SimuDataStore == null)
            {
                Random rnd = new Random();
                //借用具名顏色名稱來產生隨機資料
                string[] colorNames = typeof(Color)
                    .GetProperties(BindingFlags.Static | BindingFlags.Public)
                    .Select(o => o.Name).ToArray();
                _SimuDataStore =
                    colorNames
                    .Select(cn => new SimMemberInfo()
                    {
                        UserNo = string.Format("C{0:00000}", rnd.Next(99999)),
                        UserName = cn,
                        RegDate = DateTime.Today.AddDays(-rnd.Next(1000)),
                        Points = rnd.Next(9999)
                    }).ToList();
            }
            //-------------------------------------------------

            //-------------------------------------------------
            // * 篩選與排序
            //-------------------------------------------------
            string keywd = System.Web.HttpContext.Current.Request["keywd"] ?? "";
            string sortField = System.Web.HttpContext.Current.Request["sort[0][field]"] ?? "";
            string sortDir = System.Web.HttpContext.Current.Request["sort[0][dir]"] ?? "";

            //指定關鍵字時，使用Contains()對UserName進行比對
            var res = _SimuDataStore.Where(o =>
                string.IsNullOrEmpty(keywd) || o.UserName.Contains(keywd));
            if (!string.IsNullOrEmpty(sortField))
            {
                //宣告一個函數可傳回SimMemberInfo之指定屬性值用於依動態欄位排序
                Func<SimMemberInfo, string, string> GetColString =
                    (o, c) =>
                    {
                        switch (c)
                        {
                            case "UserNo": return o.UserNo;
                            case "UserName": return o.UserName;
                            case "RegDate": return o.RegDate.ToString("yyyyMMdd");
                            case "Points": return o.Points.ToString();
                            default: throw new ArgumentException();
                        }
                    };
                if (sortDir == "asc")
                    res = res.OrderBy(o => GetColString(o, sortField));
                else
                    res = res.OrderByDescending(o => GetColString(o, sortField));
            }
            //-------------------------------------------------

            //-------------------------------------------------
            // * 分頁
            //-------------------------------------------------
            int page=1,pageSize = 10, take = 10, skip = 0;
            //------------------------------------------
            page = int.TryParse(Value.page, out page) ? page : 1;

            pageSize = int.TryParse(Value.pageSize, out pageSize) ? pageSize : 10;
 
            //skip = int.TryParse(System.Web.HttpContext.Current.Request["skip"], out skip) ? skip : 0;
            skip = int.TryParse(Value.skip, out skip) ? skip : 0;

            //take = int.TryParse(System.Web.HttpContext.Current.Request["take"], out take) ? take : 10;
            take = int.TryParse(Value.take, out take) ? take : 10;
            //------------------------------------------
            var paged = res.Skip(skip).Take(take);
            //-------------------------------------------------

            //-------------------------------------------------
            // * 序列化
            //-------------------------------------------------
            JavaScriptSerializer jss = new JavaScriptSerializer();
            sTemp = jss.Serialize(new ResultData()
            {
                Data = paged.ToList(),
                TotalCount = res.Count()
            });
            //-------------------------------------------------
            //---------------------------------------------------------------------------------
            // Response  Jason
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.ContentType = "application/json";
            System.Web.HttpContext.Current.Response.Write(sTemp);
            System.Web.HttpContext.Current.Response.Flush();
            System.Web.HttpContext.Current.Response.End();
            //---------------------------------------------------------------------------------
        }

        // PUT api/testcontroller2/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/testcontroller2/5
        public void Delete(int id)
        {
        }
    }
}
