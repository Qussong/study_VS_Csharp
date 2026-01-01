


// 가변 배열 선언 (각 배열의 크기가 다를 수 있음)
int[][] jaggedArray = new int[3][];

// 각 행에 다른 크기의 배열을 할당
jaggedArray[0] = new int[] { 1, 2 };
jaggedArray[1] = new int[] { 3, 4, 5 };
jaggedArray[2] = new int[] { 6, 7, 8, 9 };

for(int row = 0; row < jaggedArray.GetLength(0); row++)
{
    for(int col = 0; col < jaggedArray[row].Length; ++col)
    {
        Console.Write(jaggedArray[row][col] + " ");
    }
    Console.WriteLine();
}

/* 1 2
 * 3 4 5
 * 6 7 8 9*/
Console.ReadKey();