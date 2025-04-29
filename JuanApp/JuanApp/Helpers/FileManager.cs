namespace JuanApp.Helpers;
    public static class FileManager
    {
    public static string SaveImage(this IFormFile file, string path, string folder)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Invalid file");

        string directoryPath = Path.Combine(path, folder);
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        string fullPath = Path.Combine(directoryPath, fileName);

        using FileStream fileStream = new FileStream(fullPath, FileMode.Create);
        file.CopyTo(fileStream);

        return fileName;
    }

    public static bool CheckSize(this IFormFile file, int maxSize)
        {
            return file.Length <= maxSize;
        }
        public static bool CheckType(this IFormFile file, string[] types)
        {
            return types.Contains(file.ContentType);
        }

    }
