using GraphQL.Types;
using Memoyed.Application.Dto;
using Memoyed.Domain.Cards.Cards;

namespace Memoyed.WebApi.GraphQL.Types
{
    public sealed class CardType : ObjectGraphType<ReturnModels.CardModel>
    {
        public CardType()
        {
            Name = "Card";
            Description = "Test";

            Field(c => c.Id).Description("The id of the card");
            Field(c => c.SetId).Description("Id of the set containing the card");
            Field(c => c.BoxId, nullable: true).Description("Id of the box containing the card");
            Field(c => c.NativeLanguageWord).Description("The word of the card written in the native language");
            Field(c => c.TargetLanguageWord).Description("The word of the card written in the target language");
            Field(c => c.Level, nullable: true).Description("Level of the card box the card belongs to");
            Field(c => c.Comment, nullable: true).Description("User's comment");
        }
    }
}