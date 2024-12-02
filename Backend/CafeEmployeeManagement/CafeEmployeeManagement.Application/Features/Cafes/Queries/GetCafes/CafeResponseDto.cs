namespace CafeEmployeeManagement.Application.Features.Cafes.Queries.GetCafes
{
    public class CafeResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Logo { get; set; }
        public string Location { get; set; }
        public int Employees { get; set; }
    }
}
