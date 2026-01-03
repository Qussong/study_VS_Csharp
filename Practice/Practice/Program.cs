

Console.ReadKey();

abstract class Animal 
{
    public abstract void Move();
}

// Animal <- Cat
class Cat : Animal 
{
    public sealed override void Move()
    {
    }
}

// Cat <- Tiger
class Tiger : Cat 
{
    public override void Move() // ❌ 오류발생
    {
    }
}