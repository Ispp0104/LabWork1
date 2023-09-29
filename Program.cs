//Task 5.1.1
int Sum(int a, int b)
    => a + b;

//Task 5.1.2
int GetDaysCount(int month, int year)
{
    if (month is > 13 or < 0 || year < 1)
        throw new Exception("Некоректные данные!");

    return month switch
    {
        1 or 3 or 5 or 7 or 8 or 10 or 12 => 31,
        4 or 6 or 9 or 11 => 30,
        2 => year % 400 == 0 || (year % 100 != 0 && year % 4 == 0) ? 29 : 28,
        _ => -1,
    };
}

//Task 5.2.1.a
var r1 = 2;
var r2 = 4;

if (r1 < 0 || r2 < 0)
    throw new Exception("Некоректные данные!");

var s = Math.Abs(Math.PI * r1 * r1 - Math.PI * r2 * r2);

//Task 5.2.1.b
var n = 8;

if (n < 0)
    throw new Exception("Некоректные данные!");

var sum = (1 + n) / 2 * n;

//Task 5.2.2
string GetSize(int bytes)
{
    if (bytes < 0)
        throw new Exception("Некоректные данные!");

    var unitNames = new[] { "Б", "КБ", "МБ", "ГБ" };
    double size = bytes;

    var unitIndex = 0;

    while (size >= 1024 && unitIndex < unitNames.Length - 1)
    {
        size /= 1024;
        unitIndex++;
    }

    return $"{size:F2} {unitNames[unitIndex]}";
}
Console.WriteLine(GetSize(237482));

//Task 5.2.3
double RecurseMultiply(double x, int n)
{
    if (n < 0)
        return -1;

    if (n == 0)
        return 1;
    
    var temp = RecurseMultiply(x, n / 2);

    return n % 2 == 0 ? temp * temp : x * temp * temp;
}

Console.WriteLine(RecurseMultiply(2,0));

//Task 5.3.1
async Task<double> Multiply(double a, int b)
{
    if (b == 0)
        return 1;

    if (b < 0)
    {
        a = 1 / a;
        b = -b;
    }

    var temp = a;

    for (int i = 1; i < b; i++)
    {
        await Task.Yield();
        a *= temp;
    }
        
    return a;
}
Console.WriteLine(await Multiply(2,8));

//Task 5.3.2
await Task.WhenAll(
    Task.Run(async () => Console.WriteLine(await Multiply(2,5))),
    Task.Run(async () => Console.WriteLine(await Multiply(3,3))),
    Task.Run(async () => Console.WriteLine(await Multiply(5,4)))
    );

//Task 5.3.3
async Task<double> GetCalcAsync(double a1, int x1, double a2, int x2, double a3, int x3, double a4, int x4)
{
    var numerator = await Multiply(a1, x1) + await Multiply(a2, x2);
    var denominator = await Multiply(a3, x3) - await Multiply(a4, x4);

    if (denominator == 0)
        throw new InvalidOperationException("Знаменатель не может быть равен нулю");

    return numerator / denominator;
}

Console.WriteLine(await GetCalcAsync(2, 2, 2, 2, 3, 3, 4, 4));


//Task 5.4

var cancel = new CancellationTokenSource();

async Task FileWrite(string name, int strFile, CancellationToken token = new())
{
    Console.WriteLine("Запись в файл начата...");

    await using StreamWriter writer = new(name, false);

    for (int i = 1; i <= strFile && !token.IsCancellationRequested; i++)
    {
        await writer.WriteLineAsync($"Число №{i}:{new Random().Next()}");
        await Task.Delay(250);
    }

    Console.WriteLine("Запись в файл закончена!");
}

await FileWrite("FileXXX.txt", 10);

try
{
    Task.WaitAll(
        Task.Run(async () => {
            await Task.Delay(4000);
            cancel.Cancel();
            Console.WriteLine("Все действующие операции отменены, истекло время ожидания!");
        }),
        Task.Run(async () => await FileWrite("FileX1.txt", 5, cancel.Token)),
        Task.Run(async () => await FileWrite("FileX2.txt", 100, cancel.Token)),
        Task.Run(async () => await FileWrite("FileX3.txt", 2, cancel.Token)));
}
catch
{
    Console.WriteLine("Исключение перехвачено!");
}

Console.WriteLine("Конец программы!");