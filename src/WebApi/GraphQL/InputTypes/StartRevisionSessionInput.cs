using System;
using Memoyed.Application.Dto;

namespace Memoyed.WebApi.GraphQL.InputTypes
{
    /// <summary>
    /// An input object to start a revision session from a card box set
    /// </summary>
    public class StartRevisionSessionInput : Commands.StartRevisionSessionCommand
    {
        public StartRevisionSessionInput(Guid cardBoxSetId)
        {
            CardBoxSetId = cardBoxSetId;
        }

        /// <summary>
        /// Id of the card box set from which to start a revision session
        /// </summary>
        public Guid CardBoxSetId { get; }
    }
}