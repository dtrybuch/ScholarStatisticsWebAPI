using ScholarStatistics.DAL.Models;
using System;
using System.Collections.Generic;

namespace ScholarStatistics.DAL.Interfaces
{
    public interface IAffiliationsRepository
    {
        IEnumerable<Affiliation> GetAffiliations();
        Affiliation GetAffiliationById(int id);
        int AddAffiliation(Affiliation affiliation);
        bool UpdateAffiliation(Affiliation affiliation);
        bool UpdateAffiliations(IEnumerable<Affiliation> affiliations);
        bool RemoveAffiliation(int id);
        IEnumerable<Affiliation> QueryAffiliations(Func<Affiliation, bool> predicate);
    }
}
