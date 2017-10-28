using System;
using System.Collections.Generic;
using System.Linq;
namespace RevStackCore.AuthorizeDotNet
{
    public static class ANetExtensions
    {
        public static string ToCardExpirationDate(this string expDate)
        {
            var dt = Convert.ToDateTime(expDate);
            return String.Format("{0:yyyy-MM}", dt);
        }
        public static string ToCardExpirationDate(this DateTime expDate)
        {
            return String.Format("{0:yyyy-MM}", expDate);
        }
        public static decimal ToCurrencyDecimal(this decimal amount)
        {
            string s=amount.ToString("0.00");
            return Convert.ToDecimal(s);
        }
        internal static IEnumerable<ANetMessage> ToMessageEnumerable(this ANetCreateResponseMessages src)
        {
            var messages = src.Messages.Message;
            return messages.Select(x => new ANetMessage
            {
                Code=x.Code,
                Description=x.Text
            });
        }
    }
}
