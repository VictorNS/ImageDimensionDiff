# ImageDimensionDiff

ImageDimensionDiff is a command-line tool that compares PNG and JPEG images in two directory trees. It matches images by filename and reports pairs whose pixel dimensions differ.

The tool reads image dimensions from the image metadata. It does not compare image contents, EXIF information, or file sizes.

## Requirements

- .NET 10 SDK

## Usage

```text
dotnet run --project ImageDimensionDiff.csproj -- <sample-directory> <source-directory> [destination-directory]
```

The first directory contains the images to check. The second directory is searched for matching images. If a destination directory is provided, source images from matching pairs with different dimensions are copied there. The destination directory must already exist; an existing file with the same name is overwritten.

## Matching behavior

- Both directories are scanned recursively.
- Only `.png`, `.jpg`, and `.jpeg` files are processed.
- Files are matched by filename without the extension.
- If a sample filename ends in `_cut` or `_crop`, the tool also tries to match it after removing that suffix.
- A match is reported only when its height or width differs from the sample image.
- Files that cannot be read are reported and skipped. Other file types are ignored.

Without a destination directory, the tool only prints the differing pairs. With a destination directory, it also copies the matching source images whose dimensions differ.
