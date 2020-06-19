using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Npgsql;

namespace milestone1
{
    /// <summary>
    /// Interaction logic for BusinessDetails.xaml
    /// </summary>
    public partial class BusinessDetails : Window
    {
        private string bid = "";
        public BusinessDetails(string bid)
        {
            InitializeComponent();
            this.bid = String.Copy(bid);
            loadBusinessDetails();
            loadBusinessNumbers();
            addColumnReview();
            loadReviews();
        }

        private string buildConnectionString()
        {
            return "Host = localhost; Username = postgres; Database = milstone2DB; password= 123@";
        }

        public class Review
        {
            public string date { get; set; }
            public string name { get; set; }
            public float stars { get; set; }
            public string text { get; set; }

        }

        private void executeQuery(string sqlstr, Action<NpgsqlDataReader> myf)
        {
            using (var conncetion = new NpgsqlConnection(buildConnectionString()))
            {
                conncetion.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conncetion;
                    cmd.CommandText = sqlstr;
                    try
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                            myf(reader);
                    }
                    catch (NpgsqlException ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                        System.Windows.MessageBox.Show("SQL Error - " + ex.Message.ToString());
                    }
                    finally
                    {
                        conncetion.Close();


                    }
                }
            }
        }
        private void setBusinessDetails(NpgsqlDataReader R)
        {
            bname.Text = R.GetString(0);
            state.Text = R.GetString(1);
            city.Text = R.GetString(2);

        }
        void setNumInState(NpgsqlDataReader R)
        {
            numInState.Content = R.GetInt16(0).ToString();

        }

        void setNumInCity(NpgsqlDataReader R)
        {
            numInCity.Content = R.GetInt16(0).ToString();

        }

        private void loadBusinessNumbers()
        {
            string sqlStr1 = "SELECT count(*) FROM business WHERE state = (SELECT state FROM business WHERE businessid = '" + this.bid + "');";
            executeQuery(sqlStr1, setNumInState);
            string sqlStr2 = "SELECT count(*) FROM business WHERE city = (SELECT city FROM business WHERE businessid = '" + this.bid + "');";
            executeQuery(sqlStr2, setNumInCity);

        }
        private void loadBusinessDetails()
        {
            string sqlStr = "SELECT name, state, city FROM business WHERE businessid = '" + this.bid + "';";
            executeQuery(sqlStr, setBusinessDetails);
        }

        private void addColumnReview()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Binding = new Binding("date");
            col1.Header = "Date";
            col1.Width = 75;
            reviewGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Binding = new Binding("name");
            col2.Header = "Name";
            col2.Width = 75;
            reviewGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Binding = new Binding("stars");
            col3.Header = "Stars";
            col3.Width = 50;
            reviewGrid.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Binding = new Binding("text");
            col4.Header = "Text";
            col4.Width = 500;
            reviewGrid.Columns.Add(col4);
        }

        private void addReview(NpgsqlDataReader R)
        {
            reviewGrid.Items.Add(new Review() { date = R.GetValue(0).ToString(), name = R.GetString(1), stars = R.GetFloat(2), text = R.GetString(3) });
        }

        private void loadReviews()
        {
            string sqlStr = "SELECT reviewdate, firstname, stars, reviewtext FROM review, users WHERE users.userid=review.userid and review.businessid= '" + this.bid + "';";
            executeQuery(sqlStr, addReview);
        }
    }

}