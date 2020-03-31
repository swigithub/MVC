using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace TcpService
{
    internal class CommonOperation
    {
        private string LogFilepath { get; set; }
        private string ExceptionLogFilepath { get; set; }
        private string SgnalRAPI { get; set; }

        private static object locker = new Object();
        public CommonOperation()
        {

        }

        public CommonOperation(string LgFilepath, string ExceptLogFilepath,string SignalRAPI)
        {
            LogFilepath = LgFilepath;
            ExceptionLogFilepath = ExceptLogFilepath;
            SgnalRAPI = SignalRAPI;
        }

        public static byte[] StringToByteArray(String hex)
        {
            double NumberChars = hex.Length;
            double FinalVal = Math.Ceiling(NumberChars / 2);
            byte[] bytes = new byte[Convert.ToInt16(FinalVal)];
            for (int i = 0; i < NumberChars; i += 2)
                if (i + 2 > NumberChars)
                {
                    bytes[i / 2] = Convert.ToByte(hex.Substring(i, 1), 16);

                }
                else
                {
                    bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
                }

            return bytes;
        }

        public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return (TimeZoneInfo.ConvertTimeToUtc(dateTime) -
                   new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds;
        }

        public static Int64 HexaDecimalToDecimalConversion(string hexValue)
        {
            return Convert.ToInt64(hexValue, 16);
        }

        public static string DecimalToHexaDecimal(int decValue)
        {
            return decValue.ToString("X");
        }

        public static string HexStringToBinary(string hexString)
        {
            var lup = new Dictionary<char, string>{
            { '0', "0000"},
            { '1', "0001"},
            { '2', "0010"},
            { '3', "0011"},

            { '4', "0100"},
            { '5', "0101"},
            { '6', "0110"},
            { '7', "0111"},

            { '8', "1000"},
            { '9', "1001"},
            { 'A', "1010"},
            { 'B', "1011"},

            { 'C', "1100"},
            { 'D', "1101"},
            { 'E', "1110"},
            { 'F', "1111"}};

            var ret = string.Join("", from character in hexString
                                      select lup[character]);
            return ret;
        }

        public static List<string> listGetCRCSinocastelForm(string HexaCRC)
        {
            List<string> CRCCollect = new List<string>();
            char[] HexaArr = HexaCRC.ToArray();

            if (HexaArr.Length == 4)
            {
                // CRCCollect.Add(HexaArr[3].ToString() + HexaArr[4].ToString());
                //CRCCollect.Add(HexaArr[1].ToString() + HexaArr[2].ToString());
                CRCCollect.Add(HexaArr[2].ToString() + HexaArr[3].ToString() + HexaArr[0].ToString() + HexaArr[1].ToString());
            }
            else
            {
                CRCCollect.Add("0000");
            }


            return CRCCollect;
        }

        public void WriteFile(string text)
        {
            lock (locker)
            {
                File.AppendAllText(LogFilepath, text);              
            }
        }

        public void WriteExceptionFile(string text)
        {
            lock (locker)
            {
                File.AppendAllText(ExceptionLogFilepath, text);              
            }
        }

        public async void  SendDataToApi(DataPacket DataPktobj)
        {
            try
            {
                using (var clientObj = new HttpClient())
                {
                    clientObj.BaseAddress = new Uri(SgnalRAPI.ToString());
                    clientObj.DefaultRequestHeaders.Accept.Clear();
                    clientObj.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await clientObj.PostAsJsonAsync("/swi/FleetAPI/post", DataPktobj);

                    if (response.IsSuccessStatusCode)
                    {
                        Uri ncrUrl = response.Headers.Location;
                    }
                }
            }

            catch (Exception ex)
            {

            }
        }

        public static string ConvertValueToHexa(string text)
        {
            char[] chars = text.ToCharArray();
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in chars)
            {
                stringBuilder.Append(((Int16)c).ToString("x"));
            }

            String textAsHex = stringBuilder.ToString();
            return textAsHex;
        }

    }

}
