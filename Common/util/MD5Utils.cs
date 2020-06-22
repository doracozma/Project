using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Database
{
    public class MD5Utils
    {

        private static int chrsz = 8;
        private static string b64pad = "";

        public static string GetHash(string input)
        {
            MD5 md5Hash = MD5.Create();

            byte[] hashResult = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hashResult.Length - 1; i++)
            {
                sb.Append(hashResult[i].ToString("X"));
            }

            md5Hash.Dispose();

            return sb.ToString();
        }

        public static string b64_md5(string s)
        {
            return binl2b64(core_md5(str2binl(s), s.Count() * chrsz));
        }

        public static List<int> core_md5(List<int> x, int len)
        {
            /* append padding */
            int paddingIndex = len >> 5;
            while (x.Count() != paddingIndex + 1)
            {
                x.Add(0);
            }
            x[paddingIndex] |= 0x80 << ((len) % 32);
            paddingIndex = (((int)((uint)(len + 64)) >> 9) << 4) + 14;
            while (x.Count() != paddingIndex + 1)
            {
                x.Add(0);
            }
            x[paddingIndex] = len;

            var a = 1732584193;
            var b = -271733879;
            var c = -1732584194;
            var d = 271733878;

            for (var i = 0; i < x.Count(); i += 16)
            {
                var olda = a;
                var oldb = b;
                var oldc = c;
                var oldd = d;

                a = md5_ff(a, b, c, d, x.Count > i + 0 ? x[i + 0] : 0, 7, -680876936);
                d = md5_ff(d, a, b, c, x.Count > i + 1 ? x[i + 1] : 0, 12, -389564586);
                c = md5_ff(c, d, a, b, x.Count > i + 2 ? x[i + 2] : 0, 17, 606105819);
                b = md5_ff(b, c, d, a, x.Count > i + 3 ? x[i + 3] : 0, 22, -1044525330);
                a = md5_ff(a, b, c, d, x.Count > i + 4 ? x[i + 4] : 0, 7, -176418897);
                d = md5_ff(d, a, b, c, x.Count > i + 5 ? x[i + 5] : 0, 12, 1200080426);
                c = md5_ff(c, d, a, b, x.Count > i + 6 ? x[i + 6] : 0, 17, -1473231341);
                b = md5_ff(b, c, d, a, x.Count > i + 7 ? x[i + 7] : 0, 22, -45705983);
                a = md5_ff(a, b, c, d, x.Count > i + 8 ? x[i + 8] : 0, 7, 1770035416);
                d = md5_ff(d, a, b, c, x.Count > i + 9 ? x[i + 9] : 0, 12, -1958414417);
                c = md5_ff(c, d, a, b, x.Count > i + 10 ? x[i + 10] : 0, 17, -42063);
                b = md5_ff(b, c, d, a, x.Count > i + 11 ? x[i + 11] : 0, 22, -1990404162);
                a = md5_ff(a, b, c, d, x.Count > i + 12 ? x[i + 12] : 0, 7, 1804603682);
                d = md5_ff(d, a, b, c, x.Count > i + 13 ? x[i + 13] : 0, 12, -40341101);
                c = md5_ff(c, d, a, b, x.Count > i + 14 ? x[i + 14] : 0, 17, -1502002290);
                b = md5_ff(b, c, d, a, x.Count > i + 15 ? x[i + 15] : 0, 22, 1236535329);
               
                a = md5_gg(a, b, c, d, x.Count > i + 1 ? x[i + 1] : 0, 5, -165796510);
                d = md5_gg(d, a, b, c, x.Count > i + 6 ? x[i + 6] : 0, 9, -1069501632);
                c = md5_gg(c, d, a, b, x.Count > i + 11 ? x[i + 11] : 0, 14, 643717713);
                b = md5_gg(b, c, d, a, x.Count > i + 0 ? x[i + 0] : 0, 20, -373897302);
                a = md5_gg(a, b, c, d, x.Count > i + 5 ? x[i + 5] : 0, 5, -701558691);
                d = md5_gg(d, a, b, c, x.Count > i + 10 ? x[i + 10] : 0, 9, 38016083);
                c = md5_gg(c, d, a, b, x.Count > i + 15 ? x[i + 15] : 0, 14, -660478335);
                b = md5_gg(b, c, d, a, x.Count > i + 4 ? x[i + 4] : 0, 20, -405537848);
                a = md5_gg(a, b, c, d, x.Count > i + 9 ? x[i + 9] : 0, 5, 568446438);
                d = md5_gg(d, a, b, c, x.Count > i + 14 ? x[i + 14] : 0, 9, -1019803690);
                c = md5_gg(c, d, a, b, x.Count > i + 3 ? x[i + 3] : 0, 14, -187363961);
                b = md5_gg(b, c, d, a, x.Count > i + 8 ? x[i + 8] : 0, 20, 1163531501);
                a = md5_gg(a, b, c, d, x.Count > i + 13 ? x[i + 13] : 0, 5, -1444681467);
                d = md5_gg(d, a, b, c, x.Count > i + 2 ? x[i + 2] : 0, 9, -51403784);
                c = md5_gg(c, d, a, b, x.Count > i + 7 ? x[i + 7] : 0, 14, 1735328473);
                b = md5_gg(b, c, d, a, x.Count > i + 12 ? x[i + 12] : 0, 20, -1926607734);
                
                a = md5_hh(a, b, c, d, x.Count > i + 5 ? x[i + 5] : 0, 4, -378558);
                d = md5_hh(d, a, b, c, x.Count > i + 8 ? x[i + 8] : 0, 11, -2022574463);
                c = md5_hh(c, d, a, b, x.Count > i + 11 ? x[i + 11] : 0, 16, 1839030562);
                b = md5_hh(b, c, d, a, x.Count > i + 14 ? x[i + 14] : 0, 23, -35309556);
                a = md5_hh(a, b, c, d, x.Count > i + 1 ? x[i + 1] : 0, 4, -1530992060);
                d = md5_hh(d, a, b, c, x.Count > i + 4 ? x[i + 4] : 0, 11, 1272893353);
                c = md5_hh(c, d, a, b, x.Count > i + 7 ? x[i + 7] : 0, 16, -155497632);
                b = md5_hh(b, c, d, a, x.Count > i + 10 ? x[i + 10] : 0, 23, -1094730640);
                a = md5_hh(a, b, c, d, x.Count > i + 13 ? x[i + 13] : 0, 4, 681279174);
                d = md5_hh(d, a, b, c, x.Count > i + 0 ? x[i + 0] : 0, 11, -358537222);
                c = md5_hh(c, d, a, b, x.Count > i + 3 ? x[i + 3] : 0, 16, -722521979);
                b = md5_hh(b, c, d, a, x.Count > i + 6 ? x[i + 6] : 0, 23, 76029189);
                a = md5_hh(a, b, c, d, x.Count > i + 9 ? x[i + 9] : 0, 4, -640364487);
                d = md5_hh(d, a, b, c, x.Count > i + 12 ? x[i + 12] : 0, 11, -421815835);
                c = md5_hh(c, d, a, b, x.Count > i + 15 ? x[i + 15] : 0, 16, 530742520);
                b = md5_hh(b, c, d, a, x.Count > i + 2 ? x[i + 2] : 0, 23, -995338651);
                
                a = md5_ii(a, b, c, d, x.Count > i + 0 ? x[i + 0] : 0, 6, -198630844);
                d = md5_ii(d, a, b, c, x.Count > i + 7 ? x[i + 7] : 0, 10, 1126891415);
                c = md5_ii(c, d, a, b, x.Count > i + 14 ? x[i + 14] : 0, 15, -1416354905);
                b = md5_ii(b, c, d, a, x.Count > i + 5 ? x[i + 5] : 0, 21, -57434055);
                a = md5_ii(a, b, c, d, x.Count > i + 12 ? x[i + 12] : 0, 6, 1700485571);
                d = md5_ii(d, a, b, c, x.Count > i + 3 ? x[i + 3] : 0, 10, -1894986606);
                c = md5_ii(c, d, a, b, x.Count > i + 10 ? x[i + 10] : 0, 15, -1051523);
                b = md5_ii(b, c, d, a, x.Count > i + 1 ? x[i + 1] : 0, 21, -2054922799);
                a = md5_ii(a, b, c, d, x.Count > i + 8 ? x[i + 8] : 0, 6, 1873313359);
                d = md5_ii(d, a, b, c, x.Count > i + 15 ? x[i + 15] : 0, 10, -30611744);
                c = md5_ii(c, d, a, b, x.Count > i + 6 ? x[i + 6] : 0, 15, -1560198380);
                b = md5_ii(b, c, d, a, x.Count > i + 13 ? x[i + 13] : 0, 21, 1309151649);
                a = md5_ii(a, b, c, d, x.Count > i + 4 ? x[i + 4] : 0, 6, -145523070);
                d = md5_ii(d, a, b, c, x.Count > i + 11 ? x[i + 11] : 0, 10, -1120210379);
                c = md5_ii(c, d, a, b, x.Count > i + 2 ? x[i + 2] : 0, 15, 718787259);
                b = md5_ii(b, c, d, a, x.Count > i + 9 ? x[i + 9] : 0, 21, -343485551);
                
                a = safe_add(a, olda);
                b = safe_add(b, oldb);
                c = safe_add(c, oldc);
                d = safe_add(d, oldd);
            }

            return new List<int>(new int[] { a, b, c, d });

        }

        public static int safe_add(int x, int y)
        {
            var lsw = (x & 0xFFFF) + (y & 0xFFFF);
            var msw = (x >> 16) + (y >> 16) + (lsw >> 16);
            return (msw << 16) | (lsw & 0xFFFF);
        }

        public static List<int> str2binl(string str)
        {
            int size = 0;
            for (var i = 0; i < str.Count() * chrsz; i += chrsz)
            {
                size = Math.Max(size, (i >> 5) + 1);
            }

            int[] bin = new int[size];
            var mask = (1 << chrsz) - 1;
            for (var i = 0; i < str.Count() * chrsz; i += chrsz)
            {
                char c = str[i / chrsz];
                int t = Convert.ToUInt16(c);
                int index = i >> 5;
                bin[index] |= (Convert.ToUInt16(c) & mask) << (i % 32);
            }
            return new List<int>(bin);
        }

        public static string binl2b64(List<int> binarray)
        {

            var tab = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
            var str = "";

            int initSize = binarray.Count();
            for (int i = 0; i < initSize * 4; i += 3)
            {
                var triplet = ((((binarray.Count() > i >> 2 ? binarray[i >> 2] : 0) >> 8 * (i % 4)) & 0xFF) << 16)
                      | ((((binarray.Count() > i + 1 >> 2 ? binarray[i + 1 >> 2] : 0) >> 8 * ((i + 1) % 4)) & 0xFF) << 8)
                      | (((binarray.Count() > i + 2 >> 2 ? binarray[i + 2 >> 2] : 0) >> 8 * ((i + 2) % 4)) & 0xFF);

                
                for (int j = 0; j < 4; j++)
                {
                    if (i * 8 + j * 6 > binarray.Count() * 32)
                    {
                        str += b64pad;
                    }
                    else
                    {
                        str += tab[((triplet >> 6 * (3 - j)) & 0x3F)];
                    }
                }
            }

            return str;

        }

        public static int md5_cmn(int q, int a, int b, int x, int s, int t)
        {
            return safe_add(bit_rol(safe_add(safe_add(a, q), safe_add(x, t)), s), b);
        }
        public static int md5_ff(int a, int b, int c, int d, int x, int s, int t)
        {
            return md5_cmn((b & c) | ((~b) & d), a, b, x, s, t);
        }
        public static int md5_gg(int a, int b, int c, int d, int x, int s, int t)
        {
            return md5_cmn((b & d) | (c & (~d)), a, b, x, s, t);
        }
        public static int md5_hh(int a, int b, int c, int d, int x, int s, int t)
        {
            return md5_cmn(b ^ c ^ d, a, b, x, s, t);
        }
        public static int md5_ii(int a, int b, int c, int d, int x, int s, int t)
        {
            return md5_cmn(c ^ (b | (~d)), a, b, x, s, t);
        }
        public static int bit_rol(int num, int cnt)
        {
            return (num << cnt) | ((int)((uint)num >> (32 - cnt)));
        }

    }
}
