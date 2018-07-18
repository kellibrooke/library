using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Library.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public Book(string title, int id = 0)
        {
            Title = title;
            Id = id;
        }

        public override bool Equals(System.Object otherBook)
        {
            if (!(otherBook is Book))
            {
                return false;
            }
            else
            {
                Book newBook = (Book)otherBook;
                bool idEquality = this.Id == newBook.Id;
                bool titleEquality = this.Title == newBook.Title;

                return (idEquality && titleEquality);
            }
        }

        public override int GetHashCode()
        {
            return this.Title.GetHashCode();
        }

        public void Save(Author newAuthor, Copy newCopy)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO books (title) VALUES (@Title); INSERT INTO authors (name) VALUES (@AuthorName); INSERT INTO copies (total) VALUES (@TotalCopies);";

            cmd.Parameters.AddWithValue("@Title", this.Title);
            cmd.Parameters.AddWithValue("@AuthorName", newAuthor.Name);
            cmd.Parameters.AddWithValue("@TotalCopies", newCopy.Total);

            cmd.ExecuteNonQuery();
            newAuthor.Id = (int)cmd.LastInsertedId;
            newCopy.Id = (int)cmd.LastInsertedId;
            Id = (int)cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void AddBookDetails(Author newAuthor, Copy newCopy)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO authors_books(author_id, book_id) VALUES(@AuthorId, @BookId); INSERT INTO copies_books(copy_id, book_id) VALUES(@CopyId, @BookId);";


            cmd.Parameters.AddWithValue("@AuthorId", newAuthor.Id);
            cmd.Parameters.AddWithValue("@CopyId", newCopy.Id);
            cmd.Parameters.AddWithValue("@BookId", this.Id);

            cmd.ExecuteNonQuery();
            Id = (int)cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List<Author> GetBooksByAuthor()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT authors.* FROM books
                                JOIN authors_books ON (books.id = authors_books.book_id)
                                JOIN authors ON (authors_books.author_id = authors.id)
                                WHERE book_id = @BookId;";

            cmd.Parameters.AddWithValue("@StudentId", this.Id);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;

            List<Author> allAuthors = new List<Author> { };

            while (rdr.Read())
            {
                int authorId = rdr.GetInt32(0);
                string authorName = rdr.GetString(1);
                Author newAuthor = new Author(authorName, authorId);
                allAuthors.Add(newAuthor);
            }

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return allAuthors;
        }

        //public List<Copy> GetAvailableCopies()
        //{
        //    MySqlConnection conn = DB.Connection();
        //    conn.Open();
        //    var cmd = conn.CreateCommand() as MySqlCommand;
        //    cmd.CommandText = @"SELECT copies.* FROM books
        //                        JOIN copies_books ON (books.id = copies_books.book_id)
        //                        JOIN copies ON (authors_books.copies_id = copies.id)
        //                        WHERE book_id = @BookId;";

        //    cmd.Parameters.AddWithValue("@StudentId", this.Id);
        //    var rdr = cmd.ExecuteReader() as MySqlDataReader;

        //    List<Copy> allCopies = new List<Copy> { };

        //    while (rdr.Read())
        //    {
        //        int copyId = rdr.GetInt32(0);
        //        int copyTotal = rdr.GetInt32(1);
        //        Copy newCopy = new Copy(copyTotal, copyId);
        //        allCopies.Add(newCopy);
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
