using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileS
{
    public class Folder : AbstractParent, IChild
    {
        internal Folder(IParent Parent, string Name)
        {
            this.Parent = Parent; Rename(Name);
        }

        internal Folder(IParent Parent, DirectoryInfo Directory)
        {
            this.Parent = Parent; Rename(Directory.Name);
            foreach (var d in Directory.EnumerateDirectories())
            {
                NewFolder(d);
            }
            foreach (var f in Directory.EnumerateFiles())
            {
                NewFile(f);
            }
        }

        public IParent Parent { get; }
        public override IParent Root => Parent.Root;
        public override string Signature => "FLDR";
    }
}