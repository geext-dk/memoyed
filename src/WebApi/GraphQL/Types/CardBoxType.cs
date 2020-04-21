using System.Collections.Generic;
using Dapper;
using GraphQL.Types;
using Memoyed.Application.DataModel;
using Memoyed.Application.Dto;
using Microsoft.EntityFrameworkCore;

namespace Memoyed.WebApi.GraphQL.Types
{
    public sealed class CardBoxType : ObjectGraphType<ReturnModels.CardBoxModel>
    {
        public CardBoxType(CardsContext cardsDb)
        {
            var connection = cardsDb.Database.GetDbConnection();
            Name = "CardBox";
            Description = "A single box from a card box set that contains cards";

            Field(c => c.Id).Description("The id of the card box");
            Field(c => c.SetId).Description("Id of the set containing the box belongs to");
            Field(c => c.Level).Description("Level of the box. The more the level, the more " +
                                            "days should pass until a revision of a contained card");
            Field(c => c.RevisionDelay).Description("Amount of days should pass until a " +
                                                    "revision of a contained card");
            FieldAsync<ListGraphType<CardType>, IEnumerable<ReturnModels.CardModel>>("cards", 
                resolve: async c =>
            {
                const string sql = @"SELECT c.Id, c.NativeLanguageWord, c.TargetLanguageWord,
                                                    c.Comment, c.CardBoxId, b.SetId
                                                 FROM Cards AS c
                                                 INNER JOIN CardBoxes AS b ON b.Id = c.CardBoxId
                                                 WHERE b.Id = @CardBoxId";
                return await connection.QueryAsync<ReturnModels.CardModel>(sql, new
                {
                    CardBoxId = c.Source.Id
                });
            });
        }
    }
}