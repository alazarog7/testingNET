using ShoppingCartService.BusinessLogic.Validation;
using ShoppingCartService.Models;
using System;
using Xunit;

namespace ShoppingCartServiceTests
{
    public class AddressValidatorTest
    {
        [Fact]
        public void IsValid_NoNullValues_ReturnTrue()
        {
            AddressValidator validator = new AddressValidator();
            Address address = new Address()
            {
                City = "La Paz",
                Country = "Country",
                Street = "Pasaje Ortega #37"
            };

            var result = validator.IsValid(address);

            Assert.True(result);

        }

        [Fact]
        public void IsValid_NullValues_ReturnFalse()
        {
            AddressValidator validator = new AddressValidator();
            Address address = new Address()
            {
                Country = "Country",
                Street = "Pasaje Ortega #37"
            };

            Address address2 = null;

            var result = validator.IsValid(address);
            var result2 = validator.IsValid(address2);

            Assert.False(result);
            Assert.False(result2);
        }
    }
}
