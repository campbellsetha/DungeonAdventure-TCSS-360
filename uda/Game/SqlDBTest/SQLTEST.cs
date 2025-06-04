using System.Data.SQLite;
using Godot;

namespace UDA.Game.SqlDBTest;

public partial class SQLTEST : Control
{
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        //SqlConnTest();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double theDelta)
    {
    }

    private void SqlConnTest()
    {
        string connectionString = "Data Source=MonsterDatabase.db;";
        string query = "SELECT Name FROM Monster";
        //Connet to the db
        using (SQLiteConnection conn = new SQLiteConnection(connectionString))
        {
            //Open the connection
            conn.Open();
          
            //Execute a command with a command string to the connection
            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                //open and read through the specified values
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    //While tokes are present do stuff
                    while (reader.Read())
                    {
                        GD.Print(reader["Name"]);
                    }
                }
            }
        }
    }
    
}