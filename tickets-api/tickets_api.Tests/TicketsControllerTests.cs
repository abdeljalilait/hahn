using Moq;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using FluentAssertions;
using tickets_api.Controllers;
using tickets_api.Models;
using tickets_api.Repositories;

namespace tickets_api.Tests
{
    public class TicketsControllerTests
    {
        private readonly Mock<ITicketRepository> _mockTicketRepository;
        private readonly TicketsController _controller;

        public TicketsControllerTests()
        {
            _mockTicketRepository = new Mock<ITicketRepository>();
            _controller = new TicketsController(_mockTicketRepository.Object);
        }

        [Fact]
        public async Task GetAllTickets_ShouldReturnPagedResult_WhenTicketsExist()
        {
            // Arrange
            var mockPagedResult = new PagedResult<Ticket>
            {
                data = new List<Ticket> { new Ticket { Id = 1, Description = "Test Ticket", Status = TicketStatus.Open } },
                TotalCount = 1
            };
            _mockTicketRepository.Setup(repo => repo.GetAllTicketsAsync(1, 10))
                .ReturnsAsync(mockPagedResult);

            // Act
            var result = await _controller.GetTickets(1, 10);

            // Assert
            var okResult = result as ActionResult<IEnumerable<Ticket>>;
            okResult.Should().NotBeNull();
            var tickets = okResult.Value as PagedResult<Ticket>;
            tickets.Should().NotBeNull();
            tickets.data.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetTicketById_ShouldReturnTicket_WhenTicketExists()
        {
            // Arrange
            var ticket = new Ticket { Id = 1, Description = "Test Ticket", Status = TicketStatus.Open };
            _mockTicketRepository.Setup(repo => repo.GetTicketByIdAsync(1))
                .ReturnsAsync(ticket);

            // Act
            var result = await _controller.GetTicket(1);

            // Assert
            var okResult = result as ActionResult<Ticket>;
            okResult.Should().NotBeNull();
            var ticketResult = okResult.Value as Ticket;
            ticketResult.Should().NotBeNull();
            ticketResult.Id.Should().Be(1);
            ticketResult.Description.Should().Be("Test Ticket");
        }

        [Fact]
        public async Task GetTicketById_ShouldReturnNotFound_WhenTicketDoesNotExist()
        {
            // Arrange
            _mockTicketRepository.Setup(repo => repo.GetTicketByIdAsync(1))
                .ReturnsAsync((Ticket)null);

            // Act
            var result = await _controller.GetTicket(1);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task CreateTicket_ShouldReturnCreatedAtAction_WhenTicketIsCreated()
        {
            // Arrange
            var ticket = new Ticket { Id = 1, Description = "New Ticket", Status = TicketStatus.Open };

            _mockTicketRepository.Setup(repo => repo.AddTicketAsync(ticket))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateTicket(ticket);

            // Assert
            var createdResult = result;
            createdResult.Should().NotBeNull();
            createdResult.Should().Be(nameof(_controller.GetTicket));
        }

        [Fact]
        public async Task UpdateTicket_ShouldReturnNoContent_WhenTicketIsUpdated()
        {
            // Arrange
            var ticket = new Ticket { Id = 1, Description = "Updated Ticket", Status = TicketStatus.Open };

            _mockTicketRepository.Setup(repo => repo.GetTicketByIdAsync(1))
                .ReturnsAsync(ticket);
            _mockTicketRepository.Setup(repo => repo.UpdateTicketAsync(ticket))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateTicket(1, ticket);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task UpdateTicket_ShouldReturnBadRequest_WhenIdsDoNotMatch()
        {
            // Arrange
            var ticket = new Ticket { Id = 2, Description = "Mismatched Ticket", Status = TicketStatus.Open };

            // Act
            var result = await _controller.UpdateTicket(1, ticket);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task DeleteTicket_ShouldReturnNoContent_WhenTicketIsDeleted()
        {
            // Arrange
            var ticket = new Ticket { Id = 1, Description = "Test Ticket" };

            _mockTicketRepository.Setup(repo => repo.GetTicketByIdAsync(1))
                .ReturnsAsync(ticket);
            _mockTicketRepository.Setup(repo => repo.DeleteTicketAsync(1))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteTicket(1);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteTicket_ShouldReturnNotFound_WhenTicketDoesNotExist()
        {
            // Arrange
            _mockTicketRepository.Setup(repo => repo.GetTicketByIdAsync(1))
                .ReturnsAsync((Ticket)null);

            // Act
            var result = await _controller.DeleteTicket(1);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
