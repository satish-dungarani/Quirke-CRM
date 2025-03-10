﻿using Quirke.CRM.Domain;

namespace Quirke.CRM.Models
{
    public class CustomerViewModel
    {
        public CustomerViewModel()
        {
            Customer = new CustomerModel();
            ComplianceRecords = new List<CustomerComplianceModel>();
            CustomerRecords = new List<CustomerRecordModel>();
            Compliance = new CustomerComplianceModel();
        }

        public IEnumerable<CustomerRecordModel> CustomerRecords { get; set; }
        public IEnumerable<CustomerComplianceModel> ComplianceRecords { get; set; }

        public CustomerModel Customer { get; set; } 
        public CustomerComplianceModel Compliance { get; set; } 
    }
}
