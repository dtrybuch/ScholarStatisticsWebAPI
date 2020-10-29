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
        bool UpdatePublication(Publication publication);
        bool RemovePublication(int id);
        IEnumerable<Publication> QueryPublications(Func<Publication, bool> predicate);
    }
}
