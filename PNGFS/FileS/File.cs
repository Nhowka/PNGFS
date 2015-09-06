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
        private bool _loaded;
        private string _name;
        private byte[] internalData;

        internal File(AbstractParent Parent, FileInfo File)
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

        internal File(AbstractParent Parent, string Name, int Length, int ParentOffset)
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
            this.Length = Length;
            this.ParentOffset = ParentOffset;
            _loaded = false;
        }

        internal File(AbstractParent Parent, string Name, byte[] Data)
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

        public byte[] Data
        {
            get
            {
                if (!_loaded)
                {
                    internalData = new byte[Length ?? 0];
                    Array.ConstrainedCopy(Parent.LoadedData, ParentOffset ?? 0, internalData, 0, Length ?? 0);
                    _loaded = true;
                }
                return Encoding.Default.GetBytes(Signature)
                    .Concat(new byte[] { (byte)Encoding.Default.GetByteCount(FullName) })
                    .Concat(Encoding.Default.GetBytes(FullName))
                    .Concat(BitConverter.GetBytes(internalData.Length))
                    .Concat(internalData).ToArray();
            }
        }

        public string Extension { get; }
        public string FullName => Name + Extension;
        public bool IsLoaded => _loaded;

        public int? Length { get; }

        public string Name => _name;

        public AbstractParent Parent { get; }

        public int? ParentOffset { get; }

        public AbstractParent Root => Parent.Root;

        public string Signature => "FILE";

        public void Delete()
        {
            Parent.Children.Remove(this);
        }

        public void Rename(string NewName)
        {
            _name = NewName;
        }
    }
}