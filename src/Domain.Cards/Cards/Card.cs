using System;
using Memoyed.Domain.Cards.CardBoxes;
using Memoyed.DomainFramework;

namespace Memoyed.Domain.Cards.Cards
{
    public class Card : Entity
    {
        public Card(CardId id, CardWord nativeLanguageWord, CardWord targetLanguageWord)
        {
            Id = id;
            NativeLanguageWord = nativeLanguageWord;
            TargetLanguageWord = targetLanguageWord;
            Comment = new CardComment(null);
        }

        private Card()
        {
        }

        /// <summary>
        ///     Id of the card
        /// </summary>
        public CardId Id { get; }

        /// <summary>
        ///     Id of the box that contains the card
        /// </summary>
        public CardBoxId? CardBoxId { get; private set; }

        /// <summary>
        ///     Word of the card written on the language native to the user
        /// </summary>
        public CardWord NativeLanguageWord { get; private set; }

        /// <summary>
        ///     Word of the card written on the language the user is learning
        /// </summary>
        public CardWord TargetLanguageWord { get; private set; }

        /// <summary>
        ///     A comment to the word
        /// </summary>
        public CardComment Comment { get; private set; }

        /// <summary>
        ///     The time the card was last time moved to another card box in UTC
        /// </summary>
        public DateTimeOffset? CardBoxChangedDate { get; private set; }

        /// <summary>
        ///     Changes the native language word
        /// </summary>
        /// <param name="nativeLanguageWord">A new word in a native language</param>
        public void ChangeNativeLanguageWord(CardWord nativeLanguageWord)
        {
            NativeLanguageWord = nativeLanguageWord;
        }

        /// <summary>
        ///     Change the target language word
        /// </summary>
        /// <param name="targetLanguageWord">A new word in a target language</param>
        public void ChangeTargetLanguageWord(CardWord targetLanguageWord)
        {
            TargetLanguageWord = targetLanguageWord;
        }

        /// <summary>
        ///     Change the comment
        /// </summary>
        /// <param name="comment">A new comment</param>
        public void ChangeComment(CardComment comment)
        {
            Comment = comment;
        }

        internal void ChangeCardBoxId(CardBoxId cardBoxId, DateTimeOffset now)
        {
            CardBoxChangedDate = now;
            CardBoxId = cardBoxId;
        }
    }
}