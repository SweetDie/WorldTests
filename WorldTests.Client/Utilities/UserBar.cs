using MaterialDesignThemes.Wpf;
using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WorldTests.Primitive.Models;

namespace WorldTests.Client.Utilities
{
    public static class UserBar
    {
        public static Chip GetUserChip(UserModel userModel)
        {
            byte[] bytes;

            if (userModel.Image == null)
            {
                bytes = Convert.FromBase64String(GetDefaultIconBase64());
            }
            else
            {
                bytes = Convert.FromBase64String(userModel.Image);
            }

            var bitmapImage = new BitmapImage();

            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(bytes);
            bitmapImage.EndInit();

            var image = new Image { Source = bitmapImage };

            var chip = new Chip
            {
                Content = $"{userModel.Firstname} {userModel.Lastname}",
                IsDeletable = true,
                DeleteToolTip = "Log out",
                Icon = image
            };

            return chip;
        }

        public static string GetDefaultIconBase64()
        {
            return File.ReadAllText("../../../Images/userIconDefault.txt");
        }
    }
}
