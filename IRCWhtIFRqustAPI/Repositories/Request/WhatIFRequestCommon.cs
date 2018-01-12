using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IRCWhatIFRequestAPI.Models;
using IRCWhatIFRequestAPI.DBOprations;
using IRCWhatIFRequestAPI.Repositories;

namespace IRCWhatIFRequestAPI.Repositories.Request
{
    public class WhatIFRequestCommon: IWhatIFRequestCommon
    {
        public WhatIFRequestCommon()
        {
        }
        public List<ConnectionName> getConnectionForWhatIFRequest(CommonWhatifRequestQueryModel model)
        {
            try
            {
                WhatIFRequestCommonDBOperation WhatIFReqObj = new WhatIFRequestCommonDBOperation();
                return WhatIFReqObj.getConnectionName(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<WhatifRequestHomePage> getWhatIFRequestHomePage(WhatifRequestHomePageQueryModel model)
        {
            try
            {
                WhatIFRequestCommonDBOperation WhatIFReqObj = new WhatIFRequestCommonDBOperation();
                return WhatIFReqObj.getWhatIFRequestHomePage(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
