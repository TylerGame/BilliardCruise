using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SwitchLanguageEvent : IEvent
{
    public string language;
    public SwitchLanguageEvent() { }
    public SwitchLanguageEvent(string language)
    {
        this.language = language;
    }
}
