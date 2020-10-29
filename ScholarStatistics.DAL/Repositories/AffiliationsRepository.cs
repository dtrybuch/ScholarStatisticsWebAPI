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
    public class AffiliationsRepository : IAffiliationsRepository
    {
        private readonly DatabaseContext _databaseContext;
        public AffiliationsRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public int AddAffiliation(Affiliation affiliation)
        {
            try
            {
                var exist = QueryAffiliations(affiliationQuery => affiliationQuery.Name == affiliation.Name).ToList();
                if (exist.Count() > 0) return exist[0].AffiliationId;
                var tracking = _databaseContext.Affiliations.Add(affiliation);
                _databaseContext.SaveChanges();
                return tracking.Entity.AffiliationId;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return 0;
            }
        }

        public Affiliation GetAffiliationById(int id)
        {
            try
            {
                var affiliation = _databaseContext.Affiliations.Find(id);
                return affiliation;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public IEnumerable<Affiliation> GetAffiliations()
        {
            try
            {
                var affiliations = _databaseContext.Affiliations.ToList();
                return affiliations;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public IEnumerable<Affiliation> QueryAffiliations(Func<Affiliation, bool> predicate)
        {
            try
            {
                var affiliations = _databaseContext.Affiliations.Where(predicate);
                return affiliations.ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public bool RemoveAffiliation(int id)
        {
            try
            {
                var affiliation = _databaseContext.Affiliations.Find(id);
                var tracking = _databaseContext.Affiliations.Remove(affiliation);
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

        public bool UpdateAffiliation(Affiliation affiliation)
        {
            try
            {
                var tracking = _databaseContext.Affiliations.Update(affiliation);
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
    }
}
