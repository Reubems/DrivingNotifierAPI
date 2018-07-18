using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrivingNotifierAPI.Data
{
    /* IMPORTANT:
     
         For security reasons, this class should never be commited or pushed to any remote repository with public access.
         For doing so, it should be added a new line in the .gitignore file or the corresponding file related to that
         version control system.

         As the repository where the code is hosted is private, that configuration is not mandatory for now.

     */
    public class PrivateCredentials
    {
        public static readonly string PASS_DB_REMOTE = "dnpass";
    }
}
