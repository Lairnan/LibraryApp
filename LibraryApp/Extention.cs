using System.IO;

namespace LibraryApp;

public static class Extension
{
    public static byte[]? ToByte(this string? imgPath)
    {
        byte[]? image = null;
        if (string.IsNullOrWhiteSpace(imgPath)) return image;
        using var fs = new FileStream(imgPath, FileMode.Open);
        image = new byte[fs.Length];
        fs.Read(image, 0, image.Length);

        return image;
    }
}