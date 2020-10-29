using System;
using System.Collections.Generic;
using System.Text;

namespace ScholarStatistics.DAL.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime DifferenceBetweenPublications { get; set; }
        public float RatioPublications { get; set; }
    }
}
