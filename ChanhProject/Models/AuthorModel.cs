using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChanhProject.Models
{
    public class AuthorModel
    {
        public Int64 Id { get; set; }
        public String Name { get; set; }

        public static AuthorModel Get(Int64 id)
        {
            var res = List(id);
            if (res.Count == 0) return null;

            return res[0];
        }

        public static List<String> Headers()
        {
            return new List<String>(new string[] { "ID", "Name" });
        }

        public static void Delete(Int64 authorId)
        {
            SqliteConnection conn = Database.GetConnection();

            using (var transaction = conn.BeginTransaction())
            {
                var cmd = conn.CreateCommand();
                cmd.Transaction = transaction;
                cmd.CommandText = "DELETE FROM book_author WHERE author_id=@id;" +
                    "DELETE FROM authors WHERE id = @id;";
                cmd.Parameters.AddWithValue("id", authorId);

                cmd.ExecuteNonQuery();
                transaction.Commit();
            }
        }

        public static List<AuthorModel> List(Int64 authorId = -1)
        {
            SqliteConnection conn = Database.GetConnection();
            var command = conn.CreateCommand();
            command.CommandText = "SELECT * FROM authors ";
            if (authorId >= 0)
            {
                command.CommandText += "WHERE id = $author_id;";
                command.Parameters.AddWithValue("author_id", authorId);
            }
            var res = new List<AuthorModel>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    res.Add(new AuthorModel
                    {
                        Id = reader.GetInt64(0),
                        Name = reader.GetString(1)
                    });
                }
            }

            return res;
        }

        public static void Upsert(AuthorModel model)
        {

            SqliteConnection conn = Database.GetConnection();

            using (var transaction = conn.BeginTransaction())
            {
                var cmd = conn.CreateCommand();
                cmd.Transaction = transaction;
                if (model.Id > 0)
                {
                    cmd.CommandText = "UPDATE authors SET name = @name WHERE id = @id;";
                    cmd.Parameters.AddWithValue("@id", model.Id);
                }
                else
                {
                    cmd.CommandText = "INSERT OR IGNORE INTO authors (name) VALUES (@name);";
                }
                cmd.Parameters.AddWithValue("name", model.Name);
                cmd.ExecuteNonQuery();
                transaction.Commit();
            }
        }
    }
}
