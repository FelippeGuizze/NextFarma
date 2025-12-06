using System.Linq;

namespace NextFarma.Helpers
{
    public static class Formatters
    {
        public static string FormatRG(string? rg)
        {
            if (string.IsNullOrWhiteSpace(rg)) return string.Empty;
            var digits = new string(rg.Where(char.IsDigit).ToArray());
            // Common RG format: ##.###.###-# (9 digits)
            if (digits.Length == 9)
            {
                return string.Format("{0}.{1}.{2}-{3}", digits.Substring(0,2), digits.Substring(2,3), digits.Substring(5,3), digits.Substring(8,1));
            }
            return digits;
        }

        public static string FormatPhone(string? phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return string.Empty;
            var digits = new string(phone.Where(char.IsDigit).ToArray());
            if (digits.Length == 11)
            {
                // (##) #####-####
                return string.Format("({0}) {1}-{2}", digits.Substring(0,2), digits.Substring(2,5), digits.Substring(7,4));
            }
            if (digits.Length == 10)
            {
                // (##) ####-####
                return string.Format("({0}) {1}-{2}", digits.Substring(0,2), digits.Substring(2,4), digits.Substring(6,4));
            }
            return digits;
        }

        public static string FormatCEP(string? cep)
        {
            if (string.IsNullOrWhiteSpace(cep)) return string.Empty;
            var digits = new string(cep.Where(char.IsDigit).ToArray());
            if (digits.Length == 8)
            {
                return string.Format("{0}-{1}", digits.Substring(0,5), digits.Substring(5,3));
            }
            return digits;
        }
    }
}
