using System;
using HotChocolate;

namespace Memoyed.WebApi.GraphQL.ReturnTypes
{
    /// <summary>
    /// A single card with two words on it, one written in the native language of the user, the other written in the
    /// target language of the user
    /// </summary>
    [GraphQLName("Card")]
    public class CardType
    {
        /// <summary>
        /// The id of the card
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Id of the set containing the card
        /// </summary>
        public Guid SetId { get; set; }
        
        /// <summary>
        /// Id of the box containing the card
        /// </summary>
        public Guid? CardBoxId { get; set; }
        
        /// <summary>
        /// Level of the card box the card belongs to
        /// </summary>
        public int? Level { get; set; }
        
        /// <summary>
        /// The date after which the card will appear in next revision
        /// </summary>
        public DateTimeOffset? RevisionAllowedDate { get; set; }
        
        /// <summary>
        /// The word of the card written in the target language
        /// </summary>
        public string TargetLanguageWord { get; set; }
        
        /// <summary>
        /// The word of the card written in the native language
        /// </summary>
        public string NativeLanguageWord { get; set; }
        
        /// <summary>
        /// Owner's comment
        /// </summary>
        public string Comment { get; set; }
    }
}