using Memoyed.Application.Dto;

namespace Memoyed.WebApi.GraphQL.InputTypes
{
    /// <summary>
    /// Input type for creating a card box set
    /// </summary>
    public class CreateCardBoxSetInput : Commands.CreateCardBoxSetCommand
    {
        public CreateCardBoxSetInput(string name, string nativeLanguage, string targetLanguage)
        {
            Name = name;
            NativeLanguage = nativeLanguage;
            TargetLanguage = targetLanguage;
        }
        
        /// <summary>
        /// Name of the card box set to create. Should be unique
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// A language the user knows
        /// </summary>
        public string NativeLanguage { get; }
        
        /// <summary>
        /// A language the user wants to learn
        /// </summary>
        public string TargetLanguage { get; }
    }
}