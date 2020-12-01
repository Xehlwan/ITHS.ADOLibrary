using System;
using System.Collections.Generic;
using System.Text;
using DBContactLibrary.Model;

namespace DBContactLibrary.Entites
{
    public record ContactEntity(int ID, string SSN, string FirstName, string LastName,
                                IReadOnlyList<ContactInfo> ContactInfo)
                                : Contact(ID, SSN, FirstName, LastName)
    {
        public ContactEntity(Contact contact)
            : this(contact.ID, contact.SSN, contact.FirstName, contact.LastName, default) { }

        public override string ToString()
        {
            StringBuilder sb = new("ContactEntity { ");
            base.PrintMembers(sb);
            sb.Append(", ContactInfo [");
            foreach (var info in ContactInfo)
            {
                sb.Append($"{{ID = {info.ID}, Info = {info.Info}, ContactID = {info.ContactID}}}, ");
            }
            sb.Length -= 2;
            sb.Append("] }");
            return sb.ToString();
        }
    }
}