using LibraryManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LibraryManagementSystem.Data
{
    public class DatabaseManager
    {
        private  SqlConnection connection;
        public DatabaseManager ()
        {
            connection = new SqlConnection("Server=pgh;Database=LibraryManagementDB;Trusted_Connection=True;TrustServerCertificate=True;");

        }
        public void AddBook(Book book)
        {
            string query = @"INSERT INTO Books
                            (Title, Author, Category, ISBN, PublishedYear, AvailableCopies)
                            VALUES
                            (@Title, @Author, @Category, @ISBN, @PublishedYear, @AvailableCopies)";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Title", book.Title);
            command.Parameters.AddWithValue("@Author", book.Author);
            command.Parameters.AddWithValue("@Category", book.Category);
            command.Parameters.AddWithValue("@ISBN", book.ISBN);
            command.Parameters.AddWithValue("@PublishedYear", book.PublicationYear);
            command.Parameters.AddWithValue("@AvailableCopies", book.AvailableCopies);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        public DataTable GetAllBooks()
        {
            string query = "SELECT * FROM Books";

            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);

            DataTable table = new DataTable();

            adapter.Fill(table);

            return table;
        }
        public void DeleteBook(int bookId)
        {
            string query = "DELETE FROM Books WHERE BookId=@BookId";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@BookId", bookId);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void UpdateBook(Book book, int bookId)
        {
            string query = @"UPDATE Books
                     SET Title=@Title,
                         Author=@Author,
                         Category=@Category,
                         ISBN=@ISBN,
                         PublishedYear=@PublishedYear,
                         AvailableCopies=@AvailableCopies
                     WHERE BookId=@BookId";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Title", book.Title);
            command.Parameters.AddWithValue("@Author", book.Author);
            command.Parameters.AddWithValue("@Category", book.Category);
            command.Parameters.AddWithValue("@ISBN", book.ISBN);
            command.Parameters.AddWithValue("@PublishedYear", book.PublicationYear);
            command.Parameters.AddWithValue("@AvailableCopies", book.AvailableCopies);
            command.Parameters.AddWithValue("@BookId", bookId);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        public DataTable SearchBooks(string keyword)
        {
            string query = @"SELECT * FROM Books
                     WHERE Title LIKE @Keyword
                     OR Author LIKE @Keyword
                     OR Category LIKE @Keyword
                     OR ISBN LIKE @Keyword";

            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);

            adapter.SelectCommand.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

            DataTable table = new DataTable();
            adapter.Fill(table);

            return table;
        }
    }
}

