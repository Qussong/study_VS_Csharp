


using System.Text.RegularExpressions;

List<Student> students = [
    new Student { Id = 1, Age = 20, Gender = "F", Scores = [5,3,9], Name = "Alice" },
    new Student { Id = 2, Age = 22, Gender = "M", Scores = [8,3,2], Name = "Bob" },
    new Student { Id = 3, Age = 23, Gender = "M", Scores = [4,4,1], Name = "Charlie" },
    new Student { Id = 4, Age = 21, Gender = "M", Scores = [5,6,2], Name = "David" },
    new Student { Id = 5, Age = 20, Gender = "F", Scores = [9,8,7], Name = "Eve" },
];

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

Console.ReadKey();

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