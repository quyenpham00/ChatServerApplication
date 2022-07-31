using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServerApplication.Uilities
{
    public class AccessCodeGenerator
    {
        // Base32 encoding - in ascii sort order for easy text based sorting
        private static readonly char[] encode32Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUV".ToCharArray();

        // Seed the lastConnectionId for this application instance with
        // the number of 100-nanosecond intervals that have elapsed since 12:00:00 midnight, January 1, 0001
        // for a roughly increasing _lastId over restarts
        private static long lastId = DateTime.UtcNow.Ticks;

        public string GetNextAccessCode() => GenerateAccessCode(Interlocked.Increment(ref lastId));
        private static string GenerateAccessCode(long id)
        {
            return string.Create(13, id, (buffer, value) =>
            {
                char[] encodeChars = encode32Chars;

                buffer[5] = encodeChars[(value >> 35) & 31];
                buffer[4] = encodeChars[(value >> 40) & 31];
                buffer[3] = encodeChars[(value >> 45) & 31];
                buffer[2] = encodeChars[(value >> 50) & 31];
                buffer[1] = encodeChars[(value >> 55) & 31];
                buffer[0] = encodeChars[(value >> 60) & 31];
            });
        }
    }
}
