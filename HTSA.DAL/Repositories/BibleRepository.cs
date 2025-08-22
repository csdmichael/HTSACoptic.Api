using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using HTSA.Models;
using System;
//----------------------------------------------------------
//-- Author: Michael Yaacoub
//-- Email: csdmichael@gmail.com
//----------------------------------------------------------

namespace HTSA.Repositories
{
    public class BibleRepository : IBibleRepository
    {
        ILogger _logger;
        ApplContext _context;

        public BibleRepository(ApplContext context, ILogger<BibleRepository> logger)
        {
            _logger = logger;
            _context = context;
        }

        public BibleResponse GetBooks(string baseURL, string token, string Keyword, string BookID, string BookName, string BookCode, string Testament)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            BibleResponse bibleResp = new BibleResponse();
            var lstBibleObjs = new List<BibleObj>();

            try
            {

                string sqlQuery;

                sqlQuery = @"exec BIB.[SP_Books_Get]";
                int paramCnt = 0;

                if (Keyword != null && Keyword.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @Keyword = N'" + Keyword + "'";
                    paramCnt++;
                }

                if (BookID != null && BookID.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @BookID = '" + BookID + "'";
                    paramCnt++;
                }

                if (BookName != null && BookName.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @BookName = '" + BookName + "'";
                    paramCnt++;
                }

                if (BookCode != null && BookCode.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @BookCode = '" + BookCode + "'";
                    paramCnt++;
                }

                if (Testament != null && Testament.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @Testament = '" + Testament + "'";
                    paramCnt++;
                }

                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                while (rdr.DbDataReader.Read())
                {
                    BibleObj bo = new BibleObj();

                    bo.BookCode = rdr.DbDataReader["BookCode"].ToString();
                    bo.BookID = rdr.DbDataReader["BookID"].ToString();
                    bo.BookName = rdr.DbDataReader["BookName"].ToString();
                    bo.BookNameArabic = rdr.DbDataReader["BookNameArabic"].ToString();
                    bo.CountChapters = rdr.DbDataReader["CountChapters"].ToString();
                    bo.CountVerses = rdr.DbDataReader["CountVerses"].ToString();
                    bo.Testament = rdr.DbDataReader["Testament"].ToString();
                    bo.TestamentArabic = rdr.DbDataReader["TestamentArabic"].ToString();

                    lstBibleObjs.Add(bo);
                }
                rdr.DbDataReader.Dispose();

            }
            catch (Exception ex)
            {
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='BibleRepository.GetBooks'");
            }
            bibleResp.BibleObjsList = lstBibleObjs;
            bibleResp.BibleObjsListCount = lstBibleObjs.Count;
            return bibleResp;
        }


        public BibleResponse GetChapters(string baseURL, string token, string Keyword, string BookID, string BookName, string BookCode, string Testament, string ChapterNum)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            BibleResponse bibleResp = new BibleResponse();
            var lstBibleObjs = new List<BibleObj>();

            try
            {
                string sqlQuery;

                sqlQuery = @"exec BIB.[SP_Chapters_Get]";
                int paramCnt = 0;

                if (Keyword != null && Keyword.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @Keyword = N'" + Keyword + "'";
                    paramCnt++;
                }

                if (BookID != null && BookID.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @BookID = '" + BookID + "'";
                    paramCnt++;
                }

                if (BookName != null && BookName.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @BookName = '" + BookName + "'";
                    paramCnt++;
                }

                if (BookCode != null && BookCode.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @BookCode = '" + BookCode + "'";
                    paramCnt++;
                }

                if (Testament != null && Testament.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @Testament = '" + Testament + "'";
                    paramCnt++;
                }

                if (ChapterNum != null && ChapterNum.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @ChapterNum = '" + ChapterNum + "'";
                    paramCnt++;
                }

                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                while (rdr.DbDataReader.Read())
                {
                    BibleObj bo = new BibleObj();

                    bo.BookCode = rdr.DbDataReader["BookCode"].ToString();
                    bo.BookID = rdr.DbDataReader["BookID"].ToString();
                    bo.BookName = rdr.DbDataReader["BookName"].ToString();
                    bo.BookNameArabic = rdr.DbDataReader["BookNameArabic"].ToString();
                    bo.ChapterNum = rdr.DbDataReader["ChapterNum"].ToString();
                    bo.CountVerses = rdr.DbDataReader["CountVerses"].ToString();
                    bo.Testament = rdr.DbDataReader["Testament"].ToString();
                    bo.TestamentArabic = rdr.DbDataReader["TestamentArabic"].ToString();



                    lstBibleObjs.Add(bo);
                }
                rdr.DbDataReader.Dispose();

            }
            catch (Exception ex)
            {
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='BibleRepository.GetChapters'");
            }
            bibleResp.BibleObjsList = lstBibleObjs;
            bibleResp.BibleObjsListCount = lstBibleObjs.Count;
            return bibleResp;
        }

        public BibleResponse GetVerses(string baseURL, string token, string Keyword, string BookID, string BookName, string BookCode, string Testament, string ChapterNum, string VerseID)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            BibleResponse bibleResp = new BibleResponse();
            var lstBibleObjs = new List<BibleObj>();

            try
            {
                string sqlQuery;

                sqlQuery = @"exec BIB.[SP_Verses_Get]";
                int paramCnt = 0;

                if (Keyword != null && Keyword.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @Keyword = N'" + Keyword + "'";
                    paramCnt++;
                }

                if (BookID != null && BookID.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @BookID = '" + BookID + "'";
                    paramCnt++;
                }

                if (BookName != null && BookName.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @BookName = '" + BookName + "'";
                    paramCnt++;
                }

                if (BookCode != null && BookCode.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @BookCode = '" + BookCode + "'";
                    paramCnt++;
                }

                if (Testament != null && Testament.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @Testament = '" + Testament + "'";
                    paramCnt++;
                }

                if (ChapterNum != null && ChapterNum.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @ChapterNum = '" + ChapterNum + "'";
                    paramCnt++;
                }

                if (VerseID != null && VerseID.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @VerseID = '" + VerseID + "'";
                    paramCnt++;
                }

                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                while (rdr.DbDataReader.Read())
                {
                    BibleObj bo = new BibleObj();

                    bo.BookCode = rdr.DbDataReader["BookCode"].ToString();
                    bo.BookID = rdr.DbDataReader["BookID"].ToString();
                    bo.BookName = rdr.DbDataReader["BookName"].ToString();
                    bo.BookNameArabic = rdr.DbDataReader["BookNameArabic"].ToString();
                    bo.ChapterNum = rdr.DbDataReader["ChapterNum"].ToString();
                    bo.Testament = rdr.DbDataReader["Testament"].ToString();
                    bo.TestamentArabic = rdr.DbDataReader["TestamentArabic"].ToString();
                    bo.Verse = rdr.DbDataReader["Verse"].ToString();
                    bo.VerseArabic = rdr.DbDataReader["VerseArabic"].ToString();
                    bo.VerseID = rdr.DbDataReader["VerseID"].ToString();
                    bo.VerseNum = rdr.DbDataReader["VerseNum"].ToString();


                    lstBibleObjs.Add(bo);
                }
                rdr.DbDataReader.Dispose();

            }
            catch (Exception ex)
            {
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='BibleRepository.GetVerses'");
            }
            bibleResp.BibleObjsList = lstBibleObjs;
            bibleResp.BibleObjsListCount = lstBibleObjs.Count;
            return bibleResp;
        }

        public BibleResponse GetVersesWithFavs(string baseURL, string token, string Keyword, string BookID, string BookName, string BookCode, string Testament, string ChapterNum, string VerseID, string personId)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            BibleResponse bibleResp = new BibleResponse();
            var lstBibleObjs = new List<BibleObj>();

            try
            {
                string sqlQuery;

                sqlQuery = @"exec BIB.[SP_VersesFavs_Get]";
                int paramCnt = 0;

                if (Keyword != null && Keyword.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @Keyword = N'" + Keyword + "'";
                    paramCnt++;
                }

                if (BookID != null && BookID.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @BookID = '" + BookID + "'";
                    paramCnt++;
                }

                if (BookName != null && BookName.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @BookName = '" + BookName + "'";
                    paramCnt++;
                }

                if (BookCode != null && BookCode.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @BookCode = '" + BookCode + "'";
                    paramCnt++;
                }

                if (Testament != null && Testament.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @Testament = '" + Testament + "'";
                    paramCnt++;
                }

                if (ChapterNum != null && ChapterNum.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @ChapterNum = '" + ChapterNum + "'";
                    paramCnt++;
                }

                if (VerseID != null && VerseID.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @VerseID = '" + VerseID + "'";
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

                while (rdr.DbDataReader.Read())
                {
                    BibleObj bo = new BibleObj();

                    bo.BookCode = rdr.DbDataReader["BookCode"].ToString();
                    bo.BookID = rdr.DbDataReader["BookID"].ToString();
                    bo.BookName = rdr.DbDataReader["BookName"].ToString();
                    bo.BookNameArabic = rdr.DbDataReader["BookNameArabic"].ToString();
                    bo.ChapterNum = rdr.DbDataReader["ChapterNum"].ToString();
                    bo.Testament = rdr.DbDataReader["Testament"].ToString();
                    bo.TestamentArabic = rdr.DbDataReader["TestamentArabic"].ToString();
                    bo.Verse = rdr.DbDataReader["Verse"].ToString();
                    bo.VerseArabic = rdr.DbDataReader["VerseArabic"].ToString();
                    bo.VerseID = rdr.DbDataReader["VerseID"].ToString();
                    bo.VerseNum = rdr.DbDataReader["VerseNum"].ToString();
                    bo.IsFavVerse = (rdr.DbDataReader["IsFavVerse"].ToString() == "1" ? true : false);


                    lstBibleObjs.Add(bo);
                }
                rdr.DbDataReader.Dispose();

            }
            catch (Exception ex)
            {
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='BibleRepository.GetVerses'");
            }
            bibleResp.BibleObjsList = lstBibleObjs;
            bibleResp.BibleObjsListCount = lstBibleObjs.Count;
            return bibleResp;
        }

        public BibleHierarchy GetBibleHierarchy()
        {
            BibleHierarchy hier = new BibleHierarchy();

            BibleResponse bibleBooks = this.GetBooks("", "", "", null, null, null, null);

            foreach (BibleObj bibleBook in bibleBooks.BibleObjsList)
            {
                BibleBook book = new BibleBook();

                book.BookCode = bibleBook.BookCode;
                book.BookID = bibleBook.BookID;
                book.BookName = bibleBook.BookName;
                book.BookNameArabic = bibleBook.BookNameArabic;
                book.Testament = bibleBook.Testament;
                book.TestamentArabic = bibleBook.TestamentArabic;

                

                BibleResponse bibleChaps = this.GetChapters("", "", "", book.BookID, book.BookName, book.BookCode, book.Testament, "");

                foreach (BibleObj bibleChap in bibleChaps.BibleObjsList)
                {
                    BibleChapter chap = new BibleChapter();

                    chap.ChapterNum = bibleChap.ChapterNum;

                    BibleResponse bibleVerses = this.GetVerses("", "", "", "", book.BookName, book.BookCode, book.Testament, chap.ChapterNum, null);

                    foreach (BibleObj bibleVerse in bibleVerses.BibleObjsList)
                    {
                        BibleVerse ver = new BibleVerse();

                        ver.VerseID = bibleVerse.VerseID;
                        ver.VerseNum = bibleVerse.VerseNum;
                        ver.Verse = bibleVerse.Verse;
                        ver.VerseArabic = bibleVerse.VerseArabic;


                        chap.VersesList.Add(ver);
                    }

                    book.ChaptersList.Add(chap);
                }

                hier.BooksList.Add(book);
            }

            return hier;
        } 

    }
}
