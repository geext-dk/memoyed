using System;
using Memoyed.Application.Dto;

namespace Memoyed.WebApi.GraphQL.InputTypes
{
    /// <summary>
    /// An input type for renaming card box sets
    /// </summary>
    public class RenameCardBoxSetInput : Commands.RenameCardBoxSetCommand
    {
        public RenameCardBoxSetInput(Guid cardBoxSetId, string name)
        {
            CardBoxSetId = cardBoxSetId;
            Name = name;
        }

        /// <summary>
        /// Id of the card box set to rename
        /// </summary>
        public Guid CardBoxSetId { get; }
        
        /// <summary>
        /// New name for the card box set
        /// </summary>
        public string Name { get; }

        string Commands.RenameCardBoxSetCommand.CardBoxSetName => Name;
    }
}