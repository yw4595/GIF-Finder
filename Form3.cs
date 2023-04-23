using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

// Define namespace
namespace GifFinder
{
    public partial class ImageForm : Form
    {
        // Declare instance variables
        string title;
        // Constructor method
        public ImageForm(GifFinder parent, string url, string title)
        {
            InitializeComponent();

            // Set parent form and window title
            this.MdiParent = parent;
            this.Text = title;

            // Add event handlers for parent form menu items
            parent.closeAllToolStripMenuItem.Click += new EventHandler(CloseAllToolStripMenuItem_Click);
            parent.saveToolStripMenuItem.Click += new EventHandler(SaveToolStripMenuItem_Click);

            // Add event handler for window closing
            this.FormClosing += new FormClosingEventHandler(ImageForm_FormClosing);

            // Load image from URL into picture box
            pictureBox1.ImageLocation = url;
        }

        // Event handler for "Close All" menu item
        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Close current window
            this.Close();
        }

        // Event handler for form closing
        private void ImageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Get reference to parent form
            GifFinder parent = (GifFinder)this.MdiParent;

            // Remove event handlers for parent form menu items
            parent.closeAllToolStripMenuItem.Click += new EventHandler(CloseAllToolStripMenuItem_Click);
            parent.saveToolStripMenuItem.Click += new EventHandler(SaveToolStripMenuItem_Click);
        }

        // Event handler for "Save" menu item
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Check if current window is active
            if (this != this.MdiParent.ActiveMdiChild)
            {
                return;
            }

            // Set file type filter for save dialog
            saveFileDialog1.Filter = "Bmp(*.BMP) *.BMP | Jpg (*.JPG)|*.JPG | Png (*.PNG) | *.PNG || Gif (*.GIF) | *.GIF";

            // Set default file name for save dialog
            saveFileDialog1.FileName = this.title;

            // Display save dialog and save file if user selects a file name
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                switch (Path.GetExtension(saveFileDialog1.FileName).ToUpper())
                {
                    case ".BMP":
                        pictureBox1.Image.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;

                    case ".JPG":
                    case ".JEPG":
                        pictureBox1.Image.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;

                    case ".PNG":
                        pictureBox1.Image.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Png);
                        break;

                    case ".GIF":
                        pictureBox1.Image.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                }
            }
        }
    }
}