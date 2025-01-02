using Ardalis.SmartEnum;

namespace Playground.Domain.SmartEnum
{
    /// <summary>
    /// Representa los diferentes casos de uso para los endpoints.
    /// </summary>
    public class UseCaseSmartEnum : SmartEnum<UseCaseSmartEnum>
    {
        public static readonly UseCaseSmartEnum BaseEntity = new("BaseEntity", 1);
        public static readonly UseCaseSmartEnum AllTypes = new("AllTypes", 2);
        public static readonly UseCaseSmartEnum ActivityView = new("ActivityView", 3);
        public static readonly UseCaseSmartEnum ReviewView = new("ReviewView", 4);
        public static readonly UseCaseSmartEnum AdminEducatorView = new("AdminEducatorView", 5);
        public static readonly UseCaseSmartEnum AsFilter = new("AsFilter", 6);
        public static readonly UseCaseSmartEnum HomePageView = new("HomePageView", 7);


        private UseCaseSmartEnum(string name, int value) : base(name, value)
        {
        }
    }
}