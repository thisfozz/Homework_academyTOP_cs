using Npgsql;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
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
            var productTableName = "Produce";
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString;
            string query = "SELECT * FROM Produce";
            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            var adapter = new NpgsqlDataAdapter(query, connection);
            var dataSet = new DataSet();
            adapter.Fill(dataSet, productTableName);

            var dataTable = dataSet.Tables[productTableName];
            string result = "Name\tType\tColor\tCalories\n";

            foreach (DataRow row in dataTable.Rows)
            {
                result += $"{row["name"]}\t{row["type"]}\t{row["color"]}\t{row["calories"]}\n";
            }

            MessageBox.Show(result, "Produce Table");
        }

        private void ShowAllNames(object obj)
        {
            var productTableName = "Produce";
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString;
            string query = "SELECT name FROM Produce";

            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            var adapter = new NpgsqlDataAdapter(query, connection);
            var dataSet = new DataSet();
            adapter.Fill(dataSet, productTableName);

            var dataTable = dataSet.Tables[productTableName];

            string result = "Names\n";

            foreach (DataRow row in dataTable.Rows)
            {
                result += $"{row["name"]}\n";
            }

            MessageBox.Show(result, "Produce Names");
        }

        private void ShowAllColors(object obj)
        {
            var productTableName = "Produce";
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString;
            string query = "SELECT color FROM Produce";

            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            var adapter = new NpgsqlDataAdapter(query, connection);
            var dataSet = new DataSet();
            adapter.Fill(dataSet, productTableName);

            var dataTable = dataSet.Tables[productTableName];

            string result = "Color\n";

            foreach (DataRow row in dataTable.Rows)
            {
                result += $"{row["color"]}\n";
            }

            MessageBox.Show(result, "Produce Names");
        }
        private void ShowMaxCalories(object obj)
        {
            var productTableName = "Produce";
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString;
            string query = "SELECT MAX(calories) AS MaxCalories FROM Produce";

            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            var adapter = new NpgsqlDataAdapter(query, connection);
            var dataSet = new DataSet();
            adapter.Fill(dataSet, productTableName);

            var dataTable = dataSet.Tables[productTableName];

            int maxCalories = Convert.ToInt32(dataTable.Rows[0]["MaxCalories"]);

            MessageBox.Show($"Максимальная калорийность: {maxCalories}", "Max Calories");
        }
        private void ShowMinCalories(object obj)
        {
            var productTableName = "Produce";
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString;
            string query = "SELECT MIN(calories) AS MinCalories FROM Produce";

            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            var adapter = new NpgsqlDataAdapter(query, connection);
            var dataSet = new DataSet();
            adapter.Fill(dataSet, productTableName);

            var dataTable = dataSet.Tables[productTableName];

            int minCalories = Convert.ToInt32(dataTable.Rows[0]["MinCalories"]);

            MessageBox.Show($"Минимальная колорийность калорийность: {minCalories}", "Mix Calories");
        }
        private void ShowAvgCalories(object obj)
        {
            var productTableName = "Produce";
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString;
            string query = "SELECT AVG(calories) AS AvgCalories FROM Produce";

            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            var adapter = new NpgsqlDataAdapter(query, connection);
            var dataSet = new DataSet();
            adapter.Fill(dataSet, productTableName);

            var dataTable = dataSet.Tables[productTableName];

            int avgCalories = Convert.ToInt32(dataTable.Rows[0]["AvgCalories"]);

            MessageBox.Show($"Средняя колорийность: {avgCalories}", "Avg Calories");
        }

        private void ShowAllVegetablesCount(object obj)
        {
            var productTableName = "Produce";
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString;
            string query = "SELECT COUNT(*) AS VegetablesCount FROM Produce WHERE type = 'Овощ'";

            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            var adapter = new NpgsqlDataAdapter(query, connection);
            var dataSet = new DataSet();
            adapter.Fill(dataSet, productTableName);

            var dataTable = dataSet.Tables[productTableName];

            int vegetablesCount = Convert.ToInt32(dataTable.Rows[0]["VegetablesCount"]);
            MessageBox.Show($"Количество овощей: {vegetablesCount}", "Vegetables Count");
        }

        private void ShowAllFruitsCount(object obj)
        {
            var productTableName = "Produce";
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString;
            string query = "SELECT COUNT(*) AS FruitsCount FROM Produce WHERE type = 'Фрукт'";

            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            var adapter = new NpgsqlDataAdapter(query, connection);
            var dataSet = new DataSet();
            adapter.Fill(dataSet, productTableName);

            var dataTable = dataSet.Tables[productTableName];

            int fruitsCount = Convert.ToInt32(dataTable.Rows[0]["FruitsCount"]);
            MessageBox.Show($"Количество фруктов: {fruitsCount}", "Fruits Count");
        }

        private void ShowItemsByColor(object obj)
        {
            string color = Microsoft.VisualBasic.Interaction.InputBox("Введите цвет:", "Ввод цвета", "");

            if (!string.IsNullOrEmpty(color))
            {
                var productTableName = "Produce";
                string connectionString = ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString;
                string query = "SELECT COUNT(*) AS ProduceCount FROM Produce WHERE color = @color";

                using var connection = new NpgsqlConnection(connectionString);
                connection.Open();
    
                using var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@color", color);

                using var adapter = new NpgsqlDataAdapter(command);
                var dataSet = new DataSet();
                adapter.Fill(dataSet, productTableName);

                var dataTable = dataSet.Tables[productTableName];

                int produceCount = Convert.ToInt32(dataTable.Rows[0]["ProduceCount"]);

                MessageBox.Show($"Количество овощей и фруктов цвета {color}: {produceCount}", "Produce Count by Color");
            }

        }

        private void ShowItemCountByColor(object obj)
        {
            var productTableName = "Produce";
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString;
            string query = "SELECT color, type, COUNT(*) AS ProduceCount FROM Produce GROUP BY color, type";

            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            var adapter = new NpgsqlDataAdapter(query, connection);
            var dataSet = new DataSet();
            adapter.Fill(dataSet, productTableName);

            var dataTable = dataSet.Tables[productTableName];

            string result = "Color\tType\tProduceCount\n";

            foreach (DataRow row in dataTable.Rows)
            {
                result += $"{row["color"]}\t{row["type"]}\t{row["ProduceCount"]}\n";
            }

            MessageBox.Show(result, "Produce Count by Color and Type");
        }

        private void ShowItemsBelowCalories(object obj)
        {
            string calories = Microsoft.VisualBasic.Interaction.InputBox("Введите количество калорий:", "Количество калорий", "");

            if (!string.IsNullOrEmpty(calories))
            {
                int caloriesINT = int.Parse(calories);
                var productTableName = "Produce";
                string connectionString = ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString;
                string query = "SELECT name FROM Produce WHERE calories < @calories";

                using var connection = new NpgsqlConnection(connectionString);
                connection.Open();

                using var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@calories", caloriesINT);

                using var adapter = new NpgsqlDataAdapter(command);
                var dataSet = new DataSet();
                adapter.Fill(dataSet, productTableName);

                var dataTable = dataSet.Tables[productTableName];

                string result = "Name\n";

                foreach (DataRow row in dataTable.Rows)
                {
                    result += $"{row["name"]}\n";
                }

                MessageBox.Show(result, "Produce Names Below Calories");
            }
        }

        private void ShowItemsAboveCalories(object obj)
        {
            string calories = Microsoft.VisualBasic.Interaction.InputBox("Введите количество калорий:", "Количество калорий", "");

            if (!string.IsNullOrEmpty(calories))
            {
                int caloriesINT = int.Parse(calories);
                var productTableName = "Produce";
                string connectionString = ConfigurationManager.ConnectionStrings["ConnectionStr"].ConnectionString;
                string query = "SELECT name FROM Produce WHERE calories > @calories";

                using var connection = new NpgsqlConnection(connectionString);
                connection.Open();

                using var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@calories", caloriesINT);

                using var adapter = new NpgsqlDataAdapter(command);
                var dataSet = new DataSet();
                adapter.Fill(dataSet, productTableName);

                var dataTable = dataSet.Tables[productTableName];

                string result = "Name\n";

                foreach (DataRow row in dataTable.Rows)
                {
                    result += $"{row["name"]}\n";
                }

                MessageBox.Show(result, "Produce Names Below Calories");
            }
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