# Section10

## 인터페이스
- 클래스나 구조체가 구현해야 하는 집합을 정의한다.
- 객체 지향 프로그래밍의 핵심 개념 중 하나로, 코드의 유연성과 재사용성을 높이는 데 중요한 역할을 한다.
- 인터페이스는 자체적으로 인스턴스를 생성할 수 없다.
    ```cs
    IAnimal animal = new IAnimal(); // ❌ 오류발생
    ```
- 정의 방법 : </br>
    `interface` 키워드를 사용하여 정의하며, 일반적으로 `'I'`로 시작하는 네이밍 컨벤션을 따른다.
    ```cs
    interface IAnimal
    {
        void MakeSound();
        string Name { get; set; }
    }

    // IAnimal 에 정의된 함수와 프로퍼티를 구현해야한다. 그렇지않으면 오류가 발생한다.
    class Dog : IAnimal
    {
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void MakeSound()
        {
            throw new NotImplementedException();
        }
    }
    ```
    주요 구성요소</br>
    - 메서드 : 구현해야 할 메서드의 시그니처를 정의
    - 속성 : get 및 set 접근자를 포함한 속성을 정의
    - 이벤트 : 이벤트 핸들러를 정의할 수 있다.

## 인터페이스의 주요 특징
- `다중 구현` : 하나의 클래스가 여러 인테페이스를 구현할 수 있다.
    ```cs
    interface IFlyable
    {
        void Fly();
    }

    class Bird : IAnimal, IFlyable
    {
        public string Name { get; set; } = "";

        public void Fly()
        {
            Console.WriteLine("I Fly");
        }

        public void MakeSound()
        {
            Console.WriteLine("짹짹");
        }
    }
    ```
- `추상화` : 인터페이스는 구현 세부 사항을 숨기고, 필요한 기능만을 노출하여 코드의 추상화를 돕는다.
    ```cs
    IFlyable bird = new Bird();
    bird.Fly();
    bird.MakeSound();   // ❌ 오류발생
    ```
- 인터페이스의 활용 사례
    - 의존성 역전 원칙 (DIP) : 고수준 모듈이 저수준 모듈에 의존하지 않고, 추상화에 의존하도록 설계할 떄 인터페이스를 사용한다.
    - 다형성 : 다양항 클래스가 동일한 인터페이스를 구현함으로써, 동일한 방식으로 객체를 다룰 수 있다.
    - 유닛 테스트 : 모의 객체(Mock Object)를 생성할 때 인터페이스를 사용하여 테스트의 용이성을 높인다.
    - 플러그인 아키텍처 : 확장 가능한 시스템에서 플러그인의 계약을 정의는 데 인터페이스를 사용한다.

## 명시적 인터페이스 구현
- 인터페이스의 멤버를 인터페이스를 통해서만 접근이 가능하도록 만드는 기능
    ```cs
    Dog dog = new Dog();
    dog.MakeSound();    // ❌ 오류발생

    IAnimal dog2 = new Dog();
    dog2.MakeSound();   // ⭕ 정상작동

    class Dog : IAnimal
    {
        public string Name { get; set; } = "";

        void IAnimal.MakeSound()    // public 접근제한자를 지움
        {
            Console.WriteLine("멍멍");
        }
    }
    ```

## 디폴트 구현 (C# 8.0)
- 인터페이스 내부에 메서드의 기능을 구현할 수 있다. (기존까진 불가능)
    ```cs
    IAnimal dog = new Dog();
    IAnimal bird = new Bird();

    dog.PrintInformation();     // 안녕하세요. 저는 멍멍이입니다.
    bird.PrintInformation();    // 안녕하세요. 저는 짹짹이입니다.

    interface IAnimal
    {
        void MakeSound();
        string Name { get; set; }
        void PrintInformation()
        {
            Console.WriteLine($"안녕하세요. 저는 {Name}입니다.");
        }
    }

    class Dog : IAnimal
    {
        public string Name { get; set; } = "멍멍이";

        // ...
    }

    class Bird : IAnimal, IFlyable
    {
        public string Name { get; set; } = "짹짹이";

        // ...
    }
    ```

