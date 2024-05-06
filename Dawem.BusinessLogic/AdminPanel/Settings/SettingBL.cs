using AutoMapper;
using Dawem.Contract.BusinessLogic.AdminPanel.Settings;
using Dawem.Contract.BusinessLogicCore.Dawem;
using Dawem.Contract.BusinessValidation.AdminPanel.Settings;
using Dawem.Contract.Repository.Manager;
using Dawem.Data;
using Dawem.Data.UnitOfWork;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Dawem.Attendances;
using Dawem.Models.Dtos.Dawem.Settings;
using Dawem.Models.Response.AdminPanel.Settings;
using Dawem.Translations;
using Microsoft.EntityFrameworkCore;

namespace Dawem.BusinessLogic.AdminPanel.Settings
{
    public class SettingBL : ISettingBL
    {
        private readonly IUnitOfWork<ApplicationDBContext> unitOfWork;
        private readonly RequestInfo requestInfo;
        private readonly ISettingBLValidation settingBLValidation;
        private readonly IRepositoryManager repositoryManager;
        private readonly IMapper mapper;
        private readonly IUploadBLC uploadBLC;

        public SettingBL(IUnitOfWork<ApplicationDBContext> _unitOfWork,
            IRepositoryManager _repositoryManager,
            IMapper _mapper,
           RequestInfo _requestHeaderContext,
           IUploadBLC _uploadBLC,
           ISettingBLValidation _settingBLValidation)
        {
            unitOfWork = _unitOfWork;
            requestInfo = _requestHeaderContext;
            repositoryManager = _repositoryManager;
            settingBLValidation = _settingBLValidation;
            mapper = _mapper;
            uploadBLC = _uploadBLC;

        }
        public async Task<bool> Update(UpdateSettingModel model)
        {
            #region Business Validation

            var settings = await settingBLValidation.UpdateValidation(model);

            #endregion

            unitOfWork.CreateTransaction();

            #region Update Setting

            var modelSettings = model.Settings;

            foreach (var setting in settings)
            {
                var getModelSetting = model.Settings.
                    FirstOrDefault(ms => ms.SettingType == setting.SettingType);

                switch (setting.ValueType)
                {
                    case SettingValueType.String:
                        setting.String = (string)getModelSetting.Value;
                        break;
                    case SettingValueType.Integer:
                        setting.Integer = (int)getModelSetting.Value;
                        break;
                    case SettingValueType.Decimal:
                        setting.Decimal = (decimal)getModelSetting.Value;
                        break;
                    case SettingValueType.Boolean:
                        setting.Bool = (bool)getModelSetting.Value;
                        break;
                    default:
                        break;
                }
            }

            await unitOfWork.SaveAsync();

            #endregion

            #region Handle Response

            await unitOfWork.CommitAsync();
            return true;

            #endregion
        }
        public async Task<GetSettingsResponse> Get()
        {
            var settingRepository = repositoryManager.SettingRepository;

            int? companyId = requestInfo.Type == AuthenticationType.DawemAdmin ?
                requestInfo.CompanyId : null;

            var settingGroupTypeName = requestInfo.Type == AuthenticationType.AdminPanel ?
                nameof(AdminPanelSettingGroupType) : nameof(DawemSettingGroupType);

            var settingTypeName = requestInfo.Type == AuthenticationType.AdminPanel ?
                nameof(AdminPanelSettingType) : nameof(DawemSettingType);

            #region Handle Response

            var settingsList = await repositoryManager
                .SettingRepository.
                Get(c => !c.IsDeleted && c.Type == requestInfo.Type && c.CompanyId == companyId).
                GroupBy(c => c.GroupType).
                Select(settingGroup => new GetSettingGroupModel
                {
                    GroupType = settingGroup.Key,
                    GroupTypeName = TranslationHelper.GetTranslation(settingGroupTypeName + LeillaKeys.Dash + EnumHelper.GetSettingGroupName(settingGroup.Key, requestInfo.Type), requestInfo.Lang),
                    Settings = settingGroup.Select(setting => new GetSettingModel
                    {
                        Id = setting.Id,
                        SettingType = setting.SettingType,
                        SettingTypeName = TranslationHelper.GetTranslation(settingTypeName + LeillaKeys.Dash + EnumHelper.GetSettingName(setting.SettingType, requestInfo.Type), requestInfo.Lang),
                        ValueType = setting.ValueType,
                        ValueTypeName = TranslationHelper.GetTranslation(setting.ValueTypeName, requestInfo.Lang),
                        Value = setting.ValueType == SettingValueType.Integer ? setting.Integer :
                        setting.ValueType == SettingValueType.Decimal ? setting.Decimal :
                        setting.ValueType == SettingValueType.String ? setting.String :
                        setting.ValueType == SettingValueType.Boolean ? setting.Bool : null,
                    }).ToList()
                }).ToListAsync();

            return new GetSettingsResponse
            {
                SettingsGroups = settingsList
            };

            #endregion

        }
    }
}

