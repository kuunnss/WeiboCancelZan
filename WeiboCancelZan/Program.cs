using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ConsoleApp5
{
    class Program
    {
        static void Main(string[] args)
        {
            var count = 0;
            var ids = GetIds(GetIndex());
            while (ids.Count != 0)
            {
                foreach (var i in ids)
                {
                    Delete(i);
                    Thread.Sleep(1000);
                }
                ids = GetIds(GetIndex());
                count++;
            }
            Console.WriteLine("over" + count);
            Console.ReadKey();
        }

        private static void Delete(string id)
        {
            var req = (HttpWebRequest)WebRequest.Create("https://weibo.com/aj/v6/like/add?ajwvr=6&__rnd=" +
                                                         GetTimeStamp());
            req.Method = "post";
            req.Referer = "https://weibo.com/like/outbox?leftnav=1";
            req.Host = "weibo.com";

            req.UserAgent =
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.84 Safari/537.36";
            req.Headers.Set("Origin", "https://weibo.com");
            req.ContentType = "application/x-www-form-urlencoded";
            var str =
                $"location=v6_likes_outbox&group_source=&version=mini&qid=heart&mid={id}&like_src=1&cuslike=1&_t=0";
            req.Headers.Set("cookie",
                "");
            var b = Encoding.UTF8.GetBytes(str);
            using (var r = req.GetRequestStream())
            {
                r.Write(b, 0, b.Length);
            }
            using (var res = req.GetResponse())
            {
                var sr = res.GetResponseStream();
                var ss = new StreamReader(sr);
                Console.WriteLine(ss.ReadToEnd());
            }
        }

        /// <summary>  
        /// 获取时间戳  
        /// </summary>  
        /// <returns></returns>  
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        private static string GetIndex()
        {
            var req = (HttpWebRequest)WebRequest.Create("https://weibo.com/like/outbox?leftnav=1");
            req.Method = "Get";
            req.Host = "weibo.com";
            req.UserAgent =
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.84 Safari/537.36";

            req.Headers.Set("cookie",
                "SINAGLOBAL=9938402999562.186.1513664862077; wvr=6; wb_cmtLike_1805985651=1; YF-Ugrow-G0=8751d9166f7676afdce9885c6d31cd61; login_sid_t=8ec21cde041fdc827662f5d5719d5a47; cross_origin_proto=SSL; YF-V5-G0=1312426fba7c62175794755e73312c7d; _s_tentry=passport.weibo.com; Apache=429864579322.15.1514167969240; ULV=1514167969244:3:3:1:429864579322.15.1514167969240:1513832857020; SCF=AqPgWHv_OW43FhGVsNn2Iv0sMmj1J-kGt8eKsmbfxz3-g5LQBZkprnmDxdAuEc8ECux1mHUtBPVHoeC5mu64XlU.; SUB=_2A253RC_LDeRhGedG61cY-CvKzj2IHXVUMAYDrDV8PUNbmtBeLWPWkW9NUZDF9WOGh21rsoWqIN1A1B4GmIRgh6m7; SUBP=0033WrSXqPxfM725Ws9jqgMF55529P9D9WFglO2M3DnQ9gRC7WuaLB2R5JpX5KzhUgL.Fo2Reh-41h-cSK22dJLoIEBLxKqL1KMLBKMLxK-LBo.LBoBLxKnL1h5L1h2LxKML12-L12zt; SUHB=0h1IlqsYWyP3Cy; ALF=1545704219; SSOLoginState=1514168219; YF-Page-G0=ab26db581320127b3a3450a0429cde69; UOR=www.52bug.cn,widget.weibo.com,www.baidu.com; wb_cusLike_1805985651=N");
            using (var res = req.GetResponse())
            {
                var sr = new StreamReader(res.GetResponseStream());
                var str = sr.ReadToEnd();
                return str;
            }
        }

        public static List<string> GetIds(string html)
        {
            var ids = new List<string>();
            var rrr = new Regex("mid=\\\\\\\"\\d{16}\\\\\\\"");
            MatchCollection mc = rrr.Matches(html);
            foreach (Match ma in mc)
            {
                ids.Add(ma.Value.Substring(6, 16));
            }
            return ids;

        }
    }
}
