using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Reflection;
using System.Drawing;

namespace MvcApplication1.Controllers
{
    public class TestKendoUI_CRUDController : Controller
    {
        //===========================================================================================================
        // Common Class
        private static class _MyMsg
        {
            public const string QueryFail = " Query Fail";
            public const string CreateFail = "Create Fail";
            public const string UpdateFail = "Update Fail";
            public const string DeleteFail = "Delete Fail";

            public const string OtherFail = "Other Fail";
        }

        // 結果
        public class MyResult
        {
            // Kendo UI
            public object Data;
            public int TotalCount;
            public string errors = "";

            // 自訂
            public int ResultCode;
            public string ResultMsg;
        }

        // 結果資料
        static List<MyResultData> MySourceData = null;
        public class MyResultData
        {
            public string SerNo = "";
            public string RecNo = "";

            public string UserNo; //會員編號 (Pkey)
            public string UserName; //會員名稱
            public DateTime RegDate; //註冊日期
            public bool VIP;
            public int Points; //累積點數
        }
        //----------------------------------------------------------------------------------------------------------
        // Common Vars
        //string PageName = "";
        bool HasErr = false;
        string ErrMsg = "";
        MyResultData _MyResultData = default(MyResultData);  // 單筆
        IQueryable<MyResultData> _MyResultDatas = default(IQueryable<MyResultData>);         // 全部
        IQueryable<MyResultData> _MyResultDatas_CurPage = default(IQueryable<MyResultData>); // 目前頁
        MyResult _MyResult = default(MyResult);

        //JavaScriptSerializer jss = new JavaScriptSerializer();
        //===========================================================================================================
        //
        // GET: /TestKendoUI_CRUD/
        public ActionResult Index()
        {
            if (MySourceData == null)
            {
                int _TmpCnt = 0;
                Random rnd = new Random();
                //借用具名顏色名稱來產生隨機資料
                string[] colorNames = typeof(Color)
                    .GetProperties(BindingFlags.Static | BindingFlags.Public)
                    .Select(o => o.Name).ToArray();
                MySourceData =
                    colorNames
                    .Select(cn => new MyResultData()
                    {
                        UserNo = string.Format("C{0:00000}", rnd.Next(99999)),
                        UserName = cn,
                        RegDate = DateTime.Today.AddDays(-rnd.Next(1000)),
                        VIP = _TmpCnt % 2 == 0 ? true : false,
                        Points = rnd.Next(9999) + _TmpCnt++
                    }).ToList();
            }

            return View("~/Views/Test/TestKendoUI_CRUD.cshtml");
        }
        //----------------------------------------------------------------------------------------------------------
        // POST: /TestKendoUI_CRUD/Index
        [HttpPost]
        public ActionResult Index(FormCollection MyForm)
        {
            //----------------------------------------------------------------------------------------------------------
            // Main
            if (MySourceData == null)
            {
                int _TmpCnt = 0;
                Random rnd = new Random();
                //借用具名顏色名稱來產生隨機資料
                string[] colorNames = typeof(Color)
                    .GetProperties(BindingFlags.Static | BindingFlags.Public)
                    .Select(o => o.Name).ToArray();
                MySourceData =
                    colorNames
                    .Select(cn => new MyResultData()
                    {
                        UserNo = string.Format("C{0:00000}", rnd.Next(99999)),
                        UserName = cn,
                        RegDate = DateTime.Today.AddDays(-rnd.Next(1000)),
                        VIP = _TmpCnt % 2 == 0 ? true : false,
                        Points = rnd.Next(9999) + _TmpCnt++
                    }).ToList();
            }

            try
            {
                if (Request.QueryString["Act"] == "R")
                {
                    //---------------------------------------------------------------------------------------
                    string keywd = MyForm["keywd"];
                    string dteJoin_B = MyForm["dteJoin_B"];
                    string dteJoin_E = MyForm["dteJoin_E"];
                    string sortField = MyForm["sort[0][field]"];
                    string sortDir = MyForm["sort[0][dir]"];

                    // 篩選
                    _MyResultDatas = MySourceData.Where(o =>
                        string.IsNullOrEmpty(keywd) || o.UserName.Contains(keywd)).AsQueryable();

                    try
                    {
                        DateTime _dteJoin_B = default(DateTime), _dteJoin_E = default(DateTime);

                        DateTime.TryParse(dteJoin_B, out _dteJoin_B);
                        DateTime.TryParse(dteJoin_E, out _dteJoin_E);

                        if (_dteJoin_B != default(DateTime))
                        {
                            _MyResultDatas = _MyResultDatas.Where(o => o.RegDate >= _dteJoin_B);
                        }
                        if (_dteJoin_E != default(DateTime))
                        {
                            _MyResultDatas = _MyResultDatas.Where(o => o.RegDate <= _dteJoin_E);
                        }
                    }
                    catch (Exception)
                    {


                    }

                    try
                    {
                        string sFillter_field = MyForm["filter[filters][0][field]"].Trim();
                        string sFillter_operator = MyForm["filter[filters][0][operator]"];
                        string sFillter_value = MyForm["filter[filters][0][value]"].Trim();
                        if (sFillter_field == "UserName")
                        {
                            if (sFillter_operator == "eq")
                            {
                                _MyResultDatas = _MyResultDatas.Where(o => o.UserName == sFillter_value);
                            }
                            else if (sFillter_operator == "neq")
                            {
                                _MyResultDatas = _MyResultDatas.Where(o => o.UserName != sFillter_value);
                            }
                            else if (sFillter_operator == "contains")
                            {
                                _MyResultDatas = _MyResultDatas.Where(o => o.UserName.Contains(sFillter_value));
                            }
                            else if (sFillter_operator == "doesnotcontain")
                            {
                                _MyResultDatas = _MyResultDatas.Where(o => !o.UserName.Contains(sFillter_value));
                            }
                            else if (sFillter_operator == "startswith")
                            {
                                _MyResultDatas = _MyResultDatas.Where(o => o.UserName.StartsWith(sFillter_value));
                            }
                            else if (sFillter_operator == "endswith")
                            {
                                _MyResultDatas = _MyResultDatas.Where(o => o.UserName.IndexOf(sFillter_value) >= 0 && o.UserName.IndexOf(sFillter_value) == (o.UserName.Length - sFillter_value.Length));
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }

                    // 排序
                    if (!string.IsNullOrEmpty(sortField))
                    {
                        //宣告一個函數可傳回SimMemberInfo之指定屬性值用於依動態欄位排序
                        Func<MyResultData, string, string> GetColString =
                            (o, c) =>
                            {
                                switch (c)
                                {
                                    case "UserNo": return o.UserNo;
                                    case "UserName": return o.UserName;
                                    case "RegDate": return o.RegDate.ToString("yyyyMMdd");
                                    case "Points": return o.Points.ToString();
                                    case "VIP": return o.VIP.ToString();
                                    default: throw new ArgumentException();
                                }
                            };
                        if (sortDir == "asc")
                            _MyResultDatas = _MyResultDatas.OrderBy(o => GetColString(o, sortField));
                        else
                            _MyResultDatas = _MyResultDatas.OrderByDescending(o => GetColString(o, sortField));
                    }

                    // 分頁
                    int pageSize = 10, take = 10, skip = 0;
                    pageSize = int.TryParse(MyForm["pageSize"], out pageSize) ? pageSize : 10;
                    take = int.TryParse(MyForm["take"], out take) ? take : 10;
                    skip = int.TryParse(MyForm["skip"], out skip) ? skip : 0;
                    _MyResultDatas_CurPage = _MyResultDatas.Skip(skip).Take(Math.Min(pageSize, take));
                    //---------------------------------------------------------------------------------------
                }
                else if (Request.QueryString["Act"] == "C")
                {
                    _MyResultData = new MyResultData();

                    //throw new Exception("Test  Err");
                    //---------------------------------------------------------------------------------------
                    _MyResultData.UserNo = "Test" + (MySourceData.Count() + 1).ToString();

                    _MyResultData.UserName = (MyForm["UserName"] ?? "").Trim() == "測試" ? ("測試" + (MySourceData.Count() + 1).ToString()) : (MyForm["UserName"] ?? "");

                    // _MyResultData.RegDate = DateTime.Parse( MyForm["RegDate"].ToString()).ToUniversalTime();
                    _MyResultData.RegDate = DateTime.Parse(MyForm["RegDate"].ToString()); // 同 ToUniversalTime()

                    _MyResultData.Points = MyForm["Points"] != null ? int.Parse(MyForm["Points"].ToString()) : -99;

                    bool.TryParse(MyForm["VIP"].ToString(), out _MyResultData.VIP);
                    //---------------------------------------------------------------------------------------

                    MySourceData.Add(_MyResultData);
                }
                else if (Request.QueryString["Act"] == "U")
                {
                    _MyResultData = MySourceData.Where(x => x.UserNo == (MyForm["UserNo"] ?? "")).FirstOrDefault();

                    if (_MyResultData != null)
                    {
                        //throw new Exception("Test  Err");
                        //---------------------------------------------------------------------------------------
                        _MyResultData.SerNo = MyForm["SerNo"] ?? "";
                        _MyResultData.RecNo = MyForm["RecNo"] ?? "";

                        //_MyResultData.UserNo = MyForm["UserNo"] ?? ""; // PKEY

                        _MyResultData.UserName = MyForm["UserName"] ?? "";

                        _MyResultData.RegDate = DateTime.Parse(MyForm["RegDate"].ToString());

                        _MyResultData.Points = MyForm["Points"] != null ? int.Parse(MyForm["Points"].ToString()) : -99;

                        bool.TryParse(MyForm["VIP"].ToString(), out _MyResultData.VIP);
                        //---------------------------------------------------------------------------------------
                    }
                }
                else if (Request.QueryString["Act"] == "D")
                {
                    _MyResultData = new MyResultData();

                    //throw new Exception("Test  Err");
                    //---------------------------------------------------------------------------------------
                    _MyResultData.UserNo = MyForm["UserNo"] ?? "";

                    _MyResultData.UserName = MyForm["UserName"] ?? "";

                    _MyResultData.RegDate = DateTime.Parse(MyForm["RegDate"].ToString());

                    _MyResultData.Points = MyForm["Points"] != null ? int.Parse(MyForm["Points"].ToString()) : -99;

                    bool.TryParse(MyForm["VIP"].ToString(), out _MyResultData.VIP);
                    //---------------------------------------------------------------------------------------

                    var oDelete = MySourceData.Where(x => x.UserNo == _MyResultData.UserNo).FirstOrDefault();
                    if (oDelete != null)
                    {
                        MySourceData.Remove(oDelete);
                    }
                }
                else
                {
                    throw new Exception("Invalid QueryParm-Act");
                }

                HasErr = false;
                ErrMsg = "";
            }
            catch (Exception e)
            {
                HasErr = true;
                ErrMsg = e.Message.Trim();
            }
            //----------------------------------------------------------------------------------------------------------
            // Result
            if (Request.QueryString["Act"] == "R")
            {
                if (!HasErr)
                {
                    _MyResult = new MyResult()
                    {
                        Data = _MyResultDatas_CurPage.ToList(), // 目前頁
                        TotalCount = _MyResultDatas.Count(), // 全部

                        errors = "",

                        ResultCode = 0,
                        ResultMsg = "作業成功" + " (" + (Request.QueryString["Act"] ?? "") + ")"
                    };
                }
                else
                {
                    _MyResult = new MyResult()
                    {
                        Data = null,
                        TotalCount = 0,

                        errors = _MyMsg.QueryFail,

                        ResultCode = -1,
                        ResultMsg = "作業失敗" + " (" + (Request.QueryString["Act"] ?? "") + ") --- " + ErrMsg
                    };
                }
            }
            else if (Request.QueryString["Act"] == "C")
            {
                if (!HasErr)
                {
                    _MyResult = new MyResult()
                    {
                        Data = _MyResultData, // 目前筆
                        TotalCount = _MyResultDatas != null ? _MyResultDatas.Count() : 0, // 全部

                        errors = "",

                        ResultCode = 0,
                        ResultMsg = "作業成功" + " (" + (Request.QueryString["Act"] ?? "") + ")"
                    };
                }
                else
                {
                    _MyResult = new MyResult()
                    {
                        Data = null,
                        TotalCount = 0,

                        errors = _MyMsg.CreateFail,

                        ResultCode = -1,
                        ResultMsg = "作業失敗" + " (" + (Request.QueryString["Act"] ?? "") + ") --- " + ErrMsg
                    };
                }
            }
            else if (Request.QueryString["Act"] == "U")
            {
                if (!HasErr)
                {
                    _MyResult = new MyResult()
                    {
                        Data = _MyResultData, // 目前筆
                        TotalCount = _MyResultDatas != null ? _MyResultDatas.Count() : 0, // 全部

                        errors = "",

                        ResultCode = 0,
                        ResultMsg = "作業成功" + " (" + (Request.QueryString["Act"] ?? "") + ")"
                    };
                }
                else
                {
                    _MyResult = new MyResult()
                    {
                        Data = null,
                        TotalCount = 0,

                        errors = _MyMsg.UpdateFail,

                        ResultCode = -1,
                        ResultMsg = "作業失敗" + " (" + (Request.QueryString["Act"] ?? "") + ") --- " + ErrMsg
                    };
                }
            }
            else if (Request.QueryString["Act"] == "D")
            {
                if (!HasErr)
                {
                    _MyResult = new MyResult()
                    {
                        Data = _MyResultData, // 目前筆
                        TotalCount = _MyResultDatas != null ? _MyResultDatas.Count() : 0, // 全部

                        errors = "",

                        ResultCode = 0,
                        ResultMsg = "作業成功" + " (" + (Request.QueryString["Act"] ?? "") + ")"
                    };
                }
                else
                {
                    _MyResult = new MyResult()
                    {
                        Data = null,
                        TotalCount = 0,

                        errors = _MyMsg.DeleteFail,

                        ResultCode = -1,
                        ResultMsg = "作業失敗" + " (" + (Request.QueryString["Act"] ?? "") + ") --- " + ErrMsg
                    };
                }
            }
            else
            {
                _MyResult = new MyResult()
                {
                    Data = null,
                    TotalCount = 0, // 全部

                    errors = _MyMsg.OtherFail,

                    ResultCode = -1,
                    ResultMsg = "作業失敗" + " (" + (Request.QueryString["Act"] ?? "") + ") --- " + ErrMsg
                };
            }
            //----------------------------------------------------------------------------------------------------------
            // Response  
            // return View("~/Views/Test/TestKendoUI_CRUD.cshtml");
            return Json(_MyResult);
            //----------------------------------------------------------------------------------------------------------
        }
        //===========================================================================================================
    }
}
