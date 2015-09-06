namespace FileS
{
    public interface ILazyLoad
    {
        bool IsLoaded { get; }
        int? Length { get; }
        int? ParentOffset { get; }
    }
}