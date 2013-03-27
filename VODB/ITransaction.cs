using System;

namespace VODB
{
    public interface ITransaction : IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether this instance has rolled back.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has rolled back; otherwise, <c>false</c>.
        /// </value>
        bool RolledBack { get; }

        /// <summary>
        /// Commits the changes made within this scope to the database.
        /// </summary>
        void Commit();

        /// <summary>
        /// Rollbacks all changes made within this scope.
        /// </summary>
        void Rollback();
    }
}