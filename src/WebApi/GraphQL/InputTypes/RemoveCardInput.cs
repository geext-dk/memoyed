using GraphQL.Types;

namespace Memoyed.WebApi.GraphQL.InputTypes
{
    public class RemoveCardInput : InputObjectGraphType
    {
        public RemoveCardInput()
        {
            Name = "RemoveCardInput";
            Description = "Input type for removing cards";

            Field<NonNullGraphType<GuidGraphType>>("cardBoxSetId", "Id of a card box set");
            Field<NonNullGraphType<GuidGraphType>>("cardId", "Id of the card to delete");
        }
    }
}