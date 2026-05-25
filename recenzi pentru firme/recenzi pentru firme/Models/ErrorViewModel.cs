/*
 * DESCRIERE:
 * Model de vizualizare (ViewModel) utilizat pentru transmiterea detaliilor despre erorile
 * apărute în timpul procesării cererilor HTTP către vizualizarea de eroare generică a aplicației.
 */

namespace recenzi_pentru_firme.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
