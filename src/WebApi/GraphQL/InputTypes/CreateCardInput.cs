using System;
using Memoyed.Application.Dto;

namespace Memoyed.WebApi.GraphQL.InputTypes
{
    /// <summary>
    /// Input type for creating cards
    /// </summary>
    public class CreateCardInput : Commands.CreateCardCommand
    {
        public CreateCardInput(Guid cardBoxSetId, string nativeLanguageWord, string targetLanguageWord,
            string? comment)
        {
            CardBoxSetId = cardBoxSetId;
            NativeLanguageWord = nativeLanguageWord;
            TargetLanguageWord = targetLanguageWord;
            Comment = comment;
        }

        /// <summary>
        /// Id of the card box set to create the card in
        /// </summary>
        public Guid CardBoxSetId { get; }

        /// <summary>
        /// The word written in the native language
        /// </summary>
        public string NativeLanguageWord { get; }

        /// <summary>
        /// The word written in the target language
        /// </summary>
        public string TargetLanguageWord { get; }

        /// <summary>
        /// An additional comment to the word
        /// </summary>
        public string? Comment { get; }
    }
}