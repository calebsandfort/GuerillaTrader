using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GuerillaTrader.Web.Framework
{
    public class GuerillaLogisticsApiJsonResult : ActionResult
    {
        public object Data
        {
            get;
            set;
        }

        public GuerillaLogisticsApiJsonResult()
        {

        }

        public GuerillaLogisticsApiJsonResult(object data)
        {
            this.Data = data;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "application/json";

            if (Data != null)
            {
                response.Write(JsonConvert.SerializeObject(this.Data));
            }
        }
    }
}