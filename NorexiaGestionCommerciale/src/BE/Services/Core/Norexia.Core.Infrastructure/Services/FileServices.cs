using Microsoft.Extensions.Configuration;

using Norexia.Core.Application.Common.Interfaces;

namespace Norexia.Core.Infrastructure.Services;

public class FileServices : IFileServices
{
    private readonly IConfiguration _configuration;

    public FileServices(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> StoreProductPictureAsync(Guid productId, string fileName, string data, CancellationToken cancellationToken = default)
    {
        var rootPath = _configuration.GetValue<string>("ProductsImagesRootFolder");
        var fileDirectory = Directory.CreateDirectory($@"{rootPath!}/{productId}");
        var path = Path.Combine(fileDirectory.FullName, fileName);
        using Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
        var imageDataByteArray = Convert.FromBase64String(data);
        await stream.WriteAsync(imageDataByteArray, 0, imageDataByteArray.Length, cancellationToken);

        return path;
    }

    public async Task<string> StoreAttachedDigitalInvoiceAsync(string invoiceRef, string fileName, string data, CancellationToken cancellationToken = default)
    {
        var rootPath = _configuration.GetValue<string>("AttachedDigitalInvoicesFolder");
        var fileDirectory = Directory.CreateDirectory($@"{rootPath!}/{invoiceRef}");
        var path = Path.Combine(fileDirectory.FullName, fileName);
        using Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
        var fileByteArray = Convert.FromBase64String(data);
        await stream.WriteAsync(fileByteArray, 0, fileByteArray.Length, cancellationToken);

        return path;
    }
}
