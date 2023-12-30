namespace Achiever.Shared
{
    public static class RequestUriParser
    {
        public static string Parse<TRequest>(TRequest request)
        {
            var name = nameof(TRequest);
            var endpointName = ToKebabCase(name);
            return endpointName;
        }

        private static string ToKebabCase(string str)
        {
            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "-" + x.ToString() : x.ToString())).ToLower();
        }
    }
}
