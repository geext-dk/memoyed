using Memoyed.Domain.Cards.RevisionSessions;
using Memoyed.Domain.Cards.RevisionSessions.SessionCards;

namespace Memoyed.Domain.Cards.Services
{
    public interface ICardAnswerCheckService
    {
        bool CheckAnswer(string word, string answer);
    }
}