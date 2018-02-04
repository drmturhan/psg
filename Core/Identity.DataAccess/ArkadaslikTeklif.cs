using Core.EntityFramework.SharedEntity;

namespace Identity.DataAccess
{
    public class ArkadaslikTeklif : ArkadaslikTeklif<int>
    {
        public Kullanici TeklifEden { get; set; }

        public Kullanici TeklifEdilen { get; set; }
    }

    
}
