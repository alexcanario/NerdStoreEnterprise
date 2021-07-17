using System;

namespace NSE.Identidade.API.Extensions {
    public static class DateTimeExtensions {
        public static long ToUnixEpochDate(this DateTime date) {
            var ret = (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

            return ret;
        }
        
    }
}