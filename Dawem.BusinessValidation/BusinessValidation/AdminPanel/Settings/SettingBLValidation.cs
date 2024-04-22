using Dawem.Contract.BusinessValidation.AdminPanel.Subscriptions;
using Dawem.Contract.BusinessValidationCore.AdminPanel.Subscriptions;
using Dawem.Contract.Repository.Manager;
using Dawem.Domain.Entities.Dawem;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Core.Responsibilities;
using Dawem.Models.DTOs.Dawem.Generic.Exceptions;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;


namespace Dawem.Validation.BusinessValidation.AdminPanel.Subscriptions
{

    public class SettingBLValidation : ISettingBLValidation
    {
        private readonly IRepositoryManager repositoryManager;
        private readonly RequestInfo requestInfo;
        private readonly INameTranslationBLValidationCore nameTranslationBLValidationCore;
        public SettingBLValidation(IRepositoryManager _repositoryManager,
            RequestInfo _requestInfo,
            INameTranslationBLValidationCore _nameTranslationBLValidationCore)
        {
            repositoryManager = _repositoryManager;
            requestInfo = _requestInfo;
            nameTranslationBLValidationCore = _nameTranslationBLValidationCore;
        }
        public async Task<List<Setting>> UpdateValidation(UpdateSettingModel model)
        {
            int? companyId = requestInfo.Type == AuthenticationType.DawemAdmin ?
                requestInfo.CompanyId : null;

            var getSettings = await repositoryManager
                .SettingRepository.
                GetWithTracking(c => !c.IsDeleted && c.Type == requestInfo.Type && c.CompanyId == companyId).
                ToListAsync();

            #region Validate Count And Type

            if (getSettings.Count != model.Settings.Count)
            {
                throw new BusinessValidationException(LeillaKeys.SorrySettingsCountNotCorrect);
            }
            if (getSettings.Any(s1 => !model.Settings.Any(s2 => s1.SettingType == s2.SettingType)))
            {
                throw new BusinessValidationException(LeillaKeys.SorryYouMustEnterCorrectSettingType);
            }

            #endregion

            foreach (var setting in getSettings)
            {
                var enumTypeName = requestInfo.Type == AuthenticationType.AdminPanel ?
                    nameof(AdminPanelSettingType) : nameof(DawemSettingType);

                var settingName = EnumHelper.GetSettingName(setting.SettingType, requestInfo.Type);

                var settingTypeName = TranslationHelper.
                    GetTranslation(enumTypeName + LeillaKeys.Dash + settingName, requestInfo.Lang);

                var getModelSetting = model.Settings.
                    FirstOrDefault(ms => ms.SettingType == setting.SettingType);

                #region Validate Value

                if (getModelSetting.Value is null)
                {
                    var message = TranslationHelper.GetTranslation(LeillaKeys.SorryYouMustEnterTheValueForTheSetting, requestInfo.Lang);
                    throw new BusinessValidationException(messageCode: null, message +
                        LeillaKeys.SpaceThenDashThenSpace + LeillaKeys.SettingName + settingTypeName + LeillaKeys.Dot);
                }

                #endregion

                if (getModelSetting != null)
                {
                    var valueString = getModelSetting.Value.ToString();
                    var type = getModelSetting.Value.GetType();

                    switch (setting.ValueType)
                    {
                        case SettingValueType.String:
                            if (type.Name != "String")
                            {
                                var message = TranslationHelper.GetTranslation(LeillaKeys.SorryYouMustEnterCorrectTextValueForTheSetting, requestInfo.Lang);
                                throw new BusinessValidationException(messageCode: null, message +
                                    LeillaKeys.SpaceThenDashThenSpace + TranslationHelper.GetTranslation(LeillaKeys.SettingName, requestInfo.Lang) + settingTypeName + LeillaKeys.Dot);
                            }
                            break;
                        case SettingValueType.Integer:
                            if (type.Name != "Int64")
                            {
                                var message = TranslationHelper.GetTranslation(LeillaKeys.SorryYouMustEnterCorrectNumberValueForTheSetting, requestInfo.Lang);
                                throw new BusinessValidationException(messageCode: null, message +
                                    LeillaKeys.SpaceThenDashThenSpace + TranslationHelper.GetTranslation(LeillaKeys.SettingName, requestInfo.Lang) + settingTypeName + LeillaKeys.Dot);
                            }
                            break;
                        case SettingValueType.Decimal:
                            if (!decimal.TryParse(valueString, out decimal val))
                            {
                                var message = TranslationHelper.GetTranslation(LeillaKeys.SorryYouMustEnterCorrectNumberValueForTheSetting, requestInfo.Lang);
                                throw new BusinessValidationException(messageCode: null, message +
                                    LeillaKeys.SpaceThenDashThenSpace + TranslationHelper.GetTranslation(LeillaKeys.SettingName, requestInfo.Lang) + settingTypeName + LeillaKeys.Dot);
                            }
                            break;
                        case SettingValueType.Boolean:
                            if (type.Name != "Boolean")
                            {
                                var message = TranslationHelper.GetTranslation(LeillaKeys.SorryYouMustEnterCorrectBooleanValueForTheSetting, requestInfo.Lang);
                                throw new BusinessValidationException(messageCode: null, message +
                                    LeillaKeys.SpaceThenDashThenSpace + TranslationHelper.GetTranslation(LeillaKeys.SettingName, requestInfo.Lang) + settingTypeName + LeillaKeys.Dot);
                            }
                            break;
                        default:
                            break;
                    }
                }

                #region Validate Percentage

                if (settingName.Contains(LeillaKeys.Percentage))
                {
                    int value = (int)getModelSetting.Value;
                    if (value <= 0 || value > 100)
                    {
                        throw new BusinessValidationException(LeillaKeys.SorryYouMustEnterCorrectValueForPercentageFrom1To100);
                    }
                }

                #endregion
            }

            return getSettings;
        }
    }
}
