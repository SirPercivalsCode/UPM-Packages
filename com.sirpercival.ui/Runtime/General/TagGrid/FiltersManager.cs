using SharedLibrary.Dtos.Campaigns;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class FiltersManager : MonoBehaviour
{
    [SerializeField] private TagManager includeTagsManager;
    [SerializeField] private TagManager excludeTagsManager;
    [SerializeField] private TMP_InputField searchbarIF;

    [SerializeField] private SortingOptions sortingOptions = new SortingOptions { Property = SortingProperties.None, Ascending = true };

    public List<CampaignListItemDto> ApplyFilters(List<CampaignListItemDto> campaigns)
    {
        // Filters
        string search = searchbarIF.text.ToLower();

        List<string> includedTags = includeTagsManager.GetTagsAsStrings();
        List<string> excludedTags = excludeTagsManager.GetTagsAsStrings();
        Helpers.LogStringList(includedTags, "Included Tags");
        Helpers.LogStringList(excludedTags, "Excluded Tags");

        // Sorting
        // makes no sense yet, aren't any details to sort by

        // No filters applied, return original list
        if (string.IsNullOrWhiteSpace(search)
            && includedTags.Count == 0
            && excludedTags.Count == 0
            && sortingOptions.Property == SortingProperties.None) return campaigns;

        List<CampaignListItemDto> filtered = campaigns.Where(campaign =>
        {
            bool includesSearch = campaign.CampaignName.ToLower().Contains(search) || campaign.CampaignSubtitle.ToLower().Contains(search);
            bool includesAll = !includedTags.Any() || includedTags.All(tag => campaign.CampaignTags.Contains(tag));     // Must contain all included tags
            bool excludesAll = !excludedTags.Any() || excludedTags.All(tag => !campaign.CampaignTags.Contains(tag));    // Must not contain any excluded tags

            return includesSearch && includesAll && excludesAll;
        }).ToList();

        return filtered;
    }
}

public struct SortingOptions
{
    public SortingProperties Property;
    public bool Ascending;

    public SortingOptions(SortingProperties property, bool ascending)
    {
        Property = property;
        Ascending = ascending;
    }
}

public enum SortingProperties
{
    None,
    New,
    Likes,
    Popularity
}