using System;

namespace Memoyed.Cards.ApplicationServices.Dto
{
    public static class Queries
    {
        public class GetCardBoxSetsQuery
        {
        }

        public class GetCardBoxesQuery
        {
            public Guid CardBoxSetId { get; set; }
        }

        public class GetLearningCardsQuery
        {
            public Guid CardBoxSetId { get; set; }
            public Guid CardBoxId { get; set; }
        }
    }
}