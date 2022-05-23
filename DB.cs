using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using Npgsql;

namespace ARMDoctor
{
    class DB
    {
        private static readonly string str = @"Server = localhost; " +
            "Port = 5432; " + 
            "User ID = postgres; " +
            "Password = postgres; " +
            "Database = armdoctor; " +
            "Persist Security Info = True";
        ///NpgsqlConnection connection = new NpgsqlConnection(@"Server = localhost; Port = 5432; User ID = postgres; Password = postgres; Database = armdoctor; Persist Security Info = True");
        public static NpgsqlConnection connection = new NpgsqlConnection(str);
        public static NpgsqlCommand command = default(NpgsqlCommand);
        public static string sql = string.Empty;
        
        public static string k()
        {
            return  '\u0022' + " ".Trim();
        }

        public static DateTime toData(string dateString)
        {
            var cultureInfo = new CultureInfo("ru-RU");
            var dateTime = DateTime.Parse(dateString, cultureInfo);
            return dateTime;
        }

        public void openConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
        }

        public void closeConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }

        public NpgsqlConnection getConnection()
        {
            return connection;
        }

        public static DataTable SQLGetTable(string sql)
        {
            DB db = new DB();
            DataTable table = new DataTable();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
            NpgsqlCommand command = new NpgsqlCommand(sql, db.getConnection());

            try
            {
                db.openConnection();
                adapter.SelectCommand = command;
                adapter.Fill(table);
                db.closeConnection();
                return table;
            }
            catch (Exception ex)
            {
                string str = "Ошибка выполнения SQL запроса: " + sql;
                MessageBox.Show("Обнаружена ошибка: " + ex.Message, str,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                table = null;
            }

            return table;
        }

        public static DataTable SQLRun(NpgsqlCommand com)
        {
            NpgsqlDataAdapter adapter = default(NpgsqlDataAdapter);
            DataTable table = new DataTable();

            try
            {
                adapter = new NpgsqlDataAdapter();
                adapter.SelectCommand = com;
                adapter.Fill(table);

                return table;
            }
            catch (Exception ex)
            {
                string str = "Ошибка выполнения SQL запроса ";
                MessageBox.Show("Обнаружена ошибка: " + ex.Message, str,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                table = null;
            }

            return table;
        }
    }
}
