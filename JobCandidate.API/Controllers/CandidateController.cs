using FluentValidation;
using JobCandidate.Application.Interfaces;
using JobCandidate.Domain;
using Microsoft.AspNetCore.Mvc;

namespace JobCandidate.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CandidateController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<Candidate> _validator;

        public CandidateController(IUnitOfWork unitOfWork, IValidator<Candidate> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        [HttpPost("AddCandidate")]
        public IActionResult AddCandidate([FromBody] Candidate candidate)
        {
            var validationResult = _validator.Validate(candidate);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (_unitOfWork.CandidateRepository.GetCandidateByEmail(candidate.Email) != null)
            {
                return BadRequest("A candidate with the same email already exists.");
            }

            _unitOfWork.CandidateRepository.AddCandidate(candidate);
            _unitOfWork.Commit();

            return Ok("Candidate added successfully.");
        }

        [HttpPost("UpdateCandidate")]
        public IActionResult UpdateCandidate([FromBody] Candidate candidate)
        {
            var validationResult = _validator.Validate(candidate);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var existingCandidate = _unitOfWork.CandidateRepository.GetCandidateByEmail(candidate.Email);

            if (existingCandidate != null)
            {
                _unitOfWork.CandidateRepository.UpdateCandidate(candidate);
            }
            else
            {
                _unitOfWork.CandidateRepository.AddCandidate(candidate);
            }

            _unitOfWork.Commit();

            return Ok("Candidate added/updated successfully.");
        }
    }
}