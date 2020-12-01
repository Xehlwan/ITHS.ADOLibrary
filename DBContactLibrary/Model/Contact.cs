namespace DBContactLibrary.Model
{
    public record Contact(int ID, string SSN, string FirstName, string LastName)
    {
        public Contact() : this(default) { }
        public Contact(string ssn, string firstName, string lastName) : this(-1, ssn, firstName, lastName) { }
    }
}