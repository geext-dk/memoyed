using GraphQL.Types;

namespace Memoyed.WebApi.GraphQL.InputTypes
{
    public class CompleteRevisionSessionInput : InputObjectGraphType
    {
        public CompleteRevisionSessionInput()
        {
            Name = "CompleteRevisionSessionInput";
            Description = "An input object for completing revision sessions";

            Field<NonNullGraphType<GuidGraphType>>("RevisionSessionId",
                "Id of the revision session to complete");
        }
    }
}