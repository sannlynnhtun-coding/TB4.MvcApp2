using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace TB4.MvcApp2.AjaxSample.Controllers
{
    public class BlogController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public BlogController(IConfiguration configuration) // Constructor Injection
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DbConnection")!;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List()
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            string query = @"SELECT [BlogId]
      ,[BlogTitle]
      ,[BlogAuthor]
      ,[BlogContent]
  FROM [dbo].[Tbl_Blog]
    Order by BlogId desc";
            var lst = db.Query<BlogModel>(query).ToList();
            return Json(lst);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Save(BlogModel requestModel)
        {
            //string id = Guid.NewGuid().ToString();
            string id = Ulid.NewUlid().ToString();
            requestModel.BlogId = id;
            using IDbConnection db = new SqlConnection(_connectionString);
            string query = @"INSERT INTO [dbo].[Tbl_Blog]
           ([BlogId]
           ,[BlogTitle]
           ,[BlogAuthor]
           ,[BlogContent])
     VALUES
           (@BlogId
           ,@BlogTitle
           ,@BlogAuthor
           ,@BlogContent)";
            var result = db.Execute(query, requestModel);
           return Json(new
           {
               Message = result > 0 ? "Saving successful." : "Saving failed.",
               IsSuccess = result > 0 
           });
        }

        public IActionResult Edit(string id)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            var item = db.QueryFirstOrDefault<BlogModel>("select * from Tbl_Blog where BlogId = @BlogId",
                new { BlogId = id });
            if (item is null)
            {
                string message = "No data found.";
                //ViewBag.Message = message;
                //ViewData["Message"] = message;  
                TempData["ErrorMessage"] = message;

                return Redirect("/Blog");
            }

            return View(item);
        }

        [HttpPost]
        public IActionResult Update(string id, BlogModel requestModel)
        {
            requestModel.BlogId = id;
            string query = @"UPDATE [dbo].[Tbl_Blog]
   SET [BlogTitle] = @BlogTitle
      ,[BlogAuthor] = @BlogAuthor
      ,[BlogContent] = @BlogContent
 WHERE BlogId = @BlogId";
            using IDbConnection db = new SqlConnection(_connectionString);
            var result = db.Execute(query, requestModel);

            return Json(new
            {
                Message = result > 0 ? "Updating successful." : "Updating failed.",
                IsSuccess = result > 0
            });
        }
    }

    public class BlogModel
    {
        public string BlogId { get; set; }
        public string BlogTitle { get; set; }
        public string BlogAuthor { get; set; }
        public string BlogContent { get; set; }
    }
}
