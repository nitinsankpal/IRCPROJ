using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IRCWhatIFRequestAPI.Repositories.Request;
using IRCWhatIFRequestAPI.Models;
using IRCWhatIFRequestAPI.Services;


namespace IRCWhatIFRequestAPI.Controllers
{
    public class WhatIfRequestcommonController
    {
        /// <summary>
        /// Get Connection list for What-IF and Request Creations.
        /// </summary>       
        [HttpPost]
        [Route("GetConnectionList")]
        public IEnumerable<ConnectionName> getConnectionForWhatIFRequest([FromBody]CommonWhatifRequestQueryModel Queryparam)
        {
            IWhatIFRequestCommon Obj = new WhatIFRequestCommon();
            return Obj.getConnectionForWhatIFRequest(Queryparam);
        }

        /// <summary>
        /// Get Connection list for What-IF and Request Creations.
        /// </summary>       
        [HttpPost]
        [Route("GetWhatIFRequestHomePage")]
        public IEnumerable<WhatifRequestHomePage> getWhatIFRequestHomePage([FromBody]WhatifRequestHomePageQueryModel Queryparam)
        {
            IWhatIFRequestCommon Obj = new WhatIFRequestCommon();
            return Obj.getWhatIFRequestHomePage(Queryparam);
        }

    }
}
