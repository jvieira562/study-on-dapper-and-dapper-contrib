# Acesso à dados com .NET, C#, Dapper e SQL Server

- **OBSERVAÇÕES**
    - Pesquisar mais sobre indices
    - Como funciona o SQL Server por debaixo dos panos.

**SplitOn**

O parâmetro **`splitOn`** é uma string que representa o nome da coluna que o Dapper usará para dividir os resultados em diferentes entidades. Normalmente, isso é útil quando você está realizando uma consulta que envolve várias tabelas relacionadas e deseja mapear os resultados para objetos relacionados.

- Exemplo
    
    ```csharp
    var sql = 
    		@"SELECT 
    			*
    		FROM Orders AS o 
    			INNER JOIN Customers AS c 
    				ON o.CustomerId = c.CustomerId";
    
    var results = connection.Query<Order, Customer, Order>(
        sql : sql,
        map : (order, customer) =>
        {
            order.Customer = customer;
            return order;
        },
        splitOn: "CustomerId"
    );
    ```
    

**Criando conexão com o banco**

- Exemplo
    
    ```csharp
    const string connectionString = "Server=localhost,1433;Database=balta;User ID=sa;Password=1q2w!Q@W; TrustServerCertificate=True";
    
    using (var connection = new SqlConnection(connectionString))
    {
    
        ExecuteScalar(connection);
        Console.WriteLine("\nAperte qualquer tecla para finalizar: ");
        Console.ReadKey();
    }
    ```
    

**Dynamic**

Em vez de tipar o retorno da consulta, podemos trabalhar com o dynamic usando o Dapper.

- Exemplo
    
    ```csharp
    static void Dynamic(SqlConnection connection)
    {
        var categorias = connection.Query("SELECT [Id], [Title], [Url] FROM [Category] ORDER BY [Title];").ToList();
    
        categorias.ForEach(
            categoria => Console.WriteLine($"{categoria.Id}\t{categoria.Title}\t{categoria.Url}")
        );
    }
    ```
    

**Executando procedures**

- Exemplo
    
    ```csharp
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
    // Procedure que retorna dados
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
    ```
    

**ExecuteScalar**

Permite retornar um item personalizado. Exemplo, quando inserimos um item na base e queremos saber qual ID deste item, podemos fazer o retorno do mesmo usando o ExecuteScalar.

- Exemplo
    
    ```csharp
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
    ```
    

**ExecuteMany**

É possível inserir, atualizar, excluir vários dados de uma vez através do execute many do Dapper.

- Exemplo
    
    ```csharp
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
    ```
    

**OneToOne**

Fazendo mapeamento para objetos com relação um para um.

- Exemplo
    
    ```csharp
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
    ```
    

**OneToMany**

Fazendo mapeamento para objetos com relação um para muitos.

- Exemplo
    
    ```csharp
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
    ```
    

**ManyToMany (Query Multiple)**

Usado para executar múltiplos selects que podem ser em diferentes tabelas.

- Exemplo
    
    ```csharp
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
    ```
    

**SelectIn**

O Dapper da suporte ao SELECT IN

- Exemplo
    
    ```csharp
    static void SelectIn(SqlConnection connection)
    {
        var sql =
            @"SELECT
                *
            FROM [Career]
            WHERE [Id] 
                IN @Id;";
    
        var result = connection.Query<Career>(
            sql : sql,
            param : new
            {
                Id = new[]
                {
                    "01AE8A85-B4E8-4194-A0F1-1C6190AF54CB",
                    "4327AC7E-963B-4893-9F31-9A3B28A4E72B"
                }
            }).ToList();
    
        result.ForEach( career => { Console.WriteLine(career.Title); });
    }
    ```
    

**Like**

- Exemplo
    
    ```csharp
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
    ```
    

**Transaction**

- Exemplo
    
    ```csharp
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
    ```

**Generics**

Os `generics`  são um recurso de programação que permite criar classes, interfaces, métodos e delegados em que o tipo de dados é especificado como um parâmetro quando a instância ou o método é criado. Isso fornece flexibilidade e reutilização de código, uma vez que você pode criar componentes que podem funcionar com diferentes tipos de dados sem precisar escrever código específico para cada tipo.

**Repository Pattern**
O `Repository Pattern` é um padrão de projeto de software amplamente utilizado no desenvolvimento de aplicativos para abstrair e separar a lógica de acesso a dados (por exemplo, banco de dados) da lógica de negócios de um aplicativo.
****

**Generic Repository**

- Exemplo
    
    ```csharp
    public class Repository<TModel> where TModel : class
        {
            private readonly SqlConnection _connection;
    
            public Repository(SqlConnection connection)
                => _connection = connection;
    
            public IEnumerable<TModel> Get()
                => _connection.GetAll<TModel>();
    
            public TModel GetById(int id)
                => _connection.Get<TModel>(id);
    
            public void Create(TModel model)
                => _connection.Insert<TModel>(model);        
            
            public void Update(TModel model)
                => _connection.Update<TModel>(model);
    
            public void Delete(TModel model)
                => _connection.Delete<TModel>(model);
    
            public void Delete(int id)
            {
                var model = _connection.Get<TModel>(id);
                _connection.Delete<TModel>(model);
            }
            
        }
    ```
    
    ```csharp
    
      public class UserRepository : Repository<User>
      {
          private readonly SqlConnection _connection;
          
          public UserRepository(SqlConnection connection) 
              : base(connection)
                  => _connection = connection;
    
          public List<User> GetWithRoles()
          {
              string query = 
                  @"SELECT 
                    [User].*,
                    [Role].*
                  FROM [User]
                    LEFT JOIN [UserRole]
    	                ON [UserRole].[UserId] = [User].[Id]
                    LEFT JOIN [Role] 
                          ON [UserRole].[RoleId] = [Role].[Id];";
              
              var users = new List<User>();
    
              users = _connection.Query<User, Role, User>(
                  sql : query,
                  map : (user, role) =>
                  {
                      var usr = users.Find(x => x.Id == user.Id);
    
                      if(usr == null)
                      {
                          usr = user;
                          if(role != null)
                              usr.Roles.Add(role);
                          users.Add(usr);
                      } else
                          usr.Roles.Add(role);
    
                      return user;
                  },
                  splitOn: "Id").ToList();
    
              return users;
          }
      }
    
    ```