

Calculate calculate = new Calculate();
calculate.OnValueChanged += Calculate_OnValueChanged;

void Calculate_OnValueChanged(int result, string message)
{
    Console.WriteLine($"{message} - 현재 값 : {result}");
}

calculate.Plus(5);
calculate.Plus(3);
calculate.Minus(2);
calculate.Minus(10);

Console.ReadKey();

delegate void ValueChangeHandler(int result, string message);

class Calculate()
{
    public event ValueChangeHandler OnValueChanged;
    private int _value;

    public void Plus(int value)
    {
        _value += value;
        OnValueChanged(_value, $"{value}을 더했습니다.");
    }

    public void Minus(int value)
    {
        _value -= value;
        OnValueChanged(_value, $"{value}을 뻈습니다.");
    }
}

