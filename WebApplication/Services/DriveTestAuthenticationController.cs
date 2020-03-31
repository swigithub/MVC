using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Data;
using System.IO;

namespace WebApplication.Services
{
    public class DriveTestAuthenticationController : ApiController
    {
        private SqlConnection con;

        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["AirViewConnectionString"].ConnectionString;
            con = new SqlConnection(constr);

        }
        [Route("swi/AuthResponse"), HttpPost]
        public HttpResponseMessage AuthResponse(string PayLoad)
        {
            string PublicKey = "0CF1440AD0928B68F9F867985DEA97B3562A92B09867B6732065FFC54ED6CFA7";

            try
            {
                byte[] AllData = HexStringToByteArray(PayLoad);
                byte[] PartialLoad = Decryption(AllData, PublicKey);
                byte[] deli = new byte[] { 95, 95, 95, 95 };
                byte[][] separateByteArrays = SeparateByteArraysByDelimiter(PartialLoad, deli);
                string PrivateKey = Encoding.ASCII.GetString(separateByteArrays[1]);

                byte[] FinalLoad = Decryption(separateByteArrays[0], PrivateKey);

                string Result = Encoding.UTF8.GetString(FinalLoad);
                string[] SplittedResult = Result.Split(',');

                if (SplittedResult.Length > 1)
                {
                    string IMEI = SplittedResult[1];
                    string SerialKey = SplittedResult[0];


                    // Check If Valid
                    //return true

                    connection();
                    SqlCommand com = new SqlCommand("AV_VerifyIMEIandSerial", con);
                    com.CommandType = CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@IMEI", IMEI);
                    com.Parameters.AddWithValue("@SerialNo", SerialKey);
                    con.Open();
                    SqlDataAdapter adot = new SqlDataAdapter(com);
                    DataTable dt = new DataTable();
                    adot.Fill(dt);
                    con.Close();
                    if (dt.Rows.Count > 0)
                    {
                        //res.Message = "Success";
                        return Request.CreateResponse(HttpStatusCode.OK, true);
                    }
                    else
                    {
                        //return false
                        //res.Message = "Not Matched";
                        return Request.CreateResponse(HttpStatusCode.OK, false);
                    }

                }
                else
                {
                    //res.Message = "Not Matched";
                    return Request.CreateResponse(HttpStatusCode.OK, false);
                }
            }
            catch (Exception e)
            {
                //res.Message= e.Message;
                return Request.CreateResponse(HttpStatusCode.OK, false);
            }



            //return Request.CreateResponse(HttpStatusCode.OK,true);
        }

        public static byte[] Decryption(byte[] byteArray, string password)
        {
            Byte[] newByteArray;
            MemoryStream plainText = new MemoryStream(byteArray);
            MemoryStream encryptedData = new MemoryStream();
            SharpAESCrypt.SharpAESCrypt.Decrypt(password, plainText, encryptedData);
            newByteArray = encryptedData.ToArray();
            return newByteArray;


        }
        public static byte[] Encryption(string plain, string password)
        {

            byte[] byteArray = Encoding.UTF8.GetBytes(plain);
            Byte[] newByteArray;
            MemoryStream plainText = new MemoryStream(byteArray);
            MemoryStream encryptedData = new MemoryStream();
            SharpAESCrypt.SharpAESCrypt.Encrypt(password, plainText, encryptedData);
            newByteArray = encryptedData.ToArray();
            return newByteArray;

        }
        //convert hex to byte array
        public static byte[] HexStringToByteArray(string hex)
        {
            hex = hex.Replace(" ", "").Replace("-", "");

            var numberChars = hex.Length;
            var bytes = new byte[numberChars / 2];
            for (var i = 0; i < numberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }


        // seperate byte arrays using delimiter
        public static byte[][] SeparateByteArraysByDelimiter(byte[] source, byte[] separator)
        {
            var Parts = new List<byte[]>();
            var Index = 0;
            byte[] Part;
            for (var I = 0; I < source.Length; ++I)
            {
                if (Equals(source, separator, I))
                {
                    Part = new byte[I - Index];
                    Array.Copy(source, Index, Part, 0, Part.Length);
                    Parts.Add(Part);
                    Index = I + separator.Length;
                    I += separator.Length - 1;
                }
            }
            Part = new byte[source.Length - Index];
            Array.Copy(source, Index, Part, 0, Part.Length);
            Parts.Add(Part);
            return Parts.ToArray();
        }
        static bool Equals(byte[] source, byte[] separator, int index)
        {
            for (int i = 0; i < separator.Length; ++i)
                if (index + i >= source.Length || source[index + i] != separator[i])
                    return false;
            return true;
        }



    }
}
