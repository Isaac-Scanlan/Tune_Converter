namespace TuneConverter.Framework.ABCConverter;

/// <summary>
/// 
/// </summary>
public class Program
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        Converter converter = new Converter();
        List<string> tune = new List<string>()
        //{
        //    "X: 1",
        //    "T: The Kesh 2",
        //    "R: jig",
        //    "M: 6/8",
        //    "L: 1/8",
        //    "K: Gmaj",
        //    "|:G3 GAB|A3 ABd|edd gdd|edB dBA|",
        //    "GAG GAB|ABA ABd|edd gdd|BAF G3:|",
        //    "|:B2B d2d|ege dBA|B2B dBG|ABA AGA|",
        //    "BAB d^cd|ege dBd|gfg aga|bgg g3:|"
        //};
        {
            "X: 1",
            "T: Cooley's",
            "R: reel",
            "M: 4/4",
            "L: 1/8",
            "K: Cmin",
            "|:F|CGGC G2 CG|G2 FG BGFE|(3DCB,FB, GB,FB,|DB,DF BFDB,|",
            "CGGC G2 CG|G2 FG Bcde|fdcd BGFB|B,CDF C4:|",
            "|:d|c~G2 cede|c~G2 ecBG|(3FGFDF B,FDF|GFDF Bcde|",
            "c~G2 cede|c~G2 Bcde|fdcd BGFB|B,CDF C4:|"
        };
        
        //{
        //    "X: 1",
        //    "T: Kerry Polka",
        //    "R: polka",
        //    "M: 2/4",
        //    "L: 1/8",
        //    "K: Dmaj",
        //    "|:fA BA|fA BA|d2 e>f|ed BA|fA BA|fA BA|d2 e>f|ed d2:|",
        //    "|:fa f>e|ed BA|d2 e>f|ed BA|fa f>e|ed BA|d2 e>f|ed d2:|"
        //};

        var val = converter.ConvertToInternalABC(tune);

        foreach (var v in val)
        {
            Console.WriteLine(v);
        }
    }

}