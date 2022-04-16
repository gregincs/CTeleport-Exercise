using CTeleport.Exercise.Application.Endpoints.Airports.Queires;
using FluentValidation;

namespace CTeleport.Exercise.Application.Endpoints.Airports.Validator
{
    public class AirportsDistanceQueryValidator : AbstractValidator<AirportsDistanceQuery>
    {
        public AirportsDistanceQueryValidator()
        {
            string originLengthMessage = "Origin airport IATA code must be 3 characters long";
            string destinyLengthMessage = "Destiny airport IATA code must be 3 characters long";

            RuleFor(x => x.Origin)
                .NotNull().WithMessage("Origin airport IATA code is mandatory")
                .MinimumLength(3).WithMessage(originLengthMessage)
                .MaximumLength(3).WithMessage(originLengthMessage);

            RuleFor(x => x.Destiny)
                .NotNull().WithMessage("Destiny airport IATA code is mandatory")
                .MinimumLength(3).WithMessage(destinyLengthMessage)
                .MaximumLength(3).WithMessage(destinyLengthMessage);
        }
    }
}
