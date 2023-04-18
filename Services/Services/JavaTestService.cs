using Data.Database;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Model.ViewModel;
using Services.Services.Base;
using System.Linq;

namespace Services.Services
{
    public class JavaTestService : DbService<AppDBContext>, IJavaTestService
    {
        public JavaTestService(DbContextOptions<AppDBContext> dbContextOptions)
        : base(dbContextOptions) { }

        public async Task<JavaTestViewModel> GetLastTest()
        {
            using var dbContext = CreateDbContext();

            var data = await dbContext.Tests.Include(x => x.TestCases).OrderBy(x => x.Id).LastOrDefaultAsync();

            var result = new JavaTestViewModel()
            {
                Id = data.Id,
                TestCases = data.TestCases.OrderBy(x => x.Order).Select(x => x.TestCase).ToList(),
            };

            return result;
        }

        public async Task<List<JavaTestViewModel>> GetTests()
        {
            using var dbContext = CreateDbContext();

            var data = await dbContext.Tests.Include(x => x.TestCases).ToListAsync();

            var list = new List<JavaTestViewModel>();

            foreach (var d in data)
            {
                var listTestCases = d.TestCases.OrderBy(x => x.Order).Select(x => x.TestCase).ToList();

                var model = new JavaTestViewModel()
                {
                    Id = d.Id,
                    TestCases = listTestCases
                };

                list.Add(model);
            }

            return list;
        }

        public async Task<bool> PostTests(List<string> tests)
        {
            using var dbContext = CreateDbContext();

            var tempTests = dbContext.Tests.ToList();

            var test = new TestEntity() { Id = tempTests.Count + 1 };

            var testCases = new List<TestCasesEntity>();

            for (int i = 0; i < tests.Count; i++)
            {
                testCases.Add(
                    new TestCasesEntity()
                    {
                        Order = i,
                        TestCase = tests[i]
                    });
            }

            test.TestCases = testCases;

            dbContext.Tests.Add(test);

            await dbContext.SaveChangesAsync();

            return true;
        }
    }
}
