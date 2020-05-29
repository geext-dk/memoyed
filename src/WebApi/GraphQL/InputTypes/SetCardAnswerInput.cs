using System;
using Memoyed.Application.Dto;
using Memoyed.Domain.Cards.RevisionSessions;

namespace Memoyed.WebApi.GraphQL.InputTypes
{
    /// <summary>
    /// An input for answering session cards
    /// </summary>
    public class SetCardAnswerInput : Commands.SetCardAnswerCommand
    {
        public SetCardAnswerInput(Guid revisionSessionId, Guid cardId, string answer, SessionCardAnswerType answerType)
        {
            RevisionSessionId = revisionSessionId;
            CardId = cardId;
            Answer = answer;
            AnswerType = answerType;
        }

        /// <summary>
        /// Id of the revision session to which the card belongs
        /// </summary>
        public Guid RevisionSessionId { get; }
        
        /// <summary>
        /// Id of the card to answer
        /// </summary>
        public Guid CardId { get; }
        
        /// <summary>
        /// An answer the user gave
        /// </summary>
        public string Answer { get; }
        
        /// <summary>
        /// A type of the answer
        /// </summary>
        public SessionCardAnswerType AnswerType { get; }
    }
}