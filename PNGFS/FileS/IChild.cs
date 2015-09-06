namespace FileS
{
    public interface IChild : ICommonFS, ILazyLoad
    {
        AbstractParent Parent { get; }

        void Delete();
    }
}