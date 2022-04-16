using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTeleport.Exercise.Application.Endpoints.Airports.Queires;
using CTeleport.Exercise.Application.Endpoints.Airports.Validator;
using FluentAssertions;
using Xunit;

namespace CTeleport.Exercise.Application.Tests.Endpoints.Airports.Validator
{
    public class AirportsDistanceQueryValidatorTests
    {
        private AirportsDistanceQueryValidator validator;

        public AirportsDistanceQueryValidatorTests()
        {
            validator = new AirportsDistanceQueryValidator();
        }

        [Fact]
        public void Should_give_error_length_incorrect()
        {
            //Arrange
            var request = new AirportsDistanceQuery
            {
                Destiny = "GR",
                Origin = ""
            };

            //Act
            var result = validator.Validate(request);

            //Assert
            result.Should().NotBeNull();
            result.Errors.Should().HaveCount(2)
                .And.SatisfyRespectively(
                    first =>
                    {
                        first.ErrorMessage.Should().Be("Origin airport IATA code must be 3 characters long");
                    },
                    second =>
                    {
                        second.ErrorMessage.Should().Be("Destiny airport IATA code must be 3 characters long");
                    }
                );

        }

        [Fact]
        public void Should_give_error_origin_and_destiny_is_mandatory()
        {
            //Arrange
            var request = new AirportsDistanceQuery();

            //Act
            var result = validator.Validate(request);

            //Assert
            result.Should().NotBeNull();
            result.Errors.Should().HaveCount(2)
                .And.SatisfyRespectively(
                    first =>
                    {
                        first.ErrorMessage.Should().Be("Origin airport IATA code is mandatory");
                    },
                    second =>
                    {
                        second.ErrorMessage.Should().Be("Destiny airport IATA code is mandatory");
                    }
                );

        }


    }
}
