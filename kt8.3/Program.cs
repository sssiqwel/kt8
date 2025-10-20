using System;
using System.Collections.Generic;

public class Button
{
    private string _text;
    private EventHandler _click;
    private readonly List<EventHandler> _subscribers = new List<EventHandler>();
    private const int MAX_SUBSCRIBERS = 3;

    public string Text
    {
        get => _text;
        set => _text = value;
    }

    public Button(string text)
    {
        Text = text;
    }


    public event EventHandler Click
    {
        add
        {
            if (_subscribers.Count >= MAX_SUBSCRIBERS)
            {
                throw new InvalidOperationException($"Нельзя добавить более {MAX_SUBSCRIBERS} подписчиков");
            }

            if (_subscribers.Contains(value))
            {
                throw new InvalidOperationException("Этот подписчик уже добавлен");
            }

            _click += value;
            _subscribers.Add(value);
            Console.WriteLine($"Добавлен подписчик. Всего подписчиков: {_subscribers.Count}");
        }
        remove
        {
            _click -= value;
            _subscribers.Remove(value);
            Console.WriteLine($"Удален подписчик. Всего подписчиков: {_subscribers.Count}");
        }
    }

    public void SimulateClick()
    {
        Console.WriteLine($"\n--- Нажата кнопка: '{Text}' ---");
        OnClick();
    }

    protected virtual void OnClick()
    {
        _click?.Invoke(this, EventArgs.Empty);
    }
}
public static class ButtonHandlers
{
    public static void DisplayButtonText(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            Console.WriteLine($"Обработчик 1: Текст кнопки - '{button.Text}'");
        }
    }

    public static void ChangeTextColor(object sender, EventArgs e)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        if (sender is Button button)
        {
            Console.WriteLine($"Обработчик 2: Кнопка '{button.Text}' активна!");
        }
        Console.ResetColor();
    }

    public static void DisplayClickCount(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            Console.WriteLine($"Обработчик 3: Кнопка '{button.Text}' была нажата в {DateTime.Now:HH:mm:ss}");
        }
    }

    public static void AdditionalHandler(object sender, EventArgs e)
    {
        Console.WriteLine("Обработчик 4: Это дополнительный обработчик");
    }
}
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Демонстрация Button с ограничениями на подписчиков ===");

        Button button = new Button("Моя кнопка");

        try
        {

            button.Click += ButtonHandlers.DisplayButtonText;
            button.Click += ButtonHandlers.ChangeTextColor;
            button.Click += ButtonHandlers.DisplayClickCount;

            button.Click += ButtonHandlers.AdditionalHandler;
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        button.SimulateClick();
        button.SimulateClick();


        try
        {
            button.Click += ButtonHandlers.DisplayButtonText;
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        button.Click -= ButtonHandlers.ChangeTextColor;
        button.Click += ButtonHandlers.AdditionalHandler;


        button.SimulateClick();
    }
}