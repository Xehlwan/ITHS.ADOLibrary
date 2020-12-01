namespace DBContactLibrary.Model
{
    public record ContactInfo(int ID, string Info, int? ContactID = null)
    {
        public ContactInfo(string Info, int? ContactID = null) : this(-1, Info, ContactID) { }
    }
}