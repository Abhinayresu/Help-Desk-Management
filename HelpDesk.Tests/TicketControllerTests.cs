using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using HelpDesk.Api.Controllers;
using HelpDesk.Api.Models;
using HelpDesk.Api.Repositories;

namespace HelpDesk.Tests
{
    public class TicketControllerTests
    {
        private readonly Mock<ITicketRepository> _mockRepository;
        private readonly TicketController _controller;

        public TicketControllerTests()
        {
            _mockRepository = new Mock<ITicketRepository>();
            _controller = new TicketController(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllTickets_ReturnsOkResult_WhenTicketsExist()
        {
            // Arrange
            var tickets = new List<Ticket>
            {
                new Ticket { Id = 1, Title = "Test Ticket 1", Description = "Desc 1", Priority = "Low", Status = "Open", RaisedBy = "TestUser" },
                new Ticket { Id = 2, Title = "Test Ticket 2", Description = "Desc 2", Priority = "High", Status = "In Progress", RaisedBy = "TestUser" }
            };
            _mockRepository.Setup(repo => repo.GetAllTicketsAsync()).ReturnsAsync(tickets);

            // Act
            var result = await _controller.GetAllTickets();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTickets = Assert.IsAssignableFrom<List<Ticket>>(okResult.Value);
            Assert.Equal(2, returnedTickets.Count);
        }

        [Fact]
        public async Task GetTicketById_ReturnsOkResult_WhenTicketExists()
        {
            // Arrange
            var ticket = new Ticket { Id = 1, Title = "Test Ticket", Description = "Desc", Priority = "Low", Status = "Open", RaisedBy = "TestUser" };
            _mockRepository.Setup(repo => repo.GetTicketByIdAsync(1)).ReturnsAsync(ticket);

            // Act
            var result = await _controller.GetTicketById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTicket = Assert.IsType<Ticket>(okResult.Value);
            Assert.Equal(1, returnedTicket.Id);
        }

        [Fact]
        public async Task GetTicketById_ReturnsNotFound_WhenTicketDoesNotExist()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetTicketByIdAsync(1)).ReturnsAsync((Ticket?)null);

            // Act
            var result = await _controller.GetTicketById(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreateTicket_ReturnsOkResult_WhenTicketIsCreatedSuccessfully()
        {
            // Arrange
            var ticket = new Ticket { Id = 0, Title = "New Ticket", Description = "Desc", Priority = "Low", Status = "Open", RaisedBy = "TestUser" };
            _mockRepository.Setup(repo => repo.CreateTicketAsync(It.IsAny<Ticket>()))
                           .Callback<Ticket>(t => t.Id = 123)
                           .ReturnsAsync(123);

            // Act
            var result = await _controller.CreateTicket(ticket);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedTicket = Assert.IsType<Ticket>(createdResult.Value);
            Assert.Equal(123, returnedTicket.Id);
            Assert.Equal("Open", returnedTicket.Status); // Status must be forced to Open
        }

        [Fact]
        public async Task CreateTicket_ReturnsBadRequest_WhenTicketIsNull()
        {
            // Act
            var result = await _controller.CreateTicket(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetTicketsByStatus_ReturnsOkResult_WhenMatchingTicketsExist()
        {
            // Arrange
            var tickets = new List<Ticket>
            {
                new Ticket { Id = 1, Title = "Open Ticket", Description = "Desc", Priority = "Low", Status = "Open", RaisedBy = "TestUser" }
            };
            _mockRepository.Setup(repo => repo.GetTicketsByStatusAsync("Open")).ReturnsAsync(tickets);

            // Act
            var result = await _controller.GetTicketsByStatus("Open");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTickets = Assert.IsAssignableFrom<List<Ticket>>(okResult.Value);
            Assert.Single(returnedTickets);
        }
    }
}
