using Emgu.CV;
using Emgu.CV.CvEnum;
using TuneConverter.Framework.PageImageIO.ImageComponents;
using TuneConverter.Framework.TuneComponents.TuneComponents;
using System.Drawing;
using Emgu.CV.Structure;
using TuneConverter.Framework.TuneComponents.Types;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Linq;
using TuneConverter.Framework.PageComponents;

namespace TuneConverter.Framework.PageImageIO.ImageBuilder;

public class PageAssembler
{
    private const int Height = 80;
    private const int Width = 55 + 5;
    private const double A4Width = 210;
    private const double A4Height = 297;

    private const FontFace font = FontFace.HersheyTriplex;
    private MCvScalar scale = new(255);
    private static readonly Gray White = new(255);
    private readonly LineType lineType = LineType.AntiAlias;

    private static ConcurrentDictionary<int, List<Image<Gray, byte>>> _partsDict = new();
    private static ListComparer<NoteGroup> comparer = new();
    private static ConcurrentDictionary<List<NoteGroup>, Image<Gray, byte>> _barCache = new(comparer);
    private static ConcurrentDictionary<int, Image<Gray, byte>> _pagesDict = new();

    public async Task<List<Image<Gray, byte>>> CreateTune(TuneFull tune)
    {
        List<Image<Gray, byte>>[] listOfPieces = new List<Image<Gray, byte>>[tune.tune.Count];

        int[] heights = new int[tune.tune.Count];

        var title = CreateTitlePage(tune.Title, tune.TuneType, tune.Key.NoteType.ToString() + " " + tune.Key.Keytype.ToString());

        for (int i = 0; i < tune.tune.Count; i++)
        {
            listOfPieces[i] = await Task.Factory.StartNew(() => {
                return AssemblePageSegments(tune.tune[i], tune.TuneType);
            });
        }

        int[] fullPageHeights = new int[(int)Math.Round((tune.tune.Count / 2.0), 0, MidpointRounding.AwayFromZero)];
        Array.Fill(fullPageHeights, title.Height);

        while (listOfPieces.Any(t => t.Equals(new List<Image<Gray, byte>>()) | t.Equals(null))) { }


        List<List<Image<Gray, byte>>> pages = new();
        List<Image<Gray, byte>> innerList = [title];
        var dummyHeader = new Image<Gray, byte>(title.Width, title.Height, White);

        foreach (var doo in _partsDict.Select((value, i) => (value, i)))
        {
            foreach (var dooo in doo.value.Value)
            { 
                innerList.Add(dooo);
                fullPageHeights[(int)(doo.i / 2)] += dooo.Height;
            }
            fullPageHeights[(int)(doo.i / 2)] += heights[doo.i];

            if ((doo.i + 1) % 2 == 0)
            {
                pages.Add(innerList);
                innerList = [dummyHeader];
            }
        }
        if(innerList.Count > 1)
        {
            pages.Add(innerList);
        }
        
        var pageWidth = (Width + 16) * (int)tune.TuneType;
        var image = new Image<Gray, byte>(pageWidth, fullPageHeights.Max(), White);

        return CreatePage(image, pages).Result;
    }

    public static List<Image<Gray, byte>> AssemblePageSegments(TunePart part, TuneType tuneType)
    {
        List<Image<Gray, byte>> pieces = [];
        var newPart = CreatePart(part, tuneType, part.PartNumber);

        pieces.Add(newPart);

        int pageHeight = newPart.Height;

        var pageWidth = (Width + 16) * (int)tuneType;
        var partSplit = new Image<Gray, byte>(pageWidth, 50, White);

        if (part.Link.line.Count > 0)
        {
            pieces.Add(CreateTuneLink(part.Link, tuneType));
            pieces.Add(partSplit);
            pageHeight += 141;
        }
        if (tuneType == TuneType.Reel)
        {
            pieces.Add(partSplit);
            pageHeight += 120;
            pageHeight += partSplit.Height;
        }

        _partsDict.AddOrUpdate(part.PartNumber, pieces, (x, y) => new List<Image<Gray, byte>>());

        return pieces;
    }

    public async Task<List<Image<Gray, byte>>> CreatePage(Image<Gray, byte> image, List<List<Image<Gray, byte>>> pages)
    {
        List<Image<Gray, byte>> outPages = new();

        foreach (var page in pages.Select((value, j) => (value, j)))
        {
            await Task.Factory.StartNew(() => AssemblePage(page.value, image, page.j));
        }

        while (_pagesDict.Count != pages.Count) { }

        return _pagesDict.Values.ToList();

    }

    public void AssemblePage(List<Image<Gray, byte>> page, Image<Gray, byte> image, int pageNumber)
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

        if (pageImage.Height * widthRatio < pageImage.Width)
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

        imageA4 = SetRoi(imageA4, pageImage, middleMark == 10 ? 0 : middleMark, 0);

        _pagesDict.AddOrUpdate(pageNumber, imageA4, (x, y) => image);
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
        double titleFontScale = title.Length > 9 ? (title.Length > 18 ? 1.5 : 2) : 3;
        int titleFontThickness = title.Length > 9 ? (title.Length > 18 ? 1 : 2) : 3;

        Size titleSize = CvInvoke.GetTextSize(title, font, titleFontScale, titleFontThickness, ref baseline);

        double textWidth = ((pageWidth / 2) - (titleSize.Width / 2));
        double textHeight = ((pageHeight / 3) - (titleSize.Height * 0.1));

        int titleStartX = (int)Math.Round(textWidth, 0, MidpointRounding.ToZero);
        int titleStartY = (int)Math.Round(textHeight, 0, MidpointRounding.ToZero);
        int addOn = (title.Length > 9 ? (title.Length > 18 ? 13 : 9) : 0);
        titleStartY += addOn;

        Point point = new(titleStartX, titleStartY);

        CvInvoke.PutText(titlePage, title, point, font, titleFontScale, scale, titleFontThickness, lineType);

        titlePage = CreateTitleLine(titlePage, titleStartX, titleStartY -= addOn, titleSize.Width, scale);
        
        return titlePage;
    }

    private Image<Gray, byte> CreateTitleLine(Image<Gray, byte> titlePage, int titleStartX, int titleStartY, int width, MCvScalar scale)
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

        Point tuneTypePoint = new(16, height + 30);

        CvInvoke.PutText(titlePage, text, tuneTypePoint, font, titleFontScale, scale, titleFontThickness, lineType);

        return titlePage;
    }

    #endregion

    #region Part Generation

    public static Image<Gray, byte> CreatePart(TunePart part, TuneType tuneType, int partNumber)
    {
        int pageHeight = (int)((Height + 60) * (part.part.Count + 1));
        int pageWidth = (Width + 16) * (int)tuneType;

        var image = new Image<Gray, byte>(pageWidth, pageHeight, White);

        var lineSplit = new Image<Gray, byte>(pageWidth, 40, White);

        for (int i = 0; i < part.part.Count; i++)
        {
            var imageLine = CreateLine(part.part[i], i, pageWidth);
            image = SetRoi(image, imageLine, 0, (i * 160) + 10);
            image = SetRoi(image, lineSplit, 0, (120 + (i * 160)) + 10);
        }

        var foo2 = new NoteImage();

        Image<Gray, byte> noteImage = foo2.GetType().GetField("part" + partNumber.ToString()).GetValue(foo2) as Image<Gray, byte>;

        image = SetRoi(image, noteImage, 10, 20);

        return image;
    }

    public static Image<Gray, byte> CreateLine(TuneLine line, int lineCount, int pageWidth, int pageHeight = 0, bool ifLink = false)
    {
        var image = new Image<Gray, byte>(pageWidth, Height + 40 + pageHeight, White);

        Image<Gray, byte> space = NoteImage.space;

        for (int i = 0; i < line.line.Count; i++)
        {
            Image<Gray, byte> imageNote;
            _barCache.TryGetValue( line.line[i].bar , out imageNote);

            imageNote ??= CreateBar(line.line[i]);

            _barCache.TryAdd(line.line[i].bar, imageNote);
            var barwidth = ((Width + 16) * line.line[i].CurrentLength) + 55;

            image = SetRoi(image, imageNote, (i * barwidth) + (ifLink ? 40 : 80));

            if (ifLink)
            {
                continue;
            }
            image = SetRoi(image, space, (i + 1) * barwidth + 25);
        }

        return image;
    }

    public static Image<Gray, byte> CreateBar(TuneBar bar)
    { 
        var notewidth = Width + 16;

        var image = new Image<Gray, byte>(notewidth * bar.CurrentLength, Height + 40, White);

        var isTrip = false;
        int tripCount = 0;

        for (int i = 0; i < bar.bar.Count; i++)
        {
            Image<Gray, byte> imageNote;

            switch (bar.bar[i].GetType().Name.ToString())
            {
                case "Duplet":
                    imageNote = CreateDuplet((Duplet)bar.bar[i]);
                    break;

                case "Triplet":
                    imageNote = CreateTriplet((Triplet)bar.bar[i]);
                    isTrip = true;
                    break;

                default:
                    imageNote = CreateNote((Singlet)bar.bar[i]);
                    break;
            }

            image = SetRoi(image, imageNote, (i + tripCount) * notewidth);

            if (isTrip)
            {
                tripCount += 1;
                isTrip = false;
            }
        }

        return image;
    }

    public static Image<Gray, byte> ArrangeNote(Image<Gray, byte> image, Image<Gray, byte> noteImage, Note bar, int highShiftSide = 0, int highShiftDown = 0)
    {
        if (bar.ShortLongNote)
        {
            noteImage = AddShortLong(noteImage);
        }
        
        if (bar.OctaveType == OctaveType.High)
        {
            switch (bar.AccidentalType)
            {
                case AccidentalType.Sharp:
                    noteImage = AddHighAndSharp(noteImage);
                    break;

                case AccidentalType.Flat:
                    noteImage = AddHighAndFlat(noteImage);
                    break;

                default:
                    noteImage = AddHigh(noteImage, highShiftSide, highShiftDown);
                    break;
            }

            return noteImage;
        }
        
        if (bar.OctaveType == OctaveType.Low)
        {
            noteImage = AddLow(noteImage);
        }
        if (bar.AccidentalType == AccidentalType.Sharp)
        {
            noteImage = AddSharp(noteImage);
        }
        if (bar.AccidentalType == AccidentalType.Flat)
        {
            noteImage = AddFlat(noteImage, highShiftSide);
        }
        if (bar.AccidentalType == AccidentalType.Natural)
        {
            noteImage = AddNatural(noteImage, highShiftSide);
        }
        
        return noteImage;
    }

    public static Image<Gray, byte> CreateNote(Singlet bar)
    {

        Image<Gray, byte> noteImage = typeof(NoteImage).GetField(bar.Note.NoteType.ToString()).GetValue(new NoteImage()) as Image<Gray, byte>;

        var image = new Image<Gray, byte>(Width + 16, Height + 40, White);

        noteImage = SetRoi(image, noteImage, 4, 18);

        if (bar.Note.NoteType == NoteType.r || bar.Note.NoteType == NoteType.l)
        {
            noteImage = ShiftBack(noteImage, 0, 10);
        }

        noteImage = ArrangeNote(image, noteImage, bar.Note, 10, 0);

        return noteImage;
    }

    public static Image<Gray, byte> CreateDuplet(Duplet bar)
    {
        List<Note> notes = [bar.FirstNote, bar.SecondNote];

        var foo2 = new NoteImage();

        var fullImage = new Image<Gray, byte>((Width) * 2, Height + 60, White);
        var resize = new Image<Gray, byte>((int)Math.Round(((Width) * 2) * 0.633333), (int)Math.Round((Height + 60) * 0.633333), White);

        foreach (var note in notes.Select((value, i) => (value, i)))
        {
            Image<Gray, byte> noteImage = foo2.GetType().GetField(note.value.NoteType.ToString()).GetValue(foo2) as Image<Gray, byte>;

            var image = new Image<Gray, byte>(noteImage.Width, Height + 40, White);

            noteImage = SetRoi(image, noteImage, 0, 18);
            noteImage = ArrangeNote(image, noteImage, note.value, 5, 0);
            fullImage = SetRoi(fullImage, image, (note.i * (Width)), 0);
        }

        CvInvoke.Resize(fullImage, resize, new Size(), 0.633333, 0.633333);

        var bigImage = new Image<Gray, byte>((Width + 16), Height + 40, White);
        var dupImage = NoteImage.duplet;

        bigImage = ShiftOver(
            SetRoi(SetRoi(bigImage, dupImage), resize, 0, 25)
            , 0, 10);
        return bigImage;
    }

    public static Image<Gray, byte> CreateTriplet(Triplet bar)
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
            noteImage = ArrangeNote(image, noteImage, note.value, 4, 2);
            fullImage = SetRoi(fullImage, image, (note.i * (Width + 4)), 0);
        }
        
        CvInvoke.Resize(fullImage, resize, new Size(), 0.79166667, 0.79166667);

        var bigImage = new Image<Gray, byte>((Width + 16) * 2, Height + 40, White);
        var tripImage = NoteImage.triplet;

        bigImage = SetRoi(SetRoi(bigImage, tripImage), resize, 0, 20);

        return bigImage;
    }

    public static Image<Gray, byte> CreateTuneLink(TuneLine line, TuneType tuneType)
    {
        var middleNums = line.MaxLength * (line.line[0].MaxLength + 1);

        var linkStart = NoteImage.linkStart;
        var linkMiddle = NoteImage.linkMiddle;
        var linkEnd = NoteImage.linkEnd;

        var image = new Image<Gray, byte>( (linkStart.Width * middleNums), linkStart.Height, White);

        image = SetRoi(image, linkStart, 0, 0);

        foreach (int y in Enumerable.Range(1, middleNums - 2).Select(x => x * linkStart.Width))
        {
            image = SetRoi(image, linkMiddle, y, 0);
        }
        image = SetRoi(image, linkEnd, ((middleNums - 1) * linkStart.Width), 0);

        var line2 = CreateLine(line, 0, image.Width, 21, true);
        line2 = ShiftOver(line2, 0, 20);

        InsertImageIntoOther(image, line2);

        return image;
    }

    private static Image<Gray, byte> SetRoi(Image<Gray, byte> bigImage, Image<Gray, byte> smallImage, int shiftWidth = 0, int shiftHeight = 0)
    {
        Image<Gray, byte> bigImage2 = new(bigImage.Width, bigImage.Height, White);

        foreach (int x in Enumerable.Range(0, smallImage.Width))
        {
            foreach (int y in Enumerable.Range(0, smallImage.Height))
            {
                bigImage2[y + shiftHeight, x + shiftWidth] = smallImage[y, x];
            }
        }

        InsertImageIntoOther(bigImage, bigImage2);

        return bigImage;
    }

    private static void InsertImageIntoOther(Image<Gray, byte> firstImage, Image<Gray, byte> secondImage)
    {
        CvInvoke.BitwiseNot(firstImage, firstImage);
        CvInvoke.BitwiseNot(secondImage, secondImage);
        CvInvoke.BitwiseOr(firstImage, secondImage, firstImage);
        CvInvoke.BitwiseNot(firstImage, firstImage);
    }

    private static Image<Gray, byte> ShiftOver(Image<Gray, byte> image, int shiftWidth = 0, int shiftHeight = 0) => ShiftImage(image, true, shiftWidth, shiftHeight);
    private static Image<Gray, byte> ShiftBack(Image<Gray, byte> image, int shiftWidth = 0, int shiftHeight = 0) => ShiftImage(image, false, - shiftWidth, - shiftHeight);
    private static Image<Gray, byte> ShiftImage(Image<Gray, byte> image, bool over, int shiftWidth = 0, int shiftHeight = 0)
    {
        int height = image.Height;
        int width = image.Width;

        var xRange = Enumerable.Range(over ? 0 : Math.Abs(shiftWidth), width - Math.Abs(shiftWidth));
        var yRange = Enumerable.Range(over ? 0 : Math.Abs(shiftHeight), height - Math.Abs(shiftHeight));

        Image<Gray, byte> smallImage = new(width, height, White);

        foreach (int x in xRange)
        {
            foreach (int y in yRange)
            {
                var yy = y + shiftHeight;
                var xx = x + shiftWidth;
                smallImage[yy, xx] = image[y, x];
            }
        }

        return smallImage;
    }

    private static Image<Gray, byte> AddSharp(Image<Gray, byte> bigImage) => AddAboveNote(bigImage, NoteImage.sharp);
    private static Image<Gray, byte> AddFlat(Image<Gray, byte> bigImage, int small = 0) => AddAboveNote(bigImage, NoteImage.flat, 10 - small);
    private static Image<Gray, byte> AddNatural(Image<Gray, byte> bigImage, int small = 0) => AddAboveNote(bigImage, NoteImage.natural, 10 - small);
    private static Image<Gray, byte> AddHigh(Image<Gray, byte> bigImage, int shiftSide = 0, int shiftDown = 0) => AddAboveNote(bigImage, NoteImage.high, shiftSide, shiftDown);
    private static Image<Gray, byte> AddHighAndSharp(Image<Gray, byte> bigImage) => 
        AddAboveNote(AddAboveNote(bigImage, NoteImage.high), NoteImage.sharp, NoteImage.high.Width, 1);
    private static Image<Gray, byte> AddHighAndFlat(Image<Gray, byte> bigImage) => 
        AddAboveNote(AddAboveNote(bigImage, NoteImage.high), NoteImage.flat, NoteImage.high.Width, 1);

    private static Image<Gray, byte> AddAboveNote(Image<Gray, byte> bigImage, Image<Gray, byte> symbolImage, int shiftSide = 0, int shiftDown = 0)
    {
        int symbolWidth = symbolImage.Width;

        Image<Gray, byte> newImage = new Image<Gray, byte>(bigImage.Width, bigImage.Height, White);

        foreach (int x in Enumerable.Range(0, symbolWidth))
        {
            foreach (int y in Enumerable.Range(0, symbolImage.Height))
            {
                newImage[y + shiftDown, bigImage.Width - (x + 2) - shiftSide] = symbolImage[y, symbolWidth - x - 1];
            }
        }

        InsertImageIntoOther(bigImage, newImage);

        return bigImage;
    }

    private static Image<Gray, byte> AddLow(Image<Gray, byte> bigImage) => AddBelowNote(bigImage, NoteImage.low, 6, Height + 18);
    private static Image<Gray, byte> AddShortLong(Image<Gray, byte> bigImage) => AddBelowNote(bigImage, NoteImage.__, (Width + 15 - (NoteImage.__.Width + 1)), Height + 16);
    private static Image<Gray, byte> AddBelowNote(Image<Gray, byte> bigImage, Image<Gray, byte> symbolImage, int shiftSide = 0, int shiftDown = 0)
    {
        foreach (int x in Enumerable.Range(0, symbolImage.Width))
        {
            foreach (int y in Enumerable.Range(0, symbolImage.Height))
            {
                bigImage[y + shiftDown, (x + shiftSide)] = symbolImage[y, x];
            }
        }

        return bigImage;
    }

    #endregion

    public static void DisplayImage(List<Image<Gray, byte>> assembledPages)
    {
        foreach (var assembledPage in assembledPages)
        {
            Image<Gray, byte> resizedPage = new(assembledPage.Width, assembledPage.Height);

            CvInvoke.Resize(assembledPage, resizedPage, new Size(), 0.5, 0.5);

            CvInvoke.Imshow("s", resizedPage);
            CvInvoke.WaitKey();
        }
    }
}
