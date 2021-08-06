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
        public List<AuthorModel> Authors { get; set; }

        public BookModel()
        {
        }


        public static List<String> Headers()
        {
            return new List<String>(new string[] {"ID", "Title", "Quantity", "Authors" });
        }

        public static BookModel Get(Int64 bookId)
        {
            var res = List(bookId);
            if (res.Count == 0) return null;

            return res[0];
        }

        public static void Delete(Int64 bookId)
        {
            SqliteConnection conn = Database.GetConnection();

            using (var transaction = conn.BeginTransaction())
            {
                var cmd = conn.CreateCommand();
                cmd.Transaction = transaction;
                cmd.CommandText = "DELETE FROM book_author WHERE book_id=@id;" +
                    "DELETE FROM books WHERE id = @id;";
                cmd.Parameters.AddWithValue("id", bookId);

                cmd.ExecuteNonQuery();
                transaction.Commit();
            }
        }

        public static List<BookModel> List(Int64 bookId = -1)
        {
            SqliteConnection conn = Database.GetConnection();
            var command = conn.CreateCommand();
            command.CommandText = "SELECT b.id, b.title, b.quantity, a.id, a.name  FROM books b JOIN book_author ba ON b.id = ba.book_id JOIN authors a ON a.id = ba.author_id ";
            if (bookId >= 0)
            {
                command.CommandText += "WHERE b.id = $bookId;";
                command.Parameters.AddWithValue("bookId", bookId);
            }
            var res = new Dictionary<Int64, BookModel>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var author = new AuthorModel
                    {
                        Id = reader.GetInt64(3),
                        Name = reader.GetString(4),
                    };
                    var bookIdKey = reader.GetInt64(0);
                    if (res.ContainsKey(bookIdKey))
                    {
                        res[bookIdKey].Authors.Add(author);
                        continue;
                    }
                    res[bookIdKey] = new BookModel
                    {
                        Id = bookIdKey,
                        Title = reader.GetString(1),
                        Quantity = reader.GetInt64(2),
                        Authors = new List<AuthorModel>(new AuthorModel[] { author })
                    };
                }
            }

            return new List<BookModel>(res.Values);
        }

        public static void Upsert(BookModel model)
        {

            SqliteConnection conn = Database.GetConnection();

            using (var transaction = conn.BeginTransaction())
            {
                var cmd = conn.CreateCommand();
                cmd.Transaction = transaction;
                if(model.Id > 0)
                {
                    cmd.CommandText = "UPDATE books SET title = @title, quantity = @quantity WHERE id = @id;";
                    cmd.Parameters.AddWithValue("@id", model.Id);
                } else
                {
                    cmd.CommandText = "INSERT OR IGNORE INTO books (title, quantity) VALUES (@title, @quantity);";
                }
                cmd.Parameters.AddWithValue("title", model.Title);
                cmd.Parameters.AddWithValue("quantity", model.Quantity);
                cmd.ExecuteNonQuery();
                if(model.Id == 0)
                {
                    cmd.CommandText = "select last_insert_rowid();";
                    model.Id = (Int64)cmd.ExecuteScalar();
                }
                cmd.CommandText = "DELETE FROM book_author WHERE book_id=@bookId;";
                cmd.Parameters.AddWithValue("@bookId", model.Id);
                foreach (var author in model.Authors)
                {
                    cmd.CommandText += $"INSERT OR IGNORE INTO book_author (book_id,author_id) VALUES ({model.Id}, {author.Id});";
                }

                cmd.ExecuteNonQuery();
                transaction.Commit();
            }
        }
    }
}
