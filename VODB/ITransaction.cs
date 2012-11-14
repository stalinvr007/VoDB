namespace VODB
{
    public interface ITransaction
    {
        void RollBack();
        void Commit();
    }
}