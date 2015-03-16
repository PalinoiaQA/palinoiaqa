using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    /// <summary>
    /// class to hold code for viewDefect 
    /// </summary>
    public class viewDefect
    {
        /// <summary>
        /// class variable for defect ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// class variable for defect priority 
        /// </summary>
        public int DefectPriorityID { get; set; }
        /// <summary>
        /// class variable for defect status
        /// </summary>
        public int DefectStatusID { get; set; }
        /// <summary>
        /// class variable for defect
        /// </summary>
        public int DefectTypeID { get; set; }
        /// <summary>
        /// class variable for defect owner ID
        /// </summary>
        public int OwnerID { get; set; }
        /// <summary>
        /// class variable for defect name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// class variable for defect description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// class variable for date that defect was created
        /// </summary>
        public string DateCreated { get; set; }
        /// <summary>
        /// class variable for date that defect was completed
        /// </summary>
        public string DateCompleted { get; set; }
        /// <summary>
        /// class variable to indicate whether defect is open or closed
        /// </summary>
        public bool Closed { get; set; }

        /// <summary>
        /// empty constructor for viewDefect object
        /// </summary>
        public viewDefect()
        {

        }

        /// <summary>
        /// constructor for viewDefect object with parameters
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="defectPriorityID">int</param>
        /// <param name="defectStatusID">int</param>
        /// <param name="defectTypeID">int</param>
        /// <param name="ownerID">int</param>
        /// <param name="name">string</param>
        /// <param name="description">string</param>
        /// <param name="dateCreated">DateTime</param>
        /// <param name="dateCompleted">DateTime</param>
        /// <param name="closed">bool</param>
        public viewDefect(int id,
                          int defectPriorityID,
                          int defectStatusID,
                          int defectTypeID,
                          int ownerID,
                          string name,
                          string description,
                          string dateCreated,
                          string dateCompleted,
                          bool closed)
        {
            this.ID = id;
            this.DefectPriorityID = defectPriorityID;
            this.DefectStatusID = defectStatusID;
            this.DefectTypeID = defectTypeID;
            this.OwnerID = ownerID;
            this.Name = name;
            this.Description = description;
            this.DateCreated = dateCreated;
            this.DateCompleted = dateCompleted;
            this.Closed = closed;
        }

        /// <summary>
        /// constructor for viewDefect object with all parameters except
        /// for dateCompleted
        /// </summary>
        /// <param name="id">int</param>
        /// <param name="defectPriorityID">int</param>
        /// <param name="defectStatusID">int</param>
        /// <param name="defectTypeID">int</param>
        /// <param name="ownerID">int</param>
        /// <param name="name">string</param>
        /// <param name="description">string</param>
        /// <param name="dateCreated">DateTime</param>
        /// <param name="closed">bool</param>
        public viewDefect(int id,
                          int defectPriorityID,
                          int defectStatusID,
                          int defectTypeID,
                          int ownerID,
                          string name,
                          string description,
                          string dateCreated,
                          bool closed)
        {
            this.ID = id;
            this.DefectPriorityID = defectPriorityID;
            this.DefectStatusID = defectStatusID;
            this.DefectTypeID = defectTypeID;
            this.OwnerID = ownerID;
            this.Name = name;
            this.Description = description;
            this.DateCreated = dateCreated;
            this.Closed = closed;
        }
    }
}
