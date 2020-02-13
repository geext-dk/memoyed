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

        public class InvalidRevisionDelayException : DomainException
        {
            public InvalidRevisionDelayException() : base("The given days value should be" +
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

        public class DecreasingRevisionDelayException : DomainException
        {
            public DecreasingRevisionDelayException() : base("The repeat delays of the boxes should increase" +
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

        public class CardAlreadyInSetException : DomainException
        {
            public CardAlreadyInSetException() : base("The given card is already in the set")
            {
            }
        }

        public class CardNotInSetException : DomainException
        {
            public CardNotInSetException() : base("The given card is not in the set")
            {
            }
        }

        public class NoBoxesInSetException : DomainException
        {
            public NoBoxesInSetException() : base("The set doesn't contain any boxes")
            {
            }
        }

        public class CardBoxIdMismatchException : DomainException
        {
            public CardBoxIdMismatchException() : base("The given card doesn't belong to the card box. ")
            {
            }
        }

        public class RevisionSetIdMismatchException : DomainException
        {
            public RevisionSetIdMismatchException() : base("The set isn't linked to the revision")
            {
            }
        }

        public class PromoteFromLevelMismatchException : DomainException
        {
            public PromoteFromLevelMismatchException() : base("The box the card is contained in doesn't" +
                                                              " match with the level the card must be promoted from")
            {
            }
        }

        public class CardBoxNotFoundInSetException : DomainException
        {
            public CardBoxNotFoundInSetException() : base("The given card box is not found in the set")
            {
            }
        }

        public class LocalDateTimeException : DomainException
        {
            public LocalDateTimeException() : base("The given date time is local")
            {
            }
        }

        public class NoCardsForRevisionException : DomainException
        {
            public NoCardsForRevisionException() : base("Couldn't create a revision session." +
                                                        " No cards are eligible for a revision")
            {
            }
        }

        public class SessionCardNotFoundException : DomainException
        {
            public SessionCardNotFoundException() : base("Session card with the given id hasn't been found")
            {
            }
        }

        public class NotAllCardsAnsweredException : DomainException
        {
            public NotAllCardsAnsweredException() : base("All card must be answered to complete a session")
            {
            }
        }

        public class SessionAlreadyCompletedException : DomainException
        {
            public SessionAlreadyCompletedException() : base("Couldn't complete the session because it " +
                                                             "is already completed")
            {
            }
        }
    }
}