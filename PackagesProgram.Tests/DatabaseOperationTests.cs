using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PackagesProgram.Helpers;

namespace PackagesProgram.Tests
{
    [TestClass]
    public class DatabaseOperationTests
    {
        private DatabaseOperation databaseOperation;

        [TestInitialize]
        public void Setup()
        {
            databaseOperation = new DatabaseOperation();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DatabaseOperation_RandomIdFromTheRange_WhenRangeIsIncorrect_ShouldThrowArgumentOutOfRangeException()
        {
            databaseOperation.RandomIdFromTheRange(123, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DatabaseOperation_InsertIdToPackagesTable_WhenInputIsNegative_ShouldThrowArgumentOutOfRangeException()
        {
            databaseOperation.InsertIdToPackagesTable(-3);
        }
    }
}