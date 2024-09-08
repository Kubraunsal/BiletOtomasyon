using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BiletOtomasyonu2
{
    public partial class Form1 : Form
    {
        string connectionString = "Data Source=LAPTOP-VBT65R4T;Initial Catalog=BiletOtomasyonuu;Integrated Security=True";
        private List<Button> koltuklar = new List<Button>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void cmbOtobüs_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbOtobüs.Text)
            {
                case "Travego":
                    KoltukDoldur(8, false);
                    break;
                case "Setra":
                    KoltukDoldur(12, true);
                    break;
                case "Neoplan":
                    KoltukDoldur(10, false);
                    break;
            }
        }

        private void KoltukDoldur(int sira, bool arkaBesliMi)
        {
            // Mevcut koltukları kaldır
            foreach (Button koltuk in koltuklar)
            {
                this.Controls.Remove(koltuk);
            }
            koltuklar.Clear();

            int koltukNo = 1;
            for (int i = 0; i < sira; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (arkaBesliMi == true)
                    {
                        if (i != sira - 1 && j == 2)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (j == 2)
                            continue;
                    }

                    Button koltuk = new Button();
                    koltuk.Height = koltuk.Width = 40;
                    koltuk.Top = 30 + (i * 45);
                    koltuk.Left = 5 + (j * 45);
                    koltuk.Text = koltukNo.ToString();
                    koltukNo++;
                    koltuk.ContextMenuStrip = contextMenuStrip1;
                    koltuk.MouseDown += Koltuk_MouseDown;

                    this.Controls.Add(koltuk);
                    koltuklar.Add(koltuk);
                }
            }
        }

        Button tiklanan;

        private void Koltuk_MouseDown(object sender, MouseEventArgs e)
        {
            tiklanan = sender as Button;
        }

        private void label4_Click(object sender, EventArgs e)
        {
        }

        private void label5_Click(object sender, EventArgs e)
        {
        }

        private void rezerveEtToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (cmbOtobüs.SelectedIndex == -1 || cmbNereden.SelectedIndex == -1 || cmbNereye.SelectedIndex == -1)
            {
                MessageBox.Show("Lütfen önce gerekli alanları doldurunuz");
                return;
            }

            // Check if the seat is already reserved
            if (tiklanan.BackColor == Color.Blue || tiklanan.BackColor == Color.Red)
            {
                MessageBox.Show("Bu koltuk zaten rezerve edilmiş.");
                return;
            }

            KayıtFormu kf = new KayıtFormu();
            DialogResult sonuc = kf.ShowDialog();
            if (sonuc == DialogResult.OK)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = string.Format("{0} {1}", kf.txtİsim.Text, kf.txtSoyisim.Text);
                lvi.SubItems.Add(kf.mskdTelefon.Text);
                if (kf.rdbBay.Checked)
                {
                    lvi.SubItems.Add("BAY");
                    tiklanan.BackColor = Color.Blue;
                }

                if (kf.rdbBayan.Checked)
                {
                    lvi.SubItems.Add("BAYAN");
                    tiklanan.BackColor = Color.Red;
                }

                lvi.SubItems.Add(cmbNereden.Text);
                lvi.SubItems.Add(cmbNereye.Text);
                lvi.SubItems.Add(tiklanan.Text);
                lvi.SubItems.Add(dtpTarih.Text);
                lvi.SubItems.Add(nudFiyat.Value.ToString());
                listView1.Items.Add(lvi);

                // Disable the button after reservation
                tiklanan.Enabled = false;
            }
        }

        private void cmbNereden_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void cmbNereye_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnYazdir_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("Yazdırılacak bilet yok.");
                return;
            }

            StringBuilder sb = new StringBuilder();

            foreach (ListViewItem item in listView1.Items)
            {
                string isim = item.Text;
                string telefon = item.SubItems[1].Text;
                string cinsiyet = item.SubItems[2].Text;
                string nereden = item.SubItems[3].Text;
                string nereye = item.SubItems[4].Text;
                string koltukNo = item.SubItems[5].Text;
                string tarih = item.SubItems[6].Text;
                string fiyat = item.SubItems[7].Text;

                sb.AppendLine($"İsim: {isim}");
                sb.AppendLine($"Telefon: {telefon}");
                sb.AppendLine($"Cinsiyet: {cinsiyet}");
                sb.AppendLine($"Nereden: {nereden}");
                sb.AppendLine($"Nereye: {nereye}");
                sb.AppendLine($"Koltuk No: {koltukNo}");
                sb.AppendLine($"Tarih: {tarih}");
                sb.AppendLine($"Fiyat: {fiyat}");
                sb.AppendLine(new string('-', 20));
            }

            MessageBox.Show(sb.ToString(), "Satılmış Biletler");
        }
    }
}
