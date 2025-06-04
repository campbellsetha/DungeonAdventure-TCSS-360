namespace UDA.Model;

// for thread safety, should not instantiate multiple random objects
public static class RandomSingleton
{
    private static Random _instance;

    public static Random GetInstance()
    {
        return _instance ??= new Random();
    }
}