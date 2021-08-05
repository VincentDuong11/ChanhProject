using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChanhProject.Models
{
    public class BookModel
    {
        public Int64 Id { get; set; }
        public Int64 Quantity { get; set; }
        public String Title { get; set; }
        public List<Author> Authors { get; set; }
        public List<EventModel> Events { get; set; }

        public BookModel()
        {
        }

        public BookModel(String title)
        {
            this.Title = title;
        }

        public List<String> Headers()
        {
            return new List<String>(new string[] {"ID", "Title" });
        }

        public List<BookModel> List()
        {
            SqliteConnection conn = Database.GetConnection();
            var command = conn.CreateCommand();
            command.CommandText = "SELECT * FROM books;";
            List<BookModel> res = new List<BookModel>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var title = reader.GetString(1);
                    res.Add(new BookModel(title));
                }
            }

            return res;
        }
    }
}
