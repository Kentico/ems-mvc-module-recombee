using CMS;
using CMS.ContactManagement;
using Kentico.Recombee;

[assembly: RegisterImplementation(typeof(IContactMergeProcessor), typeof(ContactMergeProcessor))]

namespace Kentico.Recombee
{
    public interface IContactMergeProcessor
    {
        void Process(ContactInfo source, ContactInfo target);
    }
}