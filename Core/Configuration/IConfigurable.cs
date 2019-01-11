using System.Collections.Generic;

/*
 * IConfigurable objects hold a properties object with string, int/float/string parameters
 */
namespace TofuCore.Configuration
{
    public interface IConfigurable
    {

        Properties Properties {
            get;
        }
    }
}
