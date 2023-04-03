using Data.Entities;
using Services.Model.ViewModel;

namespace Services.Services.Base
{
    public interface IJavaTestService
    {
        public Task<bool> PostTests(List<string> tests);
        public Task<List<JavaTestViewModel>> GetTests();
    }
}
