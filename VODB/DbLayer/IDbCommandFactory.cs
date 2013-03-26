using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VODB.DbLayer
{
    /// <summary>
    /// A Factory of IVodbCommand.
    /// </summary>
    public interface IDbCommandFactory
    {

        /// <summary>
        /// Makes the command.
        /// </summary>
        /// <returns></returns>
        IVodbCommand MakeCommand();

    }
}
