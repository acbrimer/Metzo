using System.Text.Json;
using System.Text.RegularExpressions;

namespace Metzo.Core.Filters;
public class Filter : Dictionary<string, FilterGroup>
{
    private static readonly string PREDICATE_OPERATOR_DELIMITER = "--";
    private static readonly Regex PREDICATE_OPERATOR_REGEX = new Regex($@"{PREDICATE_OPERATOR_DELIMITER}({string.Join("|", Enum.GetNames(typeof(FilterOperator)))})$");
    public string SourceJson { get; set; }

    public Filter(string json)
    {
        var jsonDoc = JsonDocument.Parse(json);
        var root = jsonDoc.RootElement;

        // Handle root properties that are not inside of `@[filterGroupKey]: {...}`
        var defaultFilterGroupItems = root.EnumerateObject()
            .Where(p => !p.Name.StartsWith("@"))
            .Select(p => ParseFilterItem(p.Name, p.Value)).ToArray();
        if (defaultFilterGroupItems.Count() > 0)
        {
            // Unnest single top-level and/or filter group
            if (defaultFilterGroupItems.Count() == 1 && defaultFilterGroupItems[0].GetType() == typeof(FilterGroup) && (FilterGroup)defaultFilterGroupItems[0] is not null)
            {
                this.Add(".", (FilterGroup)defaultFilterGroupItems[0]);
            }
            else
            {
                this.Add(".", new FilterGroup(FilterGroupOperator.AND, defaultFilterGroupItems.ToArray()));
            }
        }
        // Handle `@[filterGroupKey]: {...}`
        foreach (var property in root.EnumerateObject()
            .Where(p => p.Name.StartsWith("@") && (p.Value.ValueKind == JsonValueKind.Object)))
        {
            var filterGroupKey = property.Name.Substring(1); // Remove "@" from start
            if (!this.ContainsKey(filterGroupKey))
            {
                var nestedFilterGroupItems = property.Value.EnumerateObject()
                    .Select(p => ParseFilterItem(p.Name, p.Value)).ToArray();
                if (nestedFilterGroupItems.Count() > 0)
                {
                    // Unnest single top-level and/or filter group
                    if (nestedFilterGroupItems.Count() == 1 && nestedFilterGroupItems[0].GetType() == typeof(FilterGroup) && (FilterGroup)nestedFilterGroupItems[0] is not null)
                    {
                        this.Add(filterGroupKey, (FilterGroup)nestedFilterGroupItems[0]);
                    }
                    else
                    {
                        this.Add(filterGroupKey, new FilterGroup(FilterGroupOperator.AND, nestedFilterGroupItems.ToArray()));
                    }
                }
            }

        }
    }

    private static IFilterItem? ParseFilterItem(string name, JsonElement value)
    {
        if (name.ToLower() == "$and" || name.ToLower() == "$or")
        {
            return ParseFilterGroup(value, name == "$and" ? FilterGroupOperator.AND : FilterGroupOperator.OR);
        }
        try
        {
            var val = ParseFilterValue(value);
            (string field, FilterOperator op) = ParseFilterKey(name);
            return new FilterPredicate(field, op, val);
        }
        catch (Exception ex)
        {
            // TODO: log error or ignore?
            return null;
        }

    }
    private static FilterGroup? ParseFilterGroup(JsonElement element, FilterGroupOperator operatorStr)
    {
        if (element.ValueKind == JsonValueKind.Array && element.EnumerateArray()
                    .Where(a => a.ValueKind == JsonValueKind.Object).Count() > 0)
        {
            var arrayFilterItems = element.EnumerateArray()
                    .Where(a => a.ValueKind == JsonValueKind.Object)
                    .Select(a => a.EnumerateObject().FirstOrDefault())
                    .Select(o => ParseFilterItem(o.Name, o.Value));
            if (arrayFilterItems is null || arrayFilterItems.Count() == 0)
                return null;
            return new FilterGroup(operatorStr, arrayFilterItems.ToArray());
        }
        if (element.ValueKind == JsonValueKind.Object && element.EnumerateObject().Count() > 0)
        {
            var objectFilterItems = element.EnumerateObject()
                    .Select(o => ParseFilterItem(o.Name, o.Value));
            if (objectFilterItems is null || objectFilterItems.Count() == 0)
                return null;
            return new FilterGroup(operatorStr, objectFilterItems.ToArray());
        }
        return null;
    }
    public static (string field, FilterOperator op) ParseFilterKey(string name)
    {
        var match = PREDICATE_OPERATOR_REGEX.Match(name);
        if (match.Success)
        {
            var field = name.Substring(0, match.Index);
            var op = Enum.Parse<FilterOperator>(match.Groups[1].Value);
            return (field, op);
        }
        // If no operator suffix is found, default to eq
        return (name, FilterOperator.eq);
    }
    private static object? ParseFilterValue(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.String => element.GetString(),
            JsonValueKind.Number when element.TryGetInt32(out int i) => i,
            JsonValueKind.Number when element.TryGetInt64(out long l) => l,
            JsonValueKind.Number when element.TryGetDecimal(out decimal d) => d,
            JsonValueKind.Number when element.TryGetDouble(out double dd) => dd,
            JsonValueKind.Number => element.GetDouble(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => null,
            _ => throw new ArgumentException($"Invalid value kind: {element.ValueKind}")
        };
    }

}


