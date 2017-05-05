using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace EDLib
{
    public class Utility
    {       
        public static bool FunChechSum(byte[] nCheckByte, byte nCheckSum) {
            int XORMask = 0;

            //以Bit處理結果和以Byte相同
            //nCheckByte = Big5.GetBytes(sChechSum);
            try {
                for (int nCount = 0; nCount < nCheckByte.Length; nCount++)
                    XORMask ^= nCheckByte[nCount];

                if (XORMask != (int) nCheckSum)
                    return false;
                return true;
            } catch {
                return false;
            }
        }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public static void WriteIniString(string section, string key, string val, string filePath) {
            WritePrivateProfileString(section, key, val, filePath);
        }
        public static string ReadIniString(string section, string key, string filePath) {
            StringBuilder temp = new StringBuilder(255);
            GetPrivateProfileString(section, key, "", temp, 255, filePath);
            return temp.ToString();
        }
        public static bool IsNaturalNumber(string str) {
            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[A-Za-z0-9]+$");
            return reg1.IsMatch(str);
        }
    }
    public class dMath
    {
        //Returns the standard deviation of data array
        private static double StandardDeviation(double[] data) {
            double DataAverage = 0;
            double TotalVariance = 0;
            int Max = data.Length;

            if (Max == 0)
                return 0;

            DataAverage = data.Average();//Average(data);

            for (int i = 0; i < Max; i++)
                TotalVariance += (data[i] - DataAverage) * (data[i] - DataAverage); //Better performance than Math.pow

            return Math.Sqrt(TotalVariance / Max);
        }


        private static double SafeDivide(double value1, double value2) {
            if (value2 != 0)
                return value1 / value2;
            return double.PositiveInfinity;
        }
    }
}

