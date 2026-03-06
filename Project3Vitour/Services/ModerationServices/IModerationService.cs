namespace Project3Vitour.Services.ModerationServices
{
    public interface IModerationService
    {
        Task<(bool isClean, string reason)> AnalyzeAsync(string text);
    }
}