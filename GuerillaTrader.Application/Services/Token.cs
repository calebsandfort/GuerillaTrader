using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GuerillaTrader.Services
{
    public class Token
    {
        public static string Cookie { get; private set; }
        public static string Crumb { get; private set; }

        private static Regex _regexCrumb;

        /// <summary>
        /// Refresh cookie and crumb value
        /// </summary>
        /// <param name="symbol">Stock ticker symbol</param>
        /// <returns></returns>
        public static bool Refresh(string symbol)
        {

            try
            {
                Cookie = "";
                Crumb = "";

                var urlScrape = "https://finance.yahoo.com/quote/{0}?p={0}";

                var url = string.Format(urlScrape, symbol);

                var request = (HttpWebRequest)WebRequest.Create(url);

                request.CookieContainer = new CookieContainer();
                request.Method = "GET";

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    var cookie = response.GetResponseHeader("Set-Cookie").Split(';')[0];

                    var html = "";

                    using (var stream = response.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            html = new StreamReader(stream).ReadToEnd();
                        }
                    }

                    if (html.Length < 5000)
                    {
                        return false;
                    }
                    var crumb = GetCrumb(html);

                    if (crumb != null)
                    {
                        Cookie = cookie;
                        Crumb = crumb;
                        //Log.Debug(string.Format("Crumb: '{0}', Cookie: '{1}'", crumb, cookie));
                        return true;
                    }

                }

            }
            catch (Exception ex)
            {
                //Log.Error(ex.Message);
            }

            return false;

        }

        /// <summary>
        /// Reset the value for Cookie and Crumb
        /// </summary>
        public static void Reset()
        {
            Crumb = "";
            Cookie = "";
        }

        /// <summary>
        /// Get crumb value from HTML
        /// </summary>
        /// <param name="html">HTML code</param>
        /// <returns></returns>
        private static string GetCrumb(string html)
        {

            string crumb = null;

            try
            {
                //initialize on first time use
                if (_regexCrumb == null)
                {
                    _regexCrumb = new Regex("CrumbStore\":{\"crumb\":\"(?<crumb>.+?)\"}",
                        RegexOptions.CultureInvariant | RegexOptions.Compiled);
                }

                var matches = _regexCrumb.Matches(html);

                if (matches.Count > 0)
                {
                    crumb = matches[0].Groups["crumb"].Value;
                }
                else
                {
                    //Log.Debug("Regex no match");
                }

                //prevent regex memory leak
                matches = null;

            }
            catch (Exception ex)
            {
                //Log.Error(ex.Message);
            }

            GC.Collect();
            return crumb;
        }
    }
}
