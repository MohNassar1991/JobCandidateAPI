using JobCandidate.Domain;

namespace JobCandidate.Application.Interfaces
{
    public interface ICandidateRepository
    {
        Candidate GetCandidateByEmail(string email);

        void AddCandidate(Candidate candidate);

        void UpdateCandidate(Candidate candidate);

        IEnumerable<Candidate> GetAllCandidates();
    }
}