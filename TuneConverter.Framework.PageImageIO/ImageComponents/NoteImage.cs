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
    private static readonly string localDirec = Directory.GetParent(
                                                    Directory.GetParent(
                                                        Directory.GetParent(
                                                            Directory.GetParent(Directory.GetCurrentDirectory()).FullName)
                                                        .FullName)
                                                    .FullName)
                                                .FullName;
    private static readonly string _imagePath2 = localDirec + "/TuneConverter.Framework.PageImageIO/Images/";


    public static readonly Image<Gray, byte> A = new(@"" + _imagePath + "A.png"); 
    public static readonly Image<Gray, byte> B = new(@"" + _imagePath + "B.png"); 
    public static readonly Image<Gray, byte> C = new(@"" + _imagePath + "C.png"); 
    public static readonly Image<Gray, byte> D = new(@"" + _imagePath + "D.png");  
    public static readonly Image<Gray, byte> E = new(@"" + _imagePath + "E.png");  
    public static readonly Image<Gray, byte> F = new(@"" + _imagePath + "F.png");  
    public static readonly Image<Gray, byte> G = new(@"" + _imagePath + "G.png");  
    public static readonly Image<Gray, byte> sharp = new(@"" + _imagePath + "sharp.png"); 
    public static readonly Image<Gray, byte> flat = new(@"" + _imagePath + "flat.png");
    public static readonly Image<Gray, byte> natural = new(@"" + _imagePath + "natural.png");
    public static readonly Image<Gray, byte> high = new(@"" + _imagePath + "high.png");  
    public static readonly Image<Gray, byte> low = new(@"" + _imagePath + "low.png");  
    public static readonly Image<Gray, byte> r = new(@"" + _imagePath + "ro.png");  
    public static readonly Image<Gray, byte> l = new(@"" + _imagePath + "ll.png"); 
    public static readonly Image<Gray, byte> _ = new(@"" + _imagePath + "long.png");
    public static readonly Image<Gray, byte> __ = new(@"" + _imagePath + "small_long.png");
    public static readonly Image<Gray, byte> duplet = new(@"" + _imagePath + "duplet.png");  
    public static readonly Image<Gray, byte> triplet = new(@"" + _imagePath + "triplet.png");  
    public static readonly Image<Gray, byte> part1 = new(@"" + _imagePath + "pt_1.png"); 
    public static readonly Image<Gray, byte> part2 = new(@"" + _imagePath + "pt_2.png"); 
    public static readonly Image<Gray, byte> part3 = new(@"" + _imagePath + "pt_3.png"); 
    public static readonly Image<Gray, byte> part4 = new(@"" + _imagePath + "pt_4.png"); 
    public static readonly Image<Gray, byte> part5 = new(@"" + _imagePath + "pt_5.png"); 
    public static readonly Image<Gray, byte> part6 = new(@"" + _imagePath + "pt_6.png"); 
    public static readonly Image<Gray, byte> part7 = new(@"" + _imagePath + "pt_7.png"); 
    public static readonly Image<Gray, byte> part8 = new(@"" + _imagePath + "pt_8.png"); 
    public static readonly Image<Gray, byte> part9 = new(@"" + _imagePath + "pt_9.png"); 
    public static readonly Image<Gray, byte> part10 = new(@"" + _imagePath + "pt_10.png");
    public static readonly Image<Gray, byte> rep_1 = new(@"" + _imagePath + "rep_1.png");
    public static readonly Image<Gray, byte> rep_2 = new(@"" + _imagePath + "rep_2.png");
    public static readonly Image<Gray, byte> rep_3 = new(@"" + _imagePath + "rep_3.png");
    public static readonly Image<Gray, byte> space = new(@"" + _imagePath + "space.png");
    public static readonly Image<Gray, byte> x = new(@"" + _imagePath + "x.png");
    public static readonly Image<Gray, byte> linkStart = new(@"" + _imagePath + "Link_start.png");
    public static readonly Image<Gray, byte> linkMiddle = new(@"" + _imagePath + "Link_mid.png");
    public static readonly Image<Gray, byte> linkEnd = new(@"" + _imagePath + "Link_end.png");
}
