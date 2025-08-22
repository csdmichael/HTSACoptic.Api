using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HTSA.Models;
using HTSA.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HTSA.Website
{
    public partial class _Default : Page
    {
        IPostRepository posts;
        IList<Post> postsList;

        protected void Page_Load(object sender, EventArgs e)
        {
            string dbConn = ConfigurationManager.ConnectionStrings["CPConnection"].ToString();
            DbContextOptionsBuilder<ApplContext> optionsBuilder = new DbContextOptionsBuilder<ApplContext>();
            optionsBuilder.UseSqlServer(dbConn);

            ApplContext context = new ApplContext(optionsBuilder.Options);
            posts = new PostRepository(context, null, null);
            PostsResponse resp = new PostsResponse();

            string defChurch = ConfigurationManager.AppSettings["DefChurch"].ToString();

            resp = posts.GetPosts("", defChurch);

            postsList = resp.PostsList;

            string postsContainer = ConfigurationManager.AppSettings["PostsContainer"].ToString();
            string peopleContainer = ConfigurationManager.AppSettings["PeopleContainer"].ToString();

            foreach (Post p in postsList)
            {
                // Post Title
                //---------------
                TableRow rowPostTitle = new TableRow();

                Image personImage = new Image();
                personImage.ImageUrl = peopleContainer + p.PictureName;
                personImage.Width = 30;
                TableCell cellPostPerson = new TableCell();
                cellPostPerson.Controls.Add(personImage);
                rowPostTitle.Cells.Add(cellPostPerson);

                TableCell cellPostTitle = new TableCell();
                cellPostTitle.Text = "<b>" + p.PersonName + " @ " + p.PostTime + "</b>";
                rowPostTitle.Cells.Add(cellPostTitle);

                tblPosts.Rows.Add(rowPostTitle);

                // Post Body
                //---------------
                TableRow rowPostBody = new TableRow();
                TableCell cellPostBody = new TableCell();
                cellPostBody.Text = p.PostBody;
                cellPostBody.ColumnSpan = 2;
                rowPostBody.Cells.Add(cellPostBody);

                tblPosts.Rows.Add(rowPostBody);

                // Post Image
                //---------------
                TableRow rowPostImage = new TableRow();
                TableCell cellPostImage = new TableCell();
                Image postImage = new Image();
                postImage.ImageUrl = postsContainer + p.PostImageName;
                postImage.Width = 600;
                cellPostImage.Controls.Add(postImage);
                cellPostImage.ColumnSpan = 2;
                rowPostImage.Cells.Add(cellPostImage);

                tblPosts.Rows.Add(rowPostImage);

                // Separator
                //-----------
                TableRow rowPostSeparator = new TableRow();
                TableCell cellPostSeparator = new TableCell();
                cellPostSeparator.Text = "<br/>";
                cellPostSeparator.ColumnSpan = 2;
                rowPostSeparator.Cells.Add(cellPostSeparator);

                tblPosts.Rows.Add(rowPostSeparator);
            }
        }
    }
}