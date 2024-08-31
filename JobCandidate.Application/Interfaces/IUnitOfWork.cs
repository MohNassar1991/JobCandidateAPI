namespace JobCandidate.Application.Interfaces
{
    public interface IUnitOfWork
    {
        ICandidateRepository CandidateRepository { get; }

        void Commit();
    }
}