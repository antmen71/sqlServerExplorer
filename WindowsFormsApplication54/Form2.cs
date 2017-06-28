using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApplication54
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.ResetText();
            comboBox2.Items.Clear();
            SqlConnectionStringBuilder cs = new SqlConnectionStringBuilder();
            cs[("server")] = ".";
            cs[("database")] = comboBox1.SelectedItem;
            cs[("trusted_connection")] = true;
            SqlConnection cnn = new SqlConnection(cs.ToString());

            SqlCommand cmd = new SqlCommand("SELECT * FROM sysobjects where xtype='u'", cnn);

            cnn.Open();
            SqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                comboBox2.Items.Add(rd[0]);
            }

            cnn.Close();


        }
        ListView lstvw = new ListView();




        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {

            lstvw.Columns.Clear();
            lstvw.Items.Clear();
            SqlConnectionStringBuilder cs = new SqlConnectionStringBuilder();
            cs[("server")] = ".";
            cs[("database")] = comboBox1.Text;
            cs[("trusted_connection")] = true;
            SqlConnection cnn = new SqlConnection(cs.ToString());

            SqlCommand cmd = new SqlCommand("SELECT COUNT(*)  FROM syscolumns JOIN sysobjects ON sysobjects.id = syscolumns.id WHERE sysobjects.xtype='U' AND sysobjects.name =@tblname", cnn);

            SqlCommand cmd1 = new SqlCommand("SELECT * FROM syscolumns JOIN sysobjects ON sysobjects.id = syscolumns.id WHERE sysobjects.xtype='U' AND sysobjects.name =@tblname  ", cnn);
            
            cnn.Open();
            cmd.Parameters.AddWithValue("@tblname", comboBox2.SelectedItem);
            cmd1.Parameters.AddWithValue("@tblname", comboBox2.SelectedItem);
            
            Int32 count = (Int32)cmd.ExecuteScalar();
            SqlDataReader rd = cmd1.ExecuteReader();

            lstvw.View = View.Details;
            lstvw.FullRowSelect = true;
            lstvw.GridLines = true;
            lstvw.Left = 12;
            lstvw.Top = 40;
            lstvw.Width = this.Width - 40;
            lstvw.Height = this.Height - 100;
            lstvw.BringToFront();
            lstvw.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);
            lstvw.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            this.Controls.Add(lstvw);

            while (rd.Read())
            {
                lstvw.Columns.Add(rd[0].ToString());
            }

            rd.Close();

            string tabloIsmi = comboBox2.SelectedItem.ToString();
            if (tabloIsmi.Contains(" "))
            {
                tabloIsmi = tabloIsmi.PadLeft(tabloIsmi.Length + 1, '[');
                tabloIsmi = tabloIsmi.PadRight(tabloIsmi.Length + 1, ']');
            }
            string queryString = "SELECT * FROM " + tabloIsmi;
            SqlCommand cmd2 = new SqlCommand(queryString, cnn);


            SqlDataReader rd1 = cmd2.ExecuteReader();

            while (rd1.Read())
            {
                ListViewItem itm = new ListViewItem();
                itm.Text = rd1[0].ToString();
                for (int k = 1; k < count; k++)
                {
                    itm.SubItems.Add(rd1[k].ToString());

                }
                lstvw.Items.Add(itm);

            }

            cnn.Close();

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            SqlConnection cnn = new SqlConnection("server=.;database=master;trusted_connection=yes");
            SqlCommand cmd = new SqlCommand("select name from sysdatabases", cnn);

            cnn.Open();

            SqlDataReader rd = cmd.ExecuteReader();

            while (rd.Read())
            {
                comboBox1.Items.Add(rd[0]);
            }
            cnn.Close();

        }
    }
}
