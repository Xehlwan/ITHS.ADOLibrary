using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContactLibrary.Model
{
    public static class SetupContactAndContactInfo
    {
        const string initContactTableString = "DROP TABLE [dbo].[Contact]\r\nGO\r\n"
                                              + "CREATE TABLE [dbo].[Contact](\r\n"
                                              + "[ID] int PRIMARY KEY IDENTITY NOT NULL,\r\n"
                                              + "SSN] varchar(13) UNIQUE NOT NULL,\r\n"
                                              + "[FirstName] nvarchar(50) NOT NULL,\r\n"
                                              + "[LastName] nvarchar(50) NOT NULL)\r\n";

        const string initContactInfoTable = "DROP TABLE [[dbo].[ContactInfo]]\r\nGO\r\n"
                                            + "CREATE TABLE [dbo].[ContactInfo](\r\n"
                                            + "[ID] [int] PRIMARY KEY IDENTITY NOT NULL,\r\n"
                                            + "[Info] [nvarchar](50) UNIQUE NOT NULL,\r\n"
                                            + "[ContactID] [int] FOREIGN KEY REFERENCES [dbo].[Contact] NULL)\r\n";

        const string initCreateContact = "DROP PROCEDURE [dbo].[CreateContact]\r\nGO\r\n"
                                         + "SET ANSI_NULLS ON\r\nGO\r\n"
                                         + "SET QUOTED_IDENTIFIER ON\r\nGO\r\n"
                                         + "CREATE PROCEDURE [dbo].[CreateContact] \r\n(\r\n"
                                         + "@SSN VARCHAR(13),\r\n"
                                         + "@FirstName NVARCHAR(50),\r\n"
                                         + "@LastName NVARCHAR(50)\r\n) AS\r\nBEGIN\r\n"
                                         + "INSERT into Contact (SSN, FirstName, LastName)\r\n"
                                         + "VALUES\r\n(@SSN, @FirstName, @LastName)\r\n"
                                         + "RETURN SCOPE_IDENTITY()\r\nEND\r\nGO\r\n";

        const string initCreateContactInfo = "DROP PROCEDURE [dbo].[CreateContactInfo] \r\nGO\r\n"
                                             + "SET ANSI_NULLS ON\r\nGO\r\nSET QUOTED_IDENTIFIER ON\r\nGO\r\n"
                                             + "CREATE PROCEDURE [dbo].[CreateContactInfo]\r\n(\r\n"
                                             + "@Info NVARCHAR(50),\r\n"
                                             + "@ContactID int\r\n) AS\r\nBEGIN\r\n"
                                             + "INSERT into ContactInfo (Info, ContactID)\r\n"
                                             + "VALUES (@Info, @ContactID)\r\n"
                                             + "RETURN SCOPE_IDENTITY()\r\nEND\r\nGO";
    }
}
