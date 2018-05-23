/*******************************************************
* Author       : V U M Sastry Sagi
* Date         : 10/21/2013
* Purpose      : Base 64 Encryption and Decryption
*******************************************************/

using System;
using System.Linq;
using System.Text;

namespace Security
{
    [Serializable]
    public class Base64
    {
        #region.......Private Properties.......
        
        private int blockCount;
        private char[] encSource;
        private int length;
        private int length2;
        private int length3;
        private int paddingCount;
        private byte[] source;
        
        private byte Char2Sixbit(char c)
        {
            char[] chArray = new char[]
            {
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
                'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f',
                'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v',
                'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '+', '/'
            };
            if (c != '=')
            {
                for (int i = 0; i < 0x40; i++)
                {
                    if (chArray[i] == c)
                    {
                        return (byte)i;
                    }
                }
            }
            return 0;
        }
        
        private char Sixbit2Char(byte b)
        {
            char[] chArray = new char[]
            {
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
                'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f',
                'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v',
                'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '+', '/'
            };
            if ((b >= 0) && (b <= 0x3f))
            {
                return chArray[b];
            }
            return ' ';
        }
        
        #endregion.......Private Properties.......
        
        #region.......Public Methods.......
        
        public string Decode(string encString)
        {
            char[] chArray = encString.ToCharArray();
            int num = 0;
            this.encSource = chArray;
            this.length = chArray.Length;
            for (int i = 0; i < 2; i++)
            {
                if (chArray[(this.length - i) - 1] == '=')
                {
                    num++;
                }
            }
            this.paddingCount = num;
            this.blockCount = this.length / 4;
            this.length2 = this.blockCount * 3;
            byte[] buffer = new byte[this.length];
            byte[] buffer2 = new byte[this.length2];
            for (int j = 0; j < this.length; j++)
            {
                buffer[j] = this.Char2Sixbit(this.encSource[j]);
            }
            for (int k = 0; k < this.blockCount; k++)
            {
                byte num8 = buffer[k * 4];
                byte num9 = buffer[(k * 4) + 1];
                byte num10 = buffer[(k * 4) + 2];
                byte num11 = buffer[(k * 4) + 3];
                byte num4 = (byte)(num8 << 2);
                byte num5 = (byte)((num9 & 0x30) >> 4);
                num5 = (byte)(num5 + num4);
                num4 = (byte)((num9 & 15) << 4);
                byte num6 = (byte)((num10 & 60) >> 2);
                num6 = (byte)(num6 + num4);
                num4 = (byte)((num10 & 3) << 6);
                byte num7 = num11;
                num7 = (byte)(num7 + num4);
                buffer2[k * 3] = num5;
                buffer2[(k * 3) + 1] = num6;
                buffer2[(k * 3) + 2] = num7;
            }
            this.length3 = this.length2 - this.paddingCount;
            byte[] bytes = new byte[this.length3];
            for (int m = 0; m < this.length3; m++)
            {
                bytes[m] = buffer2[m];
            }
            StringBuilder builder = new StringBuilder();
            builder.Append(Encoding.UTF8.GetChars(bytes));
            return builder.ToString();
        }
        
        public string Encode(string decString)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(decString);
            this.source = bytes;
            this.length = bytes.Length;
            if ((this.length % 3) == 0)
            {
                this.paddingCount = 0;
                this.blockCount = this.length / 3;
            }
            else
            {
                this.paddingCount = 3 - (this.length % 3);
                this.blockCount = (this.length + this.paddingCount) / 3;
            }
            this.length2 = this.length + this.paddingCount;
            byte[] buffer2 = new byte[this.length2];
            for (int i = 0; i < this.length2; i++)
            {
                if (i < this.length)
                {
                    buffer2[i] = this.source[i];
                }
                else
                {
                    buffer2[i] = 0;
                }
            }
            byte[] buffer3 = new byte[this.blockCount * 4];
            char[] chArray = new char[this.blockCount * 4];
            for (int j = 0; j < this.blockCount; j++)
            {
                byte num2 = buffer2[j * 3];
                byte num3 = buffer2[(j * 3) + 1];
                byte num4 = buffer2[(j * 3) + 2];
                byte num6 = (byte)((num2 & 0xfc) >> 2);
                byte num5 = (byte)((num2 & 3) << 4);
                byte num7 = (byte)((num3 & 240) >> 4);
                num7 = (byte)(num7 + num5);
                num5 = (byte)((num3 & 15) << 2);
                byte num8 = (byte)((num4 & 0xc0) >> 6);
                num8 = (byte)(num8 + num5);
                byte num9 = (byte)(num4 & 0x3f);
                buffer3[j * 4] = num6;
                buffer3[(j * 4) + 1] = num7;
                buffer3[(j * 4) + 2] = num8;
                buffer3[(j * 4) + 3] = num9;
            }
            for (int k = 0; k < (this.blockCount * 4); k++)
            {
                chArray[k] = this.Sixbit2Char(buffer3[k]);
            }
            switch (this.paddingCount)
            {
                case 1:
                    chArray[(this.blockCount * 4) - 1] = '=';
                    break;
                case 2:
                    chArray[(this.blockCount * 4) - 1] = '=';
                    chArray[(this.blockCount * 4) - 2] = '=';
                    break;
            }
            StringBuilder builder = new StringBuilder();
            builder.Append(chArray);
            return builder.ToString();
        }

        #endregion.......Public Methods.......
    }
}