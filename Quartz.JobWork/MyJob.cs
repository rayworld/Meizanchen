using Quartz.Utility;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Quartz.JobWork
{
    public class MyJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Task task = null;
            try
            {
                string fileName = "printlog.txt";
                StreamWriter writer = new StreamWriter(fileName, true);
                task = writer.WriteLineAsync(string.Format("{0},测试", DateTime.Now.ToLongTimeString()));
                writer.Close();
                writer.Dispose();
            }
            catch (Exception)
            {
                //LogHelper.WriteLog(ex.Message.ToString(), ex);
            }
            return task;
        }
    }
}