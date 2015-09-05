using System.Collections.Generic;

namespace FileS
{
    public interface IParent : ICommonFS
    {
        List<IChild> Children { get; }
        IEnumerable<File> Files { get; }
        IEnumerable<Folder> Folders { get; }

        File NewFile(string Name, byte[] Data);

        File NewFile(System.IO.FileInfo File);

        Folder NewFolder(string Name);

        Folder NewFolder(System.IO.DirectoryInfo Directory);
    }
}