namespace VODB.VirtualDataBase
{
    /// <summary>
    /// Creates Tables with fields.
    /// </summary>
    internal interface ITableCreator
    {

        /// <summary>
        /// Creates a table.
        /// </summary>
        /// <returns></returns>
        Table Create();

    }
}
