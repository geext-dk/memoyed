namespace Memoyed.Domain.Cards
{
    public static class DomainChecks
    {
        /// <summary>
        ///     A Delegate representing a function that should validate the passed language for correctness
        /// </summary>
        /// <param name="language">language code</param>
        public delegate bool ValidateLanguage(string language);
    }
}