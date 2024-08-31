using FluentValidation;
using FluentValidation.Results;
using JobCandidate.API.Controllers;
using JobCandidate.Application.Interfaces;
using JobCandidate.Domain;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace JobCandidateAPI.UnitTest
{
    public class CandidateServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IValidator<Candidate>> _validatorMock;
        private readonly CandidateController _controller;

        public CandidateServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _validatorMock = new Mock<IValidator<Candidate>>();
            _controller = new CandidateController(_unitOfWorkMock.Object, _validatorMock.Object);
        }

        [Fact]
        public void AddCandidate_ValidCandidate_ReturnsOk()
        {
            // Arrange
            var candidate = new Candidate
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@example.com",
                Comment = "New candidate"
            };

            _validatorMock.Setup(v => v.Validate(It.IsAny<Candidate>()))
                          .Returns(new ValidationResult());

            // Act
            var result = _controller.AddCandidate(candidate);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Candidate added successfully.", okResult.Value);
        }

        [Fact]
        public void AddCandidate_ExistingEmail_ReturnsBadRequest()
        {
            // Arrange
            var candidate = new Candidate
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@example.com",
                Comment = "New candidate"
            };

            _validatorMock.Setup(v => v.Validate(It.IsAny<Candidate>()))
                          .Returns(new ValidationResult());

            _unitOfWorkMock.Setup(u => u.CandidateRepository.AddCandidate(It.IsAny<Candidate>()))
                           .Throws(new InvalidOperationException("Candidate with the same email already exists."));

            // Act
            var result = _controller.AddCandidate(candidate);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Candidate with the same email already exists.", badRequestResult.Value);
        }

        [Fact]
        public void UpsertCandidate_ValidCandidate_ReturnsOk()
        {
            // Arrange
            var candidate = new Candidate
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@example.com",
                Comment = "Updated candidate"
            };

            _validatorMock.Setup(v => v.Validate(It.IsAny<Candidate>()))
                          .Returns(new ValidationResult());

            _unitOfWorkMock.Setup(u => u.CandidateRepository.GetCandidateByEmail(It.IsAny<string>()))
                           .Returns(candidate);

            // Act
            var result = _controller.UpdateCandidate(candidate);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Candidate added/updated successfully.", okResult.Value);
        }
    }
}