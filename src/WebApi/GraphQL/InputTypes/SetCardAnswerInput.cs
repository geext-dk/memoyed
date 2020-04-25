using GraphQL.Types;
using Memoyed.WebApi.GraphQL.Types;

namespace Memoyed.WebApi.GraphQL.InputTypes
{
    public class SetCardAnswerInput : InputObjectGraphType
    {
        public SetCardAnswerInput()
        {
            Name = "SetCardAnswerInput";
            Description = "An input for answering session cards";

            Field<NonNullGraphType<GuidGraphType>>("RevisionSessionId",
                "Id of the revision session to which the card belongs");
            Field<NonNullGraphType<GuidGraphType>>("CardId",
                "Id of the card to answer");
            Field<NonNullGraphType<StringGraphType>>("Answer", "An answer the user gave");
            Field<NonNullGraphType<SessionCardAnswerTypeType>>("AnswerType", "A type of the answer");
        }
    }
}