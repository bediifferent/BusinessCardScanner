using Plugin.Media.Abstractions;

namespace BusinessCardScanner.Services.Interfaces
{
    public interface IDeviceInfoService
    {
        byte[] GetFileStream(MediaFile file);
        string CreateCommonDatabase();
    }
}