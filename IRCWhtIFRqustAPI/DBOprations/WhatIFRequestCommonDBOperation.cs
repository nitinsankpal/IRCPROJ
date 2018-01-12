using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IRCWhatIFRequestAPI.Models;
using System.Data;
using System.Data.SqlClient;

namespace IRCWhatIFRequestAPI.DBOprations
{
    public class WhatIFRequestCommonDBOperation
    {
        public List<ConnectionName> getConnectionName(CommonWhatifRequestQueryModel model)
        {
            List<ConnectionName> ConnectioList = new List<ConnectionName>();

            ConnectioList.Add(
                             new ConnectionName
                             {
                                 ID = "1",
                                 Name = "ION Connection"

                             });
            return ConnectioList;
        }
        public List<WhatifRequestHomePage> getWhatIFRequestHomePage(WhatifRequestHomePageQueryModel model)
        {
            List<WhatifRequestHomePage> WhatIFRequestList = new List<WhatifRequestHomePage>();

            WhatIFRequestList.Add(
                             new WhatifRequestHomePage
                             {
                                 ID = "1",
                                 Name = "ION UC",
                                 Description = "Desc"




                             });
            return WhatIFRequestList;
        }
    }
}
