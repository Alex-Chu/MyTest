using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Drawing;

namespace WebApplication2
{
    public partial class TestKendoUI2 : System.Web.UI.Page
    {
        protected void Page_Init(object sender, System.EventArgs e)
        {
            this.EnableViewState = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

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

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            //---------------------------------------------------------------------------------
            string sTemp = "";
            string sPageName = "";
            //---------------------------------------------------------------------------------
            if (Request["pageSize"] == null && Request["take"] == null && Request["skip"] == null)
            {
                base.Render(writer);
                return;
            }
            //---------------------------------------------------------------------------------
            //sPageName = Request.Url.Segments.Last();  // Ver. 4.5
            sPageName = Request.Url.Segments[Request.Url.Segments.Length-1];
           
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
            string keywd = Request["keywd"];
            string sortField = Request["sort[0][field]"];
            string sortDir = Request["sort[0][dir]"];

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
            int pageSize = 10, take = 10, skip = 0;
            pageSize = int.TryParse(Request["pageSize"], out pageSize) ? pageSize : 10;
            take=int.TryParse(Request["take"], out take)?take:10;
            skip = int.TryParse(Request["skip"], out skip) ? skip : 0;
            //var paged = res.Skip(skip).Take(take);
            var paged = res.Skip(skip).Take(Math.Min(pageSize,take));
            sTemp = jss.Serialize(new ResultData()
            {
                Data = paged.ToList(),
                TotalCount = res.Count()
            });
            //---------------------------------------------------------------------------------
            // Response  Jason
            Response.Clear();
            Response.ContentType = "application/json";
            Response.Write(sTemp);
            Response.Flush();
            Response.End();
            //---------------------------------------------------------------------------------
        }
    }
}