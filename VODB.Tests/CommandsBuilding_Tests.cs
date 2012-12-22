using Microsoft.VisualStudio.TestTools.UnitTesting;
using VODB.Core.Infrastructure;
using VODB.Core.Infrastructure.TSqlCommands;

namespace VODB.Tests
{

    [TestClass]
    public class CommandsBuilding_Tests
    {

        [TestMethod]
        public void BuildInsert()
        {
            var cmd = new TInsert(Utils.EmployeeTable);

            var result = cmd.BuildCmdStr();
            Assert.AreEqual("Insert into [Employees]( [LastName], [FirstName], [Title], [TitleOfCourtesy], [BirthDate], [HireDate], [Address], [City], [Region], [PostalCode], [Country], [HomePhone], [Extension], [Notes], [Photo], [ReportsTo], [PhotoPath]) values (@LastName,@FirstName,@Title,@TitleOfCourtesy,@BirthDate,@HireDate,@Address,@City,@Region,@PostalCode,@Country,@HomePhone,@Extension,@Notes,@Photo,@ReportsTo,@PhotoPath)", result);
        }

        [TestMethod]
        public void BuildUpdate()
        {
            var cmd = new TUpdate(Utils.EmployeeTable);

            var result = cmd.BuildCmdStr();
            Assert.AreEqual("Update [Employees] Set [LastName] = @LastName,  [FirstName] = @FirstName,  [Title] = @Title,  [TitleOfCourtesy] = @TitleOfCourtesy,  [BirthDate] = @BirthDate,  [HireDate] = @HireDate,  [Address] = @Address,  [City] = @City,  [Region] = @Region,  [PostalCode] = @PostalCode,  [Country] = @Country,  [HomePhone] = @HomePhone,  [Extension] = @Extension,  [Notes] = @Notes,  [Photo] = @Photo,  [ReportsTo] = @ReportsTo,  [PhotoPath] = @PhotoPath Where  [EmployeeId] = @OldEmployeeId", result);
        }

        [TestMethod]
        public void BuildSelect()
        {
            var select = new TSelect(Utils.EmployeeTable);

            var result = select.BuildCmdStr();
            Assert.AreEqual("Select *  From [Employees]", result);
        }

        [TestMethod]
        public void BuildSelectById()
        {
            var select = new TSelectById(Utils.EmployeeTable);

            var result = select.BuildCmdStr();
            Assert.AreEqual("Select *  From [Employees] Where  [EmployeeId] = @EmployeeId", result);
        }

        [TestMethod]
        public void CommandsHolder_Test()
        {
            var holder = new TSqlCommandHolderLazy { Table = Utils.EmployeeTable };

            Assert.AreEqual("Select *  From [Employees]", holder.Select);
            Assert.AreEqual("Select *  From [Employees] Where  [EmployeeId] = @EmployeeId", holder.SelectById);
            Assert.AreEqual("Update [Employees] Set [LastName] = @LastName,  [FirstName] = @FirstName,  [Title] = @Title,  [TitleOfCourtesy] = @TitleOfCourtesy,  [BirthDate] = @BirthDate,  [HireDate] = @HireDate,  [Address] = @Address,  [City] = @City,  [Region] = @Region,  [PostalCode] = @PostalCode,  [Country] = @Country,  [HomePhone] = @HomePhone,  [Extension] = @Extension,  [Notes] = @Notes,  [Photo] = @Photo,  [ReportsTo] = @ReportsTo,  [PhotoPath] = @PhotoPath Where  [EmployeeId] = @OldEmployeeId", holder.Update);
            Assert.AreEqual("Select Count(*)  From [Employees]", holder.Count);
            Assert.AreEqual("Select Count(*)  From [Employees] Where  [EmployeeId] = @EmployeeId", holder.CountById);
            Assert.AreEqual("Insert into [Employees]( [LastName], [FirstName], [Title], [TitleOfCourtesy], [BirthDate], [HireDate], [Address], [City], [Region], [PostalCode], [Country], [HomePhone], [Extension], [Notes], [Photo], [ReportsTo], [PhotoPath]) values (@LastName,@FirstName,@Title,@TitleOfCourtesy,@BirthDate,@HireDate,@Address,@City,@Region,@PostalCode,@Country,@HomePhone,@Extension,@Notes,@Photo,@ReportsTo,@PhotoPath)", holder.Insert);
        }
    }
}
