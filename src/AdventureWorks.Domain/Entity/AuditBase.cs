using System;

namespace AdventureWorks.Domain.Entity
{
    /// <summary>
    /// POCO class to hold audit fields
    /// </summary>
    public class AuditBase
    {
        #region Properties

        public Guid RowGuid { get; set; }

        public DateTime ModifiedDate { get; set; }

        public int? ResultCount { get; set; }

        #endregion

        #region Serialization settings

        public bool ShouldSerializeResultCount()
        {
            return ResultCount.HasValue;
        }

        #endregion
    }
}
