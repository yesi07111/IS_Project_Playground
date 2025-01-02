using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;

namespace Playground.Domain.SmartEnum
{
    public class FacilityImageSmartEnum : SmartEnum<FacilityImageSmartEnum>
    {
        public static readonly FacilityImageSmartEnum Taller = new("Taller", 1,
        [
            "/images/activities/art-workshop-1.jpg",
            "/images/activities/art-workshop-2.jpg",
            "/images/activities/art-workshop-3.jpg"
        ]);

        public static readonly FacilityImageSmartEnum Sala = new("Sala", 2,
        [
            "/images/activities/kids-guitar.jpg",
            "/images/activities/kids-music.jpg"
        ]);

        public static readonly FacilityImageSmartEnum Laboratorio = new("Laboratorio", 3,
        [
            "/images/activities/kid-science-1.jpg",
            "/images/activities/kid-science-2.jpg",
            "/images/activities/kid-science-3.jpg"
        ]);

        public static readonly FacilityImageSmartEnum Piscina = new("Piscina", 4,
        [
            "/images/activities/pool-1.jpg",
            "/images/activities/pool-2.jpg",
            "/images/activities/pool-3.jpg"

        ]);

        public static readonly FacilityImageSmartEnum Estudio = new("Estudio", 5,
        [
            "/images/activities/dance-1.jpg",
            "/images/activities/dance-2.jpg",
            "/images/activities/dance-3.jpg"
        ]);

        public static readonly FacilityImageSmartEnum Atraccion = new("Atracción", 6,
        [
            "/images/activities/park-1.jpg",
            "/images/activities/park-2.jpg",
            "/images/activities/park-3.jpg"
        ]);

        public static readonly FacilityImageSmartEnum Biblioteca = new("Biblioteca", 7,
        [
            "/images/activities/library-1.jpg",
            "/images/activities/library-2.jpg",
            "/images/activities/library-3.jpg"
        ]);

        public static readonly FacilityImageSmartEnum Gimnasio = new("Gimnasio", 8,
        [
            "/images/activities/gym-1.jpg",
            "/images/activities/gym-2.jpg",
            "/images/activities/gym-3.jpg"
        ]);

        public static readonly FacilityImageSmartEnum Cafeteria = new("Cafetería", 9,
        [
            "/images/activities/cafe-1.jpg",
            "/images/activities/cafe-2.jpg",
            "/images/activities/cafe-3.jpg"
        ]);

        public static readonly FacilityImageSmartEnum Zoologico = new("Zoológico", 10,
        [
            "/images/activities/zoo-1.jpg",
            "/images/activities/zoo-2.jpg",
            "/images/activities/zoo-3.jpg"
        ]);

        public static readonly FacilityImageSmartEnum JuegosAcuaticos = new("Juegos Acuáticos", 11,
        [
            "/images/activities/waterplay-1.jpg",
            "/images/activities/waterplay-2.jpg",
            "/images/activities/waterplay-3.jpg"
        ]);

        public static readonly FacilityImageSmartEnum Parque = new("Parque", 12,
        [
            "/images/activities/park-1-2.jpg",
            "/images/activities/park-2-2.jpg",
            "/images/activities/park-3-2.jpg"
        ]);

        // Default image paths
        private static readonly List<string> DefaultImagePaths =
        [
            "/images/activities/default-1.jpg",
            "/images/activities/default-2.jpg",
            "/images/activities/default-3.jpg",
            "/images/activities/default-4.jpg",
            "/images/activities/default-5.jpg",
            "/images/activities/default-6.jpg",
            "/images/activities/default-7.jpg",
            "/images/activities/default-8.jpg",
            "/images/activities/default-9.jpg",
            "/images/activities/default-10.jpg",
            "/images/activities/default-11.jpg",
        ];

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

        public static string GetRandomImagePathOrDefault(string facilityType)
        {
            try
            {
                var facilityImageEnum = FromName(facilityType, ignoreCase: true);
                return facilityImageEnum.GetRandomImagePath();
            }
            catch (ArgumentException)
            {
                // Return a random image from the default set if no match is found
                var random = new Random();
                return DefaultImagePaths[random.Next(DefaultImagePaths.Count)];
            }
        }
    }
}