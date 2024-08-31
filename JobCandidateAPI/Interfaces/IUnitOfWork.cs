using JobCandidateAPI.IRepositories;

namespace JobCandidateAPI.Interfaces
{
    public interface IUnitOfWork
    {
        ICandidateRepository CandidateRepository { get; }

        void Commit();
    }
}