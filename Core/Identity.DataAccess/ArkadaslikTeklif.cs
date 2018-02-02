using Core.EntityFramework.SharedEntity;

namespace Identity.DataAccess
{
    public class ArkadaslikTeklif : ArkadaslikTeklif<int>
    {
        public Kullanici ArkadaslikIsteyen { get; set; }

        public Kullanici TeklifEdilen { get; set; }
    }

    
}
