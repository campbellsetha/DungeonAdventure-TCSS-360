using System.Data.SQLite;
using Godot;

namespace UDA.Model;

public abstract class MonsterFactory
{
    private static string myName;
    private static int myHitPoints;
    private static int myAttackSpeed;
    private static double myHitChance;
    private static (int, int) myDamageRange;
    private static double myHealChance;
    private static (int, int) myHealRange;
    private static double myStunThreshold;
    
    public static Monster CreateGremlin()
    {
        string query = "SELECT * FROM Monster WHERE ID = 1";
        ConnectDB(query);
        return new Monster(myName, myHitPoints, myAttackSpeed, myHitChance, myDamageRange, myHealChance, myHealRange, myStunThreshold);
    }

    public static Monster CreateOgre()
    {
        string query = "SELECT * FROM Monster WHERE ID = 2";
        ConnectDB(query);
        return new Monster(myName, myHitPoints, myAttackSpeed, myHitChance, myDamageRange, myHealChance, myHealRange, myStunThreshold);
    }

    public static Monster CreateSkeleton()
    {
        string query = "SELECT * FROM Monster WHERE ID = 3";
        ConnectDB(query);
        return new Monster(myName, myHitPoints, myAttackSpeed, myHitChance, myDamageRange, myHealChance, myHealRange, myStunThreshold);
    }

    public static Monster CreateRandoMonster()
    {
        RandomNumberGenerator rng = new RandomNumberGenerator();
        int ranNum = rng.RandiRange(1, 3);
        
        string query = $"SELECT * FROM Monster WHERE ID = {ranNum}";
        ConnectDB(query);
        return new Monster(myName, myHitPoints, myAttackSpeed, myHitChance, myDamageRange, myHealChance, myHealRange, myStunThreshold);
    }

    private static void ConnectDB(string theQuery)
    {
        string connectionString = "Data Source=MonsterDatabase.db;";
            
        //Is this thread safe to do?
        using (SQLiteConnection conn = new SQLiteConnection(connectionString))
        {
            //Open the connection
            conn.Open();
          
            //Execute a command with a command string to the connection
            using (SQLiteCommand cmd = new SQLiteCommand(theQuery, conn))
            {
                //open and read through the specified values
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    //While tokes are present do stuff
                    while (reader.Read())
                    {
                        //Initialize values base on rows of selected table 
                        myName = reader.GetString(1);
                        myHitPoints = reader.GetInt32(2);
                        myAttackSpeed = reader.GetInt32(3);
                        myHitChance = reader.GetDouble(4);
                        myDamageRange = (reader.GetInt32(5), reader.GetInt32(6));
                        myStunThreshold = reader.GetDouble(7);
                        myHealChance = reader.GetDouble(8);
                        myHealRange = (reader.GetInt32(9), reader.GetInt32(10));
                    }
                }
            }
        }
    }
    
}