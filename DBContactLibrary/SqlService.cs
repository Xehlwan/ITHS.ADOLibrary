using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using DBContactLibrary.Entites;
using DBContactLibrary.Model;

namespace DBContactLibrary
{
    public class SqlService : IDisposable
    {
        private const ApplicationIntent applicationintentDef = ApplicationIntent.ReadWrite;
        private const int connecttimeoutDef = 30;
        private const string datasourceDef = @"(localdb)\MSSQLLocalDB";
        private const bool encryptDef = false;
        private const string initialcatalogDef = @"DBContact";
        private const bool integratedsecurityDef = true;
        private const bool multisubnetfailoverDef = false;
        private const bool trustservercertificateDef = false;

        private SqlConnectionStringBuilder builder;
        private SqlConnection connection;

        public SqlService() : this(datasourceDef,
                                   initialcatalogDef,
                                   integratedsecurityDef,
                                   connecttimeoutDef,
                                   encryptDef,
                                   trustservercertificateDef,
                                   applicationintentDef,
                                   multisubnetfailoverDef)
        { }

        public SqlService(string connectionString)
        {
            builder = new(connectionString);
            connection = new(builder.ConnectionString);
        }

        public SqlService(string dataSource,
                          string initialCatalog,
                          bool integratedSecurity = integratedsecurityDef,
                          int connectTimeout = connecttimeoutDef,
                          bool encrypt = encryptDef,
                          bool trustServerCertificate = trustservercertificateDef,
                          ApplicationIntent applicationIntent = applicationintentDef,
                          bool multiSubnetFailover = multisubnetfailoverDef)
        {
            builder = new()
            {
                DataSource = dataSource,
                InitialCatalog = initialCatalog,
                IntegratedSecurity = integratedSecurity,
                ConnectTimeout = connectTimeout,
                Encrypt = encrypt,
                TrustServerCertificate = trustServerCertificate,
                ApplicationIntent = applicationIntent,
                MultiSubnetFailover = multiSubnetFailover
            };
            connection = new(builder.ConnectionString);
        }

        /// <summary>
        /// Create a <see cref="Contact"/> in the database.
        /// </summary>
        /// <param name="contact">The contact to be added. The <b>SSN</b> must be unique.</param>
        /// <returns>
        /// A new <see cref="Contact"/> record with the assigned <b>ID</b>, or
        /// <see langword="null"/> if a duplicate exists or an error occurred.
        /// </returns>
        public Contact CreateContact(Contact contact)
        {
            using SafeCommand safe = new(this);
            SqlCommand sql = safe.Command;
            sql.CommandText = @"[dbo].[CreateContact]";
            sql.ParamVarChar("SSN", contact.SSN, 13);
            sql.ParamNVarChar("FirstName", contact.FirstName, 50);
            sql.ParamNVarChar("LastName", contact.LastName, 50);
            var result = sql.ParamInt(ParameterDirection.ReturnValue);

            try
            {
                return sql.ExecuteNonQuery() != 0 ? contact with { ID = (int)result.Value } : null;
            }
            catch (SqlException ex)
            {
                if (ex.IsErrorDuplicateKey())
                {
                    return null;
                }
                throw;
            }
        }

        /// <summary>
        /// Create a <see cref="ContactInfo"/> in the database.
        /// </summary>
        /// <param name="info">The <see cref="ContactInfo"/> to add.</param>
        /// <returns>
        /// A new <see cref="ContactInfo"/> record with the assigned <b>ID</b>, or
        /// <see langword="null"/> if a duplicate exists or an error occurred.
        /// </returns>
        public ContactInfo CreateContactInfo(ContactInfo info)
        {
            using var safe = new SafeCommand(this);
            SqlCommand sql = safe.Command;
            sql.CommandText = @"[dbo].[CreateContactInfo]";
            sql.ParamNVarChar("Info", info.Info, 50);
            sql.ParamInt("ContactID", info.ContactID);
            var result = sql.ParamInt(ParameterDirection.ReturnValue);
            try
            {
                return sql.ExecuteNonQuery() != 0 ? info with { ID = (int)result.Value } : null;
            }
            catch (SqlException ex)
            {
                if (ex.IsErrorDuplicateKey())
                {
                    return null;
                }
                throw;
            }
        }

        /// <summary>
        /// Delete a <see cref="Contact"/> from the database.
        /// </summary>
        /// <param name="ID">The ID of the <see cref="Contact"/> to delete.</param>
        /// <returns><see langword="true"/> if the deletion succeeded; otherwise, <see langword="false"/>.</returns>
        public bool DeleteContact(int ID)
        {
            using SafeCommand safe = new(this);
            SqlCommand sql = safe.Command;
            sql.CommandText = @"[dbo].[DeleteContact]";
            sql.ParamInt("Id", ID);
            return sql.ExecuteNonQuery() > 0;
        }

        /// <summary>
        /// Delete a <see cref="ContactInfo"/> from the database.
        /// </summary>
        /// <param name="ID">The ID of the <see cref="ContactInfo"/> to delete.</param>
        /// <returns><see langword="true"/> if the deletion succeeded; otherwise, <see langword="false"/>.</returns>
        public bool DeleteContactInfo(int ID)
        {
            using SafeCommand safe = new(this);
            SqlCommand sql = safe.Command;
            sql.CommandText = @"[dbo].[DeleteContactInfo]";
            sql.ParamInt("Id", ID);

            return sql.ExecuteNonQuery() > 0;
        }

        public void Dispose()
        {
            connection.Dispose();
        }

        public List<ContactInfo> ReadAllContactInfo()
        {
            using SafeCommand safe = new(this);
            SqlCommand sql = safe.Command;
            sql.CommandText = @"[dbo].[ReadAllContactInfo]";

            return sql.ExecuteReader()
                      .ToLinq()
                      .Select(q => new ContactInfo(ID: (int)q["ID"],
                                                   Info: (string)q["Info"],
                                                   ContactID: q["ContactID"] is int i ? i : null))
                      .ToList();
        }

        /// <summary>
        /// Get all <see cref="Contact"/> s in the database.
        /// </summary>
        /// <returns>
        /// A <see cref="List{Contact}"/> containing all <see cref="Contact"/> s in the database.
        /// </returns>
        public List<Contact> ReadAllContacts()
        {
            using SafeCommand safe = new(this);
            SqlCommand sql = safe.Command;
            sql.CommandText = @"[dbo].[ReadAllContacts]";

            return sql.ExecuteReader()
                      .ToLinq()
                      .Select(q => new Contact(ID: (int)q["Id"],
                                               SSN: (string)q["SSN"],
                                               FirstName: (string)q["FirstName"],
                                               LastName: (string)q["LastName"]))
                      .ToList();
        }

        /// <summary>
        /// Read a <see cref="Contact"/> from the database.
        /// </summary>
        /// <param name="ID">The ID of the contact.</param>
        /// <returns>
        /// The <see cref="Contact"/> matching the ID, or <see langword="null"/> if none exists.
        /// </returns>
        public Contact ReadContact(int ID)
        {
            using SafeCommand safe = new(this);
            SqlCommand sql = safe.Command;
            sql.CommandText = @"[dbo].[ReadContact]";
            sql.ParamInt("Id", ID);

            return sql.ExecuteReader()
                        .ToLinq()
                        .Select((q) => new Contact(ID: (int)q["Id"],
                                                   SSN: (string)q["SSN"],
                                                   FirstName: (string)q["FirstName"],
                                                   LastName: (string)q["LastName"]))
                        .FirstOrDefault();
        }

        /// <summary>
        /// Read a <see cref="ContactInfo"/> from the database.
        /// </summary>
        /// <param name="ID">The ID for the <see cref="ContactInfo"/> to find.</param>
        /// <returns>
        /// A <see cref="ContactInfo"/>, or <see langword="null"/> if the ID wasn't found.
        /// </returns>
        public ContactInfo ReadContactInfo(int ID)
        {
            using SafeCommand safe = new(this);
            SqlCommand sql = safe.Command;
            sql.CommandText = @"[dbo].[ReadContactInfo]";
            sql.ParamInt("Id", ID);

            return sql.ExecuteReader()
                      .ToLinq()
                      .Select(q => new ContactInfo(ID: (int)q["ID"],
                                                   Info: (string)q["Info"],
                                                   ContactID: q["ContactID"] is int i ? i : null))
                      .FirstOrDefault();
        }

        public ContactEntity ReadContactEntity(int ID)
        {
            if (ReadContact(ID) is not Contact contact) return null;
            var info = ReadAllContactInfo().Where(x => x.ContactID == contact.ID).ToImmutableList();
            return new(contact) { ContactInfo = info };
        }

        public List<ContactEntity> ReadAllContactEntities()
        {
            List<ContactEntity> entities = new();
            List<Contact> contacts = ReadAllContacts();
            var info = ReadAllContactInfo();
            foreach (var contact in contacts)
            {
                entities.Add(
                    new(contact)
                    {
                        ContactInfo = info.Where(x => x.ContactID == contact.ID).ToImmutableList()
                    });
            }
            return entities;
        }

        /// <summary>
        /// Update the info for a <see cref="Contact"/>. Matches on <b>ID</b> and updates all other info.
        /// </summary>
        /// <param name="contact">
        /// The <see cref="Contact"/> to update. The <b>ID</b> is used to match the <see cref="Contact"/>.
        /// </param>
        /// <returns><see langword="true"/> if the deletion succeeded; otherwise, <see langword="false"/>.</returns>
        public bool UpdateContact(Contact contact)
        {
            using SafeCommand safe = new(this);
            SqlCommand sql = safe.Command;
            sql.CommandText = @"[dbo].[UpdateContact]";
            sql.ParamInt("Id", contact.ID);
            sql.ParamVarChar("SSN", contact.SSN, 13);
            sql.ParamNVarChar("FirstName", contact.FirstName, 50);
            sql.ParamNVarChar("LastName", contact.LastName, 50);

            return sql.ExecuteNonQuery() > 0;
        }

        /// <summary>
        /// Update the info for a <see cref="ContactInfo"/>. Matches on <b>ID</b> and updates all other info.
        /// </summary>
        /// <param name="contact">
        /// The <see cref="ContactInfo"/> to update. The <b>ID</b> is used to match the <see cref="ContactInfo"/>.
        /// </param>
        /// <returns><see langword="true"/> if the deletion succeeded; otherwise, <see langword="false"/>.</returns>
        public bool UpdateContactInfo(ContactInfo info)
        {
            using SafeCommand safe = new(this);
            SqlCommand sql = safe.Command;
            sql.CommandText = @"[dbo].[UpdateContactInfo]";
            sql.ParamInt("Id", info.ID);
            sql.ParamNVarChar("Info", info.Info, 50);
            sql.ParamInt("ContactID", info.ContactID);

            return sql.ExecuteNonQuery() > 0;
        }

        private sealed class SafeCommand : IDisposable
        {
            public SqlCommand Command { get; init; }

            public SafeCommand(SqlService service)
            {
                Command = new SqlCommand()
                {
                    Connection = service.connection,
                    CommandType = CommandType.StoredProcedure
                };
                Command.Connection.Open();
            }

            public void Dispose()
            {
                if (Command.Connection is not null && Command.Connection.State != ConnectionState.Closed)
                    Command.Connection.Close();
                if (Command is not null)
                    Command.Dispose();
            }
        }
    }
}