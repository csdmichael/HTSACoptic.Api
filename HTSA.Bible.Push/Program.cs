using HTSA.DAL.Models;
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
                .AddSingleton<IBibleFavsRepository, BibleFavsRepository>()
                .AddDbContext<ApplContext>(options =>
                options.UseSqlServer(ConfigValues.dbConn))
                .BuildServiceProvider();

            var pushRepository = serviceProvider.GetService<IPushRepository>();
            var bibleFavsRepository = serviceProvider.GetService<IBibleFavsRepository>();

            Console.WriteLine("Hello Push Notifications!");

            try
            {
                List<FavVerse> favVerses = null;
                string msgTitle = "", msgSubTitle = "";

                favVerses = bibleFavsRepository.GetAllFavVerses("").FavVersesList;
                msgTitle = "+ Bible Verse of today آية اليوم +";
                msgSubTitle = favVerses[0].BookName + " " + favVerses[0].ChapterNum + ":" + favVerses[0].VerseNum + " " + favVerses[0].BookNameArabic;

                string msgContent = "";
                msgContent += favVerses[0].Verse + "\r\n";
                msgContent += favVerses[0].VerseArabic + "\r\n";
                msgContent += "Chosen By: " + favVerses[0].PersonDisplayNames.Replace(",", ", ") + "\r\n";
                msgContent += "Note: Pick your favorite bible verse(s) by clicking the star next to any verse in the bible. "
                    + "The most chosen verses get sent each day...";

                //BasicReponse br = pushRepository.SendTaggedPush("PersonId", "137E3711-D6E5-40CD-B8B4-7A4EDDDE91AC", msgTitle, msgSubTitle, msgContent);
                BasicReponse br = pushRepository.SendBroadcastPush(msgTitle, msgSubTitle, msgContent);

                if (br.IsSuccess)
                {
                    br = bibleFavsRepository.IncrementSentCount("", favVerses[0].VerseID);
                }

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
