using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;

namespace TuneConverter.Framework.PageImageIO.ImageComponents;

public class NoteImage
{
    private static readonly string _imagePath = "C:/Users/Isaac/source/repos/TuneConverter/TuneConverter.Framework.PageImageIO/Images/";

    public readonly Image<Gray, byte> A = new(@"" + _imagePath + "A.png"); 
    public readonly Image<Gray, byte> B = new(@"" + _imagePath + "B.png"); 
    public readonly Image<Gray, byte> C = new(@"" + _imagePath + "C.png"); 
    public readonly Image<Gray, byte> D = new(@"" + _imagePath + "D.png");  
    public readonly Image<Gray, byte> E = new(@"" + _imagePath + "E.png");  
    public readonly Image<Gray, byte> F = new(@"" + _imagePath + "F.png");  
    public readonly Image<Gray, byte> G = new(@"" + _imagePath + "G.png");  
    public readonly Image<Gray, byte> sharp = new(@"" + _imagePath + "sharp.png"); 
    public readonly Image<Gray, byte> flat = new(@"" + _imagePath + "flat.png"); 
    public readonly Image<Gray, byte> high = new(@"" + _imagePath + "high.png");  
    public readonly Image<Gray, byte> low = new(@"" + _imagePath + "low.png");  
    public readonly Image<Gray, byte> ro = new(@"" + _imagePath + "ro.png");  
    public readonly Image<Gray, byte> ll = new(@"" + _imagePath + "ll.png"); 
    public readonly Image<Gray, byte> _ = new(@"" + _imagePath + "long.png");  
    public readonly Image<Gray, byte> duplet = new(@"" + _imagePath + "duplet.png");  
    public readonly Image<Gray, byte> triplet = new(@"" + _imagePath + "triplet.png");  
    public readonly Image<Gray, byte> part1 = new(@"" + _imagePath + "pt_1.png"); 
    public readonly Image<Gray, byte> part2 = new(@"" + _imagePath + "pt_2.png"); 
    public readonly Image<Gray, byte> part3 = new(@"" + _imagePath + "pt_3.png"); 
    public readonly Image<Gray, byte> part4 = new(@"" + _imagePath + "pt_4.png"); 
    public readonly Image<Gray, byte> part5 = new(@"" + _imagePath + "pt_5.png"); 
    public readonly Image<Gray, byte> part6 = new(@"" + _imagePath + "pt_6.png"); 
    public readonly Image<Gray, byte> part7 = new(@"" + _imagePath + "pt_7.png"); 
    public readonly Image<Gray, byte> part8 = new(@"" + _imagePath + "pt_8.png"); 
    public readonly Image<Gray, byte> part9 = new(@"" + _imagePath + "pt_9.png"); 
    public readonly Image<Gray, byte> part10 = new(@"" + _imagePath + "pt_10.png");
    public readonly Image<Gray, byte> space = new(@"" + _imagePath + "space.png");
    public readonly Image<Gray, byte> linkStart = new(@"" + _imagePath + "Link_start.png");
    public readonly Image<Gray, byte> linkMiddle = new(@"" + _imagePath + "Link_mid.png");
    public readonly Image<Gray, byte> linkEnd = new(@"" + _imagePath + "Link_end.png");
}
