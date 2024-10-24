using Microsoft.AspNetCore.Mvc.Rendering;

namespace StudentLibraryManagement.Utility
{
    public static class Helper
    {
        public static string Admin = "Admin";
        public static string Student = "Student";
        public static int success_code = 1;
        public static int failure_code = 0;

        public static List<SelectListItem> GetRolesForDropDown()
        {
            
                return new List<SelectListItem>
                {
                    new SelectListItem{Value=Helper.Admin,Text=Helper.Admin},
                    new SelectListItem{Value=Helper.Student,Text=Helper.Student}
                };
        }

    }
}
