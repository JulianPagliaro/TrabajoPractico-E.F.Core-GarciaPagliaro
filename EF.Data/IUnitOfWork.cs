using EF.Data.Interfaces;

namespace EF.Data
{
    public interface IUnitOfWork
    {
        IDeveloperRepository Developers { get; }
        IGameRepository Games { get; }
        int Complete();
    }
}
