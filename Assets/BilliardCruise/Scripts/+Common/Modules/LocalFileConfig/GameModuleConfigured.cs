using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class GameModuleConfigured<DataType, ConfigType> : GameModuleProfiled<DataType> where DataType : ModuleData, new() where ConfigType : ModuleConfig, new()
{
    static ConfigType configLink;

    public override void Reset()
    {
        base.Reset();
        configLink = null;
    }

    public static ConfigType Config
    {
        get
        {
            if (configLink == null && LocalConfig.Data != null)
            {
                configLink = LocalConfig.Data.GetConfigFor<ConfigType>();
            }
            return configLink;
        }
    }

}
