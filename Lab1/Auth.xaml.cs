using Npgsql;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace Lab1
{
    /// <summary>
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Window
    {
        private const string connectionString = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=218178artem;Database=db_Lab_DGIB;";
        public Auth()
        {
            InitializeComponent();
            //using (SHA256Managed sHA256 = new SHA256Managed())
            //    BitConverter.ToString(sHA256.ComputeHash(Encoding.UTF8.GetBytes($"{ConvertByteToString(sHA256.ComputeHash(Encoding.UTF8.GetBytes("123")))}fec5d4fc2f5cf5"))).Replace("-", string.Empty).ToLower();
        }

        private void OpenAdminWindow()
        {
            AdminWindow mainWindow = new AdminWindow(connectionString);
            mainWindow.Show();
            Dispatcher.BeginInvoke((Action)(() => Close()));
        }
        private void OpenUserWindow()
        {
            UserWindow userWindow = new UserWindow();
            userWindow.Show();
            Dispatcher.BeginInvoke((Action)(() => Close()));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            labelError.Content = string.Empty;

            var sqlQuery = $"SELECT * FROM users WHERE username='{loginBox.Text}';";

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(sqlQuery, connection))
                    {
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var password = reader.GetString(2);
                                var salt = reader.GetString(3);

                                using (SHA256Managed sHA256 = new SHA256Managed())
                                {
                                    if (password.Equals(BitConverter.ToString(sHA256.ComputeHash(Encoding.UTF8.GetBytes($"{ConvertByteToString(sHA256.ComputeHash(Encoding.UTF8.GetBytes(passwordBox.Password)))}{salt}"))).Replace("-", string.Empty).ToLower()))
                                    {
                                        if (reader.GetBoolean(4))
                                        {
                                            Dispatcher.BeginInvoke((Action)(() => OpenAdminWindow()));
                                        }
                                        else
                                        {
                                            Dispatcher.BeginInvoke((Action)(() => OpenUserWindow()));
                                        }
                                        return;
                                    }
                                }
                            }
                            labelError.Content = "Неверное имя пользователя или пароль";
                            return;
                        }
                    }
                }
            }
            catch (NpgsqlException)
            {
                labelError.Content = "Неверное имя пользователя или пароль";
                return;
            }
        }
        public static string ConvertByteToString(byte[] source) => source != null ? Encoding.UTF8.GetString(source) : null;
        private void Button_Click_1(object sender, RoutedEventArgs e) => Close();
    }
}