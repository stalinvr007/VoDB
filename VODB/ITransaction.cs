using System;
namespace VODB
{
    public interface ITransaction : IDisposable
    {
        void RollBack();
        void Commit();
        void SavePoint();
    }
}