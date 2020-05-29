using System;
using Memoyed.Application.Dto;

namespace Memoyed.WebApi.GraphQL.InputTypes
{
    /// <summary>
    /// Input type for creating card boxes
    /// </summary>
    public class CreateCardBoxInput : Commands.CreateCardBoxCommand
    {
        public CreateCardBoxInput(Guid cardBoxSetId, int level, int revisionDelay)
        {
            CardBoxSetId = cardBoxSetId;
            Level = level;
            RevisionDelay = revisionDelay;
        }

        /// <summary>
        /// Id of a set to create box in
        /// </summary>
        public Guid CardBoxSetId { get; }
        
        /// <summary>
        /// Level of the new card box
        /// </summary>
        public int Level { get; }
        
        /// <summary>
        /// Amount of days until revision of cards in the new box
        /// </summary>
        public int RevisionDelay { get; }
    }
}