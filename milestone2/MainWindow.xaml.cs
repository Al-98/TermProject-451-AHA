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

        public class Business
        {
            public string bid { get; set; }
            public string name { get; set; }
            public string state { get; set; }
            public string city { get; set; }
            public string Zipcode { get; set; } 
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
          
        }
        private string buildConnectionString()
        {
            return "Host = localhost; Username = postgres; Database = milestone3db; password= ' '";
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

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Binding = new Binding("bid");
            col4.Header = "";
            col4.Width = 0;
            businessGrid.Columns.Add(col4);

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


        private void addGridRow(NpgsqlDataReader R)
        {
            businessGrid.Items.Add(new Business() { name = R.GetString(0), state = R.GetString(1), city = R.GetString(2), bid = R.GetString(3) });
        }
        private void addZipcode(NpgsqlDataReader R)
        {
            ZipcodeList.Items.Add(R.GetString(0));

        }
        private void addCatRow(NpgsqlDataReader R)
        {
            categoryGrid.Items.Add(new Categories() { category = R.GetString(0) });

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
                string currentUser = "";   //"---1lKK3aKOuomHnwAkAow";
                if (listUserIds.SelectedIndex > -1)
                {
                    currentUser = listUserIds.SelectedItem.ToString();
                }

                Business B = businessGrid.Items[businessGrid.SelectedIndex] as Business;
                if ((B.bid != null) && (B.bid.ToString().CompareTo("") != 0))
                {
                    BusinessDetails businesssWindow = new BusinessDetails(B.bid.ToString(), currentUser);
                    businesssWindow.Show();
                }

            }

        }

       


        private void ZipcodeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            businessGrid.Items.Clear();
            categoryGrid.Items.Clear();
            

            if (ZipcodeList.SelectedIndex > -1)
            {
               string sqlStr = "SELECT name, state, city, businessid FROM business WHERE state = '" + stateList.SelectedItem.ToString() + "' AND city = '" + cityList.SelectedItem.ToString() + "' ORDER by name; ";
               executeQuery(sqlStr, addGridRow);

               string sqlStr2 = "SELECT distinct category FROM iscat WHERE  iscat.businessid in (SELECT businessid from business WHERE state = '" + stateList.SelectedItem.ToString() + "' AND city = '" + cityList.SelectedItem.ToString() + "'AND Zipcode = '" + ZipcodeList.SelectedItem.ToString() + "');"; 
               executeQuery(sqlStr2, addCatRow);

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

                Selection S = dataGrid.Items[0] as Selection;
                string sqlStr = "SELECT * FROM " + "(SELECT name, state, city, business.businessid FROM business, iscat WHERE iscat.businessid=business.businessid AND state= '" + stateList.SelectedItem.ToString() + "' AND city = '" + cityList.SelectedItem.ToString() + "' AND Zipcode = '" + ZipcodeList.SelectedItem.ToString() + "' AND iscat.category= '" + S.selected.ToString() + "') as A0 ";
                for (int i = 1; i < dataGrid.Items.Count; i++)
                {
                    S = dataGrid.Items[i] as Selection;
                    sqlStr = sqlStr + " NATURAL JOIN (SELECT name, state, city, business.businessid FROM business, iscat WHERE iscat.businessid=business.businessid AND state= '" + stateList.SelectedItem.ToString() + "' AND city = '" + cityList.SelectedItem.ToString() + "' AND Zipcode = '" + ZipcodeList.SelectedItem.ToString() + "' AND iscat.category= '" + S.selected.ToString() + "') as A" + i.ToString() + " ";
                }
                sqlStr = sqlStr + ";";

                executeQuery(sqlStr, addGridRow);
            }
            else
            {

                businessGrid.Items.Clear();
                categoryGrid.Items.Clear();


                if (ZipcodeList.SelectedIndex > -1)
                {
                    string sqlStr = "SELECT name, state, city, businessid FROM business WHERE state = '" + stateList.SelectedItem.ToString() + "' AND city = '" + cityList.SelectedItem.ToString() + "' ORDER by name; ";
                    executeQuery(sqlStr, addGridRow);

                    string sqlStr2 = "SELECT distinct category FROM iscat WHERE  iscat.businessid in (SELECT businessid from business WHERE state = '" + stateList.SelectedItem.ToString() + "' AND city = '" + cityList.SelectedItem.ToString() + "'AND Zipcode = '" + ZipcodeList.SelectedItem.ToString() + "');";
                    executeQuery(sqlStr2, addCatRow);

                }
            }
        }

        private void remove_Click(object sender, RoutedEventArgs e)
        {
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
    }
}
