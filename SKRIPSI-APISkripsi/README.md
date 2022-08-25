# SKRIPSI

Migration Steps
 
Go to Visual Studio "Tools -> NuGet Package Manager -> Package Manager Console". Then execute the following command. 
  1. EntityFrameworkCore\Enable-Migrations – ( We need to enable the migration, only then can we do the EF Code First Migration ).
  2. EntityFrameworkCore\Add-Migration IntialDb (migration name) – ( Add a migration name and run the command ).
  3. EntityFrameworkCore\Update-Database -Verbose — if it is successful then we can see this message (Running Seed method).
  
  ATAU
  
  1. dotnet ef migrations add InitialCreate
  2. dotnet ef database update
Once Migration is done; then, we can see that the respective files are auto-generated under the “Migrations” folder.


Add New Column In Table

Step 1

To add a new column in the table add a new property in your model. As you see in the below image I add BookLanguage in my Book model.
![image](https://user-images.githubusercontent.com/45263703/169355566-adc40c6c-e569-4600-80f9-b1d555391a4d.png)

Step 2

Now execute the below command. Here AddLanguageColumn is a name which I am providing you can give as per your choice.
 1. Add-Migration AddLanguageColumn
ActionScript
  After executing the above command a new file is created in the migration folder. As you can see in the below code that this is a C# code for adding a new column that converts into SQL and we will get a new column in our table.  
  
using Microsoft.EntityFrameworkCore.Migrations;

namespace ASPNetCoreCodeFirst.Migrations

{
    public partial class AddLanguageColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BookLanguage",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookLanguage",
                table: "Books");
        }
    }
}

Step 3

Now run the below command to make changes in the SQL server. 
 1. Update-Database  
As you see in the below image new column BookLanguage is added to my table Books.
 
![image](https://user-images.githubusercontent.com/45263703/169355966-e4e55a0a-57a2-48f2-9dd9-55b1d6f194d2.png)

 
 Delete Column from Table
 
Step 1

Deleting a column from the table is simple in entity framework core you just need to remove that property or comment from your entity model. As you can see in the below image I comment out BookLanguage property.
![image](https://user-images.githubusercontent.com/45263703/169356096-362052d4-8da8-4c92-8bbb-8360447ece2f.png)

 
 Step 2

Run the below command,
 1. Add-Migration RemoveLanguageColumn
Using this command a new file is created in the Migration folder. As you can see from the below code that our BookLanguage will drop when we update the database.
 
 
using Microsoft.EntityFrameworkCore.Migrations;

namespace ASPNetCoreCodeFirst.Migrations
{
    public partial class RemoveLanguageColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookLanguage",
                table: "Books");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BookLanguage",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
 
 Step 3

Run the below command to update the database.
  1. Update-Database
After executing the above code you can see in the below image that the BookLanguage column is deleted from Books table.
![image](https://user-images.githubusercontent.com/45263703/169356521-4d42d8b4-9251-4d55-9530-ef7b7b57e87c.png)
 
 
