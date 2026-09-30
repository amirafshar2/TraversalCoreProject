using EntityLayer.Concrate;

namespace BusinessLayer.Abstract
{
    public interface INewsletterService : IGenerik_Service<Newsletter>
    {
        /// <summary>Trägt eine E-Mail ein. Gibt false zurück, wenn sie schon existiert.</summary>
        bool Subscribe(string mail);
    }
}
