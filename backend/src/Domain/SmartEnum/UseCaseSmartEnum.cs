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
        public static readonly UseCaseSmartEnum HomeView = new("HomeView", 7);
        public static readonly UseCaseSmartEnum UserProfileView = new("UserProfileView", 8);
        public static readonly UseCaseSmartEnum MyReviewView = new("MyReviewView", 9);
        public static readonly UseCaseSmartEnum SoftDelete = new("SoftDelete", 10);
        public static readonly UseCaseSmartEnum Delete = new("Delete", 11);
        public static readonly UseCaseSmartEnum AllLocations = new("AllLocations", 12); 
        public static readonly UseCaseSmartEnum CreateBoth = new ("CreateBoth", 13);
        public static readonly UseCaseSmartEnum CreateActivity = new ("CreateActivity", 14);
        public static readonly UseCaseSmartEnum CreateActivityDate = new ("CreateActivityDate", 15);

        private UseCaseSmartEnum(string name, int value) : base(name, value)
        {
        }
    }
}