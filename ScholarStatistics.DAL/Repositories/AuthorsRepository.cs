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
    public class AuthorsRepository : IAuthorsRepository
    {
        private readonly DatabaseContext _databaseContext;
        public AuthorsRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public bool AddAuthor(Author author)
        {
            try
            {
                var isExistAuthor = QueryAuthors(authorDB => authorDB.FirstName == author.FirstName && authorDB.LastName == author.LastName).ToArray();
                if (isExistAuthor.Count() > 0) return true;
                    var tracking = _databaseContext.Authors.Add(author);
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

        public List<int> AddAuthors(List<Author> authors)
        {
            try
            {
                var ids = new List<int>();
                foreach (var author in authors)
                {
                    var isExistAuthor = QueryAuthors(authorDB => authorDB.FirstName == author.FirstName && authorDB.LastName == author.LastName).ToArray();
                    if (isExistAuthor.Count() == 0)
                    {
                        var tracking = _databaseContext.Authors.Add(author);
                        _databaseContext.SaveChanges();
                        ids.Add(tracking.Entity.AuthorId);
                    }
                    else
                        ids.Add(isExistAuthor[0].AuthorId);
                }
                return ids;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public Author GetAuthorById(int id)
        {
            try
            {
                var author =_databaseContext.Authors.Find(id);
                return author;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public IEnumerable<Author> GetAuthors()
        {
            try
            {
                var authors = _databaseContext.Authors.ToList();
                return authors;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public IEnumerable<Author> QueryAuthors(Func<Author, bool> predicate)
        {
            try
            {
                var authors = _databaseContext.Authors.Where(predicate);
                return authors.ToList();
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public bool RemoveAuthor(int id)
        {
            try
            {
                var author =  _databaseContext.Authors.Find(id);
                var tracking = _databaseContext.Authors.Remove(author);
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

        public bool UpdateAuthor(Author author)
        {
            try
            {
                var tracking = _databaseContext.Authors.Update(author);
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
