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
    public class SoppingCartRepositoryTest : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _databaseFixture;
        private readonly IShoppingCartDatabaseSettings _settings;
        public SoppingCartRepositoryTest(DatabaseFixture databaseFixture)
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

            Assert.NotEmpty(cartCreated.Id);
        }
    }
}
