using Quartz.JobWork;
using Quartz.Utility;
using System;
using Topshelf;

namespace Meizanchen
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new QuartzCronForm());


            string cronExpression = "0/10 * * * * ? ";  //=>这是指每天的9点和16点执行任务
            //cronExpression = "30 0/1 * * * ?";

            //=>这是调用Cron计划方法
            QuartzHelper.ExecuteByCron<MyJob>(cronExpression).Wait();
            //QuartzHelper.ExecuteInterval<MyJob>(180} Wait();
            /*
             简单说一下Cron表达式吧，

            由7段构成：秒 分 时 日 月 星期 年（可选）

            "-" ：表示范围  MON-WED表示星期一到星期三
            "," ：表示列举 MON,WEB表示星期一和星期三
            "*" ：表是“每”，每月，每天，每周，每年等
            "/" :表示增量：0/15（处于分钟段里面） 每15分钟，在0分以后开始，3/20 每20分钟，从3分钟以后开始
            "?" :只能出现在日，星期段里面，表示不指定具体的值
            "L" :只能出现在日，星期段里面，是Last的缩写，一个月的最后一天，一个星期的最后一天（星期六）
            "W" :表示工作日，距离给定值最近的工作日
            "#" :表示一个月的第几个星期几，例如："6#3"表示每个月的第三个星期五（1=SUN...6=FRI,7=SAT）

            如果Minutes的数值是 '0/15' ，表示从0开始每15分钟执行

            如果Minutes的数值是 '3/20' ，表示从3开始每20分钟执行，也就是‘3/23/43’
            */
            HostFactory.Run(x =>
            {
                //x.UseLog4Net();
                x.Service<TownCrier>(s =>
                {
                    s.ConstructUsing(name => new TownCrier());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();
                x.SetDescription("QuartzJob任务定时发送");
                x.SetDisplayName("QuartzJob");
                x.SetServiceName("QuartzJob");

                x.EnablePauseAndContinue();
            });
        }
    }
    public class TownCrier
    {
        //Timer不要声明成局部变量，否则会被GC回收
        private readonly System.Timers.Timer _timer = new System.Timers.Timer(1000);

        public TownCrier()
        {
            _timer.AutoReset = true;
            _timer.Interval = 1000;
            _timer.Elapsed += (sender, eventArgs) => Console.WriteLine("---------------------DateTime： {0} ------------------- ", DateTime.Now);
        }
        public void Start() 
        {
            _timer.Start(); 
        }
        public void Stop() 
        {
            _timer.Stop(); 
        }
    }
}
