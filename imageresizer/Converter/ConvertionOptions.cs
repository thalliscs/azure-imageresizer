using System;
using System.IO;
using System.Text;
using ImageMagick;

namespace imageresizer.Converter
{
    public class ConvertionOptions
    {
        public const int MaxSize = 3000;
        public string Name { get; set; }
        public MagickFormat TargetFormat { get; set; }
        public int Width { get; set; } = MaxSize;
        public int Height { get; set; } = MaxSize;
        public int Quality { get; set; }

        public string TargetMimeType => GetTargetMimeType();
        public string GetCacheKey()
        {
            if(string.IsNullOrWhiteSpace(Name))
                return string.Empty;

            var fileExtension = Path.GetExtension(Name);
            var fileName = string.IsNullOrWhiteSpace(fileExtension) == false ?
                Name.Substring(0, Name.Length - fileExtension.Length)
                : Name;

            var widthKey = Width == MaxSize ? 0 : Width;
            var heightKey = Height == MaxSize ?  0 : Height;

            var builder = new StringBuilder();
            builder.Append($"{fileName}.");
            builder.Append($"w_{Width}");
            builder.Append($",h_{Height}");
            builder.Append($",q_{Quality}");
            builder.Append(GetExtension());

            return builder.ToString();
        }

        private string GetExtension()
        {
            switch(TargetFormat)
            {
                case MagickFormat.Png:
                    return ".png";
                default:
                    return ".jpeg";
            }
        }

        private string GetTargetMimeType()
        {
            switch(TargetFormat)
            {
                case MagickFormat.Png:
                    return "image/png";
                default:
                    return "image/jpeg";
            }
        }
    }
}