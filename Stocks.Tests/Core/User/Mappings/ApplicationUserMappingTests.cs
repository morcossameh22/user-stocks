using System;
using AutoMapper;
using Stocks.Core.DTO;
using Stocks.Core.Entities;
using Stocks.Core.Identity;
using Stocks.Core.Stock.DTO;
using Stocks.Core.Stock.Mappings;
using Stocks.Core.User.Mappings;

namespace Stocks.Tests.Core.User.Mappings
{
	public class ApplicationUserMappingTests
    {
        private IMapper _mapper;

        public ApplicationUserMappingTests()
		{
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ApplicationUserMapping>();
            });

            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void ShouldMapStockEntityToStockResponse()
        {
            var registerDTO = new RegisterDTO
            {
                Email = "test@test.com",
                PhoneNumber = "123456789",
                PersonName = "Test Name"
            };

            var user = _mapper.Map<ApplicationUser>(registerDTO);

            Assert.Equal(registerDTO.Email, user.Email);
            Assert.Equal(registerDTO.PhoneNumber, user.PhoneNumber);
            Assert.Equal(registerDTO.Email, user.UserName);
            Assert.Equal(registerDTO.PersonName, user.PersonName);
        }
    }
}

