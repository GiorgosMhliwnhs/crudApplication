using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using peripatoiCrud.API.Controllers;
using peripatoiCrud.API.Models.Domain;
using peripatoiCrud.API.Models.DTOs;
using peripatoiCrud.API.Repositories;

namespace peripatoiCrud.API.Tests.Controllers
{
    [TestFixture]
    public class PeripatoiControllerTests
    {
        private PeripatoiController _controller;
        private Mock<IPeripatosRepository> _mockRepository;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IPeripatosRepository>();
            _controller = new PeripatoiController(_mockRepository.Object);
        }

        [Test]
        public async Task GetAll_Epistrefei200MeListaPeripatwn()
        {
            // Arrange
            var peripatoiDomain = new List<Peripatos>
    {
        new Peripatos
        {
            Id = Guid.NewGuid(),
            Onoma = "Peripatos1",
            Perigrafh = "Description1",
            Mhkos = 5,
            EikonaUrl = "image1.jpg",
            DyskoliaId = Guid.NewGuid(),
            PerioxhId = Guid.NewGuid(),
            Perioxh = new Perioxh { Id = Guid.NewGuid(), Onoma = "Perioxh1", Kwdikos = "K1", EikonaUrl = "image1.jpg" },
            Dyskolia = new Dyskolia { Id = Guid.NewGuid(), Onoma = "Easy" }
        },
        new Peripatos
        {
            Id = Guid.NewGuid(),
            Onoma = "Peripatos2",
            Perigrafh = "Description2",
            Mhkos = 10,
            EikonaUrl = "image2.jpg",
            DyskoliaId = Guid.NewGuid(),
            PerioxhId = Guid.NewGuid(),
            Perioxh = new Perioxh { Id = Guid.NewGuid(), Onoma = "Perioxh2", Kwdikos = "K2", EikonaUrl = "image2.jpg" },
            Dyskolia = new Dyskolia { Id = Guid.NewGuid(), Onoma = "Medium" }
        }
    };
            _mockRepository.Setup(repo => repo.GetAllAsync(null, null, null, false, 1, 1000)).ReturnsAsync(peripatoiDomain);

            // Act
            var result = await _controller.GetAll(null, null, null, false, 1, 1000) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<PeripatosDto>>(result.Value);
            var returnedPeripatoi = result.Value as List<PeripatosDto>;
            Assert.AreEqual(peripatoiDomain.Count, returnedPeripatoi.Count);
        }


        [Test]
        public async Task Create_MeSwstoModelEpistrefeiActionResult()
        {
            // Arrange
            var addPeripatosRequestDto = new AddPeripatosRequestDto
            {
                Onoma = "Peripatos1",
                Perigrafh = "Description1",
                Mhkos = 5,
                EikonaUrl = "image1.jpg",
                DyskoliaId = Guid.NewGuid(),
                PerioxhId = Guid.NewGuid()
            };
            var peripatos = new Peripatos
            {
                Id = Guid.NewGuid(),
                Onoma = addPeripatosRequestDto.Onoma,
                Perigrafh = addPeripatosRequestDto.Perigrafh,
                Mhkos = addPeripatosRequestDto.Mhkos,
                EikonaUrl = addPeripatosRequestDto.EikonaUrl,
                DyskoliaId = addPeripatosRequestDto.DyskoliaId,
                PerioxhId = addPeripatosRequestDto.PerioxhId
            };
            _mockRepository.Setup(repo => repo.CreateAsync(It.IsAny<Peripatos>())).ReturnsAsync(peripatos);

            // Act
            var result = await _controller.Create(addPeripatosRequestDto) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<PeripatosDto>(result.Value);
            var createdPeripatos = result.Value as PeripatosDto;
            Assert.AreEqual(peripatos.Onoma, createdPeripatos.Onoma);
        }

        [Test]
        public async Task GetById_MeSwstoIdEpistrefei200MeTonPeripato()
        {
            // Arrange
            var peripatosId = Guid.NewGuid();
            var peripatos = new Peripatos
            {
                Id = peripatosId,
                Onoma = "Peripatos1",
                Perigrafh = "Description1",
                Mhkos = 5,
                EikonaUrl = "image1.jpg",
                DyskoliaId = Guid.NewGuid(),
                PerioxhId = Guid.NewGuid(),
                Perioxh = new Perioxh { Id = Guid.NewGuid(), Onoma = "Perioxh1", Kwdikos = "K1", EikonaUrl = "image.jpg" },
                Dyskolia = new Dyskolia { Id = Guid.NewGuid(), Onoma = "Easy" }
            };
            _mockRepository.Setup(repo => repo.GetByIdAsync(peripatosId)).ReturnsAsync(peripatos);

            // Act
            var result = await _controller.GetById(peripatosId) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<PeripatosDto>(result.Value);
            var returnedPeripatos = result.Value as PeripatosDto;
            Assert.AreEqual(peripatosId, returnedPeripatos.Id);
        }


        [Test]
        public async Task GetById_MeLathosIdEpistrefei400()
        {
            // Arrange
            var peripatosId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.GetByIdAsync(peripatosId)).ReturnsAsync((Peripatos)null);

            // Act
            var result = await _controller.GetById(peripatosId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Update_MeSwstoIdEpistrefei200()
        {
            // Arrange
            var peripatosId = Guid.NewGuid();
            var updatePeripatosRequestDto = new UpdatePeripatosRequestDto
            {
                Onoma = "UpdatedPeripatos",
                Perigrafh = "UpdatedDescription",
                Mhkos = 10,
                EikonaUrl = "updatedImage.jpg",
                DyskoliaId = Guid.NewGuid(),
                PerioxhId = Guid.NewGuid()
            };
            var updatedPeripatos = new Peripatos
            {
                Id = peripatosId,
                Onoma = updatePeripatosRequestDto.Onoma,
                Perigrafh = updatePeripatosRequestDto.Perigrafh,
                Mhkos = updatePeripatosRequestDto.Mhkos,
                EikonaUrl = updatePeripatosRequestDto.EikonaUrl,
                DyskoliaId = updatePeripatosRequestDto.DyskoliaId,
                PerioxhId = updatePeripatosRequestDto.PerioxhId
            };
            _mockRepository.Setup(repo => repo.UpdateAsync(peripatosId, It.IsAny<Peripatos>())).ReturnsAsync(updatedPeripatos);

            // Act
            var result = await _controller.Update(peripatosId, updatePeripatosRequestDto) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<PeripatosDto>(result.Value);
            var returnedPeripatos = result.Value as PeripatosDto;
            Assert.AreEqual(peripatosId, returnedPeripatos.Id);
        }


        [Test]
        public async Task Update_MeLathosIdEpistrefei400()
        {
            // Arrange
            var peripatosId = Guid.NewGuid();
            var updatePeripatosRequestDto = new UpdatePeripatosRequestDto
            {
                Onoma = "UpdatedPeripatos",
                Perigrafh = "UpdatedDescription",
                Mhkos = 10,
                EikonaUrl = "updatedImage.jpg",
                DyskoliaId = Guid.NewGuid(),
                PerioxhId = Guid.NewGuid()
            };
            _mockRepository.Setup(repo => repo.UpdateAsync(peripatosId, It.IsAny<Peripatos>())).ReturnsAsync((Peripatos)null);

            // Act
            var result = await _controller.Update(peripatosId, updatePeripatosRequestDto);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Delete_MeSwstoId_Epistrefei200MeTonPeripato()
        {
            // Arrange
            var peripatosId = Guid.NewGuid();
            var peripatos = new Peripatos { Id = peripatosId, Onoma = "Peripatos1", Perigrafh = "Description1", Mhkos = 5, EikonaUrl = "image1.jpg", DyskoliaId = Guid.NewGuid(), PerioxhId = Guid.NewGuid() };
            _mockRepository.Setup(repo => repo.DeleteAsync(peripatosId)).ReturnsAsync(peripatos);

            // Act
            var result = await _controller.Delete(peripatosId) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<PeripatosDto>(result.Value);
            var returnedPeripatos = result.Value as PeripatosDto;
            Assert.AreEqual(peripatosId, returnedPeripatos.Id);
        }

        [Test]
        public async Task Delete_MeLathosId_Epistrefei400()
        {
            // Arrange
            var peripatosId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.DeleteAsync(peripatosId)).ReturnsAsync((Peripatos)null);

            // Act
            var result = await _controller.Delete(peripatosId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}
