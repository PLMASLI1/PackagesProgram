using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PackagesProgram.Properties;

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
                throw new System.Exception(string.Format(Resources.ConnectionErrorMessage, e, e.InnerException));
            }
        }

        public IList<int> GetIdCollectionFromPackagesTable()
        {
            var list = new List<int>();

            using (var connection = GetDatabaseConnection())
            {
                try
                {
                    var sqlCommand = new SqlCommand(Properties.Resources.SelectAllRecordsFromPackagesTable, connection);
                    var reader = sqlCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        list.Add(id);
                    }
                }
                catch (Exception e)
                {
                    throw new ArgumentException($"Exception: {e}. Inner exception: {e.InnerException}");
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
                    var sqlCommand = new SqlCommand(Resources.InsertCommand, connection);
                    sqlCommand.Parameters.AddWithValue("@Id", packageId);
                    sqlCommand.Connection = connection;
                    sqlCommand.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw new ArgumentException($"Exception: {e}. Inner exception: {e.InnerException}");
                }
            }
        }

        public int RandomIdFromTheRange(int startRange, int endRange)
        {
            if (startRange < 0)
                return -1;
            if (endRange <= 0)
                return -1;
            if (startRange > endRange)
                throw new ArgumentOutOfRangeException(nameof(endRange), "The specified range is incorrect.");

            return new Random().Next(startRange + 1, endRange + 1);
        }

        public bool CheckIfIdExist(int searchId)
        {
            var ids = GetIdCollectionFromPackagesTable();
            var left = 0;
            var right = ids.Count - 1;

            while (left <= right)
            {
                var currentPosition = (right + left) / 2;

                if (ids[currentPosition] == searchId)
                    return true;
                else if (ids[currentPosition] < searchId)
                    left = currentPosition + 1;
                else
                    right = currentPosition - 1;
            }

            return false;
        }
    }
}