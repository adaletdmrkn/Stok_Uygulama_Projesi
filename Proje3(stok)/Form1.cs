using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Proje3_stok_
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection baglan = new SqlConnection("data source=DESKTOP-G81T251\\SQLEXPRESS;Initial Catalog=urunstok;Integrated Security=true");
        //sql arasındaki bağlantıyı kuruyor.
        private void verilerigörüntüle()
        {
            listView1.Items.Clear();
            baglan.Open();  //bağlantıyı aç
            SqlCommand komut = new SqlCommand("select * FROM [urunstok].[dbo].[View_urun] order by id DESC", baglan);   //"urun" isimle tabloya bağlanmasını sağlıyor.    
            SqlDataReader oku = komut.ExecuteReader();  //oku komutu atanıyor.
            while (oku.Read()) //okuma döngüsü başlatılıyor.
            {
                ListViewItem ekle = new ListViewItem();
                ekle.Text = oku["id"].ToString();
                ekle.SubItems.Add(oku["Ad"].ToString());                 //ürünleri okuması için yazılmış kod
                ekle.SubItems.Add(oku["Marka"].ToString());
                ekle.SubItems.Add(oku["urunTuru"].ToString());
                ekle.SubItems.Add(oku["Renk"].ToString());
                ekle.SubItems.Add(oku["Adet"].ToString());
                ekle.SubItems.Add(oku["isActive"].ToString());
                listView1.Items.Add(ekle);
            }
            baglan.Close();  //bağlantıyı kapat
        }
        private void btnGörüntüle_Click(object sender, EventArgs e)
        {
            verilerigörüntüle();  //görüntüle butonuna basıldığında sqle girilen ürünleri görüntülüyor.
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            baglan.Open();
            SqlCommand komut = new SqlCommand("insert into urun(Ad,Marka,TurId,RenkId,Adet,isActive) values ('" + txtAd.Text.ToString() + "','" + txtMarka.Text.ToString() + "','" + cbUrunTur.SelectedValue.ToString() + "','" + cbRenk.SelectedValue.ToString() + "','" + txtAdet.Text.ToString() + "',1)", baglan);
            komut.ExecuteNonQuery();
            baglan.Close();
            verilerigörüntüle();
        }
        private void cbTurDoldur()
        {
            string connectionString = "data source=DESKTOP-G81T251\\SQLEXPRESS;Initial Catalog=urunstok;Integrated Security=true"; // Update with your connection string
            string query = "SELECT * FROM renk"; // Your SQL query

            // Create a DataTable to hold the data
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Create a SqlDataAdapter
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);

                try
                {
                    // Open the connection
                    connection.Open();

                    // Fill the DataTable
                    dataAdapter.Fill(dataTable);

                    // You can now work with your DataTable
                    foreach (DataRow row in dataTable.Rows)
                    {
                        Console.WriteLine(string.Join(", ", row.ItemArray));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

            cbRenk.DataSource = dataTable;
            cbRenk.ValueMember = dataTable.Columns["id"].ToString();
            cbRenk.DisplayMember = dataTable.Columns["Renk"].ToString();          

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'urunstokDataSet.urunTuru' table. You can move, or remove it, as needed.
            this.urunTuruTableAdapter.Fill(this.urunstokDataSet.urunTuru);
            cbTurDoldur();
        }

        int id = 0;
        private void btnSil_Click(object sender, EventArgs e)
        {
            baglan.Open();
            SqlCommand komut = new SqlCommand("delete from urun where id=(" + id + ")", baglan);
            komut.ExecuteNonQuery();
            baglan.Close();
            verilerigörüntüle();
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            id = int.Parse(listView1.SelectedItems[0].SubItems[0].Text);
            txtAd.Text = listView1.SelectedItems[0].SubItems[1].Text;
            txtMarka.Text = listView1.SelectedItems[0].SubItems[2].Text;
            cbUrunTur.Text = listView1.SelectedItems[0].SubItems[3].Text;
            cbRenk.Text = listView1.SelectedItems[0].SubItems[4].Text;
            txtAdet.Text = listView1.SelectedItems[0].SubItems[5].Text;

        }

        
    }
}
