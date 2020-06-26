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
        private string currentUser = "";
        public BusinessDetails(string bid, string currentUser)
        {
            InitializeComponent();
            this.bid = String.Copy(bid);
            this.currentUser = String.Copy(currentUser);
            loadBusinessDetails();
            loadBusinessNumbers();
            addColumnReview();
            loadReviews();
            ratings();
        }

        private string buildConnectionString()
        {
            return "Host = localhost; Username = postgres; Database = milestone3db; password= ' '";
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
            col4.Width = 550;
            reviewGrid.Columns.Add(col4);

            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Binding = new Binding("date");
            col5.Header = "Date";
            col5.Width = 75;
            dataGrid.Columns.Add(col5);

            DataGridTextColumn col6 = new DataGridTextColumn();
            col6.Binding = new Binding("name");
            col6.Header = "Name";
            col6.Width = 75;
            dataGrid.Columns.Add(col6);

            DataGridTextColumn col7 = new DataGridTextColumn();
            col7.Binding = new Binding("stars");
            col7.Header = "Stars";
            col7.Width = 50;
            dataGrid.Columns.Add(col7);

            DataGridTextColumn col8 = new DataGridTextColumn();
            col8.Binding = new Binding("text");
            col8.Header = "Text";
            col8.Width = 550;
            dataGrid.Columns.Add(col8);
        }

        private void addReview(NpgsqlDataReader R)
        {
            reviewGrid.Items.Add(new Review() { date = R.GetValue(0).ToString(), name = R.GetString(1), stars = R.GetFloat(2), text = R.GetString(3) });
        }

        private void addFriends(NpgsqlDataReader R)
        {
            dataGrid.Items.Add(new Review() { date = R.GetValue(0).ToString(), name = R.GetString(1), stars = R.GetFloat(2), text = R.GetString(3) });
        }

        private void loadReviews()
        {
            reviewGrid.Items.Clear();
            dataGrid.Items.Clear();

            string sqlStr = "SELECT reviewdate, firstname, stars, reviewtext FROM review, users WHERE users.userid=review.userid and review.businessid= '" + this.bid + "';";
            executeQuery(sqlStr, addReview);

            if (this.currentUser != "")
            {
                string sqlStr2 = "SELECT reviewdate, firstname, stars, reviewtext FROM review natural join users WHERE userid in (SELECT friendid FROM friends WHERE userid = '" + this.currentUser + "')  and review.businessid= '" + this.bid + "';";
                executeQuery(sqlStr2, addFriends);
            }
        }

        private void ratings()
        {
            comboBox.Items.Add('1');
            comboBox.Items.Add('2');
            comboBox.Items.Add('3');
            comboBox.Items.Add('4');
            comboBox.Items.Add('5');
        }

        private string generateReviewId()
        {
            var possibleChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ-_abcdefghijklmnopqrstuvwxyz";
            var reviewId = new char[22];
            var random = new Random();

            for (int i = 0; i < 22; i++)
            {
                reviewId[i] = possibleChars[random.Next(possibleChars.Length)];
            }

            return new string(reviewId);
        }

        //private string getValidReviewId()
        //{
        //    string testString = generateReviewId();
        //    bool valid = false;

        //    while (valid==false)
        //    {
        //        string sqlStr = "SELECT revewid FROM Review WHERE reviewid='" + testString + "';";
        //        executeQuery(sqlStr, );
        //    }

        //    return testString;
        //}

        private void insert(NpgsqlDataReader R)
        {
            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (this.currentUser != "")
            {
                if (comboBox.SelectedIndex > -1)
                {
                    string nReviewId = generateReviewId();
                    DateTime todaysDate = DateTime.Now;
                    string sqlDate = todaysDate.ToString("yyyy-MM-dd HH:mm:ss.fff");

                    string sqlStr = "INSERT INTO review VALUES ('" + nReviewId + "', '" + sqlDate +"', '" + textBox.Text.ToString() + "', " + comboBox.SelectedItem.ToString() + ", '" + this.currentUser + "', '" + this.bid + "');";
                    executeQuery(sqlStr, insert);

                    loadReviews();
                }
            }
        }
    }

}