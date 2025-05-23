using EF.Data.Interfaces;

namespace EF.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly GameDeveloperContext _context;
        public UnitOfWork(GameDeveloperContext context,
         IDeveloperRepository developers,
         IGameRepository games)
        {
            _context = context;
            Developers = developers;
            Games = games;
        }
        public IDeveloperRepository Developers { get; }

        public IGameRepository Games { get; }

        public int Complete()
        {
            return _context.SaveChanges();
        }
    }

}
