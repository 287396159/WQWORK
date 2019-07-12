using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PersionAutoLocaSys.Bean
{
    class ImportAdmissExit
    {
        private String admissTime = "-";
        private String exitTime = "-";
        private String tagID = "-";
        private String name = "-";
        private String workID = "-";

        public void setAdmissData(AdmissionExit admissExit) 
        {
            if (!AdmissionExit.ADMISSION.Equals(admissExit.Model)) return;
            admissTime = admissExit.AeTime;
            tagID = admissExit.TagID;
            name = admissExit.Name;
            workID = admissExit.WorkIDStr;
        }

        public String AdmissTime
        {
            get { return admissTime; }
            set { admissTime = value; }
        }

        public String ExitTime
        {
            get { return exitTime; }
            set { exitTime = value; }
        }

        public String TagID
        {
            get { return tagID; }
            set { tagID = value; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public String WorkID
        {
            get { return workID; }
            set { workID = value; }
        }

    }
}
