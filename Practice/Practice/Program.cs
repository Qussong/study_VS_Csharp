


using System.Reflection.Metadata;

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

//var result = from student in students
//             join score in studentScores
//             on student.Id equals score.StudentId   // join 완성
//             select (student, score);

var result = students.Join(
        studentScores,                          // Join 대상의 컬렉션
        student => student.Id,                  // Key
        score => score.StudentId,               // Join 대상의 Key
        (student, score) => (student, score)    // 반환 타입 (첫번째 컬렉션의 요소, 두번째 컬렉션의 요소)
    );

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

Console.ReadKey();

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
