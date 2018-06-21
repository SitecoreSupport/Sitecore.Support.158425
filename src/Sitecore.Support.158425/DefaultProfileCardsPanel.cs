namespace Sitecore.Support.ContentTesting.Speak.Ribbon.Panels
{
  using Sitecore.Collections;
  using Sitecore.ExperienceEditor.Speak.Ribbon;
  using Sitecore.Shell.Applications.WebEdit.Panels;
  using Sitecore.Shell.Web.UI.WebControls;
  using System;

  public class DefaultProfileCardsPanel : CustomRibbonPanel
  {
    private static readonly SafeDictionary<Type, object> ProfileCardsPanels;

    static DefaultProfileCardsPanel()
    {
      ProfileCardsPanels = new SafeDictionary<Type, object>();
      //The fix: use ProfileCardsPanel from the patch
      ProfileCardsPanels.Add(typeof(RibbonComponentControlBase), new Sitecore.Support.ContentTesting.Speak.Ribbon.Panels.ProfileCardsPanel.ProfileCardsPanel());
      ProfileCardsPanels.Add(typeof(RibbonPanel), new ProfileCards());
    }

    protected override SafeDictionary<Type, object> Panels =>
        ProfileCardsPanels;
  }
}
