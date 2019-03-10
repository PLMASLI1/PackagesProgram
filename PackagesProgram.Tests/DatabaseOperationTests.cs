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

        [TestMethod]
        public void DatabaseOperation_CheckIfIdExistInDatabase_WhenInputIsCorrectAndDatabaseIsFull_ReturnCorrectOutput()
        {
            //Arrange
            var idsFromDatabaseInput = new List<int>();
            const bool expectedResult = true;

            for (var index = 0; index < 10000001; index++)
                idsFromDatabaseInput.Add(index);

            //Act
            var result = databaseOperation.CheckIfIdExistInDatabase(88, idsFromDatabaseInput);
            
            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void DatabaseOperation_CheckIfIdExistInDatabase_WhenIdIsLessThenZero_ShouldThrowArgumentOutOfRangeException()
        {
            //Arrange
            var idsFromDatabaseInput = new List<int>{ 1, 2, 3 };
            const bool expectedResult = true;

            //Act
            var result = databaseOperation.CheckIfIdExistInDatabase(-3, idsFromDatabaseInput);

            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void DatabaseOperation_CheckIfIdExistInDatabase_WhenIdsListIsEqualZero_ShouldReturnCorrectOutput()
        {
            //Arrange
            var idsFromDatabaseInput = new List<int>();
            const bool expectedResult = false;

            //Act
            var result = databaseOperation.CheckIfIdExistInDatabase(2, idsFromDatabaseInput);

            //Assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}