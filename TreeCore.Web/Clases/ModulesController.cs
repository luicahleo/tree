using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using TreeCore.Shared.DTO.General;

namespace TreeCore.Clases
{

    class Module
    {
        private List<UserInterface> _userInterfaces;
        public string Code { get; set; }
        public string Name { get; set; }
        public string Route { get; set; }
        public List<UserInterface> UserInterfaces
        {
            get
            {
                return _userInterfaces ?? new List<UserInterface>();
            }

            set
            {
                _userInterfaces = value;
            }
        }

    }

    class UserInterface
    {
        public List<UserFunctionality> UserFunctionalities { get; set; }
        public List<string> Resource { get; set; }
        public string Code { get; set; }
        public string Page { get; set; }
        public string Icon { get; set; }
    }

    class UserFunctionality
    {
        public List<string> APIFunctionalities { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
    }

    class UserFuntionalityType
    {
        public List<string> Resource { get; set; }
        public string Code { get; set; }
    }

    static class ModulesController
    {
        public static List<Module> GetModules() => Global.Configuration.GetSection($"Modules").Get<List<Module>>();

        public static List<UserFuntionalityType> GetUserFuntionalityTypes() => Global.Configuration.GetSection($"UserFuntionalityTypes").Get<List<UserFuntionalityType>>();

        public static Module GetModuleByName(string sModule) => GetModules().FirstOrDefault(x => x.Name == sModule);

        public static Module GetModuleByCode(string sModule) => GetModules().FirstOrDefault(x => x.Code == sModule);

        public static List<UserInterface> GetUserInterfaces() => GetModules().SelectMany(x => x.UserInterfaces).ToList();

        public static List<UserFunctionality> GetUserFunctionalities() => GetUserInterfaces().SelectMany(x => x.UserFunctionalities).ToList();

        public static List<UserInterface> GetUserInterfacesFromModuleName(string sModule) => GetModuleByName(sModule).UserInterfaces;

        public static List<UserInterface> GetUserInterfacesFromModuleCode(string sModule) => GetModuleByCode(sModule).UserInterfaces;

        public static List<UserFunctionality> GetUserFunctionalitiesFromUserInterface(string sCode) => GetUserInterfaces().FirstOrDefault(x => x.Code == sCode).UserFunctionalities;

        public static List<UserFunctionality> GetUserFunctionalitiesFromProfile(ProfileDTO profileDTO) => GetUserFunctionalities().Where(x => profileDTO.UserFuntionalities.Contains(x.Code)).ToList();

        public static List<UserFunctionality> GetUserFunctionalitiesFromModuleCode(string sModule) => GetUserInterfacesFromModuleCode(sModule).SelectMany(x => x.UserFunctionalities).ToList();

        public static List<UserInterface> GetUserInterfacesFromProfile(ProfileDTO profileDTO) => GetUserInterfaces().Where(x => GetUserFunctionalitiesFromProfile(profileDTO).Select(c => c.Code.Split('@')[0]).Contains(x.Code)).ToList();

    }
}
