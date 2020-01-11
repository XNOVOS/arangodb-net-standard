namespace ArangoDBNetStandard.DatabaseApi.Models
{
    /// <summary>
    /// Represents information about the current database.
    /// </summary>
    public class CurrentDatabaseInfo
    {
        public CurrentDatabaseInfo(string name, string id, string path, bool isSystem)
        {
            Name = name;
            Id = id;
            Path = path;
            IsSystem = isSystem;
        }

        /// <summary>
        /// The name of the current database.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The id of the current database.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// The filesystem path of the current database.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Whether or not the current database is the _system database.
        /// </summary>
        public bool IsSystem { get; }
    }
}
