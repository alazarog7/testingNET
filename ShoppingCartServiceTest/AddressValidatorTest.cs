using ShoppingCartService.BusinessLogic.Validation;
using ShoppingCartService.Models;
using System;
using Xunit;

namespace ShoppingCartServiceTest
{
    public class AddressValidatorTest
    {
        private IAddressValidator _addressValidator;

        public AddressValidatorTest()
        {
            _addressValidator = new AddressValidator();
        }

        [Fact]
        public void IsValid_WhenACorrectAddressIsPasses_ThenReturnTrue()
        {
            Address address = new Address()
            {
                Country = "Bolivia",
                City = "La Paz",
                Street = "Calle Ballivian"
            };
            
            bool result = _addressValidator.IsValid(address);

            Assert.True(result);
        }

        [Fact]
        public void IsValid_WhenIncorrectAddressIsPasses_ThenReturnFalse()
        {
            Address address = new Address()
            {
                Country = "",
                City = "La Paz",
                Street = "Calle Ballivian"
            };

            bool result = _addressValidator.IsValid(address);

            Assert.False(result);
        }
    }
}
