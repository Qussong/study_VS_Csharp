## 제네릭 정의 및 기초
- 클래스, 메서드, 인터페이스, 델리게이트 등을 정의할 때 **\"데이터 타입을 일반화"** 하여 재사용성을 높이고 타입 안정성을 제공하는 기능
- 주요 이점
    1. 타입 안전성 Type Safety
    2. 코드 재사용성 Code Reusability
    3. 성능 향상 Performance Improvement
- 예제 코드
    ```cs
    int a = 1;
    int b = 2;

    /*void Swap(ref int a, ref int b)
    {
        int temp = a;
        a = b;
        b = temp;
    }*/

    // 제네릭화 하여도 정상적으로 작동하는것을 볼 수 있다.
    void Swap<T>(ref T a, ref T b)
    {
        T temp = a;
        a = b;
        b = temp;
    }

    Console.WriteLine($"before) a : {a}, b : {b}");
    // before) a : 1, b : 2

    // T 가 int 인것을 암시적으로 유추할 수 있기에 생략해줘도 된다.
    Swap(ref a, ref b); // = Swap<int>(ref a, ref b); 

    Console.WriteLine($"after) a : {a}, b : {b}");
    // after) a : 2, b : 1
    ```
    `T`는 형식 매개변수, 타입을 유연하게 받아주는 역할을 한다. 아래와 같이 인자들을 다른 타입으로 변경하여도 정상적으로 동작한다.
    ```cs
    double a = 3.14;
    double b = 2.64;

    Console.WriteLine($"before) a : {a}, b : {b}");
    // before) a : 3.14, b : 2.64
    Swap(ref a, ref b);
    Console.WriteLine($"after) a : {a}, b : {b}");
    // after) a : 2.64, b : 3.14
    ```

## 제네릭 제약 조건 - struct, class
- `where` 키워드를 사용하여 제약 조건을 걸 수 있다.
    ```cs
    void Swap<T>(ref T a, ref T b) where T : class
    {
        T temp = a;
        a = b;
        b = temp;
    }
    ```
    class 외에도 struct, new(), interface 도 넣을 수 있다.
- 위와 같이 변경하게되면 인자로 class 만 넣을 수 있다. 때문에 int, double 과 같이 struct 타입의 값은 넣을 수 없게된다.
    ```cs
    int a = 1;
    int b = 2;

    Swap(ref a, ref b); // ❌ 오류발생
    ```
- where T : class 예시 코드
    ```cs
    Animal a = new Dog();
    Animal b = new Cat();

    void Swap<T> (ref T a, ref T b) where T : class
    {
        T temp = a;
        a = b;
        b = temp;
    }

    Swap(ref a, ref b);
    Console.WriteLine($"a : {a}, b : {b}");
    // a : 제 이름은 냥냥이입니다., b : 제 이름은 멍멍이입니다.

    abstract class Animal
    { 
        abstract public string Name { get; }

        public override string ToString()
        {
            return $"제 이름은 {Name}입니다.";
        }
    }

    class Dog : Animal
    {
        public override string Name => "멍멍이";
    }

    class Cat : Animal
    {
        public override string Name => "냥냥이";
    }
    ```

### Note 문자열 보간
- 문자열 보간(string Interpolation) 의 동작 방식
    ```cs
    $"a : {a}, b : {b} "
    ```
    해당 문법은 컴파일 시 내부적으로 string.Format 호출로 변환된다.
    즉, 개념적으로는 아래와 같다.
    ```cs
    string.Format("a : {0}, b : {1}", a, b);
    ```
- `string.Format`은 각 인자를 문자열로 변환해야한다. 이때 규칙은 아래와 같다.
    1. 값이 `null` 이면 빈 문자열
    2. `IFormattable`을 구현했으면 그 포맷 메서드 사용
    3. 그렇지 않으면 `ToString()` 호출

## 제네릭 제약 조건 - new()
- `new()` 제약 조건은 "해당 제네릭 타입은 매개변수 없는 public 생성자를 반드시 가져야 한다."는 의미다.
    - public
    - 매개변수 없는 생성자
    - new T() 호출 가능 생성자
    ```cs
    Animal a = new Dog();
    Animal b = new Cat();

    void CreateInstance<T, T2>(out T a, out T2 b)
        where T : class, new()
        where T2 : class, new()
    {
        a = new T();
        b = new T2();
    }

    CreateInstance(out Dog dog, out Cat cat);
    Console.WriteLine($"a : {dog}, b : {cat}");
    // a : 제 이름은 멍멍이입니다., b : 제 이름은 냥냥이입니다.
    ```
- 제네릭만을 사용해 특정 타입의 객체를 생성할 수 있다.
    ```cs
    T CreateInstance<T>() 
        where T : class, new()
    {
        return new T();
    }

    var animal = CreateInstance<Dog>(); // var 는 Dog? 타입
    Console.WriteLine(animal);          // 제 이름은 멍멍이입니다.
    var animal2 = CreateInstance<Dog>();// var 는 Cat? 타입
    Console.WriteLine(animal2);         // 제 이름은 냥냥이입니다.
    ```

## 제네릭 제약 조건 - class type
- class 대신 클래스 타입 자체를 제약조건으로 넣을 수 있다.
    ```cs
    // T 제네릭은 Animal 에 있는 속성이나 메서드를 사용할 수 있게된다.
    T CreateInstance<T>()
        where T : Animal, new()

    {
        T instance = new T();
        instance.MakeSoune();
        return instance;
    }

    var animal = CreateInstance<Dog>(); // 멍멍
    Console.WriteLine(animal);          // 제 이름은 멍멍이입니다.

    abstract class Animal
    {
        abstract public string Name { get; }
        abstract public void MakeSoune();
        public override string ToString()
        {
            return $"제 이름은 {Name}입니다.";
        }
    }

    class Dog : Animal
    {
        public override string Name => "멍멍이";

        public override void MakeSoune()
        {
            Console.WriteLine("멍멍");
        }
    }
    ```

## 제네릭 제약 조건 - interface
```cs
T CreateInstance<T>()
    where T : IAnimal, new()

{
    T instance = new T();
    instance.MakeSoune();
    return instance;
}

var animal = CreateInstance<Dog>();     // 멍멍
Console.WriteLine(animal);              // Dog
var animal2 = CreateInstance<Cat>();    // 냥냥
Console.WriteLine(animal2);             // Cat

interface IAnimal
{
    string Name { get; }
    void MakeSoune();
}

class Dog : IAnimal
{
    public string Name => "멍멍이";

    public void MakeSoune()
    {
        Console.WriteLine("멍멍");
    }
}

class Cat : IAnimal
{
    public string Name => "냥냥이";

    public void MakeSoune()
    {
        Console.WriteLine("냥냥");
    }
}
```

### Note 인테페이스의 멤버는 abstract 인가?
```cs
interface IAnimal
{
    string Name { get; }
    void MakeSoune();
}
```
- 인턴페이스의 멤버는 abstract가 숨겨져 있다고 말할 수는 있지만, 정확히는 인터페이스 멤버는 원래부터 추상 멤버이며, abstract라는 키워드를 쓰지 않는다.
- Name 과 MakeSound() 이 둘은 모두 구현부가 없으며, 반드시 구현 클래스가 구현을 해줘야 한다. 즉, 의미적으로는 abstract 와 완전히 동일한 역할을 한다.

## 제네릭 class

```cs
var genericBox = new GenericBox<string>();
genericBox.Add("myGeneric");
var item = genericBox.GetItem();
Console.WriteLine(item);    // myGeneric

Console.ReadKey();

class GenericBox<T>
{
    private T? _item;

    public void Add(T item)
    {
        _item = item;
    }

    public T? GetItem()
    {
        return _item;
    }
}
```
