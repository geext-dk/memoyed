using System;
using Memoyed.Application.Dto;

namespace Memoyed.WebApi.GraphQL.InputTypes
{
    /// <summary>
    /// An input object for completing revision sessions
    /// </summary>
    public class CompleteRevisionSessionInput : Commands.CompleteRevisionSessionCommand
    {
        public CompleteRevisionSessionInput(Guid revisionSessionId)
        {
            RevisionSessionId = revisionSessionId;
        }

        /// <summary>
        /// Id of the revision session to complete
        /// </summary>
        public Guid RevisionSessionId { get; }
    }
}