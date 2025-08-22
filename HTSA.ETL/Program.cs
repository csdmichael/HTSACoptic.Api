using System;
using HTSA.Repositories;
using HTSA.Models;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using Microsoft.EntityFrameworkCore;

namespace HTSA.ETL
{
    class Program
    {
        
        static void Main(string[] args)
        {
            
            try
            {
                IConfiguration configuration = new ConfigurationBuilder()
                                          .Build();
                ConfigValues.ReadConfigFile();


                //setup our DI
                var serviceProvider = new ServiceCollection()
                    //.AddLogging()
                    .AddSingleton<ICalendarRepository, CalendarRepository>()
                    .AddDbContext<ApplContext>(options =>
                    options.UseSqlServer(ConfigValues.dbConn))
                    .BuildServiceProvider();

                var calendarRepository = serviceProvider.GetService<ICalendarRepository>();

                Console.WriteLine("HTSA ETL started!");

                List<CalEvent> lstEvents = calendarRepository.GetEvents("htsacalendar@gmail.com");

                if (lstEvents != null)
                {
                    Console.WriteLine(lstEvents.Count.ToString() + " Events loaded!");

                    if (lstEvents.Count > 0)
                    {
                        bool isSuccess = calendarRepository.DataRefreshEvents(lstEvents);

                        if (isSuccess)
                            Console.WriteLine("Data Refresh succeeded!");
                        else
                            Console.WriteLine("Data Refresh failed!");

                    }
                }
                Console.WriteLine("HTSA ETL finished!");
                Console.ReadLine();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
