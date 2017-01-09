using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GameWeb
{  
    /// <summary>
    /// hand 的摘要说明
    /// </summary>
    public class hand : IHttpHandler
    {
        
        public void ProcessRequest(HttpContext context)
        {
            string method = context.Request.Form["method"];
            switch (method)
            {
                case "index":
                    {
                        string username = context.Request.Form["username"];
                        string password = context.Request.Form["password"];
                        bool flag = false;
                        string result = "失败";
                        // int i = Common.Excute.Execute("select * from GameData");
                        DataTable dt = Common.Excute.ExecuteQuery("select * from GameData where username='" + username + "' and password = '" + password + "'");
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            flag = true;
                        }
                        if (flag)
                        {
                            result = "成功";
                        }
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(result);
                    }
                    break;
                case "five":
                    {
                        DataTable five = Common.Excute.ExecuteQuery("select username,fivewin from GameData where fivewin = (select max(fivewin) from GameData)");
                       // string now = context.Request.Form["now"];
                       // string most = context.Request.Form["most"];
                        DataRow r = five.Rows[0];
                        string a = r["username"].ToString();
                        string b = r["fivewin"].ToString();

                        string d = a + "`" + b ;
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(d);
                    }
                    break;
                default: break;
            }
            
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}