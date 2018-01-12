using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IRCWhatIFRequestAPI.Models;

namespace IRCWhatIFRequestAPI.Repositories.Request
{
    public interface IWhatIFRequestCommon
    {
       List<ConnectionName> getConnectionForWhatIFRequest(CommonWhatifRequestQueryModel model);
       List<WhatifRequestHomePage> getWhatIFRequestHomePage(WhatifRequestHomePageQueryModel model);
    
    }
}
