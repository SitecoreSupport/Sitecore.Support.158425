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
    public static Item GetProfileItemFixed(this ContentProfile contentProfile)
    {
      Condition.Requires<ContentProfile>(contentProfile, "contentProfile").IsNotNull<ContentProfile>();
      //The fix: Get profile item from Context.Site.ContentDatabase instead of Context.ContentDatabase
      return Context.Site.ContentDatabase.GetItem(contentProfile.Definition.Id.ToID());
    }
  }
}
