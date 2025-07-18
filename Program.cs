public class VersionUpAgrs
{
    public string Path { get; set; }
    public string NewVersion { get; set; }
    public string OldVersion { get; set; }
    public string[] Extensions { get; set; } = [".csproj", ".props"];
}
public class Program
{
    public static void Main(string[] args)
    {
        if(!TryParse(args, out var versionUpArgs))
        {
            return;
        }

        var files = Directory.EnumerateFiles(versionUpArgs.Path, "*.*", SearchOption.AllDirectories);
        var extensions = new HashSet<string>(versionUpArgs.Extensions, StringComparer.OrdinalIgnoreCase);

        foreach (var file in files)
        {
            var ext = Path.GetExtension(file);

            if (!extensions.Contains(ext))
                continue;

            var text = File.ReadAllText(file);
            if (text.Contains(versionUpArgs.OldVersion))
            {
                Console.WriteLine($"Replacing version in {file}");
                var newText = text.Replace(versionUpArgs.OldVersion, versionUpArgs.NewVersion);
                File.WriteAllText(file, newText);
            }
        }
    }

    private static bool TryParse(string[] args, out VersionUpAgrs res)
    {
        int ind = 0;
        res = new VersionUpAgrs();
        res.Path = Directory.GetCurrentDirectory();
        while (ind < args.Length)
        {
            if (args[ind] == "-p" && ind < args.Length - 1)
            {
                ind++;
                res.Path = args[ind];
                ind++;
            } 
            else if (args[ind] == "-e" && ind < args.Length - 1)
            {
                ind++;
                res.Extensions = args[ind].Split(',').Select(x => "." + x).ToArray();
                ind++;
            }
            else if (args[ind] == "-?") 
            {
                PrintUsage(Console.Out);
                return false;
            } 
            else 
            {
                if (args.Length - ind != 2)
                {
                    var tw = Console.Error;
                    tw.WriteLine("Invalid argument list");
                    tw.WriteLine();
                    PrintUsage(tw);
                    return false;
                }
                res.OldVersion = args[ind++];
                res.NewVersion = args[ind];
                return true;
            }
        }
        if (string.IsNullOrEmpty(res.OldVersion) || 
            string.IsNullOrEmpty(res.NewVersion))
        {
            var tw = Console.Error;
            tw.WriteLine("Invalid argument list");
            tw.WriteLine();
            PrintUsage(tw);
            return false;
        }
        return true;
    }

    private static void PrintUsage(TextWriter tw)
    {
        tw.WriteLine("versionup [args] <oldVersion> <newVersion>");
        tw.WriteLine("args list:");
        tw.WriteLine("    -p <folder1>:      path");
        tw.WriteLine("    -e <ext1,ext2...>: extensions");
        tw.WriteLine("    -?:                show this message");
    }
}
