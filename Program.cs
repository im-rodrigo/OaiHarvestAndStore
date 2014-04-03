using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using oaiharvester;
using oai;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core;



//http://doaj.org/oai.article?verb=ListRecords&from=2004-07-16&metadataPrefix=oai_dc
namespace oaiharvester
{
    class Program
    {
        static void Main(string[] args)
        {
            MegaTableSplit splitbyauthor = new MegaTableSplit();

            //harvestDOAJ harvester = new harvestDOAJ();
            //harvester.start();
            splitbyauthor.splitToAuthorSmall();
            splitbyauthor.splitToSubjectSmall();
        }

    }
}
