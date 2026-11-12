namespace ConsoleAppFindSameNames;

public class Cover
{
	public Cover(string path, int height, int width)
	{
		Path = path;
		Name = System.IO.Path.GetFileNameWithoutExtension(path);
		Ext = System.IO.Path.GetExtension(path);
		Height = height;
		Width = width;
	}

	/// <summary>
	/// File Name Without Extension
	/// </summary>
	public string Name { get; private set; }
	/// <summary>
	/// Extension
	/// </summary>
	public string Ext { get; private set; }
	/// <summary>
	/// Full Path
	/// </summary>
	public string Path { get; private set; }
	public int Height { get; private set; }
	public int Width { get; private set; }
	/// <summary>
	/// File Name With Extension
	/// </summary>
	public string NameExt => Name + Ext;
}
