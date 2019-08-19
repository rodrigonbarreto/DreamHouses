using System.Runtime.Serialization;

namespace DreamHouses.Model
{
    [DataContract]
    public class Paging
    {
        [DataMember(Name = "AantalPaginas")]
        public int NumberOfPages { get; set; }
        [DataMember(Name = "HuidigePagina")]
        public int CurrentPage { get; set; }
        [DataMember(Name = "VolgendeUrl")]
        public string NextUrl { get; set; }
    }
}
