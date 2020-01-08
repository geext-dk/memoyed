using System;
using Memoyed.Cards.Domain.CardBoxes;
using Memoyed.DomainFramework;

namespace Memoyed.Cards.Domain.LearningCards
{
    public class LearningCard : ISnapshotable<ILearningCardSnapshot>
    {
        public LearningCard(LearningCardId id, LearningCardWord nativeLanguageWord, LearningCardWord targetLanguageWord,
            LearningCardComment comment)
        {
            Id = id;
            NativeLanguageWord = nativeLanguageWord;
            TargetLanguageWord = targetLanguageWord;
            Comment = comment;
        }

        private LearningCard(ILearningCardSnapshot snapshot) : this(
            new LearningCardId(snapshot.Id),
            new LearningCardWord(snapshot.NativeLanguageWord),
            new LearningCardWord(snapshot.TargetLanguageWord),
            new LearningCardComment(snapshot.Comment))
        {
            CardBoxChangedDate = snapshot.CardBoxChangedDate;
            if (snapshot.CardBoxId.HasValue)
            {
                CardBoxId = new CardBoxId(snapshot.CardBoxId.Value);
            }
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

        public ILearningCardSnapshot CreateSnapshot() => new Snapshot(this);
        public static LearningCard FromSnapshot(ILearningCardSnapshot snapshot) => new LearningCard(snapshot);

        private class Snapshot : ILearningCardSnapshot
        {
            private readonly LearningCard _card;

            public Snapshot(LearningCard card)
            {
                _card = card;
            }

            public Guid Id => _card.Id;
            public Guid? CardBoxId => _card.CardBoxId;
            public string NativeLanguageWord => _card.NativeLanguageWord;
            public string TargetLanguageWord => _card.TargetLanguageWord;
            public string Comment => _card.Comment;
            public DateTime? CardBoxChangedDate => _card.CardBoxChangedDate;
        }
    }
}