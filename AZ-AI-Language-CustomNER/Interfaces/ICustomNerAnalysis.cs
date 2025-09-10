using AZ_AI_Language_CustomNER.Models;

namespace AZ_AI_Language_CustomNER.Interfaces
{
    public interface ICustomNerAnalysis
    {
        Task<List<CustomEntityResponse>> CustomNerAsync(List<string> documents);
    }
}
