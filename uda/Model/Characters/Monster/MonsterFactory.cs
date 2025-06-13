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
        const string query = "SELECT * FROM Monster WHERE ID = 1";
        ConnectDb(query);
        return new Monster(_myName, _myHitPoints, _myAttackSpeed, _myHitChance, _myDamageRange, _myHealChance,
            _myHealRange, _myStunThreshold);
    }

    public static Monster CreateOgre()
    {
        const string query = "SELECT * FROM Monster WHERE ID = 2";
        ConnectDb(query);
        return new Monster(_myName, _myHitPoints, _myAttackSpeed, _myHitChance, _myDamageRange, _myHealChance,
            _myHealRange, _myStunThreshold);
    }

    public static Monster CreateSkeleton()
    {
        const string query = "SELECT * FROM Monster WHERE ID = 3";
        ConnectDb(query);
        return new Monster(_myName, _myHitPoints, _myAttackSpeed, _myHitChance, _myDamageRange, _myHealChance,
            _myHealRange, _myStunThreshold);
    }

    public static Monster CreateRandoMonster()
    {
        var rng = RandomSingleton.GetInstance();
        var ranNum = rng.Next(1, 4);

        var query = $"SELECT * FROM Monster WHERE ID = {ranNum}";
        ConnectDb(query);
        return new Monster(_myName, _myHitPoints, _myAttackSpeed, _myHitChance, _myDamageRange, _myHealChance,
        _myHealRange, _myStunThreshold);
    }
    
    private static void ConnectDb(in string theQuery)
    {
        
        const string connectionString = "Data Source=CharacterDatabase.db";
        using var conn = new SQLiteConnection(connectionString);
        //Open the connection
        try
        {
            conn.Open();
            //Execute a command with a command string to the connection
            using var cmd = new SQLiteCommand(theQuery, conn);
            //open and read through the specified values
            using var reader = cmd.ExecuteReader();
            //Initialize values base on rows of selected table 
            reader.Read();
            var count = 1;
            _myName = reader.GetString(count);
            _myHitPoints = reader.GetInt32(++count);
            _myAttackSpeed = reader.GetInt32(++count);
            _myHitChance = reader.GetDouble(++count);
            _myDamageRange = (reader.GetInt32(++count), reader.GetInt32(++count));
            _myStunThreshold = reader.GetDouble(++count);
            _myHealChance = reader.GetDouble(++count);
            _myHealRange = (reader.GetInt32(++count), reader.GetInt32(++count));
            reader.Close();
            conn.Close();
        }
        catch (SQLiteException e)
        {
            Console.WriteLine($"Exception message: {e.Message}");
        }
    }
}