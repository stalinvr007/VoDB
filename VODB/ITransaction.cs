using System;
namespace VODB
{
    public interface ITransaction : IDisposable
    {
        Boolean RolledBack { get; }
        void RollBack();
        void Commit();

    }
}