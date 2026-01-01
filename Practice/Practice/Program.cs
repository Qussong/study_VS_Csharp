
string grade = "B+";

string message = grade switch
{
    "A" => "우수한 성적입니다.",
    string g when g == "B" || g == "B+" => "좋은 성적입니다.",
    "C" => "보통 성적입니다.",
    _ => "잘 모르겠습니다."
};

Console.WriteLine(message);

/*switch(grade)
{
    case "A":
        Console.WriteLine("우수한 성적입니다.");
        break;
    //case "B":
    case string g when g == "B" || g == "B+":
        Console.WriteLine("좋은 성적입니다.");
        break;
    case "C":
        Console.WriteLine("보통 성적입니다.");
        break;
    default:
        Console.WriteLine("잘 모르겠습니다.");
        break;
}
*/
Console.ReadKey();