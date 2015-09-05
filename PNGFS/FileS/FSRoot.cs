using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileS
{
    public class FSRoot : AbstractParent
    {
        public FSRoot(string Name = "Root")
        {
            Rename(Name);
        }

        public override IParent Root => this;
        public override string Signature => "ROOT";
    }
}