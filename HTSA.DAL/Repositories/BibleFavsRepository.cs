using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using HTSA.Models;
using System;
using HTSA.DAL.Models;
using System.Threading.Tasks;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Repositories
{
    public class BibleFavsRepository : IBibleFavsRepository
    {
        ILogger _logger;
        ApplContext _context;

        public BibleFavsRepository(ApplContext context, ILogger<BibleFavsRepository> logger)
        {
            _logger = logger;
            _context = context;
        }




        public FavVersesResponse GetMyFavVerses(string baseURL, string personId)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub 
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            FavVersesResponse favversesResp = new FavVersesResponse();
            var lstFavVerses = new List<FavVerse>();

            try
            {

                string sqlQuery;

                sqlQuery = @"exec BIB.SP_Fav_Verses_Get";

                if (personId != null && personId.Trim() != "")
                {
                    sqlQuery += " @PersonId = '" + personId + "'";
                }

                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                while (rdr.DbDataReader.Read())
                {
                    FavVerse fv = new FavVerse();

                    fv.VerseID = rdr.DbDataReader["VerseID"].ToString();
                    fv.ChapterNum = rdr.DbDataReader["ChapterNum"].ToString();
                    fv.VerseNum = rdr.DbDataReader["VerseNum"].ToString();
                    fv.Verse = rdr.DbDataReader["Verse"].ToString();
                    fv.VerseArabic = rdr.DbDataReader["VerseArabic"].ToString();
                    fv.Testament = rdr.DbDataReader["Testament"].ToString();
                    fv.BookName = rdr.DbDataReader["BookName"].ToString();
                    fv.BookNameArabic = rdr.DbDataReader["BookNameArabic"].ToString();
                    fv.PersonID = rdr.DbDataReader["PersonID"].ToString();
                    fv.TotalSentCount = System.Convert.ToInt32(rdr.DbDataReader["SentCount"].ToString());
                    fv.Created = rdr.DbDataReader["Created"].ToString();


                    lstFavVerses.Add(fv);
                }
                rdr.DbDataReader.Dispose();
                //}
            }
            catch (Exception ex)
            {
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='BIB.SP_Fav_Verses_Get'");
            }
            favversesResp.FavVersesList = lstFavVerses;

            return favversesResp;

        }

        public FavVersesResponse GetAllFavVerses(string baseURL)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub 
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            FavVersesResponse favversesResp = new FavVersesResponse();
            var lstFavVerses = new List<FavVerse>();

            try
            {

                string sqlQuery;

                sqlQuery = @"exec BIB.SP_Fav_Verses_All_Get";

                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                while (rdr.DbDataReader.Read())
                {
                    FavVerse fv = new FavVerse();

                    fv.VerseID = rdr.DbDataReader["VerseID"].ToString();
                    fv.ChapterNum = rdr.DbDataReader["ChapterNum"].ToString();
                    fv.VerseNum = rdr.DbDataReader["VerseNum"].ToString();
                    fv.Verse = rdr.DbDataReader["Verse"].ToString();
                    fv.VerseArabic = rdr.DbDataReader["VerseArabic"].ToString();
                    fv.Testament = rdr.DbDataReader["Testament"].ToString();
                    fv.BookName = rdr.DbDataReader["BookName"].ToString();
                    fv.BookNameArabic = rdr.DbDataReader["BookNameArabic"].ToString();
                    fv.FavBy = System.Convert.ToInt32(rdr.DbDataReader["FavBy"].ToString());
                    fv.TotalSentCount = System.Convert.ToInt32(rdr.DbDataReader["TotalSentCount"].ToString());
                    fv.PersonIds = rdr.DbDataReader["PersonIds"].ToString();
                    fv.PersonDisplayNames = rdr.DbDataReader["PersonDisplayNames"].ToString();
                    fv.PersonPics = rdr.DbDataReader["PersonPics"].ToString();


                    lstFavVerses.Add(fv);
                }
                rdr.DbDataReader.Dispose();
                //}
            }
            catch (Exception ex)
            {
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='BIB.SP_Fav_Verses_All_Get'");
            }
            favversesResp.FavVersesList = lstFavVerses;

            return favversesResp;
        }

        public async Task<BasicReponse> AddVerseToFavorites(string baseURL, string verseId, string personId)
        {
            BasicReponse br = new BasicReponse();

            try
            {

                string sqlQuery;
                int paramCnt = 0;

                sqlQuery = @"exec BIB.SP_Fav_Verse_Add";



                if (verseId != null && verseId.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @VerseId = '" + verseId + "'";
                    paramCnt++;
                }

                if (personId != null && personId.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @PersonId = '" + personId + "'";
                    paramCnt++;
                }

                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                if (rdr.DbDataReader.HasRows)
                {
                    rdr.DbDataReader.Read();

                    if (rdr.DbDataReader["IsSuccess"].ToString() == "1")
                    {
                        br.IsSuccess = true;
                    }
                    else
                    {
                        br.IsSuccess = false;
                        br.Message = "Database Error!";
                    }
                }
                else
                {
                    br.IsSuccess = false;
                    br.Message = "Database Error!";
                }



                rdr.DbDataReader.Dispose();


            }
            catch (Exception ex)
            {
                br.IsSuccess = false;
                br.Message = ex.Message;
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='BIB.SP_Fav_Verse_Add'");
            }

            return br;
        }

        public async Task<BasicReponse> RemoveVerseFromFavorites(string baseURL, string verseId, string personId)
        {
            BasicReponse br = new BasicReponse();

            try
            {

                string sqlQuery;
                int paramCnt = 0;

                sqlQuery = @"exec BIB.SP_Fav_Verse_Delete";



                if (verseId != null && verseId.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @VerseId = '" + verseId + "'";
                    paramCnt++;
                }

                if (personId != null && personId.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @PersonId = '" + personId + "'";
                    paramCnt++;
                }

                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                if (rdr.DbDataReader.HasRows)
                {
                    rdr.DbDataReader.Read();

                    if (rdr.DbDataReader["IsSuccess"].ToString() == "1")
                    {
                        br.IsSuccess = true;
                    }
                    else
                    {
                        br.IsSuccess = false;
                        br.Message = "Database Error!";
                    }
                }
                else
                {
                    br.IsSuccess = false;
                    br.Message = "Database Error!";
                }



                rdr.DbDataReader.Dispose();


            }
            catch (Exception ex)
            {
                br.IsSuccess = false;
                br.Message = ex.Message;
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='BIB.SP_Fav_Verse_Delete'");
            }

            return br;
        }


        public BasicReponse IncrementSentCount(string baseURL, string verseId)
        {
            BasicReponse br = new BasicReponse();

            try
            {

                string sqlQuery;
                int paramCnt = 0;

                sqlQuery = @"exec BIB.SP_Fav_Verse_Sent_Incr";



                if (verseId != null && verseId.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @VerseId = '" + verseId + "'";
                    paramCnt++;
                }

                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                if (rdr.DbDataReader.HasRows)
                {
                    rdr.DbDataReader.Read();

                    if (rdr.DbDataReader["IsSuccess"].ToString() == "1")
                    {
                        br.IsSuccess = true;
                    }
                    else
                    {
                        br.IsSuccess = false;
                        br.Message = "Database Error!";
                    }
                }
                else
                {
                    br.IsSuccess = false;
                    br.Message = "Database Error!";
                }

                rdr.DbDataReader.Dispose();
                
            }
            catch (Exception ex)
            {
                br.IsSuccess = false;
                br.Message = ex.Message;
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='BIB.SP_Fav_Verse_Sent_Incr'");
            }

            return br;
        }
    }

}
