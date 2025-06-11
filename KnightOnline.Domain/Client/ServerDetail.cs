using KnightOnline.Domain.SharedKernel; // For Entity

namespace KnightOnline.Domain.Client
{
    // Represents the detailed information for a game server in the server list
    public class ServerDetail : Entity<int> // Assuming ServerId is the unique identifier
    {
        public string Name { get; private set; }
        public string IpAddress { get; private set; }
        public int Port { get; private set; }
        public int CurrentUsers { get; private set; } // Dynamic data, might be updated frequently
        public int MaxUsers { get; private set; }
        public string? ServerCategory { get; private set; } // e.g., "Normal", "Premium"

        private ServerDetail(int id, string name, string ipAddress, int port, int maxUsers, string? category) : base(id)
        {
            Name = name;
            IpAddress = ipAddress;
            Port = port;
            MaxUsers = maxUsers;
            ServerCategory = category;
            CurrentUsers = 0; // Initial state
        }

        public static ServerDetail Create(int id, string name, string ipAddress, int port, int maxUsers, string? category = null)
        {
            // Add validation for parameters
            return new ServerDetail(id, name, ipAddress, port, maxUsers, category);
        }

        public void UpdateCurrentUserCount(int count)
        {
            if (count < 0) count = 0;
            if (count > MaxUsers) count = MaxUsers;
            CurrentUsers = count;
        }
    }
}
