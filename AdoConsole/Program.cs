using System;
using DBContactLibrary;
using DBContactLibrary.Model;

namespace AdoConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            TestContact();
            Console.ReadLine();
            TestContactInfo();
            Console.ReadLine();
            TestReadContactEntity();
            Console.ReadLine();
            TestReadAllContactEntities();
        }

        private static void TestReadAllContactEntities()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("[Test of ReadAllContactEntities()]");

            SqlService db = new();
            var entities = db.ReadAllContactEntities();

            Console.ForegroundColor = ConsoleColor.White;
            foreach (var entity in entities)
            {
                Console.WriteLine($"ID = {entity.ID}, SSN = {entity.SSN}, fName = {entity.FirstName}, lName = {entity.LastName}");
                foreach (var info in entity.ContactInfo)
                {
                    Console.WriteLine($"  {info.ID:000}[{info.Info}]");
                }
                Console.WriteLine();
            }
        }

        private static void TestReadContactEntity()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("[Test of ReadContactEntity]");

            SqlService db = new();
            var entity = db.ReadContactEntity(6);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(entity);
        }

        private static void TestContactInfo()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("[Test of ContactInfo]");

            SqlService db = new();
            ContactInfo newInfo = new("absolutelynew@newnew.net");

            // CreateContactInfo
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Trying to add {newInfo} ...");
            var dbInfo = db.CreateContactInfo(newInfo);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Result: [{dbInfo}]\n");
            if (dbInfo is null) return;

            // ReadContactInfo
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Trying to read ID [{dbInfo.ID}] ...");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Result: [{db.ReadContactInfo(dbInfo.ID)}]\n");

            // ReadAllContactInfo
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Trying to read all contacts ...");
            Console.ForegroundColor = ConsoleColor.White;
            foreach (var item in db.ReadAllContactInfo())
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();

            // UpdateContactInfo
            ContactInfo updInfo = dbInfo with { Info = "updated@updupd.co.uk" };
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Trying to update from [{dbInfo}] to [{updInfo}] ...");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Updated? : [{db.UpdateContactInfo(updInfo)}]");

            // DeleteContactInfo
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Trying to delete ID[{dbInfo.ID}]...");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Deleted? : {db.DeleteContactInfo(dbInfo.ID)}\n");
        }

        private static void TestContact()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("[Test of Contact]");

            SqlService db = new();
            var newContact = new Contact("18110101-9876", "Richard", "Lionheart");

            // CreateContact
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Trying to add {newContact}...");
            var dbContact = db.CreateContact(newContact);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Result: [{dbContact}]\n");
            if (dbContact is null) return;

            // ReadContact
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Trying to read ID [{dbContact.ID}] ...");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Result: [{db.ReadContact(dbContact.ID)}]\n");

            // ReadAllContacts
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Trying to read all contacts ...");
            Console.ForegroundColor = ConsoleColor.White;
            foreach (var item in db.ReadAllContacts())
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();

            // UpdateContact
            Console.ForegroundColor = ConsoleColor.Blue;
            Contact updContact = dbContact with { LastName = "Moore" };
            Console.WriteLine($"Trying to update from [{dbContact}] to [{updContact}]...");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Updated? : {db.UpdateContact(updContact)}\n");

            // DeleteContact
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Trying to delete ID[{dbContact.ID}]...");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Deleted? : {db.DeleteContact(dbContact.ID)}\n");
        }
    }
}
