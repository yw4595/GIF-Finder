using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GifFinder
{
    public partial class SearchForm : Form
    {
        // Variables to store user input from form
        public string response;
        public string searchTerm;
        public int maxItems;
        // Constructor for SearchForm
        public SearchForm()
        {
            InitializeComponent();

            // Register event handlers for buttons and text box
            this.okButton.Click += new EventHandler(okButton_Click);
            this.cancelButton.Click += new EventHandler(cancelButton_Click);
            this.maxItemsTextBox.KeyPress += new KeyPressEventHandler(MaxItemsTextBox_KeyPress);
        }

        // Event handler for key press on maxItemsTextBox
        private void MaxItemsTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;

            // Allow only digits and backspace
            if (Char.IsDigit(e.KeyChar) || e.KeyChar == '\b')
            {
                e.Handled = false;
            }
        }

        // Event handler for okButton
        private void okButton_Click(object sender, EventArgs e)
        {
            // Store user input in variables and hide the form
            this.response = "OK";
            this.searchTerm = searchTermTextBox.Text;
            this.maxItems = Convert.ToInt32(maxItemsTextBox.Text);
            this.Hide();
        }

        // Event handler for cancelButton
        private void cancelButton_Click(object sender, EventArgs e)
        {
            // Set response variable to "Cancel" and hide the form
            this.response = "Cancel";
            this.Hide();
        }
    }
}