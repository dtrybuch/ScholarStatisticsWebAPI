using ScholarStatistics.DAL.Models;
using System;
using System.Collections.Generic;

namespace ScholarStatistics.DAL.Interfaces
{
    public interface IPublicationsRepository
    {
        IEnumerable<Publication> GetPublications();
        Publication GetPublicationById(int id);
        bool AddPublication(Publication publication);
        bool AddPublications(List<Publication> publications);
        bool UpdatePublication(Publication publication);
        bool UpdatePublicationRange(IEnumerable<Publication> publications);
        bool RemovePublication(int id);
        IEnumerable<Publication> QueryPublications(Func<Publication, bool> predicate);
        int GetCount();
        int GetScopusCount();
    }
}
