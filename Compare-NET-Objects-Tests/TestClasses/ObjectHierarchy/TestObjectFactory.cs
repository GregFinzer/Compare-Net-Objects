using System;
using System.Collections.Generic;

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses.ObjectHierarchy
{
    internal class TestObjectHierarchyFactory
    {
        internal static HoldingsReport CreateHoldingsReport(int id, DateTime generatedAt, params Holding[] holdings)
        {
            return new HoldingsReport
            {
                Id = id,
                GeneratedAt = generatedAt,
                Holdings = new List<Holding>(holdings)
            };
        }

        internal static Identifier CreateIdentifier(int id, string type, string code, string source)
        {
            return new Identifier
            {
                Id = id,
                Type = type,
                Code = code,
                Source = source
            };
        }

        internal static Attribute CreateAttribtue(int id, string name, string value, string source)
        {
            return new Attribute
            {
                Id = id,
                Name = name,
                Value = value,
                Source = source
            };
        }

        internal static Bond CreateBond(int id, IEnumerable<Identifier> identifiers, IEnumerable<Attribute> attributes,
            int issuenceCount, DateTime maturity)
        {
            return new Bond
            {
                Id = id,
                Name = $"Bond {id}",
                Description = $"Bond {id} Description",
                Identifiers = new List<Identifier>(identifiers ?? new Identifier[] { }),
                Attributes = new List<Attribute>(attributes ?? new Attribute[] { }),
                IssueSnPRating = $"Bond {id} Issuence Rating",
                IssuenceCount = issuenceCount,
                Maturity = maturity
            };
        }

        internal static FundShare CreateFundShare(int id, IEnumerable<Identifier> identifiers, IEnumerable<Attribute> attributes,
            int shareCount, decimal netAssetValue)
        {
            return new FundShare
            {
                Id = id,
                Name = $"FundShare {id}",
                Description = $"FundShare {id} Description",
                Identifiers = new List<Identifier>(identifiers ?? new Identifier[] { }),
                Attributes = new List<Attribute>(attributes ?? new Attribute[] { }),
                FundLegalName = $"Fund Share {id} Legal Name",
                ShareCount = shareCount,
                NetAssetValue = netAssetValue
            };
        }

        internal static HoldingsReport CreateHoldingsReport(int id)
        {
            var startId = id;
            var holdings = new Holding[] {
                CreateBond(++id, GenerateIdentifiers(ref id, "Bond"), GenerateAttributes(ref id, 3), (id*11), DateTime.Today.AddYears(10*(id%10))),
                CreateBond(++id, GenerateIdentifiers(ref id, "Bond"), GenerateAttributes(ref id, 3), (id*11), DateTime.Today.AddYears(10*(id%10))),
                CreateFundShare(++id, GenerateIdentifiers(ref id, "FundShare"), GenerateAttributes(ref id, 8), (id*id), (decimal) (id*(1/Math.Tan(id)))),
                CreateFundShare(++id, GenerateIdentifiers(ref id, "FundShare"), GenerateAttributes(ref id, 8), (id*id), (decimal) (id*(1/Math.Tan(id)))),
                CreateFundShare(++id, GenerateIdentifiers(ref id, "FundShare"), GenerateAttributes(ref id, 8), (id*id), (decimal) (id*(1/Math.Tan(id)))),
            };
            var holdingsReport = CreateHoldingsReport(id: startId, generatedAt: DateTime.Today, holdings: holdings);
            return holdingsReport;
        }

        internal static Attribute[] GenerateAttributes(ref int id, int type)
        {
            return new Attribute[]
            {
                CreateAttribtue(++id, $"Attribute ${id%type}", ((id * (id%type)/type)+Math.Tan(-id*type)).ToString("g"), "Reuters"),
                CreateAttribtue(++id, $"Attribute ${id%type}", ((id * (id%type)/type)+Math.Tan(+id*type)).ToString("g"), "Bloomberg")
            };
        }

        internal static Identifier[] GenerateIdentifiers(ref int id, string type)
        {
            return new Identifier[]
            {
                CreateIdentifier(++id, "FTSE", $"{type}{id}", $"{type}{id} Source"),
                CreateIdentifier(++id, "Bloomberg", $"{type}{id}", $"{type}{id} Source")
            };
        }
    }
}
