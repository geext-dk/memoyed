using Memoyed.Domain.Cards.Services;

namespace Memoyed.Application.Services
{
    public class SimpleCardAnswerCheckService : ICardAnswerCheckService
    {
        public bool CheckAnswer(string word, string answer)
        {
            return word.Trim() == answer.Trim();
        }
    }
}