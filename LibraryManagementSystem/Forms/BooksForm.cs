using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Data;

namespace LibraryManagementSystem.Forms
{
    public partial class BooksForm : Form
    {
        public BooksForm()
        {
            InitializeComponent();
            LoadBooks();
        }

        private int selectedBookId = 0;


        private void btnAddBook_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text) ||
        string.IsNullOrWhiteSpace(txtAuthor.Text) ||
        string.IsNullOrWhiteSpace(txtCategory.Text) ||
        string.IsNullOrWhiteSpace(txtISBN.Text) ||
        string.IsNullOrWhiteSpace(PublicationYear.Text) ||
        string.IsNullOrWhiteSpace(txtAvailableCopies.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (!int.TryParse(PublicationYear.Text, out int year))
            {
                MessageBox.Show("Publication Year must be a valid number.");
                return;
            }

            if (!int.TryParse(txtAvailableCopies.Text, out int copies))
            {
                MessageBox.Show("Available Copies must be a valid number.");
                return;
            }

            Book book = new Book();

            book.Title = txtTitle.Text;
            book.Author = txtAuthor.Text;
            book.Category = txtCategory.Text;
            book.ISBN = txtISBN.Text;
            book.PublicationYear = year;
            book.AvailableCopies = copies;

            DatabaseManager db = new DatabaseManager();
            db.AddBook(book);

            MessageBox.Show("Book added successfully.");

            LoadBooks();

            txtTitle.Clear();
            txtAuthor.Clear();
            txtCategory.Clear();
            txtISBN.Clear();
            PublicationYear.Clear();
            txtAvailableCopies.Clear();

            txtTitle.Focus();

           
        }
        private void LoadBooks()
        {
            DatabaseManager db = new DatabaseManager();

            dgvBooks.DataSource = db.GetAllBooks();
            dgvBooks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBooks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBooks.MultiSelect = false;
            dgvBooks.ReadOnly = true;
        }

        private void BooksForm_Load(object sender, EventArgs e)
        {

        }

        private void dgvBooks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void btnDeleteBook_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
    "Are you sure you want to delete this book?",
    "Confirm Delete",
    MessageBoxButtons.YesNo,
    MessageBoxIcon.Question);

            if (result == DialogResult.No)
                return;
            if (selectedBookId == 0)
            {
                MessageBox.Show("Please select a book first.");
                return;
            }

            DatabaseManager db = new DatabaseManager();
            db.DeleteBook(selectedBookId);

            MessageBox.Show("Book deleted successfully.");

            LoadBooks();

            selectedBookId = 0;

            txtTitle.Clear();
            txtAuthor.Clear();
            txtCategory.Clear();
            txtISBN.Clear();
            PublicationYear.Clear();
            txtAvailableCopies.Clear();
        }

        private void btnUpdateBook_Click(object sender, EventArgs e)
        {
            if (selectedBookId == 0)
            {
                MessageBox.Show("Please select a book first.");
                return;
            }

            Book book = new Book();

            book.Title = txtTitle.Text;
            book.Author = txtAuthor.Text;
            book.Category = txtCategory.Text;
            book.ISBN = txtISBN.Text;
            book.PublicationYear = int.Parse(PublicationYear.Text);
            book.AvailableCopies = int.Parse(txtAvailableCopies.Text);

            DatabaseManager db = new DatabaseManager();
            db.UpdateBook(book, selectedBookId);

            MessageBox.Show("Book updated successfully.");

            LoadBooks();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            DatabaseManager db = new DatabaseManager();

            dgvBooks.DataSource = db.SearchBooks(txtSearch.Text);
        }

        private void dgvBooks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvBooks.Rows[e.RowIndex];

                selectedBookId = Convert.ToInt32(row.Cells["BookId"].Value);

                txtTitle.Text = row.Cells["Title"].Value.ToString();
                txtAuthor.Text = row.Cells["Author"].Value.ToString();
                txtCategory.Text = row.Cells["Category"].Value.ToString();
                txtISBN.Text = row.Cells["ISBN"].Value.ToString();
                PublicationYear.Text = row.Cells["PublishedYear"].Value.ToString();
                txtAvailableCopies.Text = row.Cells["AvailableCopies"].Value.ToString();
            }
        }
    }
}
