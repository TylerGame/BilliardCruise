using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



public abstract class GameModuleProfiled<DataType> : IModule where DataType : ModuleData, new()
{
    static DataType dataLink;

    protected void ResetDataLink()
    {
        dataLink = null;
    }

    public virtual void Reset()
    {
        ResetDataLink();
    }

    public abstract IObservable<bool> Initialize();
    public abstract IEnumerable<Type> GetDependencies();

    public static DataType Data
    {
        get
        {
            if(dataLink == null)
            {

                dataLink = LocalProfile.Data.GetDataFor<DataType>();
            }
            return dataLink;
        }
    }
}
