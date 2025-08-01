using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExportTool;
using BankSystem.Domain;
using BankSystem.Domain.Models;
using BankSystem.App;
using BankSystem.App.Services;

namespace BankSystem.App.Tests
{
    public class ExportServiceTests
    {
        [Fact]
        public void ExportToCvsFileTest()
        {
            //Arrange 

            TestDataGenerator generator = new TestDataGenerator();
            var clients = generator.GenerateTestListClients(10);

            string pathToDirectory = Path.Combine("D:", "TestDirectory");
            string fileName = "SUPERTETS.csv";


            ExportService exportService = new ExportService(pathToDirectory, fileName);

            //Act 
            bool result = exportService.ExportClientToCvsFile(clients);

            //Assert
            Assert.True(result);
        }
        [Fact]
        public void ReadClientsFromCvsFileAndWriteToDb_Test()
        {
            //Arrange
            string pathToDirectory = Path.Combine("D:", "TestDirectory");
            string fileName = "SUPERTETS.csv";
            ExportService export = new ExportService(pathToDirectory, fileName);

            //Act
            bool result = export.ReadClientsFromCvsFileAndWriteToDb();

            //Assert
            Assert.True(result);
        }
    }
}
