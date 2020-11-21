using Microsoft.EntityFrameworkCore;
using ScholarStatistics.DAL.Interfaces;
using ScholarStatistics.DAL.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScholarStatistics.DAL
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly DatabaseContext _databaseContext;
        public CategoriesRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public bool AddCategory(Category category)
        {
            try
            {
                var tracking = _databaseContext.Categories.Add(category);
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

        public Category GetCategoryById(int id)
        {
            try
            {
                var category = _databaseContext.Categories.Find(id);
                return category;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public IEnumerable<Category> GetCategories()
        {
            try
            {
                var Categories = _databaseContext.Categories.ToList();
                return Categories;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public IEnumerable<Category> QueryCategories(Func<Category, bool> predicate)
        {
            try
            {
                var Categories = _databaseContext.Categories.Where(predicate);
                return Categories.ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public bool RemoveCategory(int id)
        {
            try
            {
                var category = _databaseContext.Categories.Find(id);
                var tracking = _databaseContext.Categories.Remove(category);
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

        public bool UpdateCategory(Category category)
        {
            try
            {
                var tracking = _databaseContext.Categories.Update(category);
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

        public void FillCategoriesRepository()
        {
            CategoriesString.GetCategoriesStrings().ForEach(categoryString =>
            {
                var result = QueryCategories(category => category.Code == categoryString) ?? new List<Category>();
                if (result.Count() == 0)
                    AddCategory(new Category()
                    {
                        Code = categoryString,
                        Name = CategoriesString.GetName(categoryString)
                    });
            });
        }

        public bool UpdateCategories(List<Category> categories)
        {
            try
            {
                _databaseContext.Categories.UpdateRange(categories);
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
