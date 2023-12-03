using Emgu.CV;
using Emgu.CV.CvEnum;
using TuneConverter.Framework.PageImageIO.ImageComponents;
using TuneConverter.Framework.TuneComponents.TuneComponents;
using System.Drawing;
using Emgu.CV.Structure;
using TuneConverter.Framework.TuneComponents.Types;

namespace TuneConverter.Framework.PageImageIO.ImageBuilder;

public class PageAssembler
{
    private const int Height = 80;
    private const int Width = 55 + 5;

    private const double A4Width = 210;
    private const double A4Height = 297;

    private const FontFace font = FontFace.HersheyTriplex;
    private MCvScalar scale = new(255);

    private readonly Gray White = new(255);

    private readonly LineType lineType = LineType.AntiAlias;

    public Image<Gray, byte> CreateTune(TuneFull tune)
    {
        List<Image<Gray, byte>> pieces = [];
        var pageWidth = (Width + 16) * (int)tune.TuneType;
        var partSplit = new Image<Gray, byte>(pageWidth, 50, White);

        pieces.Add(CreateTitlePage(tune.Title, tune.TuneType, tune.Key.NoteType.ToString() + " " + tune.Key.Keytype.ToString()));

        int pageHeight = pieces[0].Height;
        foreach (var part in tune.tune.Select((value, i) => (value, i)))
        {
            var newPart = CreatePart(part.value, tune.TuneType, part.i + 1);
            pieces.Add(newPart);

            if (part.value.Link.line.Count > 0)
            {
                pieces.Add(CreateTuneLink(part.value.Link, tune.TuneType));
                pieces.Add(partSplit);
                pageHeight += 120;
            }
            pageHeight += newPart.Height + partSplit.Height;
        }

        var image = new Image<Gray, byte>(pageWidth, pageHeight, White);

        return CreatePage(pieces, image);
    }

    #region Title Generation
    public Image<Gray, byte> CreateTitlePage(string title, TuneType tuneType, string key)
    {
        int pageHeight = (int)(Height * 2) + 120;
        int pageWidth = (Width + 16) * (int)tuneType;

        var image = new Image<Gray, byte>(pageWidth, pageHeight);
        
        var titlePage = CreateTitle(image, title, pageWidth, pageHeight);

        titlePage = CreateTitleSideText(titlePage, tuneType.ToString(), 150);
        titlePage = CreateTitleSideText(titlePage, key, 175);

        CvInvoke.BitwiseNot(titlePage, titlePage);

        return titlePage;
    }

    private Image<Gray, byte> CreateTitle(Image<Gray, byte> titlePage, string title, int pageWidth, int pageHeight)
    {
        int baseline = 1;
        double titleFontScale = 3;
        int titleFontThickness = 3;

        Size titleSize = CvInvoke.GetTextSize(title, font, titleFontScale, titleFontThickness, ref baseline);

        double textWidth = ((pageWidth / 2) - (titleSize.Width / 2));
        double textHeight = ((pageHeight / 3) - (titleSize.Height * 0.1));

        int titleStartX = (int)Math.Round(textWidth, 0, MidpointRounding.ToZero);
        int titleStartY = (int)Math.Round(textHeight, 0, MidpointRounding.ToZero);

        Point point = new(titleStartX, titleStartY);

        CvInvoke.PutText(titlePage, title, point, font, titleFontScale, scale, titleFontThickness, lineType);

        titlePage = CreateTitleLine(titlePage, titleStartX, titleStartY, titleSize.Width, scale);
        
        return titlePage;
    }

    private static Image<Gray, byte> CreateTitleLine(Image<Gray, byte> titlePage, int titleStartX, int titleStartY, int width, MCvScalar scale)
    {
        int lineThickness = 3;

        var startX = titleStartX - 80;
        var endX = titleStartX + width + 80;
        var lineHeight = titleStartY + 30;

        Point startPoint = new(startX, lineHeight);
        Point endPoint = new(endX, lineHeight);

        CvInvoke.Line(titlePage, startPoint, endPoint, scale, lineThickness);

        return titlePage;
    }

    private Image<Gray, byte> CreateTitleSideText(Image<Gray, byte> titlePage, string text, int height)
    {
        double titleFontScale = 0.65;
        int titleFontThickness = 1;

        Point tuneTypePoint = new((16), height + 30);

        CvInvoke.PutText(titlePage, text, tuneTypePoint, font, titleFontScale, scale, titleFontThickness, lineType);

        return titlePage;
    }

    #endregion

    #region Part Generation

    public Image<Gray, byte> CreatePage(List<Image<Gray, byte>> pieces, Image<Gray, byte> image)
    {
        int pageMarker = 0;

        foreach (var piece in pieces)
        {
            if (piece.Height == 120)
            {
                var start = (image.Width) - piece.Width - 122;
                image = SetRoi(image, piece, start, pageMarker - 70);
                pageMarker += piece.Height;
                continue;
            }
            image = SetRoi(image, piece, 0, pageMarker);
            pageMarker += piece.Height;
        }

        double widthRatio = A4Width / A4Height;
        int a4Width = (int)(image.Height * widthRatio);
        var imageA4 = new Image<Gray, byte>(a4Width, image.Height, White);

        var middleMark = ((imageA4.Width - image.Width) / 2) + 10;

        imageA4 = SetRoi(imageA4, image, middleMark, 0);

        return imageA4;
    }

    public Image<Gray, byte> CreatePart(TunePart part, TuneType tuneType, int partNumber)
    {
        int pageHeight = (int)((Height + 60) * (part.part.Count + 1));
        int pageWidth = (Width + 16) * (int)tuneType;

        var image = new Image<Gray, byte>(pageWidth, pageHeight, White);

        var lineSplit = new Image<Gray, byte>(pageWidth, 40, White);

        foreach (var line in part.part.Select((value, i) => (value, i)))
        {
            var imageLine = CreateLine(line.value, line.i, pageWidth);
            image = SetRoi(image, imageLine, 0, (line.i * 160) + 10);
            image = SetRoi(image, lineSplit, 0, (120 + (line.i * 160)) + 10);
        }

        var foo2 = new NoteImage();

        Image<Gray, byte> noteImage = foo2.GetType().GetField("part" + partNumber.ToString()).GetValue(foo2) as Image<Gray, byte>;

        image = SetRoi(image, noteImage, 10, 20);

        return image;
    }

    public Image<Gray, byte> CreateLine(TuneLine line, int lineCount, int pageWidth, bool ifLink = false)
    {
        var image = new Image<Gray, byte>(pageWidth, Height + 40, White);

        var foo2 = new NoteImage();

        Image<Gray, byte> space = foo2.space;

        foreach (var bar in line.line.Select((value, i) => (value, i)))
        {
            var imageNote = CreateBar(bar.value);
            var barwidth = ((Width + 16) * bar.value.bar.Count) + 55;
            image = SetRoi(image, imageNote, (bar.i * barwidth) + (ifLink? 40:80));

            if (ifLink)
            {
                continue;
            }
            image = SetRoi(image, space, (bar.i + 1) * barwidth + 25);
        }

        return image;
    }

    public Image<Gray, byte> CreateBar(TuneBar bar)
    { 
        var notewidth = Width + 16;

        var image = new Image<Gray, byte>(notewidth * bar.bar.Count, Height + 40, White);

        foreach (var note in bar.bar.Select((value, i) => (value, i)))
        {
            var imageNote = CreateNote(note.value);

            image = SetRoi(image, imageNote, note.i * notewidth );

        }

        return image;
    }

    public Image<Gray, byte> CreateNote(NoteGroup bar)
    {
        var foo2 = new NoteImage();

        string note = bar.Note.NoteType.ToString();

        Image<Gray, byte> noteImage = foo2.GetType().GetField(note).GetValue(foo2) as Image<Gray, byte>;

        var image = new Image<Gray, byte>(Width + 16, Height + 40, White);

        noteImage = SetRoi(image, noteImage, 4, 18);

        if (bar.Note.OctaveType == OctaveType.Low)
        {
            noteImage = AddLow(noteImage);
        }
        else if (bar.Note.OctaveType == OctaveType.High)
        {
            if (bar.Note.AccidentalType == AccidentalType.Sharp)
            {
                noteImage = AddHighAndSharp(noteImage);
            }
            else
            {
                noteImage = AddHigh(noteImage);
            }
        }
        if (bar.Note.AccidentalType == AccidentalType.Sharp && bar.Note.OctaveType != OctaveType.High)
        {
            noteImage = AddSharp(noteImage);
        }

        return noteImage;
    }

    public Image<Gray, byte> CreateTuneLink(TuneLine line, TuneType tuneType)
    {
        var foo2 = new NoteImage();

        var numBars = line.MaxLength;
        var barSize = line.line[0].MaxLength;

        var middleNums = numBars * (barSize + 1);

        var linkStart = foo2.linkStart;
        var linkMiddle = foo2.linkMiddle;
        var linkEnd = foo2.linkEnd;

        var image = new Image<Gray, byte>( (linkStart.Width * middleNums), linkStart.Height);

        image = SetRoi(image, linkStart, 0, 0);

        foreach (int y in Enumerable.Range(1, middleNums - 2).Select(x => x * linkStart.Width))
        {
            image = SetRoi(image, linkMiddle, y, 0);
        }
        image = SetRoi(image, linkEnd, ((middleNums - 1) * linkStart.Width), 0);

        var line2 = CreateLine(line, 0, image.Width, true);
        line2 = ShiftOver(line2, 0, 20);

        CvInvoke.BitwiseNot(image, image);
        CvInvoke.BitwiseNot(line2, line2);
        CvInvoke.BitwiseOr(image, line2, image);
        CvInvoke.BitwiseNot(image, image);

        return image;
    }

    private static Image<Gray, byte> SetRoi(Image<Gray, byte> bigImage, Image<Gray, byte> smallImage, int shiftWidth = 0, int shiftHeight = 0)
    {
        int height = smallImage.Height;
        int width = smallImage.Width;

        foreach (int y in Enumerable.Range(0, height))
        {
            foreach (int x in Enumerable.Range(0, width))
            {
                bigImage[y + shiftHeight, x + shiftWidth] = smallImage[y, x];
            }
        }

        return bigImage;
    }

    private Image<Gray, byte> ShiftOver(Image<Gray, byte> image, int shiftWidth = 0, int shiftHeight = 0)
    {
        int height = image.Height;
        int width = image.Width;

        Image<Gray, byte> smallImage = new Image<Gray, byte>(width, height, White);

        foreach (int y in Enumerable.Range(0, height - shiftHeight))
        {
            foreach (int x in Enumerable.Range(0, width - shiftWidth))
            {
                smallImage[y + shiftHeight, x + shiftWidth] = image[y, x];
            }
        }

        return smallImage;
    }

    private static Image<Gray, byte> AddAboveNote(Image<Gray, byte> bigImage, Image<Gray, byte> symbolImage, int shiftSide = 0, int shiftDown = 0)
    {
        int width = bigImage.Width;

        int sharpHeight = symbolImage.Height;
        int sharpWidth = symbolImage.Width;

        var shift = 2;

        foreach (int y in Enumerable.Range(0, sharpHeight))
        {
            foreach (int x in Enumerable.Range(0, sharpWidth))
            {
                bigImage[y + shiftDown, width - (x + shift) - shiftSide] = symbolImage[y, sharpWidth - x - 1];
            }
        }

        return bigImage;
    }

    private static Image<Gray, byte> AddSharp(Image<Gray, byte> bigImage)
    {
        var noteImage = new NoteImage();

        Image<Gray, byte> sharpSymbol = noteImage.sharp;

        bigImage = AddAboveNote(bigImage, sharpSymbol);

        return bigImage;
    }

    private Image<Gray, byte> AddHigh(Image<Gray, byte> bigImage, int shift = 0)
    {
        var noteImage = new NoteImage();

        Image<Gray, byte> highSymbol = noteImage.high;

        bigImage = AddAboveNote(bigImage, highSymbol);

        return bigImage;
    }

    private Image<Gray, byte> AddHighAndSharp(Image<Gray, byte> bigImage)
    {
        var noteImage = new NoteImage();

        Image<Gray, byte> highSymbol = noteImage.high;
        bigImage = AddAboveNote(bigImage, highSymbol);

        Image<Gray, byte> sharpSymbol = noteImage.sharp;
        bigImage = AddAboveNote(bigImage, sharpSymbol, highSymbol.Width, 1);

        return bigImage;
    }

    private Image<Gray, byte> AddLow(Image<Gray, byte> bigImage)
    {
        var noteImage = new NoteImage();

        Image<Gray, byte> lowSymbol = noteImage.low;

        int lowHeight = lowSymbol.Height;
        int lowWidth = lowSymbol.Width;

        var shift = 6;

        foreach (int y in Enumerable.Range(0, lowHeight))
        {
            foreach (int x in Enumerable.Range(0, lowWidth))
            {
                bigImage[y + Height + 18, (x + shift)] = lowSymbol[y, x];
            }
        }

        return bigImage;
    }

    #endregion
}
