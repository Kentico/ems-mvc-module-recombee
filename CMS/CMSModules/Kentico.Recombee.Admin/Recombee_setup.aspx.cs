using System;
using CMS.Base;
using CMS.Core;
using CMS.UIControls;
using Kentico.Recombee.DatabaseSetup;

public partial class CMSModules_Recombee_setup : GlobalAdminPage
{
    private readonly ISettingsService settingsService;
    private readonly ISiteService siteService;

    public CMSModules_Recombee_setup()
    {
        settingsService = Service.Resolve<ISettingsService>();
        siteService = Service.Resolve<ISiteService>();
    }

    public void Page_Load()
    {
        if(!IsOnlineMarketingEnabled())
        {
            btnIntDbStructure.Enabled = false;
            btnResetDatabase.Enabled = false;
            btnInitDatabase.Enabled = false;
        }

        if(!siteService.CurrentSite?.SiteName.Contains("DancingGoat") ?? false)
        {
            divHistory.Visible = false;
        }
    }

    protected void InitHistory_Click(object sender, EventArgs e)
    {
        try
        {
            var setup = new HistoryData();
            setup.CreateHistoryData();
        }
        catch (Exception ex)
        {
            ShowError(string.Format(ex.Message));
        }  
    }


    protected void ResetDatabase_Click(object sender, EventArgs e)
    {
        try
        {
            var reset = new RecombeeReset();
            reset.ResetDatabase();
        }
        catch (Exception ex)
        {
            ShowError(string.Format(ex.Message));
        }
    }

    protected void InitDatabaseStructure_Click(object sender, EventArgs e)
    {
        try
        {
            var setup = new RecombeeStructure();
            setup.SetupDatabaseStructure();
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }

    private bool IsOnlineMarketingEnabled()
    {
        return settingsService[siteService.CurrentSite?.SiteName + ".CMSEnableOnlineMarketing"].ToBoolean(false);
    }
}