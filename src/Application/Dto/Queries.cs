using System;

namespace Memoyed.Application.Dto
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

        public class GetCardsQuery
        {
            public Guid CardBoxSetId { get; set; }
            public Guid CardBoxId { get; set; }
        }
    }
}