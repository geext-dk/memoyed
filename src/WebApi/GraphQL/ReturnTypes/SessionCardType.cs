using System;
using HotChocolate;
using Memoyed.Domain.Cards.RevisionSessions.SessionCards;

namespace Memoyed.WebApi.GraphQL.ReturnTypes
{
    /// <summary>
    /// A session card from a revision session that was created from a card
    /// </summary>
    [GraphQLName("SessionCard")]
    public class SessionCardType
    {
        /// <summary>
        /// Id of a revision session this session cards belongs to
        /// </summary>
        public Guid SessionId { get; set; }
        
        /// <summary>
        /// Id of a card this session card was created from
        /// </summary>
        public Guid CardId { get; set; }
        
        /// <summary>
        /// The word written in the language the user wants to learn
        /// </summary>
        public string TargetLanguageWord { get; set; }
        
        /// <summary>
        /// The word written in the language the user understands
        /// </summary>
        public string NativeLanguageWord { get; set; }
        
        /// <summary>
        /// Status of answering the session card
        /// </summary>
        public SessionCardStatus Status { get; set; }
    }
}