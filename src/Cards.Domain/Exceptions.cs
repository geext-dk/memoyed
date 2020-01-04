using System;

namespace Memoyed.Cards.Domain
{
    public abstract class DomainException : Exception
    {
        private DomainException(string message) : base($"Cards.Domain Exception: {message}")
        {
        }

        public class EmptyWordException : DomainException
        {
            public EmptyWordException() : base("Words for cards should contain a non-whitespace character")
            {
            }
        }

        public class EmptyIdException : DomainException
        {
            public EmptyIdException() : base("Id shouldn't be empty")
            {
            }
        }

        public class InvalidLanguageException : DomainException
        {
            public InvalidLanguageException() : base("The given language string hasn't passed " +
                                                           "the language check")
            {
            }
        }

        public class CardBoxSetIdMismatchException : DomainException
        {
            public CardBoxSetIdMismatchException()
                : base($"The given card box is for other card box set")
            {
            }
        }

        public class CardBoxAlreadyInSetException : DomainException
        {
            public CardBoxAlreadyInSetException() : base("The given card box is in the " +
                                                                  "card box set already")
            {
            }
        }

        public class InvalidRepeatDelayException : DomainException
        {
            public InvalidRepeatDelayException() : base("The given days value should be" +
                                                                         " greater than 0 and lesser than 31")
            {
            }
        }

        public class InvalidCardBoxLevelException : DomainException
        {
            public InvalidCardBoxLevelException() : base("The card box level must be nonnegative")
            {
            }
        }

        public class DecreasingRepeatDelayException : DomainException
        {
            public DecreasingRepeatDelayException() : base("The repeat delays of the boxes should increase" +
                                                           " while the levels increase")
            {
            }
        }

        public class CardBoxLevelAlreadyExistException : DomainException
        {
            public CardBoxLevelAlreadyExistException() : base("The card box with that level already exists" +
                                                              " in the set")
            {
            }
        }

        public class LearningCardAlreadyInSetException : DomainException
        {
            public LearningCardAlreadyInSetException() : base("The given learning card is already in the set")
            {
            }
        }

        public class LearningCardNotInSetException : DomainException
        {
            public LearningCardNotInSetException() : base("The given learning card is not in the set")
            {
            }
        }

        public class NoBoxesInSetException : DomainException
        {
            public NoBoxesInSetException() : base("The set doesn't contain any boxes")
            {
            }
        }
    }
}