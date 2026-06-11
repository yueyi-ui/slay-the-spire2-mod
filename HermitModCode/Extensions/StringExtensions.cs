using System;
using System.IO;

namespace HermitMod.Extensions
{
    public static class StringExtensions
    {
        public static string ImagePath(this string path)
        {
            return Path.Join("HermitMod", "images", path);
        }

        public static string CardImagePath(this string path)
        {
            return Path.Join("HermitMod", "images", "card_portraits", path);
        }

        public static string BigCardImagePath(this string path)
        {
            return Path.Join("HermitMod", "images", "card_portraits", "big", path);
        }

        public static string PowerImagePath(this string path)
        {
            return Path.Join("HermitMod", "images", "powers", path);
        }

        public static string BigPowerImagePath(this string path)
        {
            return Path.Join("HermitMod", "images", "powers", "big", path);
        }

        public static string RelicImagePath(this string path)
        {
            return Path.Join("HermitMod", "images", "relics", path);
        }

        public static string BigRelicImagePath(this string path)
        {
            return Path.Join("HermitMod", "images", "relics", "big", path);
        }

        public static string CharacterUiPath(this string path)
        {
            return Path.Join("HermitMod", "images", "charui", path);
        }
    }
}