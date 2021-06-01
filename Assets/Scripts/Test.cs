using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Simulation.Edit;
using Event = Simulation.Edit.Event;

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
        Pos[0] = new Entity(new GridCell(2, 1));

        SimulationOptions Options = new SimulationOptions(One, Events);
        Simulation.Edit.Simulation SerializeObject = new Simulation.Edit.Simulation(Options, Pos);

        //Objekt wird serialisiert
        BinaryFormatter Formatter = new BinaryFormatter();
        FileStream Stream = new FileStream(@"C:\Users\taosi\Downloads\Test\test.txt", FileMode.OpenOrCreate);
        Formatter.Serialize(Stream, SerializeObject);
        Stream.Close();
        Stream = new FileStream(@"C:\Users\taosi\Downloads\Test\test.txt", FileMode.OpenOrCreate);

        //Object wird deserialisiert
        Simulation.Edit.Simulation DeserializeObject = (Simulation.Edit.Simulation)Formatter.Deserialize(Stream);

        Stream.Close();
    }
}