using System;
using System.Web;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Linq;

public class JsonDataSrc : IHttpHandler
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

    public void ProcessRequest(HttpContext context)
    {
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
        string keywd = context.Request["keywd"];
        string sortField = context.Request["sort[0][field]"];
        string sortDir = context.Request["sort[0][dir]"];

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


        JavaScriptSerializer jss = new JavaScriptSerializer();
        context.Response.ContentType = "text/plain";
        int pageSize = 10, take = 10, skip = 0;
        int.TryParse(context.Request["pageSize"], out pageSize);
        int.TryParse(context.Request["take"], out take);
        int.TryParse(context.Request["skip"], out skip);
        var paged = res.Skip(skip).Take(take);
        context.Response.Write(jss.Serialize(new ResultData()
        {
            Data = paged.ToList(),
            TotalCount = res.Count()
        }));
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}