using System;
using Memoyed.Cards.Domain.CardBoxes;

namespace Memoyed.Cards.Domain.LearningCards
{
    public class LearningCard
    {
        public LearningCard(LearningCardId id, LearningCardWord nativeLanguageWord, LearningCardWord targetLanguageWord,
            LearningCardComment comment)
        {
            Id = id;
            NativeLanguageWord = nativeLanguageWord;
            TargetLanguageWord = targetLanguageWord;
            Comment = comment;
        }
        
        /// <summary>
        /// Id of the learning card
        /// </summary>
        public LearningCardId Id { get; }
        
        /// <summary>
        /// Id of the box that contains the card
        /// </summary>
        public CardBoxId? CardBoxId { get; private set; }
        
        /// <summary>
        /// Word of the card written on the language native to the user
        /// </summary>
        public LearningCardWord NativeLanguageWord { get; private set; }
        
        /// <summary>
        /// Word of the card written on the language the user is learning
        /// </summary>
        public LearningCardWord TargetLanguageWord { get; private set; }
        
        /// <summary>
        /// A comment to the word
        /// </summary>
        public LearningCardComment Comment { get; private set; }
        
        /// <summary>
        /// The time the card was last time moved to another card box in UTC
        /// </summary>
        public DateTime? CardBoxChangedDate { get; private set; }

        /// <summary>
        /// Changes the native language word
        /// </summary>
        /// <param name="nativeLanguageWord">A new word in a native language</param>
        public void ChangeNativeLanguageWord(LearningCardWord nativeLanguageWord)
        {
            NativeLanguageWord = nativeLanguageWord;
        }

        /// <summary>
        /// Change the target language word
        /// </summary>
        /// <param name="targetLanguageWord">A new word in a target language</param>
        public void ChangeTargetLanguageWord(LearningCardWord targetLanguageWord)
        {
            TargetLanguageWord = targetLanguageWord;
        }

        /// <summary>
        /// Change the comment
        /// </summary>
        /// <param name="comment">A new comment</param>
        public void ChangeComment(LearningCardComment comment)
        {
            Comment = comment;
        }

        internal void ChangeCardBoxId(CardBoxId cardBoxId)
        {
            CardBoxChangedDate = DateTime.UtcNow;
            CardBoxId = cardBoxId;
        }
    }
}