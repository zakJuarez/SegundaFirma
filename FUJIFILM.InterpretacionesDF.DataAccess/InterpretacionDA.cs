using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FUJIFILM.InterpretacionesDF.DataAccess
{
    public class InterpretacionDA
    {
        const string saltoLinea = "\n";
        public List<vBuscaEnRevision> getInterpretacionList()
        {
            List<vBuscaEnRevision> _lisReturn = new List<vBuscaEnRevision>();
            try
            {
                try
                {
                    Helper.ConnectionString();
                }
                catch(Exception eCE)
                {
                    escribirBitacora("Error al obtener la cadena de conexión: " + eCE.Message);
                }
                using (dbRiskDataContext dc = new dbRiskDataContext(Helper.ConnectionString()))
                {
                    var query = from item in dc.vBuscaEnRevision
                                select new vBuscaEnRevision()
                                {
                                    AccessionNumber = item.AccessionNumber,
                                    ExternalPatientID = item.ExternalPatientID,
                                    InternalPatientID = item.InternalPatientID,
                                    FullName = item.FullName,
                                    FirstName = item.FirstName,
                                    LastName = item.LastName,
                                    DOB = item.DOB,
                                    Gender = item.Gender,
                                    Pregnancy = item.Pregnancy,
                                    LastMenstruationDate = item.LastMenstruationDate,
                                    StudyId = item.StudyId,
                                    RISOderId = item.RISOderId,
                                    ProcedureCode = item.ProcedureCode,
                                    ProcedureDescription = item.ProcedureDescription,
                                    Modality = item.Modality,
                                    ReasonForExam = item.ReasonForExam,
                                    StudyStatusCode = item.StudyStatusCode,
                                    StudyImageCount = item.StudyImageCount,
                                    StudyTimeStamp = item.StudyTimeStamp,
                                    AdmissionTimeDate = item.AdmissionTimeDate,
                                    VisitClass = item.VisitClass,
                                    VisitNumber = item.VisitNumber,
                                    Priority = item.Priority,
                                    SiteCode = item.SiteCode,
                                    PrimaryLocId = item.PrimaryLocId,
                                    PrimaryLocation = item.PrimaryLocation,
                                    CurrentLocId = item.CurrentLocId,
                                    CurrentLocation = item.CurrentLocation,
                                    StudySiteName = item.StudySiteName,
                                    RequestingPhysicianId = item.RequestingPhysicianId,
                                    RequestingPhysician = item.RequestingPhysician,
                                    ReferringPhysicianId = item.ReferringPhysicianId,
                                    ReferringPhysician = item.ReferringPhysician,
                                    AttendingPhysicianId = item.AttendingPhysicianId,
                                    AttendingPhysician = item.AttendingPhysician,
                                    usuarioDicto = item.usuarioDicto,
                                    STATUS = item.STATUS,
                                    oeInterpretacionAprobada = item.oeInterpretacionAprobada,
                                    bkStatus    = item.bkStatus,
                                    fechayhora = item.fechayhora,
                                    SolicitaRevision = item.SolicitaRevision == null ? 0 : item.SolicitaRevision
                                };
                    _lisReturn.AddRange(query);
                }
            }
            catch (Exception ev)
            {
                escribirBitacora("Error al consultar la vista en base de datos: " + ev.Message);
                _lisReturn = null;
            }
            return _lisReturn;
        }

        public bool getValidacionProcedeBloqueo(string NumEstudio, string userID)
        {
            bool validacion = false;
            try
            {
                using (dbRiskDataContext dc = new dbRiskDataContext(Helper.ConnectionString()))
                {
                    var query = dc.spProcedeBloqueo(NumEstudio, userID).First();
                    validacion = Convert.ToBoolean(query.Column1);
                }
            }
            catch(Exception egv)
            {
                escribirBitacora("Error al consultar la validacion: " + egv.Message);
            }
            return validacion;
        }

        public opeUsuario getUsuario(String user)
        {
            opeUsuario _mldUser = new opeUsuario();
            try
            {
                using(dbRiskDataContext dc = new dbRiskDataContext(Helper.ConnectionString()))
                {
                    var query = (from item in dc.opeUsuario
                                 where item.ouID.ToUpper() == user.Trim().ToUpper()
                                 select item);
                    if(query != null)
                    {
                        if(query.Count() > 0)
                        {
                            _mldUser = query.First();
                        }
                    }
                }
            }
            catch(Exception egU)
            {
                escribirBitacora("Error al consultar los datos del Usuario: " + egU.Message);
                _mldUser = null;
            }
            return _mldUser;
        }

        private void escribirBitacora(string msg)
        {
            if (!Directory.Exists("C:\\temp\\"))
                Directory.CreateDirectory("C:\\temp\\");
            File.AppendAllText("C:\\temp\\BitacoraSegundaFirma.txt", DateTime.Now.ToShortDateString() + ' ' + DateTime.Now.ToShortTimeString() + DateTime.Now.Second.ToString() + " " + msg + saltoLinea);
        }
    }
}
