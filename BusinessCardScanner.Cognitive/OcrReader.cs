using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BusinessCardScanner.Cognitive.Common;
using BusinessCardScanner.Cognitive.Entities;
using Newtonsoft.Json;

namespace BusinessCardScanner.Cognitive
{
    public class OcrReader
    {
        public async Task<ContactCard> ReadBusinessCard(byte[] fileContent)
        {
            OcrData data = await GetOCRData(fileContent).ConfigureAwait(false);
            ContactCard contact = new ContactCard();
            Region region = data.Regions[0];
            //contact.Name = GetName(region);
            //contact.Company = GetCompany(region);
            //contact.Position = GetPosition(region);
            contact.PhoneNo = GetFromRegex(region, Constants.RegexPatterns.Phone);
            contact.Email = GetFromRegex(region, Constants.RegexPatterns.Email);
            contact.Website = GetFromRegex(region, Constants.RegexPatterns.Website, Constants.RegexPatterns.WebsiteFacebook);
            contact.Facebook = GetFromRegex(region, Constants.RegexPatterns.Facebook);
            contact.Twitter = GetFromRegex(region, Constants.RegexPatterns.Twitter);

            return contact;
        }

        private async Task<OcrData> GetOCRData(byte[] fileContent)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add(Constants.CognitiveKeyHeader, Constants.CognitiveKey);
            HttpResponseMessage response;
            using (var content = new ByteArrayContent(fileContent))
            {
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(Constants.OctetStreamHeader);
                response = await client.PostAsync(Constants.CognitiveUrl, content).ConfigureAwait(false);
            }
           return JsonConvert.DeserializeObject<OcrData>(await response.Content.ReadAsStringAsync());
        }

        private string GetFromRegex(Region r, string pattern, string notContains = null)
        {
            foreach (Line l in r.Lines)
            {
                foreach (Word w in l.Words)
                {
                    if (Regex.IsMatch(w.Text, pattern, RegexOptions.IgnoreCase))
                    {
                        if (string.IsNullOrEmpty(notContains))
                            return w.Text;
                        else
                        {
                            if (!w.Text.Contains(notContains))
                                return w.Text;
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}
