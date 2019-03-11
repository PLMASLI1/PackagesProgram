using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Sockets;
using PackagesProgram.Properties;

namespace PackagesProgram.Helpers
{
    public class DatabaseOperation
    {
        public SqlConnection GetOpenDatabaseConnection()
        {
            var connection = new SqlConnection(Settings.Default.DatabaseConnectionString);

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

        public IList<int> GetIdsFromPackagesTable()
        {
            var list = new List<int>();

            using (var connection = GetOpenDatabaseConnection())
            {
                try
                {
                    var sqlCommand = new SqlCommand(Resources.SelectAllRecordsFromPackagesTable, connection);
                    var reader = sqlCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        list.Add(id);
                    }
                }
                catch (Exception e)
                {
                    throw new ArgumentException(string.Format(Resources.DownloadInterruptedMessage, e, e.InnerException));
                }
            }

            return list;
        }

        public void InsertIdToPackagesTable(int packageId)
        {
            if (packageId <= 0)
                throw new ArgumentOutOfRangeException(nameof(packageId));

            using (var connection = GetOpenDatabaseConnection())
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
                    throw new ArgumentException(string.Format(Resources.InsertInterruptedMessage, e, e.InnerException));
                }
            }
        }

        public int RandomIdFromTheRange(int startRange, int endRange)
        {
            if (startRange <= 0)
                return -1;
            if (endRange <= 0)
                return -1;
            if (startRange >= endRange)
                throw new ArgumentOutOfRangeException(string.Format(Resources.RandomIdInterruptedMessage, startRange, endRange));

            using (var connection = GetOpenDatabaseConnection())
            {
                try
                {
                    var center = (startRange + endRange) / 2;

                    while (startRange != endRange)
                    {                        
                        if ((int)new SqlCommand(string.Format(Resources.SelectCountIdsFromRangeCommand, center, center), connection).ExecuteScalar() == 0)
                            return center;

                        var leftRangeCount = (int)new SqlCommand(string.Format(Resources.SelectCountIdsFromRangeCommand, startRange, center), connection).ExecuteScalar();
                        var rightRangeCount = (int)new SqlCommand(string.Format(Resources.SelectCountIdsFromRangeCommand, center + 1, endRange), connection).ExecuteScalar();

                        if (leftRangeCount == 0)
                            return new Random().Next(startRange + 1, center + 1);
                        if (rightRangeCount == 0)
                            return new Random().Next(center + 1, endRange + 1);

                        if (leftRangeCount <= rightRangeCount)
                            endRange = center;
                        else
                            startRange = center;

                        center = (startRange + endRange) / 2;
                    }
                }
                catch (Exception e)
                {
                    throw new ArgumentException(string.Format(Resources.RandomIdInterruptedWithErrorMessage, e, e.InnerException));
                }
            }

            return -2;
        }
    }
}