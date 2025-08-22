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

            // Program.TestPush();
            // Program.GetBibleHierarchy();


        }

        public static void GetBibleHierarchy()
        {
            var serviceProvider = new ServiceCollection()
                //.AddLogging()
                .AddSingleton<IBibleRepository, BibleRepository>()
                .AddDbContext<ApplContext>(options =>
                options.UseSqlServer(ConfigValues.dbConn))
                .BuildServiceProvider();

            var bibleRepository = serviceProvider.GetService<IBibleRepository>();

            BibleHierarchy hier = bibleRepository.GetBibleHierarchy();

            string bibleHier = Newtonsoft.Json.JsonConvert.SerializeObject(hier);

            //Console.WriteLine(bibleHier);
            //Console.ReadLine();
            System.IO.File.WriteAllText("C:\\Projects\\New\\CP_Database\\Data\\ArabicBible.json", bibleHier);
        }

        public static void TestPush()
        {
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
                string msgTitle = "+ Test Tagged Push +";

                string msgContent = "Test Tagged Push Notifications";

                pushRepository.SendTaggedPush("PersonId", "a6aa9acc-d34f-4ad2-aacb-f2bb7fc301ec", msgTitle, "", msgContent);

                Console.WriteLine(msgTitle);
                Console.WriteLine(msgContent);

                // Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
