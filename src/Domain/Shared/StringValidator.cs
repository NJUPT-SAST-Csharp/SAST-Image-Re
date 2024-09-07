namespace Domain.Shared
{
    internal static class StringValidator
    {
        public static bool IsLetterOrDigitOrUnderline(this string value)
        {
            foreach (var c in value)
            {
                if (!char.IsLetterOrDigit(c) && c != '_')
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsLetterOrDigit(this string value)
        {
            foreach (var c in value)
            {
                if (!char.IsLetterOrDigit(c))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
