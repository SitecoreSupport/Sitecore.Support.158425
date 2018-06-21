namespace Sitecore.ContentTesting.Speak.Ribbon.Panels.ProfileCardsPanel
{
  using Sitecore.Analytics.Data;
  using Sitecore.ContentTesting.Diagnostics;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.ExperienceEditor.Speak;
  using Sitecore.ExperienceEditor.Speak.Caches;
  using Sitecore.ExperienceEditor.Speak.Ribbon;
  using Sitecore.ExperienceEditor.Utils;
  using Sitecore.Globalization;
  using Sitecore.Mvc.Presentation;
  using System;
  using System.Collections.Generic;
  using System.Diagnostics.CodeAnalysis;
  using System.Linq;
  using System.Web.UI;

  public class ProfileCardsPanel : RibbonComponentControlBase
  {
    public ProfileCardsPanel() : this(false)
    {
    }

    public ProfileCardsPanel(RenderingParametersResolver parametersResolver) : this(parametersResolver.GetBool("IsInIframeRendering", false))
    {
      Assert.IsNotNull(parametersResolver, "Parameters resolver");
    }

    public ProfileCardsPanel(bool isInIframeRendering)
    {
      this.IsInIframeRendering = isInIframeRendering;
      ResourcesCache.RequireCss(this, "ribbon", "ProfileCardsPanel.css");
      ResourcesCache.RequireJs(this, "ribbon", "ProfileCardsPanel.js");
      base.Class = "sc-profilecards-panel";
      base.DataBind = "visible: isVisible";
      base.Attributes["data-sc-isiniframerendering"] = isInIframeRendering.ToString().ToLowerInvariant();
    }

    private static string GetOpenEditProfileCardsScript() =>
        "javascript:ProfileCardsPanel.editProfileCards();";

    private static string GetOpenProfileCardInlineCode(Item profileItem) =>
        $"javascript: ProfileCardsPanel.selectProfileCard('{profileItem.ID}');";

    protected override void Render(HtmlTextWriter output)
    {
      base.Render(output);
      this.AddAttributes(output);
      output.AddAttribute(HtmlTextWriterAttribute.Id, "profileCardsList");
      output.RenderBeginTag(HtmlTextWriterTag.Div);
      try
      {
        TrackingField field;
        IEnumerable<ContentProfile> enumerable = ProfileUtil.GetProfiles(this.CurrentItem, out field).Reverse<ContentProfile>();
        if (field == null)
        {
          this.RenderThereAreNoProfileCardsMessage(output);
          return;
        }
        bool canOpen = WebEditUtility.CanOpenProfileCard(ContextUtil.ResolveItem());
        bool flag2 = false;
        foreach (ContentProfile profile in enumerable)
        {
          if (profile != null)
          {
            Item profileItem = profile.GetProfileItem();
            if (profileItem != null)
            {
              if ((profile.Presets == null) || (profile.Presets.Count == 0))
              {
                if (ProfileUtil.HasPresetData(profileItem, field))
                {
                  RenderProfileItem(output, profileItem, null, canOpen);
                  flag2 = true;
                }
              }
              else
              {
                foreach (KeyValuePair<string, double> pair in profile.Presets)
                {
                  Item profileCardItem = profile.GetProfileCardItem(pair.Key);
                  if (profileCardItem != null)
                  {
                    RenderProfileItem(output, profileItem, profileCardItem, canOpen);
                    flag2 = true;
                  }
                }
              }
            }
          }
        }
        if (!flag2)
        {
          this.RenderThereAreNoProfileCardsMessage(output);
        }
      }
      catch (Exception exception)
      {
        Logger.Error(exception.Message, exception);
      }
      output.RenderEndTag();
    }

    private static void RenderProfileItem(HtmlTextWriter output, Item profileItem, Item presetItem, bool canOpen)
    {
      string str = (presetItem == null) ? ProfileUtil.UI.GetProfileThumbnail(profileItem) : ProfileUtil.UI.GetPresetThumbnail(presetItem);
      string str2 = (presetItem == null) ? ProfileUtil.UI.GetProfileShortTooltip(profileItem) : ProfileUtil.UI.GetPresetShortTooltip(profileItem, presetItem);
      output.AddAttribute(HtmlTextWriterAttribute.Class, "ProfileCardContainer");
      output.RenderBeginTag(HtmlTextWriterTag.Div);
      output.AddAttribute(HtmlTextWriterAttribute.Title, str2);
      output.AddAttribute(HtmlTextWriterAttribute.Href, canOpen ? GetOpenProfileCardInlineCode(profileItem) : "#");
      output.RenderBeginTag(HtmlTextWriterTag.A);
      output.AddAttribute(HtmlTextWriterAttribute.Alt, str2);
      output.AddAttribute(HtmlTextWriterAttribute.Src, str);
      if (!AnalyticsUtility.IsTrackingEnabledEnabled() || !AnalyticsUtility.IsMarketingAuthor())
      {
        output.AddAttribute(HtmlTextWriterAttribute.Class, "disabled");
      }
      output.RenderBeginTag(HtmlTextWriterTag.Img);
      output.RenderEndTag();
      output.RenderEndTag();
      output.RenderEndTag();
    }

    private void RenderThereAreNoProfileCardsMessage(HtmlTextWriter output)
    {
      output.AddAttribute(HtmlTextWriterAttribute.Style, "font-size: 11px;");
      output.RenderBeginTag(HtmlTextWriterTag.Span);
      output.Write(Translate.Text("There are no profile cards associated with this page."));
      output.RenderEndTag();
      if ((!this.IsInIframeRendering && AnalyticsUtility.IsMarketingAuthor()) && AnalyticsUtility.IsTrackingEnabledEnabled())
      {
        output.Write("<br />");
        output.AddAttribute(HtmlTextWriterAttribute.Style, "text-decoration: none;");
        output.AddAttribute(HtmlTextWriterAttribute.Href, GetOpenEditProfileCardsScript());
        output.AddAttribute(HtmlTextWriterAttribute.Id, "assignProfileCardsLink");
        output.RenderBeginTag(HtmlTextWriterTag.A);
        output.Write(Translate.Text("Associate profile cards"));
        output.RenderEndTag();
      }
    }

    [Obsolete("Use Sitecore.ExperienceEditor.Utils.WebEditUtility.CanOpenProfileCard(Item) instead.")]
    protected bool CanOpenPrifileCard
    {
      get
      {
        Item currentItem = this.CurrentItem;
        return (((!currentItem.Appearance.ReadOnly && currentItem.Access.CanWrite()) && (!currentItem.IsFallback && AnalyticsUtility.IsMarketingAuthor())) && AnalyticsUtility.IsTrackingEnabledEnabled());
      }
    }

    [SuppressMessage("Microsoft.Performance", "CA1822:Mark members as static", Justification = "This will introduce breaking changes.")]
    protected Item CurrentItem =>
        ContextUtil.ResolveItem();

    protected bool IsInIframeRendering { get; private set; }
  }
}
