using Dawem.Enums.Generals;
using Dawem.Enums.Permissions;

namespace Dawem.Helpers
{
    public class EnumHelper
    {
        public static string GetScreenName(int screenCode, AuthenticationType type)
        {
            dynamic screenCodeEnum = type == AuthenticationType.AdminPanel ?
                    (AdminPanelApplicationScreenCode)screenCode :
                    (DawemAdminApplicationScreenCode)screenCode;
            return screenCodeEnum.ToString();
        }
        public static string GetSettingName(int settingType, AuthenticationType type)
        {
            dynamic settingTypeEnum = type == AuthenticationType.AdminPanel ?
                    (AdminPanelSettingType)settingType :
                    (DawemSettingType)settingType;
            return settingTypeEnum.ToString();
        }
        public static string GetSettingGroupName(int settingGroupType, AuthenticationType type)
        {
            dynamic settingGroupTypeEnum = type == AuthenticationType.AdminPanel ?
                    (AdminPanelSettingGroupType)settingGroupType :
                    (DawemSettingGroupType)settingGroupType;
            return settingGroupTypeEnum.ToString();
        }
    }
}
