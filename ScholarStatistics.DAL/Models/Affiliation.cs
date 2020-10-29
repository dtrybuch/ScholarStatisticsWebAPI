using System;
using System.Collections.Generic;
using System.Text;

namespace ScholarStatistics.DAL.Models
{
    public class Affiliation
    {
        public int AffiliationId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
