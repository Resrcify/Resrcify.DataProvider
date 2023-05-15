using System;
using System.Collections.Generic;
using Titan.DataProvider.Domain.Primitives;
using Titan.DataProvider.Domain.Shared;

namespace Titan.DataProvider.Domain.Internal.BaseData.ValueObjects
{
    public sealed class Skill : ValueObject
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string NameKey { get; private set; }
        public long MaxTier { get; private set; }
        public long Type { get; private set; }
        public string Image { get; private set; }
        public Dictionary<string, string> PowerOverrideTags { get; private set; }
        public bool IsZeta { get; private set; }
        public bool IsOmicron { get; private set; }
        private Skill(string id, string name, string nameKey, int maxTier, long type, string image, Dictionary<string, string> powerOverrideTags, bool isZeta, bool isOmicron)
        {
            Id = id;
            Name = name;
            NameKey = nameKey;
            MaxTier = maxTier;
            Type = type;
            Image = image;
            PowerOverrideTags = powerOverrideTags;
            IsZeta = isZeta;
            IsOmicron = isOmicron;
        }
        public static Result<Skill> Create(string id, string name, string nameKey, int maxTier, long type, string image, Dictionary<string, string> powerOverrideTags, bool isZeta, bool isOmicron)
        {
            return new Skill(id, name, nameKey, maxTier, type, image, powerOverrideTags, isZeta, isOmicron);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return Name;
            yield return NameKey;
            yield return MaxTier;
            yield return Type;
            yield return Image;
            yield return PowerOverrideTags;
            yield return IsZeta;
            yield return IsOmicron;
        }
    }
}