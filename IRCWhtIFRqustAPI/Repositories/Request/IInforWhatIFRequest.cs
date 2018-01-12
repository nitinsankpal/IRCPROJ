using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IRCWhatIFRequestAPI.Models;

namespace IRCWhatIFRequestAPI.Repositories.Request
{
    public interface IInforWhatIFRequest
    {
        List<Users> getUsers(UserQueryModel model);
        List<RolesFromConnection> getRolesFromSelectedConnection(RolesQueryModel model);
        List<RolesFromConnection> getRolesFromOthersConnection(RolesQueryModel model);
        List<RolesFromConnection> getRoles(RolesQueryModel model);
        List<Permission> getPermission(PermissionQueryModel model);
        List<PermissionValues> getpermissionValues(PermissionValueQueryModel model);
        bool WhatIFReuestCreatePayload(WhatIFReuestPayload BuildPayload);
    }
}
