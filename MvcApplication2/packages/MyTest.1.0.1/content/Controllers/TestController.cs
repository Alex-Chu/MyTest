using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using System.Drawing;

namespace MvcApplication1.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/

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

        public ActionResult TestKendoUI()
        {
            //return View();

            //---------------------------------------------------------------------------------
            string sPath = "";

            try
            {
                sPath = "/" + ControllerContext.RouteData.Values["controller"] + "/" + ControllerContext.RouteData.Values["action"];
            }
            catch (Exception)
            {
                
                
            }
            //---------------------------------------------------------------------------------
            /*
            if (Request["pageSize"] == null && Request["take"] == null && Request["skip"] == null)
            {
                return View();
            }
            */
            return View();
            //---------------------------------------------------------------------------------
        } // // actTestKendoUI-Get

        [HttpPost]
        public ActionResult TestKendoUI(string page, string pageSize, string take, string skip)
        {
            //---------------------------------------------------------------------------------
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

            string _keywd = Request["keywd"];
            string _sortField = Request["sort[0][field]"];
            string _sortDir = Request["sort[0][dir]"];
            //指定關鍵字時，使用Contains()對UserName進行比對
            var res = _SimuDataStore.Where(o =>
                string.IsNullOrEmpty(_keywd) || o.UserName.Contains(_keywd));
            if (!string.IsNullOrEmpty(_sortField))
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
                if (_sortDir == "asc")
                    res = res.OrderBy(o => GetColString(o, _sortField));
                else
                    res = res.OrderByDescending(o => GetColString(o, _sortField));
            }

            int _page, _pageSize = 10, _take = 10, _skip = 0;
            _page = int.TryParse(page, out _page) ? _page : 1;
            _pageSize = int.TryParse(pageSize, out _pageSize) ? _pageSize : 10;
            _take = int.TryParse(take, out _take) ? _take : 10;
            _skip = int.TryParse(skip, out _skip) ? _skip : 0;
            //var paged = res.Skip(skip).Take(take);
            var paged = res.Skip(_skip).Take(Math.Min(_pageSize, _take));
            return Json(new ResultData()
            {
                Data = paged.ToList(),
                TotalCount = res.Count()
            });
            //---------------------------------------------------------------------------------
        } // actTestKendoUI-Post
    } // class TestController
} // namespace MvcApplication1.Controllers
