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
                case "indexAdd":
                    {
                        string fflag;
                        string newna = context.Request.Form["newname"];
                        string newpa = context.Request.Form["newpass"];
                        DataTable indexSelect = Common.Excute.ExecuteQuery("select password,five,fivewin,bird from GameData where username = '" + newna + "'");
                        if (indexSelect != null && indexSelect.Rows.Count > 0)
                        {
                            fflag = "失败";
                            context.Response.ContentType = "text/plain";
                            context.Response.Write(fflag);
                        }
                        else
                        {
                            fflag = "成功";
                            Common.Excute.ExcuteCount("insert into GameData (username,password,five,fivewin,bird) values ('" + newna + "','" + newpa + "',0,0,0)");
                            context.Response.ContentType = "text/plain";
                            context.Response.Write(fflag);
                        }
                    }
                    break;
                case "five":
                    {
                        string nowusername = context.Request.Form["nowusername"];
                        // DataTable five = Common.Excute.ExecuteQuery("select username,fivewin from GameData where fivewin = (select max(fivewin) from GameData)");
                        DataTable five = Common.Excute.ExecuteQuery("select top 3 username,fivewin from GameData order by fivewin desc");
                        DataTable nowuser = Common.Excute.ExecuteQuery("select fivewin from GameData where username = '" + nowusername + "'");
                        // string most = context.Request.Form["most"];
                        DataRow aa = nowuser.Rows[0];
                        string c = aa[0].ToString();

                        //DataRow r = five.Rows[0];
                        string a1;
                        string a2;
                        string a3;
                        string b;

                        a1 = five.Rows[0]["username"].ToString() + "`" + five.Rows[0]["fivewin"].ToString();
                        a2 = five.Rows[1]["username"].ToString() + "`" + five.Rows[1]["fivewin"].ToString();
                        a3 = five.Rows[2]["username"].ToString() + "`" + five.Rows[2]["fivewin"].ToString();

                        // string d = a + "`" + b + "`" +c;
                        string d = a1 + "`" + a2 + "`" + a3 + "`" + c;
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(d);
                    }
                    break;
                case "fivewin":
                    {
                        Common.Excute.ExcuteCount("update GameData set fivewin = fivewin + 1 where username = '" + context.Request.Form["username"] + "'");
                        // string username = context.Request.Form["username"];
                        // DataTable fivewin = Common.Excute.ExecuteQuery("select fivewin from GameData where username = '"+username+"'");
                        // DataRow fivewinRow = fivewin.Rows[0];
                        // string newscore = fivewinRow["fivewin"].ToString();
                        context.Response.ContentType = "text/plain";
                        context.Response.Write("");
                    }
                    break;
                case "flappy":
                    {
                        string nowusername = context.Request.Form["nowusername"];
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