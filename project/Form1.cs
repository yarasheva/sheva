using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace project
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();            
        }
        string grouping = "";
        
        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            grouping = "";
            if (checkBox1.Checked) grouping += "Дата,";
            if (checkBox2.Checked) grouping += "Организация,";
            if (checkBox3.Checked) grouping += "Город,";
            if (checkBox4.Checked) grouping += "Страна,";
            if (checkBox5.Checked) grouping += "Менеджер,";
                try
                {
                    using (SqlConnection cn = new SqlConnection())
                    {
                       // cn.ConnectionString = @"Data Source=ЯРИК-ПК\MSSQLSERVER1;Initial Catalog=ty;User ID=sa;Password=yaroslavshevchenko18";
                        string Nameofserver = ConfigurationSettings.AppSettings["Nameofserver"];
                        string Nameofinstance = ConfigurationSettings.AppSettings["Nameofinstance"];
                        string Nameofdb = ConfigurationSettings.AppSettings["Nameofdb"];
                        string login = ConfigurationSettings.AppSettings["login"];
                        string Password = ConfigurationSettings.AppSettings["Password"];                        
                        cn.ConnectionString = string.Format("Data Source={0}\\{1};Initial Catalog={2};User ID={3};Password={4}",
                            Nameofserver, Nameofinstance, Nameofdb, login, Password);
                        cn.Open();
                        string sqlQuery;
                        SqlCommand sqlCmd;
                        sqlCmd = new SqlCommand();
                        if (grouping != "")
                        {
                            sqlQuery = "DECLARE @sqlCommand varchar(1000) SET @sqlCommand = 'SELECT ' + @columnList + ' FROM Store  group by '+@grouping EXEC (@sqlCommand)";
                            sqlCmd = new SqlCommand(sqlQuery, cn);
                            sqlCmd.Parameters.Add("@grouping", SqlDbType.VarChar).Value = grouping.Substring(0, grouping.Length - 1);
                            //MessageBox.Show(grouping.Substring(0,grouping.Length-1));
                            sqlCmd.Parameters.Add("@columnList", SqlDbType.VarChar).Value = grouping + "Количиство=Sum(Количиство),Сумма=Sum(Сумма)";
                        }
                        else { sqlQuery = "select * from Store"; sqlCmd = new SqlCommand(sqlQuery, cn); }
                        SqlDataReader reader = sqlCmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);
                            dataGridView1.DataSource = dt;
                        }
                        Console.ReadLine();
                        cn.Close();
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button1_Click(sender,e);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }
               
            


    }
}
