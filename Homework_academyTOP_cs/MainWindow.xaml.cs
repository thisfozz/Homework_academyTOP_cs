using Npgsql;
using System.Configuration;
using System.Windows;
using System.Windows.Input;
using VegetableFruitApp.Command;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Homework_academyTOP_cs
{
    public partial class MainWindow : Window
    {
        public ICommand ConnectCommandButton { get; }
        public ICommand DisconnectCommandButton { get; }
        public ICommand ShowAllDataCommand { get; }
        public ICommand ShowAllNamesCommand { get; }
        public ICommand ShowAllColorsCommand { get; }
        public ICommand ShowMaxCaloriesCommand { get; }
        public ICommand ShowMinCaloriesCommand { get; }
        public ICommand ShowAvgCaloriesCommand { get; }
        //------------------------------------------------------------------------
        public ICommand ShowAllVegetablesCountCommand { get; }
        public ICommand ShowAllFruitsCountCommand { get; }
        public ICommand ShowItemsByColorCommand { get; }
        public ICommand ShowItemCountByColorCommand { get; }
        public ICommand ShowItemsBelowCaloriesCommand { get; }
        public ICommand ShowItemsAboveCaloriesCommand { get; }
        public ICommand ShowItemsInCaloriesRangeCommand { get; }
        public ICommand ShowItemsByColorYellowRedCommand { get; }

        private NpgsqlConnection connection;

        public MainWindow()
        {
            InitializeComponent();

            ConnectCommandButton = new DelegateCommand(ConnectToDatabase, (_) => true);
            DisconnectCommandButton = new DelegateCommand(DisconnectFromDatabase, (_) => true);

            ShowAllDataCommand = new DelegateCommand(ShowAllData, (_) => true);
            ShowAllNamesCommand = new DelegateCommand(ShowAllNames, (_) => true);
            ShowAllColorsCommand = new DelegateCommand(ShowAllColors, (_) => true);
            ShowMaxCaloriesCommand = new DelegateCommand(ShowMaxCalories, (_) => true);
            ShowMinCaloriesCommand = new DelegateCommand(ShowMinCalories, (_) => true);
            ShowAvgCaloriesCommand = new DelegateCommand(ShowAvgCalories, (_) => true);

            ShowAllVegetablesCountCommand = new DelegateCommand(ShowAllVegetablesCount, (_) => true);
            ShowAllFruitsCountCommand = new DelegateCommand(ShowAllFruitsCount, (_) => true);
            ShowItemsByColorCommand = new DelegateCommand(ShowItemsByColor, (_) => true);
            ShowItemCountByColorCommand = new DelegateCommand(ShowItemCountByColor, (_) => true);
            ShowItemsBelowCaloriesCommand = new DelegateCommand(ShowItemsBelowCalories, (_) => true);
            ShowItemsAboveCaloriesCommand = new DelegateCommand(ShowItemsAboveCalories, (_) => true);
            ShowItemsInCaloriesRangeCommand = new DelegateCommand(ShowItemsInCaloriesRange, (_) => true);
            ShowItemsByColorYellowRedCommand = new DelegateCommand(ShowItemsByColorYellowRed, (_) => true);

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

        private void ShowAllData(object obj)
        {
            string query = "SELECT * FROM Produce";
        }

        private void ShowAllNames(object obj)
        {
            string query = "SELECT name FROM Produce";
        }

        private void ShowAllColors(object obj)
        {
            string query = "SELECT color FROM Produce";
        }

        private void ShowMaxCalories(object obj)
        {
            string query = "SELECT calories FROM Produce ORDER BY calories DESC LIMIT 1";
        }

        private void ShowMinCalories(object obj)
        {
            string query = "SELECT calories FROM Produce ORDER BY calories ASC LIMIT 1";
        }

        private void ShowAvgCalories(object obj)
        {
            string query = "SELECT AVG(calories) FROM Produce";
        }



        private void ShowAllVegetablesCount(object obj)
        {
            string query = "SELECT COUNT(*) FROM Produce WHERE type = 'Овощ'";
        }

        private void ShowAllFruitsCount(object obj)
        {
            string query = "SELECT COUNT(*) FROM Produce WHERE type = 'Фрукт'";
        }

        private void ShowItemsByColor(object obj)
        {
            //string inputColor = Ввод_пользователя;
            string query = "SELECT COUNT(*) FROM Produce WHERE type = 'Овощ' AND color = @color;";
            //command.Parameters.AddWithValue("@color", inputColor);
        }

        private void ShowItemCountByColor(object obj)
        {
            //string query = "SELECT COUNT(*) FROM Produce WHERE type = 'Овощ'" AND type = 'Фрукт';
        }

        private void ShowItemsBelowCalories(object obj)
        {
            //int inputColories = Ввод_пользователя;
            string query = "SELECT COUNT(*) FROM Produce WHERE colories < @colories;";
            //command.Parameters.AddWithValue("@colories", inputColories);
        }

        private void ShowItemsAboveCalories(object obj)
        {
            //int inputColories = Ввод_пользователя;
            string query = "SELECT COUNT(*) FROM Produce WHERE colories > @colories;";
            //command.Parameters.AddWithValue("@colories", inputColories);
        }

        private void ShowItemsInCaloriesRange(object obj)
        {
            //int minCalories = Ввод_пользователя;
            //int maxCalories = Ввод_пользователя50;
            string query = "SELECT * FROM Produce WHERE calories BETWEEN @minCalories AND @maxCalories";
            //command.Parameters.AddWithValue("@minCalories", minCalories);
            //command.Parameters.AddWithValue("@maxCalories", maxCalories);
        }

        private void ShowItemsByColorYellowRed(object obj)
        {
            //string query = "SELECT COUNT(*) FROM Produce WHERE color = 'Желтный'" AND color = 'Красный';
        }
    }
}