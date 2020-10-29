using ScholarStatistics.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ScholarStatistics.DAL.Interfaces
{
    public interface IAuthorsRepository
    {
        IEnumerable<Author> GetAuthors();
        Author GetAuthorById(int id);
        bool AddAuthor(Author author);
        bool UpdateAuthor(Author author);
        bool RemoveAuthor(int id);
        IEnumerable<Author> QueryAuthors(Func<Author, bool> predicate);
        List<int> AddAuthors(List<Author> authors);
    }
}
