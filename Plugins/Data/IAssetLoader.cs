using System.Collections.Generic;

namespace TofuPlugin.Data
{
    public interface IAssetLoader<T>
    {
        List<T> Load(string folderPath);
    }
}
