using System;
using System.Threading;
public class Timer
{
    public event EventHandler Tick;
    private bool _isRunning;

    public void Start()
    {
        _isRunning = true;
        Thread timerThread = new Thread(() =>
        {
            while (_isRunning)
            {
                Thread.Sleep(1000); // Ждем 1 секунду
                OnTick();
            }
        });
        timerThread.IsBackground = true;
        timerThread.Start();
    }

    public void Stop()
    {
        _isRunning = false;
    }

    protected virtual void OnTick()
    {
        Tick?.Invoke(this, EventArgs.Empty);
    }
}
public class Clock
{
    public Clock(Timer timer)
    {
        timer.Tick += OnTick;
    }

    private void OnTick(object sender, EventArgs e)
    {
        Console.WriteLine($"Clock: Текущее время - {DateTime.Now:HH:mm:ss}");
    }
}
public class Counter
{
    private int _count;

    public Counter(Timer timer)
    {
        timer.Tick += OnTick;
    }

    private void OnTick(object sender, EventArgs e)
    {
        _count++;
        Console.WriteLine($"Counter: Текущее значение - {_count}");
    }

    public int GetCount() => _count;
}
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Демонстрация Timer, Clock и Counter ===");

        Timer timer = new Timer();
        Clock clock = new Clock(timer);
        Counter counter = new Counter(timer);

        timer.Start();
        Thread.Sleep(5000);

        timer.Stop();
        Console.WriteLine("Демонстрация завершена.");
    }
}