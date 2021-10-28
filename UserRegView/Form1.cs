using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Runtime.InteropServices;


namespace UserRegView
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        #region OleDbConnection database connection string 
        OleDbConnection dbConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=accessdb.accdb");
        #endregion 

        private void pullData()
        {
            listView1.Items.Clear();
            dbConnection.Open();
            OleDbCommand dbCommand = new OleDbCommand("select * from Uyelik", dbConnection);
            OleDbDataReader dataReader = dbCommand.ExecuteReader();
            while (dataReader.Read())
            {
                ListViewItem viewItem = new ListViewItem();
                viewItem.Text = dataReader["uyeId"].ToString();
                viewItem.SubItems.Add(dataReader["ad"].ToString());
                viewItem.SubItems.Add(dataReader["soyad"].ToString());
                viewItem.SubItems.Add(dataReader["eposta"].ToString());
                viewItem.SubItems.Add(dataReader["sifre"].ToString());
                viewItem.SubItems.Add(dataReader["yas"].ToString());
                viewItem.SubItems.Add(dataReader["sozlesme"].ToString());
                listView1.Items.Add(viewItem);
            }
            dbConnection.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            #region textbox padding tasarımı/textbox padding style
            SetPadding(textBox1, new Padding(10, 10, 5, 5));
            SetPadding(textBox2, new Padding(10, 10, 5, 5));
            SetPadding(textBox3, new Padding(10, 10, 5, 5));
            SetPadding(textBox4, new Padding(10, 10, 5, 5));
            SetPadding(textBox5, new Padding(10, 10, 5, 5));
            #endregion
        }
    
        #region textbox padding ayar kodları/textbox padding setting codes
        private const int EM_SETRECT = 0xB3;

        [DllImport(@"User32.dll", EntryPoint = @"SendMessage", CharSet = CharSet.Auto)]
        private static extern int SendMessageRefRect(IntPtr hWnd, uint msg, int wParam, ref RECT rect);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public readonly int Left;
            public readonly int Top;
            public readonly int Right;
            public readonly int Bottom;

            private RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public RECT(Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom)
            {
            }
        }
        public void SetPadding(TextBox textBox, Padding padding)
        {
            var rect = new Rectangle(padding.Left, padding.Top, textBox.ClientSize.Width - padding.Left - padding.Right, textBox.ClientSize.Height - padding.Top - padding.Bottom);
            RECT rc = new RECT(rect);
            SendMessageRefRect(textBox.Handle, EM_SETRECT, 0, ref rc);
        }
        #endregion


        
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                //18-120 yaş arası kayıt yapabilir/ 18-120 age between can reg
                if (Convert.ToInt32(textBox3.Text) < 18)
                {
                    MessageBox.Show("Kayıt olmanız için yaşınızın 18 veya üzeri olması gereklidir.", "Kayıt başarısız!");
                    textBox3.Text = "";
                    textBox3.Focus();

                }

                else if (Convert.ToInt32(textBox3.Text) > 120)
                {
                    MessageBox.Show("Yaşın çok be abi", ":p", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox3.Text = "";
                    textBox3.Focus();
                }

                //sorun yoksa kaydeder/save if there is no problem 
                else
                {
                    dbConnection.Open();
                    OleDbCommand command = new OleDbCommand("insert into Uyelik (ad,soyad,eposta,sifre,yas,sozlesme) values ('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox4.Text + "','" + textBox5.Text + "','" + textBox3.Text + "','" + (int)checkBox1.CheckState + "')", dbConnection);
                    command.ExecuteNonQuery();
                    dbConnection.Close();
                    MessageBox.Show("Kayıt işlemi başarıyla gerçekleştirildi.", "İşlem Başarılı!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            catch
            {
                //boş bırakılırsa hata vermesi gerekli/ if pass empty, it should throw an error
                MessageBox.Show("Lütfen boş kısım bırakmayınız!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }


        #region textbox tasarım kısmı/ textbox design side

        #region yaş için sadece numerik karakterin girilebilmesi/should enter only numeric character for age
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
       (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
        #endregion



        #region eğer üzerine tıklarsa yazı kaybolacak ve color=black olacak, eğer ayrılırsa yazı geri gelecek ve color=gray olacak/if click textbox, text will disappear&color=black, if leave from textbox text the text will come back&color=gray 
        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Adınız")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Adınız";
                textBox1.ForeColor = Color.Gray;
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "Soyadınız")
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.Black;
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = "Soyadınız";
                textBox2.ForeColor = Color.Gray;
            }
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            if (textBox4.Text == "E-Posta Adresiniz")
            {
                textBox4.Text = "";
                textBox4.ForeColor = Color.Black;
            }
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            if (textBox4.Text == "")
            {
                textBox4.Text = "E-Posta Adresiniz";
                textBox4.ForeColor = Color.Gray;
            }
        }

        private void textBox5_Enter(object sender, EventArgs e)
        {
            if (textBox5.Text == "Şifreniz")
            {
                textBox5.Text = "";
                textBox5.ForeColor = Color.Black;
                textBox5.PasswordChar = '*';
            }
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            if (textBox5.Text == "")
            {
                textBox5.PasswordChar = char.Parse("\0");
                textBox5.Text = "Şifreniz";
                textBox5.ForeColor = Color.Gray;

            }
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            if (textBox3.Text == "Yaşınız")
            {
                textBox3.Text = "";
                textBox3.ForeColor = Color.Black;
            }
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                textBox3.Text = "Yaşınız";
                textBox3.ForeColor = Color.Gray;
            }
        }
        #endregion


        #endregion



        private void button1_Click(object sender, EventArgs e)
        {
            pullData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

    }
}
