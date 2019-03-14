using System;
using ImageMagick;
using imageresizer.Models;

namespace imageresizer.Converter
{
    public static class ConversionOptionsFactory
    {
        public static ConvertionOptions FromResizeRequest(ResizeRequestModel requestModel)
        {
            var options = new ConvertionOptions
            {
                Name = requestModel.Name
            };

            if(requestModel.Width.HasValue && requestModel.Width > 0)
                options.Width = Math.Min(requestModel.Width.Value, ConvertionOptions.MaxSize);

            if(requestModel.Height.HasValue && requestModel.Height > 0)
                options.Height = Math.Min(requestModel.Height.Value, ConvertionOptions.MaxSize);

            options.TargetFormat = GetMagickFormat(requestModel.Format);

            if(requestModel.Quality.HasValue && requestModel.Quality.Value >= 1 && requestModel.Quality.Value <= 100)
                options.Quality = requestModel.Quality.Value;
            else
                options.Quality = options.TargetFormat == MagickFormat.Png24 ? 100 : 82;
            
            return options;
        }

        private static MagickFormat GetMagickFormat(string format)
        {
            if(string.IsNullOrWhiteSpace(format) == false)
            {
                switch(format.ToLower()) 
                {
                    case "png":
                        return MagickFormat.Png;
                    case "jpeg":
                    case "jpg":
                        return MagickFormat.Jpeg;
                }
            }
            return MagickFormat.Jpeg;
        }
    }
}