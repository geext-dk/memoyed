using GraphQL.Types;

namespace Memoyed.WebApi.GraphQL.InputTypes
{
    public class RenameCardBoxSetInput : InputObjectGraphType
    {
        public RenameCardBoxSetInput()
        {
            Name = "RenameCardBoxSetInput";
            Description = "An input type for renaming card box sets";

            Field<NonNullGraphType<GuidGraphType>>("cardBoxSetId", "Id of the card box set to rename");
            Field<NonNullGraphType<StringGraphType>>("name", "New name for the card box set");
        }
    }
}