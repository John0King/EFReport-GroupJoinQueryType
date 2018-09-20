using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using GroupJoinQueryType.Models;
using Microsoft.Extensions.Logging;
using Dapper;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GroupJoinQueryType
{
    public class Program
    {
        public static async Task Main()
        {
            var logFactory = GetLoggerFactory();
            var option = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=GroupJoinQueryType#13365 ;Trusted_Connection=True;")
                .UseLoggerFactory(logFactory)
                .Options;
            try
            {
                using (var db = new AppDbContext(option))
                {
                    await db.Database.EnsureCreatedAsync();
                    await db.Database.EnsureViewCreatedAsync();
                    await db.EnsureDataAddedAsync();


                    var result = await db.Query<Student>().GroupJoin(db.Set<StudentExtendField>(), s => s.Id, f => f.StudentId, (s, f) => new StudentInfo
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Age = s.Age,
                        ExtendInfo = new List<StudentExtendField>(f)
                    }).OrderByDescending(i => i.Id)
                    .ToListAsync();

                    Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
                }
            }
            catch(Exception e)
            {
                logFactory.CreateLogger<Program>().LogError(e, "Error happend");
            }

            Console.WriteLine("Press any key to exit ...");
            Console.ReadKey();
            
        }
        public static ILoggerFactory GetLoggerFactory()
        {
            var factory = new LoggerFactory();
            factory.AddConsole();
            return factory;
        }

        
    }

    public static class Extensions
    {
        public static async Task EnsureViewCreatedAsync(this Microsoft.EntityFrameworkCore.Infrastructure.DatabaseFacade database)
        {
            var conn = database.GetDbConnection();

            // ¼ì²é±í
            var hasTable = await conn.ExecuteScalarAsync<bool>($@"SELECT CASE WHEN EXISTS(SELECT * FROM [sys].[tables] WHERE object_id = OBJECT_ID('StudentsStore')) THEN 1 ELSE 0 END");
            if (!hasTable)
            {
                await conn.ExecuteAsync($@"
CREATE TABLE [dbo].[StudentStore]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[Name] NVarchar(50) NOT NULL,
	[Age] INT Null
)

INSERT INTO [dbo].[StudentStore]([Id],[Name],[Age]) VAlues(1,N'A',12)
INSERT INTO [dbo].[StudentStore]([Id],[Name],[Age]) VAlues(2,N'B',13)
");
            }

            var hasView = await conn.ExecuteScalarAsync<bool>($@"SELECT CASE WHEN EXISTS( SELECT * FROM [sys].[views] WHERE object_id = OBJECT_ID('Students') )  THEN 1 ELSE  0 END");

            if (!hasView)
            {
                await conn.ExecuteAsync($@"
CREATE VIEW [dbo].[Students]
	AS SELECT [Id],[Name],[Age] FROM [dbo].[StudentStore]
");
            }

        }

        public static async Task EnsureDataAddedAsync(this AppDbContext db)
        {
            if(!await db.StudentExtendFields.AnyAsync())
            {
                db.Add(new StudentExtendField
                {
                    StudentId = 1,
                    FieldName = "AA",
                    FieldValue = "AAAAA"
                });
                db.Add(new StudentExtendField
                {
                    StudentId = 2,
                    FieldName = "BB",
                    FieldValue = "BBBB"
                });

                db.Add(new StudentExtendField
                {
                    StudentId = 1,
                    FieldName = "CC",
                    FieldValue = "CCCC"
                });
                await db.SaveChangesAsync();
            }
        }
    }
}
