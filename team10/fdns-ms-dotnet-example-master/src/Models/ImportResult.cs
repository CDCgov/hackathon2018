using System;
using System.Collections.Generic;
using Foundation.Sdk;

namespace Foundation.Example.WebUI.Models
{
    /// <summary>
    /// Class representing the result of a bulk import
    /// </summary>
    public sealed class ImportResult
    {
        /// <summary>
        /// The IDs that were imported
        /// </summary>
        public Dictionary<string, string> ImportedIds { get; set; }

        /// <summary>
        /// The IDs that were skipped
        /// </summary>
        public Dictionary<string, string> SkippedIds { get; set; }

        /// <summary>
        /// Metadata for the CSV file that was pushed to the FDNS Storage service
        /// </summary>
        /// <value></value>
        public StorageMetadata StorageMetadata { get; set; }
    }
}