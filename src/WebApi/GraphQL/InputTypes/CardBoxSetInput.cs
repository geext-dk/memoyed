using GraphQL.Types;

namespace Memoyed.WebApi.GraphQL.InputTypes
{
    public class CardBoxSetInput : InputObjectGraphType
    {
        public CardBoxSetInput()
        {
            Name = "CardBoxSetInput";
            Description = "Input type for creating a card box set";

            Field<NonNullGraphType<StringGraphType>>("name",
                "Name of the card box set to create. Should be unique");
            Field<NonNullGraphType<StringGraphType>>("nativeLanguage", "A language the user knows");
            Field<NonNullGraphType<StringGraphType>>("targetLanguage", "A language the user wants to learn");
        }
    }
}