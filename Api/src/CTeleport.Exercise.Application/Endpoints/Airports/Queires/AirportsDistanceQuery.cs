namespace CTeleport.Exercise.Application.Endpoints.Airports.Queires
{
    public class AirportsDistanceQuery
    {
        public string Origin { get => _origin; set => _origin = value.ToUpper(); }
        public string Destiny { get => _destiny; set => _destiny = value.ToUpper(); }

        private string _origin;
        private string _destiny;
    }
}
