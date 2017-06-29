using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUJIFILM.InterpretacionesDF.Entidades
{
    public class clsInterpretacion
    {
        public string AccessionNumber { get; set; }
        public string ExternalPatientID { get; set; }
        public string InternalPatientID { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string Pregnancy { get; set; }
        public string LastMenstruationDate { get; set; }
        public string StudyId { get; set; }
        public string RISOderId { get; set; }
        public string ProcedureCode { get; set; }
        public string ProcedureDescription { get; set; }
        public string Modality { get; set; }
        public string ReasonForExam { get; set; }
        public string StudyStatusCode { get; set; }
        public string StudyImageCount { get; set; }
        public string StudyTimeStamp { get; set; }
        public string AdmissionTimeDate { get; set; }
        public string VisitClass { get; set; }
        public string VisitNumber { get; set; }
        public string Priority { get; set; }
        public string SiteCode { get; set; }
        public string PrimaryLocId { get; set; }
        public string PrimaryLocation { get; set; }
        public string CurrentLocId { get; set; }
        public string CurrentLocation { get; set; }
        public string StudySiteName { get; set; }
        public string RequestingPhysicianId { get; set; }
        public string RequestingPhysician { get; set; }
        public string ReferringPhysicianId { get; set; }
        public string ReferringPhysician { get; set; }
        public string AttendingPhysicianId { get; set; }
        public string AttendingPhysician { get; set; }
        public string usuarioDicto { get; set; }
        public string STATUS { get; set; }
        public string oeInterpretacionAprobada { get; set; }

        public clsInterpretacion()
        {
            AccessionNumber = string.Empty;
            ExternalPatientID = string.Empty;
            InternalPatientID = string.Empty;
            FullName = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            DOB = string.Empty;
            Gender = string.Empty;
            Pregnancy = string.Empty;
            LastMenstruationDate = string.Empty;
            StudyId = string.Empty;
            RISOderId = string.Empty;
            ProcedureCode = string.Empty;
            ProcedureDescription = string.Empty;
            Modality = string.Empty;
            ReasonForExam = string.Empty;
            StudyStatusCode = string.Empty;
            StudyImageCount = string.Empty;
            StudyTimeStamp = string.Empty;
            AdmissionTimeDate = string.Empty;
            VisitClass = string.Empty;
            VisitNumber = string.Empty;
            Priority = string.Empty;
            SiteCode = string.Empty;
            PrimaryLocId = string.Empty;
            PrimaryLocation = string.Empty;
            CurrentLocId = string.Empty;
            CurrentLocation = string.Empty;
            StudySiteName = string.Empty;
            RequestingPhysicianId = string.Empty;
            RequestingPhysician = string.Empty;
            ReferringPhysicianId = string.Empty;
            ReferringPhysician = string.Empty;
            AttendingPhysicianId = string.Empty;
            AttendingPhysician = string.Empty;
            usuarioDicto = string.Empty;
            STATUS = string.Empty;
            oeInterpretacionAprobada = string.Empty;
        }



    }
}
