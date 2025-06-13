using System.Data.SQLite;

namespace UDA.Model.Characters;

public abstract class HeroFactory
{
    private static int _myHitPoints;
    private static int _myAttackSpeed;
    private static double _myHitChance;
    private static (int, int) _myDamageRange;
    private static double _myBlockChance;
    private static string _mySkill;

    private HeroFactory()
    {
    }

    public static Hero CreatePriest(string theName)
    {
        const string query = "SELECT * FROM Hero WHERE ID = 'Priest'";
        ConnectDb(query);
        return new Priest(theName, _myHitPoints, _myAttackSpeed, _myHitChance, _myDamageRange, _myBlockChance, _mySkill);
    }
    
    public static Hero CreateThief(string theName)
    {
        const string query = "SELECT * FROM Hero WHERE ID = 'Thief'";
        ConnectDb(query);
        return new Thief(theName, _myHitPoints, _myAttackSpeed, _myHitChance, _myDamageRange, _myBlockChance, _mySkill);
    }

    public static Hero CreateWarrior(string theName)
    {
        const string query = "SELECT * FROM Hero WHERE ID = 'Warrior'";
        ConnectDb(query);
        return new Warrior(theName, _myHitPoints, _myAttackSpeed, _myHitChance, _myDamageRange, _myBlockChance, _mySkill);
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
            _myHitPoints = reader.GetInt32(count);
            _myAttackSpeed = reader.GetInt32(++count);
            _myHitChance = reader.GetDouble(++count);
            _myDamageRange = (reader.GetInt32(++count), reader.GetInt32(++count));
            _myBlockChance = reader.GetDouble(++count);
            _mySkill = reader.GetString(++count);
            reader.Close();
            conn.Close();
        }
        catch (SQLiteException e)
        {
            Console.WriteLine($"Exception message: {e.Message}");
        }
    }

    /*public static Hero CreateHero(string theClassType, string theName)
    {
        return theClassType.ToLower() switch
        {
            Warrior => new Warrior(theName),
            Priest => new Priest(theName),
            Thief => new Thief(theName),
            _ => throw new ArgumentException("The class type must be Warrior, Priest, or Thief")
        };
    }*/
}