using Dapper;
using System.Data;
using DapperAndSQLServer;
using Microsoft.Data.SqlClient;
using DapperAndSQLServer.Models;

const string connectionString = "Server=localhost,1433;Database=balta;User ID=sa;Password=1q2w!Q@W; TrustServerCertificate=True";

using (var connection = new SqlConnection(connectionString))
{
    Transaction(connection);
    Console.WriteLine("\nAperte qualquer tecla para finalizar: ");
    Console.ReadKey();
}
static void Dynamic(SqlConnection connection)
{
    var categorias = connection.Query("SELECT [Id], [Title], [Url] FROM [Category] ORDER BY [Title];").ToList();

    categorias.ForEach(
        categoria => Console.WriteLine($"{categoria.Id}\t{categoria.Title}\t{categoria.Url}")
    );
}
static void ListCategories(SqlConnection connection)
{
    var categorias = connection.Query<Category>("SELECT [Id], [Title], [Url] FROM [Category] ORDER BY [Title];").ToList();

    categorias.ForEach(
        categoria => Console.WriteLine($"{categoria.Id}\t{categoria.Title}\t{categoria.Url}")
    );
}
static void CreateCategory(SqlConnection connection)
{
    var category = new Category
    {
        Id = Guid.NewGuid(),
        Title = "Amazon AWS",
        Url = "amazon",
        Description = "Categoria destinada a serviços do AWS.",
        Summary = "AWS Cloud",
        Order = 8,
        Featured = false,
    };
    var insertSql =
        @"INSERT INTO 
        [Category] 
    VALUES (
        @Id,
        @Title,
        @Url,
        @Summary,
        @Order,
        @Description,
        @Featured);";

    var rows = connection.Execute(
        sql: insertSql,
        param: new
        {
            category.Id,
            category.Title,
            category.Url,
            category.Summary,
            category.Order,
            category.Description,
            category.Featured,
        });

    Console.WriteLine($"\nLinhas afetadas: {rows}.");
}
static void UpdateCategory(SqlConnection connection)
{
    string updateSql = "UPDATE [Category] SET [Url] = @Url WHERE [Id] = @Id;";

    var category = new Category
    {
        Id = new Guid("1f123bf3-574d-4793-bcb6-972d0053ed5c"),
        Title = "Amazon AWS",
        Url = "amazonaws",
        Description = "Categoria destinada a serviços do AWS.",
        Summary = "AWS Cloud",
        Order = 8,
        Featured = false,
    };

    var rows = connection.Execute(
        sql : updateSql,
        param : new 
        {
            category.Id,
            category.Title,
            category.Url,
            category.Summary,
            category.Order,
            category.Description,
            category.Featured,
        });
    Console.WriteLine($"\nLinhas afetadas: {rows}.");
}
static void CreateManyCategory(SqlConnection connection)
{
    var category = new Category
    {
        Id = Guid.NewGuid(),
        Title = "Amazon AWS",
        Url = "amazon",
        Description = "Categoria destinada a serviços do AWS.",
        Summary = "AWS Cloud",
        Order = 8,
        Featured = false,
    };

    var category2 = new Category
    {
        Id = Guid.NewGuid(),
        Title = "Azure",
        Url = "azure",
        Description = "Categoria destinada a serviços do Microsoft Azure.",
        Summary = "Azure",
        Order = 9,
        Featured = true,
    };
    var insertSql =
        @"INSERT INTO 
        [Category] 
    VALUES (
        @Id,
        @Title,
        @Url,
        @Summary,
        @Order,
        @Description,
        @Featured);";

    var rows = connection.Execute(
        sql : insertSql,
        param : new[] 
        { 
            new
            {
                category.Id,
                category.Title,
                category.Url,
                category.Summary,
                category.Order,
                category.Description,
                category.Featured,
            },
            new
            {
                category2.Id,
                category2.Title,
                category2.Url,
                category2.Summary,
                category2.Order,
                category2.Description,
                category2.Featured,
            }
        });

    Console.WriteLine($"\nLinhas afetadas: {rows}.");
}
static void ExecuteProcedure(SqlConnection connection)
{
    var proc = "spDeleteStudent";
    var pars = new { StudentId = "0909FD72-9F59-4983-B94F-9A1E8F8422B2" };

    var rows = connection.Execute( 
        sql : proc,
        param : pars,
        commandType : CommandType.StoredProcedure);

    Console.WriteLine($"Linhas afetadas: {rows}.");
}
static void ExecuteReadProcedure(SqlConnection connection)
{
    var proc = "spGetCoursesByCategory";
    var pars = new { CategoryId = "09CE0B7B-CFCA-497B-92C0-3290AD9D5142" };

    var courses = 
        connection.Query(
            sql: proc,
            param: pars,
            commandType: CommandType.StoredProcedure)
        .ToList();

    courses.ForEach(
        item => { Console.WriteLine($"{item.Id}\t{item.Title}"); });
}
static void ExecuteScalar(SqlConnection connection)
{
    var category = new Category
    {
        Title = "Amazon AWS",
        Url = "amazon",
        Description = "Categoria destinada a serviços do AWS.",
        Summary = "AWS Cloud",
        Order = 8,
        Featured = false,
    };
    var insertSql =
        @"INSERT INTO 
            [Category]
        OUTPUT inserted.[Id]
        VALUES (
            NEWID(),
            @Title,
            @Url,
            @Summary,
            @Order,
            @Description,
            @Featured);";

    var id = connection.ExecuteScalar<Guid>(
        sql: insertSql,
        param: new
        {
            category.Title,
            category.Url,
            category.Summary,
            category.Order,
            category.Description,
            category.Featured,
        });

    Console.WriteLine($"\nId da categoria inserida: {id}.");
}
static void OneToOne(SqlConnection connection)
{
    string sql =
        @"SELECT 
            * 
        FROM [CareerItem]
        INNER JOIN [Course]
            ON [CareerItem].[CourseId] = [Course].[Id];";
    // Uma careira tem um curso.
    var itens = connection.Query<CareerItem, Course, CareerItem>(
        sql : sql,
        ( careerItem, course ) => // Criando método para realizar o mapeamento
        {
            careerItem.Course = course;
            return careerItem;
        },
        splitOn : "Id")
        .ToList();

    itens.ForEach(
        item => 
        { 
            Console.WriteLine(item.Course.Title); 
        });
}
static void OneToMany(SqlConnection connection)
{
    string sql =
        @"SELECT 
            [Career].[Id],
            [Career].[Title],
            [CareerItem].[CareerId],
            [CareerItem].[Title]
        FROM [Career] 
        INNER JOIN [CareerItem] 
            ON [CareerItem].[CareerId] = [Career].[Id]
        ORDER BY [Career].[Title]";

    var careers = new List<Career>();
    var items = connection.Query<Career, CareerItem, Career>(
        sql: sql,
        (career, item) =>
        {
            var car = careers.Where( x => x.Id == career.Id).FirstOrDefault();
            if(car == null)
            {
                car = career;
                car.Items.Add(item);
                careers.Add(car);
            } else
            {
                car.Items.Add(item);
            }

            return career;
        },
        splitOn: "CareerId")
        .ToList();

    careers.ForEach(
        career =>
        {
            Console.WriteLine(career.Title);
            foreach (var item in career.Items)
            {
                Console.WriteLine($" - {item.Title}");
            }
        });
}
static void QueryMultiple(SqlConnection connection)
{
    var sql =
        @"SELECT
            *
        FROM [Category];
        SELECT
            * 
        FROM [Course];";

    using (var multi = connection.QueryMultiple(sql))
    {
        var categories = multi.Read<Category>();
        var courses = multi.Read<Course>();

        foreach(var item in categories)
        {
            Console.WriteLine(item.Title);
        }
        foreach (var item in courses)
        {
            Console.WriteLine(item.Title);
        }
    }
}
static void SelectIn(SqlConnection connection)
{
    var sql =
        @"SELECT
            *
        FROM [Course]
        WHERE [CategoryId] 
            IN @Id;";

    var result = connection.Query<Course>(
        sql : sql,
        param : new
        {
            Id = new[]
            {
                "09CE0B7B-CFCA-497B-92C0-3290AD9D5142",
                "6CD9BA03-5521-43FA-8275-553FD5CA042A"
            }
        }).ToList();

    result.ForEach( course => { Console.WriteLine(course.Title); });
}
static void Like(SqlConnection connection)
{
    Console.WriteLine("\nPesquisar curso: ");
    var titleCourseIn = Console.ReadLine();
    Console.WriteLine();

    var sql =
        @"SELECT
            *
        FROM [Course]
        WHERE [Title] 
            Like @Exp;";

    var result = connection.Query<Course>(
        sql: sql,
        param: new
        {
            Exp = $"%{titleCourseIn}%"
        }).ToList();

    result.ForEach(course => { Console.WriteLine(course.Title); });
}
static void Transaction(SqlConnection connection)
{
    var category = new Category
    {
        Id = Guid.NewGuid(),
        Title = "nÃO QUEROOO",
        Url = "amazon",
        Description = "Categoria destinada a serviços do AWS.",
        Summary = "AWS Cloud",
        Order = 8,
        Featured = false,
    };
    var insertSql =
        @"INSERT INTO 
        [Category] 
    VALUES (
        @Id,
        @Title,
        @Url,
        @Summary,
        @Order,
        @Description,
        @Featured);";

    connection.Open();

    using (var transaction = connection.BeginTransaction())
    {
        var rows = connection.Execute(
        sql: insertSql,
        param: new
        {
            category.Id,
            category.Title,
            category.Url,
            category.Summary,
            category.Order,
            category.Description,
            category.Featured,
        },
        transaction : transaction);

        //transaction.Commit();
        transaction.Rollback();

        Console.WriteLine($"\nLinhas afetadas: {rows}.");
    }

}