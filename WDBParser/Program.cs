using System;
using System.IO;
using System.Linq;
using System.Text;

namespace WDBParser
{
    class Program
    {
        const string fileName = "gameobjectcache.wdb";

        static void Main()
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException();

            using var wdbStream = new BinaryReader(File.OpenRead(fileName));
            var cacheIdentifier = Encoding.UTF8.GetString(wdbStream.ReadBytes(4)
                                          .Reverse().ToArray());
            var clientBuild = wdbStream.ReadUInt32();
            var clientlocale = Encoding.UTF8.GetString(wdbStream.ReadBytes(4)
                                            .Reverse().ToArray());
            var recordSize = wdbStream.ReadUInt32();
            var recordVersion = wdbStream.ReadUInt32();
            var cacheVersion = wdbStream.ReadUInt32();

            Console.WriteLine("Parsing WDB file: " + fileName);
            Console.WriteLine("Cache Identifier: " + cacheIdentifier.ToString());
            Console.WriteLine("Cache Version: " + cacheVersion);
            Console.WriteLine("Client Locale: " + clientlocale);
            Console.WriteLine("Client Build: " + clientBuild);
            Console.WriteLine("Record Size: " + recordSize);
            Console.WriteLine("Record Version: " + recordVersion);
        }
    }
}
