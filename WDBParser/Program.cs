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

            int recordIndex = 0;

            while (wdbStream.BaseStream.Position != wdbStream.BaseStream.Length)
            {
                var entryId = wdbStream.ReadInt32();
                var size = wdbStream.ReadInt32();

                if (entryId == 0 && size == 0)
                    break;

                ++recordIndex;

                Console.WriteLine($"Game object entry Id: {entryId} Size: {size} bytes at Index: {recordIndex}");

                var entryBytes = wdbStream.ReadBytes(size);
                var entryStream = new BinaryReader(new MemoryStream(entryBytes));

                int type = entryStream.ReadInt32();
                int displayId = entryStream.ReadInt32();

                const int sizeOfNamelessEntry = 139;
                var sizeOfName = size - sizeOfNamelessEntry;
                string name = Encoding.UTF8.GetString(entryStream.ReadBytes(sizeOfName));                            

                Console.WriteLine("Type: " + type);
                Console.WriteLine("DisplayId: " + displayId);
                Console.WriteLine("Name: " + name);
            }

            Console.WriteLine("Total records in cache: " + recordIndex);
        }
    }
}
