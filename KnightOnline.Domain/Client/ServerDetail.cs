using KnightOnline.Domain.SharedKernel;

namespace KnightOnline.Domain.Client
{
    public class ServerDetail : Entity<int>
    {
        public string Name { get; private set; }
        public string IpAddress { get; private set; }
        public int Port { get; private set; }
        public int CurrentUsers { get; private set; }
        public int MaxUsers { get; private set; }
        public string? ServerCategory { get; private set; }
        public int UserMaxFree { get; private set; } // Added

        private ServerDetail(int id, string name, string ipAddress, int port, int maxUsers, int userMaxFree, string? category) : base(id) // Added userMaxFree
        {
            Name = name;
            IpAddress = ipAddress;
            Port = port;
            MaxUsers = maxUsers;
            UserMaxFree = userMaxFree; // Added
            ServerCategory = category;
            CurrentUsers = 0;
        }

        public static ServerDetail Create(int id, string name, string ipAddress, int port, int maxUsers, int userMaxFree, string? category = null) // Added userMaxFree
        {
            // Add validation for parameters
            return new ServerDetail(id, name, ipAddress, port, maxUsers, userMaxFree, category); // Added userMaxFree
        }

        public void UpdateCurrentUserCount(int count)
        {
            if (count < 0) count = 0;
            if (count > MaxUsers) count = MaxUsers; // Should it be MaxUsers or UserMaxFree or combined logic? For now, MaxUsers.
            CurrentUsers = count;
        }
    }
}
