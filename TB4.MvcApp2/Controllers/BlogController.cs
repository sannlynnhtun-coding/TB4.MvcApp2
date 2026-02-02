using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections;
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

        public IActionResult Generate()
        {
            List<BlogModel> blogs = new List<BlogModel>
{
    new BlogModel
    {
        BlogId = "B001",
        BlogTitle = "Introduction to SQL",
        BlogAuthor = "Admin",
        BlogContent = "This blog explains the basics of SQL and relational databases."
    },
    new BlogModel
    {
        BlogId = "B002",
        BlogTitle = "Advanced SQL Joins",
        BlogAuthor = "Admin",
        BlogContent = "Learn INNER JOIN, LEFT JOIN, RIGHT JOIN, and FULL JOIN with examples."
    },
    new BlogModel
    {
        BlogId = "B003",
        BlogTitle = "Indexing in SQL Server",
        BlogAuthor = "John Doe",
        BlogContent = "Indexes help improve query performance in SQL Server."
    },
    new BlogModel
    {
        BlogId = "B004",
        BlogTitle = "Stored Procedures",
        BlogAuthor = "Jane Smith",
        BlogContent = "Stored procedures allow reusable and optimized SQL code."
    },
    new BlogModel
    {
        BlogId = "B005",
        BlogTitle = "Views in SQL Server",
        BlogAuthor = "John Doe",
        BlogContent = "Views are virtual tables based on SELECT queries."
    },
    new BlogModel
    {
        BlogId = "B006",
        BlogTitle = "Normalization Basics",
        BlogAuthor = "Admin",
        BlogContent = "Database normalization reduces data redundancy."
    },
    new BlogModel
    {
        BlogId = "B007",
        BlogTitle = "Denormalization Explained",
        BlogAuthor = "Jane Smith",
        BlogContent = "Denormalization improves read performance in some cases."
    },
    new BlogModel
    {
        BlogId = "B008",
        BlogTitle = "Primary vs Foreign Key",
        BlogAuthor = "Admin",
        BlogContent = "Understanding primary keys and foreign keys is essential."
    },
    new BlogModel
    {
        BlogId = "B009",
        BlogTitle = "SQL Constraints",
        BlogAuthor = "John Doe",
        BlogContent = "Constraints enforce rules on data in tables."
    },
    new BlogModel
    {
        BlogId = "B010",
        BlogTitle = "Transactions in SQL",
        BlogAuthor = "Jane Smith",
        BlogContent = "Transactions ensure data consistency using ACID properties."
    },

    new BlogModel
    {
        BlogId = "B101",
        BlogTitle = "Getting Started with Blogging",
        BlogAuthor = "Admin",
        BlogContent = "This blog helps beginners understand how to start blogging."
    },
    new BlogModel
    {
        BlogId = "B102",
        BlogTitle = "Why Learn SQL",
        BlogAuthor = "John Doe",
        BlogContent = "SQL is essential for managing and querying relational databases."
    },
    new BlogModel
    {
        BlogId = "B103",
        BlogTitle = "Web Development Basics",
        BlogAuthor = "Jane Smith",
        BlogContent = "An introduction to HTML, CSS, and JavaScript for beginners."
    },
    new BlogModel
    {
        BlogId = "B104",
        BlogTitle = "Bootstrap Tips",
        BlogAuthor = "Admin",
        BlogContent = "Bootstrap helps build responsive websites quickly."
    },
    new BlogModel
    {
        BlogId = "B105",
        BlogTitle = "Understanding MVC",
        BlogAuthor = "John Doe",
        BlogContent = "MVC separates application logic into Model, View, and Controller."
    },
    new BlogModel
    {
        BlogId = "B106",
        BlogTitle = "ASP.NET Core Intro",
        BlogAuthor = "Jane Smith",
        BlogContent = "ASP.NET Core is a modern framework for web applications."
    },
    new BlogModel
    {
        BlogId = "B107",
        BlogTitle = "CRUD Operations",
        BlogAuthor = "Admin",
        BlogContent = "CRUD stands for Create, Read, Update, and Delete operations."
    },
    new BlogModel
    {
        BlogId = "B108",
        BlogTitle = "REST API Basics",
        BlogAuthor = "John Doe",
        BlogContent = "REST APIs allow communication between client and server."
    },
    new BlogModel
    {
        BlogId = "B109",
        BlogTitle = "Database Design Tips",
        BlogAuthor = "Jane Smith",
        BlogContent = "Good database design improves performance and maintainability."
    },
    new BlogModel
    {
        BlogId = "B110",
        BlogTitle = "Software Best Practices",
        BlogAuthor = "Admin",
        BlogContent = "Writing clean and maintainable code is a key skill."
    }
};
            using IDbConnection db = new SqlConnection(_connectionString);
            foreach (var blog in blogs)
            {
                blog.BlogId = Ulid.NewUlid().ToString();
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
                db.Execute(query, blog);
            }
            return Redirect("/Blog");
        }

        // https://localhost:3000/blog/edit/01KFXEKBC84QQ5QSAEC8SWKRZZ
        // https://localhost:3000/blog/edit?id=01KFXEKBC84QQ5QSAEC8SWKRZZ
        // https://localhost:3000/blog/edit?id=01KFXEKBC84QQ5QSAEC8SWKRZZ&type=Testing&amount=10000
        public IActionResult Edit(string id)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            var item = db.QueryFirstOrDefault<BlogModel>("select * from Tbl_Blog where BlogId = @BlogId",
                new { BlogId = id });
            if(item is null)
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

            TempData["SuccessMessage"] = "Updating successful.";

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
