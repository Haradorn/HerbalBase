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

namespace HealthHerbs
{
    public partial class HerbAddForm : Form
    {
        public HerbAddForm()
        {
            InitializeComponent();
        }

        private void ButtonAddPhoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Пожалуйста выберите фотографию";
            ofd.Filter = "JPG|*.jpg|PNG|*.png|GIF|*.gif";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.pictureBoxPhoto.ImageLocation = ofd.FileName;
            }
        }
    }
}
