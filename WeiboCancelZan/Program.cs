using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using Newtonsoft.Json;

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
                    Delete2(i);
                    Thread.Sleep(5000);
                }
                ids = GetIds(GetIndex());
                count++;
            }
            Console.WriteLine("over" + count);
            Console.ReadKey();
        }

        private static void Delete2(string id)
        {
            var req = (HttpWebRequest)WebRequest.Create("https://weibo.com/aj/like/del?ajwvr=6");
            req.Method = "post";
            req.Referer = "https://weibo.com/like/outbox?leftnav=1";
            req.Host = "weibo.com";

            req.UserAgent =
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.84 Safari/537.36";
            req.Headers.Set("Origin", "https://weibo.com");
            req.ContentType = "application/x-www-form-urlencoded";
            var str =
                $"mid={id}";
            req.Headers.Set("cookie",
                "SINAGLOBAL=9938402999562.186.1513664862077; wvr=6; wb_cmtLike_1805985651=1; YF-Ugrow-G0=8751d9166f7676afdce9885c6d31cd61; login_sid_t=8ec21cde041fdc827662f5d5719d5a47; cross_origin_proto=SSL; YF-V5-G0=1312426fba7c62175794755e73312c7d; _s_tentry=passport.weibo.com; Apache=429864579322.15.1514167969240; ULV=1514167969244:3:3:1:429864579322.15.1514167969240:1513832857020; SSOLoginState=1514168219; YF-Page-G0=ab26db581320127b3a3450a0429cde69; SUBP=0033WrSXqPxfM725Ws9jqgMF55529P9D9WFglO2M3DnQ9gRC7WuaLB2R5JpX5KMhUgL.Fo2Reh-41h-cSK22dJLoIEBLxKqL1KMLBKMLxK-LBo.LBoBLxKnL1h5L1h2LxKML12-L12zt; ALF=1545790653; SCF=AqPgWHv_OW43FhGVsNn2Iv0sMmj1J-kGt8eKsmbfxz3-hdUOBhTYpZvEk2N79RfskNhSgEhjH9zGAk2KRQUhJic.; SUB=_2A253RcFuDeRhGedG61cY-CvKzj2IHXVUMrWmrDV8PUNbmtBeLUPZkW9NUZDF9S8NNkgqrL4uSEL1ygDyRzV6aTv1; SUHB=06ZEzfmwYXmKfZ; UOR=www.52bug.cn,widget.weibo.com,login.sina.com.cn; wb_cusLike_1805985651=N");
            var b = Encoding.UTF8.GetBytes(str);
            using (var r = req.GetRequestStream())
            {
                r.Write(b, 0, b.Length);
            }
            using (var res = req.GetResponse())
            {
                var sr = res.GetResponseStream();
                var ss = new StreamReader(sr);
                var str2 = ss.ReadToEnd();
                var model = JsonConvert.DeserializeObject<ReturnModel>(str2);
                Console.WriteLine(HttpUtility.UrlDecode(model.msg));
                Console.WriteLine(model.code);
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
                "SINAGLOBAL=9938402999562.186.1513664862077; wvr=6; wb_cmtLike_1805985651=1; YF-Ugrow-G0=8751d9166f7676afdce9885c6d31cd61; login_sid_t=8ec21cde041fdc827662f5d5719d5a47; cross_origin_proto=SSL; YF-V5-G0=1312426fba7c62175794755e73312c7d; _s_tentry=passport.weibo.com; Apache=429864579322.15.1514167969240; ULV=1514167969244:3:3:1:429864579322.15.1514167969240:1513832857020; SSOLoginState=1514168219; YF-Page-G0=ab26db581320127b3a3450a0429cde69; SUBP=0033WrSXqPxfM725Ws9jqgMF55529P9D9WFglO2M3DnQ9gRC7WuaLB2R5JpX5KMhUgL.Fo2Reh-41h-cSK22dJLoIEBLxKqL1KMLBKMLxK-LBo.LBoBLxKnL1h5L1h2LxKML12-L12zt; ALF=1545790653; SCF=AqPgWHv_OW43FhGVsNn2Iv0sMmj1J-kGt8eKsmbfxz3-hdUOBhTYpZvEk2N79RfskNhSgEhjH9zGAk2KRQUhJic.; SUB=_2A253RcFuDeRhGedG61cY-CvKzj2IHXVUMrWmrDV8PUNbmtBeLUPZkW9NUZDF9S8NNkgqrL4uSEL1ygDyRzV6aTv1; SUHB=06ZEzfmwYXmKfZ; UOR=www.52bug.cn,widget.weibo.com,login.sina.com.cn; wb_cusLike_1805985651=N");
            using (var res = req.GetResponse())
            {
                var sr = new StreamReader(res.GetResponseStream());
                var str = sr.ReadToEnd();
                res.Close();
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
                var temp = ma.Value.Substring(6, 16);
                if (!ids.Contains(temp))
                    ids.Add(ma.Value.Substring(6, 16));
            }
            return ids;

        }
    }
}
