using Microsoft.EntityFrameworkCore;
using ScholarStatistics.DAL.Interfaces;
using ScholarStatistics.DAL.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ScholarStatistics.DAL.Repositories
{
    public class PublicationsRepository : IPublicationsRepository
    {
        private readonly DatabaseContext _databaseContext;
        public PublicationsRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public bool AddPublication(Publication publication)
        {
            try
            {
                var exist = QueryPublications(publicationQuery => publicationQuery.Title.Replace("\n", "").Replace("\r", "").ToLower() == publication.Title.Replace("\n", "").Replace("\r", "").ToLower()).ToList();
                if (exist.Count() > 0) return true;
                var tracking = _databaseContext.Publications.Add(publication);
                _databaseContext.SaveChanges();
                var isAdded = tracking.State == EntityState.Added;
                return isAdded;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }
        //we need to be sure that publications are unique
        public bool AddPublications(List<Publication> publications)
        {
            try
            {
                _databaseContext.Publications.AddRange(publications);
                _databaseContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public Publication GetPublicationById(int id)
        {
            try
            {
                var publication = _databaseContext.Publications.Find(id);
                return publication;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public IEnumerable<Publication> GetPublications()
        {
            try
            {
                var publications = _databaseContext.Publications.ToList();
                return publications;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public IEnumerable<Publication> QueryPublications(Func<Publication, bool> predicate)
        {
            try
            {
                var publications = _databaseContext.Publications.Where(predicate);
                return publications.ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public bool RemovePublication(int id)
        {
            try
            {
                var publication = _databaseContext.Publications.Find(id);
                var tracking = _databaseContext.Publications.Remove(publication);
                _databaseContext.SaveChanges();
                var isDeleted = tracking.State == EntityState.Deleted;
                return isDeleted;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public bool UpdatePublication(Publication publication)
        {
            try
            {
                var tracking = _databaseContext.Publications.Update(publication);
                _databaseContext.SaveChanges();
                var isModified = tracking.State == EntityState.Modified;
                return isModified;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }
        public bool UpdatePublicationRange(IEnumerable<Publication> publications)
        {
            try
            {
                _databaseContext.Publications.UpdateRange(publications);
                _databaseContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }
    }
}
