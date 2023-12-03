using TuneConverter.Framework.TuneIO.TuneReader;

public class Program
{
    public static void Main(string[] args)
    {
       
        TuneReader reader = new TuneReader();
        var file = reader.readFile("Kesh Jig.txt");
    }
}