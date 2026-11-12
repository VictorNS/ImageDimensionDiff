namespace ConsoleAppFindSameNames;

public static class FolderReader
{
	public static Dictionary<string, Cover> ScanFolder(string path)
	{
		Console.WriteLine($"{path}:");
		int origRow = Console.CursorTop;

		var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
		var covers = new Dictionary<string, Cover>();

		for (int i = 0; i < files.Length; i++)
		{
			Console.SetCursorPosition(0, origRow);
			Console.Write(i);

			var filePath = files[i];
			var info = ImageReader.GetInfo(filePath);

			if (info.IsIgnore)
				continue;

			if (!info.IsSuccess)
			{
				Console.WriteLine($"Could not read {filePath}");
				origRow = Console.CursorTop;
				continue;
			}

			var cover = new Cover(filePath, info.Height, info.Width);

			if (covers.TryGetValue(cover.Name, out var ex))
			{
				if (cover.Height > ex.Height || cover.Width > ex.Width)
					covers[cover.Name] = cover;
			}
			else
			{
				covers.Add(cover.Name, cover);
			}
		}

		Console.WriteLine();
		Console.WriteLine();
		return covers;
	}
}
