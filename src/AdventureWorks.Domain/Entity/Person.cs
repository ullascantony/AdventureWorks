namespace AdventureWorks.Domain.Entity
{
    /// <summary>
    /// POCO class to map data for Person
    /// </summary>
    public class Person : AuditBase
    {
        #region Properties

        public int BusinessEntityID { get; set; }

        public string PersonType { get; set; }

        public string Title { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Suffix { get; set; }

        #endregion

        #region Serialization settings

        public bool ShouldSerializeTitle()
        {
            return !string.IsNullOrWhiteSpace(Title);
        }

        public bool ShouldSerializeMiddleName()
        {
            return !string.IsNullOrWhiteSpace(MiddleName);
        }

        public bool ShouldSerializeSuffix()
        {
            return !string.IsNullOrWhiteSpace(Suffix);
        }

        #endregion
    }
}
