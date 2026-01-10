## Linq - 정의
- Language Integrated Query
- C#과 같은 .NET 언어에서 데이터 소스를 통합적으로 조회할 수 있게 해주는 기능
- 데이터베이스, XML, 컬렉션 등 다양한 데이터 소스에 대해 일관된 쿼리 방식 제공
- 컬렉셔(List, Array 등) 내의 데이터를 필터링, 정렬, 그룹화, 변환 등을 간단한 구문으로 수행할 수 있다는 장점이 있다.

## Linq - 쿼리 구조 및 기초
- Linq의 문법은 크게 2가지로 나뉘어진다. 
    1. 쿼리 구문의 문법 (Query Syntax)
    2. 메서드 구문의 문법 
- `쿼리 구문의 문법`은 SQL과 유사한 "선언전 문법"이다.
- 구조
    ```
    var 결과 = from 범위변수 in 데이터소스
                where 조건
                orderby 정렬기준
                select 결과선택;
    ```
    - 데이터 소스 : 컬렉션
    - 범위변수 : 컬렉션 속에 있는 각각의 요소
    - 컬렉션 속의 요소들을 하나씩 순회를 하며 "where ~" 조건을 확인한다.
- LINQ 문을 사용하지 않고 값을 추려내는 코드 예제
    ```cs
    List<Student> students = [
        new Student { Id = 1, Name = "Alice", Age = 20},
        new Student { Id = 2, Name = "Bob", Age = 22},
        new Student { Id = 3, Name = "Charlie", Age = 23},
        new Student { Id = 4, Name = "David", Age = 21},
        new Student { Id = 5, Name = "Eve", Age = 20},
    ];

    // 나이가 21 이상인 사람들을 추려내자
    List<Student> newStudents = new List<Student>();
    foreach (Student student in students)
    {
        if (student.Age >= 21)
            newStudents.Add(student);
    }

    foreach (Student student in newStudents)
    {
        Console.WriteLine(student);
    }
    /*
    * Id: 2, Name: Bob, Age: 22
    * Id: 3, Name: Charlie, Age: 23
    * Id: 4, Name: David, Age: 21
    */

    class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Age { get; set; }

        public override string ToString()
        {
            return $"Id : {Id}, Name : {Name}, Age : {Age}";
        }
    }
    ```
- LINQ 를 사용한다면 아래와 같이 줄일 수 있다.
    ```cs
    // newStudents 는 IEnumerable<Student> 타입이다.
    var newStudents = from student in students
                    where student.Age >= 21
                    select student;
    ```

## Linq - 쿼리 select
- LINQ 의 반환값은 select 문의 반환 타입(`T`)에 대한 열거형(`IEnumerable<T>`)이다.
- 예제 코드
    ```cs
    var results = from student in students
                    select student.Name;

    foreach (string name in results)
    {
        Console.Write(name + " ");
    }
    // Alice Bob Charlie David Eve
    ```
    - `select student.Name`에 의해서 string이 반환되고 그 결과 results 의 타입은 `IEnumerable<string>`이 된다.
    - 
- 익명 타입 활용 예제
    ```cs
    var results2 = from student in students
                    select new { student.Name, student.Age };

    foreach (var result in results2)
    {
        Console.WriteLine($"{result.Name}, {result.Age}");
    }
    // Bob, 22
    // Charlie, 23
    // David, 21
    // Eve, 20
    ```
- 익명 타입에서는 값의 이름을 쉽게 바꿀 수 있다. 아래와 같이 변경 가능
    ```cs
    var results2 = from student in students
                    select new { MyName = student.Name, MyAge = student.Age };

    foreach (var result in results2)
    {
        Console.WriteLine($"{result.MyName}, {result.MyAge}");
    }
    ```
- ValueTuple 을 활용하며 익명 타입은 메서드/시그니처 경계를 넘어 타입을 표현할 수 없던 문제를 해결 할 수 있다.
    ```cs
    // IEnumerable<(string MyName, int MyAge)> 타입
    var results2 = from student in students
                    select (MyName : student.Name, MyAge : student.Age);
    ```
    즉, ValueTuple은 익명 타입이 못 했던 "메서드 간 구조적 데이터 전달(계약)"을 가능하게 해준다.

### Note_익명 타입
- C#의 익명 타입은 이름이 없는 타입을 의미한다.
    ```cs
    var person = new { Name = "Alice", Age = 30 };
    ```
    컴파일러가 내부적으로 클래스를 하나 만들어주지만, 우리는 그 타입의 이름을 알 수 없고, 사용할 수 없다.
- "구조화를 할 수 없다"는 말의 의미 :
    1. 메서드의 매개변수 타입으로 사용할 수 없다.
        ```cs
        // ❌ 불가능
        void PrintPerson(new { string Name, int Age } person)
        {
        }
        ```
        - 익명 타입은 타입 이름이 없기 때문에
        - 메서드 시그니처에는 명확한 타입이 필요하다.
    2. 메서드의 반환 타입으로 사용할 수 없다.
        ```cs
        // ❌ 불가능
        new { string Name, int Age } GetPerson()
        {
        }
        ```
        - 위와 마찬가지의 이유로 사용 불가
- 아래 코드의 경우 var 타입으로 받을 수 있는 이유는 var는 컴파일 타임에 타입이 결정되기에, 같은 메서드 내부 스코프에서는 컴파일러가 익명 타입을 알고 있다.
    ```cs
    // ⭕ 가능
    var p = new { Name = "Bob", Age = 20 };

    //  ❌ 컴파일 불가
    var p = GetSomething();
    var GetSomething()
    {
        return new { Name = "Bob", Age = 20 };
    }
    ```

## Linq - 쿼리 where
- where 키워드는 from 과 select 절 사이에 위치할 수 있으며, 추출되어지는 요소에대한 조건을 설정할 수 있다.
    ```cs
    var results = from ~
                where ~
                select ~
    ```
- 데이터 소스의 각 요소를 순회하던 중 where 절을 충족하지 않으면, select 절을 수행하지 않고 다음 요소로 넘어간다.

## Linq - 쿼리 let
- 쿼리 구문의 변수 선언 방법
- from 절과 select 절 사이에 위치한다.
    ```cs
    var results = from ~
                let ~
                select ~
    ```
- 예제코드 1
    ```cs
    List<Student> students = [
        new Student { Id = 1, Age = 20, Gender = "F", Scores = [5,3,9], Name = "Alice" },
        new Student { Id = 2, Age = 22, Gender = "M", Scores = [8,3,2], Name = "Bob" },
        new Student { Id = 3, Age = 23, Gender = "M", Scores = [4,4,1], Name = "Charlie" },
        new Student { Id = 4, Age = 21, Gender = "M", Scores = [5,6,2], Name = "David" },
        new Student { Id = 5, Age = 20, Gender = "F", Scores = [9,8,7], Name = "Eve" },
    ];

    var results = from student in students
              let average = student.Scores.Average()
              where average > 5
              select (MyName: student.Name, student.Age);
    /*
    * (Alice, 20)
    * (Bob, 22)
    * (David, 21)
    * (Eve, 20)
    */    

    class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Age { get; set; }
        public string Gender { get; set; } = "";
        public List<int> Scores { get; set; } = [];

        public override string ToString()
        {
            return $"Id : {Id}, Name : {Name}, Age : {Age}";
        }
    }
    ```
- 예제코드 2
    ```cs
    var results = from student in students
              let gender = student.Gender
              where gender == "M"
              let average = student.Scores.Average()
              where average > 3
              select (MyName: student.Name, student.Age);
    /*
    * (Bob, 22)
    * (David, 21)
    */
    ```

## Linq - 쿼리 orderby
- 쿼리 구문의 데이터 정렬 기능
- from 과 select 절 사이에 위치
    ```cs
    var results = from ~
                orederby ~
                select ~
    ```
- 코드 예시
    ```cs
    var results = from student in students
              let average = student.Scores.Average()
              where average > 3
              orderby student.Age   // 나이의 오름차순으로 정렬됨
              select (MyName: student.Name, student.Age, Average: average);

    foreach(var result in results)
        Console.WriteLine($"{result.MyName}, {result.Age}, Average : {result.Average}");

    /*
        (Before)
        Alice, 20, Average : 5.666666666666667
        Bob, 22, Average : 4.333333333333333
        David, 21, Average : 4.333333333333333
        Eve, 20, Average : 8
        
        (After)
        Alice, 20, Average : 5.666666666666667
        Eve, 20, Average : 8
        David, 21, Average : 4.333333333333333
        Bob, 22, Average : 4.333333333333333
    */
    ```
- 콤마(,)를 추가하여 추가 조건을 붙일 수 있으며, `descending` 키워드를 추가하면 내림차순 정렬할 수 있다.
    ```cs
    // 나이가 낮은순으로 정렬되고 같은 나이라면 평균이 높을수록 앞에 있다.
    var results = from student in students
            let average = student.Scores.Average()
            where average > 3
            orderby student.Age, average descending
            select (MyName: student.Name, student.Age, Average: average);

    /*
        Eve, 20, Average : 8
        Alice, 20, Average : 5.666666666666667
        David, 21, Average : 4.333333333333333
        Bob, 22, Average : 4.333333333333333
    */
    ```
- 문자열을 손쉽게 정렬할 수 있다는 장점이 있다.
    ```cs
    var results = from student in students
              let average = student.Scores.Average()
              where average > 3
              orderby student.Name descending   // 아스키 코드가 큰값에서 작은값 순으로 정렬됨
              select (MyName: student.Name, student.Age, Average: average);

    foreach(var result in results)
        Console.WriteLine($"{result.MyName}, {result.Age}, Average : {result.Average}");

    /*
        Eve, 20, Average : 8
        David, 21, Average : 4.333333333333333
        Bob, 22, Average : 4.333333333333333
        Alice, 20, Average : 5.666666666666667
    */
    ```

## Linq - 쿼리 group
- 쿼리 구문에서 데이터 그룹핑하는 방법
- from 과 select 절 사이에 위치할 수 있다.
    ```cs
    var results = from ~
                group ~ by ~ into
                select ~
    // group + 순회할 요소 + by + 기준 + into 그룹이름
    ```
- 예제코드
    ```cs
    var genderGroups = from student in students
                let average = student.Scores.Average()
                where average > 3
                orderby student.Name descending
                group student by student.Gender into g
                select (Gender: g.Key, Group: g);    // 그룹핑이 되었기에 group 을 활용해서 select 데이터를 만들어줘야한다.

    foreach (var genderGroup in genderGroups)
    {
        Console.WriteLine($"Gender: {genderGroup.Gender}");
        foreach (var student in genderGroup.Group)
        {
            Console.WriteLine($"{student.Name}, {student.Age}, Average : {student.Scores.Average()}"); 
        }
        Console.WriteLine("------------");
    }

    /*
        Gender: F
        Eve, 20, Average : 8
        Alice, 20, Average : 5.666666666666667
        ------------
        Gender: M
        David, 21, Average : 4.333333333333333
        Bob, 22, Average : 4.333333333333333
        ------------
    */
    ```

## Linq - 메소드 정의
- LINQ는 컬레션 데이터를 처리하는 기능으로 사용된다.
- "메서드 체인"을 통해 필터링, 정렬, 그룹화, 투영 등의 작업을 할 수 있다.

## Linq - 메소드 Select
- 데이터를 투영하는 메서드
- 투영이란?
    1. 데이터의 형식 변경 :</br>
        컬렉션의 요소를 다른 형식으로 변환 예를 들어, 객체에서 특정 필드만 추출하거나 계산된 값을 반환하는게 이에 해당한다.
    2. 구조 변경 :</br>
        복합적인 데이터 구조를 단순화하거나, 필요한 데이터만 선택적으로 포함
- 기본 예제
    ```cs
    var ages = students.Select(student => student.Age);

    foreach (var age in ages)
        Console.Write(age + " ");
    // 20 22 23 21 20
    ```
- 응용 예제 (익명타입 활용)
    ```cs
    // case 1)
    // 익명타입 활용시 편하긴하지만 외부에서 타입을 알 수 없다.
    var myStudents = students.Select(student => new { MyName = student.Name, MyAge = student.Age });
    foreach (var student in myStudents)
    {
        Console.WriteLine($"{student.MyName}, {student.MyAge}");
    }

    // case 2)
    // ValueTuple 활용시 myStudents2의 타입이 명확해진다.
    var myStudents2 = students.Select(student => (MyName: student.Name, MyAge: student.Age));
    foreach (var student in myStudents2)
    {
        Console.WriteLine($"{student.MyName}, {student.MyAge}");
    }
    ```
    
## Linq - 메소드 SelectMany
- 중첩 데이터 평탄화하는 메서드
- 중첩된 컬렉션을 하나의 평면 컬렉션으로 변환한다.
- 예제 코드
    ```cs
    List<Student> students = [
        new Student { Id = 1, Age = 20, Gender = "F", Scores = [5,3,9], Name = "Alice" },
        new Student { Id = 2, Age = 22, Gender = "M", Scores = [8,3,2], Name = "Bob" },
        new Student { Id = 3, Age = 23, Gender = "M", Scores = [4,4,1], Name = "Charlie" },
        new Student { Id = 4, Age = 21, Gender = "M", Scores = [5,6,2], Name = "David" },
        new Student { Id = 5, Age = 20, Gender = "F", Scores = [9,8,7], Name = "Eve" },
    ];

    // scores 의 타입 : IEnumerable<List<int>>
    var scores = students.Select(student => student.Scores);
    // scores2 의 타입 : IEnumerable<int>
    var scores2 = students.SelectMany(student => student.Scores);
    /*
        scores = [
                [5, 3, 9]
                [8, 3, 2]
                [4, 4, 1]
                [5, 6, 2]
                [9, 8, 7]
                ]

        scores2 -> [5, 3, 9,8, 3, 2,4, 4, 1,5, 6, 2, 9, 8, 7]
    */
    ```

## Linq - 메소드 Where
- 특정 조건을 만족하는 요소로 필터링하는 메서드
- 예제 코드
    ```cs
    List<Student> students = [
        new Student { Id = 1, Age = 20, Gender = "F", Scores = [5,3,9], Name = "Alice" },
        new Student { Id = 2, Age = 22, Gender = "M", Scores = [8,3,2], Name = "Bob" },
        new Student { Id = 3, Age = 23, Gender = "M", Scores = [4,4,1], Name = "Charlie" },
        new Student { Id = 4, Age = 21, Gender = "M", Scores = [5,6,2], Name = "David" },
        new Student { Id = 5, Age = 20, Gender = "F", Scores = [9,8,7], Name = "Eve" },
    ];

    var result = students.Where(student => student.Name.EndsWith("e"));
    foreach (var student in result)
    {
        Console.Write(student.Name + " ");
    }
    // Alice Charlie Eve
    ```

## Linq - 메소드 OrderBy, 메서드 체이닝
- 데이터 정렬하는 메서드
- 메서드 뒤에 또 다른 메서드를 호출하는것을 "메서드 체이닝"이라고 하며 이를 활용할 수 있다.
- 예제 코드
    ```cs
    var result = students.Where(student => student.Name.EndsWith("e"))
                        .OrderBy(student => student.Age);
    foreach (Student student in result)
    {
        Console.WriteLine(student);
    }
    /*
        Id: 1, Name: Alice, Age: 20
        Id: 5, Name: Eve, Age: 20
        Id: 3, Name: Charlie, Age: 23
    */
    ```
- 꼭 메서드 체이닝이 아니더라도 사용할 수 있다. 또한 `OrderByDescending()`을 통해 역순으로 정렬할 수 있다.
    ```cs
    var result = students
                .OrderByDescending(student => student.Age);
    foreach (Student student in result)
    {
        Console.WriteLine(student);
    }
    /*
        Id : 3, Name : Charlie, Age : 23
        Id : 2, Name : Bob, Age : 22
        Id : 4, Name : David, Age : 21
        Id : 1, Name : Alice, Age : 20
        Id : 5, Name : Eve, Age : 20
    */
    ```
- 추가적인 정렬 조건은 `TheBy()`, `ThenByDescending()`을 통해 추가할 수 있다.
    ```cs
    var result = students
                .OrderByDescending(student => student.Age)
                .ThenByDescending(student => student.Name);
    foreach (Student student in result)
    {
        Console.WriteLine(student);
    }
    /*
        Id : 3, Name : Charlie, Age : 23
        Id : 2, Name : Bob, Age : 22
        Id : 4, Name : David, Age : 21
        Id : 5, Name : Eve, Age : 20
        Id : 1, Name : Alice, Age : 20
    */
    ```

## Linq - 메소드 GroupBy
- 데이터를 그룹화하는 메서드
- 그룹핑된 각 그룹에는 Key Value 가 존재한다.
- 예제 코드
    ```cs
    var result = students
                .GroupBy(Student => Student.Gender);
    foreach(var genderGroup in result)
    {
        Console.Write($"{genderGroup.Key} : ");
        foreach(var student in genderGroup)
        {
            Console.Write(student.Name + " ");
        }
        Console.WriteLine();
    }
    /*
        F: Alice Eve
        M: Bob Charlie David
    */
    ```
- 출력순서를 조정하고 싶다면 메서드 체이닝을 통해 OrderBy 를 활용하면 된다.
    ```cs
    var result = students
                .GroupBy(Student => Student.Gender)
                .OrderByDescending(g => g.Key);
    foreach(var genderGroup in result)
    {
        Console.Write($"{genderGroup.Key} : ");
        foreach(var student in genderGroup)
        {
            Console.Write(student.Name + " ");
        }
        Console.WriteLine();
    }
    /*
        M : Bob Charlie David
        F : Alice Eve
    */
    ```

## Linq - 쿼리 join
- Join 문은 두 개 이상의 컬렉션을 특정 키(기준 값)를 기준으로 결합할 때 사용하는 LINQ 구문이다.
- Joint 문을 사용하면 두 리스트나 컬렉션에 포함된 데이터를 연결하여 새로운 결과를 생성할 수 있다.
- 예제 코드
    ```cs
    List<Student> students = [
        new Student { Id = 1, Age = 20, Gender = "F", Name = "Alice" },
        new Student { Id = 2, Age = 22, Gender = "M", Name = "Bob" },
        new Student { Id = 3, Age = 23, Gender = "M", Name = "Charlie" },
        new Student { Id = 4, Age = 21, Gender = "M", Name = "David" },
        new Student { Id = 5, Age = 20, Gender = "F", Name = "Eve" },
    ];

    List<Score> studentScores = [
        new Score { StudentId = 1, ScoreValue = 5, Subject = "Math" },
        new Score { StudentId = 1, ScoreValue = 3, Subject = "Science" },
        new Score { StudentId = 1, ScoreValue = 9, Subject = "History" },
        new Score { StudentId = 2, ScoreValue = 8, Subject = "Math" },
        new Score { StudentId = 2, ScoreValue = 3, Subject = "Science" },
        new Score { StudentId = 2, ScoreValue = 2, Subject = "History" },
        new Score { StudentId = 3, ScoreValue = 4, Subject = "Math" },
        new Score { StudentId = 3, ScoreValue = 4, Subject = "Science" },
        new Score { StudentId = 3, ScoreValue = 1, Subject = "History" },
        new Score { StudentId = 4, ScoreValue = 5, Subject = "Math" },
        new Score { StudentId = 4, ScoreValue = 6, Subject = "Science" },
        new Score { StudentId = 4, ScoreValue = 2, Subject = "History" },
        new Score { StudentId = 5, ScoreValue = 9, Subject = "Math" },
        new Score { StudentId = 5, ScoreValue = 8, Subject = "Science" },
        new Score { StudentId = 5, ScoreValue = 7, Subject = "History" },
    ];

    var result = from student in students
                join score in studentScores
                on student.Id equals score.StudentId   // join 완성
                select (student, score);

    // ValueTuple의 경우 요소를 분해해서 꺼낼 수 있다.
    foreach (var (student, score) in result)
    {
        Console.WriteLine(
            $"Student : {student.Name}, " +
            $"Subject : {score.Subject}, " +
            $"Score : {score.ScoreValue}");
    }

    /*
        Student: Alice, Subject: Math, Score: 5
        Student: Alice, Subject: Science, Score: 3
        Student: Alice, Subject: History, Score: 9
        Student: Bob, Subject: Math, Score: 8
        Student: Bob, Subject: Science, Score: 3
        Student: Bob, Subject: History, Score: 2
        Student: Charlie, Subject: Math, Score: 4
        Student: Charlie, Subject: Science, Score: 4
        Student: Charlie, Subject: History, Score: 1
        Student: David, Subject: Math, Score: 5
        Student: David, Subject: Science, Score: 6
        Student: David, Subject: History, Score: 2
        Student: Eve, Subject: Math, Score: 9
        Student: Eve, Subject: Science, Score: 8
        Student: Eve, Subject: History, Score: 7
    */

    class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Age { get; set; }
        public string Gender { get; set; } = "";
        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Age: {Age}";
        }
    }

    class Score
    {
        public int StudentId { get; set; }  // Student 의 Id 와 밀접한 관련이 있다.
        public int ScoreValue { get; set; }
        public string Subject { get; set; } = "";
    }
    ```

## Linq - 메서드 join
- 위의 쿼리 구문을 메서드로 표현해보자
    ```cs
    var result = students.Join(
            studentScores,                          // Join 대상의 컬렉션
            student => student.Id,                  // Key
            score => score.StudentId,               // Join 대상의 Key
            (student, score) => (student, score)    // 반환 타입 (첫번째 컬렉션의 요소, 두번째 컬렉션의 요소)
        );
    ```

## Linq 참고 사이트
- Linq 에는 수많은 함수가 있기에 이를 다 알 수 없다.
- 참고 사이트 : [MSDN - C#의 LINQ 쿼리 소개 URL](https://learn.microsoft.com/ko-kr/dotnet/csharp/linq/get-started/introduction-to-linq-queries)