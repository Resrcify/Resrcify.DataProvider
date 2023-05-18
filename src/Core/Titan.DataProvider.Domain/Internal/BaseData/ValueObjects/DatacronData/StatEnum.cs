using System.Collections.Generic;
using Titan.DataProvider.Domain.Primitives;
using Titan.DataProvider.Domain.Shared;

namespace Titan.DataProvider.Domain.Internal.BaseData.ValueObjects
{
    public sealed class StatEnum : ValueObject
    {
        public int Id { get; private set; }
        public string StatId { get; private set; }
        public string LangId { get; private set; }
        public string NameKey { get; private set; }
        private StatEnum(int id, string statId, string langId, string nameKey)
        {
            Id = id;
            StatId = statId;
            LangId = langId;
            NameKey = nameKey;

        }
        public static Result<StatEnum> Create(int id, string statId, string langId, string nameKey)
        {
            return new StatEnum(id, statId, langId, nameKey);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return StatId;
            yield return LangId;
            yield return NameKey;
        }
    }
}