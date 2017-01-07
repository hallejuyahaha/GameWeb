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
            string username = context.Request.Form["username"];

            DataTable dt = Common.Excute.ExecuteQuery("select * from GameData");
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
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