using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SumFileSplitter
{
    class EBCDICHandler
    {
        protected static int[] NumtoPack3 = { 0x0000, 0x0001, 0x0002, 0x0003, 0x0004, 0x0005, 0x0006, 0x0007, 0x0008,
            0x0009, 0x0010, 0x0011, 0x0012, 0x0013, 0x0014, 0x0015, 0x0016, 0x0017, 0x0018, 0x0019, 0x0020, 0x0021,
            0x0022, 0x0023, 0x0024, 0x0025, 0x0026, 0x0027, 0x0028, 0x0029, 0x0030, 0x0031, 0x0032, 0x0033, 0x0034,
            0x0035, 0x0036, 0x0037, 0x0038, 0x0039, 0x0040, 0x0041, 0x0042, 0x0043, 0x0044, 0x0045, 0x0046, 0x0047,
            0x0048, 0x0049, 0x0050, 0x0051, 0x0052, 0x0053, 0x0054, 0x0055, 0x0056, 0x0057, 0x0058, 0x0059, 0x0060,
            0x0061, 0x0062, 0x0063, 0x0064, 0x0065, 0x0066, 0x0067, 0x0068, 0x0069, 0x0070, 0x0071, 0x0072, 0x0073,
            0x0074, 0x0075, 0x0076, 0x0077, 0x0078, 0x0079, 0x0080, 0x0081, 0x0082, 0x0083, 0x0084, 0x0085, 0x0086,
            0x0087, 0x0088, 0x0089, 0x0090, 0x0091, 0x0092, 0x0093, 0x0094, 0x0095, 0x0096, 0x0097, 0x0098, 0x0099 };
        protected static string[] Pack3toNum = { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "EE",
			"EE", "EE", "EE", "EE", "EE", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "EE", "EE", "EE",
			"EE", "EE", "EE", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "EE", "EE", "EE", "EE", "EE",
			"EE", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "EE", "EE", "EE", "EE", "EE", "EE", "40",
			"41", "42", "43", "44", "45", "46", "47", "48", "49", "EE", "EE", "EE", "EE", "EE", "EE", "50", "51", "52",
			"53", "54", "55", "56", "57", "58", "59", "EE", "EE", "EE", "EE", "EE", "EE", "60", "61", "62", "63", "64",
			"65", "66", "67", "68", "69", "EE", "EE", "EE", "EE", "EE", "EE", "70", "71", "72", "73", "74", "75", "76",
			"77", "78", "79", "EE", "EE", "EE", "EE", "EE", "EE", "80", "81", "82", "83", "84", "85", "86", "87", "88",
			"89", "EE", "EE", "EE", "EE", "EE", "EE", "90", "91", "92", "93", "94", "95", "96", "97", "98", "99", "EE",
			"EE", "EE", "EE", "EE", "EE" };
	    protected static int[] NumtoPack3E = { 0x000F, 0x001F, 0x002F, 0x003F, 0x004F, 0x005F, 0x006F, 0x007F, 0x008F,
            0x009F };
        protected static  string[] Pack3toNumE = { "0F", "1F", "2F", "3F", "4F", "5F", "6F", "7F", "8F", "9F", };

        // unpack comp-3 field into a string
        public static string unPacknum3(byte[] bytebuf, int offset, int len)
        {
            int noofbytes = len;            
            byte[] charchr = new byte[2 * noofbytes - 1];

            int count = 0;
            int i = 0;
            string val = "";
            for (count = 0; count < noofbytes - 1; count++)
            {
                val = val + Pack3toNum[(int)(bytebuf[offset + count] & 0x000000ff)];
            }

            val = val + Pack3toNumE[(int)((bytebuf[offset + count] & 0x000000f0) >> 4)];
            return (val.Substring(0, val.Length - 1));
        }

        // pack string into comp-3 field
        public static byte[] Packnum3(string str, int len)
        {
            string str1 = str;            
            int leng = 2 * len - 1;
            byte[] bytebuf = new byte[len];
            while (str1.Length < leng)
            {
                str1 = "0" + str1;
            }
            Console.WriteLine(str1);
            int count = 0;
            int i = 0;
            byte[] charbuf = new byte[len];
            charbuf = Encoding.ASCII.GetBytes(str1);

            while (count < str1.Length - 1)
            {
                bytebuf[i++] = (byte)NumtoPack3[Int32.Parse(str1.Substring(count, 2))];
                count += 2;
            }
            bytebuf[i] = (byte)NumtoPack3E[Int32.Parse(str1.Substring(count, 1))];
            return bytebuf;
        }

        public static string unPacknum3(byte[] bytebuf, int offset, int len,string format)
        {
            int noofbytes = len;
            byte[] charchr = new byte[2 * noofbytes - 1];

            int count = 0;
            int i = 0;
            string val = "";
            for (count = 0; count < noofbytes - 1; count++)
            {
                val = val + Pack3toNum[(int)(bytebuf[offset + count] & 0x000000ff)];
            }

            val = val + Pack3toNumE[(int)((bytebuf[offset + count] & 0x000000f0) >> 4)];
            val = val.Substring(0, val.Length - 1);
            if (format.ToUpper().IndexOf("V") > 0)
            {
                int decpos = format.Length - format.ToUpper().IndexOf("V");
                val = val.Substring(0, val.Length - decpos + 1)  + "." + val.Substring(val.Length - decpos + 1);
            }
            return val;
        }

        public static byte[] Packnum3(string str, int len,string format)
        {
            string str1 = str;
            bool positiveAmount = false;
            bool negativeAmount = false;
            if (format.ToUpper().IndexOf("V") > 0)
            {
                str1 = str1.Replace(".", "");

            }
            if (format.ToUpper().IndexOf("S") >= 0)
            {
                if (str1.IndexOf("+") >= 0)
                {
                    str1 = str1.Replace("+", "");
                    positiveAmount = true;
                }
                if (str1.IndexOf("-") >= 0)
                {
                    str1 = str1.Replace("-", "");
                    negativeAmount = true;
                }
            }
            byte[] bytebuf = new byte[len];
            while (str1.Length < 2 * len - 1)
            {
                str1 = "0" + str1;
            }
            Console.WriteLine(str1);
            int count = 0;
            int i = 0;
            byte[] charbuf = new byte[2 * len];
            charbuf = Encoding.ASCII.GetBytes(str1);

            while (count < str1.Length - 1)
            {
                bytebuf[i++] = (byte)NumtoPack3[Int32.Parse(str1.Substring(count, 2))];
                count += 2;
            }
            bytebuf[i] = (byte)NumtoPack3E[Int32.Parse(str1.Substring(count, 1))];
            if (positiveAmount)
            {

            }
            if (negativeAmount)
            {

            }
            return bytebuf;
        }

        #region public static byte[] ConvertAsciiToEbcdic(byte[] asciiData)
        public static byte[] ConvertAsciiToEbcdic(byte[] asciiData)
        {
            // Create two different encodings.         
            Encoding ascii = Encoding.ASCII;
            Encoding ebcdic = Encoding.GetEncoding("IBM037");

            //Retutn Ebcdic Data
            return Encoding.Convert(ascii, ebcdic, asciiData);
        }
        #endregion

        #region public static byte[] ConvertEbcdicToAscii(byte[] ebcdicData)
        public static byte[] ConvertEbcdicToAscii(byte[] ebcdicData)
        {
            // Create two different encodings.      
            Encoding ascii = Encoding.ASCII;
            Encoding ebcdic = Encoding.GetEncoding("IBM037");

            //Retutn Ascii Data 
            return Encoding.Convert(ebcdic, ascii, ebcdicData);
        }
        #endregion

        #region public static byte[] ConvertAsciiToEbcdic(byte[] asciiData,int off, int lgth)
        public static byte[] ConvertAsciiToEbcdic(byte[] asciiData, int off, int lgth)
        {
            byte[] data = new byte[lgth];
            Array.Copy(asciiData, off, data, 0, lgth);
            // Create two different encodings.         
            Encoding ascii = Encoding.ASCII;
            Encoding ebcdic = Encoding.GetEncoding("IBM037");

            //Retutn Ebcdic Data
            return Encoding.Convert(ascii, ebcdic, data);
        }
        #endregion

        #region public static byte[] ConvertEbcdicToAscii(byte[] ebcdicData,int off, int lgth)
        public static byte[] ConvertEbcdicToAscii(byte[] ebcdicData, int off, int lgth)
        {
            byte[] data = new byte[lgth];
            Array.Copy(ebcdicData, off, data, 0, lgth);
            // Create two different encodings.      
            Encoding ascii = Encoding.ASCII;
            Encoding ebcdic = Encoding.GetEncoding("IBM037");

            //Retutn Ascii Data 
            return Encoding.Convert(ebcdic, ascii, data);
        }
        #endregion
        public static void convertFileToEbcdic(string fileName, string outFileName, int recLgth)
        {
            //debugLogger.Log(FileLogger.TRACE_LEVEL, "Method Enter: " + "convertFileToEbcdic");
            try
            {
                BinaryWriter bw1 = new BinaryWriter(new FileStream(fileName + "-convert.dat", FileMode.Create));
                BinaryReader br = new BinaryReader(File.Open(fileName, FileMode.Open));
                while (br.BaseStream.Position != br.BaseStream.Length)
                {
                    byte[] indata = br.ReadBytes(recLgth);
                    bw1.Write(ConvertAsciiToEbcdic(indata, 0, recLgth));
                }
                bw1.Close();
                br.Close();
            }
            catch (Exception ex)
            {
                //debugLogger.Log(ex.Message);
                //errorLogger.Log(ex.ToString());
            }
            //debugLogger.Log(FileLogger.TRACE_LEVEL, "Method Exit: " + "convertFileToEbcdic");
        }
        public static void convertFileToAscii(string fileName, string outFileName, int recLgth)
        {
            //debugLogger.Log(FileLogger.TRACE_LEVEL, "Method Enter: " + "convertFileToAscii");
            try
            {
                BinaryWriter bw1 = new BinaryWriter(new FileStream(outFileName, FileMode.Create));
                BinaryReader br = new BinaryReader(File.Open(fileName, FileMode.Open));
                while (br.BaseStream.Position != br.BaseStream.Length)
                {
                    byte[] indata = br.ReadBytes(recLgth);
                    byte[] outData = ConvertEbcdicToAscii(indata);
                    bw1.Write(ConvertEbcdicToAscii(indata, 0, recLgth));
                    //bw1.Write(System.Environment.NewLine);
                }
                bw1.Close();
                br.Close();
            }
            catch (Exception ex)
            {
                //debugLogger.Log(ex.Message);
                //errorLogger.Log(ex.ToString());
                Console.WriteLine(ex);
            }
            //debugLogger.Log(FileLogger.TRACE_LEVEL, "Method Exit: " + "convertFileToAscii");
        }
        public static string convertStringtoNumeric(string input, int fldLgth)
        {
            string inputstr = input.Trim();
            while (inputstr.Length < fldLgth)
                inputstr = "0" + inputstr;
            return inputstr;
        }
    }
}
