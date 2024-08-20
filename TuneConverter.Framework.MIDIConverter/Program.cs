using System;
using NAudio.Midi;
using NAudio.Wave;
using Sanford.Multimedia.Midi;
using System.IO;

public class Program
{
    public static void Main(string[] args)
    {
        Decode("");

        //// Create a new sequence
        //Sequence sequence = new Sequence();

        //// Create a track for the melody
        //Track melodyTrack = new Track();
        //sequence.Add(melodyTrack);

        //// Define the melody (MIDI note numbers)
        //int[] melody = { 60, 62, 64, 65, 67, 69, 71, 72 };
        //int[] melody2 = { 60, 59, 60, 60, 62, 64, 62, 64, 62, 62, 64, 67 };

        //// Add notes to the track
        //foreach (int noteNumber in melody2)
        //{
        //    // Create a note-on event (channel message)
        //    ChannelMessage noteOnEvent = new ChannelMessage(ChannelCommand.NoteOn, 0, noteNumber, 100);

        //    // Insert the note-on event into the track
        //    melodyTrack.Insert(0, noteOnEvent);

        //    // Create a note-off event (channel message) to end the note after 1 second
        //    ChannelMessage noteOffEvent = new ChannelMessage(ChannelCommand.NoteOff, 0, noteNumber, 0);

        //    // Insert the note-off event into the track after 1 second
        //    melodyTrack.Insert(1000, noteOffEvent);
        //}

        //// Save the sequence to a MIDI file
        //sequence.Save("C:/Users/Isaac/source/repos/TuneConverter/TuneConverter.Framework.MIDIConverter/Midi/melody.mid");

        //Console.WriteLine("MIDI melody generated and saved to melody.mid.");


    }

    private static void Decode(string fileName)
    {
        List<string> lines = new List<string>()
         {
             "193 land" ,
"284 sun" ,
"16 too" ,
"136 huge" ,
"26 dont" ,
"286 such" ,
"130 noun" ,
"202 student" ,
"184 brown" ,
"135 complete" ,
"118 play" ,
"29 cook" ,
"72 yard" ,
"233 clock" ,
"275 would" ,
"265 plain" ,
"5 excite" ,
"132 fire" ,
"28 wish" ,
"213 cool" ,
"272 child" ,
"163 past" ,
"212 colony" ,
"222 oil" ,
"7 dog" ,
"115 back" ,
"100 money" ,
"214 kind" ,
"64 open" ,
"107 finger" ,
"19 touch" ,
"109 are" ,
"241 dad" ,
"104 am" ,
"208 modern" ,
"108 meant" ,
"44 ocean" ,
"228 pitch" ,
"194 suit" ,
"58 town" ,
"179 east" ,
"204 over" ,
"35 group" ,
"250 good" ,
"137 kind" ,
"257 down" ,
"71 band" ,
"203 especially" ,
"113 organ" ,
"2 of" ,
"218 fire" ,
"197 out" ,
"247 area" ,
"280 touch" ,
"299 happen" ,
"126 sat" ,
"105 electric" ,
"198 wrote" ,
"67 buy" ,
"120 lot" ,
"252 stop" ,
"13 corn" ,
"201 where" ,
"264 check" ,
"34 live" ,
"150 best" ,
"86 hold" ,
"292 cause" ,
"235 grand" ,
"30 present" ,
"138 indicate" ,
"92 counter" ,
"87 we" ,
"183 like" ,
"36 visit" ,
"79 state" ,
"263 morning" ,
"227 true" ,
"209 are" ,
"234 ball" ,
"254 history" ,
"219 seat" ,
"62 rain" ,
"53 less" ,
"84 glass" ,
"178 tone" ,
"48 song" ,
"156 fair" ,
"226 element" ,
"25 speed" ,
"77 produce" ,
"223 quotient" ,
"46 sand" ,
"232 begin" ,
"83 moment" ,
"66 offer" ,
"267 probable" ,
"3 all" ,
"140 necessary" ,
"281 post" ,
"38 cent" ,
"225 happen" ,
"278 speech" ,
"161 object" ,
"283 silver" ,
"216 third" ,
"166 crease" ,
"261 wait" ,
"168 triangle" ,
"239 idea" ,
"240 clothe" ,
"169 young" ,
"139 discuss" ,
"243 field" ,
"89 company" ,
"96 capital" ,
"255 compare" ,
"99 chart" ,
"122 possible" ,
"174 written" ,
"162 remember" ,
"144 mile" ,
"256 cold" ,
"259 lady" ,
"274 felt" ,
"95 against" ,
"123 skin" ,
"124 prepare" ,
"42 he" ,
"294 card" ,
"196 organ" ,
"145 object" ,
"253 our" ,
"287 major" ,
"20 discuss" ,
"153 system" ,
"111 hole" ,
"75 above" ,
"266 they" ,
"236 produce" ,
"224 straight" ,
"285 level" ,
"164 though" ,
"290 modern" ,
"65 dry" ,
"262 bought" ,
"90 milk" ,
"127 make" ,
"165 show" ,
"23 middle" ,
"170 center" ,
"97 blood" ,
"159 speak" ,
"12 prove" ,
"51 select" ,
"4 power" ,
"172 come" ,
"68 brown" ,
"167 experiment" ,
"230 strong" ,
"101 hurry" ,
"24 touch" ,
"296 reach" ,
"22 case" ,
"119 beat" ,
"182 over" ,
"185 dry" ,
"40 hill" ,
"69 company" ,
"8 opposite" ,
"143 work" ,
"268 field" ,
"188 felt" ,
"187 prepare" ,
"45 now" ,
"82 his" ,
"269 stay" ,
"279 toward" ,
"245 observe" ,
"289 time" ,
"149 stop" ,
"295 possible" ,
"171 card" ,
"14 prepare" ,
"190 current" ,
"57 compare" ,
"116 neighbor" ,
"88 thus" ,
"112 include" ,
"103 copy" ,
"47 bit" ,
"133 stead" ,
"134 does" ,
"80 general" ,
"258 solve" ,
"271 glad" ,
"158 duck" ,
"229 offer" ,
"176 happen" ,
"177 ball" ,
"291 bread" ,
"244 like" ,
"117 machine" ,
"238 come" ,
"59 any" ,
"129 band" ,
"63 it" ,
"94 section" ,
"60 close" ,
"273 heavy" ,
"43 produce" ,
"237 got" ,
"102 possible" ,
"251 insect" ,
"231 way" ,
"50 before" ,
"18 men" ,
"211 bird" ,
"146 ease" ,
"220 trade" ,
"151 winter" ,
"277 am" ,
"157 repeat" ,
"114 first" ,
"242 to" ,
"154 each" ,
"297 guide" ,
"152 column" ,
"206 single" ,
"260 remember" ,
"155 wild" ,
"282 major" ,
"125 coast" ,
"175 class" ,
"11 done" ,
"160 jump" ,
"217 sister" ,
"248 feel" ,
"15 check" ,
"76 fire" ,
"17 nine" ,
"181 indicate" ,
"276 parent" ,
"10 whole" ,
"121 her" ,
"192 the" ,
"128 temperature" ,
"1 design" ,
"56 big" ,
"6 skill" ,
"186 friend" ,
"33 hit" ,
"300 wait" ,
"191 instant" ,
"288 blow" ,
"85 about" ,
"32 chick" ,
"199 answer" ,
"210 man" ,
"81 material" ,
"249 current" ,
"246 think" ,
"98 print" ,
"141 nor" ,
"142 better" ,
"73 example" ,
"61 people" ,
"41 drink" ,
"27 gun" ,
"110 together" ,
"49 cost" ,
"180 require" ,
"293 or" ,
"91 people" ,
"39 planet" ,
"54 ease" ,
"215 ready" ,
"74 enough" ,
"37 sugar" ,
"21 deal" ,
"52 with" ,
"131 us" ,
"270 share" ,
"93 office" ,
"106 protect" ,
"200 low" ,
"221 thus" ,
"173 farm" ,
"9 oxygen" ,
"207 fire" ,
"70 force" ,
"195 select" ,
"147 paragraph" ,
"298 always" ,
"205 poem" ,
"31 chick" ,
"78 planet" ,
"189 fact" ,
"55 moment" ,
"148 term" ,

         };

       // var lines = File.ReadAllLines(fileName);

        Dictionary<int, string> data = lines
            .Select(line => line.Split(' '))
            .ToDictionary(
                parts => int.Parse(parts[0]),
                parts => string.Join(" ", parts.Skip(1)));

        bool messageCompleted = false;
        int endLineNumber = 1;
        int endLineNumberCounter = 1;
        string decryptedMessage = "";
        while (!messageCompleted)
        {
            if (data.ContainsKey(endLineNumber))
            {
                decryptedMessage +=  data[endLineNumber] + " ";
                endLineNumberCounter++;
                endLineNumber += endLineNumberCounter;
                Console.WriteLine(endLineNumber);
            }
            else
            {
                messageCompleted = true;
            }
        }

        Console.WriteLine(decryptedMessage);
    }

    private static void Test1()
    {
        // Create a new output device (MIDI device)
        OutputDevice outDevice = new OutputDevice(0); // Change the index if needed
        PatchChangeMessage(outDevice, GeneralMidiInstrument.Fiddle);

        // Set up the melody (MIDI note numbers)
        int[] melody = { 60, 62, 64, 65, 67, 69, 71, 72 };
        int[] melody2 = { 60, 59, 60, 60, 62, 64, 62, 64, 62, 62, 64, 67 };

        // Set the duration for each note (in milliseconds)
        int noteDuration = 400;

        int foo = 2;

        // Play each note in the melody
        foreach (int noteNumber in melody2)
        {
            int minus = foo == 3 ? 100 : 0;
            // Send note on message
            ChannelMessageBuilder builder = new ChannelMessageBuilder();
            builder.Command = ChannelCommand.NoteOn;
            builder.MidiChannel = 0; // MIDI channel (0-15)
            builder.Data1 = noteNumber; // MIDI note number
            builder.Data2 = 127; // Velocity
            builder.Build();
            outDevice.Send(builder.Result);

            // Wait for the duration of the note
            System.Threading.Thread.Sleep(noteDuration - minus);

            // Send note off message
            builder.Command = ChannelCommand.NoteOff;
            builder.Data2 = 0; // Velocity
            builder.Build();
            outDevice.Send(builder.Result);

            System.Threading.Thread.Sleep(minus / 20);

            if (foo >= 3)
            {
                foo = 1;
            }
            else
            {
                foo++;
            }
        }

        // Dispose the output device
        outDevice.Dispose();
    }

    static void PatchChangeMessage(OutputDevice device, GeneralMidiInstrument instrument)
    {
        // Construct a PatchChange message
        ChannelMessage patchChangeMessage = new ChannelMessage(ChannelCommand.ProgramChange, 0, (int)instrument, 0);

        // Send the PatchChange message to the output device
        device.Send(patchChangeMessage);
    }
}
