using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRCWhatIFRequestAPI.Models
{

    public class ConnectionName
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }
    public class CommonWhatifRequestQueryModel
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
    public class WhatifRequestHomePageQueryModel
    {
        public string TanentId { get; set; }
        public string ProfileId { get; set; }
        public string IsRequests { get; set; }

        public string ParamStatus { get; set; }
        public string PageIndex { get; set; }
        public string PageSize { get; set; }
        public string SearchColumn { get; set; }
        public string SearchValue { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }

    }
    public class WhatifRequestHomePage
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Requesttype { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public string Requestorfirstname { get; set; }
        public string Requestorlastname { get; set; }
        public string Displayname { get; set; }
        public string Requestedon { get; set; }
        public string Lastupdatedon { get; set; }
        public string Totalstages { get; set; }
        public string Serialno { get; set; }
        public string Stagetype { get; set; }
        public string Modifyingobject { get; set; }
        public string Stageentrydate { get; set; }
        public string Applicationname { get; set; }
        public string Isconnectionowner { get; set; }
        public string State { get; set; }
        public string Whatifstatus { get; set; }
        public string Analysisstatus { get; set; }
        public string Isviolationpresent { get; set; }
        public string Isaod { get; set; }
        public string Isusertoaddexists { get; set; }
        public string Currentstage { get; set; }
        public string Isapprovercommentmandatory { get; set; }
        public string Cantakeaction { get; set; }
        public string Processmodulename { get; set; }

    }

}
