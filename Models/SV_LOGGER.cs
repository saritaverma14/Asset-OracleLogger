//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Asset_OracleLogger.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class SV_LOGGER
    {
        public decimal ID { get; set; }
        [DisplayName("Program:")]
        public string PROGRAM { get; set; }
        [DisplayName("Project:")]
        public string PROJECT { get; set; }
        [DisplayName("Release:")]
        public string RELEASE { get; set; }
        [DisplayName("Sprint:")]
        public string SPRINT { get; set; }
        [DisplayName("KeyData:")]
        public string KEYDATA { get; set; }
        public Nullable<System.DateTime> TIMERECEIVED { get; set; }
        public Nullable<System.DateTime> RESPONSETIME { get; set; }
        public string METHOD { get; set; }
        public string URI { get; set; }
        [DisplayName("Api Operation:")]
        public string APIOPERATION { get; set; }
        [DisplayName("Request:")]
        public string RESPONSE { get; set; }
        [DisplayName("Response:")]
        public string REQUEST { get; set; }
        [DisplayName("Operation:")]
        public string OPERATION { get; set; }
        public string SERVICETYPE { get; set; }
        public List<SV_LOGGER> sDetails { get; set; }
    }
}
