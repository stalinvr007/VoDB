### Getting Started
1. Create a class to represent a database table/model.
2. Derive it from DbEntity.
3. Create properties to represents the fields for that model.
4. Decorate the properties with [DbField, DbKey, DbIdentity, DbRequired, DbBind, DbIgnore] See [Employee.cs](https://github.com/AlienEngineer/VoDB/blob/master/VODB.Tests/Models/Northwind/Employee.cs)
* [DbField]    -> Allows users to specify the physical name of the field;
* [DbKey]		 -> Well... its a key...
* [DbIdentity] -> The field is the Identity field increasing automatically;
* [DbRequired] -> Allows the framework to check for the required fields before executing SQL commands.
* [DbBind] -> Lets the framework know whats the key field in the associated table/entity. Use only when field names don't match.
* [DbIgnore] -> Tells the framework to ignore a field from mapping.
5. Create a ConnectionString entry in the app.config/web.config file with the name of your machine.
* set the providerName to System.Data.SqlClient or other...

### Example
```
// Basic example for CRUD Operations.
using (var session = new EagerSession()) {
   // Get Employee data.
   Employee employee1 = session.GetById(new Employee() { EmployeeId = 1 });

   // Get All Employees.
   IEnumerable<Employee> employees = session.GetAll<Employee>();

   // Get All Employees with the EmployeeId greater than 5. 
   // Makes a conditional sql statement, doesn't uses the Linq library. 
   // Therefore doesn't load all the employees.
   IEnumerable<Employee> employees1 = session.GetAll<Employee>().Where(e => e.EmployeeId > 5);

   // Order matters
   // Select * From Employees Order By City
   IEnumerable<Employee> employees2 = session.GetAll<Employee>().OrderBy(e => e.City);

   // Select * From Employees Order By City Desc
   IEnumerable<Employee> employees3 = session.GetAll<Employee>().OrderBy(e => e.City).Descending();

   var collection = session
      .GetAll<Employee>().Where(m => m.EmployeeId <= 5);

   /* Select * from Employees where EmployeeId In (Select EmployeeId From Employees where EmployeeId <= 5) */
   var employees4 = session
     .GetAll<Employee>().Where(m => m.EmployeeId).In(collection);

   // Insert a new Employee
   Employee Sergio = session.Insert(new Employee() {
                    FirstName = "Sérgio",
                    LastName = "Ferreira",
                    BirthDate = new DateTime(1983, 4, 16)
   });

   // Change a field.
   sergio.LastName = "test";
   // Update the employee.
   session.Update(sergio);
   // Deletes the employee.
   session.Delete(sergio);
}
```

### Support or Contact
If you have any suggestions for this project or something is not working right please contact me at Alien.Software.Engineer@gmail.com or @AlienEngineer
