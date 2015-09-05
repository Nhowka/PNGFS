using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileS
{
    public class File : IChild
    {
        private readonly byte[] internalData;
        private string _name;

        internal File(IParent Parent, FileInfo File)
        {
            this.Parent = Parent;
            var lastIndex = File.Name.LastIndexOf('.');
            if (lastIndex != -1)
                _name = File.Name.Substring(0, lastIndex);
            else
                _name = File.Name;
            Extension = File.Extension;
            using (var file = File.OpenRead())
            {
                internalData = new byte[file.Length];
                file.Read(internalData, 0, (int)file.Length);
            }
        }

        internal File(IParent Parent, string Name, byte[] Data)
        {
            this.Parent = Parent;
            var lastIndex = Name.LastIndexOf('.');
            if (lastIndex != -1)
            {
                Extension = Name.Substring(lastIndex);
                _name = Name.Substring(0, lastIndex);
            }
            else
            {
                _name = Name;
                Extension = "";
            }
            internalData = Data;
        }

        public byte[] Data => Encoding.Default.GetBytes(Signature)
                    .Concat(new byte[] { (byte)Encoding.Default.GetByteCount(FullName) })
                    .Concat(Encoding.Default.GetBytes(FullName))
                    .Concat(BitConverter.GetBytes(internalData.Length))
                    .Concat(internalData).ToArray();

        public string Extension { get; }
        public string FullName => Name + Extension;
        public string Name => _name;

        public IParent Parent { get; }

        public IParent Root => Parent.Root;

        public string Signature => "FILE";

        public void Rename(string NewName)
        {
            _name = NewName;
        }
    }
}