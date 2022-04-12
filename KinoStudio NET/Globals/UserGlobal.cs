using KinoStudio_NET.View;
using System.Windows;

namespace KinoStudio_NET.Globals
{
    public static class UserGlobal
    {
        public static string Login { get; set; }

        public static int RoleId { get; set; }

        public static (bool Actors, bool Employees, bool Organisations, bool Users, bool PostersImages) GetPermissions()
        {
            switch (RoleId)
            {
                case 1:
                    return (true, true, true, true, true);
                case 2:
                    return (true, false, false, false, false);
                case 3:
                    return (false, true, false, false, false);
                case 4:
                    return (false, false, false, true, false);
                case 5:
                    return (true, true, true, false, false);
                default:
                    return (false, false, false, false, false);
            }
        }

        public static void RunStartWindow()
        {
            switch (RoleId)
            {
                case 1:
                    RunWindow(new UsersView());
                    break;
                case 2:
                    RunWindow(new ActorsView());
                    break;
                case 3:
                    RunWindow(new EmployeesView());
                    break;
                case 4:
                    RunWindow(new UsersView());
                    break;
                case 5:
                    RunWindow(new OrganisationsView());
                    break;
                default:
                    return;
            }
        }

        private static void RunWindow<TWindow>(this TWindow window) where TWindow : Window, new()
        {
            window.Show();
        }
    }
}