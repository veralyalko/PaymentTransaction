using Xunit;
using Moq;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentTransactions.Controllers;
using PaymentTransaction.Repositories;
using PaymentTransaction.Models.DTO;
using PaymentTransaction.Models.Domain;
using PaymentTransaction.Data;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PaymentTransaction.Tests.Controllers
{
  public class PaymentTransactionControllerTests
  {
      private readonly Mock<ITransactionRepository> _transactionRepoMock = new();
      private readonly Mock<IProviderRepository> _providerRepoMock = new();
      private readonly Mock<IMapper> _mapperMock = new();
      private readonly PaymentTransactionController _controller;

      public PaymentTransactionControllerTests()
      {
          var dbContextMock = new Mock<PaymentTransactionDbContext>();

          _controller = new PaymentTransactionController(
              dbContextMock.Object,
              _transactionRepoMock.Object,
              _providerRepoMock.Object,
              _mapperMock.Object
          );

          // Needed if your controller reads headers
          _controller.ControllerContext = new ControllerContext
          {
              HttpContext = new DefaultHttpContext()
          };
      }

      private AddTransactionRequestDto CreateValidTransactionDto() => new()
      {
          PaymentMethodId = Guid.Parse("7b84f982-aab2-4495-9b2d-9bcbab2d08d8"),
          Amount = (double)100.00m,
          PayerEmail = "payer@example.com",
          CurrencyId = Guid.Parse("7b84f982-aab2-4495-9b2d-9bcbab2308d7"),
          ProviderId = Guid.Parse("7b84f982-aab2-4495-9b2d-9bcbab2308d4"),
          StatusId = Guid.Parse("7b84f982-aab2-4495-9b2d-9bcbab2308d8"),
      };

      // GetById() returns 404 if the transaction doesn't exist
      [Fact]
      public async Task GetById_WhenTransactionNotFound_ReturnsNotFound()
      {
          // Arrange
          var fakeId = Guid.NewGuid();
          _transactionRepoMock.Setup(r => r.GetByIdAsync(fakeId)).ReturnsAsync((Transaction?)null);

          // Act
          var result = await _controller.GetById(fakeId);

          // Assert
          Assert.IsType<NotFoundResult>(result);
      }

      // Delete() returns 404 when nothing is deleted
      [Fact]
      public async Task Delete_WhenTransactionNotFound_ReturnsNotFound()
      {
          var fakeId = Guid.NewGuid();
          _transactionRepoMock.Setup(r => r.DeleteAsync(fakeId)).ReturnsAsync((Transaction?)null);

          var result = await _controller.Delete(fakeId);

          Assert.IsType<NotFoundResult>(result);
      }

      // Create() returns 201 Created when a new transaction is added
      [Fact]
      public async Task Create_ValidInput_ReturnsCreatedAtAction()
      {
          // Arrange
          var requestDto = CreateValidTransactionDto(); // uses your existing helper

          var domainModel = new Transaction
          {
              Id = Guid.NewGuid(),
              Amount = requestDto.Amount,
              PayerEmail = requestDto.PayerEmail,
              ProviderId = requestDto.ProviderId,
              CurrencyId = requestDto.CurrencyId,
              PaymentMethodId = requestDto.PaymentMethodId,
              StatusId = requestDto.StatusId,
              Timestamp = DateTime.UtcNow,
              IdempotencyKey = "test-key",

              Provider = new Provider
              {
                  ProviderId = requestDto.ProviderId,
                  ProviderName = "Test Provider",
                  Type = ProviderType.PayPal, // or whatever enum value makes sense
                  MetadataJson = "{}"
              },
              Currency = new Currency
              {
                  CurrencyId = requestDto.CurrencyId,
                  CurrencyName = "USD"
              },
              PaymentMethod = new PaymentMethod
              {
                  PaymentMethodId = requestDto.PaymentMethodId,
                  PaymentMethodName = "Credit Card"
              },
              Status = new Status
              {
                  StatusId = requestDto.StatusId,
                  StatusName = "Completed"
              }
          };

          var responseDto = new TransactionDto
          {
              Id = domainModel.Id,
            Amount = domainModel.Amount,
            PayerEmail = domainModel.PayerEmail,
            Timestamp = domainModel.Timestamp,

            Provider = new ProviderDto
            {
                ProviderId = domainModel.Provider.ProviderId,
                ProviderName = domainModel.Provider.ProviderName,
                Type = domainModel.Provider.Type,
                MetadataJson = domainModel.Provider.MetadataJson
            },
            Currency = new CurrencyDto
            {
                CurrencyId = domainModel.Currency.CurrencyId,
                CurrencyName = domainModel.Currency.CurrencyName
            },
            PaymentMethod = new PaymentMethodDto
            {
                PaymentMethodId = domainModel.PaymentMethod.PaymentMethodId,
                PaymentMethodName = domainModel.PaymentMethod.PaymentMethodName
            },
            Status = new StatusDto
            {
                StatusId = domainModel.Status.StatusId,
                StatusName = domainModel.Status.StatusName
            }
          };

          _mapperMock.Setup(m => m.Map<Transaction>(requestDto)).Returns(domainModel);
          _transactionRepoMock.Setup(r => r.CreateAsync(domainModel)).ReturnsAsync(domainModel);
          _mapperMock.Setup(m => m.Map<TransactionDto>(domainModel)).Returns(responseDto);

          var result = await _controller.Create(requestDto) as CreatedAtActionResult;

          // Assert
          Assert.NotNull(result);
          Assert.Equal(nameof(_controller.GetById), result.ActionName);
          Assert.Equal(responseDto.Id, ((TransactionDto)result.Value!).Id);
      }

    }
}