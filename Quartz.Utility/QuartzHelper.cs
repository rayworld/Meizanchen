using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quartz.Utility
{
    public class QuartzHelper
    {
        static readonly IScheduler _scheduler;
        static QuartzHelper()
        {
            //创建一个工厂
            var schedulerFactory = new StdSchedulerFactory();
            //启动
            _scheduler = schedulerFactory.GetScheduler().Result;
            //1、开启调度
            _scheduler.Start();
        }
        /// <summary>
        /// 时间间隔执行任务
        /// </summary>
        /// <typeparam name="T">任务类，必须实现IJob接口</typeparam>
        /// <param name="seconds">时间间隔(单位：秒)</param>
        public static async Task<bool> ExecuteInterval<T>(int seconds) where T : IJob
        {
            //2、创建工作任务
            IJobDetail job = JobBuilder.Create<T>().Build();
            // 3、创建触发器
            ITrigger trigger = TriggerBuilder.Create()
                .StartNow()
                .WithSimpleSchedule(
                x => x.WithIntervalInSeconds(seconds)
                //x.WithIntervalInMinutes(1)
                .RepeatForever())
                .Build();
            //4、将任务加入到任务池
            await _scheduler.ScheduleJob(job, trigger);
            return true;
        }

        /// <summary>
        /// 指定时间执行任务
        /// </summary>
        /// <typeparam name="T">任务类，必须实现IJob接口</typeparam>
        /// <param name="cronExpression">cron表达式，即指定时间点的表达式</param>
        public static async Task<bool> ExecuteByCron<T>(string cronExpression) where T : IJob
        {
            //2、创建工作任务
            IJobDetail job = JobBuilder.Create<T>().Build();
            //3、创建触发器
            ICronTrigger trigger = (ICronTrigger)TriggerBuilder.Create()
                .StartNow()
                .WithCronSchedule(cronExpression)
                .Build();
            //4、将任务加入到任务池
            await _scheduler.ScheduleJob(job, trigger);
            return true;
        }
    }
}