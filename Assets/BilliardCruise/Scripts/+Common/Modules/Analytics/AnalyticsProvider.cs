using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface AnalyticsProvider
{
    public void SendEvent(string eventName, Dictionary<string, string> parameters = null);


}