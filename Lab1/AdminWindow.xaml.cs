using Npgsql;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace Lab1
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private int saltLengthLimit = 7;
        private string connectionString;
        private DataTable usersTable;
        private NpgsqlConnection connection;
        private NpgsqlDataAdapter adapter;
        private NpgsqlCommandBuilder comandbuilder;
        public AdminWindow(string connection)
        {
            InitializeComponent();
            connectionString = connection;
            Dispatcher.BeginInvoke((Action)(() => FillTable()));
        }
        private void FillTable()
        {
            try
            {
                connection = new NpgsqlConnection(connectionString);
                var sqlQuery = "SELECT * FROM users;";
                using (NpgsqlCommand command = new NpgsqlCommand(sqlQuery, connection))
                {
                    adapter = new NpgsqlDataAdapter(command);
                    usersTable = new DataTable();

                    connection.Open();
                    adapter.Fill(usersTable);
                    comandbuilder = new NpgsqlCommandBuilder(adapter);
                    adapter.InsertCommand = comandbuilder.GetInsertCommand();
                    adapter.UpdateCommand = comandbuilder.GetUpdateCommand();
                    adapter.DeleteCommand = comandbuilder.GetDeleteCommand();
                    usersGrid.ItemsSource = usersTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private byte[] GetSalt()
        {
            return GetSalt(saltLengthLimit);
        }
        private byte[] GetSalt(int maximumSaltLength)
        {
            var salt = new byte[maximumSaltLength];
            using (RNGCryptoServiceProvider random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }

            return salt;
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SHA256Managed sHA256 = new SHA256Managed())
                {
                    var salt = BitConverter.ToString(GetSalt()).Replace("-", string.Empty).ToLower();
                    var password = BitConverter.ToString(sHA256.ComputeHash(Encoding.UTF8.GetBytes($"{Auth.ConvertByteToString(sHA256.ComputeHash(Encoding.UTF8.GetBytes(passwordField.Password)))}{salt}"))).Replace("-", string.Empty).ToLower();
                    var sqlQuery = $@"INSERT INTO public.users(username, password, salt, ""isAdmin"") VALUES('{usernameField.Text}', '{password}', '{salt}', {isAdmin.IsChecked});";

                    using (NpgsqlCommand command = new NpgsqlCommand(sqlQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                usernameField.Text = string.Empty;
                passwordField.Password = string.Empty;
                isAdmin.IsChecked = false;
                Dispatcher.BeginInvoke((Action)(() => FillTableAfterAdd()));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void FillTableAfterAdd()
        {
            var sqlQuery = "SELECT * FROM users;";
            using (NpgsqlCommand command = new NpgsqlCommand(sqlQuery, connection))
            {
                adapter = new NpgsqlDataAdapter(command);
                usersTable.Clear();
                adapter.Fill(usersTable);
                comandbuilder = new NpgsqlCommandBuilder(adapter);
                adapter.InsertCommand = comandbuilder.GetInsertCommand();
                adapter.UpdateCommand = comandbuilder.GetUpdateCommand();
                adapter.DeleteCommand = comandbuilder.GetDeleteCommand();
                usersGrid.ItemsSource = usersTable.DefaultView;
            }
        }
        private void UpdateDB() => adapter.Update(usersTable);

        private void UpdateButton_Click(object sender, RoutedEventArgs e) => Dispatcher.BeginInvoke((Action)(() => UpdateDB()));

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (usersGrid.SelectedItems != null)
            {
                for (int i = 0; i < usersGrid.SelectedItems.Count; i++)
                {
                    DataRowView datarowView = usersGrid.SelectedItems[i] as DataRowView;
                    if (datarowView != null)
                    {
                        DataRow dataRow = datarowView.Row;
                        dataRow.Delete();
                    }
                }
            }
            Dispatcher.BeginInvoke((Action)(() => UpdateDB()));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            connection.CloseAsync();
            new Auth().Show();
        }
    }
}
