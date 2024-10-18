namespace NEASL.Base.Package;

public static class PackageManager
{
    public static bool CreatePackage(string packageName, string packageRootFolder, string outputName, string outputFolder)
    {
        return false;
    }

    public static string[] SearchPackageFiles(string packageRootFolder)
    {
        string folderPath = @packageRootFolder; // Specify your folder path
        string fileExtension = "*.neasl"; // Specify the file extension (e.g., "*.txt" for text files)
        
        string[] files = Directory.GetFiles(folderPath, fileExtension, SearchOption.AllDirectories);
        return files;
    }
    
    //public Type FindTypeByIdentifierName(string identifierName, )

}