using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CargoTrace
{
    public class Handler
    {
        public static bool IsEqual(User user1, User user2)
        {
            if (user1 == null || user2 == null)
            {
                return false;
            }
            else if (user1.UserLoginAuthID != user2.UserLoginAuthID)
            {
                return false;
            }
            else if (user1.Password != user2.Password)
            {
                return false;
            }
            return true;
        }

        public static void ShowMessage()
        {
            System.Windows.MessageBox.Show("Username and Password do not match each other, please check your personal information and try again", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }
}
