using MongoDB.Bson;
using ShoppingCartService.Config;
using ShoppingCartService.DataAccess;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ShoppingCartServiceTest.DataAccess
{
    public class SoppingCartRepositoryIntegrationTest : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _databaseFixture;
        private readonly IShoppingCartDatabaseSettings _settings;
        public SoppingCartRepositoryIntegrationTest(DatabaseFixture databaseFixture)
        {
            _databaseFixture = databaseFixture;
            _settings = new ShoppingCartDatabaseSettings()
            {
                CollectionName = "ShoppingCart",
                ConnectionString = "mongodb://localhost:1111",
                DatabaseName = "ShoppingCartDb"
            };
        }

        [Fact]
        public void Create_WhenIsCalled_ThenReturnCartRecord()
        {
            //Arrange
            var repository = new ShoppingCartRepository(_settings);

            var cart = new Cart()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                CustomerId = Guid.NewGuid().ToString(),
                CustomerType = CustomerType.Standard,
                ShippingMethod = ShippingMethod.Standard,
                ShippingAddress = new Address()
                {
                    Country = "Bolivia",
                    City = "La Paz",
                    Street = "Calle 1"
                },
                Items = new List<Item>()
                {
                    new Item()
                    {
                        ProductId = Guid.NewGuid().ToString(),
                        ProductName = "Test1",
                        Price = 10,
                        Quantity = 1,
                    }
                }
            };

            //Act
            var cartCreated = repository.Create(cart);

            //Assert
            Assert.False(string.IsNullOrEmpty(cartCreated.Id));
        }

        [Fact]
        public void FindAll_WhenIsCalled_ThenReturnAllCarts()
        {
            //Arrange
            var repository = new ShoppingCartRepository(_settings);

            //Act
            var carts = repository.FindAll();

            //Assert
            Assert.True(carts.Any());
        }


        [Fact]
        public void FindById_WhenIsCalled_ThenReturnSpecifiedCart()
        {
            //Arrange
            var repository = new ShoppingCartRepository(_settings);
            var cart = repository.FindAll().FirstOrDefault();

            //Act
            var cartFound = repository.FindById(cart.Id);

            //Assert
            Assert.NotNull(cartFound);
        }

        [Fact]
        public void Update_WhenIsCalled_ThenReturnUpdatedCart()
        {
            // Arrange
            var repository = new ShoppingCartRepository(_settings);
            var cart = repository.FindAll().FirstOrDefault();

            cart.ShippingMethod = ShippingMethod.Express;

            // Act
            repository.Update(cart.Id, cart);

            // Assert
            var cartUpdated = repository.FindById(cart.Id);
            Assert.Equal(ShippingMethod.Express, cartUpdated.ShippingMethod);
        }


        [Fact]
        public void Remove_WhenRemoveIsCalled_ThenCartNoLongerExistsOnDatabase()
        {
            // Arrange
            var repository = new ShoppingCartRepository(_settings);
            var cart = repository.FindAll().FirstOrDefault();

            // Act
            repository.Remove(cart.Id);

            // Assert
            var cartUpdated = repository.FindById(cart.Id);
            Assert.Null(cartUpdated);
        }
    }
}
