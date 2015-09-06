using System.Collections.Generic;

namespace FileS
{
    public interface ICommonFS
    {
        byte[] Data { get; }
        string Name { get; }
        AbstractParent Root { get; }
        string Signature { get; }

        void Rename(string NewName);
    }
}