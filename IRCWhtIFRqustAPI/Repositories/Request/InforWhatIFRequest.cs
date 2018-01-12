using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IRCWhatIFRequestAPI.Models;
using IRCWhatIFRequestAPI.DBOprations;
using IRCWhatIFRequestAPI.Repositories.Request;
using IRCWhatIFRequestAPI.Utils;
using System.Xml;
using IRCWhatIFRequestAPI.Services;

namespace IRCWhatIFRequestAPI.Repositories.Request
{
    public class InforWhatIFRequest : IInforWhatIFRequest
    {
        public List<Users> getUsers(UserQueryModel model)
        {
            try
            {
                InforWhatIFRequesDBOperation WhatIFReqObj = new InforWhatIFRequesDBOperation();
                return WhatIFReqObj.getUsers(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<RolesFromConnection> getRolesFromSelectedConnection(RolesQueryModel model)
        {
            try
            {
                InforWhatIFRequesDBOperation WhatIFReqObj = new InforWhatIFRequesDBOperation();
                return WhatIFReqObj.getRolesFromSelectedConnection(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<RolesFromConnection> getRolesFromOthersConnection(RolesQueryModel model)
        {
            try
            {
                InforWhatIFRequesDBOperation WhatIFReqObj = new InforWhatIFRequesDBOperation();
                return WhatIFReqObj.getRolesFromOtherConnection(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<RolesFromConnection> getRoles(RolesQueryModel RoleModel)
        {
            try
            {
                InforWhatIFRequesDBOperation WhatIFReqObj = new InforWhatIFRequesDBOperation();
                return WhatIFReqObj.getRoles(RoleModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool WhatIFReuestCreatePayload(WhatIFReuestPayload BuildPayload)
        {
            bool isCreated=false;
            try
            {
                string docPayload = Util.BuildPayLoadXML(BuildPayload);
                isCreated=WhatIFRequestImplementaionService.CreateTemplate(BuildPayload.isSDK, docPayload, BuildPayload.CreatedBy);

            }
            catch(Exception ex)
            {
                throw ex;
            }
            return isCreated;
        }
        public List<Permission> getPermission(PermissionQueryModel PermissionModel)
        {
            try
            {
                InforWhatIFRequesDBOperation PermissionObj = new InforWhatIFRequesDBOperation();
                return PermissionObj.getPermissions(PermissionModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<PermissionValues> getpermissionValues(PermissionValueQueryModel ValueModel)
        {
            try
            {
                InforWhatIFRequesDBOperation ValueModelObj = new InforWhatIFRequesDBOperation();
                return ValueModelObj.getPermissionsValues(ValueModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
