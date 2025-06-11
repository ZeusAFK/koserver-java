namespace KnightOnline.Domain.Game.Definitions
{
    public class QuestDefinition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RequiredLevel { get; set; }
        // Add other properties like Objectives, Rewards, NpcStart, NpcEnd, etc.

        public QuestDefinition()
        {
            Name = string.Empty;
        }
    }
}
