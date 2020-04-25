using GraphQL.Types;
using Memoyed.Application.Dto;

namespace Memoyed.WebApi.GraphQL.Types
{
    public sealed class SessionCardType : ObjectGraphType<ReturnModels.SessionCardModel>
    {
        public SessionCardType()
        {
            Name = "SessionCard";
            Description = "A session card from a revision session that was created from a card";

            Field(c => c.SessionId)
                .Description("Id of a revision session this session cards belongs to");

            Field(c => c.CardId)
                .Description("Id of a card this session card was created from");

            Field(c => c.NativeLanguageWord)
                .Description("The word written in the language the user understands");

            Field(c => c.TargetLanguageWord)
                .Description("The word written in the language the user wants to learn");

            Field<SessionCardStatusType>("Status", "Status of answering the session card",
                resolve: c => c.Source.Status);
        }
    }
}