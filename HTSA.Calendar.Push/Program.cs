using HTSA.Models;
using HTSA.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace HTSA.Push
{
    class Program
    {
        static void Main(string[] args)
        {
            

            IConfiguration configuration = new ConfigurationBuilder().Build();
            ConfigValues.ReadConfigFile();


            //setup our DI
            var serviceProvider = new ServiceCollection()
                //.AddLogging()
                .AddSingleton<IPushRepository, PushRepository>()
                .AddSingleton<ICalendarRepository, CalendarRepository>()
                .AddDbContext<ApplContext>(options =>
                options.UseSqlServer(ConfigValues.dbConn))
                .BuildServiceProvider();

            var pushRepository = serviceProvider.GetService<IPushRepository>();
            var calendarRepository = serviceProvider.GetService<ICalendarRepository>();

            Console.WriteLine("Hello Push Notifications!");

            try
            {
                List<CalEvent> evts = null;
                string msgTitle = "";

                if (DateTime.Now.Hour > 0 && DateTime.Now.Hour < 12)
                {
                    evts = calendarRepository.GetEventsFromDB_Today();
                    msgTitle = "+ Church Events Today +";
                }
                else
                {
                    evts = calendarRepository.GetEventsFromDB_Tomorrow();
                    msgTitle = "+ Church Events Tomorrow +";
                }

                string msgContent = "";
                foreach (CalEvent evt in evts)
                {
                    msgContent += evt.StartTime + " - " + evt.Summary + "\r\n";
                }

                if (String.IsNullOrWhiteSpace(msgContent))
                    msgContent = "No events planned. Lord bless your day!";

                pushRepository.SendBroadcastPush(msgTitle, "", msgContent);

                Console.WriteLine(msgTitle);
                Console.WriteLine(msgContent);

                // Console.ReadLine();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
