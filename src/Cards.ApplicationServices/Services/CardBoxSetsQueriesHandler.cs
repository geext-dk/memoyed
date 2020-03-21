using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Memoyed.Cards.ApplicationServices.Dto;
using Microsoft.EntityFrameworkCore;
using Dapper;
using Memoyed.Cards.ApplicationServices.DataModel;

namespace Memoyed.Cards.ApplicationServices.Services
{
    public class CardBoxSetsQueriesHandler
    {
        private readonly IDbConnection _connection;

        public CardBoxSetsQueriesHandler(CardsContext context)
        {
            _connection = context.Database.GetDbConnection();
        }
        
        public async Task<IEnumerable<ReturnModels.CardBoxSetModel>> GetCardBoxSets(Queries.GetCardBoxSetsQuery query)
        {
            const string getCardBoxSetsSql = @"SELECT Id, Name, NativeLanguage, TargetLanguage
                                                FROM CardBoxSets";

            return await _connection.QueryAsync<ReturnModels.CardBoxSetModel>(getCardBoxSetsSql);
        }

        public async Task<IEnumerable<ReturnModels.CardBoxModel>> GetCardBoxes(Queries.GetCardBoxesQuery query)
        {
            const string getCardBoxesSql = @"SELECT Id, SetId, Level, RevisionDelay
                                             FROM CardBoxes
                                             WHERE SetId = @SetId";

            return await _connection.QueryAsync<ReturnModels.CardBoxModel>(getCardBoxesSql, new
            {
                SetId = query.CardBoxSetId
            });
        }

        public async Task<IEnumerable<ReturnModels.CardModel>> GetCards(Queries.GetCardsQuery query)
        {
            const string getCardsSql = @"SELECT c.Id, c.NativeLanguageWord, c.TargetLanguageWord,
                                                    c.Comment, c.CardBoxId, b.SetId
                                                 FROM Cards AS c
                                                 INNER JOIN CardBoxes AS b ON b.Id = c.CardBoxId
                                                 WHERE c.CardBoxId = @CardBoxId AND b.SetId = @SetId";

            return await _connection.QueryAsync<ReturnModels.CardModel>(getCardsSql, new
            {
                CardBoxId = query.CardBoxId,
                SetId = query.CardBoxSetId
            });
        }
        
    }
}