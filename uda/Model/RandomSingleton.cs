namespace UDA.Model;

// for thread safety, should not instantiate multiple random objects
public static class RandomSingleton
{
    private static Random _instance;

    public static Random GetInstance()
    {
        return _instance ??= new Random();
    }

    public static void SetSeed(int theSeed)
    {
        _instance = new Random(theSeed);
    }

    public static void UnsetSeed()
    {
        _instance = new Random();
    }
}