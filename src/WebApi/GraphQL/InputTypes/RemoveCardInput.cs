using System;
using Memoyed.Application.Dto;

namespace Memoyed.WebApi.GraphQL.InputTypes
{
    /// <summary>
    /// Input type for removing cards
    /// </summary>
    public class RemoveCardInput : Commands.RemoveCardCommand
    {
        public RemoveCardInput(Guid cardBoxSetId, Guid cardId)
        {
            CardBoxSetId = cardBoxSetId;
            CardId = cardId;
        }

        /// <summary>
        /// Id of a card box set
        /// </summary>
        public Guid CardBoxSetId { get; }
        
        /// <summary>
        /// Id of the card to delete
        /// </summary>
        public Guid CardId { get; }
    }
}