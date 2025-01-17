﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ScholarStatistics.DAL.Models
{
    public class Affiliation
    {
        public int AffiliationId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public double Lattitude { get; set; }
        public double Longitude { get; set; }
        public int CountOfTopTenCategories { get; set; }
        public int CountOfPublications { get; set; }
        [ForeignKey("Category")]
        public virtual List<int> CategoriesUsingInThisAffiliationFK { get; set; }
    }
}
