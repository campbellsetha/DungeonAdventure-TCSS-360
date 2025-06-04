using System.Data.SQLite;
using Godot;

namespace UDA.Model.Characters.Monster;


// Move monster class as private inner class here?
public abstract class MonsterFactory
{
    private static string _myName;
    private static int _myHitPoints;
    private static int _myAttackSpeed;
    private static double _myHitChance;
    private static (int, int) _myDamageRange;
    private static double _myHealChance;
    private static (int, int) _myHealRange;
    private static double _myStunThreshold;
    
    public static Monster CreateGremlin()
    {
        string query = "SELECT * FROM Monster WHERE ID = 1";
        ConnectDB(query);
        return new Monster(_myName, _myHitPoints, _myAttackSpeed, _myHitChance, _myDamageRange, _myHealChance, _myHealRange, _myStunThreshold);
    }

    public static Monster CreateOgre()
    {
        string query = "SELECT * FROM Monster WHERE ID = 2";
        ConnectDB(query);
        return new Monster(_myName, _myHitPoints, _myAttackSpeed, _myHitChance, _myDamageRange, _myHealChance, _myHealRange, _myStunThreshold);
    }

    public static Monster CreateSkeleton()
    {
        string query = "SELECT * FROM Monster WHERE ID = 3";
        ConnectDB(query);
        return new Monster(_myName, _myHitPoints, _myAttackSpeed, _myHitChance, _myDamageRange, _myHealChance, _myHealRange, _myStunThreshold);
    }

    public static Monster CreateRandoMonster()
    {
        RandomNumberGenerator rng = new RandomNumberGenerator();
        int ranNum = rng.RandiRange(1, 3);
        
        string query = $"SELECT * FROM Monster WHERE ID = {ranNum}";
        ConnectDB(query);
        return new Monster(_myName, _myHitPoints, _myAttackSpeed, _myHitChance, _myDamageRange, _myHealChance, _myHealRange, _myStunThreshold);
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
                        _myName = reader.GetString(1);
                        _myHitPoints = reader.GetInt32(2);
                        _myAttackSpeed = reader.GetInt32(3);
                        _myHitChance = reader.GetDouble(4);
                        _myDamageRange = (reader.GetInt32(5), reader.GetInt32(6));
                        _myStunThreshold = reader.GetDouble(7);
                        _myHealChance = reader.GetDouble(8);
                        _myHealRange = (reader.GetInt32(9), reader.GetInt32(10));
                    }
                }
            }
        }
    }
    
}