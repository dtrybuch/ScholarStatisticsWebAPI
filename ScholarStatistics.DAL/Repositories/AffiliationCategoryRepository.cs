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
    public class AffiliationCategoryRepository : IAffiliationCategoryRepository
    {
        private readonly DatabaseContext _databaseContext;
        public AffiliationCategoryRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public int AddAffiliationCategory(AffiliationCategory affiliationCategories)
        {
            try
            {
                var exist = QueryAffiliationCategories(affiliationCategoriesQuery => affiliationCategoriesQuery.AffiliationFK == affiliationCategories.AffiliationFK &&
                affiliationCategoriesQuery.CategoriesFK == affiliationCategories.CategoriesFK).ToList();
                if (exist.Any())
                {
                    if(exist[0].CountOfCategoryPublications != affiliationCategories.CountOfCategoryPublications)
                    {
                        exist[0].CountOfCategoryPublications = affiliationCategories.CountOfCategoryPublications;
                        UpdateAffiliationCategory(exist[0]);
                    }
                    return exist[0].AffiliationCategoryId;
                };
                var tracking = _databaseContext.AffiliationCategories.Add(affiliationCategories);
                _databaseContext.SaveChanges();
                return tracking.Entity.AffiliationCategoryId;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return 0;
            }
        }

        public AffiliationCategory GetAffiliationCategoryById(int id)
        {
            try
            {
                var affiliationCategories = _databaseContext.AffiliationCategories.Find(id);
                return affiliationCategories;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public IEnumerable<AffiliationCategory> GetAffiliationCategories()
        {
            try
            {
                var affiliationCategoriess = _databaseContext.AffiliationCategories.ToList();
                return affiliationCategoriess;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public IEnumerable<AffiliationCategory> QueryAffiliationCategories(Func<AffiliationCategory, bool> predicate)
        {
            try
            {
                var affiliationCategoriess = _databaseContext.AffiliationCategories.Where(predicate);
                return affiliationCategoriess.ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public bool RemoveAffiliationCategory(int id)
        {
            try
            {
                var affiliationCategories = _databaseContext.AffiliationCategories.Find(id);
                var tracking = _databaseContext.AffiliationCategories.Remove(affiliationCategories);
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

        public bool RemoveAffiliationCategories(List<AffiliationCategory> affiliationCategoriess)
        {
            try
            {
                _databaseContext.AffiliationCategories.RemoveRange(affiliationCategoriess);
                _databaseContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public bool UpdateAffiliationCategory(AffiliationCategory affiliationCategories)
        {
            try
            {
                var tracking = _databaseContext.AffiliationCategories.Update(affiliationCategories);
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

        public bool UpdateAffiliationCategories(IEnumerable<AffiliationCategory> affiliationCategoriess)
        {
            try
            {
                _databaseContext.AffiliationCategories.UpdateRange(affiliationCategoriess);
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
