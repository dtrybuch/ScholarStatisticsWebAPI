using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ScholarStatistics.DAL.Models
{
    public class AffiliationCategory
    {
        public int AffiliationCategoryId { get; set; }
        [ForeignKey("Affiliation")]
        public int AffiliationFK { get; set; }
        [ForeignKey("Category")]
        public int CategoryFK { get; set; }
        public int CountOfCategoryPublications { get; set; }
    }
}
