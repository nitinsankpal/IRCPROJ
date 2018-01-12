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
    public class InforWhatIfRequestController
    {
        [HttpPost]
        [Route("GetUsers")]
        public IEnumerable<Users> getUsers([FromBody]UserQueryModel Queryparam)
        {
            IInforWhatIFRequest Obj = new InforWhatIFRequest();
            return Obj.getUsers(Queryparam);
        }
        [HttpPost]
        [Route("GetRolesFromSelectedConnection")]
        public IEnumerable<RolesFromConnection> getRolesFromSelectedConnection([FromBody]RolesQueryModel Queryparam)
        {
            IInforWhatIFRequest Obj = new InforWhatIFRequest();
            return Obj.getRolesFromSelectedConnection(Queryparam);
        }

        [HttpPost]
        [Route("GetRolesFromOtherConnection")]
        public IEnumerable<RolesFromConnection> getRolesFromOtherConnection([FromBody]RolesQueryModel Queryparam)
        {
            IInforWhatIFRequest Obj = new InforWhatIFRequest();
            return Obj.getRolesFromOthersConnection(Queryparam);
        }

        [HttpPost]
        [Route("GetRoles")]
        public IEnumerable<RolesFromConnection> getRoles([FromBody]RolesQueryModel Queryparam)
        {
            IInforWhatIFRequest Obj = new InforWhatIFRequest();
            return Obj.getRoles(Queryparam);
        }

        [HttpPost]
        [Route("CreatePayload")]
        public bool CreatePayload([FromBody]WhatIFReuestPayload WhatIFReuestPayload)
        {
            bool IsPayload=false;
            if (WhatIFReuestPayload != null)
            {
                 IInforWhatIFRequest Obj = new InforWhatIFRequest();
                 IsPayload = Obj.WhatIFReuestCreatePayload(WhatIFReuestPayload);         
            }
            return IsPayload;
        }
    
        [HttpPost]
        [Route("GetPermissions")]
        public IEnumerable<Permission> GetPermissions([FromBody]PermissionQueryModel Queryparam)
        {
            IInforWhatIFRequest Obj = new InforWhatIFRequest();
            return Obj.getPermission(Queryparam);
        }


        [HttpPost]
        [Route("GetPermissionValues")]
        public IEnumerable<PermissionValues> GetPermissionValues([FromBody]PermissionValueQueryModel Queryparam)
        {
            IInforWhatIFRequest Obj = new InforWhatIFRequest();
            return Obj.getpermissionValues(Queryparam);
        }



        //Product[] productsList
    }
}
