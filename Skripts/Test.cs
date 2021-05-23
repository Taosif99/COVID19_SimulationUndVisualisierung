using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace KLASSEN_INF21
{
    class Test
    {
        static void Main(string[] args)
        {
            Policies One = new Policies(MaskType.None);
            Event[] Events = new Event[3];
            Events[0] = new Event(2, 40);
            Events[1] = new Event(3, 111);
            Events[2] = new Event(4, 999);

            Entity[] Pos = new Entity[1];
            Pos[0] = new Entity(new Vector2(2.5f, 1.8f));

            SimulationOptions Options = new SimulationOptions(One, Events);
            Simulation SerializeObject = new Simulation(Options, Pos);

            //Objekt wird serialisiert
            BinaryFormatter Formatter = new BinaryFormatter();
            FileStream Stream = new FileStream(@"C:\Users\taosi\Downloads\Test\test.txt", FileMode.OpenOrCreate);
            Formatter.Serialize(Stream, SerializeObject);
            Stream.Close();
            Stream = new FileStream(@"C:\Users\taosi\Downloads\Test\test.txt", FileMode.OpenOrCreate);

            //Object wird deserialisiert
            Simulation DeserializeObject = (Simulation)Formatter.Deserialize(Stream);

            Stream.Close();
        }
    }
}