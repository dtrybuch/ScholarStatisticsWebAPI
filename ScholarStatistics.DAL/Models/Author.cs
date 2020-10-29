using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ScholarStatistics.DAL.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [ForeignKey("Affiliation")]
        public int AffiliationFK { get; set; }
    }
}
