using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace GifFinder
{
    public partial class GifFinder : Form
    {
        SearchForm searchForm; // Instance variable for the search form

        // Constructor method
        public GifFinder()
        {
            InitializeComponent();

            try
            {
                // Set Internet Explorer emulation mode in registry for compatibility
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(
                @"SOFTWARE\\WOW6432Node\\Microsoft\\Internet Explorer\\MAIN\\FeatureControl\\FEATURE_BROWSER_EMULATION",
                true);
                key.SetValue(Application.ExecutablePath.Replace(Application.StartupPath + "\\", ""), 12001, Microsoft.Win32.RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {
                // If registry modification fails, do nothing
            }

            // Create an instance of the search form
            searchForm = new SearchForm();

            // Set up event handler for the timer
            timer1.Interval = 100;
            timer1.Tick += new EventHandler(Timer1_Tick);

            // Navigate to the web page for the application
            webBrowser1.Navigate("https://people.rit.edu/dxsigm/gif-finder.html");

            // Suppress script errors in the web browser control
            webBrowser1.ScriptErrorsSuppressed = true;

            // Set up event handlers for menu items
            this.tileToolStripMenuItem.Click += new EventHandler(TileToolStripMenuItem_Click);
            this.cascadeToolStripMenuItem.Click += new EventHandler(CascadeToolStripMenuItem_Click);
            this.exitToolStripMenuItem.Click += new EventHandler(ExitToolStripMenuItem_Click);
            this.newSearchToolStripMenuItem.Click += new EventHandler(NewSearchToolStripMenuItem_Click);
        }

        // Event handler for the "Tile" menu item
        private void TileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Arrange child forms in a tiled manner
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        // Event handler for the "Cascade" menu item
        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Arrange child forms in a cascaded manner
            this.LayoutMdi(MdiLayout.Cascade);
        }

        // Event handler for the "Exit" menu item
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Exit the application
            Application.Exit();
        }

        // Event handler for the "New Search" menu item
        private void NewSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Display the search form dialog box
            this.searchForm.ShowDialog();

            // If the user clicked "OK" in the search form dialog box
            if (searchForm.response == "OK")
            {
                // Set the search term in the web page's search form
                HtmlElement htmlElement = webBrowser1.Document.GetElementById("searchterm");
                htmlElement.SetAttribute("value", searchForm.searchTerm);

                // Set the maximum items in the web page's search form
                htmlElement = webBrowser1.Document.GetElementById("limit");
                htmlElement.SetAttribute("value", Convert.ToString(searchForm.maxItems));

                // Invoke the search button click event in the web page
                webBrowser1.Document.InvokeScript("searchButtonClicked");

                // Start the timer to check for new images
                timer1.Start();
            }
        }

        // Event handler for the timer
        private void Timer1_Tick(object sender, EventArgs e)
        {
            // Stop the timer
            timer1.Stop();

            // Find the HTML element that tracks the last loaded
            HtmlElement htmlElement = webBrowser1.Document.GetElementById("lastelement");
            // Check if the element exists
            if (htmlElement != null)
            {
                // Get all the HTML img elements
                HtmlElementCollection htmlElementCollection;
                htmlElementCollection = webBrowser1.Document.GetElementsByTagName("img");

                // Loop through each image element and create an ImageForm for it
                foreach (HtmlElement htmlElement1 in htmlElementCollection)
                {
                    // Create an ImageForm and pass in the image source and title attributes
                    ImageForm imageForm = new ImageForm(this, htmlElement1.GetAttribute("src"), htmlElement1.GetAttribute("title"));
                    imageForm.Show();
                }

                // Remove the lastelement HTML element from the page
                htmlElement.OuterHtml = "";
            }
            else
            {
                // If the lastelement HTML element does not exist, restart the timer
                timer1.Start();
            }
        }
    }
}