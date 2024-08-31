using JobCandidateAPI.IRepositories;
using JobCandidateAPI.Models;
using JobCandidateAPI.Services;

namespace JobCandidateAPI.Repositories
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly CsvCandidateService _csvService;

        public CandidateRepository(CsvCandidateService csvService)
        {
            _csvService = csvService;
        }

        public Candidate GetCandidateByEmail(string email)
        {
            return _csvService.GetCandidateByEmail(email);
        }

        public void AddCandidate(Candidate candidate)
        {
            _csvService.AddCandidate(candidate);
        }

        public void UpdateCandidate(Candidate candidate)
        {
            _csvService.UpdateCandidate(candidate);
        }

        public IEnumerable<Candidate> GetAllCandidates()
        {
            return _csvService.GetAllCandidates();
        }
    }
}