using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;
using System.IO;

namespace HealthHerbs
{
    public partial class Form1 : Form
    {
        HerbsContext db;
        public Form1()
        {
            InitializeComponent();
            db = new HerbsContext();
            db.Herbs.Load();
            dataGridView1.DataSource = db.Herbs.Local.ToBindingList();
            List<Herbs> herbs = db.Herbs.ToList();
            this.listHerbs.DataSource = herbs;
            this.listHerbs.ValueMember = "Id";
            this.listHerbs.DisplayMember = "Name";
        }
        private byte[] ConvertFiletoByte(string sPath)
        {
            byte[] data = null;
            if (sPath == null)
            {
                sPath = @"C:\Users\Amadeus\source\repos\HealthHerbs\empty.jpg";
                FileInfo fInfo = new FileInfo(sPath);
                long numBytes = fInfo.Length;
                FileStream fStream = new FileStream(sPath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fStream);
                data = br.ReadBytes((int)numBytes);
                return data;
            }
            else
            {
                FileInfo fInfo = new FileInfo(sPath);
                long numBytes = fInfo.Length;
                FileStream fStream = new FileStream(sPath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fStream);
                data = br.ReadBytes((int)numBytes);
                return data;
            }
        }
        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            HerbAddForm hrForm = new HerbAddForm();
            DialogResult result = hrForm.ShowDialog(this);
            if (result == DialogResult.Cancel)
                return;
            else
            {
                try
                {
                    Herbs herbs = new Herbs();
                    herbs.Name = hrForm.textBox1.Text;
                    herbs.Description = hrForm.textBox2.Text;
                    herbs.Photo = ConvertFiletoByte(hrForm.pictureBoxPhoto.ImageLocation);
                    db.Herbs.Add(herbs);
                    db.SaveChanges();
                    MessageBox.Show("Новый объект добален");
                }
                catch
                {
                    MessageBox.Show("Не получилось добавить новый объект");
                }
            }
        }
        private Image ConvertBytetoImage(byte[] photo)
        {
            Image newImage;
            using (MemoryStream ms = new MemoryStream(photo, 0, photo.Length))
            {
                ms.Write(photo, 0, photo.Length);
                newImage = Image.FromStream(ms, true);
                return newImage;
            }
        }
        private void ButtonSearch_Click(object sender, EventArgs e)
        {
            try
            {
                HerbsContext db = new HerbsContext();
                string name = textBox2.Text;
                Herbs Hb = db.Herbs.Single(hb => hb.Name == name);
                this.textBox3.Text = Hb.Description;
                this.pictureBox1.Image = ConvertBytetoImage(Hb.Photo);
            }
            catch
            {
                MessageBox.Show("Не удалось найти объект");
            }
        }
        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int index = dataGridView1.SelectedRows[0].Index;
                int id = 0;
                bool converted = Int32.TryParse(dataGridView1[0, index].Value.ToString(), out id);
                if (converted == false)
                    return;
                Herbs hb = db.Herbs.Find(id);
                db.Herbs.Remove(hb);
                db.SaveChanges();
                MessageBox.Show("Объект удален");
            }
        }
        private void ButtonChange_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int index = dataGridView1.SelectedRows[0].Index;
                int id = 0;
                bool converted = Int32.TryParse(dataGridView1[0, index].Value.ToString(), out id);
                if (converted == false)
                    return;
                Herbs hb = db.Herbs.Find(id);
                HerbAddForm hrForm = new HerbAddForm();
                hrForm.textBox1.Text = hb.Name;
                hrForm.textBox2.Text = hb.Description;
                hrForm.pictureBoxPhoto.Image = ConvertBytetoImage(hb.Photo);
                DialogResult result = hrForm.ShowDialog(this);
                if (result == DialogResult.Cancel)
                    return;
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Желаете поменять изображение для этого объекта? " +
                        "Если вы не выбрали новое изображение для него и нажали Да, то старое изображение пропадет",
                        "Сменить изображение?", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        hb.Name = hrForm.textBox1.Text;
                        hb.Description = hrForm.textBox2.Text;
                        hb.Photo = ConvertFiletoByte(hrForm.pictureBoxPhoto.ImageLocation);
                        db.SaveChanges();
                        dataGridView1.Refresh();
                        MessageBox.Show("Объект обновлен");
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        hb.Name = hrForm.textBox1.Text;
                        hb.Description = hrForm.textBox2.Text;
                        db.SaveChanges();
                        dataGridView1.Refresh();
                        MessageBox.Show("Объект обновлен");
                    }
                }
            }
        }
        int count = 0;
        private void ButtonNext_Click(object sender, EventArgs e)
        {
            if (count < 0)
                count = 0;
            if (count == this.listHerbs.Items.Count)
            {
                count = this.listHerbs.Items.Count - 1;
            }
            Herbs hb = (Herbs)this.listHerbs.Items[count];
            this.textBox2.Text = hb.Name;
            this.textBox3.Text = hb.Description;
            this.pictureBox1.Image = ConvertBytetoImage(hb.Photo);
            count++;
        }
        private void ListHerbs_MouseClick(object sender, MouseEventArgs e)
        {
            Herbs hb = (Herbs)this.listHerbs.SelectedItem;
            this.textBox2.Text = hb.Name;
            this.textBox3.Text = hb.Description;
            this.pictureBox1.Image = ConvertBytetoImage(hb.Photo);
        }
        private void ButtonPrev_Click(object sender, EventArgs e)
        {
            if (count < 0)
            {
                count = 0;
            }
            if (count == this.listHerbs.Items.Count)
            {
                count = this.listHerbs.Items.Count - 1;
            }
            Herbs hb = (Herbs)this.listHerbs.Items[count];
            this.textBox2.Text = hb.Name;
            this.textBox3.Text = hb.Description;
            this.pictureBox1.Image = ConvertBytetoImage(hb.Photo);
            count--;
        }
    }
}