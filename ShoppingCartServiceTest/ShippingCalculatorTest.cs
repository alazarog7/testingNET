using ShoppingCartService.BusinessLogic;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShoppingCartServiceTest
{
    public class ShippingCalculatorTest
    {
        private IShippingCalculator _shippingCalculator;
        public ShippingCalculatorTest()
        {
            _shippingCalculator = new ShippingCalculator();
        }

        [Fact]
        public void CalculateShippingCost_HavingProducts_ReturnTotalCost()
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

            var total = _shippingCalculator.CalculateShippingCost(cart);

            Assert.Equal(15, total);
        }


        [Fact]
        public void CalculateShippingCost_HavingNoProducts_ReturnTotalCostZero()
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
                }
            };

            var total = _shippingCalculator.CalculateShippingCost(cart);

            Assert.Equal(0, total);
        }
    }
}
