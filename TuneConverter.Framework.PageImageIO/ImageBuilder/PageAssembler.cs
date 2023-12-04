using Emgu.CV;
using Emgu.CV.CvEnum;
using TuneConverter.Framework.PageImageIO.ImageComponents;
using TuneConverter.Framework.TuneComponents.TuneComponents;
using System.Drawing;
using Emgu.CV.Structure;
using TuneConverter.Framework.TuneComponents.Types;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;

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

    public List<Image<Gray, byte>> CreateTune(TuneFull tune)
    {
        List<Image<Gray, byte>> pieces = [];
        var pageWidth = (Width + 16) * (int)tune.TuneType;
        var partSplit = new Image<Gray, byte>(pageWidth, 50, White);

        pieces.Add(CreateTitlePage(tune.Title, tune.TuneType, tune.Key.NoteType.ToString() + " " + tune.Key.Keytype.ToString()));

        int pageHeight = pieces[0].Height;
        int pageHeight2 = pieces[0].Height;
        bool pageMarker = true;

        foreach (var part in tune.tune.Select((value, i) => (value, i)))
        {
            var newPart = CreatePart(part.value, tune.TuneType, part.i + 1);
            pieces.Add(newPart);

            if ((part.i + 1) % 2 != 0 && (part.i + 1) != 1)
            {
                pageHeight = pageHeight2 > pageHeight? pageHeight2: pageHeight;
                pageMarker = false;
                pageHeight2 = pieces[0].Height;
            }

            if (part.value.Link.line.Count > 0)
            {
                pieces.Add(CreateTuneLink(part.value.Link, tune.TuneType));
                pieces.Add(partSplit);
                if(pageMarker)
                {
                    pageHeight += 141;
                }
                else
                {
                    pageHeight2 += 141;
                }
            }
            if (tune.TuneType == TuneType.Reel)
            {
                pieces.Add(partSplit);
                if (pageMarker)
                {
                    pageHeight += 120;
                }
                else
                {
                    pageHeight2 += 120;
                }
            }
            if (pageMarker)
            {
                pageHeight += newPart.Height + partSplit.Height;
            }
            else
            {
                pageHeight2 += newPart.Height + partSplit.Height;
            }
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
        double titleFontScale = title.Length > 9 ? (title.Length > 18 ? 1 : 2) : 3;
        int titleFontThickness = title.Length > 9 ? (title.Length > 18 ? 1 : 2) : 3;

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

    public List<Image<Gray, byte>> CreatePage(List<Image<Gray, byte>> pieces, Image<Gray, byte> image)
    {
        List<List<Image<Gray, byte>>> pages = new();
        List<Image<Gray, byte>> innerList = [ pieces[0] ];

        int partNums = 0;

        List<Image<Gray, byte>> outPages = new();
        var dummyHeader = new Image<Gray, byte>(pieces[0].Width, pieces[0].Height, White);

        foreach (var piece in pieces.Select((value, i) => (value, i)))
        {
            if (piece.i == 0)
            {
                continue;
            }
            if (piece.value.Height > 200)
            {
                partNums += 1;
            }
            if (partNums == 3)
            {
                pages.Add(innerList);
                innerList = [dummyHeader];
                partNums = 1;
            }
            innerList.Add(piece.value);

        }
        pages.Add(innerList);

        foreach (var page in pages)
        {
            int pageMarker = 0;
            Image<Gray, byte> pageImage = new(image.Width, image.Height, White);
            foreach (var piece in page.Select((value, i) => (value, i)))
            {
                if (piece.value.Height == 141)
                {
                    var start = (pageImage.Width) - piece.value.Width - 122;
                    pageImage = SetRoi(pageImage, piece.value, start, pageMarker - 70);
                    pageMarker += piece.value.Height;
                
                    continue;
                }
                

                pageImage = SetRoi(pageImage, piece.value, 0, pageMarker);
                pageMarker += piece.value.Height;
            }

            double widthRatio = A4Width / A4Height;
            int a4Height;
            int a4Width;
            int middleMark;
            if (pageImage.Height * widthRatio < pageImage.Width )
            {
                a4Height = (int)(pageImage.Width / widthRatio);
                a4Width = pageImage.Width + 10;
                middleMark = 0;
                pageImage = ShiftOver(pageImage, 25, 0);
            
            }
            else
            {
                a4Height = pageImage.Height;
                a4Width = (int)(pageImage.Height * widthRatio);
                middleMark = ((a4Width - pageImage.Width) / 2);
                pageImage = ShiftOver(pageImage, 10, 0);
            }

            var imageA4 = new Image<Gray, byte>(a4Width, a4Height, White);
        
            imageA4 = SetRoi(imageA4, pageImage, middleMark == 10? 0: middleMark, 0);

            outPages.Add(imageA4);

        }

        return outPages;
        
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

    public Image<Gray, byte> CreateLine(TuneLine line, int lineCount, int pageWidth, int pageHeight = 0, bool ifLink = false)
    {
        var image = new Image<Gray, byte>(pageWidth, Height + 40 + pageHeight, White);

        var foo2 = new NoteImage();

        Image<Gray, byte> space = foo2.space;

        foreach (var bar in line.line.Select((value, i) => (value, i)))
        {
            var imageNote = CreateBar(bar.value);
            var barwidth = ((Width + 16) * bar.value.CurrentLength) + 55;
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

        var image = new Image<Gray, byte>(notewidth * bar.CurrentLength, Height + 40, White);

        var isTrip = false;
        int tripCount = 0;

        foreach (var note in bar.bar.Select((value, i) => (value, i)))
        {
            Image<Gray, byte> imageNote;

            if (note.value.GetType().Equals(typeof(Duplet)))
            {
                imageNote = CreateDuplet((Duplet)note.value);
            }
            else if (note.value.GetType().Equals(typeof(Triplet)))
            {
                imageNote = CreateTriplet((Triplet)note.value);
                isTrip = true;
            }
            else
            {
                imageNote = CreateNote((Singlet)note.value);
            }

            image = SetRoi(image, imageNote, (note.i + tripCount) * notewidth);
            if (isTrip)
            {
                tripCount += 1;
                isTrip = false;
            }
        }

        return image;
    }

    public Image<Gray, byte> CreateNote(Singlet bar)
    {
        var foo2 = new NoteImage();

        string note = bar.Note.NoteType.ToString();

        Image<Gray, byte> noteImage = foo2.GetType().GetField(note).GetValue(foo2) as Image<Gray, byte>;

        var image = new Image<Gray, byte>(Width + 16, Height + 40, White);

        noteImage = SetRoi(image, noteImage, 4, 18);

        if (bar.Note.NoteType == NoteType.r || bar.Note.NoteType == NoteType.l)
        {
            noteImage = ShiftBack(noteImage, 0, 10);
        }

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
                noteImage = AddHigh(noteImage, 10);
            }
        }
        if (bar.Note.AccidentalType == AccidentalType.Sharp && bar.Note.OctaveType != OctaveType.High)
        {
            noteImage = AddSharp(noteImage);
        }
        if (bar.Note.ShortLongNote)
        {
            noteImage = AddShortLong(noteImage);
        }

        return noteImage;
    }

    public Image<Gray, byte> CreateDuplet(Duplet bar)
    {
        List<Note> notes = [bar.FirstNote, bar.SecondNote];

        var foo2 = new NoteImage();

        var fullImage = new Image<Gray, byte>((Width) * 2, Height + 60, White);
        var resize = new Image<Gray, byte>((int)Math.Round(((Width) * 2) * 0.633333), (int)Math.Round((Height + 60) * 0.633333), White);

        foreach (var note in notes.Select((value, i) => (value, i)))
        {
            Image<Gray, byte> noteImage = foo2.GetType().GetField(note.value.NoteType.ToString()).GetValue(foo2) as Image<Gray, byte>;

            var image = new Image<Gray, byte>(noteImage.Width, Height + 40, White);

            noteImage = SetRoi(image, noteImage,0, 18);

            if (note.value.OctaveType == OctaveType.Low)
            {
                noteImage = AddLow(noteImage);
            }
            else if (note.value.OctaveType == OctaveType.High)
            {
                if (note.value.AccidentalType == AccidentalType.Sharp)
                {
                    noteImage = AddHighAndSharp(noteImage);
                }
                else
                {
                    noteImage = AddHigh(noteImage, 5);
                }
            }
            if (note.value.AccidentalType == AccidentalType.Sharp && note.value.OctaveType != OctaveType.High)
            {
                noteImage = AddSharp(noteImage);
            }

            fullImage = SetRoi(fullImage, image, (note.i * (Width)), 0);

        }
        fullImage = ShiftBack(fullImage, 0, 0);
        CvInvoke.Resize(fullImage, resize, new Size(), 0.633333, 0.633333);

        var bigImage = new Image<Gray, byte>((Width + 16), Height + 40, White);
        var dupImage = foo2.duplet;

        bigImage = ShiftOver(
            SetRoi(SetRoi(bigImage, dupImage), resize, 0, 25)
            , 0, 10);
        return bigImage;
    }

    public Image<Gray, byte> CreateTriplet(Triplet bar)
    {
        List<Note> notes = [bar.FirstNote, bar.SecondNote, bar.ThirdNote];

        var foo2 = new NoteImage();

        var fullImage = new Image<Gray, byte>((Width + 4) * 3, Height + 40, White);
        var resize = new Image<Gray, byte>((int)(((Width + 4) * 3) * 0.79166667), (int)((Height + 40) * 0.79166667), White);

        foreach (var note in notes.Select((value, i) => (value, i)))
        {
            Image<Gray, byte> noteImage = foo2.GetType().GetField(note.value.NoteType.ToString()).GetValue(foo2) as Image<Gray, byte>;

            var image = new Image<Gray, byte>(Width + 4, Height + 40, White);

            noteImage = SetRoi(image, noteImage, 4, 18);

            if (note.value.OctaveType == OctaveType.Low)
            {
                noteImage = AddLow(noteImage);
            }
            else if (note.value.OctaveType == OctaveType.High)
            {
                if (note.value.AccidentalType == AccidentalType.Sharp)
                {
                    noteImage = AddHighAndSharp(noteImage);
                }
                else
                {
                    noteImage = AddHigh(noteImage, 4, 2);
                }
            }
            if (note.value.AccidentalType == AccidentalType.Sharp && note.value.OctaveType != OctaveType.High)
            {
                noteImage = AddSharp(noteImage);
            }
            

            fullImage = SetRoi(fullImage, image, (note.i * (Width + 4)), 0);
            
        }
        fullImage = ShiftBack(fullImage, 0, 0);
        
        CvInvoke.Resize(fullImage, resize, new Size(), 0.79166667, 0.79166667);

        var bigImage = new Image<Gray, byte>((Width + 16) * 2, Height + 40, White);
        var tripImage = foo2.triplet;

        bigImage = SetRoi(bigImage, tripImage);
        bigImage = SetRoi(bigImage, resize, 0, 20);

        return bigImage;
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

        var image = new Image<Gray, byte>( (linkStart.Width * middleNums), linkStart.Height, White);

        image = SetRoi(image, linkStart, 0, 0);

        foreach (int y in Enumerable.Range(1, middleNums - 2).Select(x => x * linkStart.Width))
        {
            image = SetRoi(image, linkMiddle, y, 0);
        }
        image = SetRoi(image, linkEnd, ((middleNums - 1) * linkStart.Width), 0);

        var line2 = CreateLine(line, 0, image.Width, 21, true);
        line2 = ShiftOver(line2, 0, 20);

        CvInvoke.BitwiseNot(image, image);
        CvInvoke.BitwiseNot(line2, line2);
        CvInvoke.BitwiseOr(image, line2, image);
        CvInvoke.BitwiseNot(image, image);

        return image;
    }

    private Image<Gray, byte> SetRoi(Image<Gray, byte> bigImage, Image<Gray, byte> smallImage, int shiftWidth = 0, int shiftHeight = 0)
    {
        int height = smallImage.Height;
        int width = smallImage.Width;

        Image<Gray, byte> bigImage2 = new(bigImage.Width, bigImage.Height, White);

        foreach (int y in Enumerable.Range(0, height))
        {
            foreach (int x in Enumerable.Range(0, width))
            {
                bigImage2[y + shiftHeight, x + shiftWidth] = smallImage[y, x];
            }
        }

        CvInvoke.BitwiseNot(bigImage, bigImage);
        CvInvoke.BitwiseNot(bigImage2, bigImage2);
        CvInvoke.BitwiseOr(bigImage, bigImage2, bigImage);
        CvInvoke.BitwiseNot(bigImage, bigImage);

        return bigImage;
    }

    private Image<Gray, byte> ShiftOver(Image<Gray, byte> image, int shiftWidth = 0, int shiftHeight = 0)
    {
        int height = image.Height;
        int width = image.Width;

        Image<Gray, byte> smallImage = new(width, height, White);

        foreach (int y in Enumerable.Range(0, height - shiftHeight))
        {
            foreach (int x in Enumerable.Range(0, width - shiftWidth))
            {
                smallImage[y + shiftHeight, x + shiftWidth] = image[y, x];
            }
        }

        return smallImage;
    }

    private Image<Gray, byte> ShiftBack(Image<Gray, byte> image, int shiftWidth = 0, int shiftHeight = 0)
    {
        int height = image.Height;
        int width = image.Width;

        Image<Gray, byte> smallImage = new(width, height, White);

        foreach (int y in Enumerable.Range(shiftHeight, (height - shiftHeight)))
        {
            foreach (int x in Enumerable.Range(shiftWidth, (width - shiftWidth)))
            {
                smallImage[y - shiftHeight, x - shiftWidth] = image[y, x];
            }
        }

        return smallImage;
    }

    private Image<Gray, byte> AddAboveNote(Image<Gray, byte> bigImage, Image<Gray, byte> symbolImage, int shiftSide = 0, int shiftDown = 0)
    {
        int width = bigImage.Width;

        int sharpHeight = symbolImage.Height;
        int sharpWidth = symbolImage.Width;

        var shift = 2;

        Image<Gray, byte> newImage = new Image<Gray, byte>(bigImage.Width, bigImage.Height, White);

        foreach (int y in Enumerable.Range(0, sharpHeight))
        {
            foreach (int x in Enumerable.Range(0, sharpWidth))
            {
                newImage[y + shiftDown, width - (x + shift) - shiftSide] = symbolImage[y, sharpWidth - x - 1];
            }
        }

        CvInvoke.BitwiseNot(bigImage, bigImage);
        CvInvoke.BitwiseNot(newImage, newImage);
        CvInvoke.BitwiseOr(bigImage, newImage, bigImage);
        CvInvoke.BitwiseNot(bigImage, bigImage);

        return bigImage;
    }

    private Image<Gray, byte> AddSharp(Image<Gray, byte> bigImage)
    {
        var noteImage = new NoteImage();

        Image<Gray, byte> sharpSymbol = noteImage.sharp;

        bigImage = AddAboveNote(bigImage, sharpSymbol);

        return bigImage;
    }

    private Image<Gray, byte> AddHigh(Image<Gray, byte> bigImage, int shiftSide = 0, int shiftDown = 0)
    {
        var noteImage = new NoteImage();

        Image<Gray, byte> highSymbol = noteImage.high;

        bigImage = AddAboveNote(bigImage, highSymbol, shiftSide, shiftDown);

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

    private Image<Gray, byte> AddShortLong(Image<Gray, byte> bigImage)
    {
        var noteImage = new NoteImage();

        Image<Gray, byte> lowSymbol = noteImage.__;

        int lowHeight = lowSymbol.Height;
        int lowWidth = lowSymbol.Width;

        foreach (int y in Enumerable.Range(0, lowHeight))
        {
            foreach (int x in Enumerable.Range(0, lowWidth))
            {
                bigImage[y + Height + 16, (Width + 15 - x)] = lowSymbol[y, x];
            }
        }

        return bigImage;
    }

    #endregion
}
