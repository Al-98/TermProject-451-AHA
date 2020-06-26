using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;

namespace milestone1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string MsqlStr;
        public static string MpsqlStr;

        public class Business
        {
            public string bid { get; set; }
            public string name { get; set; }
            public string state { get; set; }
            public string city { get; set; }
            public string Zipcode { get; set; }
            public string avgrating { get; set; }
            public string numofreviews { get; set; }
            public string numofcheckins { get; set; }
        }

        public class Categories
        {
            // uncommet out below to run code
            //public bool select { get; set; }
            public string category { get; set; }
        }
        private class Selection
        {
            public string selected { get; set; }
        }
        public MainWindow()
        {
            InitializeComponent();
            addState();
            addColumns2Grid();
            addZipcode();
            addSortBy();
        }
        private string buildConnectionString()
        {
            return "Host = localhost; Username = postgres; Database = yelpdb; password= 'AHA'";
        }
        private void addSortBy()
        {
            //name(defult),Highest rated, most reviewed, best review rating, most checkins, nearest
            comboBox.Items.Add("name(defult)");
            comboBox.Items.Add("Highest rated");
            comboBox.Items.Add("most reviewed");
            // comboBox.Items.Add("best review rating");
            comboBox.Items.Add("most checkins");
            // comboBox.Items.Add("nearest");

        }
        private void addState()
        {
            using ( var connection = new NpgsqlConnection(buildConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {

                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT distinct state FROM business ORDER BY state";
                    try
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                            stateList.Items.Add(reader.GetString(0));

                    }
                    catch (NpgsqlException ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                        System.Windows.MessageBox.Show("SQL Error - " + ex.Message.ToString());


                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        
        private void addZipcode()
        {

            using (var connection = new NpgsqlConnection(buildConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {

                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT distinct Zipcode FROM business ORDER BY Zipcode";
                    try
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                            ZipcodeList.Items.Add(reader.GetString(0));

                    }
                    catch (NpgsqlException ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                        System.Windows.MessageBox.Show("SQL Error - " + ex.Message.ToString());


                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

        }
        private void addColumns2Grid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Binding = new Binding("name");
            col1.Header = "BusinessName";
            col1.Width = 255;
            businessGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Binding = new Binding("state");
            col2.Header = "State";
            col2.Width = 60;
            businessGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Binding = new Binding("city");
            col3.Header = "City";
            col3.Width = 150;
            businessGrid.Columns.Add(col3);

            DataGridTextColumn col5 = new DataGridTextColumn();
            col5.Binding = new Binding("category");
            col5.Header = "category";
            col5.Width = 150;
            categoryGrid.Columns.Add(col5);

            DataGridTextColumn col6 = new DataGridTextColumn();
            col6.Binding = new Binding("selected");
            col6.Header = "Selected";
            col6.Width = 150;
            dataGrid.Columns.Add(col6);

            DataGridTextColumn col7 = new DataGridTextColumn();
            col7.Binding = new Binding("category");
            col7.Header = "category";
            col7.Width = 150;
            dataGrid1.Columns.Add(col7);

            DataGridTextColumn col8 = new DataGridTextColumn();
            col8.Binding = new Binding("avgrating");
            col8.Header = "avgrating";
            col8.Width = 100;
            businessGrid.Columns.Add(col8);

            DataGridTextColumn col19 = new DataGridTextColumn();
            col19.Binding = new Binding("numofreviews");
            col19.Header = "numofreviews";
            col19.Width = 50;
            businessGrid.Columns.Add(col19);

            DataGridTextColumn col20 = new DataGridTextColumn();
            col20.Binding = new Binding("numofcheckins");
            col20.Header = "numofcheckins";
            col20.Width = 50;
            businessGrid.Columns.Add(col20);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Binding = new Binding("bid");
            col4.Header = "";
            col4.Width = 0;
            businessGrid.Columns.Add(col4);

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

        private void addCity(NpgsqlDataReader R)
        {
            cityList.Items.Add(R.GetString(0));
        }

        private void stateList_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            cityList.Items.Clear();
            if (stateList.SelectedIndex > -1)
            {
                string sqlStr = "SELECT distinct city FROM business WHERE state = '" + stateList.SelectedItem.ToString() + "' ORDER BY city";
                executeQuery(sqlStr, addCity);
            }

        }

        private void DoseNothing(NpgsqlDataReader R)
        {

        }
        private void addGridRow(NpgsqlDataReader R)
        {
            businessGrid.Items.Add(new Business() { name = R.GetString(0), state = R.GetString(1), city = R.GetString(2), avgrating = R.GetValue(4).ToString(), numofreviews = R.GetInt64(5).ToString(), numofcheckins = R.GetInt64(6).ToString(), bid = R.GetString(3) });
        }
        private void addZipcode(NpgsqlDataReader R)
        {
            ZipcodeList.Items.Add(R.GetString(0));

        }
        private void addCatRow(NpgsqlDataReader R)
        {
            categoryGrid.Items.Add(new Categories() { category = R.GetString(0) });

        }

        private void addCatBuisRow(NpgsqlDataReader R)
        {
            dataGrid1.Items.Add(new Categories() { category = R.GetString(0) });

        }
        private void addSelectRow(Categories C)
        {
            dataGrid.Items.Add(new Selection() { selected=C.category });
        }

        private void cityList_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ZipcodeList.Items.Clear();
            if (cityList.SelectedIndex > -1)
            {
                string sqlStr = "SELECT distinct Zipcode FROM business WHERE state = '" + stateList.SelectedItem.ToString() + "' AND city = '" + cityList.SelectedItem.ToString() + "' ORDER by Zipcode;";
                executeQuery(sqlStr, addZipcode);
            }
        }


        private void businessGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (businessGrid.SelectedIndex > -1)
            {
                dataGrid1.Items.Clear();
                string currentUser = "";   //"---1lKK3aKOuomHnwAkAow";
                if (listUserIds.SelectedIndex > -1)
                {
                    currentUser = listUserIds.SelectedItem.ToString();
                }

                Business B = businessGrid.Items[businessGrid.SelectedIndex] as Business;
                if ((B.bid != null) && (B.bid.ToString().CompareTo("") != 0))
                {
                    //BusinessDetails businesssWindow = new BusinessDetails(B.bid.ToString(), currentUser);
                    //businesssWindow.Show();
                    string TsqlStr = "SELECT DISTINCT category from iscat as c0, business as b0 where c0.businessid='" + B.bid.ToString() + "' and c0.businessid=b0.businessid ;";
                    executeQuery(TsqlStr, addCatBuisRow);
                    Console.WriteLine(B.bid.ToString());

                    DayOfWeek day = DateTime.Now.DayOfWeek;
                    string theday = day.ToString();
                    //need day of the week
                    textBox12.Text = "";
                    string sqlStr = "SELECT name, address FROM business where businessid='" + B.bid.ToString() + "'";
                    string sqlStr2 = "SELECT name, address, opentime,closetime,weekday from (" + sqlStr + ") as z0 , dayhours as z4 where z4.businessid='" + B.bid.ToString() + "' and weekday='" + theday + "' ;";
                    sqlStr = sqlStr + ";";
                    executeQuery(sqlStr, setpartialBusinessDetails);
                    Console.WriteLine(sqlStr2);
                    executeQuery(sqlStr2, setBusinessDetails);
                }

            }

        }

       


        private void ZipcodeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            businessGrid.Items.Clear();
            categoryGrid.Items.Clear();
            dataGrid1.Items.Clear();


            if (ZipcodeList.SelectedIndex > -1)
            {
                    MpsqlStr = "SELECT name, state, city, businessid, avgreview,numofreviews, numofcheckins FROM business WHERE state = '" + stateList.SelectedItem.ToString() + "' AND city = '" + cityList.SelectedItem.ToString() + "' AND zipcode='" + ZipcodeList.SelectedItem.ToString() + "'";
                string sqlStr1 = MpsqlStr + "ORDER BY name;";
                executeQuery(sqlStr1, addGridRow);

               string sqlStr2 = "SELECT distinct category FROM iscat WHERE  iscat.businessid in (SELECT businessid from business WHERE state = '" + stateList.SelectedItem.ToString() + "' AND city = '" + cityList.SelectedItem.ToString() + "'AND Zipcode = '" + ZipcodeList.SelectedItem.ToString() + "');"; 
               executeQuery(sqlStr2, addCatRow);
                loadNumofbusi(MpsqlStr);
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (categoryGrid.SelectedIndex > -1)
            {
                Categories C = categoryGrid.Items[categoryGrid.SelectedIndex] as Categories;

                addSelectRow(C);
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
           
            if (dataGrid.Items.Count > 0)
            {
                businessGrid.Items.Clear();
                dataGrid1.Items.Clear();

                Selection S = dataGrid.Items[0] as Selection;
                MsqlStr = "SELECT * FROM " + "(SELECT name, state, city, business.businessid,avgreview,numofreviews, numofcheckins FROM business, iscat WHERE iscat.businessid=business.businessid AND state= '" + stateList.SelectedItem.ToString() + "' AND city = '" + cityList.SelectedItem.ToString() + "' AND Zipcode = '" + ZipcodeList.SelectedItem.ToString() + "' AND iscat.category= '" + S.selected.ToString() + "') as A0 ";
                for (int i = 1; i < dataGrid.Items.Count; i++)
                {
                    S = dataGrid.Items[i] as Selection;
                    MsqlStr = MsqlStr + " NATURAL JOIN (SELECT name, state, city, business.businessid,avgreview,numofreviews,numofcheckins FROM business, iscat WHERE iscat.businessid=business.businessid AND state= '" + stateList.SelectedItem.ToString() + "' AND city = '" + cityList.SelectedItem.ToString() + "' AND Zipcode = '" + ZipcodeList.SelectedItem.ToString() + "' AND iscat.category= '" + S.selected.ToString() + "') as A" + i.ToString() + " ";
                }
                string sqlStr = MsqlStr + "Order BY name;";

                executeQuery(sqlStr, addGridRow);
                loadNumofbusi(MsqlStr);

            }
            else
            {

                businessGrid.Items.Clear();
                categoryGrid.Items.Clear();
                dataGrid1.Items.Clear();


                if (ZipcodeList.SelectedIndex > -1)
                {
                    //might need to change back to Mpsql
                    //string sqlStr = "SELECT name, state, city, businessid FROM business WHERE state = '" + stateList.SelectedItem.ToString() + "' AND city = '" + cityList.SelectedItem.ToString() + "' ORDER by name; ";
                    //executeQuery(sqlStr, addGridRow);
                    MpsqlStr = "SELECT name, state, city, businessid, avgreview,numofreviews, numofcheckins FROM business WHERE state = '" + stateList.SelectedItem.ToString() + "' AND city = '" + cityList.SelectedItem.ToString() + "' AND zipcode='" + ZipcodeList.SelectedItem.ToString() + "'";
                    string sqlStr1 = MpsqlStr + "ORDER BY name;";
                    executeQuery(sqlStr1, addGridRow);

                    string sqlStr2 = "SELECT distinct category FROM iscat WHERE  iscat.businessid in (SELECT businessid from business WHERE state = '" + stateList.SelectedItem.ToString() + "' AND city = '" + cityList.SelectedItem.ToString() + "'AND Zipcode = '" + ZipcodeList.SelectedItem.ToString() + "');";
                    executeQuery(sqlStr2, addCatRow);
                    loadNumofbusi(MsqlStr);

                }
            }
        }

        private void remove_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedIndex > -1)
            {
                Categories C = categoryGrid.Items[categoryGrid.SelectedIndex] as Categories;

                dataGrid.Items.Remove(dataGrid.Items[dataGrid.SelectedIndex]);
            }

            if (dataGrid.Items!=null) {
                businessGrid.Items.Clear();
                string sqlStr3 = MsqlStr + "Order BY name;";

                executeQuery(sqlStr3, addGridRow);
            }
        }

        private void typeName_TextChanged(object sender, TextChangedEventArgs e)
        {
            listUserIds.Items.Clear();
            if (typeName.Text != "")
            {
                string sqlStr = "select userid from users where firstname like '" + typeName.Text + "%';";
                executeQuery(sqlStr, addUserIds);
            }
        }

        private void addUserIds(NpgsqlDataReader R)
        {
            listUserIds.Items.Add(R.GetString(0));
        }

        private void listUserIds_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FriendGrid.Items.Clear();
            FavoriteGrid.Items.Clear();
            dataGrid.Items.Clear();
            StarsText.Clear();
            FansText.Clear();
            NameText.Clear();
            YelSinceText.Clear();
            LatText.Clear();

            if (listUserIds.SelectedIndex>-1)
            {
                string info = "select firstname, avgstars, joindate, lat, long from users where userid= '" + listUserIds.SelectedItem.ToString() + "';";
            }
        }

        private void addFavorite(NpgsqlDataReader R)
        {

        }

        private void button22_Click(object sender, RoutedEventArgs e)
        {
            string currentUser = "";   //"---1lKK3aKOuomHnwAkAow";
            if (listUserIds.SelectedIndex > -1)
            {
                if (businessGrid.SelectedIndex > -1)
                {
                    Business B = businessGrid.Items[businessGrid.SelectedIndex] as Business;
                    currentUser = listUserIds.SelectedItem.ToString();

                    string SqlStr = "Insert Into favorite Values ('" + currentUser + "', '" + B.bid.ToString() +"');";
                    executeQuery(SqlStr, addFavorite);
                } 
            }
        }

        private void button21_Click(object sender, RoutedEventArgs e)
        {
            if (businessGrid.SelectedIndex > -1)
            {
                Business B = businessGrid.Items[businessGrid.SelectedIndex] as Business;
                if ((B.bid != null) && (B.bid.ToString().CompareTo("") != 0))
                {
                    BusinessDetails businesssWindow = new BusinessDetails(B.bid.ToString(), "---1lKK3aKOuomHnwAkAow");
                    businesssWindow.Show();
                }
            }
        }

        private void loadNumofbusi(string MsqlStr)
        {
            string sqlStr = "SELECT count(*) FROM(" + MsqlStr + ") as z0;";
            Console.WriteLine(sqlStr);
            executeQuery(sqlStr, setNumofbusi);
        }

        private void setNumofbusi(NpgsqlDataReader R)
        {
            numofbusi.Text = R.GetInt64(0).ToString();
        }

        private void setpartialBusinessDetails(NpgsqlDataReader R)
        {
            textBox1.Text = R.GetString(0);
            textBox11.Text = R.GetString(1);

        }
        private void setBusinessDetails(NpgsqlDataReader R)
        {
            textBox1.Text = R.GetString(0);
            textBox11.Text = R.GetString(1);
            textBox12.Text = R.GetValue(2).ToString() + " to " + R.GetValue(3).ToString() + " on " + R.GetValue(4).ToString();

        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            businessGrid.Items.Clear();

            string[] filterby = { "name", "avgreview DESC", "numofreviews DESC", "numofcheckins DESC" };
            if (comboBox.SelectedIndex > -1)
            {

                string TsqlStr = MpsqlStr + "ORDER BY " + filterby[comboBox.SelectedIndex] + ";";
                Console.WriteLine(TsqlStr);
                executeQuery(TsqlStr, addGridRow);
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (businessGrid.SelectedIndex > -1)
            {
                DayOfWeek day = DateTime.Now.DayOfWeek;
                string theday = day.ToString();
                int time = DateTime.Now.Hour;
                Console.WriteLine(time.ToString());
                string thetime = time.ToString() + ":00";
                
                Business B = businessGrid.Items[businessGrid.SelectedIndex] as Business;
                string sqlStr = "UPDATE checkin SET numofcheckins= (SELECT numofcheckins FROM checkin WHERE checkin.businessid='" + B.bid.ToString() + "' AND checkin.checkintime='" + thetime + "' AND checkin.weekday='" + theday + "')+1 WHERE businessid='" + B.bid.ToString() + "' AND checkintime='" + thetime + "' AND weekday='" + theday + "';";
                executeQuery(sqlStr, DoseNothing);

                string sqlStr2 = "insert into checkin values ('" + B.bid.ToString() + "' , '" + theday + "','" + thetime + "',1);";
                executeQuery(sqlStr2, DoseNothing);

                if(MpsqlStr != null)
                {
                    businessGrid.Items.Clear();
                    if (MsqlStr != null)
                    {
                        string sqlStr3 = MsqlStr + "Order BY name;";

                        executeQuery(sqlStr3, addGridRow);

                    }
                    else
                    {
                        //MpsqlStr = "SELECT name, state, city, businessid, avgreview,numofreviews, numofcheckins FROM business WHERE state = '" + stateList.SelectedItem.ToString() + "' AND city = '" + cityList.SelectedItem.ToString() + "' AND zipcode='" + ZipcodeList.SelectedItem.ToString() + "'";
                        string sqlStr4 = MpsqlStr + "ORDER BY name;";
                        executeQuery(sqlStr4, addGridRow);
                    }
                   
                }
            }
        }
    }
}
