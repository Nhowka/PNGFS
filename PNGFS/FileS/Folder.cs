using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileS
{
    public class Folder : AbstractParent, IChild, ILazyLoad
    {
        private bool _loaded;

        internal Folder(AbstractParent Parent, string Name) : base(Name)
        {
            this.Parent = Parent; _loaded = true;
        }

        internal Folder(AbstractParent Parent, string Name, int Length, int ParentOffset) : base(Name)
        {
            this.Parent = Parent;
            this.Length = Length;
            this.ParentOffset = ParentOffset;
            _loaded = false;
        }

        internal Folder(AbstractParent Parent, DirectoryInfo Directory)
        {
            this.Parent = Parent; Rename(Directory.Name);
            _loaded = true;
            foreach (var d in Directory.EnumerateDirectories())
            {
                NewFolder(d);
            }
            foreach (var f in Directory.EnumerateFiles())
            {
                NewFile(f);
            }
        }

        public override List<IChild> Children
        {
            get
            {
                if (LoadedData != null == _loaded)
                {
                    if (!_loaded)
                    {
                        LoadedData = new byte[Length ?? 0];
                        Array.ConstrainedCopy(Parent.LoadedData, ParentOffset ?? 0, LoadedData, 0, Length ?? 0);
                        var readingOffset = 0;
                        int childrenCount = BitConverter.ToInt32(LoadedData, readingOffset);
                        readingOffset += 4;
                        int nameLength;
                        int childLength;
                        string Name;
                        for (int i = 0; i < childrenCount; ++i)
                        {
                            var signature = new string(Encoding.Default.GetChars(LoadedData, readingOffset, 4));
                            readingOffset += 4;
                            nameLength = LoadedData[readingOffset++];
                            Name = new string(Encoding.Default.GetChars(LoadedData, readingOffset, nameLength));
                            readingOffset += nameLength;
                            childLength = BitConverter.ToInt32(LoadedData, readingOffset);
                            readingOffset += 4;
                            if (signature == "FLDR")
                            {
                                base.Children.Add(new Folder(this, Name, childLength, readingOffset));
                            }
                            if (signature == "FILE")
                            {
                                base.Children.Add(new File(this, Name, childLength, readingOffset));
                            }
                            readingOffset += childLength;
                        }
                        _loaded = true;
                    }
                    if (base.Children.Any() && base.Children.All(x => x.IsLoaded))
                        LoadedData = null;
                }
                return base.Children;
            }
        }

        public bool IsLoaded => _loaded;
        public int? Length { get; }

        public AbstractParent Parent { get; }
        public int? ParentOffset { get; }
        public override AbstractParent Root => Parent.Root;
        public override string Signature => "FLDR";

        public void Delete()
        {
            Parent.Children.Remove(this);
        }
    }
}