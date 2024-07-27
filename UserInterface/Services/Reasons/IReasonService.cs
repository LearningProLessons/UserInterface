namespace UserInterface.Services.Reasons;

public interface IReasonService
{
    Task<string> GetReasonListAsync();
}
