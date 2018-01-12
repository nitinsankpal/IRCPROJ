using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IRCWhatIFRequestAPI.Models;

namespace IRCWhatIFRequestAPI.DBOprations
{
    public class InforWhatIFRequesDBOperation
    {
        public List<Users> getUsers(UserQueryModel model)
        {
            List<Users> WhatIFUserList = new List<Users>();

            WhatIFUserList.Add(
                             new Users
                             {
                                 UserName = "1",
                                 UserID = "ION UC",
                                 StartDate = "Desc"
                             });
            return WhatIFUserList;
        }
        public List<RolesFromConnection> getRolesFromSelectedConnection(RolesQueryModel model)
        {
            List<RolesFromConnection> RolesList = new List<RolesFromConnection>();

            RolesList.Add(
                             new RolesFromConnection
                             {
                                 RoleID = "1",
                                 RID = "ION UC",
                                 RoleName = "Desc"
                             });
            return RolesList;
        }
        public List<RolesFromConnection> getRolesFromOtherConnection(RolesQueryModel model)
        {
            List<RolesFromConnection> RolesList = new List<RolesFromConnection>();

            RolesList.Add(
                             new RolesFromConnection
                             {
                                 RoleID = "1",
                                 RID = "ION UC",
                                 RoleName = "Desc"
                             });
            return RolesList;
        }
        public List<RolesFromConnection> getRoles(RolesQueryModel model)
        {
            List<RolesFromConnection> RolesList = new List<RolesFromConnection>();

            RolesList.Add(
                             new RolesFromConnection
                             {
                                 RoleID = "1",
                                 RID = "ION UC",
                                 RoleName = "Desc"
                             });
            return RolesList;
        }
        public List<Permission> getPermissions(PermissionQueryModel model)
        {
            List<Permission> PermissionList = new List<Permission>();

            PermissionList.Add(
                             new Permission
                             {
                                 PermissionID = "P1",
                                 PersmissionDescription = "P2 descriptions",
                                 PemissionName = "R2 Permission",
                                 ID = "12"
                             });

            return PermissionList;
        }
        public List<PermissionValues> getPermissionsValues(PermissionValueQueryModel model)
        {
            List<PermissionValues> PermissionValueList = new List<PermissionValues>();

            PermissionValueList.Add(
                             new PermissionValues
                             {
                                 PermissionValue = "V1"
                             });

            return PermissionValueList;
        }
    }
}
