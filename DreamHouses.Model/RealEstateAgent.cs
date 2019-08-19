using System.Runtime.Serialization;

namespace DreamHouses.Model
{
    [DataContract]
    public class RealEstateAgent
    {
        [DataMember(Name = "MakelaarId")]
        public int Id { get; set; }

        [DataMember(Name = "MakelaarNaam")]
        public string Name { get; set; }


    }
}
