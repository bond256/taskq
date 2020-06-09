using EO.WebBrowser;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace WindowsFormsApp12
{
    public partial class Form1 : Form
    {

        private string[] speed_wind;
        private string[] direct_wind;
        private string[] date_name;
        public Form1()
        {
            InitializeComponent();
            webControl1.WebView.Url = "https://www.windguru.cz/87721";
            System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer();
            timer1.Interval = 7200000;
            timer1.Tick += new EventHandler(cheked);

            // Enable timer.  
            timer1.Enabled = true;

        }


        private void set_time()
        {
            System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer();
            timer1.Interval = 7200000;
            timer1.Tick += new EventHandler(cheked);

            // Enable timer.  
            timer1.Enabled = true;
        }





        private void button1_Click(object sender, EventArgs e)
        {
            init();
        }


        private void init()
        {
            string[] directs = null;
            string[] mas = new string[200];

            var hrt = webControl1.WebView.GetHtml();
            var document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(hrt);

            HtmlNodeCollection WINDSPD = document.DocumentNode.SelectNodes(".//tr[@id='tabid_0_0_WINDSPD']");
            HtmlNodeCollection dates = document.DocumentNode.SelectNodes(".//tr[@id='tabid_0_0_dates']");
            HtmlNodeCollection direct = document.DocumentNode.SelectNodes(".//tr[@id='tabid_0_0_SMER']");
            if (WINDSPD != null && dates != null && direct != null)
            {
                string[] result_speed = getResult(WINDSPD);
                string[] result_data = getResult(dates);

                foreach (var lo in direct)
                {
                    directs = search(lo.InnerHtml);

                }

                direct_wind = directs;
                speed_wind = result_speed;
                date_name = result_data;
                input_table();
                prepere_sent();
            }

            else
            {
                MessageBox.Show("Error, data");
            }

        }


        public string[] search(string str)
        {
            String[] result_vec = str.Split(new char[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
            double[] initVectorDouble = new double[result_vec.Length];
            string[] rets = new string[100];
            for (int i = 1, j = 0; i < result_vec.Length; i += 6, j++)
            {
                rets[j] = result_vec[i];
            }
            return rets;
        }


        private string[] getResult(HtmlNodeCollection input)
        {
            string[] result_mas = null;
            foreach (HtmlNode link in input)
            {
                var tred = link.ChildNodes;
                int k = 0;
                result_mas = new string[tred.Count];
                foreach (var res in tred)
                {

                    if (k < tred.Count)
                    {
                        result_mas[k] = res.InnerText;
                        k++;
                    }

                }

            }
            return result_mas;
        }



        private void input_table()
        {
            string[] tab = { "wind speed", "derection" };
            DataTable dt = new DataTable();
            dt.Columns.Add("-");
            int count = 0;
            int i = 0;
            int row = date_name.GetUpperBound(0) + 1;
            int size = date_name.Length;

            for (count = 0; count < size; count++)
            {
                dt.Columns.Add(date_name[count]);
            }

            for (i = 0; i < 2; i++)
            {
                DataRow r = dt.NewRow();
                r["-"] = tab[i];
                dt.Rows.Add(r);
            }


            dataGridView1.DataSource = dt;



            for (int count_data_j = 1; count_data_j < speed_wind.Length + 1; count_data_j++)
            {
                dataGridView1.Rows[0].Cells[count_data_j].Value = speed_wind[count_data_j - 1];
            }

            for (int count_data_j = 1; count_data_j < speed_wind.Length + 1; count_data_j++)
            {
                dataGridView1.Rows[1].Cells[count_data_j].Value = direct_wind[count_data_j - 1];
            }

        }


        private void cheked(object Sender, EventArgs e)
        {
            init();
        }


        private string[] getMas()
        {
            string[] result_date_mas = new string[date_name.Length];
            string ret_result = "";
            for (int i = 0; i < date_name.Length; i++)
            {
                string[] str_res = date_name[i].Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                string temp = str_res[0];
                ret_result = "";
                for (int k = 0; k < temp.Length; k++)
                {
                    Boolean gth = char.IsDigit(temp[k]);
                    if (char.IsDigit(temp[k]))
                        ret_result += temp[k];

                }
                result_date_mas[i] = ret_result;
            }
            return result_date_mas;

        }



        private void setValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("hello");
            Sittings sit = new Sittings();
            sit.Show();

        }

        private string[] getDrWind()
        {
            string res_dir = "";
            string[] mas_wind_data = new string[direct_wind.Length];
            for(int i=0; i<mas_wind_data.Length; i++)
            {
                res_dir = "";
                if (direct_wind[i] != null)
                {
                    for (int j = 0; j < direct_wind[i].Length - 1; j++)
                    {
                        res_dir += direct_wind[i][j];
                    }
                    mas_wind_data[i] = res_dir;
                }

            }
            return mas_wind_data;
        }

        private void prepere_sent()
        {
            string[] taday = getMas();
            string[] direct_whothout_sin = getDrWind();
            int count = 0;
            Boolean che = false;
            string grt = Properties.Settings.Default.win_sp2;
            for (int i = 0; i < getMas().Length; i++)
            {
                if (taday[0] == taday[i])
                {
                    count++;
                }
            }

            for (int n = 0; n < count; n++)
            {
               if(Convert.ToInt32(speed_wind[n]) <= Convert.ToInt32(Properties.Settings.Default.win_sp2) && Convert.ToInt32(speed_wind[n]) >= Convert.ToInt32(Properties.Settings.Default.win_sp1) &&  Convert.ToInt32(direct_whothout_sin[n]) >= Convert.ToInt32(Properties.Settings.Default.dir_sp1) && Convert.ToInt32(direct_whothout_sin[n]) <= Convert.ToInt32(Properties.Settings.Default.dir_sp2))
                {
                    che=true;
                }
            }

            if (che == true)
            {
                sentEmail();
            }
            
        }

        private void sentEmail()
        {
            
                MailAddress from = new MailAddress("jack.coldon@gmail.com", "weather");

                MailAddress to = new MailAddress("dmitriy.bondarenko.2017@gmail.com", "dima");

                MailMessage message = new MailMessage(from, to);
                message.Subject = "Status Weather";
           
            message.Body = "Good wheather";
                

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);

                smtp.Credentials = new NetworkCredential("jack.coldon@gmail.com", "ghost54Now");
                smtp.EnableSsl = true;
                smtp.Send(message);
            
        }
    }
}
