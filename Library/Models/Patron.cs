using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Library.Models
{
    public class Patron
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Patron(string name, int id = 0)
        {
            Name = name;
            Id = id;
        }

        public override bool Equals(System.Object otherPatron)
        {
            if (!(otherPatron is Book))
            {
                return false;
            }
            else
            {
                Patron newPatron = (Patron)otherPatron;
                bool idEquality = this.Id == newPatron.Id;
                bool nameEquality = this.Name == newPatron.Name;

                return (idEquality && nameEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO patrons (name) VALUES (@Name);";
            cmd.Parameters.AddWithValue("@Name", this.Name);

            cmd.ExecuteNonQuery();
            Id = (int)cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public int FindRemainingCopies(Book newBook)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT copies.* FROM books
                                JOIN copies_books ON (books.id = copies_books.book_id)
                                JOIN copies ON (copies_books.copy_id = copies.id)
                                WHERE book_id = @BookId;";
            
            cmd.Parameters.AddWithValue("@BookId", newBook.Id);
            cmd.ExecuteNonQuery();

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int totalCopies = 0;

            while (rdr.Read())
            {
                totalCopies = rdr.GetInt32(1);
            }

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return totalCopies;
        }

        public void Checkout(Book newBook)
        {
            int totalCopies = this.FindRemainingCopies(newBook);
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE copies_books SET copy_id = @TotalCopies WHERE book_id = @BookId; INSERT INTO patrons_books (patron_id, book_id) VALUES (@PatronId, @BookId);";

            cmd.Parameters.AddWithValue("@TotalCopies", totalCopies);
            cmd.Parameters.AddWithValue("@BookId", newBook.Id);
            cmd.Parameters.AddWithValue("@PatronId", this.Id);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        //public List<Author> GetBooksByAuthor()
        //{
        //    MySqlConnection conn = DB.Connection();
        //    conn.Open();
        //    var cmd = conn.CreateCommand() as MySqlCommand;
        //    cmd.CommandText = @"UPDATE authors.* FROM books
        //                        JOIN authors_books ON (books.id = authors_books.book_id)
        //                        JOIN authors ON (authors_books.author_id = authors.id)
        //                        WHERE book_id = @BookId;";

        //    cmd.Parameters.AddWithValue("@StudentId", this.Id);
        //    var rdr = cmd.ExecuteReader() as MySqlDataReader;

        //    List<Author> allAuthors = new List<Author> { };

        //    while (rdr.Read())
        //    {
        //        int authorId = rdr.GetInt32(0);
        //        string authorName = rdr.GetString(1);
        //        Author newAuthor = new Author(authorName, authorId);
        //        allAuthors.Add(newAuthor);
        //    }

        //    conn.Close();
        //    if (conn != null)
        //    {
        //        conn.Dispose();
        //    }

        //    return allAuthors;
        //}
    }
}