using Npgsql;
using System.Configuration;
using System.Windows;
using System.Windows.Input;
using VegetableFruitApp.Command;

namespace Homework_academyTOP_cs
{
    public partial class MainWindow : Window
    {
        public ICommand ConnectCommandButton { get; }
        public ICommand DisconnectCommandButton { get; }

        private NpgsqlConnection connection;

        public MainWindow()
        {
            InitializeComponent();

            ConnectCommandButton = new DelegateCommand(ConnectToDatabase, (_) => true);
            DisconnectCommandButton = new DelegateCommand(DisconnectFromDatabase, (_) => true);

            DataContext = this;
        }

        private void ConnectToDatabase(object obj)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString;

            if (connection == null || connection.State != System.Data.ConnectionState.Open)
            {
                try
                {
                    using var connection = new NpgsqlConnection(connectionString);
                    connection.Open();

                    MessageBox.Show("Успешное подключение к базе Vegetable and Fruit");
                }
                catch (NpgsqlException ex)
                {
                    MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Уже существует активное подключение.");
            }
        }

        private void DisconnectFromDatabase(object obj)
        {
            if (connection != null && connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
                MessageBox.Show("ОК! Успешно откллючились от базы Vegetable and Fruit");
            }
            else
            {
                MessageBox.Show("Нет активного подключения для отключения.");
            }
        }

    }
}