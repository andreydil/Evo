using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Evo.Core.Universe;

namespace Evo.Core.Saving
{
    public static class WorldSaver
    {
        public static void Save(World world, string path)
        {
            var binaryFormatter = new BinaryFormatter();
            using (var stream = File.OpenWrite(path))
            {
                binaryFormatter.Serialize(stream, world);
            }
        }

        public static World Load(string path)
        {
            var binaryFormatter = new BinaryFormatter();
            using (var stream = File.OpenRead(path))
            {
                return (World)binaryFormatter.Deserialize(stream);
            }
        }
    }
}
