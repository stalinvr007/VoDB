using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VODB.VirtualDataBase.TSqlCommands;

namespace VODB.Tests.Models.TSqlCommands {

    [TestClass]
    public class CommandsBuildingTester {

        [TestMethod]
        public void BuildInsert() {
            var cmd = new TInsert(new Employees().Table);

            var result = cmd.BuildCmdStr();
            Assert.AreEqual("Insert into [Employees]( [LastName], [FirstName], [Title], [TitleOfCourtesy], [BirthDate], [HireDate], [Address], [City], [Region], [PostalCode], [Country], [HomePhone], [Extension], [Notes], [ReportsTo], [PhotoPath]) values (@LastName,@FirstName,@Title,@TitleOfCourtesy,@BirthDate,@HireDate,@Address,@City,@Region,@PostalCode,@Country,@HomePhone,@Extension,@Notes,@ReportsTo,@PhotoPath)", result);
        }

        [TestMethod]
        public void BuildUpdate() {
            var cmd = new TUpdate(new Employees().Table);

            var result = cmd.BuildCmdStr();
            Assert.AreEqual("Update [Employees] Set [LastName] = @LastName,  [FirstName] = @FirstName,  [Title] = @Title,  [TitleOfCourtesy] = @TitleOfCourtesy,  [BirthDate] = @BirthDate,  [HireDate] = @HireDate,  [Address] = @Address,  [City] = @City,  [Region] = @Region,  [PostalCode] = @PostalCode,  [Country] = @Country,  [HomePhone] = @HomePhone,  [Extension] = @Extension,  [Notes] = @Notes,  [ReportsTo] = @ReportsTo,  [PhotoPath] = @PhotoPath Where  [EmployeeId] = @OldEmployeeId", result);
        }

        [TestMethod]
        public void BuildSelect() {
            var select = new TSelect(new Employees().Table);

            var result = select.BuildCmdStr();
            Assert.AreEqual("Select *  From [Employees] with (nolock) ", result);
        }

        [TestMethod]
        public void BuildSelectById() {
            var select = new TSelectById(new Employees().Table);

            var result = select.BuildCmdStr();
            Assert.AreEqual("Select *  From [Employees] with (nolock)  Where  [EmployeeId] = @EmployeeId", result);
        }


    }
}
