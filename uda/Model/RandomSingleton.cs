namespace UDA.Model;

// for thread safety, should not instantiate multiple random objects
public static class RandomSingleton
{
    private static Random _instance;

    /// <summary>
    /// Gets an instance of the Random object
    /// </summary>
    public static Random GetInstance()
    {
        return _instance ??= new Random();
    }

    /// <summary>
    /// Sets the seed.
    /// </summary>
    public static void SetSeed(int theSeed)
    {
        _instance = new Random(theSeed);
    }

    /// <summary>
    /// Sets the seed to the default value.
    /// </summary>
    public static void UnsetSeed()
    {
        _instance = new Random();
    }
}