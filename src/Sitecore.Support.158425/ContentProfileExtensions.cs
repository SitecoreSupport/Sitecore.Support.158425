namespace Sitecore.Support.Analytics.Data
{
  using Sitecore.Common;
  using Sitecore.Data.Items;
  using Sitecore.Framework.Conditions;
  using Sitecore.Marketing.Definitions.Profiles;
  using Sitecore.Xdb.Configuration;
  using Sitecore.Analytics.Data;
  public static class ContentProfileExtensions
  {
    public static Item GetProfileCardItem(this ContentProfile contentProfile, string alias)
    {
      Condition.Requires<ContentProfile>(contentProfile, "contentProfile").IsNotNull<ContentProfile>();
      Condition.Requires<string>(alias, "alias").IsNotNull<string>();
      IProfileCardDefinition preset = contentProfile.GetPreset(alias);
      if (preset != null)
      {
        return DefinitionDatabase.Database.GetItem(preset.Id.ToID());
      }
      return null;
    }

    public static Item GetProfileItem(this ContentProfile contentProfile)
    {
      Condition.Requires<ContentProfile>(contentProfile, "contentProfile").IsNotNull<ContentProfile>();
      return DefinitionDatabase.Database.GetItem(contentProfile.Definition.Id.ToID());
    }
  }
}
