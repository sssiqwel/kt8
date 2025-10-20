using System;
using System.IO;
public class BankAccount
{
    private decimal _balance;

    public decimal Balance
    {
        get => _balance;
        private set
        {
            if (_balance != value)
            {
                _balance = value;
                BalanceChanged?.Invoke(_balance);
            }
        }
    }

    public event Action<decimal> BalanceChanged;

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Сумма должна быть положительной");

        Balance += amount;
        Console.WriteLine($"Внесено: {amount:C}. Новый баланс: {Balance:C}");
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Сумма должна быть положительной");
        if (amount > Balance)
            throw new InvalidOperationException("Недостаточно средств");

        Balance -= amount;
        Console.WriteLine($"Снято: {amount:C}. Новый баланс: {Balance:C}");
    }
}
public class Logger
{
    private readonly string _logFilePath;

    public Logger(string logFilePath)
    {
        _logFilePath = logFilePath;
        File.WriteAllText(_logFilePath, string.Empty);
    }

    public void SubscribeToAccount(BankAccount account)
    {
        account.BalanceChanged += OnBalanceChanged;
    }

    private void OnBalanceChanged(decimal newBalance)
    {
        string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Баланс изменен: {newBalance:C}";

        try
        {
            File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
            Console.WriteLine($"Logger: Запись в файл - {logEntry}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка записи в лог: {ex.Message}");
        }
    }
}
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Демонстрация BankAccount и Logger ===");

        BankAccount account = new BankAccount();
        Logger logger = new Logger("bank_log.txt");

        logger.SubscribeToAccount(account);
        account.Deposit(1000);
        account.Withdraw(200);
        account.Deposit(500);
        account.Withdraw(300);

        Console.WriteLine($"Финальный баланс: {account.Balance:C}");
        Console.WriteLine("Операции записаны в файл bank_log.txt");
    }
}