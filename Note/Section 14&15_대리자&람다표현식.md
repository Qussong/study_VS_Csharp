# Section 14

## 대리자
- delegate 는 특정 메서드의 참조를 캡슐화하는 객체다.
- 메서드를 변수처럼 사용하겠다는 의미
- 만드는 법 : delegate 키워드를 사용한다.
    ```cs
    
    void MyMethod()                     // 2. 함수 정의
    {
        Console.WriteLine("안녕하세요.");
    }

    MyDelegate myDelegate = MyMethod;   // 3. 대리자 선언 및 할당

    myDelegate();                       // 4. 대리자 호출

    delegate void MyDelegate();         // 1. 대리자 정의
    ```
- 대리자는 메서드를 참조하여 대리자를 메서드를 대신하여 사용된다.

## 매개변수가 있는 대리자
```cs
void Plus(int a, int b)
{
    Console.WriteLine($"a + b = {a + b}");
}

Operation operation = Plus;

operation(10, 5);   // a + b = 15

delegate void Operation(int a, int b);
```

## 반환 값이 있는 대리자
```cs
int Plus(int a, int b)
{
    return a + b;
}

Operation operation = Plus;

int result = operation(10, 5);
Console.WriteLine(result);  // 15

delegate int Operation(int a, int b);
```

## 대리자 - 멀티 캐스팅
- 대리자는 하나의 메서드만이 아닌 여러개의 메서드를 참조할 수 있다.
    ```cs
    int Plus(int a, int b)
    {
        Console.WriteLine($"a + b = {a + b}");
        return a + b;
    }

    int Minus(int a, int b)
    {
        Console.WriteLine($"a - b = {a - b}");
        return a - b;
    }

    Operation operation = Plus;
    operation += Minus;

    int result = operation(10, 5);
    Console.WriteLine(result);
    /*
    * a + b = 15
    * a - b = 5
    * 5    <- 제일 마지막에 참조한 메서드의 결과를 반환
    */

    operation -= Plus;  // 참조 메서드 제거
    result = operation(20, 5);
    Console.WriteLine(result);
    /* 
    * a - b = 15
    * 15
    */

    // 대리자 선언
    delegate int Operation(int a, int b);
    ```

## 대리자 - 이벤트
- 대리자는 주로 이벤트를 발생시킬 때 많이 사용된다.
    ```cs
    Calculate calculate = new Calculate();
    calculate.OnValueChanged += Calculate_OnValueChanged;   // 이벤트 등록

    void Calculate_OnValueChanged(int result, string message)
    {
        Console.WriteLine($"{message} - 현재 값 : {result}");
    }
        
    calculate.Plus(5);      // 5을 더했습니다. - 현재 값 : 5
    calculate.Plus(3);      // 3을 더했습니다. - 현재 값 : 8
    calculate.Minus(2);     // 2을 뻈습니다. - 현재 값 : 6
    calculate.Minus(10);    // 10을 뻈습니다. - 현재 값 : -4

    // 대리자 선언
    delegate void ValueChangeHandler(int result, string message);

    // 클래스 정의
    class Calculate()
    {
        // 이벤트 선언
        public event ValueChangeHandler? OnValueChanged;
        private int _value;

        public void Plus(int value)
        {
            _value += value;
            // case 1. null 검사
            if(null != OnValueChanged)
                OnValueChanged(_value, $"{value}을 더했습니다.");   // 대리자 호출
        }

        public void Minus(int value)
        {
            _value -= value;
            // case 2. Invoke() 를 통한 호출
            OnValueChanged.?Invoke(_value, $"{value}을 뻈습니다.");     // 대리자 호출
        }
    }
    ```
- 이벤트를 제거하는 방법은 멀티 캐스트와 동일하게 `-=` 연산자를 사용해주면된다.

### Note_event 사용 이유
- C#의 event는 델리게이트를 안전하게 공개하기 위한 언어 차원의 접근 제어 장치다.
- 일반 델리게이트는 "함수 포인터 필드" 다.
    ```cs
    public MyDelegate myDelegate;
    ```
    외부에서 모든 조작이 가능하다.
    ```cs
    obj.myDelegate = Foo;  // 교체
    obj.myDelegate = null; // 제거
    obj.myDelegate();      // 직접 호출
    ```
    소유권이 완전히 외부에 있다.
- event 는 "델리게이트에 대한 제한된 인터페이스" 다.
    ```cs
    public event MyDelegate OnMyDelegate
    ```
    외부에서 가능한 것은 구독, 구독해제 뿐이다.
    ```cs
    obj.OnMyDelegate += Foo;    // 구독
    obj.OnMyDelegate -= Foo;    // 구독 해제
    ```
    외부에서 절대 못하는 것들
    ```cs
    obj.OnMyDelegate = Foo; // ❌
    obj.OnMyDelegate = null;// ❌
    obj.OnMyDelegate();     // ❌
    ```
    ⭐ **호출 권한은 오직 선언한 클래스 내부에만 있다.**
- 델리게이트만 사용하면 생기는 문제들 :
    ```cs
    class Button
    {
        public Action Clicked;
    }

    button.Clicked += OnClick;
    button.Clicked = null;      // 💥기존 구독자 모두 삭제
    button.Clicked();           // 💥외부에서 강제 실행
    ```
    위의 상황은 완전히 캡슐화가 깨진 상태다.
- event 가 해결하는 것 : 
    ```cs
    class Button
    {
        public event Action Clicked;
    }

    button.Clicked += OnClick;  // 구독 가능
    button.Clicked();           // ❌ 컴파일 에러 - 외부호출 불가능

    // 클래스 내부에서만 호출 할 수 있다.
    Clicked?.Invoke();          // ⭕
    ```
- 즉, delegate는 "함수 자체"이고, event는 "함수를 등록할 수 있는 권한만 공개한 계약"이다.

### Note_Invoke()
- `Invoke`는 델리게이트 인스턴스가 참조하고 있는 메서드를 호출하는 메서드
    ```cs
    delegate void MyDelegate();

    MyDelegate d = Foo;
    d.Invoke(); // Foo 호출
    d();        // 완전히 동일
    ```
    `d()` 는 `d.Invoke()` 의 문법 축약(Syntax Sugar) 이다.
- 델리게이트는 객체이고 "객체라면 메서드를 가진다"는 개념이 필요했다. 이에 생겨난 메서드가 Invoke() 다.
- 이벤트의 경우 외부에서 Invoke() 를 할 수 없다.
- 이벤트는 Invoke() 권한을 제한한 델리게이트다.

## 대리자 - 함수 매개변수
## 대리자 - Func
## 대리자 - Action
## 대리자 - Predicate
## 대리자  - Comparison

# Section 15

## 람다(Lambda) 표현식
