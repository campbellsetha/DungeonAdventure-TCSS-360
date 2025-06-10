using System.Data.SQLite;

namespace UDA.Model.Characters.Monster;

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
        var query = "SELECT * FROM Monster WHERE ID = 1";
        ConnectDB(query);
        return new Monster(_myName, _myHitPoints, _myAttackSpeed, _myHitChance, _myDamageRange, _myHealChance,
            _myHealRange, _myStunThreshold);
    }

    public static Monster CreateOgre()
    {
        var query = "SELECT * FROM Monster WHERE ID = 2";
        ConnectDB(query);
        return new Monster(_myName, _myHitPoints, _myAttackSpeed, _myHitChance, _myDamageRange, _myHealChance,
            _myHealRange, _myStunThreshold);
    }

    public static Monster CreateSkeleton()
    {
        var query = "SELECT * FROM Monster WHERE ID = 3";
        ConnectDB(query);
        return new Monster(_myName, _myHitPoints, _myAttackSpeed, _myHitChance, _myDamageRange, _myHealChance,
            _myHealRange, _myStunThreshold);
    }

    public static Monster CreateRandoMonster()
    {
        var rng = RandomSingleton.GetInstance();
        var ranNum = rng.Next(1, 4);

        var query = $"SELECT * FROM Monster WHERE ID = {ranNum}";
        ConnectDB(query);
        return new Monster(_myName, _myHitPoints, _myAttackSpeed, _myHitChance, _myDamageRange, _myHealChance,
            _myHealRange, _myStunThreshold);
    }

    // TODO: add try catch
    private static void ConnectDB(string theQuery)
    {
        string connectionString = "Data Source=MonsterDatabase.db";

        //Is this thread safe to do?
        using var conn = new SQLiteConnection(connectionString);
        //Open the connection
        conn.Open();

        //Execute a command with a command string to the connection
        using var cmd = new SQLiteCommand(theQuery, conn);
        //open and read through the specified values
        using var reader = cmd.ExecuteReader();
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
        reader.Close();
        conn.Close();
    }
}