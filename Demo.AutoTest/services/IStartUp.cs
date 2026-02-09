using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.AutoTest.services
{
    public interface IStartUp
    {
        void Initilize();
    }

    public class StartUp : IStartUp
    {
        public void Initilize()
        {
            throw new NotImplementedException();
        }
    }
}
