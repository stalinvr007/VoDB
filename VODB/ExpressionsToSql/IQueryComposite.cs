namespace VODB.ExpressionsToSql
{
    public interface IQueryConditionComposite : IQueryCondition
    {

        void Add(IQueryCondition query);
        void InsertBeforeLast(IQueryCondition query);
    }
}
