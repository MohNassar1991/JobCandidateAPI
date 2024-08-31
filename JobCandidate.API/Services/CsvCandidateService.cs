using CsvHelper;
using JobCandidate.Domain;
using Microsoft.Extensions.Caching.Memory;
using System.Globalization;

namespace JobCandidate.API.Services
{
    public class CsvCandidateService
    {
        private readonly string _filePath = "candidates.csv";
        private readonly IMemoryCache _cache;
        private readonly MemoryCacheEntryOptions _cacheOptions;

        public CsvCandidateService(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
            _cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };
        }

        public List<Candidate> GetAllCandidates()
        {
            if (!File.Exists(_filePath)) return new List<Candidate>();

            using (var reader = new StreamReader(_filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<Candidate>().ToList();
            }
        }

        public Candidate GetCandidateByEmail(string email)
        {
            if (_cache.TryGetValue(email, out Candidate? candidate))
            {
                return candidate;
            }

            var candidates = GetAllCandidates();
            candidate = candidates.FirstOrDefault(c => c.Email == email);

            if (candidate != null)
            {
                _cache.Set(email, candidate, _cacheOptions);
            }

            return candidate;
        }

        public void AddCandidate(Candidate candidate)
        {
            var candidates = GetAllCandidates();

            if (candidates.Any(c => c.Email == candidate.Email))
            {
                throw new InvalidOperationException("Candidate with the same email already exists.");
            }

            candidates.Add(candidate);
            _cache.Set(candidate.Email, candidate, _cacheOptions);
            SaveCandidatesToFile(candidates);
        }

        public void UpdateCandidate(Candidate candidate)
        {
            var candidates = GetAllCandidates();
            var existingCandidate = candidates.FirstOrDefault(c => c.Email == candidate.Email);

            if (existingCandidate != null)
            {
                existingCandidate.FirstName = candidate.FirstName;
                existingCandidate.LastName = candidate.LastName;
                existingCandidate.PhoneNumber = candidate.PhoneNumber;
                existingCandidate.PreferredCallTime = candidate.PreferredCallTime;
                existingCandidate.LinkedInProfileUrl = candidate.LinkedInProfileUrl;
                existingCandidate.GitHubProfileUrl = candidate.GitHubProfileUrl;
                existingCandidate.Comment = candidate.Comment;

                _cache.Set(existingCandidate.Email, existingCandidate, _cacheOptions);
            }
            else
            {
                candidates.Add(candidate);
                _cache.Set(candidate.Email, candidate, _cacheOptions);
            }

            SaveCandidatesToFile(candidates);
        }

        private void SaveCandidatesToFile(List<Candidate> candidates)
        {
            using (var writer = new StreamWriter(_filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(candidates);
            }
        }
    }
}