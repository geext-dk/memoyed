using GraphQL.Types;

namespace Memoyed.WebApi.GraphQL.InputTypes
{
    public class CardInput : InputObjectGraphType
    {
        public CardInput()
        {
            Name = "CardInput";
            Description = "Input type for creating cards";

            Field<NonNullGraphType<GuidGraphType>>("cardBoxSetId", "Id of the card box set to create the card in");
            Field<NonNullGraphType<StringGraphType>>("nativeLanguageWord", "The word written in the native language");
            Field<NonNullGraphType<StringGraphType>>("targetLanguageWord", "The word written in the target language");
            Field<StringGraphType>("comment", "An additional comment to the word");
        }
    }
}