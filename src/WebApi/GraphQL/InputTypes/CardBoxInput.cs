using GraphQL.Types;

namespace Memoyed.WebApi.GraphQL.InputTypes
{
    public class CardBoxInput : InputObjectGraphType
    {
        public CardBoxInput()
        {
            Name = "CardBoxInput";
            Description = "Input type for creating card boxes";

            Field<NonNullGraphType<GuidGraphType>>("cardBoxSetId", "id of a set to create box in");
            Field<NonNullGraphType<IntGraphType>>("level", "Level of the new card box");
            Field<NonNullGraphType<IntGraphType>>("revisionDelay",
                "Amount of days until revision of cards in the new box");
        }
    }
}