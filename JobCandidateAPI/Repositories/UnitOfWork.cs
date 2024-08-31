using JobCandidateAPI.Interfaces;
using JobCandidateAPI.IRepositories;

namespace JobCandidateAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ICandidateRepository _candidateRepository;

        public UnitOfWork(ICandidateRepository candidateRepository)
        {
            _candidateRepository = candidateRepository;
        }

        public ICandidateRepository CandidateRepository => _candidateRepository;

        public void Commit()
        {
            // ToDo : This for save changes to the database.
        }
    }
}