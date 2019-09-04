using System.Collections.Generic;
using System.Linq;

using CMS.ContactManagement;

namespace Kentico.Recombee.DatabaseSetup
{
    /// <summary>
    /// Generates contacts for customers.
    /// </summary>
    public class ContactsGenerator
    {
        private readonly string[] contactNames =
        {
            "Deneen Fernald", "Antonio Buker", "Marlon Loos", "Nolan Steckler", "Johnetta Tall",
            "Florence Ramsdell", "Modesto Speaker", "Alissa Ferguson", "Calvin Hollier", "Diamond Paik",
            "Mardell Dohrmann", "Dinorah Clower", "Andrea Humbert", "Tyrell Galvan", "Yong Inskeep",
            "Tom Goldschmidt", "Kimbery Rincon", "Genaro Kneeland", "Roselyn Mulvey", "Nancee Jacobson",
            "Jaime Link", "Fonda Belnap", "Muoi Ishmael", "Pearlene Minjarez", "Eustolia Studstill",
            "Marilynn Manos", "Pamila Turnbow", "Lieselotte Satcher", "Sharron Mellon", "Bennett Heatherington",
            "Spring Hessel", "Lashay Blazier", "Veronika Lecuyer", "Mark Spitz", "Peggy Olson",
            "Tyron Bednarczyk", "Terese Betty", "Bibi Kling", "Bruno Spier", "Cristen Bussey",
            "Daine Pridemore", "Gerald Turpen", "Lela Briese", "Sharda Bonnie", "Omar Martin",
            "Marlyn Pettie", "Shiela Cleland", "Marica Granada", "Garland Reagan", "Mora Gillmore",
            "Mariana Rossow", "Betty Pollan", "Analisa Costilla", "Evelyn Mendez", "April Rubino",
            "Zachariah Roberson", "Sheilah Steinhauser", "Araceli Vallance", "Lashawna Weise", "Charline Durante",
            "Melania Nightingale", "Ema Stiltner", "Lynelle Threet", "Dorcas Cully", "Gregg Carranco",
            "Karla Heiner", "Judson Siegmund", "Alyson Oday", "Winston Laxton", "Jarod Turrentine",
            "Israel Shanklin", "Miquel Jorstad", "Brianne Darrow", "Tamara Rulison", "Elliot Rameriz",
            "Gearldine Nova", "Debi Fritts", "Leota Cape", "Tyler Saleem", "Starr Hyden",
            "Loreen Spigner", "Raisa Germain", "Grace Vigue", "Maryann Munsch", "Jason Chon",
            "Gisele Mcquillen", "Juliane Comeaux", "Willette Dodrill", "Sherril Weymouth", "Ashleigh Dearman",
            "Bret Bourne", "Brittney Cron", "Dustin Evans", "Barbie Dinwiddie", "Ricki Wiener",
            "Bess Pedretti", "Monica King", "Edgar Schuetz", "Jettie Boots", "Jefferson Hinkle",
            "Boots Pedretti", "Boots King", "Boots Schuetz", "Jettie Boots", "Boots Jefferson",
            "King Pedretti", "King King", "King Schuetz", "King Boots", "King Jefferson",
            "King Bednarczyk", "King Betty", "King Kling", "King Spier", "King Bussey",
            "King Pettie", "King Cleland", "King Granada", "King Reagan", "King Gillmore",
            "Fernald Fernald", "Fernald Buker", "Fernald Loos", "Fernald Steckler", "Fernald Tall",
            "Mariana Fernald", "Betty Fernald", "Analisa Fernald", "Evelyn Fernald", "April Fernald",
            "Monica Fernald", "Monica Fernald", "Analisa Monica", "Evelyn Monica", "April Monica",
        };


        /// <summary>
        /// Generates contacts for customers. When database already contains contacts for given customers, new contatact is not created
        /// </summary>
        public IList<ContactInfo> Generate()
        {
            IList<ContactInfo> contacts;
            int numberOfContacts = ContactInfoProvider.GetContacts().Count;

            if (numberOfContacts < 50)
            {
                numberOfContacts = contactNames.Length;
                contacts = new List<ContactInfo>();
                for (int i = 0; i < numberOfContacts; i++)
                {
                    contacts.Add(CreateContact(contactNames[i]));
                }
            }
            else
            {
                contacts = ContactInfoProvider.GetContacts().ToList();
            }

            return contacts;
        }


        private ContactInfo CreateContact(string fullName)
        {
            var words = fullName.Trim().Split(' ');
            var firstName = words[0];
            var lastName = words[1];

            var contact = new ContactInfo
            {
                ContactEmail = $"{firstName.ToLowerInvariant()}@{lastName.ToLowerInvariant()}.local",
                ContactFirstName = firstName,
                ContactLastName = lastName
            };

            ContactInfoProvider.SetContactInfo(contact);
            return contact;
        }
    }
}
