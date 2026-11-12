using MetadataExtractor;
using MetadataExtractor.Formats.Jpeg;
using MetadataExtractor.Formats.Png;

namespace ConsoleAppFindSameNames;

public static class ImageReader
{
	public static Result GetInfo(string filePath)
	{
		var ext = Path.GetExtension(filePath);

		if (ext is null)
			return Result.Ignore();

		if (!ext.Equals(".PNG", StringComparison.InvariantCultureIgnoreCase)
			&& !ext.Equals(".JPG", StringComparison.InvariantCultureIgnoreCase)
			&& !ext.Equals(".JPEG", StringComparison.InvariantCultureIgnoreCase))
			return Result.Ignore();

		IReadOnlyList<MetadataExtractor.Directory> directories;

		try
		{
			directories = ImageMetadataReader.ReadMetadata(filePath);
		}
		catch
		{
			return Result.Fail();
		}

		if (ext.Equals(".PNG", StringComparison.InvariantCultureIgnoreCase))
		{
			var d = directories.OfType<PngDirectory>().FirstOrDefault();

			if (d is null)
				return Result.Fail();

			static int? GetInt(PngDirectory d, int tagType)
			{
				var desc = d.GetString(tagType);
				if (desc is null)
					return null;
				if (int.TryParse(desc, out var result))
					return result;
				return null;
			}

			return Result.Ok(GetInt(d, PngDirectory.TagImageHeight), GetInt(d, PngDirectory.TagImageWidth));
		}

		if (ext.Equals(".JPG", StringComparison.InvariantCultureIgnoreCase) || ext.Equals(".JPEG", StringComparison.InvariantCultureIgnoreCase))
		{
			var d = directories.OfType<JpegDirectory>().FirstOrDefault();

			if (d is null)
				return Result.Fail();

			static int? GetInt(JpegDirectory d, int tagType)
			{
				var desc = d.GetString(tagType);
				if (desc is null)
					return null;
				if (int.TryParse(desc, out var result))
					return result;
				return null;
			}

			return Result.Ok(GetInt(d, JpegDirectory.TagImageHeight), GetInt(d, JpegDirectory.TagImageWidth));
		}

		return Result.Fail();
	}
}

public record Result(int Height, int Width, bool IsSuccess, bool IsIgnore)
{
	public static Result Ok(int? height, int? width) => height is null || width is null ? new(0, 0, false, false) : new(height.Value, width.Value, true, false);
	public static Result Fail() => new(0, 0, false, false);
	public static Result Ignore() => new(0, 0, false, true);
}
