using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScholarStatistics.DAL.Models
{
    public class Publication
    {
        public int PublicationID { get; set; }
        public string Title { get; set; }
        public DateTime DateOfAddedToArxiv { get; set; }
        public DateTime DateOfPublished { get; set; }
        [ForeignKey("Category")]
        public virtual List<int> CategoriesFK { get; set; }
        [ForeignKey("Affiliation")]
        public virtual int AffiliationFK { get; set; }
        public virtual int CountOfCited { get; set; }
        public bool WasCheckedInScopus { get; set; }

    }
}