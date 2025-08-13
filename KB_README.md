# User Manager Notes - Tech Test

## Added projects 
UserManagment.Blazor (Added to meet criteria 5)
UserManagment.TestAsyncHelper (Added as a hared library to support the mocking of async calls)


## Packages Updated

### UserManagment.Data Packages Added 
Microsoft.EntityFrameworkCore 9.6
Microsoft.EntityFrameworkCore.Abstractions 9.6
Microsoft.EntityFrameworkCore.SqlServer 9.6

### UserManagment.Services.Tests 
TestAsyncHelper

### UserManagment.Web.Tests 
TestAsyncHelper

## Database Setup Dac Pac
1)	Go to sql server and create a brand new database
2)	Deploy the dacpack to the newly created sql server database
3)	Go to the serviceCollectionExtension on the data project and insert the connection string into the usesqlserver brackets 

## Project Startup 
For running this project, I have selected both the .web and .blazor applications as start up projects