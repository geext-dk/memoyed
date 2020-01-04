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

        public void ChangeNativeLanguageWord(LearningCardWord nativeLanguageWord)
        {
            NativeLanguageWord = nativeLanguageWord;
        }

        public void ChangeTargetLanguageWord(LearningCardWord targetLanguageWord)
        {
            TargetLanguageWord = targetLanguageWord;
        }

        public void ChangeComment(LearningCardComment comment)
        {
            Comment = comment;
        }
        
        /// <summary>
        /// Id of the learning card
        /// </summary>
        public LearningCardId Id { get; }
        
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
    }
}