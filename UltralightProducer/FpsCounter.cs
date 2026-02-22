using System;
using System.Diagnostics;

public class FpsCounter
{
    Stopwatch stopwatch = new Stopwatch();
    int frames = 0;
    double fps = 0;

    public FpsCounter()
    {
        stopwatch.Start();
    }

    public void Frame()
    {
        frames++;

        if (stopwatch.ElapsedMilliseconds >= 1000)
        {
            fps = frames * 1000.0 / stopwatch.ElapsedMilliseconds;
            frames = 0;
            stopwatch.Restart();

            Console.WriteLine($"FPS: {fps:0}");
        }
    }
}