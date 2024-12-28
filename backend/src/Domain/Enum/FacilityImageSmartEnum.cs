using Ardalis.SmartEnum;

namespace Playground.Domain.Enum
{
    public class FacilityImageSmartEnum : SmartEnum<FacilityImageSmartEnum>
    {
        public static readonly FacilityImageSmartEnum Taller = new FacilityImageSmartEnum("Taller", 1, new List<string>
        {
            "/images/activities/art-workshop-1.jpg",
            "/images/activities/art-workshop-2.jpg",
            "/images/activities/art-workshop-3.jpg"
        });

        public static readonly FacilityImageSmartEnum Sala = new FacilityImageSmartEnum("Sala", 2, new List<string>
        {
            "/images/activities/kids-guitar.jpg",
            "/images/activities/kids-music.jpg"
        });

        public static readonly FacilityImageSmartEnum Laboratorio = new FacilityImageSmartEnum("Laboratorio", 3, new List<string>
        {
            "/images/activities/kids-science.jpg",
            "/images/activities/kid-science-2.jpg"
        });

        public static readonly FacilityImageSmartEnum Piscina = new FacilityImageSmartEnum("Piscina", 4, new List<string>
        {
            "/images/activities/kids-pool.jpg",
            "/images/activities/pool-1.jpg",
            "/images/activities/pool-2.jpg"
        });

        public static readonly FacilityImageSmartEnum Estudio = new FacilityImageSmartEnum("Estudio", 5, new List<string>
        {
            "/images/activities/dance-1.jpg",
            "/images/activities/dance-2.jpg",
            "/images/activities/dance-3.jpg"
        });

        public static readonly FacilityImageSmartEnum Atraccion = new FacilityImageSmartEnum("Atracción", 6, new List<string>
        {
            "/images/activities/park-1.jpg",
            "/images/activities/park-2.jpg",
            "/images/activities/park-3.jpg"
        });

        private readonly List<string> _imagePaths;

        private FacilityImageSmartEnum(string name, int value, List<string> imagePaths) : base(name, value)
        {
            _imagePaths = imagePaths;
        }

        public string GetRandomImagePath()
        {
            var random = new Random();
            return _imagePaths[random.Next(_imagePaths.Count)];
        }
    }
}