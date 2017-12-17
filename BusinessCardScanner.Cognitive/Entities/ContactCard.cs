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
            sb.AppendLine("Name: " + Name);
            sb.AppendLine("Company: " + Company);
            sb.AppendLine("Position: " + Position);
            sb.AppendLine("Phone: " + PhoneNo);
            sb.AppendLine("Email: " + Email);
            sb.AppendLine("Website: " + Website);
            sb.AppendLine("Facebook: " + Facebook);
            sb.AppendLine("Twitter: " + Twitter);

            return sb.ToString();
        }
    }
}