


IAnimal dog = new Dog();
IAnimal bird = new Bird();

dog.PrintInformation();
bird.PrintInformation();

Console.ReadKey();

interface IAnimal
{
    void MakeSound();
    string Name { get; set; }
    void PrintInformation()
    {
        Console.WriteLine($"안녕하세요. 저는 {Name}입니다.");
    }
}

interface IFlyable
{
    void Fly();
}

class Dog : IAnimal
{
    public string Name { get; set; } = "멍멍이";

    void MakeSound()
    {
        Console.WriteLine("멍멍");
    }

    void IAnimal.MakeSound()
    {
        MakeSound();
    }
}

class Bird : IAnimal, IFlyable
{
    public string Name { get; set; } = "짹짹이";

    public void Fly()
    {
        Console.WriteLine("나는 난다.");
    }

    public void MakeSound()
    {
        Console.WriteLine("짹짹");
    }
}

