using System;
using System.IO;

namespace WeijiaMod.Extensions
{
    public static class StringExtensions
    {
        public static string ImagePath(this string path)
        {
            return Path.Join( "images", path);
        }

        public static string CardImagePath(this string path)
        {
            return Path.Join( "images", "card_portraits", path);
        }

        public static string BigCardImagePath(this string path)
        {
            return Path.Join( "images", "card_portraits", "big", path);
        }

        public static string PowerImagePath(this string path)
        {
            return Path.Join( "images", "powers", path);
        }

        public static string BigPowerImagePath(this string path)
        {
            return Path.Join( "images", "powers", "big", path);
        }

        public static string RelicImagePath(this string path)
        {
            return Path.Join( "images", "relics", path);
        }

        public static string BigRelicImagePath(this string path)
        {
            return Path.Join( "images", "relics", "big", path);
        }

        public static string CharacterUiPath(this string path)
        {
            return Path.Join( "images", "charui", path);
        }
    }
}




