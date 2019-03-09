using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PackagesProgram.Helpers
{
    public class DatabaseOperation
    {
        public SqlConnection GetDatabaseConnection()
        {
            var connection = new SqlConnection(Properties.Resources.databaseConnectionString);
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                return connection;
            }
            catch (SqlException e)
            {
                Console.WriteLine($"Problem with connection to database. Exception: {e}");
                Console.ReadKey();
            }

            return connection;
        }

        public IList<int> GetIdCollectionFromPackagesTable()
        {
            var list = new List<int>();

            using (var connection = GetDatabaseConnection())
            {
                try
                {
                    var sqlCommand = new SqlCommand(Properties.Resources.databaseConnectionString, connection);
                    var reader = sqlCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        list.Add(id);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return list;
        }

        public void InsertIdToPackagesTable(int packageId)
        {
            if (packageId <= 0)
                throw new ArgumentOutOfRangeException(nameof(packageId));

            using (var connection = GetDatabaseConnection())
            {
                try
                {
                    var command = "INSERT INTO Packages (Id) VALUES (@Id)";
                    var sqlCommand = new SqlCommand(command, connection);

                    sqlCommand.Parameters.AddWithValue("@Id", packageId);
                    sqlCommand.Connection = connection;
                    sqlCommand.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        public int RandomIdFromTheRange(int startRange, int endRange)
        {
            if (startRange <= 0)
                throw new ArgumentOutOfRangeException(nameof(startRange));
            if (endRange <= 0)
                throw new ArgumentOutOfRangeException(nameof(endRange));

            if (startRange > endRange)
                throw new ArgumentOutOfRangeException(nameof(endRange));

            var random = new Random();

            return random.Next(startRange, endRange + 1);
        }

        public bool CheckIfIdExist(int searchId)
        {
            var ids = GetIdCollectionFromPackagesTable();
            var left = 0;
            var right = ids.Count - 1;

            while (left <= right)
            {
                var currentPosition = left + (right + left) / 2;

                if (ids[currentPosition] == searchId)
                    return true;
                if (ids[currentPosition] < searchId)
                    left = currentPosition + 1;
                if (ids[currentPosition] > searchId)
                    right = currentPosition - 1;
            }

            return false;
        }
    }
}