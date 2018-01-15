﻿using Abp.AutoMapper;
using MyCompanyName.AbpZeroTemplate.Authorization.Roles.Dto;
using MyCompanyName.AbpZeroTemplate.Web.Areas.Mpa.Models.Common;

namespace MyCompanyName.AbpZeroTemplate.Web.Areas.Mpa.Models.Roles
{
    [AutoMapFrom(typeof(GetRoleForEditOutput))]
    public class CreateOrEditRoleModalViewModel : GetRoleForEditOutput, IPermissionsEditViewModel
    {
        public bool IsEditMode
        {
            get { return Role.Id.HasValue; }
        }

        public CreateOrEditRoleModalViewModel(GetRoleForEditOutput output)
        {
            output.MapTo(this);
        }
    }
}