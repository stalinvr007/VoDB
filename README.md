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

### See the examples [here] (http://alienengineer.github.com/VoDB/)


### Support or Contact
If you have any suggestions for this project or something is not working right please contact me at Alien.Software.Engineer@gmail.com or @AlienEngineer
