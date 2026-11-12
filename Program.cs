using ConsoleAppFindSameNames;

if (args == null || args.Length < 2)
	return WriteError("Two paths as arguments are expected.");

var samplePath = args[0];
var sourcePath = args[1];
var destPath = "";

if (!Directory.Exists(samplePath))
	return WriteError($"{samplePath} doesn't exists");
if (!Directory.Exists(sourcePath))
	return WriteError($"{sourcePath} doesn't exists");

if (args.Length >2)
{
	destPath = args[2];

	if (!Directory.Exists(destPath))
		return WriteError($"{destPath} doesn't exists");
}

var isNeedCopy = !string.IsNullOrEmpty(destPath);
var sampleCovers = FolderReader.ScanFolder(samplePath);
var sourceCovers = FolderReader.ScanFolder(sourcePath);

foreach (var sample in sampleCovers.Values)
{
	var exists = sourceCovers.TryGetValue(sample.Name, out var source);

	if (!exists && sample.Name.ToLower().EndsWith("_cut"))
		exists = sourceCovers.TryGetValue(sample.Name[..^4], out source);
	else if (!exists && sample.Name.ToLower().EndsWith("_crop"))
		exists = sourceCovers.TryGetValue(sample.Name[..^5], out source);

	if (exists && source is not null)
	{
		if (sample.Height != source.Height || sample.Width != source.Width)
		{
			Console.WriteLine($"{sample.NameExt} => {source.Path} ({sample.Height} x {sample.Width} {source.Height} x {source.Width})");

			if (isNeedCopy)
				File.Copy(source.Path, Path.Combine(destPath, source.NameExt), true);
		}
	}
}

return 0;

static int WriteError(string message)
{
	Console.ForegroundColor = ConsoleColor.Red;
	Console.WriteLine(message);
	return -1;
}
