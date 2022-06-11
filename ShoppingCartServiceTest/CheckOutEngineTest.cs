using AutoMapper;
using Moq;
using ShoppingCartService.BusinessLogic;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Mapping;
using ShoppingCartService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShoppingCartServiceTest
{
    public class CheckOutEngineTest
    {
        Mock<IShippingCalculator> _mockShippingCalculator;
        ICheckOutEngine _checkOutEngine;

        public CheckOutEngineTest()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            
            IMapper mapper = mappingConfig.CreateMapper();

            _mockShippingCalculator = new Mock<IShippingCalculator>();
            _checkOutEngine = new CheckOutEngine(_mockShippingCalculator.Object, mapper);

        }

        [Fact]
        public void CalculateTotals_GivenACart_ThenCalculateTotals()
        {
            Cart cart = new Cart()
            {
                Id = "1",
                CustomerId = "1",
                CustomerType = CustomerType.Premium,
                ShippingMethod = ShippingMethod.Standard,
                ShippingAddress = new Address()
                {
                    Street = "Las lomas",
                    City = "La Paz",
                    Country = "Bolivia"
                },
                Items = new List<Item>()
                {
                    new Item()
                    {
                        ProductId = "1",
                        ProductName = "Product 1",
                        Price = 100,
                        Quantity = 1
                    }
                }
            };

            _mockShippingCalculator.Setup(x => x.CalculateShippingCost(It.IsAny<Cart>())).Returns(15);

            var result = _checkOutEngine.CalculateTotals(cart);

            Assert.Equal(103.5, result.Total);

        }
    }
}
