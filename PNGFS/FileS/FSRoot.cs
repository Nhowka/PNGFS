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
            var signature = new string(Encoding.Default.GetChars(LoadedData, readingOffset, 4));
            if (Signature != signature)
            {
                throw new Exception("Invalid input");
            }
            readingOffset += 4;
            var nameLength = BitConverter.ToInt32(Data, readingOffset);
            readingOffset += 4;
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
                nameLength = BitConverter.ToInt32(LoadedData, readingOffset);
                readingOffset += 4;
                Name = new string(Encoding.Default.GetChars(LoadedData, readingOffset, nameLength));
                readingOffset += nameLength;
                Length = BitConverter.ToInt32(LoadedData, readingOffset);
                readingOffset += 4;
                if (signature == "FLDR")
                {
                    base.Children.Add(new Folder(this, Name, Length, readingOffset));
                }
                if (signature == "FILE")
                {
                    base.Children.Add(new File(this, Name, Length, readingOffset));
                }
            }
        }

        public override AbstractParent Root => this;
        public override string Signature => "ROOT";
    }
}