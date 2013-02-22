<<<<<<< HEAD
### Getting Started
1. Create a class to represent a database table/model.
2. Create properties to represents the fields for that model.
3. Decorate the properties with [DbField, DbKey, DbIdentity, DbRequired, DbBind, DbIgnore] See [Employee.cs](https://github.com/AlienEngineer/VoDB/blob/master/VODB.Tests/Models/Northwind/Employee.cs)
 * [DbField]    -> Allows users to specify the physical name of the field;
 * [DbKey]		 -> Well... its a key...
 * [DbIdentity] -> The field is the Identity field increasing automatically;
 * [DbRequired] -> Allows the framework to check for the required fields before executing SQL commands.
 * [DbBind] -> Lets the framework know whats the key field in the associated table/entity. Use only when field names don't match.
 * [DbIgnore] -> Tells the framework to ignore a field from mapping.
4. Create a ConnectionString entry in the app.config/web.config file with the name of your machine.
 * set the providerName to System.Data.SqlClient or other...

### For more info click [here] (http://alienengineer.github.com/VoDB/)
=======
ConcurrentReader
================

Wrapper that enables concurrent reading from a database or any other IDataReader implementations.

```C#
// Returns some IDataReader implementation...
IDataReader reader = GetReader();
// Wraps the IDataReader making it Thread-Safe
var cReader = reader.AsParallel(); 
```
>>>>>>> 9a396c32582dc7790f9de47ca95aae9a90c7db55
