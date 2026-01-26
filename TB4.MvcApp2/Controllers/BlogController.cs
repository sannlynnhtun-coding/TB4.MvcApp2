using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace TB4.MvcApp2.Controllers
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

        // read
        // hardcode
        public IActionResult Index()
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            string query = @"SELECT [BlogId]
      ,[BlogTitle]
      ,[BlogAuthor]
      ,[BlogContent]
  FROM [dbo].[Tbl_Blog]
    Order by BlogId desc";
            var lst = db.Query<BlogModel>(query).ToList();

            return View(lst);
        }

        public IActionResult Create()
        {
            return View();
        }

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
            return Redirect("/Blog");
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
