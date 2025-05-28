namespace KnightOnline.Application.DTOs
{
    public class PlayerDto
    {
        public int Id { get; set; } // Typically the domain ID
        public string Name { get; set; }
        public int Level { get; set; }
        // Other relevant player data for client
    }
}
