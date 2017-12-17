namespace BusinessCardScanner.Cognitive.Common
{
    public class Constants
    {
        public const string CognitiveUrl = "https://westcentralus.api.cognitive.microsoft.com/vision/v1.0";
        public const string CognitiveKeyHeader = "Ocp-Apim-Subscription-Key";
        public const string CognitiveKey = "03103390f4644b52b25ba2cb88e4851f";
        public const string OctetStreamHeader = "application/octet-stream";
        public class RegexPatterns
        {
            public const string Phone = @"^d+$";
            public const string Email = @"^([a-z0-9_.-]+)@([da-z.-]+).([a-z.]{2,6})$";
            public const string Website = "^www.";
            public const string WebsiteFacebook = "facebook";
            public const string Facebook = @"^www.Facebook.com";
            public const string Twitter = "^@";
        }
    }
}