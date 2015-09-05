namespace FileS
{
    public interface IChild : ICommonFS
    {
        IParent Parent { get; }
    }
}