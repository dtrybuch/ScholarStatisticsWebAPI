using System;
using System.Collections.Generic;
using System.Text;

namespace ScholarStatistics.DAL.Helpers
{
    public interface IFilterDataHelper
    {
        void SaveArxivDataByCategories();
        void SavePublicationsFromScopus();
        void SetDifferenceBetweenPublicationsInDays();
        void SetRatioPublications();
        void SetCountryToCategories();
        void SetAffiliationLatAndLong();
        void SaveCountOfDays();
        void AddCountOfPublicationsToCategory();
        void AddCountOfPublicationsToAffiliation();
        void AddCountOfTopTenCategoriesToAffiliation();
        void AddCategoriesToAffiliation();
        void AddValueToAffiliationCategory();
        void AddCountOfPublicationFromScopus();
    }
}
