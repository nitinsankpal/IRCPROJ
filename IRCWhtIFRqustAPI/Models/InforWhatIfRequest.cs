using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRCWhatIFRequestAPI.Models
{
    public class UserQueryModel
    {
        public string TanentId { get; set; }
        public string ProfileId { get; set; }
        public string PageIndex { get; set; }
        public string PageSize { get; set; }
        public string SearchColumn { get; set; }
        public string SearchValue { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }

    }
    public class RolesQueryModel
    {
        public string TanentId { get; set; }
        public string UserName { get; set; }
        public string ConnectionID { get; set; }
        public string PageIndex { get; set; }
        public string PageSize { get; set; }
        public string SearchColumn { get; set; }
        public string SearchValue { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }

    }
    public class Users
    {
        public string UserName { get; set; }
        public string UserID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string EnailAddress { get; set; }
        public string EmployeeFullName { get; set; }
        public string AssinedID { get; set; }
        public string MangerProfile { get; set; }
        public string MangerID { get; set; }
        public string FirestName { get; set; }
        public string LastName { get; set; }

    }
    public class RolesFromConnection
    {
        public string RoleID { get; set; }
        public string RID { get; set; }
        public string RoleName { get; set; }
        public string RoleType { get; set; }
        public string RoleDesc { get; set; }
        public string ApplicationID { get; set; }
        public string ValidFrom { get; set; }
        public string ValidTo { get; set; }
        public string RoleUniqueID { get; set; }
        public string ApplicationName { get; set; }

    }
    public class WhatIFReuestPayload
    {
        public string FirstName { get; set; }
        public string UserName { get; set; }
        public string LastName { get; set; }

        public int CreatedBy { get; set; }
        public string[] NewRoles { get; set; }
        public string[] RevokedRoles { get; set; }
        public string[] ExistingRoles { get; set; }
        public bool isUC { get; set; }
        public bool isSDK { get; set; }

        public GenericRequestPayload GenericFields;
    }
    public class GenericRequestPayload
    {
        public string name;
        public string ProfileID;
        public string ProfileName;
        public int connectionID;
        public RuleBookPermissionList[] rulebooksInfo;
    }
    public class RuleBookPermissionList
    {
        public string name;
        public int id;
    }
    public class PermissionQueryModel
    {
        public string TanentId { get; set; }
        public string RoleID { get; set; }
        public string ConnectionID { get; set; }
        public string PageIndex { get; set; }
        public string PageSize { get; set; }
        public string SearchColumn { get; set; }
        public string SearchValue { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }

    }
    public class Permission
    {
        public string ID { get; set; }
        public string PemissionName { get; set; }
        public string PermissionID { get; set; }
        public string PersmissionDescription { get; set; }

    }
    public class PermissionValueQueryModel
    {
        public string TanentId { get; set; }
        public string ConnectionID { get; set; }
        public string PageIndex { get; set; }
        public string PageSize { get; set; }
        public string SearchColumn { get; set; }
        public string SearchValue { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }

    }
    public class PermissionValues
    {
        public string PermissionValue { get; set; }
    }


}
