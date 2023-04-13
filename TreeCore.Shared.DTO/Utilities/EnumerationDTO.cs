using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TreeCore.Shared.DTO.Utilities
{
    public abstract class EnumerationDTO : BaseDTO, IComparable
    {
        public string Name { get; private set; }

        protected EnumerationDTO(string name) => (Name) = (name);

        public override string ToString() => Name;

        public static IEnumerable<T> GetAll<T>() where T : EnumerationDTO =>
            typeof(T).GetFields(BindingFlags.Static)
                     .Select(f => f.GetValue(null))
                     .Cast<T>();

        public static T GetByCode<T>(string name) where T : EnumerationDTO =>
            typeof(T).GetFields().ToList()
                     .Where(f => f.Name == name)
                     .Select(f => f.GetValue(null))
                     .Cast<T>().FirstOrDefault();

        public override bool Equals(object obj)
        {
            var otherValue = obj as EnumerationDTO;

            if (otherValue == null)
            {
                return false;
            }

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Name.Equals(otherValue.Name);

            return typeMatches && valueMatches;
        }

        public int CompareTo(object other) => Name.CompareTo(((EnumerationDTO)other).Name);
    }
}
