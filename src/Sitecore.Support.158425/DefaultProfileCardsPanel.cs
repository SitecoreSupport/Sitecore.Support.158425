namespace Sitecore.ContentTesting.Speak.Ribbon.Panels
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
      new SafeDictionary<Type, object>().Add(typeof(RibbonComponentControlBase), new Sitecore.ContentTesting.Speak.Ribbon.Panels.ProfileCardsPanel.ProfileCardsPanel());
      new SafeDictionary<Type, object>().Add(typeof(RibbonPanel), new ProfileCards());
      ProfileCardsPanels = new SafeDictionary<Type, object>();
    }

    protected override SafeDictionary<Type, object> Panels =>
        ProfileCardsPanels;
  }
}
