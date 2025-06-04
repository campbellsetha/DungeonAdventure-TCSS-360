using UDA.Model.Characters.Monster;
namespace Tests;

// will throw error if a test class is public just a fyi
class Tests
{
    /*[SetUp]
    public void Setup()
    {
    }*/

    [Test]
    public void Test1()
    {
        Monster monst = MonsterFactory.CreateGremlin();
        Console.WriteLine(monst);
        //Assert.Pass();
    }
}