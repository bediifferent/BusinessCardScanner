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
            ContactCard cc = new ContactCard();
            Region r = data.Regions[0];
            //cc.Name = GetName(r);
            //cc.Company = GetCompany(r);
            //cc.Position = GetPosition(r);
            cc.PhoneNo = GetFromRegex(r, @"^d+$");
            cc.Email = GetFromRegex(r, @"^([a-z0-9_.-]+)@([da-z.-]+).([a-z.]{2,6})$");
            cc.Website = GetFromRegex(r, "^www.", "facebook");
            cc.Facebook = GetFromRegex(r, @"^www.Facebook.com");
            cc.Twitter = GetFromRegex(r, "^@");

            return cc;
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
