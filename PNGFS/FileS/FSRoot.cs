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
        public FSRoot(string Name = "Root") : base(Name)
        {
        }

        public FSRoot(byte[] Data)
        {
            var readingOffset = 0;
            var signature = new string(Encoding.Default.GetChars(Data, readingOffset, 4));
            if (Signature != signature)
            {
                throw new Exception("Invalid input");
            }
            readingOffset += 4;
            var nameLength = (int)Data[readingOffset++];
            Rename(new string(Encoding.Default.GetChars(Data, readingOffset, nameLength)));
            readingOffset += nameLength;
            var Length = BitConverter.ToInt32(Data, readingOffset);
            readingOffset += 4;
            LoadedData = new byte[Length];
            Array.ConstrainedCopy(Data, readingOffset, LoadedData, 0, Length);
            readingOffset = 0;
            int childrenCount = BitConverter.ToInt32(LoadedData, readingOffset);
            readingOffset += 4;
            string Name;
            for (int i = 0; i < childrenCount; ++i)
            {
                signature = new string(Encoding.Default.GetChars(LoadedData, readingOffset, 4));
                readingOffset += 4;
                nameLength = LoadedData[readingOffset++];
                Name = new string(Encoding.Default.GetChars(LoadedData, readingOffset, nameLength));
                readingOffset += nameLength;
                Length = BitConverter.ToInt32(LoadedData, readingOffset);
                readingOffset += 4;
                if (signature == "FLDR")
                {
                    base.Children.Add(new Folder(this, Name, Length, readingOffset));
                }
                else if (signature == "FILE")
                {
                    base.Children.Add(new File(this, Name, Length, readingOffset));
                }
                readingOffset += Length;
            }
        }

        public override List<IChild> Children
        {
            get
            {
                if (LoadedData != null && base.Children.Any() && base.Children.All(x => x.IsLoaded))
                    LoadedData = null;
                return base.Children;
            }
        }

        public override AbstractParent Root => this;
        public override string Signature => "ROOT";
    }
}