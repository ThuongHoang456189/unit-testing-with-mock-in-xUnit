using DataAccess.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment4_BookManagement
{
    public class CustomizedApplication : IApplication
    {
        private IBookDataAccess _bookDataAccess;
        public CustomizedApplication(IBookDataAccess bookDataAccess)
        {
            this._bookDataAccess = bookDataAccess;
        }
        public void Run()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMaintainBooks(_bookDataAccess));
        }
    }
}
