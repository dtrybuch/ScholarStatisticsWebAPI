using ScholarStatistics.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ScholarStatistics.DAL.Interfaces
{
    public interface ICategoriesRepository
    {
        IEnumerable<Category> GetCategories();
        Category GetCategoryById(int id);
        bool AddCategory(Category category);
        bool UpdateCategory(Category category);
        bool RemoveCategory(int id);
        IEnumerable<Category> QueryCategories(Func<Category, bool> predicate);
        void FillCategoriesRepository();
    }
}
