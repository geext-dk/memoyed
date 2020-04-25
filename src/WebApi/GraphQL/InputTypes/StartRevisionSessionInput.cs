using GraphQL.Types;

namespace Memoyed.WebApi.GraphQL.InputTypes
{
    public class StartRevisionSessionInput : InputObjectGraphType
    {
        public StartRevisionSessionInput()
        {
            Name = "StartRevisionSessionInput";
            Description = "An input object to start a revision session from a card box set";

            Field<NonNullGraphType<GuidGraphType>>("cardBoxSetId",
                "Id of the card box set from which to start a revision session");
        }
    }
}