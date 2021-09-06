using System;
using Topshelf;

namespace topshelf111
{
    public class TopshelfTest
    {
        readonly System.Timers.Timer _timer = new System.Timers.Timer();
        public TopshelfTest()
        {
            _timer.AutoReset = true;
            _timer.Interval = 1000;
            _timer.Elapsed += (sender, eventArgs) => { Run(); };
        }
        public void Start() { _timer.Start(); }
        public void Stop() { _timer.Stop(); }
        public static void Run()
        {
            Console.WriteLine("hello Topshelf");
        }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.RunAsLocalSystem();
                x.SetDescription("topshelf测试");
                x.SetDisplayName("topshelftest");
                x.SetServiceName("topshelftest");

                x.Service<TopshelfTest>(s =>
                {
                    s.ConstructUsing(name => new TopshelfTest());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
            });
        }
    }
}
