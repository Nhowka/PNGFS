using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileS
{
    public abstract class AbstractParent : IParent, ILazyEnabler
    {
        private string _name;

        internal AbstractParent(string Name)
        {
            _name = Name;
        }

        internal AbstractParent()
        {
        }

        public virtual List<IChild> Children { get; } = new List<IChild>();

        public byte[] Data
        {
            get
            {
                var childData = BitConverter.GetBytes(Children.Count).Concat(Children.SelectMany(x => x.Data)).ToArray();
                return Encoding.Default.GetBytes(Signature)
                    .Concat(new byte[] { (byte)Encoding.Default.GetByteCount(Name) })
                    .Concat(Encoding.Default.GetBytes(Name))
                    .Concat(BitConverter.GetBytes(childData.Length))
                    .Concat(childData).ToArray();
            }
        }

        public IEnumerable<File> Files => Children.OfType<File>();

        public IEnumerable<Folder> Folders => Children.OfType<Folder>();

        public byte[] LoadedData
        {
            get;
            protected set;
        }

        public string Name => _name;

        public abstract AbstractParent Root { get; }

        public abstract string Signature { get; }

        public File NewFile(FileInfo File)
        {
            var newFile = new File(this, File);
            Children.Add(newFile);
            return newFile;
        }

        public File NewFile(string Name, byte[] Data)
        {
            var newFile = new File(this, Name, Data);
            Children.Add(newFile);
            return newFile;
        }

        public Folder NewFolder(DirectoryInfo Directory)
        {
            var Folder = new Folder(this, Directory);
            Children.Add(Folder);
            return Folder;
        }

        public Folder NewFolder(string Name)
        {
            var Folder = new Folder(this, Name);
            Children.Add(Folder);
            return Folder;
        }

        public void Rename(string NewName)
        {
            _name = NewName;
        }
    }
}