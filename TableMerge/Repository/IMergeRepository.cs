namespace TableMerge.Repository
{
    public interface IMergeRepository
    {
        Task<string> MergeStudentData(int Id, string Name);
    }
}
