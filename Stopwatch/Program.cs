using System;
using System.Threading;

public class Stopwatch
{
    public delegate void StopwatchEventHandler(string message);

    // Events
    public event StopwatchEventHandler OnStarted;
    public event StopwatchEventHandler OnStopped;
    public event StopwatchEventHandler OnReset;

    // Fields
    private TimeSpan timeElapsed;
    private bool isRunning;
    private Timer timer;

    // Properties
    public TimeSpan TimeElapsed => timeElapsed;
    public bool IsRunning => isRunning;

    // Constructor
    public Stopwatch()
    {
        timeElapsed = TimeSpan.Zero;
        isRunning = false;
        timer = new Timer(Tick, null, Timeout.Infinite, 1000); // Timer is set to tick every second
    }

    // Methods
    public void Start()
    {
        if (!isRunning)
        {
            isRunning = true;
            OnStarted?.Invoke("Stopwatch Started!");
            timeElapsed = TimeSpan.Zero; // Reset the time when starting
            timer.Change(0, 1000); // Start the timer immediately
        }
    }

    public void Stop()
    {
        if (isRunning)
        {
            isRunning = false;
            OnStopped?.Invoke($"Stopwatch Stopped! Time elapsed: {timeElapsed}");
            timer.Change(Timeout.Infinite, Timeout.Infinite); // Stop the timer
        }
    }

    public void Reset()
    {
        timeElapsed = TimeSpan.Zero;
        if (isRunning)
        {
            OnStopped?.Invoke($"Stopwatch Stopped! Time elapsed: {timeElapsed}");
            timer.Change(Timeout.Infinite, Timeout.Infinite); // Stop the timer
            isRunning = false;
        }
        OnReset?.Invoke("Stopwatch Reset!");
    }

    private void Tick(object state)
    {
        if (isRunning)
        {
            timeElapsed = timeElapsed.Add(TimeSpan.FromSeconds(1)); // Increment time by 1 second
        }
    }
}

class Program
{
    static void Main()
    {
        Stopwatch stopwatch = new Stopwatch();

        // Subscribe to events
        stopwatch.OnStarted += HandleStarted;
        stopwatch.OnStopped += HandleStopped;
        stopwatch.OnReset += HandleReset;

        bool running = true;

        while (running)
        {
            Console.Clear();
            Console.WriteLine("Stopwatch Console");
            Console.WriteLine("Press S to start, T to stop, R to reset, Q to quit.");
            Console.WriteLine($"Time Elapsed: {stopwatch.TimeElapsed}");
            Console.WriteLine($"Stopwatch is {(stopwatch.IsRunning ? "Running" : "Stopped")}.");
            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.S:
                    stopwatch.Start();
                    break;
                case ConsoleKey.T:
                    stopwatch.Stop();
                    break;
                case ConsoleKey.R:
                    stopwatch.Reset();
                    break;
                case ConsoleKey.Q:
                    running = false;
                    break;
            }

            // Wait for 1 second to simulate real-time updates
            System.Threading.Thread.Sleep(1000);
        }
    }

    // Event Handlers
    static void HandleStarted(string message)
    {
        Console.WriteLine(message);
    }

    static void HandleStopped(string message)
    {
        Console.WriteLine(message);
    }

    static void HandleReset(string message)
    {
        Console.WriteLine(message);
    }
}

