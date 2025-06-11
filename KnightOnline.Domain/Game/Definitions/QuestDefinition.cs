using System.Collections.Generic; // For List

namespace KnightOnline.Domain.Game.Definitions
{
    public enum QuestObjectiveType
    {
        KillMonster,
        CollectItem,
        TalkToNpc
        // Add more as needed
    }

    public class QuestReward
    {
        public long Experience { get; set; }
        public int Gold { get; set; }
        public List<QuestRewardItem> Items { get; set; }

        public QuestReward()
        {
            Items = new List<QuestRewardItem>();
        }
    }

    public class QuestRewardItem
    {
        public int ItemId { get; set; }
        public int Count { get; set; }
        // public byte ClassRestriction { get; set; } // Optional: if item reward varies by class

        public QuestRewardItem(int itemId, int count)
        {
            ItemId = itemId;
            Count = count;
        }
    }

    public class QuestObjective
    {
        public QuestObjectiveType Type { get; set; }
        public int TargetId { get; set; } // e.g., MonsterId to kill, ItemId to collect, NpcId to talk to
        public int RequiredCount { get; set; }
        public string Description { get; set; } // e.g. "Defeat 10 Orc Warriors"

        public QuestObjective()
        {
            Description = string.Empty;
            RequiredCount = 1;
        }
    }

    public class QuestDefinition
    {
        public int Id { get; set; } // Quest ID
        public string Title { get; set; } // Quest Title
        public string Summary { get; set; } // Short description shown in quest log before accepting
        public string Details { get; set; } // Full description, dialogs

        public int RequiredLevel { get; set; }
        // public int RequiredQuestId { get; set; } // Prerequisite quest ID
        // public List<int> ForbiddenQuestIds { get; set; } // Quests that prevent this one

        public int StartNpcId { get; set; }
        public int EndNpcId { get; set; }

        public List<QuestObjective> Objectives { get; set; }
        public QuestReward Rewards { get; set; }

        // public bool IsRepeatable { get; set; }
        // public int TimeLimitMinutes { get; set; } // Optional time limit

        public QuestDefinition()
        {
            Title = string.Empty;
            Summary = string.Empty;
            Details = string.Empty;
            Objectives = new List<QuestObjective>();
            Rewards = new QuestReward();
            // ForbiddenQuestIds = new List<int>();
        }
    }
}
