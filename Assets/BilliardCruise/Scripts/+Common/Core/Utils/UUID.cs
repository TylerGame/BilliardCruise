using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UUID
{
    public static string GenerateUUID()
    {
        return System.Guid.NewGuid().ToString();
    }
}
