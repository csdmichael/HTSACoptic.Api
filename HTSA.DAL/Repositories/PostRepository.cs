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
    public class PostRepository : IPostRepository
    {
        ILogger _logger;
        ApplContext _context;
        IPushRepository _pushRepository;

        public PostRepository(ApplContext context, ILogger<PostRepository> logger, IPushRepository pushRepository)
        {
            _logger = logger;
            _context = context;
            _pushRepository = pushRepository;
        }

        

        public async Task<BasicReponse> AddPost(string baseURL, string churchID, string personId, string postBody, string postImageName, string personName)
        {
            BasicReponse br = new BasicReponse();

            try
            {


               
                string sqlQuery;
                int paramCnt = 0;

                sqlQuery = @"exec  CHRCH.SP_Posts_Add";

                // @ChurchID char(10)
                //,@PostTitle nvarchar(1000)
                //,@PostBody nvarchar(max)
                //,@PostImageName nvarchar(512) = NULL
                //,@PostedBy VARCHAR(512)

                if (churchID != null && churchID.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @ChurchID = '" + churchID + "'";
                    paramCnt++;
                }

                if (postBody != null && postBody.Trim() != "")
                {
                    int titleLen = postBody.Length;
                    if (titleLen > 800) titleLen = 800;
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @PostTitle = N'" + postBody.Substring(0, titleLen) + "'";
                    paramCnt++;
                }

                if (postBody != null && postBody.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @PostBody = N'" + postBody + "'";
                    paramCnt++;
                }

                if (postImageName != null && postImageName.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @PostImageName = '" + postImageName + "'";
                    paramCnt++;
                }

                if (personId != null && personId.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @PostedBy = '" + personId + "'";
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
                        br.Message = "Incorrect Old Password!";
                    }
                }
                else
                {
                    br.IsSuccess = false;
                    br.Message = "Database Error!";
                }



                rdr.DbDataReader.Dispose();

                if (br.IsSuccess)
                {
                    //_pushRepository.SendTaggedPush("PersonId", personId, "New Post by " + personName, null, postBody);
                    _pushRepository.SendBroadcastPush("New Post by " + personName, null, postBody);
                }

            }
            catch (Exception ex)
            {
                br.IsSuccess = false;
                br.Message = ex.Message;
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='CHRCH.SP_Posts_Add'");
            }

            return br;
        }

        public async Task<BasicReponse> UpdatePost(string baseURL, int postId, string personId, string postBody, string postImageName, string personName)
        {
            BasicReponse br = new BasicReponse();

            try
            {



                string sqlQuery;
                int paramCnt = 0;

                sqlQuery = @"exec  CHRCH.SP_Posts_Update";

                // @ChurchID char(10)
                //,@PostTitle nvarchar(1000)
                //,@PostBody nvarchar(max)
                //,@PostImageName nvarchar(512) = NULL
                //,@PostedBy VARCHAR(512)

                if (postId > 0)
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @PostID = '" + postId + "'";
                    paramCnt++;
                }

                if (postBody != null && postBody.Trim() != "")
                {
                    int titleLen = postBody.Length;
                    if (titleLen > 800) titleLen = 800;
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @PostTitle = N'" + postBody.Substring(0, titleLen) + "'";
                    paramCnt++;
                }

                if (postBody != null && postBody.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @PostBody = N'" + postBody + "'";
                    paramCnt++;
                }

                if (postImageName != null && postImageName.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @PostImageName = '" + postImageName + "'";
                    paramCnt++;
                }

                if (personId != null && personId.Trim() != "")
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @PostedBy = '" + personId + "'";
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
                        br.Message = "Incorrect Old Password!";
                    }
                }
                else
                {
                    br.IsSuccess = false;
                    br.Message = "Database Error!";
                }



                rdr.DbDataReader.Dispose();

                if (br.IsSuccess)
                {
                    //_pushRepository.SendTaggedPush("PersonId", personId, "Updated Post by " + personName, null, postBody);
                    _pushRepository.SendBroadcastPush("New Post by " + personName, null, postBody);
                }

            }
            catch (Exception ex)
            {
                br.IsSuccess = false;
                br.Message = ex.Message;
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='CHRCH.SP_Posts_Update'");
            }

            return br;
        }

        public async Task<BasicReponse> DeletePost(string baseURL, int postId, string personId)
        {
            BasicReponse br = new BasicReponse();

            try
            {



                string sqlQuery;
                int paramCnt = 0;

                sqlQuery = @"exec  CHRCH.SP_Post_Delete";

                // @ChurchID char(10)
                //,@PostTitle nvarchar(1000)
                //,@PostBody nvarchar(max)
                //,@PostImageName nvarchar(512) = NULL
                //,@PostedBy VARCHAR(512)

                if (postId > 0)
                {
                    if (paramCnt > 0)
                        sqlQuery += ", ";
                    sqlQuery += " @PostID = '" + postId + "'";
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
                        br.Message = "Incorrect Old Password!";
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
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='CHRCH.SP_Post_Delete'");
            }

            return br;
        }

        public PostsResponse GetPosts(string baseURL, string churchID)
        {
            //----------------------------------------------------------
            //-- Michael Yaacoub - 10/06/2017
            //-- csdmichael@gmail.com
            //----------------------------------------------------------
            PostsResponse postsResp = new PostsResponse();
            var lstPosts = new List<Post>();

            try
            {
                
                //AuthResponse auth = _tokenInfoRepository.VerifyToken(token, "GetPosts", "");
                //postsResp.authResponse = auth;
                /*
                if (auth.success)
                {
                */
                string sqlQuery;

                sqlQuery = @"exec CHRCH.[SP_Posts_Get] @IsActive = 1";

                if (churchID != null && churchID.Trim() != "")
                {
                    sqlQuery += ", @ChurchID = '" + churchID + "'";
                }

                var rdr = _context.Database.ExecuteSqlQuery(sqlQuery);

                while (rdr.DbDataReader.Read())
                {
                    Post post = new Post();

                    post.PostID = System.Convert.ToInt32(rdr.DbDataReader["PostID"]);
                    post.PersonID = rdr.DbDataReader["PersonID"].ToString();
                    post.PersonName = rdr.DbDataReader["PersonName"].ToString();
                    post.PostBody = rdr.DbDataReader["PostBody"].ToString();
                    post.PostImageName = rdr.DbDataReader["PostImageName"].ToString();
                    post.PostTime = rdr.DbDataReader["PostTime"].ToString();

                    post.PictureName = rdr.DbDataReader["PictureName"].ToString().ToLower();


                    lstPosts.Add(post);
                }
                rdr.DbDataReader.Dispose();
                //}
            }
            catch (Exception ex)
            {
                _context.Database.ExecuteSqlQuery(@"
				exec [LOGS].[SP_LogApplicationMsg] @Type='ERROR', @Message='" + ex.Message + "', @Module='PostRepository.GetPosts'");
            }
            postsResp.PostsList = lstPosts;
            postsResp.PostsListCount = lstPosts.Count;
            return postsResp;
        }
    }
}
