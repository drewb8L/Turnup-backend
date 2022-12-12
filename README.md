# Turnup-backend

>Installation 

Clone repo into a suitable directory and cd into the Turnup folder. If using Visual studio on Windows or Mac, double click the solution file 'Turnup.sln' to open the project.

>Windows

Open the Program.cs file and uncomment the lines that are marked 'uncomment if on windows' And comment out the lines that are marked 'comment if on mac or linux'.

Create a directory on your C: drive to store the sqlite db file like so: `C:\turnupapi`. Make sure that this directory is reflected in this line in Program.cs as:
`var conn = new SqliteConnection($"Data Source=C:\\turnupapi\\turnup.db")`

In a terminal window cd into the folder where the Turnup.csproj file is and run this command, 'dotnet ef database update' to run migrations.

Once migrations are complete run the application, a Swagger UI browser tab should appear which will allow you to test the API endpoints.

>Mac or Linux

On operating systems other than windows there are a few options. Run an Sqlite instance locally or MS Sql server instance in Docker. Follow the instructions for you DB deployment and point your connection string to the proper database.

Once the database is setup run `dotnet ef database update` in a terminal window in the directory where your Turnup.csproj file lives.

After the migrations are complete, run the application and you should see a Swagger UI tab open up in the browser to test the API endpoints.

Start this application first before attempting to run Turnup front end. The application will run on localhost and the frontend app will be able to connect either in an emulator or on a physical device.
