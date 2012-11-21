using System;
using System.Collections.Generic;
using System.Text;
using VODB.VirtualDataBase;
using System.Linq;
using VODB.Annotations;

namespace VODB.Sys
{
    [DbTable("sys.objects")]
    public sealed class Objects : DbEntity
    {

        public int Object_id { get; set; }

        [DbKey]
        public String Name { get; set; }

        public IEnumerable<All_Columns> Columns
        {
            get { return GetValues<All_Columns>().Where("Object_id = {0}", Object_id); }
        }

    }

    [DbTable("sys.all_columns")]
    public sealed class All_Columns : DbEntity
    {

        [ DbKey]
        public int Object_id { get; set; }

        [ DbKey]
        public int Column_id { get; set; }

        public String Name { get; set; }

        int max_length;

        public int Max_Length
        {
            get { return System_type_id == 231 ? max_length / 2 : max_length; }
            set { max_length = value; }
        }

        public int System_type_id { get; set; }

        public Boolean Is_Ansi_Padded { get; set; }

    }
}
