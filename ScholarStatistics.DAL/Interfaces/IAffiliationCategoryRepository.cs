using ScholarStatistics.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScholarStatistics.DAL.Interfaces
{
    public interface IAffiliationCategoryRepository
    {
        IEnumerable<AffiliationCategory> GetAffiliationCategories();
        AffiliationCategory GetAffiliationCategoryById(int id);
        int AddAffiliationCategory(AffiliationCategory affiliationCategory);
        bool UpdateAffiliationCategory(AffiliationCategory affiliationCategory);
        bool UpdateAffiliationCategories(IEnumerable<AffiliationCategory> affiliationCategories);
        bool RemoveAffiliationCategory(int id);
        bool RemoveAffiliationCategories(List<AffiliationCategory> affiliationCategories);
        IEnumerable<AffiliationCategory> QueryAffiliationCategories(Func<AffiliationCategory, bool> predicate);
    }
}
