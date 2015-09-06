using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace FileSTests
{
    [TestClass]
    public class CreationTest
    {
        [TestMethod]
        public void Creation()
        {
            var fs = new FileS.FSRoot();
            var folder = fs.NewFolder(new System.IO.DirectoryInfo(System.IO.Directory.GetCurrentDirectory()));
            foreach (var child in folder.Children)
            {
                Console.WriteLine(child.Name);
            }
            System.IO.File.WriteAllBytes(System.IO.Directory.GetCurrentDirectory() + @"\Test.pfs", fs.Data);
        }

        [TestMethod]
        public void Load()
        {
            var fs = new FileS.FSRoot(System.IO.File.ReadAllBytes(System.IO.Directory.GetCurrentDirectory() + @"\Test.pfs"));
            System.IO.File.WriteAllBytes(System.IO.Directory.GetCurrentDirectory() + @"\Test2.pfs", fs.Data);
            var original = System.IO.File.ReadAllBytes(System.IO.Directory.GetCurrentDirectory() + @"\Test.pfs");
            var saved = System.IO.File.ReadAllBytes(System.IO.Directory.GetCurrentDirectory() + @"\Test2.pfs");
            var differences = (from i in Enumerable.Range(0, original.Length)
                               where original[i] != saved[i]
                               select new { Index = i, Original = original[i], Saved = saved[i] }).ToArray();
            Assert.AreEqual(false, differences.Any(), $"Original length: {original.Length}. Saved length: {saved.Length}.");
        }
    }
}