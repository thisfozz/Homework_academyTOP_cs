using Npgsql;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Input;
using VegetableFruitApp.Command;

namespace Homework_academyTOP_cs
{
    public partial class MainWindow : Window
    {
        public ICommand ConnectCommandButton { get; } // Подключение к базе данных
        public ICommand DisconnectCommandButton { get; } // Отключение от базы данных
        public ICommand ShowAllDataCommand { get; } // Отображение всей информации из таблицы с овощами и фруктами
        public ICommand ShowAllNamesCommand { get; } // Отображение всех названий овощей и фруктов
        public ICommand ShowAllColorsCommand { get; } // Отображение всех цветов
        public ICommand ShowMaxCaloriesCommand { get; } // ■ Показать максимальную калорийность
        public ICommand ShowMinCaloriesCommand { get; } // Показать минимальную калорийность
        public ICommand ShowAvgCaloriesCommand { get; } // Показать среднюю калорийность
        public ICommand ShowAllVegetablesCountCommand { get; } // Показать количество овощей
        public ICommand ShowAllFruitsCountCommand { get; } // Показать количество фруктов
        public ICommand ShowItemsByColorCommand { get; } //  Показать количество овощей и фруктов заданного цвета
        public ICommand ShowItemCountByColorCommand { get; } // Показать количество овощей фруктов каждого цвета
        public ICommand ShowItemsBelowCaloriesCommand { get; } // Показать овощи и фрукты с калорийностью ниже указанной
        public ICommand ShowItemsAboveCaloriesCommand { get; } //  Показать овощи и фрукты с калорийностью выше указанной
        public ICommand ShowItemsInCaloriesRangeCommand { get; } // Показать овощи и фрукты с калорийностью в указанном диапазоне
        public ICommand ShowItemsByColorYellowRedCommand { get; } // Показать все овощи и фрукты, у которых цвет желтый или красный

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
                    connection = new NpgsqlConnection(connectionString);
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

        private void ExecuteIfConnectionOpen(Action action)
        {
            if (connection != null && connection.State == ConnectionState.Open)
            {
                action.Invoke();
            }
            else
            {
                MessageBox.Show("Нет подключения к базе данных");
            }
        }

        private int GetCaloriesInput()
        {
            string calories = Microsoft.VisualBasic.Interaction.InputBox("Введите количество калорий:", "Количество калорий", "");

            if (!string.IsNullOrEmpty(calories) && int.TryParse(calories, out int caloriesINT))
            {
                return caloriesINT;
            }

            return -1;
        }

        private void ShowAllData(object obj)
        {
            ExecuteIfConnectionOpen(() =>
            {
                string query = "SELECT * FROM Produce";
                using var command = new NpgsqlCommand(query, connection);
                using var reader = command.ExecuteReader();

                string result = "Name\tType\tColor\tCalories\n";

                while (reader.Read())
                {
                    result += $"{reader["name"]}\t{reader["type"]}\t{reader["color"]}\t{reader["calories"]}\n";
                }
                MessageBox.Show(result, "Produce Table");
            });
        }
        private void ShowAllNames(object obj)
        {
            ExecuteIfConnectionOpen(() =>
            {
                string query = "SELECT name FROM Produce";
                using var command = new NpgsqlCommand(query, connection);
                using var reader = command.ExecuteReader();

                string result = "Names\n";

                while (reader.Read())
                {
                    result += $"{reader["name"]}\n";
                }

                MessageBox.Show(result, "Название продуктов");
            });
        }
        private void ShowAllColors(object obj)
        {
            ExecuteIfConnectionOpen(() =>
            {
                string query = "SELECT color FROM Produce";
                using var command = new NpgsqlCommand(query, connection);
                using var reader = command.ExecuteReader();

                string result = "Color\n";

                while (reader.Read())
                {
                    result += $"{reader["color"]}\n";
                }

                MessageBox.Show(result, "Цвета продуктов");
            });
        }
        private void ShowMaxCalories(object obj)
        {
            ExecuteIfConnectionOpen(() =>
            {
                string query = "SELECT MAX(calories) AS MaxCalories FROM Produce";
                using var command = new NpgsqlCommand(query, connection);
                using var reader = command.ExecuteReader();

                int maxCalories = 0;

                if (reader.Read())
                {
                    maxCalories = Convert.ToInt32(reader["MaxCalories"]);
                }

                MessageBox.Show($"Максимальная калорийность: {maxCalories}", "Максимальная калорийность");
            });
        }
        private void ShowMinCalories(object obj)
        {
            ExecuteIfConnectionOpen(() =>
            {
                string query = "SELECT MIN(calories) AS MinCalories FROM Produce";
                using var command = new NpgsqlCommand(query, connection);
                using var reader = command.ExecuteReader();

                int minCalories = 0;

                if (reader.Read())
                {
                    minCalories = Convert.ToInt32(reader["MinCalories"]);
                }

                MessageBox.Show($"Минимальная калорийность: {minCalories}", "Минимальная калорийность");
            });
        }
        private void ShowAvgCalories(object obj)
        {
            ExecuteIfConnectionOpen(() =>
            {
                string query = "SELECT AVG(calories) AS AvgCalories FROM Produce";
                using var command = new NpgsqlCommand(query, connection);
                using var reader = command.ExecuteReader();

                int avgCalories = 0;

                if (reader.Read())
                {
                    avgCalories = Convert.ToInt32(reader["AvgCalories"]);
                }

                MessageBox.Show($"Средняя калорийность: {avgCalories}", "Средняя калорийность");
            });
        }
        private void ShowAllVegetablesCount(object obj)
        {
            ExecuteIfConnectionOpen(() =>
            {
                string query = "SELECT COUNT(*) AS VegetablesCount FROM Produce WHERE type = 'Овощ'";
                using var command = new NpgsqlCommand(query, connection);
                using var reader = command.ExecuteReader();

                int vegetablesCount = 0;

                if (reader.Read())
                {
                    vegetablesCount = Convert.ToInt32(reader["VegetablesCount"]);
                }

                MessageBox.Show($"Количество овощей: {vegetablesCount}", "Количество овощей");
            });
        }
        private void ShowAllFruitsCount(object obj)
        {
            ExecuteIfConnectionOpen(() =>
            {
                string query = "SELECT COUNT(*) AS FruitsCount FROM Produce WHERE type = 'Фрукт'";
                using var command = new NpgsqlCommand(query, connection);
                using var reader = command.ExecuteReader();

                int fruitsCount = 0;

                if (reader.Read())
                {
                    fruitsCount = Convert.ToInt32(reader["FruitsCount"]);
                }

                MessageBox.Show($"Количество фруктов: {fruitsCount}", "Количество фруктов");
            });
        }
        private void ShowItemsByColor(object obj)
        {
            ExecuteIfConnectionOpen(() =>
            {
                string color = Microsoft.VisualBasic.Interaction.InputBox("Введите цвет:", "Ввод цвета", "");

                if (!string.IsNullOrEmpty(color))
                {
                    string query = "SELECT COUNT(*) AS ProduceCount FROM Produce WHERE color = @color";

                    using var command = new NpgsqlCommand(query, connection);
                    command.Parameters.AddWithValue("@color", color);
                    using var reader = command.ExecuteReader();

                    int produceCount = 0;

                    if (reader.Read())
                    {
                        produceCount = Convert.ToInt32(reader["ProduceCount"]);
                    }

                    MessageBox.Show($"Количество овощей и фруктов цвета {color}: {produceCount}", "Количество овощей и фруктов");
                }
            });
        }
        private void ShowItemCountByColor(object obj)
        {
            ExecuteIfConnectionOpen(() =>
            {
                string query = "SELECT color, type, COUNT(*) AS ProduceCount FROM Produce GROUP BY color, type";
                using var command = new NpgsqlCommand(query, connection);
                using var reader = command.ExecuteReader();

                string result = "Color\tType\tProduceCount\n";

                while (reader.Read())
                {
                    result += $"{reader["color"]}\t{reader["type"]}\t{reader["ProduceCount"]}\n";
                }

                MessageBox.Show(result, "Количество продуктов по цвету и типу");
            });
        }
        private void ShowItemsBelowCalories(object obj)
        {
            ExecuteIfConnectionOpen(() =>
            {
                int calories = GetCaloriesInput();
                if (calories >= 0)
                {
                    string query = "SELECT name FROM Produce WHERE calories < @calories";

                    using var command = new NpgsqlCommand(query, connection);
                    command.Parameters.AddWithValue("@calories", calories);

                    using var reader = command.ExecuteReader();

                    string result = "Name\n";

                    while (reader.Read())
                    {
                        result += $"{reader["name"]}\n";
                    }

                    MessageBox.Show(result, "Produce Names Below Calories");
                }
            });
        }

        private void ShowItemsAboveCalories(object obj)
        {
            ExecuteIfConnectionOpen(() =>
            {
                int calories = GetCaloriesInput();
                if (calories >= 0)
                {
                    string query = "SELECT name FROM Produce WHERE calories > @calories";

                    using var command = new NpgsqlCommand(query, connection);
                    command.Parameters.AddWithValue("@calories", calories);

                    using var reader = command.ExecuteReader();

                    string result = "Name\n";

                    while (reader.Read())
                    {
                        result += $"{reader["name"]}\n";
                    }

                    MessageBox.Show(result, "Produce Names Above Calories");
                }
            });
        }

        private void ShowItemsInCaloriesRange(object obj)
        {
            ExecuteIfConnectionOpen(() =>
            {
                string caloriesBegin = Microsoft.VisualBasic.Interaction.InputBox("Введите начало диапазона:", "Начало диапазона", "");
                string caloriesEnd = Microsoft.VisualBasic.Interaction.InputBox("Введите конец диапазона:", "Конец диапазона", "");

                if (!string.IsNullOrEmpty(caloriesBegin) && !string.IsNullOrEmpty(caloriesEnd))
                {
                    if (int.TryParse(caloriesBegin, out int caloriesBeginINT) && (int.TryParse(caloriesEnd, out int caloriesEndINT)))
                    {
                        string query = "SELECT * FROM Produce WHERE calories BETWEEN @minCalories AND @maxCalories";

                        using var command = new NpgsqlCommand(query, connection);
                        command.Parameters.AddWithValue("@minCalories", caloriesBeginINT);
                        command.Parameters.AddWithValue("@maxCalories", caloriesEndINT);

                        using var reader = command.ExecuteReader();

                        string result = "Name:\n";

                        while (reader.Read())
                        {
                            result += $"{reader["name"]}\n";
                        }

                        MessageBox.Show(result, "Продукты по калорийности");
                    }
                }
            });
        }

        private void ShowItemsByColorYellowRed(object obj)
        {
            ExecuteIfConnectionOpen(() =>
            {
                string query = "SELECT name FROM Produce WHERE color = 'Желтый' OR color = 'Красный'";

                using var command = new NpgsqlCommand(query, connection);
                using var reader = command.ExecuteReader();

                string result = "Name:\n";

                while (reader.Read())
                {
                    result += $"{reader["name"]}\n";
                }

                MessageBox.Show(result, "Produce Names Color - Yellow OR Red");
            });
        }
    }
}