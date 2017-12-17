using System.Text;

namespace BusinessCardScanner.Cognitive.Entities
{
    public class ContactCard
    {
        public string Name { get; set; }
        public string Company { get; set; }
        public string Position { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format(Titles.Name, Name));
            sb.AppendLine(string.Format(Titles.Company, Company));
            sb.AppendLine(string.Format(Titles.Position, Position));
            sb.AppendLine(string.Format(Titles.Phone, PhoneNo));
            sb.AppendLine(string.Format(Titles.Email, Email));
            sb.AppendLine(string.Format(Titles.Website, Website));
            sb.AppendLine(string.Format(Titles.Facebook, Facebook));
            sb.AppendLine(string.Format(Titles.Twitter, Twitter));
            return sb.ToString();
        }
    }
}