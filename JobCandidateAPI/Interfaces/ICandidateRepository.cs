using JobCandidateAPI.Models;

namespace JobCandidateAPI.IRepositories
{
    public interface ICandidateRepository
    {
        Candidate GetCandidateByEmail(string email);

        void AddCandidate(Candidate candidate);

        void UpdateCandidate(Candidate candidate);

        IEnumerable<Candidate> GetAllCandidates();
    }
}