using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using peripatoiCrud.API.Controllers;
using peripatoiCrud.API.Models.Domain;
using peripatoiCrud.API.Models.DTOs;
using peripatoiCrud.API.Repositories;
using System.Collections.Generic;

namespace peripatoiCrud.API.Tests.Controllers
{
    [TestFixture]
    public class PerioxesControllerTests
    {
        private PerioxesController _controller;
        private Mock<IPerioxhRepository> _mockRepository;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IPerioxhRepository>();
            _controller = new PerioxesController(null, _mockRepository.Object);
        }

        [Test]
        public async Task GetAll_Epistrefei200MeListaPerioxwn()
        {
            // Arrange
            var perioxesDomain = new List<Perioxh>
            {
                new Perioxh { Id = Guid.NewGuid(), Onoma = "Perioxh1", Kwdikos = "123", EikonaUrl = "image1.jpg" },
                new Perioxh { Id = Guid.NewGuid(), Onoma = "Perioxh2", Kwdikos = "456", EikonaUrl = "image2.jpg" }
            };
            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(perioxesDomain);

            // Act
            var result = await _controller.GetAll() as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<PerioxhDto>>(result.Value);
            Assert.AreEqual(perioxesDomain.Count, (result.Value as List<PerioxhDto>).Count);
        }

        [Test]
        public async Task GetById_MeSwstoIdEpistrefei200MeThPerioxh()
        {
            // Arrange
            var perioxhId = Guid.NewGuid();
            var perioxh = new Perioxh { Id = perioxhId, Onoma = "Perioxh1", Kwdikos = "123", EikonaUrl = "image1.jpg" };
            _mockRepository.Setup(repo => repo.GetByIdAsync(perioxhId)).ReturnsAsync(perioxh);

            // Act
            var result = await _controller.GetById(perioxhId) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<PerioxhDto>(result.Value);
            var returnedPerioxh = result.Value as PerioxhDto;
            Assert.AreEqual(perioxhId, returnedPerioxh.Id);
        }

        [Test]
        public async Task GetById_MeLathosIdEpistrefei400()
        {
            // Arrange
            var perioxhId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.GetByIdAsync(perioxhId)).ReturnsAsync((Perioxh)null);

            // Act
            var result = await _controller.GetById(perioxhId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Create_MeSwstoModelEpistrefeiCreatedAtActionResult()
        {
            // Arrange
            var addPerioxhRequestDto = new AddPerioxhRequestDto
            {
                Kwdikos = "123",
                Onoma = "Perioxh1",
                EikonaUrl = "image1.jpg"
            };
            var perioxh = new Perioxh
            {
                Id = Guid.NewGuid(),
                Kwdikos = addPerioxhRequestDto.Kwdikos,
                Onoma = addPerioxhRequestDto.Onoma,
                EikonaUrl = addPerioxhRequestDto.EikonaUrl
            };
            _mockRepository.Setup(repo => repo.CreateAsync(It.IsAny<Perioxh>())).ReturnsAsync(perioxh);

            // Act
            var result = await _controller.Create(addPerioxhRequestDto) as CreatedAtActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<PerioxhDto>(result.Value);
            var createdPerioxh = result.Value as PerioxhDto;
            Assert.AreEqual(perioxh.Onoma, createdPerioxh.Onoma);
        }

        [Test]
        public async Task Update_MeSwstoIdEpistrefei200()
        {
            // Arrange
            var perioxhId = Guid.NewGuid();
            var updatePerioxhRequestDto = new UpdatePerioxhRequestDto
            {
                Kwdikos = "123",
                Onoma = "Perioxh1",
                EikonaUrl = "image1.jpg"
            };
            var updatedPerioxh = new Perioxh
            {
                Id = perioxhId,
                Kwdikos = updatePerioxhRequestDto.Kwdikos,
                Onoma = updatePerioxhRequestDto.Onoma,
                EikonaUrl = updatePerioxhRequestDto.EikonaUrl
            };
            _mockRepository.Setup(repo => repo.UpdateAsync(perioxhId, It.IsAny<Perioxh>())).ReturnsAsync(updatedPerioxh);

            // Act
            var result = await _controller.Update(perioxhId, updatePerioxhRequestDto) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<PerioxhDto>(result.Value);
            var returnedPerioxh = result.Value as PerioxhDto;
            Assert.AreEqual(perioxhId, returnedPerioxh.Id);
        }

        [Test]
        public async Task Delete_MeSwstoId_Epistrefei200MeThPerioxh()
        {
            // Arrange
            var perioxhId = Guid.NewGuid();
            var perioxh = new Perioxh { Id = perioxhId, Onoma = "Perioxh1", Kwdikos = "123", EikonaUrl = "image1.jpg" };
            _mockRepository.Setup(repo => repo.DeleteAsync(perioxhId)).ReturnsAsync(perioxh);

            // Act
            var result = await _controller.Delete(perioxhId) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<PerioxhDto>(result.Value);
            var returnedPerioxh = result.Value as PerioxhDto;
            Assert.AreEqual(perioxhId, returnedPerioxh.Id);
        }

        [Test]
        public async Task Delete_MeLathosId_Epistrefei400()
        {
            // Arrange
            var perioxhId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.DeleteAsync(perioxhId)).ReturnsAsync((Perioxh)null);

            // Act
            var result = await _controller.Delete(perioxhId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}
